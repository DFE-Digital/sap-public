using Moq;
using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Interfaces.Repositories.KS4.Absence;
using SAPPub.Core.Services.KS4.Absence;

namespace SAPPub.Core.Tests.Services.KS4.Absence;

public class LAAbsenceServiceTests
{
    private readonly Mock<ILAAbsenceRepository> _mockRepo;
    private readonly LAAbsenceService _service;

    public LAAbsenceServiceTests()
    {
        _mockRepo = new Mock<ILAAbsenceRepository>();
        _service = new LAAbsenceService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetLAAbsenceAsync_ShouldReturnCorrectItem_WhenLaCodeExists()
    {
        // Arrange
        var laCode = "100";
        var expected = new LAAbsence { Id = laCode, Abs_Tot_LA_Current_Pct = 5.5 };

        _mockRepo
            .Setup(r => r.GetLAAbsenceAsync(laCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var result = await _service.GetLAAbsenceAsync(laCode, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(laCode, result.Id);
        Assert.Equal(expected.Abs_Tot_LA_Current_Pct, result.Abs_Tot_LA_Current_Pct);
    }

    [Fact]
    public async Task GetLAAbsenceAsync_ShouldReturnDefault_WhenLaCodeDoesNotExist()
    {
        // Arrange
        var laCode = "99999";

        _mockRepo
            .Setup(r => r.GetLAAbsenceAsync(laCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new LAAbsence());

        // Act
        var result = await _service.GetLAAbsenceAsync(laCode, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.Abs_Tot_LA_Current_Pct);
    }

    [Fact]
    public async Task GetLAAbsenceAsync_ShouldPropagateException_WhenRepositoryThrows()
    {
        // Arrange
        var laCode = "error";

        _mockRepo
            .Setup(r => r.GetLAAbsenceAsync(laCode, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => _service.GetLAAbsenceAsync(laCode, CancellationToken.None));
        Assert.Equal("Database error", ex.Message);
    }
}
