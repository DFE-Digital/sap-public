namespace SAPPub.E2E.TestDataGenerator;

/// <summary>
/// Generates the JSON test data files for a single key stage.
/// </summary>
internal interface ITestDataGenerator
{
    /// <summary>
    /// Generate the json files and return the paths written.
    /// </summary>
    IReadOnlyList<string> Generate();
}
