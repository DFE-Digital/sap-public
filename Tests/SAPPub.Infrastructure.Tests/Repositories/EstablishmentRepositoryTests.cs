using Microsoft.Extensions.Logging;
using Moq;
using Npgsql;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Infrastructure.Repositories;
using System.Data;


namespace SAPPub.Infrastructure.Tests.Repositories
{
    public class EstablishmentRepositoryTests
    {
        private readonly Mock<IGenericRepository<Establishment>> _mockGenericRepo;
        private readonly Mock<ILogger<EstablishmentRepository>> _mockLogger;
        private readonly EstablishmentRepository _sut;

        private static NpgsqlDataSource CreateSafeDataSource()
        {
            // Safe because our tests below do not actually open a connection
            // (we target early return / missing SQL branches).
            return NpgsqlDataSource.Create("Host=127.0.0.1;Port=1;Username=x;Password=x;Database=x;Timeout=1;Command Timeout=1");
        }

        public EstablishmentRepositoryTests()
        {
            _mockGenericRepo = new Mock<IGenericRepository<Establishment>>();
            _mockLogger = new Mock<ILogger<EstablishmentRepository>>();
            _sut = new EstablishmentRepository(_mockGenericRepo.Object, CreateSafeDataSource());
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
