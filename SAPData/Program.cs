using CsvHelper;
using Microsoft.Extensions.Configuration;
using SAPData.Helpers;
using SAPData.Models;
using System.Globalization;

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
        string baseDir = DirectoryHelpers.FindProjectDirectoryDownwards("SAPData.csproj");

        string dataMapDir = Path.Combine(baseDir, "DataMap");
        string rawInputDir = Path.Combine(dataMapDir, "SourceFiles");
        string cleanedDir = Path.Combine(dataMapDir, "CleanedFiles");
        string dataMapCsv = Path.Combine(dataMapDir, "datamap.csv");
        string sqlDir = Path.Combine(baseDir, "Sql");
        string tableMappingPath = Path.Combine(sqlDir, "tablemapping.csv");
        string rawTablesToRebuildPath = SelectiveIngestionHelpers.ResolveRawTablesToRebuildPath(baseDir, configuration);

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

        var rebuildAllRawTables = SelectiveIngestionHelpers.ShouldRebuildAllRawTables(configuration);
        var logicalKeysToRebuild = rebuildAllRawTables
            ? new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            : SelectiveIngestionHelpers.LoadLogicalKeysToRebuild(rawTablesToRebuildPath);


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

        SqlHelpers.WriteCleanupSql(Path.Combine(sqlDir, "00_cleanup.sql"), rawTables.RebuiltTableNames, rebuildAllRawTables);

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
}