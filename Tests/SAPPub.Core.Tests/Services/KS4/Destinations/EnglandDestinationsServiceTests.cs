using Moq;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Interfaces.Repositories.KS4.Destinations;
using SAPPub.Core.Services.KS4.Destinations;

namespace SAPPub.Core.Tests.Services.KS4.Destinations
{
    public class EnglandDestinationsServiceTests
    {
        private readonly Mock<IEnglandDestinationsRepository> _mockRepo;
        private readonly EnglandDestinationsService _service;

        public EnglandDestinationsServiceTests()
        {
            _mockRepo = new Mock<IEnglandDestinationsRepository>();
            _service = new EnglandDestinationsService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetEnglandDestinationsAsync_ShouldReturnData()
        {
            // Arrange
            var expected = new EnglandDestinations { AllDest_Tot_Eng_Current_Pct = 99.99 };

            _mockRepo
                .Setup(r => r.GetEnglandDestinationsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.GetEnglandDestinationsAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(99.99, result.AllDest_Tot_Eng_Current_Pct);
        }

        [Fact]
        public async Task GetEnglandDestinationsAsync_ShouldReturnDefault_WhenNoData()
        {
            // Arrange
            _mockRepo
                .Setup(r => r.GetEnglandDestinationsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new EnglandDestinations());

            // Act
            var result = await _service.GetEnglandDestinationsAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetEnglandDestinationsAsync_ShouldPropagateException_WhenRepositoryThrows()
        {
            // Arrange
            _mockRepo
                .Setup(r => r.GetEnglandDestinationsAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.GetEnglandDestinationsAsync(CancellationToken.None));
            Assert.Equal("Database error", ex.Message);
        }
    }
}
