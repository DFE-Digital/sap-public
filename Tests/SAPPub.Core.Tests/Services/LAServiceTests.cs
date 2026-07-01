using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Services;
using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Tests.Services;

[ExcludeFromCodeCoverage]
public class LAServiceTests
{
    private readonly Mock<ILaUrlsRepository> _mockLaUrlsRepository;
    private readonly LAService? _service;

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
        ResourcedProvisionName = "Resourced provision",
        EstablishmentTypeGroupId = "4",
        SenTypes = "SLCN - Speech, language and Communication, ASD - Autistic Spectrum Disorder, SEMH - Social"
    };

    public LAServiceTests()
    {
        _mockLaUrlsRepository = new Mock<ILaUrlsRepository>();
        _service = new LAService(_mockLaUrlsRepository.Object);
    }

    [Fact]
    public async Task GetLaUrlsAsyncFallsBackToAdministrativeCode_When_GSSLaCodeNotFound()
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

        _mockLaUrlsRepository
               .Setup(r => r.GetLaAsync(fakeEstablishment!.GSSLACode!, It.IsAny<CancellationToken>()))
               .ReturnsAsync((LaUrls?)null);

        _mockLaUrlsRepository
           .Setup(r => r.GetLaAsync(fakeEstablishment!.DistrictAdministrativeId!, It.IsAny<CancellationToken>()))
           .ReturnsAsync(expectedLaUrl);

        // Act
        var result = await _service!.GetLaUrlsAsync(fakeEstablishment, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedLaUrl.Name, result.Name);
        Assert.Equal(expectedLaUrl.LAMainUrl, result.LAMainUrl);
        _mockLaUrlsRepository.Verify(a => a.GetLaAsync(fakeEstablishment.GSSLACode!, It.IsAny<CancellationToken>()), Times.Once);
        _mockLaUrlsRepository.Verify(a => a.GetLaAsync(districtAdministrativeId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetLaUrlsListForEstablishmentsAsync_ReturnsCorrectData()
    {
        // Arrange
        var gssLaCode = "123-123";
        var districtAdministrativeId = "234-234";

        var establishments = new List<Establishment>
        {
            new() { GSSLACode = gssLaCode },
            new() { DistrictAdministrativeId = districtAdministrativeId }
        };

        var laListForGss = new List<LaUrls?> { new() { Id = gssLaCode, Name = "Test" } };
        var laForDaIds = new List<LaUrls?> { new() { Id = districtAdministrativeId, Name = "Test2" } };

        _mockLaUrlsRepository
            .Setup(a => a.GetLaUrlsForEstablishmentsAsync(new List<string> { gssLaCode }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(laListForGss);

        _mockLaUrlsRepository
            .Setup(a => a.GetLaUrlsForEstablishmentsAsync(new List<string> { districtAdministrativeId }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(laForDaIds);

        // Act
        var result = await _service!.GetLaUrlsListForEstablishmentsAsync(establishments, It.IsAny<CancellationToken>());

        // Assert
        Assert.Equal(2, result.Count());
        _mockLaUrlsRepository
            .Verify(a => a.GetLaUrlsForEstablishmentsAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GetLaUrlsListForEstablishmentsAsync_ReturnsEmptyList_WhenEstablishmentsIsNull()
    {
        // Arrange
        List<Establishment>? establishments = null;

        // Act
        var result = await _service!.GetLaUrlsListForEstablishmentsAsync(establishments!, It.IsAny<CancellationToken>());

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetLaUrlsListForEstablishmentsAsync_ReturnsList_WhenAllFoundByGssLaCode()
    {
        // Arrange
        var gssLaCode1 = "123-123";
        var gssLaCode2 = "234-234";

        var establishments = new List<Establishment>
        {
            new() { GSSLACode = gssLaCode1 },
            new() { GSSLACode = gssLaCode2 }
        };

        var laListForGss = new List<LaUrls?>
        {
            new() { Id = gssLaCode1, Name = "Test" },
            new() { Id = gssLaCode2, Name = "Test2" }
        };

        _mockLaUrlsRepository
            .Setup(a => a.GetLaUrlsForEstablishmentsAsync(new List<string> { gssLaCode1, gssLaCode2 }, It.IsAny<CancellationToken>()))
            .ReturnsAsync(laListForGss);

        // Act
        var result = await _service!.GetLaUrlsListForEstablishmentsAsync(establishments, It.IsAny<CancellationToken>());

        // Assert
        Assert.Equal(2, result.Count());
        _mockLaUrlsRepository
            .Verify(a => a.GetLaUrlsForEstablishmentsAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockLaUrlsRepository
            .Verify(a => a.GetLaAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
