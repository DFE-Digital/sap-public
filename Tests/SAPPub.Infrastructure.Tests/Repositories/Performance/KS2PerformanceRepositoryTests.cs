using Moq;
using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.ValueObjects;
using SAPPub.Infrastructure.Repositories.Performance;

namespace SAPPub.Infrastructure.Tests.Repositories.Performance;

public class KS2PerformanceRepositoryTests
{
    private readonly Mock<IGenericRepository<KS2EstablishmentPerformance>> _mockEstablishmentKS2PerformanceRepo;
    private readonly Mock<IGenericRepository<KS2EnglandPerformance>> _mockEnglandKS2PerformanceRepo;    
    private readonly Mock<IGenericRepository<KS2LAPerformance>> _mockLAKS2PerformanceRepo;
    private readonly KS2PerformanceRepository _sut;

    public KS2PerformanceRepositoryTests()
    {
        _mockEstablishmentKS2PerformanceRepo = new Mock<IGenericRepository<KS2EstablishmentPerformance>>();
        _mockEnglandKS2PerformanceRepo = new Mock<IGenericRepository<KS2EnglandPerformance>>();
        _mockLAKS2PerformanceRepo = new Mock<IGenericRepository<KS2LAPerformance>>();
        _sut = new KS2PerformanceRepository(
            _mockEstablishmentKS2PerformanceRepo.Object,
            _mockEnglandKS2PerformanceRepo.Object,            
            _mockLAKS2PerformanceRepo.Object);
    }

    [Fact]
    public async Task GetEstablishmentKS2PerformanceAsync_ReturnsNewEstablishmentPerformanceWhenUrnDoesNotExist()
    {
        // Arrange
        var urn = "999";

        _mockEstablishmentKS2PerformanceRepo
            .Setup(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync((KS2EstablishmentPerformance?)null);

        // Act
        var result = await _sut.GetEstablishmentPerformanceAsync(urn, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(string.Empty, result.Id);

        _mockEstablishmentKS2PerformanceRepo.Verify(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetEstablishmentKS2PerformanceAsync_ReturnsCorrectItemWhenUrnExists()
    {
        // Arrange
        var urn = "123654";
        var expected = new KS2EstablishmentPerformance 
        {
            Id = urn,
            READ_AVERAGE_Est_Current_Num_Coded = new CodedDouble(1, string.Empty, "1"),
            READ_AVERAGE_Est_Previous_Num_Coded = new CodedDouble(2, string.Empty, "2"),
            READ_AVERAGE_Est_Previous2_Num_Coded = new CodedDouble(3, string.Empty, "3")
        };

        _mockEstablishmentKS2PerformanceRepo
            .Setup(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var result = await _sut.GetEstablishmentPerformanceAsync(urn, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected.Id, result.Id);
        Assert.Equal(expected.READ_AVERAGE_Est_Current_Num_Coded, result.READ_AVERAGE_Est_Current_Num_Coded);
        Assert.Equal(expected.READ_AVERAGE_Est_Previous_Num_Coded, result.READ_AVERAGE_Est_Previous_Num_Coded);
        Assert.Equal(expected.READ_AVERAGE_Est_Previous2_Num_Coded, result.READ_AVERAGE_Est_Previous2_Num_Coded);

        _mockEstablishmentKS2PerformanceRepo.Verify(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetEnglandKS2PerformanceAsync_ReturnsNewEnglandPerformance_WhenReturnsNull()
    {
        // Arrange
        _mockEnglandKS2PerformanceRepo
            .Setup(r => r.ReadSingleAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((KS2EnglandPerformance?)null);

        // Act
        var result = await _sut.GetEnglandPerformanceAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        var expected = new KS2EnglandPerformance();
        Assert.Equal(expected.READ_AVERAGE_Eng_Current_Num_Coded, result.READ_AVERAGE_Eng_Current_Num_Coded);
        Assert.Equal(expected.READ_AVERAGE_Eng_Previous_Num_Coded, result.READ_AVERAGE_Eng_Previous_Num_Coded);
        Assert.Equal(expected.READ_AVERAGE_Eng_Previous2_Num_Coded, result.READ_AVERAGE_Eng_Previous2_Num_Coded);

        _mockEnglandKS2PerformanceRepo.Verify(
            r => r.ReadSingleAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetEnglandPerformanceAsync_ReturnsItem()
    {
        // Arrange
        var expected = new KS2EnglandPerformance
        {
            READ_AVERAGE_Eng_Current_Num_Coded = new CodedDouble(1, string.Empty, "1"),
            READ_AVERAGE_Eng_Previous_Num_Coded = new CodedDouble(2, string.Empty, "2"),
            READ_AVERAGE_Eng_Previous2_Num_Coded = new CodedDouble(3, string.Empty, "3")
        };

        _mockEnglandKS2PerformanceRepo
            .Setup(r => r.ReadSingleAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var result = await _sut.GetEnglandPerformanceAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected.READ_AVERAGE_Eng_Current_Num_Coded, result.READ_AVERAGE_Eng_Current_Num_Coded);
        Assert.Equal(expected.READ_AVERAGE_Eng_Previous_Num_Coded, result.READ_AVERAGE_Eng_Previous_Num_Coded);
        Assert.Equal(expected.READ_AVERAGE_Eng_Previous2_Num_Coded, result.READ_AVERAGE_Eng_Previous2_Num_Coded);

        _mockEnglandKS2PerformanceRepo.Verify(
            r => r.ReadSingleAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetLAPerformanceAsync_ReturnsNewLAPerformanceWhenLaCodeDoesNotExist()
    {
        // Arrange
        var laCode = "999";

        _mockLAKS2PerformanceRepo
            .Setup(r => r.ReadAsync(laCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync((KS2LAPerformance?)null);

        // Act
        var result = await _sut.GetLaPerformanceAsync(laCode, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(string.Empty, result.Id);

        _mockLAKS2PerformanceRepo.Verify(r => r.ReadAsync(laCode, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetLAPerformanceAsync_ReturnsCorrectItemWhenLaCodeExists()
    {
        // Arrange
        var laCode = "1";
        var expected = new KS2LAPerformance 
        { 
            Id = laCode,
            READ_AVERAGE_LA_Current_Num_Coded = new CodedDouble(1, string.Empty, "1"),
            READ_AVERAGE_LA_Previous_Num_Coded = new CodedDouble(2, string.Empty, "2"),
            READ_AVERAGE_LA_Previous2_Num_Coded = new CodedDouble(3, string.Empty, "3")
        };

        _mockLAKS2PerformanceRepo
            .Setup(r => r.ReadAsync(laCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var result = await _sut.GetLaPerformanceAsync(laCode, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("1", result.Id);
        Assert.Equal(expected.READ_AVERAGE_LA_Current_Num_Coded, result.READ_AVERAGE_LA_Current_Num_Coded);
        Assert.Equal(expected.READ_AVERAGE_LA_Previous_Num_Coded, result.READ_AVERAGE_LA_Previous_Num_Coded);
        Assert.Equal(expected.READ_AVERAGE_LA_Previous2_Num_Coded, result.READ_AVERAGE_LA_Previous2_Num_Coded);

        _mockLAKS2PerformanceRepo.Verify(r => r.ReadAsync(laCode, It.IsAny<CancellationToken>()), Times.Once);
    }
}
