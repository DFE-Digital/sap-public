using Microsoft.FeatureManagement;
using Moq;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Profiles.KS2;

[Collection("WebAppCollection")]
public class PupilCharacteristicsPageTests : PageTestsBase
{
    private readonly string _urn = "143034";
    private readonly string _schoolName = "St Paul's Church of England Academy";
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;

    public static IEnumerable<object[]> PageHeadings =>
    [
        ["/primary-performance/pupil-progress/all-pupils", "Progress made between year 2 and year 6"],
        ["/primary-performance/pupil-progress/disadvantaged-pupils", "Progress made by disadvantaged pupils"],
        ["/primary-performance/pupil-progress/english-as-additional-language", "Progress made by pupils with English as an additional language"],
        ["/primary-performance/pupil-progress/girls-and-boys", "Progress made by girls and boys"],
        ["/primary-performance/pupil-progress/different-starting-points", "Pupils with different starting points"],
        ["/primary-performance/pupil-progress/non-mobile-pupils", "Non-mobile pupils"],
    ];

    public static IEnumerable<object[]> SelectedDropdownOptions =>
 [
     ["/primary-performance/pupil-progress/all-pupils", "All pupils"],
        ["/primary-performance/pupil-progress/disadvantaged-pupils", "Disadvantaged pupils"],
        ["/primary-performance/pupil-progress/english-as-additional-language", "Pupils with English as an additional language"],
        ["/primary-performance/pupil-progress/girls-and-boys", "Girls and boys"],
        ["/primary-performance/pupil-progress/different-starting-points", "Pupils with different starting points"],
        ["/primary-performance/pupil-progress/non-mobile-pupils", "Non-mobile pupils"],
    ];

    public PupilCharacteristicsPageTests(WebAppFixture fixture) : base(fixture)
    {
        _mockEstablishmentService = UseMock<IEstablishmentService>();

        var progressServiceMock = UseMock<IKS2PupilProgressService>();
        progressServiceMock
            .Setup(a=>a.GetPupilProgressAsync(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS2PupilPerformanceBandings { Urn = _urn });

        progressServiceMock
            .Setup(a => a.GetDisadvantagedPupilProgressAsync(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS2PupilPerformanceBandings { Urn = _urn });

        progressServiceMock
            .Setup(a => a.GetEalPupilProgressAsync(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS2PupilPerformanceBandings { Urn = _urn });

        progressServiceMock
            .Setup(a => a.GetGirlBoyPupilProgressAsync(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                [
                    new KS2PupilPerformanceBandings { Urn = _urn, CharacteristicName = "Girls" },
                    new KS2PupilPerformanceBandings { Urn = _urn, CharacteristicName = "Boys" }
                ]);

        var establishment = new EstablishmentMinimumTestBuilder()
            .WithURN(_urn)
            .WithEstablishmentName(_schoolName)
            .WithIsKeyStage2(true)
            .WithIsKeyStage4(false)
            .BuildServiceModel();

        _mockEstablishmentService
            .Setup(a => a.GetEstablishmentMinimumAsync(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(establishment);

        var featureManagerMock = UseMock<IFeatureManager>();
        featureManagerMock
            .Setup(f => f.IsEnabledAsync(It.IsAny<string>()))
            .ReturnsAsync(true);
        featureManagerMock
            .Setup(f => f.IsEnabledAsync(Constants.Constants.EnableKS2PupilCharacteristicDropdown))
            .ReturnsAsync(true);
    }

    [Theory]
    [MemberData(nameof(PageHeadings))]
    public async Task PupilCharacteristicPage_ReturnsSuccess_AndDisplaysHeading(string pageRoute, string expectedHeading)
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var headings = doc.GetElementsByTagName("h3");
        Assert.Contains(headings, x => x.TextContent.Trim() == expectedHeading);
    }

    [Theory]
    [MemberData(nameof(SelectedDropdownOptions))]
    public async Task PupilCharacteristicPage_Displays_PupilCharacteristicDropdown_WithSelectedOption(string pageRoute, string expectedSelectedOption)
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var selector = doc.QuerySelector("#pupilCharacteristicSelector");
        Assert.NotNull(selector);
        var options = selector.QuerySelectorAll("option");
        Assert.Equal(6, options.Length);

        var selectedOption = selector.QuerySelector("option[selected]");
        Assert.NotNull(selectedOption);
        Assert.Equal(expectedSelectedOption, selectedOption.TextContent.Trim());
    }

    [Theory]
    [MemberData(nameof(PageHeadings))]
    public async Task PupilCharacteristicPage_Displays_SinglePrimaryAcademicPerformance_ActiveNavItem(string pageRoute, string _)
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        Assert.Single(doc.QuerySelectorAll(".moj-side-navigation__item--active"));
    }

    [Fact]
    public async Task PupilCharacteristicSelection_Redirects_ToSelectedPlaceholderPage()
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, "/primary-performance/pupil-progress/pupil-characteristic?selectedPupilCharacteristic=GirlsAndBoys");

        // Act
        var response = await Fixture.Client.GetAsync(url, CancellationToken.None);

        // Assert
        Assert.True(response.StatusCode is System.Net.HttpStatusCode.Redirect or System.Net.HttpStatusCode.Found or System.Net.HttpStatusCode.Moved);
        Assert.NotNull(response.Headers.Location);
        Assert.EndsWith("/girls-and-boys", response.Headers.Location!.OriginalString);
    }

    [Fact]
    public async Task PupilCharacteristicPage_ReturnsNotFound_WhenFeatureFlagDisabled()
    {
        // Arrange
        var featureManagerMock = UseMock<IFeatureManager>();
        featureManagerMock
            .Setup(f => f.IsEnabledAsync(It.IsAny<string>()))
            .ReturnsAsync(true);
        featureManagerMock
            .Setup(f => f.IsEnabledAsync(Constants.Constants.EnableKS2PupilCharacteristicDropdown))
            .ReturnsAsync(false);

        var url = BuildUrl(_urn, _schoolName, "/primary-performance/pupil-progress/all-pupils");

        // Act
        var response = await Fixture.Client.GetAsync(url, CancellationToken.None);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task PupilCharacteristicSelection_ReturnsNotFound_WhenFeatureFlagDisabled()
    {
        // Arrange
        var featureManagerMock = UseMock<IFeatureManager>();
        featureManagerMock
            .Setup(f => f.IsEnabledAsync(It.IsAny<string>()))
            .ReturnsAsync(true);
        featureManagerMock
            .Setup(f => f.IsEnabledAsync(Constants.Constants.EnableKS2PupilCharacteristicDropdown))
            .ReturnsAsync(false);

        var url = BuildUrl(_urn, _schoolName, "/primary-performance/pupil-progress/pupil-characteristic?selectedPupilCharacteristic=GirlsAndBoys");

        // Act
        var response = await Fixture.Client.GetAsync(url, CancellationToken.None);

        // Assert
        Assert.False(response.IsSuccessStatusCode);
    }
}
