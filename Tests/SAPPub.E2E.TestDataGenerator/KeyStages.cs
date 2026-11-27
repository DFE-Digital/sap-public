using SAPPub.E2E.TestDataGenerator.KS4;

namespace SAPPub.E2E.TestDataGenerator;

/// <summary>
/// The key stages the generator supports. Register new key stages here.
/// </summary>
/// <param name="Name">Value accepted by --key-stage.</param>
/// <param name="DefaultOutputSubPath">Default output directory, relative to the E2E tests project.</param>
/// <param name="Create">Creates the generator given the csv and output directories.</param>
internal sealed record KeyStageDefinition(
    string Name,
    string DefaultOutputSubPath,
    Func<string, string, ITestDataGenerator> Create);

internal static class KeyStages
{
    private static readonly KeyStageDefinition[] All =
    [
        new("ks4", Path.Combine("TestData", "2526", "KS4"),
            (csvDirectory, outputDirectory) => new Ks4TestDataGenerator(csvDirectory, outputDirectory))
    ];

    public static string SupportedValues => string.Join(", ", All.Select(k => k.Name));

    public static KeyStageDefinition? Find(string? name) =>
        All.FirstOrDefault(k => string.Equals(k.Name, name, StringComparison.OrdinalIgnoreCase));
}
