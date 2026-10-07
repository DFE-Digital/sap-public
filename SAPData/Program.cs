using CsvHelper;
using Microsoft.Extensions.Configuration;
using SAPData.Filters;
using SAPData.Models;
using System.Globalization;
using System.Text;

namespace SAPData;

internal class Program
{
    static void Main(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddUserSecrets<Program>()
            .Build();

        Console.WriteLine("Generating Raw Data Tables and Scripts...");

        // In CI the working directory is often the repo root.
        // Find SAPData.csproj anywhere under the current directory and use its folder.
        string baseDir = FindProjectDirectoryDownwards("SAPData.csproj");

        string dataMapDir = Path.Combine(baseDir, "DataMap");
        string rawInputDir = Path.Combine(dataMapDir, "SourceFiles");
        string cleanedDir = Path.Combine(dataMapDir, "CleanedFiles");
        string dataMapCsv = Path.Combine(dataMapDir, "datamap.csv");
        string sqlDir = Path.Combine(baseDir, "Sql");
        string tableMappingPath = Path.Combine(sqlDir, "tablemapping.csv");
        string rawTablesToRebuildPath = ResolveRawTablesToRebuildPath(baseDir, configuration);

        // -------------------------------------------------
        // 1. Load DataMap
        // -------------------------------------------------
        List<DataMapRow> dataMaps;
        using (var reader = new StreamReader(dataMapCsv))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            csv.Context.RegisterClassMap<DataMapMapping>();
            dataMaps = csv.GetRecords<DataMapRow>().ToList();
        }

        Console.WriteLine($"Loaded {dataMaps.Count} DataMap rows");

        var rebuildAllRawTables = ShouldRebuildAllRawTables(configuration);
        var logicalKeysToRebuild = rebuildAllRawTables
            ? new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            : LoadLogicalKeysToRebuild(rawTablesToRebuildPath);


        // -------------------------------------------------
        // 2. Generate raw tables + cleaned files + mapping
        // -------------------------------------------------
        var rawTables = new GenerateRawTables(
            rawInputDir,
            cleanedDir,
            sqlDir,
            logicalKeysToRebuild,
            rebuildAllRawTables
        );

        rawTables.Run();

        WriteCleanupSql(Path.Combine(sqlDir, "00_cleanup.sql"), rawTables.RebuiltTableNames, rebuildAllRawTables);

        // -------------------------------------------------
        // 3. Generate views
        // -------------------------------------------------
        new GenerateViews(dataMaps, tableMappingPath, sqlDir, logicalKeysToRebuild, rebuildAllRawTables).Run();

        // -------------------------------------------------
        // 4. Generate indexes
        // -------------------------------------------------
        new GenerateIndexes(sqlDir).Run();

        Console.WriteLine("Run Complete.");

        // Optional: avoid blocking in CI
        if (!Console.IsInputRedirected)
        {
            Console.ReadLine();
        }
    }

    private static string ResolveRawTablesToRebuildPath(string baseDir, IConfiguration configuration)
    {
        var configuredPath =
            configuration["RawTablesToRebuildPath"]
            ?? Environment.GetEnvironmentVariable("RAW_TABLES_TO_REBUILD_PATH");

        if (string.IsNullOrWhiteSpace(configuredPath))
        {
            return Path.Combine(baseDir, "raw_tables_to_rebuild.txt");
        }

        var resolvedPath = Path.IsPathRooted(configuredPath)
            ? configuredPath
            : Path.GetFullPath(Path.Combine(baseDir, configuredPath));

        if (!File.Exists(resolvedPath))
        {
            throw new FileNotFoundException($"Configured raw table rebuild list not found (paths are relative to {baseDir}.", resolvedPath);
        }

        Console.WriteLine($"Using raw table rebuild list from: {resolvedPath}");
        return resolvedPath;
    }

    private static bool ShouldRebuildAllRawTables(IConfiguration configuration)
    {
        var configuredValue =
            configuration["RebuildAllRawTables"]
            ?? Environment.GetEnvironmentVariable("REBUILD_ALL_RAW_TABLES");

        var rebuildAll = bool.TryParse(configuredValue, out var parsed) && parsed;

        if (rebuildAll)
        {
            Console.WriteLine("Full raw-table rebuild enabled. The rebuild list will be ignored.");
        }

        return rebuildAll;
    }

    private static HashSet<string> LoadLogicalKeysToRebuild(string path)
    {
        if (!File.Exists(path))
        {
            Console.WriteLine($"No raw table rebuild list found at {path}. No raw tables will be dropped or recreated.");
            return new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        var keys = File.ReadAllLines(path)
            .Select(line => line.Trim())
            .Where(line => !string.IsNullOrWhiteSpace(line) && !line.StartsWith('#'))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        Console.WriteLine($"Loaded {keys.Count} raw table key(s) to rebuild.");
        return keys;
    }

    private static void WriteCleanupSql(string path, IEnumerable<string> tableNamesToRebuild, bool rebuildAllRawTables)
    {
        var tablesToRebuild = tableNamesToRebuild
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(name => name, StringComparer.OrdinalIgnoreCase)
            .ToList();

        var sql = new StringBuilder(4000);
        sql.AppendLine("-- ================================================================");
        sql.AppendLine("-- 00_cleanup.sql");
        sql.AppendLine("-- Auto-generated by SAPData/Program.cs");
        sql.AppendLine("-- Drops only explicitly listed raw tables before regeneration.");
        sql.AppendLine("-- Recreates helper functions used by generated views.");
        sql.AppendLine("-- ================================================================");
        sql.AppendLine(@"\echo 'Ensuring required PostgreSQL extensions...''");
        sql.AppendLine(@"CREATE EXTENSION IF NOT EXISTS postgis;");
        sql.AppendLine();
        sql.AppendLine(@"\echo 'Cleaning up listed raw tables and regenerating helper functions...'");
        sql.AppendLine();
        sql.AppendLine("DO $$");
        sql.AppendLine("DECLARE");
        sql.AppendLine("  v_schema text := current_schema();");
        sql.AppendLine("  r record;");
        if (tablesToRebuild.Count > 0)
        {
            sql.AppendLine($"  tables_to_rebuild text[] := ARRAY[{string.Join(", ", tablesToRebuild.Select(name => $"'{name}'"))}];");
        }
        else
        {
            sql.AppendLine("  tables_to_rebuild text[] := ARRAY[]::text[];");
        }
        sql.AppendLine("BEGIN");
        sql.AppendLine("  EXECUTE format('SET search_path TO %I', v_schema);");
        sql.AppendLine();
        sql.AppendLine("  -- Drop only listed raw tables");
        sql.AppendLine("  FOR r IN");
        sql.AppendLine("    SELECT schemaname, tablename");
        sql.AppendLine("    FROM pg_tables");
        sql.AppendLine("    WHERE schemaname = v_schema");
        if (rebuildAllRawTables)
        {
            sql.AppendLine("      AND tablename LIKE 't\\_%' ESCAPE '\\'");
        }
        else
        {
            sql.AppendLine("      AND tablename = ANY(tables_to_rebuild)");
        }
        sql.AppendLine("  LOOP");
        sql.AppendLine("    EXECUTE format('DROP TABLE IF EXISTS %I.%I CASCADE', r.schemaname, r.tablename);");
        sql.AppendLine("  END LOOP;");
        sql.AppendLine("END $$;");
        sql.AppendLine();
        sql.AppendLine("-- =========================");
        sql.AppendLine("-- Cleaning helpers");
        sql.AppendLine("-- =========================");
        sql.AppendLine();
        sql.AppendLine("CREATE OR REPLACE FUNCTION clean_int(value TEXT)");
        sql.AppendLine("RETURNS INT");
        sql.AppendLine("LANGUAGE plpgsql");
        sql.AppendLine("IMMUTABLE");
        sql.AppendLine("AS $$");
        sql.AppendLine("BEGIN");
        sql.AppendLine("    IF value IS NULL OR trim(value) IN ('', 'NE', 'N', 'na', 'n/a', 'N/A', 'SUPP', '.', '-', '--', 'z') THEN");
        sql.AppendLine("        RETURN NULL;");
        sql.AppendLine("    END IF;");
        sql.AppendLine();
        sql.AppendLine("    RETURN value::INT;");
        sql.AppendLine();
        sql.AppendLine("EXCEPTION WHEN others THEN");
        sql.AppendLine("    RETURN NULL;");
        sql.AppendLine("END;");
        sql.AppendLine("$$;");
        sql.AppendLine();
        sql.AppendLine("CREATE OR REPLACE FUNCTION clean_numeric(value TEXT)");
        sql.AppendLine("RETURNS NUMERIC");
        sql.AppendLine("LANGUAGE plpgsql");
        sql.AppendLine("IMMUTABLE");
        sql.AppendLine("AS $$");
        sql.AppendLine("BEGIN");
        sql.AppendLine("    IF value IS NULL OR trim(value) IN ('', 'NE', 'N', 'na', 'n/a', 'N/A', 'SUPP', '.', '-', '--', 'z') THEN");
        sql.AppendLine("        RETURN NULL;");
        sql.AppendLine("    END IF;");
        sql.AppendLine();
        sql.AppendLine("    RETURN value::NUMERIC;");
        sql.AppendLine();
        sql.AppendLine("EXCEPTION WHEN others THEN");
        sql.AppendLine("    RETURN NULL;");
        sql.AppendLine("END;");
        sql.AppendLine("$$;");
        sql.AppendLine();
        sql.AppendLine("CREATE OR REPLACE FUNCTION clean_date(value TEXT)");
        sql.AppendLine("RETURNS DATE");
        sql.AppendLine("LANGUAGE plpgsql");
        sql.AppendLine("IMMUTABLE");
        sql.AppendLine("AS $$");
        sql.AppendLine("BEGIN");
        sql.AppendLine("    IF value IS NULL OR trim(value) IN ('', 'na', 'n/a', 'N/A', '.', '-', '--') THEN");
        sql.AppendLine("        RETURN NULL;");
        sql.AppendLine("    END IF;");
        sql.AppendLine();
        sql.AppendLine("    RETURN value::DATE;");
        sql.AppendLine();
        sql.AppendLine("EXCEPTION WHEN others THEN");
        sql.AppendLine("    RETURN NULL;");
        sql.AppendLine("END;");
        sql.AppendLine("$$;");
        sql.AppendLine();
        sql.AppendLine("CREATE OR REPLACE FUNCTION public.normalize_text(input_text text)");
        sql.AppendLine("RETURNS text AS $$");
        sql.AppendLine("DECLARE");
        sql.AppendLine("words text[];");
        sql.AppendLine("w text;");
        sql.AppendLine("syn text;");
        sql.AppendLine("normalized_words text[];");
        sql.AppendLine("BEGIN");
        sql.AppendLine("-- Split the input into words");
        sql.AppendLine("    words := regexp_split_to_array(lower(input_text), '\\s+');");
        sql.AppendLine("    FOREACH w IN ARRAY words LOOP");
        sql.AppendLine("        SELECT synonym INTO syn FROM public.thesaurus_synonyms WHERE word = w LIMIT 1;");
        sql.AppendLine("        normalized_words := array_append(normalized_words, COALESCE(syn, w));");
        sql.AppendLine("    END LOOP;");
        sql.AppendLine("    RETURN array_to_string(normalized_words, ' ');");
        sql.AppendLine("END;");
        sql.AppendLine("$$ LANGUAGE plpgsql IMMUTABLE;");
        sql.AppendLine();
        sql.AppendLine(@"\echo 'Cleanup complete.'");

        File.WriteAllText(path, sql.ToString(), new UTF8Encoding(false));
    }

    private static string FindProjectDirectoryDownwards(string projectFileName)
    {
        var startDir = Directory.GetCurrentDirectory();

        // Fast path: if we're already in the project directory.
        var direct = Path.Combine(startDir, projectFileName);
        if (File.Exists(direct))
            return startDir;

        // Primary search (works in GitHub Actions)
        var matches = Directory.GetFiles(startDir, projectFileName, SearchOption.AllDirectories);

        if (matches.Length == 0)
        {
            // ---------- LOCAL FALLBACK ----------
            // If running from bin/Debug/... or bin/Release/..., walk up until we exit /bin
            var dir = new DirectoryInfo(startDir);
            while (dir != null && dir.Name.Equals("bin", StringComparison.OrdinalIgnoreCase) == false)
            {
                dir = dir.Parent;
            }

            // If we found /bin, move one level up (project root candidate)
            if (dir?.Parent != null)
            {
                var projectRootCandidate = dir.Parent.FullName;

                var fallbackMatches = Directory.GetFiles(
                    projectRootCandidate,
                    projectFileName,
                    SearchOption.AllDirectories);

                if (fallbackMatches.Length > 0)
                {
                    var preferredFallback = fallbackMatches
                        .FirstOrDefault(p =>
                            string.Equals(
                                new DirectoryInfo(Path.GetDirectoryName(p)!).Name,
                                "SAPData",
                                StringComparison.OrdinalIgnoreCase))
                        ?? fallbackMatches[0];

                    return Path.GetDirectoryName(preferredFallback)!;
                }
            }

            throw new DirectoryNotFoundException(
                $"Could not find {projectFileName} under {startDir} (including bin fallback)"
            );
        }

        // Prefer SAPData project if multiple found
        var preferred = matches
            .FirstOrDefault(p =>
                string.Equals(
                    new DirectoryInfo(Path.GetDirectoryName(p)!).Name,
                    "SAPData",
                    StringComparison.OrdinalIgnoreCase))
            ?? matches[0];

        return Path.GetDirectoryName(preferred)!;
    }
}
