using Moq;
using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Interfaces.Repositories.KS4.Absence;
using SAPPub.Core.Services.KS4.Absence;

namespace SAPPub.Core.Tests.Services.KS4.Absence
{
    public class EstablishmentAbsenceServiceTests
    {
        private readonly Mock<IEstablishmentAbsenceRepository> _mockRepo;
        private readonly EstablishmentAbsenceService _service;

        public EstablishmentAbsenceServiceTests()
        {
            _mockRepo = new Mock<IEstablishmentAbsenceRepository>();
            _service = new EstablishmentAbsenceService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllEstablishmentAbsenceAsync_ShouldReturnAllItems()
        {
            // Arrange
            var expectedAbsences = new List<EstablishmentAbsence>
            {
                new() { Id = "100", Abs_Tot_Est_Current_Pct = 99.99 },
                new() { Id = "101", Abs_Tot_Est_Current_Pct = 90.00 }
            };

            _mockRepo
                .Setup(r => r.GetAllEstablishmentAbsenceAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedAbsences);

            // Act
            var result = await _service.GetAllEstablishmentAbsenceAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, a => a.Abs_Tot_Est_Current_Pct == 99.99);
            Assert.Contains(result, a => a.Abs_Tot_Est_Current_Pct == 90.00);
        }

        [Fact]
        public async Task GetAllEstablishmentAbsenceAsync_ShouldReturnEmpty_WhenNoData()
        {
            // Arrange
            _mockRepo
                .Setup(r => r.GetAllEstablishmentAbsenceAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<EstablishmentAbsence>());

            // Act
            var result = await _service.GetAllEstablishmentAbsenceAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetEstablishmentAbsenceAsync_ShouldReturnCorrectItem_WhenUrnExists()
        {
            // Arrange
            var urn = "100";
            var expectedAbsence = new EstablishmentAbsence { Id = urn, Abs_Tot_Est_Current_Pct = 100 };

            _mockRepo
                .Setup(r => r.GetEstablishmentAbsenceAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedAbsence);

            // Act
            var result = await _service.GetEstablishmentAbsenceAsync(urn, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(urn, result.Id);
            Assert.Equal(100, result.Abs_Tot_Est_Current_Pct);
        }

        [Fact]
        public async Task GetEstablishmentAbsenceAsync_ShouldReturnDefault_WhenUrnDoesNotExist()
        {
            // Arrange
            var urn = "99999";

            // Repo (per refactor) returns a default object when not found.
            _mockRepo
                .Setup(r => r.GetEstablishmentAbsenceAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new EstablishmentAbsence());

            // Act
            var result = await _service.GetEstablishmentAbsenceAsync(urn, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.UnAuth_Tot_Est_Current_Pct);
        }

        [Fact]
        public async Task GetEstablishmentAbsenceAsync_ShouldPropagateException_WhenRepositoryThrows()
        {
            // Arrange
            var urn = "error";

            _mockRepo
                .Setup(r => r.GetEstablishmentAbsenceAsync(urn, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Database error"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<System.Exception>(() => _service.GetEstablishmentAbsenceAsync(urn, CancellationToken.None));
            Assert.Equal("Database error", ex.Message);
        }
    }
}
