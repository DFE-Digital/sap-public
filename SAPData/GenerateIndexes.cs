using System.Text;

namespace SAPData;

public class GenerateIndexes
{
    private readonly string _sqlDir;
    private static readonly Encoding Utf8NoBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

    public GenerateIndexes(string sqlDir)
    {
        _sqlDir = sqlDir;
    }

    public void Run()
    {
        string outputPath = Path.Combine(_sqlDir, "04_indexes.sql");

        var sb = new StringBuilder();

        sb.AppendLine("-- ================================================================");
        sb.AppendLine("-- 04_indexes.sql");
        sb.AppendLine("-- Indexes for materialized views (AUTO-GENERATED)");
        sb.AppendLine("-- ================================================================");
        sb.AppendLine();
        sb.AppendLine("-- NOTE:");
        sb.AppendLine("-- - Uses quoted identifiers to respect case-sensitive columns");
        sb.AppendLine("-- - Guards index creation when a view is not present");
        sb.AppendLine("-- - Emits NOTICE messages so execution output is descriptive");
        sb.AppendLine();

        var indexes = new Dictionary<string, string>
        {
            // Establishment
            { "v_establishment",                 "\"URN\"" },
            { "v_establishment_links",           "\"URN\"" },
            { "v_establishment_group_links",     "\"Group UID\"" },
            { "v_establishment_subject_entries", "\"school_urn\"" },

            { "v_establishment_absence",      "\"Id\"" },
            { "v_establishment_destinations", "\"Id\"" },
            { "v_establishment_performance",  "\"Id\"" },
            { "v_establishment_workforce",    "\"Id\"" },

            // England
            { "v_england_destinations", "\"Id\"" },
            { "v_england_performance",  "\"Id\"" },

            // Local Authority
            { "v_la_destinations", "\"Id\"" },
            { "v_la_performance",  "\"Id\"" }
        };

        foreach (var kvp in indexes)
        {
            string view = kvp.Key;
            string column = kvp.Value;

            string colSlug = column.Trim('"')
                .ToLowerInvariant()
                .Replace(" ", "_");

            string indexName = $"idx_{view}_{colSlug}";

            // Use a DO block so we can conditionally check view + index existence and print notices.
            sb.AppendLine("DO $$");
            sb.AppendLine("BEGIN");
            sb.AppendLine($"  IF to_regclass('public.{view}') IS NULL THEN");
            sb.AppendLine($"    RAISE NOTICE 'SKIP: view public.{view} does not exist (index {indexName} not created)';");
            sb.AppendLine("  ELSE");
            sb.AppendLine($"    IF to_regclass('public.{indexName}') IS NULL THEN");
            sb.AppendLine($"      RAISE NOTICE 'CREATE: index public.{indexName} on public.{view} ({column})';");
            sb.AppendLine($"      CREATE INDEX {indexName} ON public.{view} ({column});");
            sb.AppendLine("    ELSE");
            sb.AppendLine($"      RAISE NOTICE 'OK: index public.{indexName} already exists';");
            sb.AppendLine("    END IF;");
            sb.AppendLine("  END IF;");
            sb.AppendLine("END $$;");
            sb.AppendLine();
        }

        File.WriteAllText(outputPath, sb.ToString(), Utf8NoBom);

        Console.WriteLine("Generated view index script:");
        Console.WriteLine(outputPath);
    }
}
