using Moq;
using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Repositories.Performance;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.Services.Performance;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Tests.Services.Performance;

public class KS2PupilProgressTests
{
    private readonly Mock<IEstablishmentService> _establishmentService = new();
    private readonly Mock<IKS2PerformanceRepository> _ks2PerformanceRepository = new();
    private readonly KS2PupilProgressService _service;

    public KS2PupilProgressTests()
    {
        _service = new KS2PupilProgressService(_establishmentService.Object, _ks2PerformanceRepository.Object);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GetPupilProgressAsync_ThrowsForInvalidUrn(string? urn)
    {
        var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            _service.GetPupilProgressAsync(urn!, It.IsAny<AcademicYearSelection>(), CancellationToken.None));

        Assert.Equal("urn", ex.ParamName);
    }

    [Fact]
    public async Task GetPupilProgressAsync_ThrowsWhenCancellationAlreadyRequested()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            _service.GetPupilProgressAsync(It.IsAny<string>(), It.IsAny<AcademicYearSelection>(), cts.Token));

        // Assert
        _establishmentService.Verify(a => a.GetEstablishmentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _ks2PerformanceRepository.Verify(a => a.GetEstablishmentPerformanceAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _ks2PerformanceRepository.Verify(a => a.GetLaPerformanceAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);

    }

    [Fact]
    public async Task GetPupilProgressAsync_ReturnsAll_Previous2_DataCorrectly()
    {
        // Arrange
        var urn = "123456";
        var laId = "TST123";

        _establishmentService
            .Setup(a => a.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentServiceModel { URN = urn, LAId = laId });

        _ks2PerformanceRepository
            .Setup(a => a.GetEstablishmentPerformanceAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS2EstablishmentPerformance 
            {
                READPROG_Est_Previous2_Num_Coded = new CodedDouble(1, string.Empty, "1"),
                READPROG_DESCR_Est_Previous2_Num_Coded = new CodedString("2", string.Empty, "2"),
                READPROG_UPPER_Est_Previous2_Num_Coded = new CodedDouble(3, string.Empty, "3"),
                READPROG_LOWER_Est_Previous2_Num_Coded = new CodedDouble(4, string.Empty, "4"),
                WRITPROG_Est_Previous2_Num_Coded = new CodedDouble(5, string.Empty, "5"),
                WRITPROG_DESCR_Est_Previous2_Num_Coded = new CodedString("6", string.Empty, "6"),
                WRITPROG_UPPER_Est_Previous2_Num_Coded = new CodedDouble(7, string.Empty, "7"),
                WRITPROG_LOWER_Est_Previous2_Num_Coded = new CodedDouble(8, string.Empty, "8"),
                MATPROG_Est_Previous2_Num_Coded = new CodedDouble(9, string.Empty, "9"),
                MATPROG_DESCR_Est_Previous2_Num_Coded = new CodedString("10", string.Empty, "10"),
                MATPROG_UPPER_Est_Previous2_Num_Coded = new CodedDouble(11, string.Empty, "11"),
                MATPROG_LOWER_Est_Previous2_Num_Coded = new CodedDouble(12, string.Empty, "12")
            });

        _ks2PerformanceRepository
            .Setup(a => a.GetLaPerformanceAsync(laId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS2LAPerformance 
            {
                READPROG_LA_Previous2_Num_Coded = new CodedDouble(30, string.Empty, "30"),
                WRITPROG_LA_Previous2_Num_Coded = new CodedDouble(31, string.Empty, "31"),
                MATPROG_LA_Previous2_Num_Coded = new CodedDouble(32, string.Empty, "32")
            });

        // Act
        var result = await _service.GetPupilProgressAsync(urn, AcademicYearSelection.Previous2, CancellationToken.None);

        // Assert
        Assert.Equal(urn, result.Urn);
        Assert.Equal(1, result.EstablishmentReadingScore!.Value);
        Assert.Equal("2", result.EstablishmentReadingDescription!.Value);
        Assert.Equal(3, result.EstablishmentReadingConfidenceUpper!.Value);
        Assert.Equal(4, result.EstablishmentReadingConfidenceLower!.Value);
        Assert.Equal(5, result.EstablishmentWritingScore!.Value);
        Assert.Equal("6", result.EstablishmentWritingDescription!.Value);
        Assert.Equal(7, result.EstablishmentWritingConfidenceUpper!.Value);
        Assert.Equal(8, result.EstablishmentWritingConfidenceLower!.Value);
        Assert.Equal(9, result.EstablishmentMathsScore!.Value);
        Assert.Equal("10", result.EstablishmentMathsDescription!.Value);
        Assert.Equal(11, result.EstablishmentMathsConfidenceUpper!.Value);
        Assert.Equal(12, result.EstablishmentMathsConfidenceLower!.Value);

        Assert.Equal(30, result.LaReadingScore!.Value);
        Assert.Equal(31, result.LaWritingScore!.Value);
        Assert.Equal(32, result.LaMathsScore!.Value);

        _establishmentService
            .Verify(a => a.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()), Times.Once);

        _ks2PerformanceRepository
            .Verify(a => a.GetEstablishmentPerformanceAsync(urn, It.IsAny<CancellationToken>()), Times.Once);

        _ks2PerformanceRepository
            .Verify(a => a.GetLaPerformanceAsync(laId, It.IsAny<CancellationToken>()), Times.Once);
    }
}
