using SAPPub.E2E.TestDataGenerator;

var keyStageArgument = GetArgument(args, "--key-stage=");
var keyStage = KeyStages.Find(keyStageArgument);
if (keyStage is null)
{
    Console.Error.WriteLine(string.IsNullOrWhiteSpace(keyStageArgument)
        ? "--key-stage is required."
        : $"Unsupported key stage '{keyStageArgument}'.");
    Console.Error.WriteLine($"Supported values: {KeyStages.SupportedValues}");
    Console.Error.WriteLine("Usage: dotnet run --project Tests/SAPPub.E2E.TestDataGenerator -- --key-stage=<value> [--output=<path>] [--repo-root=<path>]");
    return 1;
}

var repositoryRoot = ResolveRepositoryRoot(args);
var csvDirectory = Path.Combine(repositoryRoot, "SAPData", "DataMap", "SourceFiles");
var outputDirectory = GetArgument(args, "--output=")
    ?? Path.Combine(repositoryRoot, "Tests", "SAPPub.E2E.Tests", keyStage.DefaultOutputSubPath);

var generator = keyStage.Create(csvDirectory, outputDirectory);
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
