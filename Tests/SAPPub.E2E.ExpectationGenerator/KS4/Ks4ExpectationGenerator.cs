using System.Text.Encodings.Web;
using System.Text.Json;
using SAPPub.E2E.TestDataExpectationGenerator;
using SAPPub.E2E.Tests.TestData.Models.KS4;

namespace SAPPub.E2E.TestDataExpectationGenerator.KS4;

/// <summary>
/// Reads the KS4 CSV files into the E2E test data models and writes one JSON expectation file
/// for each <see cref="FilterSpecification"/>.
/// </summary>
/// <param name="csvDirectory">Directory containing the cleaned CSV files.</param>
/// <param name="outputDirectory">Directory the JSON files are written to.</param>
/// <param name="schoolUrns">The schools to include, in the order they appear in the output.</param>
internal sealed class Ks4ExpectationGenerator(
    string csvDirectory,
    string outputDirectory,
    IReadOnlyList<string> schoolUrns)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public IReadOnlyList<string> Generate()
    {
        Directory.CreateDirectory(outputDirectory);

        var written = new List<string>();
        written.AddRange(GeneratePerformanceTables());
        written.AddRange(GenerateAllStateLa());
        written.AddRange(GenerateAllStateEngland());
        written.AddRange(GenerateInformationAboutSchools());
        return written;
    }

    private List<string> GeneratePerformanceTables()
    {
        var written = new List<string>();
        foreach (var spec in Ks4FilterSpecifications.CreatePerformanceTables())
        {
            var models = Ks4CsvMappings.PerformanceTables
                .Read(csvDirectory, row => spec.Matches(row) && schoolUrns.Contains(row["school_urn"]));

            written.Add(WriteJson(spec, OrderByUrn(models, m => m.SchoolUrn, spec)));
        }

        return written;
    }

    private List<string> GenerateAllStateLa()
    {
        var written = new List<string>();
        foreach (var spec in Ks4FilterSpecifications.CreateAllStateLa())
        {
            var laCodeByUrn = GetOldLaCodeByUrn(spec.Criteria["time_period"]);

            var laRows = Ks4CsvMappings.AllStateFundedPupilsLa
                .ReadWithRows(csvDirectory, row =>
                    spec.Matches(row)
                    && row["geographic_level"] == "Local authority"
                    && row["establishment_type_group"] == "All state-funded");

            var laModelByCode = new Dictionary<string, AllStateFundedPupilsLaTestDataModel>(StringComparer.Ordinal);
            foreach (var (row, model) in laRows)
            {
                if (!laModelByCode.TryAdd(row["old_la_code"], model))
                {
                    throw new InvalidOperationException(
                        $"{spec.OutputFileName}: more than one LA row found for old_la_code '{row["old_la_code"]}'.");
                }
            }

            var models = new List<AllStateFundedPupilsLaTestDataModel>();
            foreach (var urn in schoolUrns)
            {
                if (laCodeByUrn.TryGetValue(urn, out var laCode)
                    && laModelByCode.TryGetValue(laCode, out var laModel))
                {
                    models.Add(laModel with { Urn = urn });
                }
            }

            written.Add(WriteJson(spec, models));
        }

        return written;
    }

    private List<string> GenerateAllStateEngland()
    {
        var written = new List<string>();
        foreach (var spec in Ks4FilterSpecifications.CreateAllStateEngland())
        {
            var models = Ks4CsvMappings.AllStateFundedPupils
                .Read(csvDirectory, row =>
                    spec.Matches(row)
                    && row["geographic_level"] == "National"
                    && row["establishment_type_group"] == "All state-funded");

            written.Add(WriteJson(spec, models));
        }

        return written;
    }

    private List<string> GenerateInformationAboutSchools()
    {
        var written = new List<string>();
        foreach (var spec in Ks4FilterSpecifications.CreateInformationAboutSchools())
        {
            var models = Ks4CsvMappings.InformationAboutSchools
                .Read(csvDirectory, row => spec.Matches(row) && schoolUrns.Contains(row["school_urn"]));

            written.Add(WriteJson(spec, OrderByUrn(models, m => m.SchoolUrn, spec)));
        }

        return written;
    }

    // The all-state CSV has no school URN, so each school is linked to its LA through the school's
    // old_la_code in the performance tables CSV (Total/Total row for the same time period).
    private Dictionary<string, string> GetOldLaCodeByUrn(string timePeriod)
    {
        var path = Ks4CsvMappings.PerformanceTables.GetPath(csvDirectory);

        return CsvReader.ReadRows(path)
            .Where(row => row["time_period"] == timePeriod
                          && row["breakdown_topic"] == "Total"
                          && row["breakdown"] == "Total"
                          && schoolUrns.Contains(row["school_urn"]))
            .ToDictionary(row => row["school_urn"], row => row["old_la_code"], StringComparer.Ordinal);
    }

    private List<T> OrderByUrn<T>(List<T> models, Func<T, string> getUrn, FilterSpecification spec)
    {
        var duplicate = models.GroupBy(getUrn).FirstOrDefault(g => g.Count() > 1);
        if (duplicate is not null)
        {
            throw new InvalidOperationException(
                $"{spec.OutputFileName}: {duplicate.Count()} rows found for school URN '{duplicate.Key}'.");
        }

        return models.OrderBy(m => schoolUrns.ToList().IndexOf(getUrn(m))).ToList();
    }

    private string WriteJson<T>(FilterSpecification spec, IReadOnlyList<T> models)
    {
        var path = Path.Combine(outputDirectory, $"{spec.OutputFileName}.json");
        File.WriteAllText(path, JsonSerializer.Serialize(models, JsonOptions) + Environment.NewLine);
        return path;
    }
}
