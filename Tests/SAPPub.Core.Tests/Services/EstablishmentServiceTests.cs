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
        private readonly Mock<ILookupService> _mockLookupService;
        private readonly EstablishmentService _service;

        public EstablishmentServiceTests()
        {
            _mockRepo = new Mock<IEstablishmentRepository>();
            _mockLookupService = new Mock<ILookupService>();
            _service = new EstablishmentService(_mockRepo.Object, _mockLookupService.Object);
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

        //[Fact]
        //public async Task GetEstablishmentAsync_ShouldReturnCorrectItem_WhenUrnExists() //uses lookup - uncomment when view is available
        //{
        //    // Arrange
        //    var urn = "123456";

        //    // Establishment must include the ids used for lookup enrichment, otherwise
        //    // the service will just set empty strings for those fields.
        //    var establishmentFromRepo = new Establishment
        //    {
        //        URN = FakeEstablishmentOne.URN,
        //        EstablishmentName = FakeEstablishmentOne.EstablishmentName,
        //        PhaseOfEducationName = FakeEstablishmentOne.PhaseOfEducationName,

        //        TypeOfEstablishmentId = "1",
        //        AdmissionsPolicyId = "2",
        //        DistrictAdministrativeId = "3",
        //        PhaseOfEducationId = "4",
        //        GenderId = "5",
        //        ReligiousCharacterId = "6",
        //        UrbanRuralId = "7",
        //        TrustsId = "8",
        //        LAId = "9",
        //    };

        //    _mockRepo
        //        .Setup(r => r.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(establishmentFromRepo);

        //    // Provide lookups so enrichment can run without issues
        //    var lookups = new List<Lookup>
        //    {
        //        new() { LookupType = "TypeOfEstablishment", Id = "1", Name = "TypeName" },
        //        new() { LookupType = "AdmissionsPolicy", Id = "2", Name = "AdmissionsPolicyName" },
        //        new() { LookupType = "DistrictAdministrative", Id = "3", Name = "DistrictName" },
        //        new() { LookupType = "PhaseOfEducation", Id = "4", Name = "PhaseName" },
        //        new() { LookupType = "Gender", Id = "5", Name = "GenderName" },
        //        new() { LookupType = "ReligiousCharacter", Id = "6", Name = "ReligiousName" },
        //        new() { LookupType = "UrbanRural", Id = "7", Name = "UrbanRuralName" },
        //        new() { LookupType = "Trusts", Id = "8", Name = "TrustName" },
        //        new() { LookupType = "LA", Id = "9", Name = "LAName" },
        //    };

        //    _mockLookupService
        //        .Setup(r => r.GetAllLookupsAsync(It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(lookups);

        //    // Act
        //    var result = await _service.GetEstablishmentAsync(urn, CancellationToken.None);

        //    // Assert (basic)
        //    Assert.NotNull(result);
        //    Assert.Equal(urn, result.URN);
        //    Assert.Equal(FakeEstablishmentOne.EstablishmentName, result.EstablishmentName);
        //    Assert.Equal(FakeEstablishmentOne.PhaseOfEducationName, result.PhaseOfEducationName);

        //    // Assert (enrichment ran)
        //    Assert.Equal("TypeName", result.TypeOfEstablishmentName);
        //    Assert.Equal("AdmissionsPolicyName", result.AdmissionPolicy);
        //    Assert.Equal("DistrictName", result.DistrictAdministrativeName);
        //    Assert.Equal("PhaseName", result.PhaseOfEducationName);
        //    Assert.Equal("GenderName", result.GenderName);
        //    Assert.Equal("ReligiousName", result.ReligiousCharacterName);
        //    Assert.Equal("UrbanRuralName", result.UrbanRuralName);
        //    Assert.Equal("TrustName", result.TrustName);
        //    Assert.Equal("LAName", result.LAName);
        //}

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
