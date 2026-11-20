using SAPPub.E2E.Tests.TestData.Models.KS4;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace SAPPub.E2E.TestDataExpectationGenerator.KS4;

/// <summary>
/// Reads the KS4 CSV files into the E2E test data models and writes one JSON expectation file
/// for each <see cref="FilterSpecification"/>.
/// </summary>
/// <param name="csvDirectory">Directory containing the cleaned CSV files.</param>
/// <param name="outputDirectory">Directory the JSON files are written to.</param>
internal sealed class Ks4ExpectationGenerator(
    string csvDirectory,
    string outputDirectory)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    /// <summary>
    /// Generate json files for the tests
    /// </summary>
    /// <returns></returns>
    public IReadOnlyList<string> Generate()
    {
        Directory.CreateDirectory(outputDirectory);

        var written = new List<string>();
        written.AddRange(GeneratePerformanceTables());
        written.AddRange(GenerateAllStateLa());
        written.AddRange(GenerateAllStateEngland());
        written.AddRange(GenerateInformationAboutSchools());
        written.AddRange(GenerateSubjectEntries());
        written.AddRange(GenerateDestinations());
        return written;
    }

    // School and LA destinations are keyed by URN; England is one grouped object.
    private List<string> GenerateDestinations()
    {
        var written = new List<string>();

        var establishmentSpecs = Ks4FilterSpecifications.CreateDestinationsEstablishment();
        var urns = establishmentSpecs[0].SchoolUrns;
        var establishmentRows = establishmentSpecs
            .SelectMany(spec => Ks4CsvMappings.Destinations.Read(csvDirectory, spec.Matches))
            .ToList();
        var establishmentRowsByUrn = establishmentRows
            .GroupBy(row => row.SchoolUrn)
            .ToDictionary(
                group => group.Key,
                group => ToDestinationsModel(group, includeLaCode: true),
                StringComparer.Ordinal);
        var establishmentModels = urns
            .Where(establishmentRowsByUrn.ContainsKey)
            .ToDictionary(urn => urn, urn => establishmentRowsByUrn[urn], StringComparer.Ordinal);
        written.Add(WriteJson(establishmentSpecs[0], establishmentModels));

        var laCodeByUrn = establishmentRows
            .GroupBy(m => m.SchoolUrn)
            .ToDictionary(g => g.Key, g => g.First().OldLaCode, StringComparer.Ordinal);
        var laSpecs = Ks4FilterSpecifications.CreateDestinationsLa();
        var laRows = laSpecs
            .Select(spec => Ks4CsvMappings.Destinations.Read(csvDirectory, row => spec.Without(FilterSpecification.SchoolUrnColumn).Matches(row)))
            .ToList();
        var laModels = urns
            .Where(laCodeByUrn.ContainsKey)
            .ToDictionary(
                urn => urn,
                urn => ToDestinationsModel(
                    laRows.SelectMany(rows => rows
                        .Where(row => row.OldLaCode == laCodeByUrn[urn])
                        .Select(row => row with { SchoolUrn = urn })),
                    includeLaCode: true),
                StringComparer.Ordinal);
        written.Add(WriteJson(laSpecs[0], laModels));

        var englandSpecs = Ks4FilterSpecifications.CreateDestinationsEngland();
        var englandRows = englandSpecs
            .SelectMany(spec => Ks4CsvMappings.Destinations.Read(csvDirectory, spec.Matches))
            .ToList();
        written.Add(WriteJson(englandSpecs[0], ToDestinationsModel(englandRows, includeLaCode: false)));

        return written;
    }

    private static KS4DestinationsTestDataModel ToDestinationsModel(
        IEnumerable<KS4DestinationsInputDataModel> rows,
        bool includeLaCode)
    {
        var rowList = rows.ToList();
        if (rowList.Count == 0)
        {
            throw new InvalidOperationException("Cannot generate a destinations model without source rows.");
        }

        var periods = rowList
            .GroupBy(row => row.TimePeriod)
            .OrderByDescending(period => period.Key, StringComparer.Ordinal)
            .ToDictionary(
                period => period.Key,
                period => ToPeriodModel(period.ToList(), includeLaCode));

        return new KS4DestinationsTestDataModel
        {
            Periods = periods
        };
    }

    private static KS4DestinationsPeriodTestDataModel ToPeriodModel(
        List<KS4DestinationsInputDataModel> rows,
        bool includeLaCode)
    {
        var oldLaCodes = rows
            .Select(row => row.OldLaCode)
            .Distinct(StringComparer.Ordinal)
            .ToList();
        if (includeLaCode && oldLaCodes.Count != 1)
        {
            throw new InvalidOperationException(
                $"Expected one old_la_code for {rows[0].TimePeriod} destinations data, found {oldLaCodes.Count}.");
        }

        return new KS4DestinationsPeriodTestDataModel
        {
            OldLaCode = includeLaCode ? oldLaCodes[0] : null,
            DisadvantageStatuses = rows
                .GroupBy(row => row.DisadvantageStatus)
                .ToDictionary(
                    status => status.Key,
                    status => new KS4DestinationsGroupTestDataModel
                    {
                        CohortCount = status
                            .Single(row => row.DestinationGroup == "Overall destination")
                            .CohortCount,
                        Destinations = status
                            .Select(row => new KS4DestinationTestDataModel
                            {
                                DestinationGroup = row.DestinationGroup,
                                DestinationDescription = row.DestinationDescription,
                                PupilPercent = row.PupilPercent
                            })
                            .ToList()
                    })
        };
    }

    // SubjectsEntered.json holds, for each school, { urn, gcse, vocational, other } where each list is [subject, count] pairs.
    private List<string> GenerateSubjectEntries()
    {
        var gcseSpec = Ks4FilterSpecifications.CreateSubjectGcseEntries().Single();
        var vocationalSpec = Ks4FilterSpecifications.CreateSubjectVocationalEntries().Single();
        var otherSpec = Ks4FilterSpecifications.CreateSubjectOtherEntries().Single();

        var gcseBySchool = ToSubjectCounts(Ks4CsvMappings.SubjectsEntries.Read(csvDirectory, gcseSpec.Matches));
        var vocationalBySchool = ToSubjectCounts(Ks4CsvMappings.SubjectsEntries.Read(csvDirectory, vocationalSpec.Matches));
        var otherBySchool = ToSubjectCounts(Ks4CsvMappings.SubjectsEntries.Read(csvDirectory, otherSpec.Matches));

        var schools = gcseSpec.SchoolUrns
            .Select(urn => new
            {
                urn,
                gcse = gcseBySchool[urn].ToList(),
                vocational = vocationalBySchool[urn].ToList(),
                other = otherBySchool[urn].ToList()
            })
            .ToList();

        return [WriteJson(gcseSpec, schools)];
    }

    private static ILookup<string, object[]> ToSubjectCounts(List<SubjectEntriesInputDataModel> rows) =>
        rows.ToLookup(
            r => r.SchoolUrn,
            r => new object[] { r.SubjectDiscountGroup, int.Parse(r.NumberAchieving, System.Globalization.CultureInfo.InvariantCulture) });

    private List<string> GeneratePerformanceTables()
    {
        var written = new List<string>();
        foreach (var spec in Ks4FilterSpecifications.CreatePerformanceTables())
        {
            var models = Ks4CsvMappings.PerformanceTables
                .Read(csvDirectory, spec.Matches);

            written.Add(WriteJson(spec, OrderByUrn(models, m => m.SchoolUrn, spec.SchoolUrns, spec)));
        }

        return written;
    }

    private List<string> GenerateAllStateLa()
    {
        var written = new List<string>();
        foreach (var spec in Ks4FilterSpecifications.CreateAllStateLa())
        {
            // use the urn filter to find the LA code for each school, then use that to find the LA row in the all-state CSV
            var urns = spec.SchoolUrns;
            var laCodeByUrn = GetOldLaCodeByUrn(spec.Criteria["time_period"].Single(), urns);

            // The all-state CSV has no school URN column, so the URN criterion is not applied to its rows.
            var csvSpec = spec.Without(FilterSpecification.SchoolUrnColumn);
            var laRows = Ks4CsvMappings.AllStateFundedPupilsLa
                .ReadWithRows(csvDirectory, row =>
                    csvSpec.Matches(row)
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
            foreach (var urn in urns)
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
                .Read(csvDirectory, spec.Matches);

            written.Add(WriteJson(spec, OrderByUrn(models, m => m.SchoolUrn, spec.SchoolUrns, spec)));
        }

        return written;
    }

    // The all-state CSV
    // old_la_code in the performance tables CSV (Total/Total row for the same time period).
    private Dictionary<string, string> GetOldLaCodeByUrn(string timePeriod, IReadOnlyList<string> urns)
    {
        var path = Ks4CsvMappings.PerformanceTables.GetPath(csvDirectory);

        return CsvModelReader.ReadRows(path)
            .Where(row => row["time_period"] == timePeriod
                          && row["breakdown_topic"] == "Total"
                          && row["breakdown"] == "Total"
                          && urns.Contains(row["school_urn"]))
            .ToDictionary(row => row["school_urn"], row => row["old_la_code"], StringComparer.Ordinal);
    }

    private List<T> OrderByUrn<T>(List<T> models, Func<T, string> getUrn, IReadOnlyList<string> urnOrder, FilterSpecification spec)
    {
        var duplicate = models.GroupBy(getUrn).FirstOrDefault(g => g.Count() > 1);
        if (duplicate is not null)
        {
            throw new InvalidOperationException(
                $"{spec.OutputFileName}: {duplicate.Count()} rows found for school URN '{duplicate.Key}'.");
        }

        return models.OrderBy(m => urnOrder.ToList().IndexOf(getUrn(m))).ToList();
    }

    private string WriteJson<T>(FilterSpecification spec, T model)
    {
        var path = Path.Combine(outputDirectory, $"{spec.OutputFileName}.json");
        File.WriteAllText(path, JsonSerializer.Serialize(model, JsonOptions) + Environment.NewLine);
        return path;
    }
}
