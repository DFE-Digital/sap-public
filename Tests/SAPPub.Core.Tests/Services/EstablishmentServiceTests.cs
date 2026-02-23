using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Services;
using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Tests.Services
{
    [ExcludeFromCodeCoverage]
    public class EstablishmentServiceTests
    {
        private readonly Mock<IEstablishmentRepository> _mockRepo;
        private readonly EstablishmentService _service;

        public EstablishmentServiceTests()
        {
            _mockRepo = new Mock<IEstablishmentRepository>();
            _service = new EstablishmentService(_mockRepo.Object);
        }

        private readonly Establishment FakeEstablishmentOne = new()
        {
            URN = "123456",
            EstablishmentName = "Test Establishment One",
            PhaseOfEducationName = "Primary School"
        };

        private readonly Establishment FakeEstablishmentTwo = new()
        {
            URN = "456789",
            EstablishmentName = "Test Establishment Two",
            PhaseOfEducationName = "Secondary School"
        };

        [Fact]
        public async Task GetAllEstablishmentsAsync_ShouldReturnAllItems()
        {
            // Arrange
            var expected = new List<Establishment>
            {
                FakeEstablishmentOne,
                FakeEstablishmentTwo
            };

            _mockRepo
                .Setup(r => r.GetAllEstablishmentsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.GetAllEstablishmentsAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, a => a.URN == "123456");
            Assert.Contains(result, a => a.URN == "456789");
        }

        [Fact]
        public async Task GetAllEstablishmentsAsync_ShouldReturnEmpty_WhenNoData()
        {
            // Arrange
            _mockRepo
                .Setup(r => r.GetAllEstablishmentsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Establishment>());

            // Act
            var result = await _service.GetAllEstablishmentsAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetEstablishmentAsync_ShouldReturnCorrectItem_WhenUrnExists() 
        {
            // Arrange
            var urn = "123456";

            var establishmentFromRepo = new Establishment
            {
                URN = FakeEstablishmentOne.URN,
                EstablishmentName = FakeEstablishmentOne.EstablishmentName,
                PhaseOfEducationName = FakeEstablishmentOne.PhaseOfEducationName,

                TypeOfEstablishmentId = "1",
                AdmissionsPolicyId = "2",
                DistrictAdministrativeId = "3",
                PhaseOfEducationId = "4",
                GenderId = "5",
                ReligiousCharacterId = "6",
                UrbanRuralId = "7",
                TrustsId = "8",
                LAId = "9",
            };

            _mockRepo
                .Setup(r => r.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync(establishmentFromRepo);

            // Act
            var result = await _service.GetEstablishmentAsync(urn, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(urn, result.URN);
            Assert.Equal(FakeEstablishmentOne.EstablishmentName, result.EstablishmentName);
            Assert.Equal(FakeEstablishmentOne.PhaseOfEducationName, result.PhaseOfEducationName);
        }

        [Fact]
        public async Task GetEstablishmentAsync_ShouldReturnDefault_WhenUrnDoesNotExist()
        {
            // Arrange
            var urn = "99999";

            _mockRepo
                .Setup(r => r.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Establishment()); // not found / default

            // Act
            var result = await _service.GetEstablishmentAsync(urn, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(string.IsNullOrWhiteSpace(result.URN));
        }

        [Fact]
        public async Task GetEstablishmentAsync_ShouldPropagateException_WhenRepositoryThrows()
        {
            // Arrange
            var urn = "error";

            _mockRepo
                .Setup(r => r.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Database error"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<System.Exception>(() => _service.GetEstablishmentAsync(urn, CancellationToken.None));
            Assert.Equal("Database error", ex.Message);
        }
    }
}
