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
    public class GatewayUserAuditRepositoryTests
    {
        [Fact]
        public async Task InsertAsync_CallsWriteAsyncWithProvidedUser_ReturnsTrue()
        {
            // Arrange
            var user = new GatewayUserAudit { Id = Guid.NewGuid() };
            var genericRepoMock = new Mock<IGenericRepository<GatewayUserAudit>>(MockBehavior.Strict);
            genericRepoMock
                .Setup(r => r.WriteAsync(It.Is<GatewayUserAudit>(g => object.ReferenceEquals(g, user)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .Verifiable();

            var loggerMock = new Mock<ILogger<Microsoft.Extensions.Logging.Abstractions.NullLogger>>();
            var loggerForTest = new Mock<ILogger<SAPPub.Core.Entities.Establishment>>().Object;

            var repository = new GatewayUserAuditRepository(genericRepoMock.Object, loggerForTest);

            // Act
            var result = await repository.InsertAsync(user);

            // Assert
            Assert.True(result);
            genericRepoMock.Verify(r => r.WriteAsync(It.Is<GatewayUserAudit>(g => object.ReferenceEquals(g, user)), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task InsertAsync_ReturnsFalse_WhenRepositoryReturnsFalse()
        {
            // Arrange
            var user = new GatewayUserAudit { Id = Guid.NewGuid() };
            var genericRepoMock = new Mock<IGenericRepository<GatewayUserAudit>>();
            genericRepoMock
                .Setup(r => r.WriteAsync(It.IsAny<GatewayUserAudit>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var loggerForTest = new Mock<ILogger<SAPPub.Core.Entities.Establishment>>().Object;
            var repository = new GatewayUserAuditRepository(genericRepoMock.Object, loggerForTest);

            // Act
            var result = await repository.InsertAsync(user);

            // Assert
            Assert.False(result);
            genericRepoMock.Verify(r => r.WriteAsync(It.Is<GatewayUserAudit>(g => object.ReferenceEquals(g, user)), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task InsertAsync_PropagatesException_WhenRepositoryThrows()
        {
            // Arrange
            var user = new GatewayUserAudit { Id = Guid.NewGuid() };
            var expected = new InvalidOperationException("write failed");

            var genericRepoMock = new Mock<IGenericRepository<GatewayUserAudit>>();
            genericRepoMock
                .Setup(r => r.WriteAsync(It.IsAny<GatewayUserAudit>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(expected);

            var loggerForTest = new Mock<ILogger<SAPPub.Core.Entities.Establishment>>().Object;
            var repository = new GatewayUserAuditRepository(genericRepoMock.Object, loggerForTest);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => repository.InsertAsync(user));
            Assert.Same(expected, ex);
            genericRepoMock.Verify(r => r.WriteAsync(It.Is<GatewayUserAudit>(g => object.ReferenceEquals(g, user)), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
