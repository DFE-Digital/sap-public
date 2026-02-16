using Microsoft.Extensions.Logging;
using Moq;
using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Infrastructure.Repositories.KS4.Absence;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SAPPub.Infrastructure.Tests.Repositories.Absence
{
    public class EstablishmentAbsenceRepositoryTests
    {
        private readonly Mock<IGenericRepository<EstablishmentAbsence>> _mockGenericRepo;
        private readonly Mock<ILogger<EstablishmentAbsenceRepository>> _mockLogger;
        private readonly EstablishmentAbsenceRepository _sut;

        public EstablishmentAbsenceRepositoryTests()
        {
            _mockGenericRepo = new Mock<IGenericRepository<EstablishmentAbsence>>();
            _mockLogger = new Mock<ILogger<EstablishmentAbsenceRepository>>();

            _sut = new EstablishmentAbsenceRepository(_mockGenericRepo.Object);
        }

        [Fact]
        public async Task GetAllEstablishmentAbsenceAsync_ReturnsAllItemsFromGenericRepository()
        {
            // Arrange
            var expected = new List<EstablishmentAbsence>
            {
                new() { Id = "1", Abs_Tot_Est_Current_Pct = 99.99 },
                new() { Id = "2", Abs_Tot_Est_Current_Pct = 88.88 }
            };

            _mockGenericRepo
                .Setup(r => r.ReadAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _sut.GetAllEstablishmentAbsenceAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, e => e.Id == "1");
            Assert.Contains(result, e => e.Id == "2");

            _mockGenericRepo.Verify(r => r.ReadAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAllEstablishmentAbsenceAsync_ReturnsEmptyWhenGenericRepositoryReturnsEmpty()
        {
            // Arrange
            _mockGenericRepo
                .Setup(r => r.ReadAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<EstablishmentAbsence>());

            // Act
            var result = await _sut.GetAllEstablishmentAbsenceAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _mockGenericRepo.Verify(r => r.ReadAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetEstablishmentAbsenceAsync_ReturnsCorrectItemWhenUrnExists()
        {
            // Arrange
            var urn = "1";
            var expected = new EstablishmentAbsence { Id = urn, Abs_Tot_Est_Current_Pct = 99.99 };

            _mockGenericRepo
                .Setup(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _sut.GetEstablishmentAbsenceAsync(urn, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(urn, result.Id);
            Assert.Equal(99.99, result.Abs_Tot_Est_Current_Pct);

            _mockGenericRepo.Verify(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetEstablishmentAbsenceAsync_ReturnsNewEstablishmentAbsenceWhenUrnDoesNotExist()
        {
            // Arrange
            var urn = "999";

            _mockGenericRepo
                .Setup(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync((EstablishmentAbsence?)null);

            // Act
            var result = await _sut.GetEstablishmentAbsenceAsync(urn, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(string.Empty, result.Id);

            _mockGenericRepo.Verify(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
