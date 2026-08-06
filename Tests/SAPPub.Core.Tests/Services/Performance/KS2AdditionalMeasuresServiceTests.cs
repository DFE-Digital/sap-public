using Moq;
using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Interfaces.Repositories.Performance;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.Services.Performance;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Tests.Services.Performance;

public class KS2AdditionalMeasuresServiceTests
{
    private readonly Mock<IEstablishmentService> _establishmentService = new();
    private readonly Mock<IKS2PerformanceRepository> _ks2PerformanceRepository = new();
    private readonly KS2AdditionalMeasuresService _service;

    public KS2AdditionalMeasuresServiceTests()
    {
        _service = new KS2AdditionalMeasuresService(_establishmentService.Object, _ks2PerformanceRepository.Object);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GetAdditionalMeasures_ThrowsForInvalidUrn(string? urn)
    {
        var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            _service.GetAdditionalMeasures(urn!, CancellationToken.None));

        Assert.Equal("urn", ex.ParamName);
    }


    [Fact]
    public async Task GetAdditionalMeasures_ThrowsWhenCancellationAlreadyRequested()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            _service.GetAdditionalMeasures(It.IsAny<string>(), cts.Token));

        // Assert
        _establishmentService.Verify(a => a.GetEstablishmentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _ks2PerformanceRepository.Verify(a => a.GetEstablishmentPerformanceAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _ks2PerformanceRepository.Verify(a => a.GetLaPerformanceAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _ks2PerformanceRepository.Verify(a => a.GetEnglandPerformanceAsync(It.IsAny<CancellationToken>()), Times.Never);

    }

    [Fact]
    public async Task GetAdditionalMeasures_ReturnsAllDataCorrectly()
    {
        // Arrange
        var urn = "123456";
        var laId = "TST123";

        var expectedModel = GetKS2AdditionalMeasuresModel();

        _establishmentService
            .Setup(a => a.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentServiceModel { URN = urn, LAId = laId });

        _ks2PerformanceRepository
            .Setup(a => a.GetEstablishmentPerformanceAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS2EstablishmentPerformance { PTGPS_EXP_Est_Current_Pct_Coded = new CodedDouble(1, string.Empty, "1"), PTGPS_HIGH_Est_Current_Pct_Coded = new CodedDouble(1, string.Empty, "2") });

        _ks2PerformanceRepository
            .Setup(a => a.GetLaPerformanceAsync(laId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS2LAPerformance { PTGPS_EXP_LA_Current_Pct_Coded = new CodedDouble(1, string.Empty, "3"), PTGPS_HIGH_LA_Current_Pct_Coded = new CodedDouble(1, string.Empty, "4") });

        _ks2PerformanceRepository
            .Setup(a => a.GetEnglandPerformanceAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS2EnglandPerformance { PTGPS_EXP_Eng_Current_Pct_Coded = new CodedDouble(1, string.Empty, "5"), PTGPS_HIGH_Eng_Current_Pct_Coded = new CodedDouble(1, string.Empty, "6") });

        // Act
        var result = await _service.GetAdditionalMeasures(urn, CancellationToken.None);

        // Assert
        Assert.Equal(expectedModel.EstablishmentGrammarAtExpectedStandard, result.EstablishmentGrammarAtExpectedStandard);
        Assert.Equal(expectedModel.EstablishmentGrammarAtHigherStandard, result.EstablishmentGrammarAtHigherStandard);
        Assert.Equal(expectedModel.LAGrammarAtExpectedStandard, result.LAGrammarAtExpectedStandard);
        Assert.Equal(expectedModel.LAGrammarAtHigherStandard, result.LAGrammarAtHigherStandard);
        Assert.Equal(expectedModel.EnglandGrammarAtExpectedStandard, result.EnglandGrammarAtExpectedStandard);
        Assert.Equal(expectedModel.EnglandGrammarAtHigherStandard, result.EnglandGrammarAtHigherStandard);

    }

    private static KS2AdditionalMeasuresModel GetKS2AdditionalMeasuresModel()
    {
        return new KS2AdditionalMeasuresModel
        {
            EstablishmentGrammarAtExpectedStandard = new CodedDouble(1, string.Empty, "1"),
            EstablishmentGrammarAtHigherStandard = new CodedDouble(1, string.Empty, "2"),
            LAGrammarAtExpectedStandard = new CodedDouble(1, string.Empty, "3"),
            LAGrammarAtHigherStandard = new CodedDouble(1, string.Empty, "4"),
            EnglandGrammarAtExpectedStandard = new CodedDouble(1, string.Empty, "5"),
            EnglandGrammarAtHigherStandard = new CodedDouble(1, string.Empty, "6"),
        };
    }
}
