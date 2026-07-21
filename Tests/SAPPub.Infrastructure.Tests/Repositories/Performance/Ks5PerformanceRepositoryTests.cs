using Moq;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Infrastructure.Repositories.Performance;

namespace SAPPub.Infrastructure.Tests.Repositories.Performance;

public class Ks5PerformanceRepositoryTests
{
    private readonly Mock<IGenericRepository<EstablishmentKs5Performance>> _mockEstablishmentKs5PerformanceRepo;
    private readonly Mock<IGenericRepository<EnglandKs5Performance>> _mockEnglandKs5PerformanceRepo;    
    private readonly Mock<IGenericRepository<LAKs5Performance>> _mockLAKs5PerformanceRepo;
    private readonly Ks5PerformanceRepository _sut;

    public Ks5PerformanceRepositoryTests()
    {
        _mockEstablishmentKs5PerformanceRepo = new Mock<IGenericRepository<EstablishmentKs5Performance>>();
        _mockEnglandKs5PerformanceRepo = new Mock<IGenericRepository<EnglandKs5Performance>>();
        _mockLAKs5PerformanceRepo = new Mock<IGenericRepository<LAKs5Performance>>();
        _sut = new Ks5PerformanceRepository(
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
            .ReturnsAsync((EstablishmentKs5Performance?)null);

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
        var expected = new EstablishmentKs5Performance 
        {
            Id = urn,
            TALLPUP_ACAD_1618_Est_Current_Num = 150,
            VA_INS_ALEV_Est_Current_Num = 50.5,
            PROGRESS_BAND_ALEV_Est_Current = "Average",
            UCI_INS_ALEV_Est_Current_Num = -0.5,
            LCI_INS_ALEV_Est_Current_Num = -0.7,
        };

        _mockEstablishmentKs5PerformanceRepo
            .Setup(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var result = await _sut.GetEstablishmentPerformanceAsync(urn, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected.Id, result.Id);
        Assert.Equal(expected.TALLPUP_ACAD_1618_Est_Current_Num, result.TALLPUP_ACAD_1618_Est_Current_Num);
        Assert.Equal(expected.VA_INS_ALEV_Est_Current_Num, result.VA_INS_ALEV_Est_Current_Num);
        Assert.Equal(expected.PROGRESS_BAND_ALEV_Est_Current, result.PROGRESS_BAND_ALEV_Est_Current);
        Assert.Equal(expected.UCI_INS_ALEV_Est_Current_Num, result.UCI_INS_ALEV_Est_Current_Num);
        Assert.Equal(expected.LCI_INS_ALEV_Est_Current_Num, result.LCI_INS_ALEV_Est_Current_Num);

        _mockEstablishmentKs5PerformanceRepo.Verify(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetEnglandKs5PerformanceAsync_ReturnsNewEnglandPerformance_WhenReturnsNull()
    {
        // Arrange
        _mockEnglandKs5PerformanceRepo
            .Setup(r => r.ReadSingleAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((EnglandKs5Performance?)null);

        // Act
        var result = await _sut.GetEnglandPerformanceAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        var expected = new EnglandKs5Performance();
        Assert.Equal(expected.VA_INS_ALEV_Eng_Current_Num_Coded, result.VA_INS_ALEV_Eng_Current_Num_Coded);

        _mockEnglandKs5PerformanceRepo.Verify(
            r => r.ReadSingleAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetEnglandPerformanceAsync_ReturnsItem()
    {
        // Arrange
        var expected = new EnglandKs5Performance
        {
            VA_INS_ALEV_Eng_Current_Num = 75.55
        };

        _mockEnglandKs5PerformanceRepo
            .Setup(r => r.ReadSingleAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var result = await _sut.GetEnglandPerformanceAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected.VA_INS_ALEV_Eng_Current_Num, result.VA_INS_ALEV_Eng_Current_Num);

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
            .ReturnsAsync((LAKs5Performance?)null);

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
        var expected = new LAKs5Performance { Id = laCode, TALLPPE_ALEV_1618_LA_Current_Num = 85.20 };

        _mockLAKs5PerformanceRepo
            .Setup(r => r.ReadAsync(laCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var result = await _sut.GetLaPerformanceAsync(laCode, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("1", result.Id);
        Assert.Equal(expected.TALLPPE_ALEV_1618_LA_Current_Num, result.TALLPPE_ALEV_1618_LA_Current_Num);

        _mockLAKs5PerformanceRepo.Verify(r => r.ReadAsync(laCode, It.IsAny<CancellationToken>()), Times.Once);
    }
}
