using Microsoft.Extensions.Logging;
using Moq;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Infrastructure.Repositories.KS4.Destinations;

namespace SAPPub.Infrastructure.Tests.Repositories.Destinations
{
    public class LADestinationsRepositoryTests
    {
        private readonly Mock<IGenericRepository<LADestinations>> _mockGenericRepo;
        private readonly Mock<ILogger<LADestinationsRepository>> _mockLogger;
        private readonly LADestinationsRepository _sut;

        public LADestinationsRepositoryTests()
        {
            _mockGenericRepo = new Mock<IGenericRepository<LADestinations>>();
            _mockLogger = new Mock<ILogger<LADestinationsRepository>>();
            _sut = new LADestinationsRepository(_mockGenericRepo.Object);
        }

        [Fact]
        public async Task GetAllLADestinationsAsync_ReturnsAllItemsFromGenericRepository()
        {
            // Arrange
            var expected = new List<LADestinations>
            {
                new() { Id = "1", AllDest_Tot_LA_Current_Pct = 99.99 },
                new() { Id = "2", AllDest_Tot_LA_Current_Pct = 88.88 }
            };

            _mockGenericRepo
                .Setup(r => r.ReadAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _sut.GetAllLADestinationsAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, e => e.Id == "1");
            Assert.Contains(result, e => e.Id == "2");

            _mockGenericRepo.Verify(r => r.ReadAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAllLADestinationsAsync_ReturnsEmptyWhenGenericRepositoryReturnsEmpty()
        {
            // Arrange
            _mockGenericRepo
                .Setup(r => r.ReadAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<LADestinations>());

            // Act
            var result = await _sut.GetAllLADestinationsAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _mockGenericRepo.Verify(r => r.ReadAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetLADestinationsAsync_ReturnsCorrectItemWhenLaCodeExists()
        {
            // Arrange
            var laCode = "1";
            var expected = new LADestinations { Id = laCode, AllDest_Tot_LA_Current_Pct = 99.99 };

            _mockGenericRepo
                .Setup(r => r.ReadAsync(laCode, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _sut.GetLADestinationsAsync(laCode, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1", result.Id);
            Assert.Equal(99.99, result.AllDest_Tot_LA_Current_Pct);

            _mockGenericRepo.Verify(r => r.ReadAsync(laCode, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetLADestinationsAsync_ReturnsDefaultWhenLaCodeDoesNotExist()
        {
            // Arrange
            var laCode = "999";

            _mockGenericRepo
                .Setup(r => r.ReadAsync(laCode, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new LADestinations());

            // Act
            var result = await _sut.GetLADestinationsAsync(laCode, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(string.Empty, result.Id);

            _mockGenericRepo.Verify(r => r.ReadAsync(laCode, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
