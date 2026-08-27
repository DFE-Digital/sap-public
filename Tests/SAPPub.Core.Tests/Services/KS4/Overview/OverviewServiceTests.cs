using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Entities.Destinations;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Interfaces.Repositories.Overview;
using SAPPub.Core.Services.Overview;
using SAPPub.Core.ValueObjects;
using OverviewEntity = SAPPub.Core.Entities.Overview.Overview;

namespace SAPPub.Core.Tests.Services.Overview;

public class OverviewServiceTests
{
    private readonly Mock<IOverviewRepository> _repository = new();
    private readonly OverviewService _sut;

    public OverviewServiceTests()
    {
        _sut = new OverviewService(_repository.Object);
    }

    [Fact]
    public async Task GetOverviewAsync_ReturnsNull_WhenRepositoryReturnsNull()
    {
        _repository
            .Setup(r => r.GetOverviewAsync("123456", It.IsAny<CancellationToken>()))
            .ReturnsAsync((OverviewEntity?)null);

        var result = await _sut.GetOverviewAsync("123456", CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetOverviewAsync_ReturnsNull_WhenEstablishmentIsNull()
    {
        _repository
            .Setup(r => r.GetOverviewAsync("123456", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new OverviewEntity());

        var result = await _sut.GetOverviewAsync("123456", CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetOverviewAsync_MapsAllOverviewFields()
    {
        var overview = CreateCompleteOverview();

        _repository
            .Setup(r => r.GetOverviewAsync("123456", It.IsAny<CancellationToken>()))
            .ReturnsAsync(overview);

        var result = await _sut.GetOverviewAsync("123456", CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("123456", result.Urn);
        Assert.Equal("Test School", result.SchoolName);
        Assert.Equal("Secondary", result.PhaseOfEducation);
        Assert.Equal("11", result.AgeRangeLow);
        Assert.Equal("16", result.AgeRangeHigh);
        Assert.Equal("1234", result.NumberOfPupils);
        Assert.Equal("ASD - Autistic Spectrum Disorder", result.SenProvision);
        Assert.Equal("0114 123 4567", result.Phone);
        Assert.Equal("https://school.example", result.Website);
        Assert.False(result.IsKS2);
        Assert.True(result.IsKS4);
        Assert.False(result.IsKS5);

        AssertCodedDouble(result.Attainment8, 52.1);
        AssertCodedDouble(result.EnglishAndMathsGrade5Establishment, 61.2);
        AssertCodedDouble(result.EnglishAndMathsGrade5LA, 58.3);
        AssertCodedDouble(result.EnglishAndMathsGrade5England, 59.4);
        AssertCodedDouble(result.MoreThanOneForeignLanguage, 42.5);
        AssertCodedDouble(result.DestinationsEstablishment, 91.1);
        AssertCodedDouble(result.DestinationsLA, 89.2);
        AssertCodedDouble(result.DestinationsEngland, 90.3);
        AssertCodedDouble(result.ReadingWritingMathsExpectedEstablishment, 67.1);
        AssertCodedDouble(result.ReadingWritingMathsExpectedLA, 65.2);
        AssertCodedDouble(result.ReadingWritingMathsExpectedEngland, 64.3);
        AssertCodedDouble(result.ReadingWritingMathsHigherEstablishment, 12.1);
        AssertCodedDouble(result.ReadingWritingMathsHigherLA, 11.2);
        AssertCodedDouble(result.ReadingWritingMathsHigherEngland, 10.3);
    }

    [Fact]
    public async Task GetOverviewAsync_MapsMissingOptionalResultSetsToNull()
    {
        var overview = new OverviewEntity
        {
            Establishment = new Establishment
            {
                URN = "123456",
                EstablishmentName = "Test School"
            }
        };

        _repository
            .Setup(r => r.GetOverviewAsync("123456", It.IsAny<CancellationToken>()))
            .ReturnsAsync(overview);

        var result = await _sut.GetOverviewAsync("123456", CancellationToken.None);

        Assert.NotNull(result);
        Assert.Null(result.Attainment8);
        Assert.Null(result.EnglishAndMathsGrade5Establishment);
        Assert.Null(result.EnglishAndMathsGrade5LA);
        Assert.Null(result.EnglishAndMathsGrade5England);
        Assert.Null(result.MoreThanOneForeignLanguage);
        Assert.Null(result.DestinationsEstablishment);
        Assert.Null(result.DestinationsLA);
        Assert.Null(result.DestinationsEngland);
        Assert.Null(result.ReadingWritingMathsExpectedEstablishment);
        Assert.Null(result.ReadingWritingMathsExpectedLA);
        Assert.Null(result.ReadingWritingMathsExpectedEngland);
        Assert.Null(result.ReadingWritingMathsHigherEstablishment);
        Assert.Null(result.ReadingWritingMathsHigherLA);
        Assert.Null(result.ReadingWritingMathsHigherEngland);
    }

    [Fact]
    public async Task GetOverviewAsync_ForwardsUrnAndCancellationToken()
    {
        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        _repository
            .Setup(r => r.GetOverviewAsync("654321", token))
            .ReturnsAsync(new OverviewEntity
            {
                Establishment = new Establishment
                {
                    URN = "654321",
                    EstablishmentName = "Another School"
                }
            });

        await _sut.GetOverviewAsync("654321", token);

        _repository.Verify(r => r.GetOverviewAsync("654321", token), Times.Once);
    }

    [Fact]
    public async Task GetOverviewAsync_ReturnsMappedOverview()
    {
        const string urn = "123456";

        var overview = new Core.Entities.Overview.Overview
        {
            Establishment = new Establishment
            {
                URN = urn,
                EstablishmentName = "Test School",
                LAName = "Test LA",
                PhaseOfEducationName = "Secondary",
                AgeRangeLow = "11",
                AgeRangeHigh = "18",
                TotalPupils = "1000",
                SenTypes = "Autism",
                TelephoneNum = "01234567890",
                Website = "https://example.com",
                IsKS2 = false,
                IsKS4 = true,
                IsKS5 = true
            },

            KS4Performance = new EstablishmentPerformance
            {
                Attainment8_Tot_Est_Current_Num_Coded =
                    new CodedDouble(52.3, null, null),

                EngMaths59_Tot_Est_Current_Pct_Coded =
                    new CodedDouble(61.2, null, null),

                More1FL_Tot_Est_Current_Pct_Coded =
                    new CodedDouble(25.4, null, null)
            },

            KS4LAPerformance = new LAPerformance
            {
                EngMaths59_Tot_LA_Current_Pct_Coded =
                    new CodedDouble(55.1, null, null)
            },

            KS4EnglandPerformance = new EnglandPerformance
            {
                EngMaths59_Tot_Eng_Current_Pct_Coded =
                    new CodedDouble(57.8, null, null)
            },

            Destinations = new KS4EstablishmentDestinations
            {
                AllDest_Tot_Est_Current_Pct_Coded =
                    new CodedDouble(91.2, null, null)
            },

            LADestinations = new KS4LADestinations
            {
                AllDest_Tot_LA_Current_Pct_Coded =
                    new CodedDouble(89.3, null, null)
            },

            EnglandDestinations = new KS4EnglandDestinations
            {
                AllDest_Tot_Eng_Current_Pct_Coded =
                    new CodedDouble(90.1, null, null)
            },

            KS2Performance = new KS2EstablishmentPerformance
            {
                PTRWM_EXP_Est_Current_Pct_Coded =
                    new CodedDouble(72.1, null, null),

                PTRWM_HIGH_Est_Current_Pct_Coded =
                    new CodedDouble(15.2, null, null)
            },

            KS2LAPerformance = new KS2LAPerformance
            {
                PTRWM_EXP_LA_Current_Pct_Coded =
                    new CodedDouble(68.4, null, null),

                PTRWM_HIGH_LA_Current_Pct_Coded =
                    new CodedDouble(12.7, null, null)
            },

            KS2EnglandPerformance = new KS2EnglandPerformance
            {
                PTRWM_EXP_Eng_Current_Pct_Coded =
                    new CodedDouble(70.0, null, null),

                PTRWM_HIGH_Eng_Current_Pct_Coded =
                    new CodedDouble(13.5, null, null)
            }
        };

        _repository
            .Setup(x => x.GetOverviewAsync(
                urn,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(overview);

        var result = await _sut.GetOverviewAsync(
            urn,
            CancellationToken.None);

        Assert.NotNull(result);

        Assert.Equal(urn, result.Urn);
        Assert.Equal("Test School", result.SchoolName);
        Assert.Equal("Secondary", result.PhaseOfEducation);
        Assert.Equal("11", result.AgeRangeLow);
        Assert.Equal("18", result.AgeRangeHigh);
        Assert.Equal("1000", result.NumberOfPupils);
        Assert.Equal("Autism", result.SenProvision);
        Assert.Equal("01234567890", result.Phone);
        Assert.Equal("https://example.com", result.Website);

        Assert.Equal(
            52.3,
            result.Attainment8?.Value);

        Assert.Equal(
            61.2,
            result.EnglishAndMathsGrade5Establishment?.Value);

        Assert.Equal(
            55.1,
            result.EnglishAndMathsGrade5LA?.Value);

        Assert.Equal(
            57.8,
            result.EnglishAndMathsGrade5England?.Value);

        Assert.Equal(
            25.4,
            result.MoreThanOneForeignLanguage?.Value);

        Assert.Equal(
            91.2,
            result.DestinationsEstablishment?.Value);

        Assert.Equal(
            89.3,
            result.DestinationsLA?.Value);

        Assert.Equal(
            90.1,
            result.DestinationsEngland?.Value);

        Assert.Equal(
            72.1,
            result.ReadingWritingMathsExpectedEstablishment?.Value);

        Assert.Equal(
            68.4,
            result.ReadingWritingMathsExpectedLA?.Value);

        Assert.Equal(
            70.0,
            result.ReadingWritingMathsExpectedEngland?.Value);

        Assert.Equal(
            15.2,
            result.ReadingWritingMathsHigherEstablishment?.Value);

        Assert.Equal(
            12.7,
            result.ReadingWritingMathsHigherLA?.Value);

        Assert.Equal(
            13.5,
            result.ReadingWritingMathsHigherEngland?.Value);

        _repository.Verify(
            x => x.GetOverviewAsync(
                urn,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static OverviewEntity CreateCompleteOverview()
    {
        return new OverviewEntity
        {
            Establishment = new Establishment
            {
                URN = "123456",
                EstablishmentName = "Test School",
                PhaseOfEducationName = "Secondary",
                AgeRangeLow = "11",
                AgeRangeHigh = "16",
                TotalPupils = "1234",
                SenTypes = "ASD - Autistic Spectrum Disorder",
                TelephoneNum = "0114 123 4567",
                Website = "https://school.example",
                IsKS2 = false,
                IsKS4 = true,
                IsKS5 = false
            },
            KS4Performance = new EstablishmentPerformance
            {
                Attainment8_Tot_Est_Current_Num_Coded = Coded(52.1),
                EngMaths59_Tot_Est_Current_Pct_Coded = Coded(61.2),
                More1FL_Tot_Est_Current_Pct_Coded = Coded(42.5)
            },
            KS4LAPerformance = new LAPerformance
            {
                EngMaths59_Tot_LA_Current_Pct_Coded = Coded(58.3)
            },
            KS4EnglandPerformance = new EnglandPerformance
            {
                EngMaths59_Tot_Eng_Current_Pct_Coded = Coded(59.4)
            },
            Destinations = new KS4EstablishmentDestinations
            {
                AllDest_Tot_Est_Current_Pct_Coded = Coded(91.1)
            },
            LADestinations = new KS4LADestinations
            {
                AllDest_Tot_LA_Current_Pct_Coded = Coded(89.2)
            },
            EnglandDestinations = new KS4EnglandDestinations
            {
                AllDest_Tot_Eng_Current_Pct_Coded = Coded(90.3)
            },
            KS2Performance = new KS2EstablishmentPerformance
            {
                PTRWM_EXP_Est_Current_Pct_Coded = Coded(67.1),
                PTRWM_HIGH_Est_Current_Pct_Coded = Coded(12.1)
            },
            KS2LAPerformance = new KS2LAPerformance
            {
                PTRWM_EXP_LA_Current_Pct_Coded = Coded(65.2),
                PTRWM_HIGH_LA_Current_Pct_Coded = Coded(11.2)
            },
            KS2EnglandPerformance = new KS2EnglandPerformance
            {
                PTRWM_EXP_Eng_Current_Pct_Coded = Coded(64.3),
                PTRWM_HIGH_Eng_Current_Pct_Coded = Coded(10.3)
            }
        };
    }

    private static CodedDouble Coded(double value) =>
        new(value, string.Empty, value.ToString(System.Globalization.CultureInfo.InvariantCulture));

    private static void AssertCodedDouble(CodedDouble? actual, double expected)
    {
        Assert.True(actual.HasValue);
        Assert.Equal(expected, actual.Value.Value);
    }
}
