using Moq;
using SAPPub.Core.Entities.KS4.Workforce;
using SAPPub.Core.Interfaces.Repositories.KS4.Workforce;
using SAPPub.Core.Services.KS4.Workforce;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SAPPub.Core.Tests.Services.KS4.Workforce
{
    [ExcludeFromCodeCoverage]
    public class EstablishmentWorkforceServiceTests
    {
        private readonly Mock<IEstablishmentWorkforceRepository> _mockRepo;
        private readonly EstablishmentWorkforceService _service;

        public EstablishmentWorkforceServiceTests()
        {
            _mockRepo = new Mock<IEstablishmentWorkforceRepository>();
            _service = new EstablishmentWorkforceService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllEstablishmentWorkforceAsync_ShouldReturnAllItems()
        {
            // Arrange
            var expected = new List<EstablishmentWorkforce>
            {
                new() { Id = "100", Workforce_PupTeaRatio_Est_Current_Num = 99.99 },
                new() { Id = "101", Workforce_PupTeaRatio_Est_Current_Num = 90.00 }
            };

            _mockRepo
                .Setup(r => r.GetAllEstablishmentWorkforceAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.GetAllEstablishmentWorkforceAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, a => a.Workforce_PupTeaRatio_Est_Current_Num == 99.99);
            Assert.Contains(result, a => a.Workforce_PupTeaRatio_Est_Current_Num == 90.00);
        }

        [Fact]
        public async Task GetAllEstablishmentWorkforceAsync_ShouldReturnEmpty_WhenNoData()
        {
            // Arrange
            _mockRepo
                .Setup(r => r.GetAllEstablishmentWorkforceAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<EstablishmentWorkforce>());

            // Act
            var result = await _service.GetAllEstablishmentWorkforceAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetEstablishmentWorkforceAsync_ShouldReturnCorrectItem_WhenUrnExists()
        {
            // Arrange
            var urn = "100";
            var expected = new EstablishmentWorkforce { Id = urn, Workforce_PupTeaRatio_Est_Current_Num = 100 };

            _mockRepo
                .Setup(r => r.GetEstablishmentWorkforceAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.GetEstablishmentWorkforceAsync(urn, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(urn, result.Id);
            Assert.Equal(100, result.Workforce_PupTeaRatio_Est_Current_Num);
        }

        [Fact]
        public async Task GetEstablishmentWorkforceAsync_ShouldReturnDefault_WhenUrnDoesNotExist()
        {
            // Arrange
            var urn = "99999";

            _mockRepo
                .Setup(r => r.GetEstablishmentWorkforceAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new EstablishmentWorkforce());

            // Act
            var result = await _service.GetEstablishmentWorkforceAsync(urn, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Workforce_TotPupils_Est_Current_Num);
        }

        [Fact]
        public async Task GetEstablishmentWorkforceAsync_ShouldPropagateException_WhenRepositoryThrows()
        {
            // Arrange
            var urn = "error";

            _mockRepo
                .Setup(r => r.GetEstablishmentWorkforceAsync(urn, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.GetEstablishmentWorkforceAsync(urn, CancellationToken.None));
            Assert.Equal("Database error", ex.Message);
        }
    }
}
