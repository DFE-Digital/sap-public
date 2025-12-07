using Microsoft.Extensions.Logging;
using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Infrastructure.Repositories.KS4.Absence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Infrastructure.Tests.Repositories.Absence
{
    public class EstablishmentAbsenceRepositoryTests
    {
        private readonly Mock<IGenericRepository<EstablishmentAbsence>> _mockGenericRepo;
        private readonly Mock<ILogger<EstablishmentAbsence>> _mockLogger;
        private readonly EstablishmentAbsenceRepository _sut;

        public EstablishmentAbsenceRepositoryTests()
        {
            _mockGenericRepo = new Mock<IGenericRepository<EstablishmentAbsence>>();
            _mockLogger = new Mock<ILogger<EstablishmentAbsence>>();
            _sut = new EstablishmentAbsenceRepository(_mockGenericRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public void GetAllEstablishmentAbsences_ReturnsAllItemsFromGenericRepository()
        {
            // Arrange
            var expected = new List<EstablishmentAbsence>
            {
                new() { Id = "1", Abs_Tot_Est_Current_Pct= 99.99 },
                new() { Id = "2", Abs_Tot_Est_Current_Pct = 88.88 }
            };
            _mockGenericRepo.Setup(r => r.ReadAll()).Returns(expected);

            // Act
            var result = _sut.GetAllEstablishmentAbsence();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, e => e.Id == "1");
            Assert.Contains(result, e => e.Id == "2");
            _mockGenericRepo.Verify(r => r.ReadAll(), Times.Once);
        }

        [Fact]
        public void GetAllEstablishmentAbsences_ReturnsEmptyWhenGenericRepositoryReturnsNull()
        {
            // Arrange
            _mockGenericRepo.Setup(r => r.ReadAll()).Returns((IEnumerable<EstablishmentAbsence>?)null);

            // Act
            var result = _sut.GetAllEstablishmentAbsence();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockGenericRepo.Verify(r => r.ReadAll(), Times.Once);
        }

        [Fact]
        public void GetEstablishmentAbsence_ReturnsCorrectItemWhenUrnExists()
        {
            // Arrange
            var expected = new EstablishmentAbsence { Id = "1", Abs_Tot_Est_Current_Pct = 99.99 };
            _mockGenericRepo.Setup(r => r.ReadAll()).Returns(new[] { expected });

            // Act
            var result = _sut.GetEstablishmentAbsence("1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1", result.Id);
            Assert.Equal(99.99, result.Abs_Tot_Est_Current_Pct);
            _mockGenericRepo.Verify(r => r.ReadAll(), Times.Once);
        }

        [Fact]
        public void GetEstablishmentAbsence_ReturnsNewEstablishmentAbsenceWhenUrnDoesNotExist()
        {
            // Arrange
            _mockGenericRepo.Setup(r => r.ReadAll()).Returns([]);

            // Act
            var result = _sut.GetEstablishmentAbsence("999");

            // Assert
            Assert.NotNull(result);
            // When not found the repository returns a new EstablishmentAbsence (defaults are empty strings / zeros)
            Assert.Equal(string.Empty, result.Id);
            _mockGenericRepo.Verify(r => r.ReadAll(), Times.Once);
        }
    }
}
