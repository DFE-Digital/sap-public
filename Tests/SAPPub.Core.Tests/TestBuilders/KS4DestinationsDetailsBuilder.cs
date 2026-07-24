using SAPPub.Core.Entities;
using SAPPub.Core.ServiceModels.Destinations;

namespace SAPPub.Core.Tests.TestBuilders;

public class KS4DestinationsDetailsBuilder
{
    private string? _urn;
    private string? _establishmentName;
    private string? _laName;
    private bool _isKs4;
    private bool _isKs5;
    private Optional<double?> _englandPercentage = new Optional<double?>();
    private Optional<double?> _laPercentage = new Optional<double?>();

    public KS4DestinationsDetailsBuilder WithUrn(string urn)
    {
        _urn = urn;
        return this;
    }

    public KS4DestinationsDetailsBuilder WithEstablishmentName(string establishmentName)
    {
        _establishmentName = establishmentName;
        return this;
    }

    public KS4DestinationsDetailsBuilder WithLAName(string laName)
    {
        _laName = laName;
        return this;
    }

    public KS4DestinationsDetailsBuilder WithLaPercentage(double? laPercentage)
    {
        _laPercentage.SetValue(laPercentage);
        return this;
    }
    public KS4DestinationsDetailsBuilder WithEnglandPercentage(double? englandPercentage)
    {
        _englandPercentage.SetValue(englandPercentage);
        return this;
    }

    public KS4DestinationsDetailsBuilder WithKS4(bool isKS4)
    {
        _isKs4 = isKS4;
        return this;
    }
    public KS4DestinationsDetailsBuilder WithKS5(bool isKS5)
    {
        _isKs5 = isKS5;
        return this;
    }

    public KS4DestinationsDetails Build()
    {
        var faker = new Bogus.Faker();
        return new KS4DestinationsDetails
        {
            Urn = _urn ?? string.Empty,
            SchoolName = _establishmentName ?? string.Empty,
            IsKS2 = false,
            IsKS4 = _isKs4,
            IsKS5 = _isKs5,
            LocalAuthorityName = _laName ?? string.Empty,
            SchoolAll = new RelativeYearValues<double?>
            {
                CurrentYear = Math.Round(faker.Random.Double(5, 100), 1),
                PreviousYear = Math.Round(faker.Random.Double(5, 100), 1),
                TwoYearsAgo = Math.Round(faker.Random.Double(5, 100), 1)
            },
            LocalAuthorityAll = new RelativeYearValues<double?>
            {
                CurrentYear = _laPercentage.IsSet ? _laPercentage.Value : Math.Round(faker.Random.Double(5, 100), 1),
                PreviousYear = Math.Round(faker.Random.Double(5, 100), 1),
                TwoYearsAgo = Math.Round(faker.Random.Double(5, 100), 1)
            },
            EnglandAll = new RelativeYearValues<double?>
            {
                CurrentYear = _englandPercentage.IsSet ? _englandPercentage.Value : Math.Round(faker.Random.Double(5, 100), 1),
                PreviousYear = Math.Round(faker.Random.Double(5, 100), 1),
                TwoYearsAgo = Math.Round(faker.Random.Double(5, 100), 1)
            },
            SchoolEducation = new RelativeYearValues<double?>
            {
                CurrentYear = Math.Round(faker.Random.Double(5, 100), 1),
                PreviousYear = Math.Round(faker.Random.Double(5, 100), 1),
                TwoYearsAgo = Math.Round(faker.Random.Double(5, 100), 1)
            },
            LocalAuthorityEducation = new RelativeYearValues<double?>
            {
                CurrentYear = Math.Round(faker.Random.Double(5, 100), 1),
                PreviousYear = Math.Round(faker.Random.Double(5, 100), 1),
                TwoYearsAgo = Math.Round(faker.Random.Double(5, 100), 1)
            },
            EnglandEducation = new RelativeYearValues<double?>
            {
                CurrentYear = Math.Round(faker.Random.Double(5, 100), 1),
                PreviousYear = Math.Round(faker.Random.Double(5, 100), 1),
                TwoYearsAgo = Math.Round(faker.Random.Double(5, 100), 1)
            },
            SchoolEmployment = new RelativeYearValues<double?>
            {
                CurrentYear = Math.Round(faker.Random.Double(5, 100), 1),
                PreviousYear = Math.Round(faker.Random.Double(5, 100), 1),
                TwoYearsAgo = Math.Round(faker.Random.Double(5, 100), 1)
            },
            LocalAuthorityEmployment = new RelativeYearValues<double?>
            {
                CurrentYear = Math.Round(faker.Random.Double(5, 100), 1),
                PreviousYear = Math.Round(faker.Random.Double(5, 100), 1),
                TwoYearsAgo = Math.Round(faker.Random.Double(5, 100), 1)
            },
            EnglandEmployment = new RelativeYearValues<double?>
            {
                CurrentYear = Math.Round(faker.Random.Double(5, 100), 1),
                PreviousYear = Math.Round(faker.Random.Double(5, 100), 1),
                TwoYearsAgo = Math.Round(faker.Random.Double(5, 100), 1)
            },
            SchoolApprentice = new RelativeYearValues<double?>
            {
                CurrentYear = Math.Round(faker.Random.Double(5, 100), 1),
                PreviousYear = Math.Round(faker.Random.Double(5, 100), 1),
                TwoYearsAgo = Math.Round(faker.Random.Double(5, 100), 1)
            },
            LocalAuthorityApprentice = new RelativeYearValues<double?>
            {
                CurrentYear = Math.Round(faker.Random.Double(5, 100), 1),
                PreviousYear = Math.Round(faker.Random.Double(5, 100), 1),
                TwoYearsAgo = Math.Round(faker.Random.Double(5, 100), 1)
            },
            EnglandApprentice = new RelativeYearValues<double?>
            {
                CurrentYear = Math.Round(faker.Random.Double(5, 100), 1),
                PreviousYear = Math.Round(faker.Random.Double(5, 100), 1),
                TwoYearsAgo = Math.Round(faker.Random.Double(5, 100), 1)
            },
        };
    }

    public KS4DestinationsDetails BuildResultsNotAvailable()
    {
        return new KS4DestinationsDetails
        {
            Urn = _urn ?? string.Empty,
            SchoolName = _establishmentName ?? string.Empty,
            IsKS2 = false,
            IsKS4 = true,
            IsKS5 = true,
            LocalAuthorityName = _laName ?? string.Empty,
            SchoolAll = new RelativeYearValues<double?> { CurrentYear = null },
            LocalAuthorityAll = new RelativeYearValues<double?> { CurrentYear = null },
            EnglandAll = new RelativeYearValues<double?> { CurrentYear = null },
            SchoolEducation = new RelativeYearValues<double?> { CurrentYear = null },
            LocalAuthorityEducation = new RelativeYearValues<double?> { CurrentYear = null },
            EnglandEducation = new RelativeYearValues<double?> { CurrentYear = null },
            SchoolEmployment = new RelativeYearValues<double?> { CurrentYear = null },
            LocalAuthorityEmployment = new RelativeYearValues<double?> { CurrentYear = null },
            EnglandEmployment = new RelativeYearValues<double?> { CurrentYear = null },
            SchoolApprentice = new RelativeYearValues<double?> { CurrentYear = null },
            LocalAuthorityApprentice = new RelativeYearValues<double?> { CurrentYear = null },
            EnglandApprentice = new RelativeYearValues<double?> { CurrentYear = null },
        };
    }
}
