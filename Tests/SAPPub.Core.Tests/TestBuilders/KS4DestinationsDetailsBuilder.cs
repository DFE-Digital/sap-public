using SAPPub.Core.Entities;
using SAPPub.Core.ServiceModels.Destinations;
using SAPPub.Core.ValueObjects;

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
        RelativeYearValues<CodedDouble> RandomYears() => new()
        {
            CurrentYear = CodedDoubleFactory.Create(Math.Round(faker.Random.Double(5, 100), 1)),
            PreviousYear = CodedDoubleFactory.Create(Math.Round(faker.Random.Double(5, 100), 1)),
            TwoYearsAgo = CodedDoubleFactory.Create(Math.Round(faker.Random.Double(5, 100), 1))
        };

        return new KS4DestinationsDetails
        {
            Urn = _urn ?? string.Empty,
            SchoolName = _establishmentName ?? string.Empty,
            IsKS2 = false,
            IsKS4 = _isKs4,
            IsKS5 = _isKs5,
            LocalAuthorityName = _laName ?? string.Empty,
            SchoolAll = RandomYears(),
            LocalAuthorityAll = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = CodedDoubleFactory.Create(_laPercentage.IsSet ? _laPercentage.Value : Math.Round(faker.Random.Double(5, 100), 1)),
                PreviousYear = CodedDoubleFactory.Create(Math.Round(faker.Random.Double(5, 100), 1)),
                TwoYearsAgo = CodedDoubleFactory.Create(Math.Round(faker.Random.Double(5, 100), 1))
            },
            EnglandAll = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = CodedDoubleFactory.Create(_englandPercentage.IsSet ? _englandPercentage.Value : Math.Round(faker.Random.Double(5, 100), 1)),
                PreviousYear = CodedDoubleFactory.Create(Math.Round(faker.Random.Double(5, 100), 1)),
                TwoYearsAgo = CodedDoubleFactory.Create(Math.Round(faker.Random.Double(5, 100), 1))
            },
            SchoolDisadvantagedAll = RandomYears(),
            LocalAuthorityDisadvantagedAll = RandomYears(),
            EnglandDisadvantagedAll = RandomYears(),
            LocalAuthorityNonDisadvantagedAll = RandomYears(),
            EnglandNonDisadvantagedAll = RandomYears(),
            SchoolEducation = RandomYears(),
            LocalAuthorityEducation = RandomYears(),
            EnglandEducation = RandomYears(),
            SchoolEmployment = RandomYears(),
            LocalAuthorityEmployment = RandomYears(),
            EnglandEmployment = RandomYears(),
            SchoolApprentice = RandomYears(),
            LocalAuthorityApprentice = RandomYears(),
            EnglandApprentice = RandomYears(),
            SchoolFurtherEducation = RandomYears(),
            LocalAuthorityFurtherEducation = RandomYears(),
            EnglandFurtherEducation = RandomYears(),
            SchoolSchoolSixthForm = RandomYears(),
            LocalAuthoritySchoolSixthForm = RandomYears(),
            EnglandSchoolSixthForm = RandomYears(),
            SchoolCollegeSixthForm = RandomYears(),
            LocalAuthorityCollegeSixthForm = RandomYears(),
            EnglandCollegeSixthForm = RandomYears(),
            SchoolOtherEducation = RandomYears(),
            LocalAuthorityOtherEducation = RandomYears(),
            EnglandOtherEducation = RandomYears(),
            SchoolNotSustained = RandomYears(),
            LocalAuthorityNotSustained = RandomYears(),
            EnglandNotSustained = RandomYears(),
            SchoolUnknown = RandomYears(),
            LocalAuthorityUnknown = RandomYears(),
            EnglandUnknown = RandomYears(),
        };
    }

    public KS4DestinationsDetails BuildResultsNotAvailable()
    {
        RelativeYearValues<CodedDouble> NoValue() => new() { CurrentYear = CodedDoubleFactory.Create(null) };

        return new KS4DestinationsDetails
        {
            Urn = _urn ?? string.Empty,
            SchoolName = _establishmentName ?? string.Empty,
            IsKS2 = false,
            IsKS4 = true,
            IsKS5 = true,
            LocalAuthorityName = _laName ?? string.Empty,
            SchoolAll = NoValue(),
            LocalAuthorityAll = NoValue(),
            EnglandAll = NoValue(),
            SchoolDisadvantagedAll = NoValue(),
            LocalAuthorityDisadvantagedAll = NoValue(),
            EnglandDisadvantagedAll = NoValue(),
            LocalAuthorityNonDisadvantagedAll = NoValue(),
            EnglandNonDisadvantagedAll = NoValue(),
            SchoolEducation = NoValue(),
            LocalAuthorityEducation = NoValue(),
            EnglandEducation = NoValue(),
            SchoolEmployment = NoValue(),
            LocalAuthorityEmployment = NoValue(),
            EnglandEmployment = NoValue(),
            SchoolApprentice = NoValue(),
            LocalAuthorityApprentice = NoValue(),
            EnglandApprentice = NoValue(),
            SchoolFurtherEducation = NoValue(),
            LocalAuthorityFurtherEducation = NoValue(),
            EnglandFurtherEducation = NoValue(),
            SchoolSchoolSixthForm = NoValue(),
            LocalAuthoritySchoolSixthForm = NoValue(),
            EnglandSchoolSixthForm = NoValue(),
            SchoolCollegeSixthForm = NoValue(),
            LocalAuthorityCollegeSixthForm = NoValue(),
            EnglandCollegeSixthForm = NoValue(),
            SchoolOtherEducation = NoValue(),
            LocalAuthorityOtherEducation = NoValue(),
            EnglandOtherEducation = NoValue(),
            SchoolNotSustained = NoValue(),
            LocalAuthorityNotSustained = NoValue(),
            EnglandNotSustained = NoValue(),
            SchoolUnknown = NoValue(),
            LocalAuthorityUnknown = NoValue(),
            EnglandUnknown = NoValue(),
        };
    }
}
