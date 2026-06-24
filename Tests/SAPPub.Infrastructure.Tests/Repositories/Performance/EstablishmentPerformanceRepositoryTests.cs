using Moq;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Infrastructure.Repositories.KS4.Performance;

namespace SAPPub.Infrastructure.Tests.Repositories.Performance
{
    public class EstablishmentPerformanceRepositoryTests
    {
        private readonly Mock<IGenericRepository<EstablishmentPerformance>> _mockGenericRepo;
        private readonly EstablishmentPerformanceRepository _sut;

        public EstablishmentPerformanceRepositoryTests()
        {
            _mockGenericRepo = new Mock<IGenericRepository<EstablishmentPerformance>>();
            _sut = new EstablishmentPerformanceRepository(_mockGenericRepo.Object);
        }

        [Fact]
        public async Task GetAllEstablishmentPerformanceAsync_ReturnsAllItemsFromGenericRepository()
        {
            // Arrange
            var expected = new List<EstablishmentPerformance>
            {
                new() { Id = "1", Attainment8_Tot_Est_Current_Num = 99.99 },
                new() { Id = "2", Attainment8_Tot_Est_Current_Num = 88.88 }
            };

            _mockGenericRepo
                .Setup(r => r.ReadAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _sut.GetAllEstablishmentPerformanceAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, e => e.Id == "1");
            Assert.Contains(result, e => e.Id == "2");

            _mockGenericRepo.Verify(r => r.ReadAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAllEstablishmentPerformanceAsync_ReturnsEmptyWhenGenericRepositoryReturnsEmpty()
        {
            // Arrange
            _mockGenericRepo
                .Setup(r => r.ReadAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<EstablishmentPerformance>());

            // Act
            var result = await _sut.GetAllEstablishmentPerformanceAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _mockGenericRepo.Verify(r => r.ReadAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetEstablishmentPerformanceAsync_ReturnsCorrectItemWhenUrnExists()
        {
            // Arrange
            var urn = "1";
            var expected = new EstablishmentPerformance { Id = urn, Attainment8_Tot_Est_Current_Num = 99.99 };

            _mockGenericRepo
                .Setup(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _sut.GetEstablishmentPerformanceAsync(urn, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1", result.Id);
            Assert.Equal(99.99, result.Attainment8_Tot_Est_Current_Num);

            _mockGenericRepo.Verify(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetEstablishmentPerformanceAsync_ReturnsNewEstablishmentPerformanceWhenUrnDoesNotExist()
        {
            // Arrange
            var urn = "999";

            _mockGenericRepo
                .Setup(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync((EstablishmentPerformance?)null);

            // Act
            var result = await _sut.GetEstablishmentPerformanceAsync(urn, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(string.Empty, result.Id);

            _mockGenericRepo.Verify(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetEstablishmentsPerformanceAsync_ReturnsCorrectItemsWhenUrnsExists()
        {
            // Arrange
            var urns = new List<string> { "123", "234" };

            var expectedEstablishmentsPerformance = new List<EstablishmentPerformance>
            {
                new() { Id = "123", EngMaths49_Tot_Est_Current_Pct = 99.99 },
                new() { Id = "234", EngMaths49_Tot_Est_Current_Pct = 55.67 }
            };

            _mockGenericRepo
                .Setup(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEstablishmentsPerformance);

            // Act
            var results = await _sut.GetEstablishmentsPerformanceAsync(urns, CancellationToken.None);

            // Assert
            Assert.NotNull(results);
            Assert.Equal(2, results.Count());

            foreach (var expectedEstablishmentPerformance in expectedEstablishmentsPerformance)
            {
                var actualEstablishmentPerformance = results.FirstOrDefault(r => r.Id == expectedEstablishmentPerformance.Id);
                Assert.NotNull(actualEstablishmentPerformance);                
                Assert.Equal(actualEstablishmentPerformance.EngMaths49_Tot_Est_Current_Pct, expectedEstablishmentPerformance.EngMaths49_Tot_Est_Current_Pct);
            }            

            _mockGenericRepo.Verify(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetEstablishmentsPerformanceAsync_ReturnsEmptyWhenUrnDoesNotExist()
        {
            // Arrange
            var urns = new List<string> { "123", "234" };
            var expectedEstablishmentsPerformance = new List<EstablishmentPerformance>();

            _mockGenericRepo
                .Setup(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEstablishmentsPerformance);

            // Act
            var result = await _sut.GetEstablishmentsPerformanceAsync(urns, CancellationToken.None);

            // Assert
            Assert.Empty(result!);

            _mockGenericRepo.Verify(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
