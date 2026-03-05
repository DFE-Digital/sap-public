using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Services.KS4.AboutSchool;

namespace SAPPub.Core.Tests.Services.KS4.AboutSchool;

public class AboutSchoolServiceTests
{
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;
    private readonly Mock<ILaUrlsRepository> _mockLaUrlsRepository;

    private readonly AboutSchoolService _service;

    private readonly Establishment fakeEstablishment = new()
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
        ResourcedProvision = "Resourced provision",
    };

    public AboutSchoolServiceTests()
    {
        _mockEstablishmentService = new();
        _mockLaUrlsRepository = new();

        _service = new AboutSchoolService(
            _mockEstablishmentService.Object,
            _mockLaUrlsRepository.Object);
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
        Assert.Equal(fakeEstablishment.ResourcedProvision, result.ResourcedProvision);
    }
}
