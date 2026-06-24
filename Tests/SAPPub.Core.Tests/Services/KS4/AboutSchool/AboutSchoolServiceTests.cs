using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.Services.KS4.AboutSchool;

namespace SAPPub.Core.Tests.Services.KS4.AboutSchool;

public class AboutSchoolServiceTests
{
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;
    private readonly Mock<ILaUrlsRepository> _mockLaUrlsRepository;
    private readonly Mock<IEstablishmentLinksRepository> _mockEstablishmentLinksRepository;

    private readonly AboutSchoolService _service;

    private readonly EstablishmentServiceModel fakeEstablishment = new()
    {
        URN = "123456",
        EstablishmentName = "Test Establishment",
        PhaseOfEducationName = "Secondary School",
        LAName = "Council",
        LAId = "E09000001",
        GSSLACode = "LACode",
        TrustName = "Trust",
        Website = "www.test.com",
        TelephoneNum = "012346",
        AddressStreet = "Street",
        AddressLocality = "Locality",
        AddressTown = "Town",
        AddressPostcode = "Postcode",
        TypeOfEstablishmentName = "EstablishmentName",
        HeadteacherTitle = "Title",
        HeadteacherFirstName = "First",
        HeadteacherLastName = "Last",
        AgeRangeLow = "11",
        AgeRangeHigh = "18",
        TotalPupils = "1118",
        GenderName = "Gender",
        ReligiousCharacterName = "ReligiousCharacter",
        OfficialSixthFormId = "No",
        ResourcedProvisionName = "Resourced provision",
        EstablishmentTypeGroupId = "4",
        SenTypes = "SLCN - Speech, language and Communication, ASD - Autistic Spectrum Disorder, SEMH - Social"
    };

    public AboutSchoolServiceTests()
    {
        _mockEstablishmentService = new();
        _mockLaUrlsRepository = new();
        _mockEstablishmentLinksRepository = new();

        _service = new AboutSchoolService(
            _mockEstablishmentService.Object,
            _mockLaUrlsRepository.Object,
            _mockEstablishmentLinksRepository.Object);
    }

    [Fact]
    public async Task GetAboutSchoolDetailsAsync_ShouldReturnData()
    {
        var expectedLaUrl = new LaUrls
        {
            Id = "E09000001",
            Name = "Test1",
            LAMainUrl = "www.test1.com"
        };

        _mockEstablishmentService
               .Setup(r => r.GetEstablishmentAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
               .ReturnsAsync(fakeEstablishment);

        _mockLaUrlsRepository
               .Setup(r => r.GetLaAsync(fakeEstablishment.GSSLACode!, It.IsAny<CancellationToken>()))
               .ReturnsAsync(expectedLaUrl);

        // Act
        var result = await _service.GetAboutSchoolDetailsAsync(fakeEstablishment.URN, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fakeEstablishment.URN, result.Urn);
        Assert.Equal(fakeEstablishment.EstablishmentName, result.SchoolName);
        Assert.Equal(fakeEstablishment.LAName, result.LocalAuthority);
        Assert.Equal(expectedLaUrl.Name, result.LocalAuthorityName);
        Assert.Equal(expectedLaUrl.LAMainUrl, result.LocalAuthorityWebsite);
        Assert.Equal(fakeEstablishment.TrustName, result.AcademyTrust);
        Assert.Equal(fakeEstablishment.Website, result.Website);
        Assert.Equal(fakeEstablishment.TelephoneNum, result.Telephone);
        Assert.Equal(fakeEstablishment.Address, result.Address);
        Assert.Equal(fakeEstablishment.Easting, result.Easting);
        Assert.Equal(fakeEstablishment.Northing, result.Northing);
        Assert.Equal(fakeEstablishment.TypeOfEstablishmentName, result.TypeOfSchool);
        Assert.Equal(fakeEstablishment.Headteacher, result.HeadTeacher);
        Assert.Equal(fakeEstablishment.AgeRange, result.AgeRange);
        Assert.Equal(fakeEstablishment.TotalPupils, result.NumberOfPupils);
        Assert.Equal(fakeEstablishment.GenderName, result.PupilSex);
        Assert.Equal(fakeEstablishment.ReligiousCharacterName, result.ReligiousCharacter);
        Assert.Equal(fakeEstablishment.OfficialSixthFormId, result.OfficialSixthFormId);
        Assert.Equal(fakeEstablishment.ResourcedProvisionName, result.ResourcedProvisionName);
        Assert.Equal(fakeEstablishment.EstablishmentTypeGroupId, result.EstablishmentTypeGroupId);
        Assert.Equal(fakeEstablishment.SenTypes, result.SenTypes);
    }

    [Fact]
    public async Task GetAboutSchoolDetailsAsync_FallsBackToAdministrativeCode_When_GSSLaCodeNotFound()
    {
        // Arrange
        var districtAdministrativeId = "X999999";

        var expectedLaUrl = new LaUrls
        {
            Id = "E09000001",
            Name = "Test1",
            LAMainUrl = "www.test1.com"
        };

        fakeEstablishment.DistrictAdministrativeId = districtAdministrativeId;

        _mockEstablishmentService
               .Setup(r => r.GetEstablishmentAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
               .ReturnsAsync(fakeEstablishment);

        _mockLaUrlsRepository
               .Setup(r => r.GetLaAsync(fakeEstablishment.GSSLACode!, It.IsAny<CancellationToken>()))
               .ReturnsAsync((LaUrls?)null);

        _mockLaUrlsRepository
           .Setup(r => r.GetLaAsync(fakeEstablishment.DistrictAdministrativeId!, It.IsAny<CancellationToken>()))
           .ReturnsAsync(expectedLaUrl);

        // Act
        var result = await _service.GetAboutSchoolDetailsAsync(fakeEstablishment.URN, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fakeEstablishment.LAName, result.LocalAuthority);
        Assert.Equal(expectedLaUrl.Name, result.LocalAuthorityName);
        Assert.Equal(expectedLaUrl.LAMainUrl, result.LocalAuthorityWebsite);
        _mockLaUrlsRepository.Verify(a => a.GetLaAsync(fakeEstablishment.GSSLACode!, It.IsAny<CancellationToken>()), Times.Once);
        _mockLaUrlsRepository.Verify(a => a.GetLaAsync(districtAdministrativeId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData(360, true)]
    [InlineData(2000, false)]
    public async Task GetAboutSchoolDetailsAsync_HasPredecessors_GivenSchoolOpenedDate_ReturnsExpected(int numberOfDaysAgoSchoolOpened, bool expectedPredecessors)
    {
        // Arrange
        var urn = "123456";
        var predecessorUrn = "654321";
        var establishment = new EstablishmentServiceModel
        {
            URN = urn,
            EstablishmentName = "Test Establishment",
            PhaseOfEducationName = "Secondary School",
            OpenDate = DateTime.Today.AddDays(-numberOfDaysAgoSchoolOpened).ToString("dd-MM-yyyy")
        };

        _mockEstablishmentService.Setup(s => s.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(establishment);
        _mockEstablishmentLinksRepository.Setup(r => r.GetLinksAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<EstablishmentLinks>
                {
                    new EstablishmentLinks { Urn = urn, LinkType = "Predecessor", LinkUrn = predecessorUrn}
                });

        // Act
        var result = await _service.GetAboutSchoolDetailsAsync(urn, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(urn, result.Urn);
        Assert.Equal(establishment.EstablishmentName, result.SchoolName);
        Assert.Equal(establishment.OpenDate, result.OpenDate!.Value.ToString("dd-MM-yyyy"));
        if (expectedPredecessors)
        {
            Assert.NotNull(result.Predecessors);
            Assert.Contains(predecessorUrn, result.Predecessors.Select(p => p.Urn));
        }
        else
        {
            Assert.Null(result.Predecessors);
        }
    }

    [Fact]
    public async Task GetAboutSchoolDetailsAsync_ClosedSchoolHasSuccessors_ReturnsExpected()
    {
        // Arrange
        var urn = "123456";
        var successorUrn = "654321";
        var establishment = new EstablishmentServiceModel
        {
            URN = urn,
            EstablishmentName = "Test Establishment",
            PhaseOfEducationName = "Secondary School",
            StatusCode = 2
        };

        _mockEstablishmentService.Setup(s => s.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(establishment);
        _mockEstablishmentLinksRepository.Setup(r => r.GetLinksAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<EstablishmentLinks>
                {
                    new EstablishmentLinks { Urn = urn, LinkType = "Successor", LinkUrn = successorUrn}
                });

        // Act
        var result = await _service.GetAboutSchoolDetailsAsync(urn, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(urn, result.Urn);
        Assert.Equal(establishment.EstablishmentName, result.SchoolName);
        Assert.NotNull(result.Successors);
        Assert.Contains(successorUrn, result.Successors.Select(p => p.Urn));
    }
}
