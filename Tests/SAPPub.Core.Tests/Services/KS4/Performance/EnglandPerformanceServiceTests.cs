using Moq;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Interfaces.Repositories.KS4.Performance;
using SAPPub.Core.Services.KS4.Performance;

namespace SAPPub.Core.Tests.Services.KS4.Performance
{
    public class EnglandPerformanceServiceTests
    {
        private readonly Mock<IEnglandPerformanceRepository> _mockRepo;
        private readonly EnglandPerformanceService _service;

        public EnglandPerformanceServiceTests()
        {
            _mockRepo = new Mock<IEnglandPerformanceRepository>();
            _service = new EnglandPerformanceService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetEnglandPerformanceAsync_ShouldReturnData()
        {
            // Arrange
            var expected = new EnglandPerformance { Attainment8_Tot_Eng_Current_Num = 99.99 };

            _mockRepo
                .Setup(r => r.GetEnglandPerformanceAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.GetEnglandPerformanceAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(99.99, result.Attainment8_Tot_Eng_Current_Num);
        }

        [Fact]
        public async Task GetEnglandPerformanceAsync_ShouldReturnDefault_WhenNoData()
        {
            // Arrange
            _mockRepo
                .Setup(r => r.GetEnglandPerformanceAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new EnglandPerformance());

            // Act
            var result = await _service.GetEnglandPerformanceAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetEnglandPerformanceAsync_ShouldPropagateException_WhenRepositoryThrows()
        {
            // Arrange
            _mockRepo
                .Setup(r => r.GetEnglandPerformanceAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.GetEnglandPerformanceAsync(CancellationToken.None));
            Assert.Equal("Database error", ex.Message);
        }
    }
}
