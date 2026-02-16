using Microsoft.Extensions.Logging;
using Moq;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Infrastructure.Repositories.KS4.Performance;

namespace SAPPub.Infrastructure.Tests.Repositories.Performance
{
    public class LAPerformanceRepositoryTests
    {
        private readonly Mock<IGenericRepository<LAPerformance>> _mockGenericRepo;
        private readonly Mock<ILogger<LAPerformanceRepository>> _mockLogger;
        private readonly LAPerformanceRepository _sut;

        public LAPerformanceRepositoryTests()
        {
            _mockGenericRepo = new Mock<IGenericRepository<LAPerformance>>();
            _mockLogger = new Mock<ILogger<LAPerformanceRepository>>();
            _sut = new LAPerformanceRepository(_mockGenericRepo.Object);
        }

        [Fact]
        public async Task GetAllLAPerformanceAsync_ReturnsAllItemsFromGenericRepository()
        {
            // Arrange
            var expected = new List<LAPerformance>
            {
                new() { Id = "1", Attainment8_Tot_LA_Current_Num = 99.99 },
                new() { Id = "2", Attainment8_Tot_LA_Current_Num = 88.88 }
            };

            _mockGenericRepo
                .Setup(r => r.ReadAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _sut.GetAllLAPerformanceAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, e => e.Id == "1");
            Assert.Contains(result, e => e.Id == "2");

            _mockGenericRepo.Verify(r => r.ReadAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAllLAPerformanceAsync_ReturnsEmptyWhenGenericRepositoryReturnsEmpty()
        {
            // Arrange
            _mockGenericRepo
                .Setup(r => r.ReadAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<LAPerformance>());

            // Act
            var result = await _sut.GetAllLAPerformanceAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _mockGenericRepo.Verify(r => r.ReadAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetLAPerformanceAsync_ReturnsCorrectItemWhenLaCodeExists()
        {
            // Arrange
            var laCode = "1";
            var expected = new LAPerformance { Id = laCode, Attainment8_Tot_LA_Current_Num = 99.99 };

            _mockGenericRepo
                .Setup(r => r.ReadAsync(laCode, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _sut.GetLAPerformanceAsync(laCode, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1", result.Id);
            Assert.Equal(99.99, result.Attainment8_Tot_LA_Current_Num);

            _mockGenericRepo.Verify(r => r.ReadAsync(laCode, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetLAPerformanceAsync_ReturnsNewLAPerformanceWhenLaCodeDoesNotExist()
        {
            // Arrange
            var laCode = "999";

            _mockGenericRepo
                .Setup(r => r.ReadAsync(laCode, It.IsAny<CancellationToken>()))
                .ReturnsAsync((LAPerformance?)null);

            // Act
            var result = await _sut.GetLAPerformanceAsync(laCode, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(string.Empty, result.Id);

            _mockGenericRepo.Verify(r => r.ReadAsync(laCode, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
