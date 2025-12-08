using Microsoft.Extensions.Logging;
using Moq;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Infrastructure.Repositories.KS4.Performance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Infrastructure.Tests.Repositories.Performance
{
    public class EstablishmentPerformanceRepositoryTests
    {
        private readonly Mock<IGenericRepository<EstablishmentPerformance>> _mockGenericRepo;
        private readonly Mock<ILogger<EstablishmentPerformance>> _mockLogger;
        private readonly EstablishmentPerformanceRepository _sut;

        public EstablishmentPerformanceRepositoryTests()
        {
            _mockGenericRepo = new Mock<IGenericRepository<EstablishmentPerformance>>();
            _mockLogger = new Mock<ILogger<EstablishmentPerformance>>();
            _sut = new EstablishmentPerformanceRepository(_mockGenericRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public void GetAllEstablishmentPerformances_ReturnsAllItemsFromGenericRepository()
        {
            // Arrange
            var expected = new List<EstablishmentPerformance>
            {
                new EstablishmentPerformance { Id = "1", Attainment8_Tot_Est_Current_Num= 99.99 },
                new EstablishmentPerformance { Id = "2", Attainment8_Tot_Est_Current_Num = 88.88 }
            };
            _mockGenericRepo.Setup(r => r.ReadAll()).Returns(expected);

            // Act
            var result = _sut.GetAllEstablishmentPerformance();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, e => e.Id == "1");
            Assert.Contains(result, e => e.Id == "2");
            _mockGenericRepo.Verify(r => r.ReadAll(), Times.Once);
        }

        [Fact]
        public void GetAllEstablishmentPerformances_ReturnsEmptyWhenGenericRepositoryReturnsNull()
        {
            // Arrange
            _mockGenericRepo.Setup(r => r.ReadAll()).Returns((IEnumerable<EstablishmentPerformance>?)null);

            // Act
            var result = _sut.GetAllEstablishmentPerformance();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockGenericRepo.Verify(r => r.ReadAll(), Times.Once);
        }

        [Fact]
        public void GetEstablishmentPerformance_ReturnsCorrectItemWhenUrnExists()
        {
            // Arrange
            var expected = new EstablishmentPerformance { Id = "1", Attainment8_Tot_Est_Current_Num = 99.99 };
            _mockGenericRepo.Setup(r => r.ReadAll()).Returns(new[] { expected });

            // Act
            var result = _sut.GetEstablishmentPerformance("1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1", result.Id);
            Assert.Equal(99.99, result.Attainment8_Tot_Est_Current_Num);
            _mockGenericRepo.Verify(r => r.ReadAll(), Times.Once);
        }

        [Fact]
        public void GetEstablishmentPerformance_ReturnsNewEstablishmentPerformanceWhenUrnDoesNotExist()
        {
            // Arrange
            _mockGenericRepo.Setup(r => r.ReadAll()).Returns(Enumerable.Empty<EstablishmentPerformance>());

            // Act
            var result = _sut.GetEstablishmentPerformance("999");

            // Assert
            Assert.NotNull(result);
            // When not found the repository returns a new EstablishmentPerformance (defaults are empty strings / zeros)
            Assert.Equal(string.Empty, result.Id);
            _mockGenericRepo.Verify(r => r.ReadAll(), Times.Once);
        }
    }
}
