using Moq;
using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.ValueObjects;
using SAPPub.Infrastructure.Repositories.Performance;

namespace SAPPub.Infrastructure.Tests.Repositories.Performance;

public class KS5PerformanceRepositoryTests
{
    private readonly Mock<IGenericRepository<KS5EstablishmentPerformance>> _mockEstablishmentKs5PerformanceRepo;
    private readonly Mock<IGenericRepository<KS5EnglandPerformance>> _mockEnglandKs5PerformanceRepo;    
    private readonly Mock<IGenericRepository<KS5LAPerformance>> _mockLAKs5PerformanceRepo;
    private readonly KS5PerformanceRepository _sut;

    public KS5PerformanceRepositoryTests()
    {
        _mockEstablishmentKs5PerformanceRepo = new Mock<IGenericRepository<KS5EstablishmentPerformance>>();
        _mockEnglandKs5PerformanceRepo = new Mock<IGenericRepository<KS5EnglandPerformance>>();
        _mockLAKs5PerformanceRepo = new Mock<IGenericRepository<KS5LAPerformance>>();
        _sut = new KS5PerformanceRepository(
            _mockEstablishmentKs5PerformanceRepo.Object,
            _mockEnglandKs5PerformanceRepo.Object,            
            _mockLAKs5PerformanceRepo.Object);
    }

    [Fact]
    public async Task GetEstablishmentKs5PerformanceAsync_ReturnsNewEstablishmentPerformanceWhenUrnDoesNotExist()
    {
        // Arrange
        var urn = "999";

        _mockEstablishmentKs5PerformanceRepo
            .Setup(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync((KS5EstablishmentPerformance?)null);

        // Act
        var result = await _sut.GetEstablishmentPerformanceAsync(urn, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(string.Empty, result.Id);

        _mockEstablishmentKs5PerformanceRepo.Verify(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetEstablishmentKs5PerformanceAsync_ReturnsCorrectItemWhenUrnExists()
    {
        // Arrange
        var urn = "123654";
        var expected = new KS5EstablishmentPerformance 
        {
            Id = urn,
            TALLPUP_ALEV_1618_Est_Current_Num_Coded = new CodedDouble(150, string.Empty, string.Empty),
            VA_INS_ALEV_Est_Current_Num_Coded = new CodedDouble(50.5, string.Empty, string.Empty),
            PROGRESS_BAND_ALEV_Est_Current = new CodedString("Average", string.Empty, string.Empty),
            UCI_INS_ALEV_Est_Current_Num_Coded = new CodedDouble(-0.5, string.Empty, string.Empty),
            LCI_INS_ALEV_Est_Current_Num_Coded = new CodedDouble(-0.7, string.Empty, string.Empty),
            TALLPPE_ALEV_1618_Est_Current_Num_Coded = new CodedDouble(45.15, string.Empty, string.Empty),
            TALLPPEGRD_ALEV_1618_Est_Current = new CodedString("B", string.Empty, string.Empty),

            TALLPUP_ACAD_1618_Est_Current_Num_Coded = new CodedDouble(145, string.Empty, string.Empty),
            VA_INS_ACAD_Est_Current_Num_Coded = new CodedDouble(65.33, string.Empty, string.Empty),
            PROGRESS_BAND_ACAD_Est_Current = new CodedString("Average", string.Empty, string.Empty),
            UCI_INS_ACAD_Est_Current_Num_Coded = new CodedDouble(1.5, string.Empty, string.Empty),
            LCI_INS_ACAD_Est_Current_Num_Coded = new CodedDouble(0.2, string.Empty, string.Empty),
            TALLPPE_ACAD_1618_Est_Current_Num_Coded = new CodedDouble(39.15, string.Empty, string.Empty),
            TALLPPEGRD_ACAD_1618_Est_Current = new CodedString("A", string.Empty, string.Empty)
        };

        _mockEstablishmentKs5PerformanceRepo
            .Setup(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var result = await _sut.GetEstablishmentPerformanceAsync(urn, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected.Id, result.Id);
        Assert.Equal(expected.TALLPUP_ALEV_1618_Est_Current_Num_Coded, result.TALLPUP_ALEV_1618_Est_Current_Num_Coded);
        Assert.Equal(expected.VA_INS_ALEV_Est_Current_Num_Coded, result.VA_INS_ALEV_Est_Current_Num_Coded);
        Assert.Equal(expected.PROGRESS_BAND_ALEV_Est_Current, result.PROGRESS_BAND_ALEV_Est_Current);
        Assert.Equal(expected.UCI_INS_ALEV_Est_Current_Num_Coded, result.UCI_INS_ALEV_Est_Current_Num_Coded);
        Assert.Equal(expected.LCI_INS_ALEV_Est_Current_Num_Coded, result.LCI_INS_ALEV_Est_Current_Num_Coded);
        Assert.Equal(expected.TALLPPE_ALEV_1618_Est_Current_Num_Coded, result.TALLPPE_ALEV_1618_Est_Current_Num_Coded);
        Assert.Equal(expected.TALLPPEGRD_ALEV_1618_Est_Current, result.TALLPPEGRD_ALEV_1618_Est_Current);

        Assert.Equal(expected.TALLPUP_ACAD_1618_Est_Current_Num_Coded, result.TALLPUP_ACAD_1618_Est_Current_Num_Coded);
        Assert.Equal(expected.VA_INS_ACAD_Est_Current_Num_Coded, result.VA_INS_ACAD_Est_Current_Num_Coded);
        Assert.Equal(expected.PROGRESS_BAND_ACAD_Est_Current, result.PROGRESS_BAND_ACAD_Est_Current);
        Assert.Equal(expected.UCI_INS_ACAD_Est_Current_Num_Coded, result.UCI_INS_ACAD_Est_Current_Num_Coded);
        Assert.Equal(expected.LCI_INS_ACAD_Est_Current_Num_Coded, result.LCI_INS_ACAD_Est_Current_Num_Coded);
        Assert.Equal(expected.TALLPPE_ACAD_1618_Est_Current_Num_Coded, result.TALLPPE_ACAD_1618_Est_Current_Num_Coded);
        Assert.Equal(expected.TALLPPEGRD_ACAD_1618_Est_Current, result.TALLPPEGRD_ACAD_1618_Est_Current);

        _mockEstablishmentKs5PerformanceRepo.Verify(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetEnglandKs5PerformanceAsync_ReturnsNewEnglandPerformance_WhenReturnsNull()
    {
        // Arrange
        _mockEnglandKs5PerformanceRepo
            .Setup(r => r.ReadSingleAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((KS5EnglandPerformance?)null);

        // Act
        var result = await _sut.GetEnglandPerformanceAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        var expected = new KS5EnglandPerformance();
        Assert.Equal(expected.VA_INS_ALEV_Eng_Current_Num_Coded, result.VA_INS_ALEV_Eng_Current_Num_Coded);
        Assert.Equal(expected.TALLPPE_ALEV_1618_Eng_Current_Num_Coded, result.TALLPPE_ALEV_1618_Eng_Current_Num_Coded);
        Assert.Equal(expected.TALLPPEGRD_ALEV_1618_Eng_Current, result.TALLPPEGRD_ALEV_1618_Eng_Current);

        Assert.Equal(expected.VA_INS_ACAD_Eng_Current_Num_Coded, result.VA_INS_ACAD_Eng_Current_Num_Coded);
        Assert.Equal(expected.TALLPPE_ACAD_1618_Eng_Current_Num_Coded, result.TALLPPE_ACAD_1618_Eng_Current_Num_Coded);
        Assert.Equal(expected.TALLPPEGRD_ACAD_1618_Eng_Current, result.TALLPPEGRD_ACAD_1618_Eng_Current);

        _mockEnglandKs5PerformanceRepo.Verify(
            r => r.ReadSingleAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetEnglandPerformanceAsync_ReturnsItem()
    {
        // Arrange
        var expected = new KS5EnglandPerformance
        {
            VA_INS_ALEV_Eng_Current_Num_Coded = new CodedDouble(75.55, string.Empty, string.Empty),
            TALLPPE_ALEV_1618_Eng_Current_Num_Coded = new CodedDouble(85, string.Empty, string.Empty),
            TALLPPEGRD_ALEV_1618_Eng_Current = new CodedString("A", string.Empty, string.Empty),

            VA_INS_ACAD_Eng_Current_Num_Coded = new CodedDouble(53.15, string.Empty, string.Empty),
            TALLPPE_ACAD_1618_Eng_Current_Num_Coded = new CodedDouble(50, string.Empty, string.Empty),
            TALLPPEGRD_ACAD_1618_Eng_Current = new CodedString("B", string.Empty, string.Empty)
        };

        _mockEnglandKs5PerformanceRepo
            .Setup(r => r.ReadSingleAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var result = await _sut.GetEnglandPerformanceAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected.VA_INS_ALEV_Eng_Current_Num_Coded, result.VA_INS_ALEV_Eng_Current_Num_Coded);
        Assert.Equal(expected.TALLPPE_ALEV_1618_Eng_Current_Num_Coded, result.TALLPPE_ALEV_1618_Eng_Current_Num_Coded);
        Assert.Equal(expected.TALLPPEGRD_ALEV_1618_Eng_Current, result.TALLPPEGRD_ALEV_1618_Eng_Current);

        Assert.Equal(expected.VA_INS_ACAD_Eng_Current_Num_Coded, result.VA_INS_ACAD_Eng_Current_Num_Coded);
        Assert.Equal(expected.TALLPPE_ACAD_1618_Eng_Current_Num_Coded, result.TALLPPE_ACAD_1618_Eng_Current_Num_Coded);
        Assert.Equal(expected.TALLPPEGRD_ACAD_1618_Eng_Current, result.TALLPPEGRD_ACAD_1618_Eng_Current);

        _mockEnglandKs5PerformanceRepo.Verify(
            r => r.ReadSingleAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetLAPerformanceAsync_ReturnsNewLAPerformanceWhenLaCodeDoesNotExist()
    {
        // Arrange
        var laCode = "999";

        _mockLAKs5PerformanceRepo
            .Setup(r => r.ReadAsync(laCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync((KS5LAPerformance?)null);

        // Act
        var result = await _sut.GetLaPerformanceAsync(laCode, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(string.Empty, result.Id);

        _mockLAKs5PerformanceRepo.Verify(r => r.ReadAsync(laCode, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetLAPerformanceAsync_ReturnsCorrectItemWhenLaCodeExists()
    {
        // Arrange
        var laCode = "1";
        var expected = new KS5LAPerformance 
        { 
            Id = laCode,
            TALLPPE_ALEV_1618_LA_Current_Num_Coded = new CodedDouble(85.20, string.Empty, string.Empty),
            TALLPPEGRD_ALEV_1618_LA_Current = new CodedString("A", string.Empty, string.Empty),

            TALLPPE_ACAD_1618_LA_Current_Num_Coded = new CodedDouble(45.15, string.Empty, string.Empty),
            TALLPPEGRD_ACAD_1618_LA_Current = new CodedString("B", string.Empty, string.Empty),
        };

        _mockLAKs5PerformanceRepo
            .Setup(r => r.ReadAsync(laCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var result = await _sut.GetLaPerformanceAsync(laCode, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("1", result.Id);
        Assert.Equal(expected.TALLPPE_ALEV_1618_LA_Current_Num_Coded, result.TALLPPE_ALEV_1618_LA_Current_Num_Coded);
        Assert.Equal(expected.TALLPPEGRD_ALEV_1618_LA_Current, result.TALLPPEGRD_ALEV_1618_LA_Current);

        Assert.Equal(expected.TALLPPE_ACAD_1618_LA_Current_Num_Coded, result.TALLPPE_ACAD_1618_LA_Current_Num_Coded);
        Assert.Equal(expected.TALLPPEGRD_ACAD_1618_LA_Current, result.TALLPPEGRD_ACAD_1618_LA_Current);

        _mockLAKs5PerformanceRepo.Verify(r => r.ReadAsync(laCode, It.IsAny<CancellationToken>()), Times.Once);
    }
}
