using Microsoft.Extensions.Logging;
using Moq;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Infrastructure.Repositories.KS4.Destinations;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SAPPub.Infrastructure.Tests.Repositories.Destinations
{
    public class EstablishmentDestinationsRepositoryTests
    {
        private readonly Mock<IGenericRepository<EstablishmentDestinations>> _mockGenericRepo;
        private readonly Mock<ILogger<EstablishmentDestinationsRepository>> _mockLogger;
        private readonly EstablishmentDestinationsRepository _sut;

        public EstablishmentDestinationsRepositoryTests()
        {
            _mockGenericRepo = new Mock<IGenericRepository<EstablishmentDestinations>>();
            _mockLogger = new Mock<ILogger<EstablishmentDestinationsRepository>>();
            _sut = new EstablishmentDestinationsRepository(_mockGenericRepo.Object);
        }

        [Fact]
        public async Task GetAllEstablishmentDestinationsAsync_ReturnsAllItemsFromGenericRepository()
        {
            // Arrange
            var expected = new List<EstablishmentDestinations>
            {
                new() { Id = "1", AllDest_Tot_Est_Current_Pct = 99.99 },
                new() { Id = "2", AllDest_Tot_Est_Current_Pct = 88.88 }
            };

            _mockGenericRepo
                .Setup(r => r.ReadAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _sut.GetAllEstablishmentDestinationsAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, e => e.Id == "1");
            Assert.Contains(result, e => e.Id == "2");

            _mockGenericRepo.Verify(r => r.ReadAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAllEstablishmentDestinationsAsync_ReturnsEmptyWhenGenericRepositoryReturnsEmpty()
        {
            // Arrange
            _mockGenericRepo
                .Setup(r => r.ReadAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<EstablishmentDestinations>());

            // Act
            var result = await _sut.GetAllEstablishmentDestinationsAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _mockGenericRepo.Verify(r => r.ReadAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetEstablishmentDestinationsAsync_ReturnsCorrectItemWhenUrnExists()
        {
            // Arrange
            var urn = "1";
            var expected = new EstablishmentDestinations { Id = urn, AllDest_Tot_Est_Current_Pct = 99.99 };

            _mockGenericRepo
                .Setup(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _sut.GetEstablishmentDestinationsAsync(urn, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1", result.Id);
            Assert.Equal(99.99, result.AllDest_Tot_Est_Current_Pct);

            _mockGenericRepo.Verify(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetEstablishmentDestinationsAsync_ReturnsNullWhenUrnDoesNotExist()
        {
            // Arrange
            var urn = "999";

            _mockGenericRepo
                .Setup(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync((EstablishmentDestinations?)null);

            // Act
            var result = await _sut.GetEstablishmentDestinationsAsync(urn, CancellationToken.None);

            // Assert
            Assert.Null(result);

            _mockGenericRepo.Verify(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
