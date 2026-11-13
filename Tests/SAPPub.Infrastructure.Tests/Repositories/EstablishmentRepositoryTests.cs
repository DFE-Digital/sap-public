using Moq;
using Npgsql;
using SAPPub.Core.Entities;
using SAPPub.Core.Helpers;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Services.Search;
using SAPPub.Core.ServiceModels.Search.InputModels;
using SAPPub.Core.Specifications;
using SAPPub.Infrastructure.Repositories;

namespace SAPPub.Infrastructure.Tests.Repositories
{
    public class EstablishmentRepositoryTests
    {
        private readonly Mock<IGenericRepository<Establishment>> _mockGenericRepo;
        private readonly EstablishmentRepository _sut;

        public EstablishmentRepositoryTests()
        {
            _mockGenericRepo = new Mock<IGenericRepository<Establishment>>();
            _sut = new EstablishmentRepository(_mockGenericRepo.Object);
        }

        [Fact]
        public async Task GetEstablishmentAsync_ReturnsCorrectItemWhenUrnExists()
        {
            var urn = "123";
            var expected = new Establishment { URN = urn, EstablishmentName = "Found" };

            _mockGenericRepo
                .Setup(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            var result = await _sut.GetEstablishmentAsync(urn, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(urn, result.URN);
            Assert.Equal("Found", result.EstablishmentName);

            _mockGenericRepo.Verify(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetEstablishmentAsync_ReturnsNullWhenUrnDoesNotExist()
        {
            var urn = "999";

            _mockGenericRepo
                .Setup(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Establishment?)null);

            var result = await _sut.GetEstablishmentAsync(urn, CancellationToken.None);

            Assert.Null(result);
            _mockGenericRepo.Verify(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetEstablishmentsAsync_ReturnsCorrectItemsWhenUrnExists()
        {
            // Arrange
            var urns = new List<string> {"123","234"};
            var expectedEstablishments = new List<Establishment> 
            {
                new() { URN = "123", EstablishmentName = "Urn1" },
                new() { URN = "234", EstablishmentName = "Urn2" }
            };
            
            _mockGenericRepo
                .Setup(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEstablishments);

            // Act
            var results = await _sut.GetEstablishmentsAsync(urns, CancellationToken.None);

            // Assert
            Assert.NotNull(results);
            Assert.Equal(2, results.Count());

            foreach (var expectedEstablishment in expectedEstablishments)
            {
                var actualEstablishment = results.FirstOrDefault(r => r.URN == expectedEstablishment.URN);
                Assert.NotNull(actualEstablishment);
                Assert.Equal(expectedEstablishment.URN, actualEstablishment.URN);
                Assert.Equal(expectedEstablishment.EstablishmentName, actualEstablishment.EstablishmentName);
            }      

            _mockGenericRepo.Verify(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetEstablishmentsAsync_ReturnsEmptyWhenUrnDoesNotExist()
        {
            // Arrange
            var urns = new List<string> { "123", "234" };
            var expectedEstablishments = new List<Establishment>();

            _mockGenericRepo
                .Setup(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEstablishments);

            // Act
            var result = await _sut.GetEstablishmentsAsync(urns, CancellationToken.None);

            // Assert
            Assert.Empty(result!);

            _mockGenericRepo.Verify(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetEstablishmentsAsync_ReturnsNullWhenUrnsIsNUll()
        {
            // Arrange
            List<string> urns = null!;

            // Act
            var result = await _sut.GetEstablishmentsAsync(urns, CancellationToken.None);

            // Assert
            Assert.Null(result);

            _mockGenericRepo.Verify(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}