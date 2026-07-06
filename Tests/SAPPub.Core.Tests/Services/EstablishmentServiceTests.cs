using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Exceptions;
using SAPPub.Core.Interfaces.Repositories;
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

                TypeOfEstablishmentId = 1,
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

        [Fact]
        public async Task GetEstablishmentsAsync_ShouldReturnCorrectItems_WhenUrnsExists()
        {
            // Arrange
            var urns = new List<string> { "123456", "785456" };

            var expectedEstablishments = new List<Establishment>
            {
                new() { URN = "123456", EstablishmentName = "Est1" },
                new() { URN = "785456", EstablishmentName = "Est2" }
            };

            _mockRepo
                .Setup(r => r.GetEstablishmentsAsync(urns, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEstablishments);

            // Act
            var results = await _service.GetEstablishmentsAsync(urns, CancellationToken.None);

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
        }

        [Fact]
        public async Task GetEstablishmentdAsync_ShouldThrow_NotFoundException_WhenRepository_Returns_NotResults()
        {
            // Arrange
            var urns = new List<string> { "123456", "785456" };
            var expectedEstablishments = new List<Establishment>();

            _mockRepo
                .Setup(r => r.GetEstablishmentsAsync(urns, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEstablishments);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => _service.GetEstablishmentsAsync(urns, CancellationToken.None));
            Assert.Equal($"Establishments not found for the given URNs: {string.Join(", ", urns)}", ex.Message);
        }

        [Fact]
        public async Task GetEstablishmentdAsync_ShouldPropagateException_WhenRepositoryThrows()
        {
            // Arrange
            var urns = new List<string> { "123456", "785456" };

            _mockRepo
                .Setup(r => r.GetEstablishmentsAsync(urns, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.GetEstablishmentsAsync(urns, CancellationToken.None));
            Assert.Equal("Database error", ex.Message);
        }
    }
}
