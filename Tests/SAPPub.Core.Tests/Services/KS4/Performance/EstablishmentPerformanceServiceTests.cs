using Moq;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Interfaces.Repositories.KS4.Performance;
using SAPPub.Core.Services.KS4.Performance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SAPPub.Core.Tests.Services.KS4.Performance
{
    public class EstablishmentPerformanceServiceTests
    {
        private readonly Mock<IEstablishmentPerformanceRepository> _mockRepo;
        private readonly EstablishmentPerformanceService _service;

        public EstablishmentPerformanceServiceTests()
        {
            _mockRepo = new Mock<IEstablishmentPerformanceRepository>();
            _service = new EstablishmentPerformanceService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllEstablishmentPerformanceAsync_ShouldReturnAllItems()
        {
            // Arrange
            var expected = new List<EstablishmentPerformance>
            {
                new() { Id = "100", Attainment8_Tot_Est_Current_Num = 99.99 },
                new() { Id = "101", Attainment8_Tot_Est_Current_Num = 90.00 }
            };

            _mockRepo
                .Setup(r => r.GetAllEstablishmentPerformanceAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.GetAllEstablishmentPerformanceAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, a => a.Attainment8_Tot_Est_Current_Num == 99.99);
            Assert.Contains(result, a => a.Attainment8_Tot_Est_Current_Num == 90.00);
        }

        [Fact]
        public async Task GetAllEstablishmentPerformanceAsync_ShouldReturnEmpty_WhenNoData()
        {
            // Arrange
            _mockRepo
                .Setup(r => r.GetAllEstablishmentPerformanceAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<EstablishmentPerformance>());

            // Act
            var result = await _service.GetAllEstablishmentPerformanceAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetEstablishmentPerformanceAsync_ShouldReturnCorrectItem_WhenUrnExists()
        {
            // Arrange
            var urn = "100";
            var expected = new EstablishmentPerformance { Id = urn, Attainment8_Tot_Est_Current_Num = 100 };

            _mockRepo
                .Setup(r => r.GetEstablishmentPerformanceAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.GetEstablishmentPerformanceAsync(urn, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(urn, result.Id);
            Assert.Equal(100, result.Attainment8_Tot_Est_Current_Num);
        }

        [Fact]
        public async Task GetEstablishmentPerformanceAsync_ShouldReturnDefault_WhenUrnDoesNotExist()
        {
            // Arrange
            var urn = "99999";

            _mockRepo
                .Setup(r => r.GetEstablishmentPerformanceAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new EstablishmentPerformance());

            // Act
            var result = await _service.GetEstablishmentPerformanceAsync(urn, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Attainment8_Tot_Est_Current_Num);
        }

        [Fact]
        public async Task GetEstablishmentPerformanceAsync_ShouldPropagateException_WhenRepositoryThrows()
        {
            // Arrange
            var urn = "error";

            _mockRepo
                .Setup(r => r.GetEstablishmentPerformanceAsync(urn, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.GetEstablishmentPerformanceAsync(urn, CancellationToken.None));
            Assert.Equal("Database error", ex.Message);
        }
    }
}
