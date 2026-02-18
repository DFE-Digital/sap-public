using Moq;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Interfaces.Repositories.KS4.Destinations;
using SAPPub.Core.Services.KS4.Destinations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SAPPub.Core.Tests.Services.KS4.Destinations
{
    public class LADestinationsServiceTests
    {
        private readonly Mock<ILADestinationsRepository> _mockRepo;
        private readonly LADestinationsService _service;

        public LADestinationsServiceTests()
        {
            _mockRepo = new Mock<ILADestinationsRepository>();
            _service = new LADestinationsService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllLADestinationsAsync_ShouldReturnAllItems()
        {
            // Arrange
            var expected = new List<LADestinations>
            {
                new() { Id = "100", AllDest_Tot_LA_Current_Pct = 99.99 },
                new() { Id = "101", AllDest_Tot_LA_Current_Pct = 90.00 }
            };

            _mockRepo
                .Setup(r => r.GetAllLADestinationsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.GetAllLADestinationsAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, a => a.AllDest_Tot_LA_Current_Pct == 99.99);
            Assert.Contains(result, a => a.AllDest_Tot_LA_Current_Pct == 90.00);
        }

        [Fact]
        public async Task GetAllLADestinationsAsync_ShouldReturnEmpty_WhenNoData()
        {
            // Arrange
            _mockRepo
                .Setup(r => r.GetAllLADestinationsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<LADestinations>());

            // Act
            var result = await _service.GetAllLADestinationsAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetLADestinationsAsync_ShouldReturnCorrectItem_WhenLaCodeExists()
        {
            // Arrange
            var laCode = "100";
            var expected = new LADestinations { Id = laCode, AllDest_Tot_LA_Current_Pct = 100 };

            _mockRepo
                .Setup(r => r.GetLADestinationsAsync(laCode, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.GetLADestinationsAsync(laCode, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(laCode, result.Id);
            Assert.Equal(100, result.AllDest_Tot_LA_Current_Pct);
        }

        [Fact]
        public async Task GetLADestinationsAsync_ShouldReturnDefault_WhenLaCodeDoesNotExist()
        {
            // Arrange
            var laCode = "99999";

            // Repo (per refactor) may return a default object when not found.
            _mockRepo
                .Setup(r => r.GetLADestinationsAsync(laCode, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new LADestinations());

            // Act
            var result = await _service.GetLADestinationsAsync(laCode, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.AllDest_Tot_LA_Previous2_Pct);
        }

        [Fact]
        public async Task GetLADestinationsAsync_ShouldPropagateException_WhenRepositoryThrows()
        {
            // Arrange
            var laCode = "error";

            _mockRepo
                .Setup(r => r.GetLADestinationsAsync(laCode, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.GetLADestinationsAsync(laCode, CancellationToken.None));
            Assert.Equal("Database error", ex.Message);
        }
    }
}
