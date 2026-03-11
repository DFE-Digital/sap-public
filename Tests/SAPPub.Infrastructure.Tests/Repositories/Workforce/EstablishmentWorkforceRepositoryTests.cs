using Microsoft.Extensions.Logging;
using Moq;
using SAPPub.Core.Entities.KS4.Workforce;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Infrastructure.Repositories.KS4.Workforce;

namespace SAPPub.Infrastructure.Tests.Repositories.Workforce
{
    public class EstablishmentWorkforceRepositoryTests
    {
        private readonly Mock<IGenericRepository<EstablishmentWorkforce>> _mockGenericRepo;
        private readonly Mock<ILogger<EstablishmentWorkforceRepository>> _mockLogger;
        private readonly EstablishmentWorkforceRepository _sut;

        public EstablishmentWorkforceRepositoryTests()
        {
            _mockGenericRepo = new Mock<IGenericRepository<EstablishmentWorkforce>>();
            _mockLogger = new Mock<ILogger<EstablishmentWorkforceRepository>>();
            _sut = new EstablishmentWorkforceRepository(_mockGenericRepo.Object);
        }

        [Fact]
        public async Task GetAllEstablishmentWorkforceAsync_ReturnsAllItemsFromGenericRepository()
        {
            // Arrange
            var expected = new List<EstablishmentWorkforce>
            {
                new() { Id = "1", Workforce_PupTeaRatio_Est_Current_Num = 99.99 },
                new() { Id = "2", Workforce_PupTeaRatio_Est_Current_Num = 88.88 }
            };

            _mockGenericRepo
                .Setup(r => r.ReadAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _sut.GetAllEstablishmentWorkforceAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, e => e.Id == "1");
            Assert.Contains(result, e => e.Id == "2");

            _mockGenericRepo.Verify(r => r.ReadAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAllEstablishmentWorkforceAsync_ReturnsEmptyWhenGenericRepositoryReturnsEmpty()
        {
            // Arrange
            _mockGenericRepo
                .Setup(r => r.ReadAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<EstablishmentWorkforce>());

            // Act
            var result = await _sut.GetAllEstablishmentWorkforceAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _mockGenericRepo.Verify(r => r.ReadAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetEstablishmentWorkforceAsync_ReturnsCorrectItemWhenUrnExists()
        {
            // Arrange
            var urn = "1";
            var expected = new EstablishmentWorkforce
            {
                Id = urn,
                Workforce_PupTeaRatio_Est_Current_Num = 99.99
            };

            _mockGenericRepo
                .Setup(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _sut.GetEstablishmentWorkforceAsync(urn, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1", result.Id);
            Assert.Equal(99.99, result.Workforce_PupTeaRatio_Est_Current_Num);

            _mockGenericRepo.Verify(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetEstablishmentWorkforceAsync_ReturnsNewEstablishmentWorkforceWhenUrnDoesNotExist()
        {
            // Arrange
            var urn = "999";

            _mockGenericRepo
                .Setup(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync((EstablishmentWorkforce?)null);

            // Act
            var result = await _sut.GetEstablishmentWorkforceAsync(urn, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(string.Empty, result.Id);

            _mockGenericRepo.Verify(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
