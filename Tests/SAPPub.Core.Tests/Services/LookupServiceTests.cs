using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Services;
using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Tests.Services
{
    [ExcludeFromCodeCoverage]
    public class LookupServiceTests
    {
        private readonly Mock<ILookupRepository> _mockRepo;
        private readonly LookupService _service;

        public LookupServiceTests()
        {
            _mockRepo = new Mock<ILookupRepository>();
            _service = new LookupService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllLookupsAsync_ShouldReturnAllItems()
        {
            // Arrange
            var expected = new List<Lookup>
            {
                new() { Id = "100", Name = "Test One", LookupType = "Testing" },
                new() { Id = "101", Name = "Test Two", LookupType = "Testing" },
            };

            _mockRepo
                .Setup(r => r.GetAllLookupsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.GetAllLookupsAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, a => a.Name == "Test One");
            Assert.Contains(result, a => a.Name == "Test Two");
        }

        [Fact]
        public async Task GetAllLookupsAsync_ShouldReturnEmpty_WhenNoData()
        {
            // Arrange
            _mockRepo
                .Setup(r => r.GetAllLookupsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Lookup>());

            // Act
            var result = await _service.GetAllLookupsAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetLookupAsync_ShouldReturnCorrectItem_WhenIdExists()
        {
            // Arrange
            var id = "100";
            var expected = new Lookup { Id = id, Name = "Test One", LookupType = "Testing" };

            _mockRepo
                .Setup(r => r.GetLookupAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.GetLookupAsync(id, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal("Test One", result.Name);
            Assert.Equal("Testing", result.LookupType);
        }

        [Fact]
        public async Task GetLookupAsync_ShouldReturnDefault_WhenIdDoesNotExist()
        {
            // Arrange
            var id = "99999";

            _mockRepo
                .Setup(r => r.GetLookupAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Lookup?)null);

            // Act
            var result = await _service.GetLookupAsync(id, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(string.Empty, result.Name);
        }

        [Fact]
        public async Task GetLookupAsync_ShouldReturnDefault_WhenIdIsNullOrWhitespace()
        {
            // Act
            var result = await _service.GetLookupAsync("   ", CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(string.Empty, result.Name);

            // Verify repo not called
            _mockRepo.Verify(r => r.GetLookupAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task GetLookupAsync_ShouldPropagateException_WhenRepositoryThrows()
        {
            // Arrange
            var id = "error";

            _mockRepo
                .Setup(r => r.GetLookupAsync(id, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Database error"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<System.Exception>(() => _service.GetLookupAsync(id, CancellationToken.None));
            Assert.Equal("Database error", ex.Message);
        }
    }
}
