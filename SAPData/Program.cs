using SAPData.Models;
using CsvHelper;
using System.Globalization;

namespace SAPData;

internal class Program
{
    static void Main(string[] args)
    {
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

        // -------------------------------------------------
        // 2. Generate raw tables + cleaned files + mapping
        // -------------------------------------------------
        new GenerateRawTables(
            rawInputDir,
            cleanedDir,
            sqlDir
        ).Run();

        // -------------------------------------------------
        // 3. Generate views
        // -------------------------------------------------
        new GenerateViews(
            dataMaps,
            tableMappingPath,
            sqlDir
        ).Run();

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

    private static string FindProjectDirectoryDownwards(string projectFileName)
    {
        var startDir = Directory.GetCurrentDirectory();

        // Fast path: if we're already in the project directory.
        var direct = Path.Combine(startDir, projectFileName);
        if (File.Exists(direct))
            return startDir;

        // Search for the csproj under the current directory (repo root in CI).
        var matches = Directory.GetFiles(startDir, projectFileName, SearchOption.AllDirectories);

        if (matches.Length == 0)
        {
            throw new DirectoryNotFoundException(
                $"Could not find {projectFileName} under {startDir}"
            );
        }

        // If multiple exist, choose the one under a folder named "SAPData" if possible.
        var preferred = matches
            .FirstOrDefault(p => string.Equals(new DirectoryInfo(Path.GetDirectoryName(p)!).Name, "SAPData", StringComparison.OrdinalIgnoreCase))
            ?? matches[0];

        return Path.GetDirectoryName(preferred)!;
    }
}
