using Microsoft.Extensions.Logging;
using Moq;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Infrastructure.Repositories.KS4.Destinations;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SAPPub.Infrastructure.Tests.Repositories.Destinations
{
    public class EnglandDestinationsRepositoryTests
    {
        private readonly Mock<IGenericRepository<EnglandDestinations>> _mockGenericRepo;
        private readonly Mock<ILogger<EnglandDestinationsRepository>> _mockLogger;
        private readonly EnglandDestinationsRepository _sut;

        public EnglandDestinationsRepositoryTests()
        {
            _mockGenericRepo = new Mock<IGenericRepository<EnglandDestinations>>();
            _mockLogger = new Mock<ILogger<EnglandDestinationsRepository>>();
            _sut = new EnglandDestinationsRepository(_mockGenericRepo.Object);
        }

        [Fact]
        public async Task GetEnglandDestinationsAsync_ReturnsItemFromGenericRepository()
        {
            // Arrange
            var expected = new EnglandDestinations
            {
                AllDest_Tot_Eng_Current_Pct = 99.99
            };

            _mockGenericRepo
                .Setup(r => r.ReadSingleAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _sut.GetEnglandDestinationsAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(99.99, result.AllDest_Tot_Eng_Current_Pct);

            _mockGenericRepo.Verify(
                r => r.ReadSingleAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetEnglandDestinationsAsync_ReturnsDefaultWhenGenericRepositoryReturnsNull()
        {
            // Arrange
            _mockGenericRepo
                .Setup(r => r.ReadSingleAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((EnglandDestinations?)null);

            // Act
            var result = await _sut.GetEnglandDestinationsAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result); // repository returns new EnglandDestinations()

            _mockGenericRepo.Verify(
                r => r.ReadSingleAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
