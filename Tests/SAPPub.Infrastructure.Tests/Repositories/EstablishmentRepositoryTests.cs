using Microsoft.Extensions.Logging;
using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Infrastructure.Repositories;

namespace SAPPub.Infrastructure.Tests.Repositories
{
    public class EstablishmentRepositoryTests
    {
        private readonly Mock<IGenericRepository<Establishment>> _mockGenericRepo;
        private readonly Mock<ILogger<EstablishmentRepository>> _mockLogger;
        private readonly EstablishmentRepository _sut;

        public EstablishmentRepositoryTests()
        {
            _mockGenericRepo = new Mock<IGenericRepository<Establishment>>();
            _mockLogger = new Mock<ILogger<EstablishmentRepository>>();
            _sut = new EstablishmentRepository(_mockGenericRepo.Object);
        }

        [Fact]
        public async Task GetEstablishmentAsync_ReturnsCorrectItemWhenUrnExists()
        {
            // Arrange
            var urn = "123";
            var expected = new Establishment { URN = urn, EstablishmentName = "Found" };

            _mockGenericRepo
                .Setup(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _sut.GetEstablishmentAsync(urn, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(urn, result.URN);
            Assert.Equal("Found", result.EstablishmentName);

            _mockGenericRepo.Verify(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetEstablishmentAsync_ReturnsNullWhenUrnDoesNotExist()
        {
            // Arrange
            var urn = "999";

            _mockGenericRepo
                .Setup(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Establishment?)null);

            // Act
            var result = await _sut.GetEstablishmentAsync(urn, CancellationToken.None);

            // Assert
            Assert.Null(result);

            _mockGenericRepo.Verify(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
