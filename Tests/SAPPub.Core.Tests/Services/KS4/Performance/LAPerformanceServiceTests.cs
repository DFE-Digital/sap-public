using Moq;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Interfaces.Repositories.KS4.Performance;
using SAPPub.Core.Services.KS4.Performance;

namespace SAPPub.Core.Tests.Services.KS4.Performance
{
    public class LAPerformanceServiceTests
    {
        private readonly Mock<ILAPerformanceRepository> _mockRepo;
        private readonly LAPerformanceService _service;

        public LAPerformanceServiceTests()
        {
            _mockRepo = new Mock<ILAPerformanceRepository>();
            _service = new LAPerformanceService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllLAPerformanceAsync_ShouldReturnAllItems()
        {
            // Arrange
            var expected = new List<LAPerformance>
            {
                new() { Id = "100", Attainment8_Tot_LA_Current_Num = 99.99 },
                new() { Id = "101", Attainment8_Tot_LA_Current_Num = 90.00 }
            };

            _mockRepo
                .Setup(r => r.GetAllLAPerformanceAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.GetAllLAPerformanceAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, a => a.Attainment8_Tot_LA_Current_Num == 99.99);
            Assert.Contains(result, a => a.Attainment8_Tot_LA_Current_Num == 90.00);
        }

        [Fact]
        public async Task GetAllLAPerformanceAsync_ShouldReturnEmpty_WhenNoData()
        {
            // Arrange
            _mockRepo
                .Setup(r => r.GetAllLAPerformanceAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<LAPerformance>());

            // Act
            var result = await _service.GetAllLAPerformanceAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetLAPerformanceAsync_ShouldReturnCorrectItem_WhenLaCodeExists()
        {
            // Arrange
            var laCode = "100";
            var expected = new LAPerformance { Id = laCode, Attainment8_Tot_LA_Current_Num = 100 };

            _mockRepo
                .Setup(r => r.GetLAPerformanceAsync(laCode, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.GetLAPerformanceAsync(laCode, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(laCode, result.Id);
            Assert.Equal(100, result.Attainment8_Tot_LA_Current_Num);
        }

        [Fact]
        public async Task GetLAPerformanceAsync_ShouldReturnDefault_WhenLaCodeDoesNotExist()
        {
            // Arrange
            var laCode = "99999";

            _mockRepo
                .Setup(r => r.GetLAPerformanceAsync(laCode, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new LAPerformance());

            // Act
            var result = await _service.GetLAPerformanceAsync(laCode, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Attainment8_Tot_LA_Current_Num);
        }

        [Fact]
        public async Task GetLAPerformanceAsync_ShouldPropagateException_WhenRepositoryThrows()
        {
            // Arrange
            var laCode = "error";

            _mockRepo
                .Setup(r => r.GetLAPerformanceAsync(laCode, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.GetLAPerformanceAsync(laCode, CancellationToken.None));
            Assert.Equal("Database error", ex.Message);
        }
    }
}
