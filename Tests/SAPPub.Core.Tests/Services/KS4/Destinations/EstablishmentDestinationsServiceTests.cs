using Moq;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Interfaces.Repositories.KS4.Destinations;
using SAPPub.Core.Services.KS4.Destinations;

namespace SAPPub.Core.Tests.Services.KS4.Destinations
{
    public class EstablishmentDestinationsServiceTests
    {
        private readonly Mock<IEstablishmentDestinationsRepository> _mockRepo;
        private readonly EstablishmentDestinationsService _service;

        public EstablishmentDestinationsServiceTests()
        {
            _mockRepo = new Mock<IEstablishmentDestinationsRepository>();
            _service = new EstablishmentDestinationsService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllEstablishmentDestinationsAsync_ShouldReturnAllItems()
        {
            // Arrange
            var expected = new List<EstablishmentDestinations>
            {
                new() { Id = "100", AllDest_Tot_Est_Current_Pct = 99.99 },
                new() { Id = "101", AllDest_Tot_Est_Current_Pct = 90.00 }
            };

            _mockRepo
                .Setup(r => r.GetAllEstablishmentDestinationsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.GetAllEstablishmentDestinationsAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, a => a.AllDest_Tot_Est_Current_Pct == 99.99);
            Assert.Contains(result, a => a.AllDest_Tot_Est_Current_Pct == 90.00);
        }

        [Fact]
        public async Task GetAllEstablishmentDestinationsAsync_ShouldReturnEmpty_WhenNoData()
        {
            // Arrange
            _mockRepo
                .Setup(r => r.GetAllEstablishmentDestinationsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<EstablishmentDestinations>());

            // Act
            var result = await _service.GetAllEstablishmentDestinationsAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetEstablishmentDestinationsAsync_ShouldReturnCorrectItem_WhenUrnExists()
        {
            // Arrange
            var urn = "100";
            var expected = new EstablishmentDestinations { Id = urn, AllDest_Tot_Est_Current_Pct = 100 };

            _mockRepo
                .Setup(r => r.GetEstablishmentDestinationsAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.GetEstablishmentDestinationsAsync(urn, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(urn, result.Id);
            Assert.Equal(100, result.AllDest_Tot_Est_Current_Pct);
        }

        [Fact]
        public async Task GetEstablishmentDestinationsAsync_ShouldReturnDefault_WhenUrnDoesNotExist()
        {
            // Arrange
            var urn = "99999";

            // Service returns nullable; repo may return null; support either by returning null from repo setup.
            _mockRepo
                .Setup(r => r.GetEstablishmentDestinationsAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync((EstablishmentDestinations?)null);

            // Act
            var result = await _service.GetEstablishmentDestinationsAsync(urn, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetEstablishmentDestinationsAsync_ShouldPropagateException_WhenRepositoryThrows()
        {
            // Arrange
            var urn = "error";

            _mockRepo
                .Setup(r => r.GetEstablishmentDestinationsAsync(urn, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.GetEstablishmentDestinationsAsync(urn, CancellationToken.None));
            Assert.Equal("Database error", ex.Message);
        }
    }
}
