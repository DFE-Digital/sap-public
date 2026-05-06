using Moq;
using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Interfaces.Repositories.KS4.Absence;
using SAPPub.Core.Services.KS4.Absence;

namespace SAPPub.Core.Tests.Services.KS4.Absence;

public class EnglandAbsenceServiceTests
{
    private readonly Mock<IEnglandAbsenceRepository> _mockRepo;
    private readonly EnglandAbsenceService _service;

    public EnglandAbsenceServiceTests()
    {
        _mockRepo = new Mock<IEnglandAbsenceRepository>();
        _service = new EnglandAbsenceService(_mockRepo.Object);
    }


    [Fact]
    public async Task GetEnglandAbsenceAsync_ShouldReturnData()
    {
        // Arrange
        var expected = new EnglandAbsence { Abs_Tot_Eng_Current_Pct = 10.99 };

        _mockRepo
            .Setup(r => r.GetEnglandAbsenceAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var result = await _service.GetEnglandAbsenceAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected.Abs_Tot_Eng_Current_Pct, result.Abs_Tot_Eng_Current_Pct);
    }

    [Fact]
    public async Task GetEnglandAbsenceAsync_ShouldReturnDefault_WhenNoData()
    {
        // Arrange
        _mockRepo
            .Setup(r => r.GetEnglandAbsenceAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EnglandAbsence());

        // Act
        var result = await _service.GetEnglandAbsenceAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetEnglandAbsenceAsync_ShouldPropagateException_WhenRepositoryThrows()
    {
        // Arrange
        _mockRepo
            .Setup(r => r.GetEnglandAbsenceAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => _service.GetEnglandAbsenceAsync(CancellationToken.None));
        Assert.Equal("Database error", ex.Message);
    }
}
