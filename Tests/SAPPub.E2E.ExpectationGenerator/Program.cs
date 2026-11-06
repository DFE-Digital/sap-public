using SAPPub.E2E.TestDataExpectationGenerator.KS4;

var repositoryRoot = ResolveRepositoryRoot(args);
var csvDirectory = Path.Combine(repositoryRoot, "SAPData", "DataMap", "SourceFiles");
var outputDirectory = GetArgument(args, "--output=")
    ?? Path.Combine(repositoryRoot, "Tests", "SAPPub.E2E.Tests", "TestData", "2526", "KS4");

// CML TODO put the Urn list somewhere else
string[] schoolUrns = ["124856", "136489", "136791", "137205", "137252", "138765", "140424"];

var generator = new Ks4ExpectationGenerator(csvDirectory, outputDirectory, schoolUrns);
var written = generator.Generate();

Console.WriteLine($"Wrote {written.Count} files to {outputDirectory}");
return 0;

static string? GetArgument(string[] args, string name) =>
    args.FirstOrDefault(a => a.StartsWith(name, StringComparison.OrdinalIgnoreCase))?[name.Length..];

static string ResolveRepositoryRoot(string[] args)
{
    var provided = GetArgument(args, "--repo-root=");
    if (provided is not null)
    {
        return Path.GetFullPath(provided);
    }

    foreach (var start in new[] { Directory.GetCurrentDirectory(), AppContext.BaseDirectory })
    {
        for (var directory = new DirectoryInfo(start); directory is not null; directory = directory.Parent)
        {
            if (File.Exists(Path.Combine(directory.FullName, "SAPPub.sln")))
            {
                return directory.FullName;
            }
        }
    }

    throw new InvalidOperationException("Could not locate SAPPub.sln. Pass --repo-root=<path>.");
}
