using Microsoft.Extensions.Logging;
using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SAPPub.Infrastructure.Tests.Repositories
{
    public class LookupRepositoryTests
    {
        private readonly Mock<IGenericRepository<Lookup>> _mockGenericRepo;
        private readonly Mock<ILogger<LookupRepository>> _mockLogger;
        private readonly LookupRepository _sut;

        public LookupRepositoryTests()
        {
            _mockGenericRepo = new Mock<IGenericRepository<Lookup>>();
            _mockLogger = new Mock<ILogger<LookupRepository>>();
            _sut = new LookupRepository(_mockGenericRepo.Object);
        }

        [Fact]
        public async Task GetAllLookupsAsync_ReturnsAllItemsFromGenericRepository()
        {
            // Arrange
            var expected = new List<Lookup>
            {
                new Lookup { Id = "1", Name = "One", LookupType = "Lookup" },
                new Lookup { Id = "2", Name = "Two", LookupType = "Type" }
            };

            _mockGenericRepo
                .Setup(r => r.ReadAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _sut.GetAllLookupsAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, e => e.Id == "1");
            Assert.Contains(result, e => e.Id == "2");

            _mockGenericRepo.Verify(r => r.ReadAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAllLookupsAsync_ReturnsEmptyWhenGenericRepositoryReturnsEmpty()
        {
            // Arrange
            _mockGenericRepo
                .Setup(r => r.ReadAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<Lookup>());

            // Act
            var result = await _sut.GetAllLookupsAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _mockGenericRepo.Verify(r => r.ReadAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetLookupAsync_ReturnsCorrectItemWhenIdExists()
        {
            // Arrange
            var id = "1";
            var expected = new Lookup { Id = id, Name = "One", LookupType = "Lookup" };

            _mockGenericRepo
                .Setup(r => r.ReadAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _sut.GetLookupAsync(id, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1", result!.Id);
            Assert.Equal("One", result.Name);

            _mockGenericRepo.Verify(r => r.ReadAsync(id, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetLookupAsync_ReturnsNullWhenIdDoesNotExist()
        {
            // Arrange
            var id = "999";

            _mockGenericRepo
                .Setup(r => r.ReadAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Lookup?)null);

            // Act
            var result = await _sut.GetLookupAsync(id, CancellationToken.None);

            // Assert
            Assert.Null(result);

            _mockGenericRepo.Verify(r => r.ReadAsync(id, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
