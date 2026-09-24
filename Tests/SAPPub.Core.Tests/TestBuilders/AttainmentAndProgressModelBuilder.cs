using Bogus;
using SAPPub.Core.Entities;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Tests.TestBuilders;

public class AttainmentAndProgressModelBuilder
{
    private Faker _faker = new();
    private bool _isKS2;
    private bool _isKS4 = true;
    private bool _isKS5;

    private RelativeYearValues<CodedDouble>? _establishmentProgress8Score;
    private RelativeYearValues<CodedDouble>? _establishmentProgress8CILower;
    private RelativeYearValues<CodedDouble>? _establishmentProgress8CIUpper;
    private RelativeYearValues<string?>? _establishmentProgress8Banding;
    private RelativeYearValues<CodedString>? _progress8BandingContextDescription;
    private RelativeYearValues<CodedDouble>? _localAuthorityProgress8Score;
    private RelativeYearValues<CodedDouble>? _establishmentAttainment8Score;
    private RelativeYearValues<CodedDouble>? _establishmentAttainment8DisadvantagedScore;
    private RelativeYearValues<CodedDouble>? _localAuthorityAttainment8Score;
    private RelativeYearValues<CodedDouble>? _localAuthorityAttainment8DisadvantagedScore;
    private RelativeYearValues<CodedDouble>? _englandAttainment8Score;
    private RelativeYearValues<CodedDouble>? _englandAttainment8DisadvantagedScore;
    private RelativeYearValues<CodedDouble>? _establishmentProgress8TotalPupils;

    private CodedDouble? _localAuthorityAttainment8NonDisadvantagedScore;
    private CodedDouble? _englandAttainment8NonDisadvantagedScore;
    private CodedDouble? _establishmentAttainment8GirlScore;
    private CodedDouble? _establishmentAttainment8BoyScore;
    private CodedDouble? _establishmentAttainment8EALScore;
    private CodedDouble? _establishmentAttainment8NonMobileScore;

    public AttainmentAndProgressModelBuilder WithIsKS2(bool isKS)
    {
        _isKS2 = isKS;
        return this;
    }
    public AttainmentAndProgressModelBuilder WithIsKS4(bool isKS)
    {
        _isKS4 = isKS;
        return this;
    }
    public AttainmentAndProgressModelBuilder WithIsKS5(bool isKS)
    {
        _isKS5 = isKS;
        return this;
    }

    public AttainmentAndProgressModelBuilder WithEstablishmentAttainment8Score(double? score)
    {
        _establishmentAttainment8Score = new RelativeYearValues<CodedDouble>
        {
            CurrentYear = CodedDoubleFactory.Create(score),
            PreviousYear = CodedDoubleFactory.Create(score),
            TwoYearsAgo = CodedDoubleFactory.Create(score)
        };
        return this;
    }

    public AttainmentAndProgressModelBuilder WithLocalAuthorityAttainment8Score(double? score)
    {
        _localAuthorityAttainment8Score = new RelativeYearValues<CodedDouble>
        {
            CurrentYear = CodedDoubleFactory.Create(score),
            PreviousYear = CodedDoubleFactory.Create(score),
            TwoYearsAgo = CodedDoubleFactory.Create(score)
        };
        return this;
    }

    public AttainmentAndProgressModelBuilder WithNationalAttainment8Score(double? score)
    {
        _englandAttainment8Score = new RelativeYearValues<CodedDouble>
        {
            CurrentYear = CodedDoubleFactory.Create(score),
            PreviousYear = CodedDoubleFactory.Create(score),
            TwoYearsAgo = CodedDoubleFactory.Create(score)
        };
        return this;
    }

    public AttainmentAndProgressModelBuilder WithEstablishmentProgress8Banding(string? banding)
    {
        _establishmentProgress8Banding = new RelativeYearValues<string?>
        {
            CurrentYear = banding,
            PreviousYear = banding,
            TwoYearsAgo = banding
        };
        return this;
    }

    public AttainmentAndProgressModelBuilder WithProgress8BandingContextDescription(CodedString description)
    {
        _progress8BandingContextDescription = new RelativeYearValues<CodedString>
        {
            CurrentYear = description,
            PreviousYear = description,
            TwoYearsAgo = description
        };
        return this;
    }

    public AttainmentAndProgressModelBuilder WithEstablishmentProgress8Data()
    {
        _establishmentProgress8Score = new RelativeYearValues<CodedDouble>
        {
            CurrentYear = CodedDoubleFactory.Create(), // there's no data for the current year
            PreviousYear = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(-1, 1), 2)),
            TwoYearsAgo = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(-1, 1), 2))
        };
        _establishmentProgress8Banding = new RelativeYearValues<string?>
        {
            CurrentYear = null, // there's no data for the current year
            PreviousYear = _faker.PickRandom(new[] { "Well above average", "Above average", "Average", "Below average", "Well below average" }),
            TwoYearsAgo = _faker.PickRandom(new[] { "Well above average", "Above average", "Average", "Below average", "Well below average" })
        };
        _establishmentProgress8CILower = new RelativeYearValues<CodedDouble>
        {
            CurrentYear = CodedDoubleFactory.Create(), // there's no data for the current year
            PreviousYear = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(-1, 1), 2)),
            TwoYearsAgo = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(-1, 1), 2))
        };
        _establishmentProgress8CIUpper = new RelativeYearValues<CodedDouble>
        {
            CurrentYear = CodedDoubleFactory.Create(), // there's no data for the current year
            PreviousYear = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(-1, 1), 2)),
            TwoYearsAgo = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(-1, 1), 2))
        };
        _establishmentProgress8TotalPupils = new RelativeYearValues<CodedDouble>
        {
            CurrentYear = CodedDoubleFactory.Create(), // there's no data for the current year
            PreviousYear = CodedDoubleFactory.Create(_faker.Random.Int(20, 200)),
            TwoYearsAgo = CodedDoubleFactory.Create(_faker.Random.Int(20, 200))
        };
        return this;
    }

    public AttainmentAndProgressModelBuilder WithLaProgressData()
    {
        _localAuthorityProgress8Score = new RelativeYearValues<CodedDouble>
        {
            CurrentYear = CodedDoubleFactory.Create(), // there's no data for the current year
            PreviousYear = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(20, 80), 1)),
            TwoYearsAgo = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(20, 80), 2))
        };
        return this;
    }

    public AttainmentAndProgressModelBuilder WithAttainment8Data()
    {
        _establishmentAttainment8Score = new RelativeYearValues<CodedDouble>
        {
            CurrentYear = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(20, 80), 1)),
            PreviousYear = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(20, 80), 1)),
            TwoYearsAgo = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(20, 80), 2))
        };
        _establishmentAttainment8DisadvantagedScore = new RelativeYearValues<CodedDouble>
        {
            CurrentYear = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(20, 80), 1)),
            PreviousYear = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(20, 80), 1)),
            TwoYearsAgo = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(20, 80), 2))
        };
        _localAuthorityAttainment8Score = new RelativeYearValues<CodedDouble>
        {
            CurrentYear = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(20, 80), 1)),
            PreviousYear = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(20, 80), 1)),
            TwoYearsAgo = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(20, 80), 2))
        };
        _localAuthorityAttainment8DisadvantagedScore = new RelativeYearValues<CodedDouble>
        {
            CurrentYear = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(20, 80), 1)),
            PreviousYear = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(20, 80), 1)),
            TwoYearsAgo = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(20, 80), 2))
        };
        _englandAttainment8Score = new RelativeYearValues<CodedDouble>
        {
            CurrentYear = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(20, 80), 1)),
            PreviousYear = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(20, 80), 1)),
            TwoYearsAgo = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(20, 80), 2))
        };
        _englandAttainment8DisadvantagedScore = new RelativeYearValues<CodedDouble>
        {
            CurrentYear = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(20, 80), 1)),
            PreviousYear = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(20, 80), 1)),
            TwoYearsAgo = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(20, 80), 2))
        };
        return this;
    }

    public AttainmentAndProgressModelBuilder WithAttainmentNonDisadvantaged8Data()
    {
        _localAuthorityAttainment8NonDisadvantagedScore = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(20, 80), 1));
        _englandAttainment8NonDisadvantagedScore = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(20, 80), 1));
        return this;
    }

    public AttainmentAndProgressModelBuilder WithAttainment8PupilCharacteristics()
    {
        _establishmentAttainment8GirlScore = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(20, 80), 1));
        _establishmentAttainment8BoyScore = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(20, 80), 1));
        _establishmentAttainment8EALScore = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(20, 80), 1));
        _establishmentAttainment8NonMobileScore = CodedDoubleFactory.Create(Math.Round(_faker.Random.Double(20, 80), 1));
        return this;
    }

    public AttainmentAndProgressModel Build()
    {
        return new AttainmentAndProgressModel
        {
            Urn = "123456",
            IsKS2 = _isKS2,
            IsKS4 = _isKS4,
            IsKS5 = _isKS5,
            SchoolName = "Test School",
            EstablishmentAttainment8Score = _establishmentAttainment8Score ?? CreateEmptyRelativeYearValues(),
            EstablishmentAttainment8DisadvantagedScore = _establishmentAttainment8DisadvantagedScore ?? CreateEmptyRelativeYearValues(),
            LocalAuthorityAttainment8Score = _localAuthorityAttainment8Score ?? CreateEmptyRelativeYearValues(),
            LocalAuthorityAttainment8DisadvantagedScore = _localAuthorityAttainment8DisadvantagedScore ?? CreateEmptyRelativeYearValues(),
            LocalAuthorityAttainment8NonDisadvantagedScore = _localAuthorityAttainment8NonDisadvantagedScore.HasValue ? _localAuthorityAttainment8NonDisadvantagedScore.Value : CodedDoubleFactory.Create(),
            EnglandAttainment8Score = _englandAttainment8Score ?? CreateEmptyRelativeYearValues(),
            EnglandAttainment8DisadvantagedScore = _englandAttainment8DisadvantagedScore ?? CreateEmptyRelativeYearValues(),
            EnglandAttainment8NonDisadvantagedScore = _englandAttainment8NonDisadvantagedScore.HasValue ? _englandAttainment8NonDisadvantagedScore.Value : CodedDoubleFactory.Create(),
            EstablishmentProgress8Banding = _establishmentProgress8Banding ?? new RelativeYearValues<string?> { CurrentYear = null, PreviousYear = null, TwoYearsAgo = null },
            Progress8BandingContextDescription = _progress8BandingContextDescription ?? new RelativeYearValues<CodedString> { CurrentYear = CodedString.Empty },
            EstablishmentProgress8Score = _establishmentProgress8Score ?? CreateEmptyRelativeYearValues(),
            EstablishmentProgress8CILower = _establishmentProgress8CILower ?? CreateEmptyRelativeYearValues(),
            EstablishmentProgress8CIUpper = _establishmentProgress8CIUpper ?? CreateEmptyRelativeYearValues(),
            EstablishmentProgress8TotalPupils = _establishmentProgress8TotalPupils ?? CreateEmptyRelativeYearValues(),
            EstablishmentTotalPupils = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = CodedDoubleFactory.Create(_faker.Random.Double(20,200)),
                PreviousYear = CodedDoubleFactory.Create(_faker.Random.Double(20,200)),
                TwoYearsAgo = CodedDoubleFactory.Create(_faker.Random.Double(20,200))
            },
            LocalAuthorityProgress8Score = _localAuthorityProgress8Score ?? CreateEmptyRelativeYearValues(),
            EstablishmentAttainment8GirlsScore = _establishmentAttainment8GirlScore ?? CodedDouble.Empty,
            EstablishmentAttainment8BoysScore = _establishmentAttainment8BoyScore ?? CodedDouble.Empty,
            EstablishmentAttainment8EALScore = _establishmentAttainment8EALScore ?? CodedDouble.Empty,
            EstablishmentAttainment8NonMobileScore = _establishmentAttainment8NonMobileScore ?? CodedDouble.Empty
        };
    }

    private static RelativeYearValues<CodedDouble> CreateEmptyRelativeYearValues()
    {
        return new RelativeYearValues<CodedDouble>
        {
            CurrentYear = CodedDoubleFactory.Create(),
            PreviousYear = CodedDoubleFactory.Create(),
            TwoYearsAgo = CodedDoubleFactory.Create()
         };
    }
}
