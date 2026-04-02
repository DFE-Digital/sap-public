using Microsoft.Extensions.Logging;
using Moq;
using SAPPub.Core.Entities.Gateway;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Infrastructure.Repositories.Gateway;

namespace SAPPub.Infrastructure.Tests.Repositories.Gateway
{
    public class GatewayLocalAuthorityRepositoryTests
    {
        [Fact]
        public async Task GetByIdAsync_ReturnsEntity_WhenFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expected = new GatewayLocalAuthority { Id = id };

            var genericRepoMock = new Mock<IGenericRepository<GatewayLocalAuthority>>(MockBehavior.Strict);
            genericRepoMock
                .Setup(r => r.ReadSingleAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected)
                .Verifiable();

            var loggerMock = new Mock<ILogger<Microsoft.Extensions.Logging.Abstractions.NullLogger>>(MockBehavior.Loose);
            // The repository expects an ILogger<Establishment> in the source; tests only need a logger instance.
            var loggerForTest = new Mock<ILogger<Core.Entities.Establishment>>().Object;

            var repository = new GatewayLocalAuthorityRepository(genericRepoMock.Object, loggerForTest);

            // Act
            var result = await repository.GetByIdAsync(id, CancellationToken.None);

            // Assert
            Assert.Same(expected, result);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();

            var genericRepoMock = new Mock<IGenericRepository<GatewayLocalAuthority>>();
            genericRepoMock
                .Setup(r => r.ReadSingleAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GatewayLocalAuthority?)null);

            var loggerForTest = new Mock<ILogger<Core.Entities.Establishment>>().Object;
            var repository = new GatewayLocalAuthorityRepository(genericRepoMock.Object, loggerForTest);

            // Act
            var result = await repository.GetByIdAsync(id, CancellationToken.None);

            // Assert
            Assert.Null(result);
            genericRepoMock.Verify(r => r.ReadSingleAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAll_WhenRepositoryReturnsValues()
        {
            // Arrange
            var list = new[]
            {
                new GatewayLocalAuthority { Id = Guid.NewGuid() },
                new GatewayLocalAuthority { Id = Guid.NewGuid() }
            };

            var genericRepoMock = new Mock<IGenericRepository<GatewayLocalAuthority>>();
            genericRepoMock
                .Setup(r => r.ReadAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(list.AsEnumerable());

            var loggerForTest = new Mock<ILogger<Core.Entities.Establishment>>().Object;
            var repository = new GatewayLocalAuthorityRepository(genericRepoMock.Object, loggerForTest);

            // Act
            var result = await repository.GetAllAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            genericRepoMock.Verify(r => r.ReadAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsEmpty_WhenRepositoryReturnsNull()
        {
            // Arrange
            var genericRepoMock = new Mock<IGenericRepository<GatewayLocalAuthority>>();
            genericRepoMock
                .Setup(r => r.ReadAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<GatewayLocalAuthority>)[]);

            var loggerForTest = new Mock<ILogger<Core.Entities.Establishment>>().Object;
            var repository = new GatewayLocalAuthorityRepository(genericRepoMock.Object, loggerForTest);

            // Act
            var result = await repository.GetAllAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            genericRepoMock.Verify(r => r.ReadAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Constructor_Throws_WhenGenericRepositoryIsNull()
        {
            // Arrange
            var loggerForTest = new Mock<ILogger<Core.Entities.Establishment>>().Object;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new GatewayLocalAuthorityRepository(null!, loggerForTest));
        }

        [Fact]
        public void Constructor_Throws_WhenLoggerIsNull()
        {
            // Arrange
            var genericRepoMock = new Mock<IGenericRepository<GatewayLocalAuthority>>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new GatewayLocalAuthorityRepository(genericRepoMock.Object, null!));
        }
    }
}