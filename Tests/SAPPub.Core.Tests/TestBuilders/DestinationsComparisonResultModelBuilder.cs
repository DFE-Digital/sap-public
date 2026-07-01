using SAPPub.Core.ServiceModels.Compare;

namespace SAPPub.Core.Tests.TestBuilders;

public class SchoolDestinationDetailsBuilder
{
    private string? _urn;
    private Optional<double?> _percentInEducationEmploymentOrTraining = new();

    public SchoolDestinationDetailsBuilder WithUrn(string urn)
    {
        _urn = urn;
        return this;
    }

    public SchoolDestinationDetailsBuilder WithPercentInEducationEmploymentOrTraining(double? percentInEducationEmploymentOrTraining)
    {
        _percentInEducationEmploymentOrTraining.SetValue(percentInEducationEmploymentOrTraining);
        return this;
    }

    public SchoolDestinationDetails Build()
    {
        return new SchoolDestinationDetails
        {
            URN = _urn ?? new Bogus.Faker().Random.Int(100000, 999999).ToString(),
            PercentInEducationEmploymentOrTraining =
                _percentInEducationEmploymentOrTraining.IsSet
                    ? _percentInEducationEmploymentOrTraining.Value
                    : Math.Round(new Bogus.Faker().Random.Double(5, 100), 1)
        };
    }
}

public class DestinationsComparisonResultModelBuilder
{
    private Optional<double?> _englandPercentage = new Optional<double?>();
    private List<string>? _urns;
    private List<Action<SchoolDestinationDetailsBuilder>>? _schoolConfigurations;

    public DestinationsComparisonResultModelBuilder WithEnglandPercentage(double? englandPercentage)
    {
        _englandPercentage.SetValue(englandPercentage);
        return this;
    }
    public DestinationsComparisonResultModelBuilder WithSchoolUrns(List<string> urns)
    {
        _urns = urns;
        return this;
    }

    public DestinationsComparisonResultModelBuilder WithNumberOfSchools(int numberOfSchools)
    {
        _urns = Enumerable.Range(0, numberOfSchools).Select(_ => new Bogus.Faker().Random.Int(100000, 999999).ToString()).ToList();
        return this;
    }

    public DestinationsComparisonResultModelBuilder WithSchoolDetails(List<Action<SchoolDestinationDetailsBuilder>> configureList)
    {
        _schoolConfigurations = configureList;
        return this;
    }

    public DestinationsComparisonResultModel Build()
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
                var schoolDestinationDetailsBuilder = new SchoolDestinationDetailsBuilder();
                configure(schoolDestinationDetailsBuilder);
                return schoolDestinationDetailsBuilder.Build();
            }).ToList();
            return new DestinationsComparisonResultModel
            {
                EnglandPercentage = _englandPercentage.IsSet ? _englandPercentage.Value : Math.Round(faker.Random.Double(5, 100), 1),
                SchoolDetails = schoolDetails
            };
        }
        else
        {
            var schoolDetails = _urns!.Select(urn =>
            {
                return new SchoolDestinationDetailsBuilder()
                 .WithUrn(urn)
                 .Build();
            }).ToList();

            return new DestinationsComparisonResultModel
            {
                EnglandPercentage = _englandPercentage.IsSet ? _englandPercentage.Value : Math.Round(faker.Random.Double(5, 100), 1),
                SchoolDetails = schoolDetails
            };
        }
    }
}
