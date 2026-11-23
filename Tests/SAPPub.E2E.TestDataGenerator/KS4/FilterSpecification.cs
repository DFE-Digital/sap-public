namespace SAPPub.E2E.TestDataGenerator.KS4;

/// <summary>
/// Describes one expectation JSON file to generate: its output file name and the CSV column values (filter criteria)
/// that a row must have to be included. A row must match every criterion.
/// </summary>
/// <param name="OutputFileName">The JSON file name, without the .json extension.</param>
/// <param name="Criteria">CSV column name to the accepted values; a row matches a column if it has any one of them.</param>
internal sealed record FilterSpecification(
    string OutputFileName,
    IReadOnlyDictionary<string, IReadOnlyList<string>> Criteria)
{
    public static FilterSpecification Create(
        string outputFileName,
        params FilterCriterion[] criteria) =>
        new(outputFileName, criteria.ToDictionary(c => c.Column, c => c.Values, StringComparer.Ordinal));

    public bool Matches(IReadOnlyDictionary<string, string> row) =>
        Criteria.All(c => row.TryGetValue(c.Key, out var value) && c.Value.Contains(value, StringComparer.Ordinal));

    public const string SchoolUrnColumn = "school_urn";

    public IReadOnlyList<string> SchoolUrns => Criteria[SchoolUrnColumn];

    public FilterSpecification Without(string column) =>
        this with { Criteria = Criteria.Where(c => c.Key != column).ToDictionary(c => c.Key, c => c.Value, StringComparer.Ordinal) };
}

/// <summary>
/// A CSV column and the values accepted for it (OR). Converts implicitly from (column, value) and (column, values).
/// </summary>
internal readonly record struct FilterCriterion(string Column, IReadOnlyList<string> Values)
{
    public static implicit operator FilterCriterion((string Column, string Value) criterion) =>
        new(criterion.Column, [criterion.Value]);

    public static implicit operator FilterCriterion((string Column, string[] Values) criterion) =>
        new(criterion.Column, criterion.Values);
}
