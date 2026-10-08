namespace SAPData.Helpers;

internal static class DirectoryHelpers
{
    public static string FindProjectDirectoryDownwards(string projectFileName)
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
