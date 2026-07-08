using SAPPub.Core.ServiceModels.Compare;

namespace SAPPub.Core.Tests.TestBuilders;

public class SchoolAttainmentAndProgressDetailsBuilder
{
    private string? _urn;
    private Optional<double?> _attainment8Score = new();

    public SchoolAttainmentAndProgressDetailsBuilder WithUrn(string urn)
    {
        _urn = urn;
        return this;
    }

    public SchoolAttainmentAndProgressDetailsBuilder WithAttainment8Score(double? attainment8Score)
    {
        _attainment8Score.SetValue(attainment8Score);
        return this;
    }

    public SchoolAttainmentAndProgressDetails Build()
    {
        return new SchoolAttainmentAndProgressDetails
        {
            Urn = _urn ?? new Bogus.Faker().Random.Int(100000, 999999).ToString(),
            Attainment8Score =
                _attainment8Score.IsSet
                    ? _attainment8Score.Value
                    : Math.Round(new Bogus.Faker().Random.Double(5, 100), 1)
        };
    }
}
