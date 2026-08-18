using Moq;
using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Interfaces.Repositories.Performance;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.Services.Performance;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Tests.Services.Performance;

public class KS2ScaledScoresServiceTests
{
    private readonly Mock<IEstablishmentService> _establishmentService = new();
    private readonly Mock<IKS2PerformanceRepository> _ks2PerformanceRepository = new();
    private readonly KS2ScaledScoresService _service;

    public KS2ScaledScoresServiceTests()
    {
        _service = new KS2ScaledScoresService(_establishmentService.Object, _ks2PerformanceRepository.Object);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GetScaledScoreModel_ThrowsForInvalidUrn(string? urn)
    {
        var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() => _service.GetScaledScoreModel(urn!, CancellationToken.None));
        Assert.Equal("urn", ex.ParamName);
    }


    [Fact]
    public async Task GetScaledScoreModel_ThrowsWhenCancellationAlreadyRequested()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            _service.GetScaledScoreModel(It.IsAny<string>(), cts.Token));

        // Assert
        _establishmentService.Verify(a => a.GetEstablishmentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _ks2PerformanceRepository.Verify(a => a.GetEstablishmentPerformanceAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _ks2PerformanceRepository.Verify(a => a.GetLaPerformanceAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _ks2PerformanceRepository.Verify(a => a.GetEnglandPerformanceAsync(It.IsAny<CancellationToken>()), Times.Never);

    }

    [Fact]
    public async Task GetScaledScoreModel_ReturnsAllDataCorrectly()
    {
        // Arrange
        var urn = "123456";
        var laId = "TST123";

        var expectedModel = GetKS2ScaledScoreModelModel();

        _establishmentService
            .Setup(a => a.GetEstablishmentMinimumAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentMinimumServiceModel { URN = urn, LAId = laId });

        _ks2PerformanceRepository
            .Setup(a => a.GetEstablishmentPerformanceAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS2EstablishmentPerformance 
            {
                READ_AVERAGE_Est_Current_Num_Coded = new CodedDouble(1, string.Empty, "1"),
                READ_AVERAGE_Est_Previous_Num_Coded = new CodedDouble(2, string.Empty, "2"),
                READ_AVERAGE_Est_Previous2_Num_Coded = new CodedDouble(3, string.Empty, "3"),
                MAT_AVERAGE_Est_Current_Num_Coded = new CodedDouble(4, string.Empty, "4"),
                MAT_AVERAGE_Est_Previous_Num_Coded = new CodedDouble(5, string.Empty, "5"),
                MAT_AVERAGE_Est_Previous2_Num_Coded = new CodedDouble(6, string.Empty, "6"),
            });

        _ks2PerformanceRepository
            .Setup(a => a.GetLaPerformanceAsync(laId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS2LAPerformance 
            {
                READ_AVERAGE_LA_Current_Num_Coded = new CodedDouble(7, string.Empty, "7"),
                READ_AVERAGE_LA_Previous_Num_Coded = new CodedDouble(8, string.Empty, "8"),
                READ_AVERAGE_LA_Previous2_Num_Coded = new CodedDouble(9, string.Empty, "9"),
                MAT_AVERAGE_LA_Current_Num_Coded = new CodedDouble(10, string.Empty, "10"),
                MAT_AVERAGE_LA_Previous_Num_Coded = new CodedDouble(11, string.Empty, "11"),
                MAT_AVERAGE_LA_Previous2_Num_Coded = new CodedDouble(12, string.Empty, "12")
            });

        _ks2PerformanceRepository
            .Setup(a => a.GetEnglandPerformanceAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS2EnglandPerformance 
            {
                READ_AVERAGE_Eng_Current_Num_Coded = new CodedDouble(13, string.Empty, "13"),
                READ_AVERAGE_Eng_Previous_Num_Coded = new CodedDouble(14, string.Empty, "14"),
                READ_AVERAGE_Eng_Previous2_Num_Coded = new CodedDouble(15, string.Empty, "15"),
                MAT_AVERAGE_Eng_Current_Num_Coded = new CodedDouble(16, string.Empty, "16"),
                MAT_AVERAGE_Eng_Previous_Num_Coded = new CodedDouble(17, string.Empty, "17"),
                MAT_AVERAGE_Eng_Previous2_Num_Coded = new CodedDouble(18, string.Empty, "18"),
            });

        // Act
        var result = await _service.GetScaledScoreModel(urn, CancellationToken.None);

        // Assert
        Assert.Equal(expectedModel.ReadAverageEstablishment.CurrentYear, result.ReadAverageEstablishment.CurrentYear);
        Assert.Equal(expectedModel.ReadAverageEstablishment.PreviousYear, result.ReadAverageEstablishment.PreviousYear);
        Assert.Equal(expectedModel.ReadAverageEstablishment.TwoYearsAgo, result.ReadAverageEstablishment.TwoYearsAgo);
        Assert.Equal(expectedModel.ReadAverageLA.CurrentYear, result.ReadAverageLA.CurrentYear);
        Assert.Equal(expectedModel.ReadAverageLA.PreviousYear, result.ReadAverageLA.PreviousYear);
        Assert.Equal(expectedModel.ReadAverageLA.TwoYearsAgo, result.ReadAverageLA.TwoYearsAgo); 
        Assert.Equal(expectedModel.ReadAverageEngland.CurrentYear, result.ReadAverageEngland.CurrentYear);
        Assert.Equal(expectedModel.ReadAverageEngland.PreviousYear, result.ReadAverageEngland.PreviousYear);
        Assert.Equal(expectedModel.ReadAverageEngland.TwoYearsAgo, result.ReadAverageEngland.TwoYearsAgo);

        Assert.Equal(expectedModel.MathsAverageEstablishment.CurrentYear, result.MathsAverageEstablishment.CurrentYear);
        Assert.Equal(expectedModel.MathsAverageEstablishment.PreviousYear, result.MathsAverageEstablishment.PreviousYear);
        Assert.Equal(expectedModel.MathsAverageEstablishment.TwoYearsAgo, result.MathsAverageEstablishment.TwoYearsAgo);
        Assert.Equal(expectedModel.MathsAverageLA.CurrentYear, result.MathsAverageLA.CurrentYear);
        Assert.Equal(expectedModel.MathsAverageLA.PreviousYear, result.MathsAverageLA.PreviousYear);
        Assert.Equal(expectedModel.MathsAverageLA.TwoYearsAgo, result.MathsAverageLA.TwoYearsAgo);
        Assert.Equal(expectedModel.MathsAverageEngland.CurrentYear, result.MathsAverageEngland.CurrentYear);
        Assert.Equal(expectedModel.MathsAverageEngland.PreviousYear, result.MathsAverageEngland.PreviousYear);
        Assert.Equal(expectedModel.MathsAverageEngland.TwoYearsAgo, result.MathsAverageEngland.TwoYearsAgo);
    }

    private static KS2ScaledScoreModel GetKS2ScaledScoreModelModel()
    {
        return new KS2ScaledScoreModel
        {
            ReadAverageEstablishment = new Entities.RelativeYearValues<CodedDouble>()
            {
                CurrentYear = new CodedDouble(1, string.Empty, "1"),
                PreviousYear = new CodedDouble(2, string.Empty, "2"),
                TwoYearsAgo = new CodedDouble(3, string.Empty, "3"),
            },
            ReadAverageLA = new Entities.RelativeYearValues<CodedDouble>()
            {
                CurrentYear = new CodedDouble(7, string.Empty, "7"),
                PreviousYear = new CodedDouble(8, string.Empty, "8"),
                TwoYearsAgo = new CodedDouble(9, string.Empty, "9"),
            },
            ReadAverageEngland = new Entities.RelativeYearValues<CodedDouble>()
            {
                CurrentYear = new CodedDouble(13, string.Empty, "13"),
                PreviousYear = new CodedDouble(14, string.Empty, "14"),
                TwoYearsAgo = new CodedDouble(15, string.Empty, "15"),
            },
            MathsAverageEstablishment = new Entities.RelativeYearValues<CodedDouble>()
            {
                CurrentYear = new CodedDouble(4, string.Empty, "4"),
                PreviousYear = new CodedDouble(5, string.Empty, "5"),
                TwoYearsAgo = new CodedDouble(6, string.Empty, "6"),
            },
            MathsAverageLA = new Entities.RelativeYearValues<CodedDouble>()
            {
                CurrentYear = new CodedDouble(10, string.Empty, "10"),
                PreviousYear = new CodedDouble(11, string.Empty, "11"),
                TwoYearsAgo = new CodedDouble(12, string.Empty, "12"),
            },
            MathsAverageEngland = new Entities.RelativeYearValues<CodedDouble>()
            {
                CurrentYear = new CodedDouble(16, string.Empty, "16"),
                PreviousYear = new CodedDouble(17, string.Empty, "17"),
                TwoYearsAgo = new CodedDouble(18, string.Empty, "18"),
            }
        };
    }
}
