using Microsoft.Extensions.Logging;
using Moq;
using SAPPub.Core.Entities.Gateway;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Infrastructure.Repositories.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Infrastructure.Tests.Repositories.Gateway
{
    public class GatewayUserRepositoryTests
    {
        [Fact]
        public async Task GetByIdAsync_ReturnsEntity_WhenFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expected = new GatewayUser { Id = id };

            var genericRepoMock = new Mock<IGenericRepository<GatewayUser>>(MockBehavior.Strict);
            genericRepoMock
                .Setup(r => r.ReadSingleAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected)
                .Verifiable();

            var logger = new Mock<ILogger<SAPPub.Core.Entities.Establishment>>().Object;
            var repository = new GatewayUserRepository(genericRepoMock.Object, logger);

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

            var genericRepoMock = new Mock<IGenericRepository<GatewayUser>>();
            genericRepoMock
                .Setup(r => r.ReadSingleAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GatewayUser?)null);

            var logger = new Mock<ILogger<SAPPub.Core.Entities.Establishment>>().Object;
            var repository = new GatewayUserRepository(genericRepoMock.Object, logger);

            // Act
            var result = await repository.GetByIdAsync(id, CancellationToken.None);

            // Assert
            Assert.Null(result);
            genericRepoMock.Verify(r => r.ReadSingleAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task InsertAsync_ReturnsTrue_WhenRepositoryReturnsTrue()
        {
            // Arrange
            var user = new GatewayUser { Id = Guid.NewGuid() };
            var genericRepoMock = new Mock<IGenericRepository<GatewayUser>>(MockBehavior.Strict);
            genericRepoMock
                .Setup(r => r.WriteAsync(It.Is<GatewayUser>(g => ReferenceEquals(g, user)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .Verifiable();

            var logger = new Mock<ILogger<SAPPub.Core.Entities.Establishment>>().Object;
            var repository = new GatewayUserRepository(genericRepoMock.Object, logger);

            // Act
            var result = await repository.InsertAsync(user);

            // Assert
            Assert.True(result);
            genericRepoMock.Verify(r => r.WriteAsync(It.Is<GatewayUser>(g => ReferenceEquals(g, user)), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task InsertAsync_ReturnsFalse_WhenRepositoryReturnsFalse()
        {
            // Arrange
            var user = new GatewayUser { Id = Guid.NewGuid() };
            var genericRepoMock = new Mock<IGenericRepository<GatewayUser>>();
            genericRepoMock
                .Setup(r => r.WriteAsync(It.IsAny<GatewayUser>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var logger = new Mock<ILogger<SAPPub.Core.Entities.Establishment>>().Object;
            var repository = new GatewayUserRepository(genericRepoMock.Object, logger);

            // Act
            var result = await repository.InsertAsync(user);

            // Assert
            Assert.False(result);
            genericRepoMock.Verify(r => r.WriteAsync(It.Is<GatewayUser>(g => ReferenceEquals(g, user)), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task InsertAsync_PropagatesException_WhenRepositoryThrows()
        {
            // Arrange
            var user = new GatewayUser { Id = Guid.NewGuid() };
            var expected = new InvalidOperationException("write failed");

            var genericRepoMock = new Mock<IGenericRepository<GatewayUser>>();
            genericRepoMock
                .Setup(r => r.WriteAsync(It.IsAny<GatewayUser>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(expected);

            var logger = new Mock<ILogger<SAPPub.Core.Entities.Establishment>>().Object;
            var repository = new GatewayUserRepository(genericRepoMock.Object, logger);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => repository.InsertAsync(user));
            Assert.Same(expected, ex);
            genericRepoMock.Verify(r => r.WriteAsync(It.Is<GatewayUser>(g => ReferenceEquals(g, user)), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAll_WhenRepositoryReturnsValues()
        {
            // Arrange
            var list = new[]
            {
                new GatewayUser { Id = Guid.NewGuid() },
                new GatewayUser { Id = Guid.NewGuid() }
            };

            var genericRepoMock = new Mock<IGenericRepository<GatewayUser>>();
            genericRepoMock
                .Setup(r => r.ReadAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<GatewayUser>)list);

            var logger = new Mock<ILogger<SAPPub.Core.Entities.Establishment>>().Object;
            var repository = new GatewayUserRepository(genericRepoMock.Object, logger);

            // Act
            var result = await repository.GetAllAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            genericRepoMock.Verify(r => r.ReadAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsNull_WhenRepositoryReturnsNull()
        {
            // Arrange
            var genericRepoMock = new Mock<IGenericRepository<GatewayUser>>();
            genericRepoMock
                .Setup(r => r.ReadAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<GatewayUser>)[]);

            var logger = new Mock<ILogger<SAPPub.Core.Entities.Establishment>>().Object;
            var repository = new GatewayUserRepository(genericRepoMock.Object, logger);

            // Act
            var result = await repository.GetAllAsync(CancellationToken.None);

            // Assert
            Assert.Empty(result);
            genericRepoMock.Verify(r => r.ReadAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
