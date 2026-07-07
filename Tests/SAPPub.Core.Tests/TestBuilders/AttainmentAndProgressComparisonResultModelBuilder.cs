using SAPPub.Core.ServiceModels.Compare;

namespace SAPPub.Core.Tests.TestBuilders;

public class AttainmentAndProgressComparisonResultModelBuilder
{
    private Optional<double?> _englandPercentage = new Optional<double?>();
    private List<string>? _urns;
    private List<Action<SchoolAttainmentAndProgressDetailsBuilder>>? _schoolConfigurations;

    public AttainmentAndProgressComparisonResultModelBuilder WithEnglandPercentage(double? englandPercentage)
    {
        _englandPercentage.SetValue(englandPercentage);
        return this;
    }
    public AttainmentAndProgressComparisonResultModelBuilder WithSchoolUrns(List<string> urns)
    {
        _urns = urns;
        return this;
    }

    public AttainmentAndProgressComparisonResultModelBuilder WithNumberOfSchools(int numberOfSchools)
    {
        _urns = Enumerable.Range(0, numberOfSchools).Select(_ => new Bogus.Faker().Random.Int(100000, 999999).ToString()).ToList();
        return this;
    }

    public AttainmentAndProgressComparisonResultModelBuilder WithSchoolDetails(List<Action<SchoolAttainmentAndProgressDetailsBuilder>> configureList)
    {
        _schoolConfigurations = configureList;
        return this;
    }

    public AttainmentAndProgressComparisonResultsModel Build()
    {
        if (_urns is null && _schoolConfigurations is null)
        {
            throw new ArgumentException("Must provide either urn configuration, number of schools or school configurations. Please provide one.");
        }
        if (_urns is not null && _schoolConfigurations is not null)
        {
            throw new ArgumentException("Cannot provide both urn configuration and school configurations. Please provide only one.");
        }

        var faker = new Bogus.Faker();

        if (_schoolConfigurations is not null)
        {
            var schoolDetails = _schoolConfigurations.Select(configure =>
            {
                var schoolAttainmentAndProgressDetailsBuilder = new SchoolAttainmentAndProgressDetailsBuilder();
                configure(schoolAttainmentAndProgressDetailsBuilder);
                return schoolAttainmentAndProgressDetailsBuilder.Build();
            }).ToList();

            return new AttainmentAndProgressComparisonResultsModel
            {
                EnglandAverage = _englandPercentage.IsSet ? _englandPercentage.Value : Math.Round(faker.Random.Double(5, 100), 1),
                SchoolDetails = schoolDetails
            };
        }
        else
        {
            var schoolDetails = _urns!.Select(urn =>
            {
                return new SchoolAttainmentAndProgressDetailsBuilder()
                 .WithUrn(urn)
                 .Build();
            }).ToList();

            return new AttainmentAndProgressComparisonResultsModel
            {
                EnglandAverage = _englandPercentage.IsSet ? _englandPercentage.Value : Math.Round(faker.Random.Double(5, 100), 1),
                SchoolDetails = schoolDetails
            };
        }
    }
}
