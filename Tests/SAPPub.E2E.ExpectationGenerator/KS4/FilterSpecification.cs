namespace SAPPub.E2E.TestDataExpectationGenerator.KS4;

/// <summary>
/// Describes one expectation JSON file to generate: its output file name and the CSV column values
/// (including time_period) that a row must have to be included. A row must match every criterion.
/// </summary>
/// <param name="OutputFileName">The JSON file name, without the .json extension.</param>
/// <param name="Criteria">CSV column name to required value.</param>
internal sealed record FilterSpecification(
    string OutputFileName,
    IReadOnlyDictionary<string, string> Criteria)
{
    public static FilterSpecification Create(
        string outputFileName,
        params (string Column, string Value)[] criteria) =>
        new(outputFileName, criteria.ToDictionary(c => c.Column, c => c.Value, StringComparer.Ordinal));

    public bool Matches(IReadOnlyDictionary<string, string> row) =>
        Criteria.All(c => row.TryGetValue(c.Key, out var value) && value == c.Value);
}
