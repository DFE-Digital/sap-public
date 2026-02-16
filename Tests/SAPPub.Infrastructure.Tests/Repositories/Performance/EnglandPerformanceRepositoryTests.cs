using Moq;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Infrastructure.Repositories.KS4.Performance;

namespace SAPPub.Infrastructure.Tests.Repositories.Performance
{
    public class EnglandPerformanceRepositoryTests
    {
        private readonly Mock<IGenericRepository<EnglandPerformance>> _mockGenericRepo;
        private readonly EnglandPerformanceRepository _sut;

        public EnglandPerformanceRepositoryTests()
        {
            _mockGenericRepo = new Mock<IGenericRepository<EnglandPerformance>>();
            _sut = new EnglandPerformanceRepository(_mockGenericRepo.Object);
        }

        [Fact]
        public async Task GetEnglandPerformanceAsync_ReturnsItemFromGenericRepository()
        {
            // Arrange
            var expected = new EnglandPerformance
            {
                Attainment8_Tot_Eng_Current_Num = 99.99
            };

            _mockGenericRepo
                .Setup(r => r.ReadSingleAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _sut.GetEnglandPerformanceAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(99.99, result.Attainment8_Tot_Eng_Current_Num);

            _mockGenericRepo.Verify(
                r => r.ReadSingleAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetEnglandPerformanceAsync_ReturnsNewEnglandPerformance_WhenGenericRepositoryReturnsNull()
        {
            // Arrange
            _mockGenericRepo
                .Setup(r => r.ReadSingleAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((EnglandPerformance?)null);

            // Act
            var result = await _sut.GetEnglandPerformanceAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0d, result.Attainment8_Tot_Eng_Current_Num);

            _mockGenericRepo.Verify(
                r => r.ReadSingleAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetAllEnglandPerformanceAsync_ReturnsAllItemsFromGenericRepository()
        {
            // Arrange
            var expected = new List<EnglandPerformance>
            {
                new() { Attainment8_Tot_Eng_Current_Num = 99.99 }
            };

            _mockGenericRepo
                .Setup(r => r.ReadAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _sut.GetAllEnglandPerformanceAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(99.99, result.First().Attainment8_Tot_Eng_Current_Num);

            _mockGenericRepo.Verify(r => r.ReadAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
