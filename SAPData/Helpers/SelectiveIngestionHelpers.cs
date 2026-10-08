using Microsoft.Extensions.Configuration;

namespace SAPData.Helpers;

internal static class SelectiveIngestionHelpers
{

    public static string ResolveRawTablesToRebuildPath(string baseDir, IConfiguration configuration)
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

    public static bool ShouldRebuildAllRawTables(IConfiguration configuration)
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

    public static HashSet<string> LoadLogicalKeysToRebuild(string path)
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

}
