using AngleSharp.Dom;
using Microsoft.FeatureManagement;
using Moq;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Profiles.KS4;

[Collection("WebAppCollection")]
public class CurriculumAndExtraCurricularActivitiesPageTests : PageTestsBase
{
    private string _pageRoute = "/curriculum/secondary";
    private string _urn = "143034";
    private string _schoolName = "St Paul's Church of England Academy";
    private string _schoolNameMultiPhase = "Abraham Moss Community School";
    private string _urnMultiPhase = "150009";
    private readonly EstablishmentMinimumServiceModel _establishment = new();
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;

    public CurriculumAndExtraCurricularActivitiesPageTests(WebAppFixture fixture) : base(fixture)
    {
        _mockEstablishmentService = UseMock<IEstablishmentService>();

        _establishment = new EstablishmentMinimumTestBuilder()
            .WithURN(_urn)
            .WithEstablishmentName(_schoolName)
            .WithIsKeyStage2(false)
            .WithIsKeyStage4(true)
            .BuildServiceModel();

        _mockEstablishmentService
            .Setup(a => a.GetEstablishmentMinimumAsync(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_establishment);
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_HasCorrectTitle()
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var title = doc.QuerySelector("title");
        Assert.NotNull(title);
        Assert.Contains("Secondary Curriculum", title.TextContent.Trim());
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_DisplaysMainHeading()
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var heading = doc.QuerySelector("h1");
        Assert.NotNull(heading);
        Assert.NotEmpty(heading.TextContent.Trim());
    }

    [Theory]
    [InlineData(true, true, 7)] // Multi-phase school (KS2 and KS4)
    [InlineData(true, false, 6)] // ks4 only school
    public async Task CurriculumAndExtraCurricularActivitiesPage_Displays_VerticalNavigation(bool isKs4, bool isKs2, int expectedItemCount)
    {
        var establishment = new EstablishmentMinimumTestBuilder()
        .WithURN(_urnMultiPhase)
        .WithEstablishmentName(_schoolNameMultiPhase)
        .WithIsKeyStage2(isKs2)
        .WithIsKeyStage4(isKs4)
        .BuildServiceModel();

        _mockEstablishmentService
            .Setup(a => a.GetEstablishmentMinimumAsync(_urnMultiPhase, It.IsAny<CancellationToken>()))
            .ReturnsAsync(establishment);

        var url = BuildUrl(_urnMultiPhase, _schoolNameMultiPhase, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        Assert.NotNull(doc.QuerySelector(".moj-side-navigation"));
        var list = doc.QuerySelectorAll(".moj-side-navigation__item");
        Assert.Equal(expectedItemCount, doc.QuerySelectorAll(".moj-side-navigation__item").Length);
        Assert.Single(doc.QuerySelectorAll(".moj-side-navigation__item--active"));
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_Displays_SchoolName_Caption()
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var schoolNameCaption = doc.QuerySelector("#school-name-caption");
        Assert.NotNull(schoolNameCaption);
        Assert.Equal(_schoolName, schoolNameCaption.TextContent.Trim());
    }

    [Fact]
    public async Task CurriculumPage_DoesNotDisplay_SubNavigation_WhenOnlyKS4()
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var subNav = doc.QuerySelector("#sub-navigation-academic-performance");
        Assert.Null(subNav);
    }

    [Fact]
    public async Task CurriculumPage_Displays_SubNavigation_WhenMultiplePhases()
    {
        // Arrange
        ConfigureMultiPhaseSchool();

        var url = BuildUrl(_urnMultiPhase, _schoolNameMultiPhase, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var subNav = doc.QuerySelector("#sub-navigation-academic-performance");
        Assert.NotNull(subNav);
    }

    [Fact]
    public async Task AdmissionsPage_DoesNotDisplay_SubNavigation_WhenFeatureFlagDisabled()
    {
        // Arrange
        var featureManagerMock = UseMock<IFeatureManager>();
        featureManagerMock
            .Setup(f => f.IsEnabledAsync(Constants.Constants.EnablePrimary))
            .ReturnsAsync(false);

        ConfigureMultiPhaseSchool();

        var url = BuildUrl(_urnMultiPhase, _schoolNameMultiPhase, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var subNav = doc.QuerySelector("#sub-navigation-academic-performance");
        Assert.Null(subNav);
    }


    [Fact]
    public async Task CurriculumPage_SubNavigation_HasCorrectLinks_WhenMultiplePhases()
    {
        // Arrange
        ConfigureMultiPhaseSchool();

        var url = BuildUrl(_urnMultiPhase, _schoolNameMultiPhase, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var subNav = doc.QuerySelector("#sub-navigation-academic-performance");
        Assert.NotNull(subNav);

        var secondaryLink = subNav.QuerySelector("a[aria-current='page']");
        var primaryLink = subNav.QuerySelector("a:not([aria-current='page'])");

        Assert.NotNull(primaryLink);
        Assert.NotNull(secondaryLink);
        Assert.Contains("Primary Curriculum", primaryLink.TextContent.Trim());
        Assert.Contains("Secondary Curriculum", secondaryLink.TextContent.Trim());
    }

    [Fact]
    public async Task CurriculumPage_DisplaysBottomPagination_WithCorrectDestinations()
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var pagination = doc.QuerySelector("nav.govuk-pagination");
        Assert.NotNull(pagination);

        var previousLink = pagination.QuerySelector(".govuk-pagination__prev a");
        var nextLink = pagination.QuerySelector(".govuk-pagination__next a");

        Assert.NotNull(previousLink);
        Assert.Contains("/admissions/secondary", previousLink.GetAttribute("href"));

        Assert.NotNull(nextLink);
        Assert.Contains("/attendance", nextLink.GetAttribute("href"));
    }

    [Fact]
    public async Task CurriculumPage_DisplaysBottomPagination_WithCorrectDestinations_WhenMultiplePhases()
    {
        // Arrange
        ConfigureMultiPhaseSchool();
        var url = BuildUrl(_urnMultiPhase, _schoolNameMultiPhase, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var pagination = doc.QuerySelector("nav.govuk-pagination");
        Assert.NotNull(pagination);

        var previousLink = pagination.QuerySelector(".govuk-pagination__prev a");
        var nextLink = pagination.QuerySelector(".govuk-pagination__next a");

        Assert.NotNull(previousLink);
        Assert.Contains("/curriculum/primary", previousLink.GetAttribute("href"));

        Assert.NotNull(nextLink);
        Assert.Contains("/attendance", nextLink.GetAttribute("href"));
    }

    private void ConfigureMultiPhaseSchool()
    {
        var multiPhaseEstablishment = new EstablishmentMinimumTestBuilder()
           .WithURN(_urnMultiPhase)
           .WithEstablishmentName(_schoolNameMultiPhase)
           .WithIsKeyStage2(true)
           .WithIsKeyStage4(true)
           .BuildServiceModel();

        _mockEstablishmentService
            .Setup(a => a.GetEstablishmentMinimumAsync(_urnMultiPhase, It.IsAny<CancellationToken>()))
            .ReturnsAsync(multiPhaseEstablishment);
    }
}
