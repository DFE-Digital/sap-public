using Bogus;
using SAPPub.Core.Entities;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Tests.TestBuilders;

public class EnglishAndMathsResultsModelBuilder
{
    private Faker _faker = new Faker();
    private string? _urn;
    private string? _establishmentName;
    private string? _laName;
    private bool _isKS2;
    private bool _isKS4;
    private bool _isKS5;

    public EnglishAndMathsResultsModelBuilder WithUrn(string urn)
    {
        _urn = urn;
        return this;
    }
    public EnglishAndMathsResultsModelBuilder WithEstablishmentName(string establishmentName)
    {
        _establishmentName = establishmentName;
        return this;
    }

    public EnglishAndMathsResultsModelBuilder WithLaName(string laName)
    {
        _laName = laName;
        return this;
    }

    public EnglishAndMathsResultsModelBuilder WithIsKS2(bool isKS)
    {
        _isKS2 = isKS;
        return this;
    }
    public EnglishAndMathsResultsModelBuilder WithIsKS4(bool isKS)
    {
        _isKS4 = isKS;
        return this;
    }
    public EnglishAndMathsResultsModelBuilder WithIsKS5(bool isKS)
    {
        _isKS5 = isKS;
        return this;
    }

    private RelativeYearValues<double?> _EstablishmentAll { get; set; } = new RelativeYearValues<double?>
    {
        CurrentYear = null
    };
    private RelativeYearValues<double?> _LocalAuthorityAll { get; set; } = new RelativeYearValues<double?>
    {
        CurrentYear = null
    };
    private RelativeYearValues<double?> _EnglandAll { get; set; } = new RelativeYearValues<double?>
    {
        CurrentYear = null
    };
    private RelativeYearValues<double?> _EstablishmentBoys { get; set; } = new RelativeYearValues<double?>
    {
        CurrentYear = null
    };
    private RelativeYearValues<double?> _LocalAuthorityBoys { get; set; } = new RelativeYearValues<double?>
    {
        CurrentYear = null
    };
    private RelativeYearValues<double?> _EnglandBoys { get; set; } = new RelativeYearValues<double?>
    {
        CurrentYear = null
    };
    private RelativeYearValues<double?> _EstablishmentGirls { get; set; } = new RelativeYearValues<double?>
    {
        CurrentYear = null
    };
    private RelativeYearValues<double?> _LocalAuthorityGirls { get; set; } = new RelativeYearValues<double?>
    {
        CurrentYear = null
    };
    private RelativeYearValues<double?> _EnglandGirls { get; set; } = new RelativeYearValues<double?>
    {
        CurrentYear = null
    };
    private RelativeYearValues<CodedDouble> _EstablishmentDisadvantaged { get; set; } = CodedYearNoValue("z");
    private RelativeYearValues<CodedDouble> _LocalAuthorityDisadvantaged { get; set; } = CodedYearNoValue("z");
    private RelativeYearValues<CodedDouble> _EnglandDisadvantaged { get; set; } = CodedYearNoValue("z");
    private RelativeYearValues<CodedDouble> _LocalAuthorityNonDisadvantaged { get; set; } = CodedYearNoValue("z");
    private RelativeYearValues<CodedDouble> _EnglandNonDisadvantaged { get; set; } = CodedYearNoValue("z");

    public EnglishAndMathsResultsModelBuilder WithCurrentYearData()
    {
        _EstablishmentAll = new RelativeYearValues<double?>
        {
            CurrentYear = Math.Round(_faker.Random.Double(0, 80), 1)
        };
        _LocalAuthorityAll = new RelativeYearValues<double?>
        {
            CurrentYear = Math.Round(_faker.Random.Double(0, 80), 1)
        };
        _EnglandAll = new RelativeYearValues<double?>
        {
            CurrentYear = Math.Round(_faker.Random.Double(0, 80), 1)
        };
        _EstablishmentBoys = new RelativeYearValues<double?>
        {
            CurrentYear = Math.Round(_faker.Random.Double(0, 80), 1)
        };
        _LocalAuthorityBoys = new RelativeYearValues<double?>
        {
            CurrentYear = Math.Round(_faker.Random.Double(0, 80), 1),
        };
        _EnglandBoys = new RelativeYearValues<double?>
        {
            CurrentYear = Math.Round(_faker.Random.Double(0, 80), 1),
        };
        _EstablishmentGirls = new RelativeYearValues<double?>
        {
            CurrentYear = Math.Round(_faker.Random.Double(0, 80), 1)
        };
        _LocalAuthorityGirls = new RelativeYearValues<double?>
        {
            CurrentYear = Math.Round(_faker.Random.Double(0, 80), 1),
        };
        _EnglandGirls = new RelativeYearValues<double?>
        {
            CurrentYear = Math.Round(_faker.Random.Double(0, 80), 1),
        };
        _EstablishmentDisadvantaged = new RelativeYearValues<CodedDouble>
        {
            CurrentYear = new CodedDouble(Math.Round(_faker.Random.Double(0, 80), 1), string.Empty, string.Empty)
        };
        _LocalAuthorityDisadvantaged = RandomCodedYearValue();
        _EnglandDisadvantaged = RandomCodedYearValue();
        _LocalAuthorityNonDisadvantaged = RandomCodedYearValue();
        _EnglandNonDisadvantaged = RandomCodedYearValue();
        return this;
    }

    public EnglishAndMathsResultsModelBuilder WithData()
    {
        _EstablishmentAll = new RelativeYearValues<double?>
        {
            CurrentYear = Math.Round(_faker.Random.Double(0, 80), 1),
            PreviousYear = Math.Round(_faker.Random.Double(0, 80), 1),
            TwoYearsAgo = Math.Round(_faker.Random.Double(0, 80), 1)
        };
        _LocalAuthorityAll = new RelativeYearValues<double?>
        {
            CurrentYear = Math.Round(_faker.Random.Double(0, 80), 1),
            PreviousYear = Math.Round(_faker.Random.Double(0, 80), 1),
            TwoYearsAgo = Math.Round(_faker.Random.Double(0, 80), 1)
        };
        _EnglandAll = new RelativeYearValues<double?>
        {
            CurrentYear = Math.Round(_faker.Random.Double(0, 80), 1),
            PreviousYear = Math.Round(_faker.Random.Double(0, 80), 1),
            TwoYearsAgo = Math.Round(_faker.Random.Double(0, 80), 1)
        };
        _EstablishmentBoys = new RelativeYearValues<double?>
        {
            CurrentYear = Math.Round(_faker.Random.Double(0, 80), 1)
        };
        _LocalAuthorityBoys = new RelativeYearValues<double?>
        {
            CurrentYear = Math.Round(_faker.Random.Double(0, 80), 1),
        };
        _EnglandBoys = new RelativeYearValues<double?>
        {
            CurrentYear = Math.Round(_faker.Random.Double(0, 80), 1),
        };
        _EstablishmentGirls = new RelativeYearValues<double?>
        {
            CurrentYear = Math.Round(_faker.Random.Double(0, 80), 1)
        };
        _LocalAuthorityGirls = new RelativeYearValues<double?>
        {
            CurrentYear = Math.Round(_faker.Random.Double(0, 80), 1),
        };
        _EnglandGirls = new RelativeYearValues<double?>
        {
            CurrentYear = Math.Round(_faker.Random.Double(0, 80), 1),
        };
        _EstablishmentDisadvantaged = RandomCodedYearValue();
        _LocalAuthorityDisadvantaged = RandomCodedYearValue();
        _EnglandDisadvantaged = RandomCodedYearValue();
        _LocalAuthorityNonDisadvantaged = RandomCodedYearValue();
        _EnglandNonDisadvantaged = RandomCodedYearValue();
        return this;
    }

    public EnglishAndMathsResultsModel Build()
    {
        return new EnglishAndMathsResultsModel
        {
            Urn = _urn ?? _faker.Random.Int(100000, 999999).ToString(),
            SchoolName = _establishmentName ?? $"{_faker.Name.LastName()} school",
            LAName = _laName,
            EstablishmentAll = _EstablishmentAll,
            LocalAuthorityAll = _LocalAuthorityAll,
            EnglandAll = _EnglandAll,
            EstablishmentBoys = _EstablishmentBoys,
            LocalAuthorityBoys = _LocalAuthorityBoys,
            EnglandBoys = _EnglandBoys,
            EstablishmentGirls = _EstablishmentGirls,
            LocalAuthorityGirls = _LocalAuthorityGirls,
            EnglandGirls = _EnglandGirls,
            EstablishmentDisadvantaged = _EstablishmentDisadvantaged,
            LocalAuthorityDisadvantaged = _LocalAuthorityDisadvantaged,
            EnglandDisadvantaged = _EnglandDisadvantaged,
            LocalAuthorityNonDisadvantaged = _LocalAuthorityNonDisadvantaged,
            EnglandNonDisadvantaged = _EnglandNonDisadvantaged,

            IsKS2 = _isKS2,
            IsKS4 = _isKS4,
            IsKS5 = _isKS5
        };
    }

    private RelativeYearValues<CodedDouble> RandomCodedYearValue()
    {
        var value = Math.Round(_faker.Random.Double(0, 80), 1);

        return new RelativeYearValues<CodedDouble>
        {
            CurrentYear = new CodedDouble(value, string.Empty, value.ToString())
        };
    }

    private static RelativeYearValues<CodedDouble> CodedYearNoValue(string reasonCode)
    {
        // this should do something cleverer with the reason code and be somewhere everyone can use it
        // but we're not using different reasons so it can be this basic for now
        return new RelativeYearValues<CodedDouble>
        {
            CurrentYear = new CodedDouble(null, "Not applicable", reasonCode)
        };
    }
}
