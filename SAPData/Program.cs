using SAPData.Models;
using CsvHelper;
using System.Globalization;

namespace SAPData;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Generating Raw Data Tables and Scripts...");

        // Find the folder that contains SAPData.csproj
        string baseDir = FindProjectDirectory("SAPData.csproj");

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
        Console.ReadLine();
    }

    private static string FindProjectDirectory(string projectFileName)
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (dir != null)
        {
            if (File.Exists(Path.Combine(dir.FullName, projectFileName)))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException(
            $"Could not find {projectFileName} starting from {Directory.GetCurrentDirectory()}"
        );
    }
}
