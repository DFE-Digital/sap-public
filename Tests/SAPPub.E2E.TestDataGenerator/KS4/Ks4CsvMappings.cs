using SAPPub.E2E.Tests.TestData.Models.KS4;

namespace SAPPub.E2E.TestDataGenerator.KS4;

/// <summary>
/// Maps one CSV file to the E2E test data model that its rows populate.
/// </summary>
/// <typeparam name="TModel">The test data model created for each CSV row.</typeparam>
/// <param name="FileName">The CSV file name within the cleaned files directory.</param>
/// <param name="OptionalColumns">Model columns that may be absent from the CSV and are set later.</param>
internal sealed record Ks4CsvMapping<TModel>(
    string FileName,
    IReadOnlySet<string>? OptionalColumns = null)
    where TModel : new()
{
    public string GetPath(string csvDirectory) => Path.Combine(csvDirectory, FileName);

    public List<TModel> Read(
        string csvDirectory,
        Func<IReadOnlyDictionary<string, string>, bool>? rowFilter = null) =>
        CsvModelReader.Read<TModel>(GetPath(csvDirectory), rowFilter, OptionalColumns);

    public List<(IReadOnlyDictionary<string, string> Row, TModel Model)> ReadWithRows(
        string csvDirectory,
        Func<IReadOnlyDictionary<string, string>, bool>? rowFilter = null) =>
        CsvModelReader.ReadWithRows<TModel>(GetPath(csvDirectory), rowFilter, OptionalColumns);
}

/// <summary>
/// The KS4 CSV files and the test data model each one is read into.
/// </summary>
internal static class Ks4CsvMappings
{
    public static readonly Ks4CsvMapping<PerformanceTablesTestDataModel> PerformanceTables =
        new("manual_202526_performance_tables_schools_provisional.csv");

    public static readonly Ks4CsvMapping<AllStateFundedPupilsTestDataModel> AllStateFundedPupils =
        new("manual_202526_all_state_funded_pupils_characteristics_and_geography_breakdowns_provisional.csv");

    public static readonly Ks4CsvMapping<AllStateFundedPupilsLaTestDataModel> AllStateFundedPupilsLa =
        new(
            "manual_202526_all_state_funded_pupils_characteristics_and_geography_breakdowns_provisional.csv",
            new HashSet<string>(StringComparer.Ordinal) { "urn" });

    public static readonly Ks4CsvMapping<InformationAboutSchoolsTestDataModel> InformationAboutSchools =
        new("manual_202526_information_about_schools_provisional.csv");

    public static readonly Ks4CsvMapping<KS4DestinationsInputDataModel> Destinations =
        new("manual_ees_ks4_202324_api.csv");

    public static readonly Ks4CsvMapping<SubjectEntriesInputDataModel> SubjectsEntries =
        new("manual_202526_subject_school_all_exam_entriesgrades_provisional.csv");
}
