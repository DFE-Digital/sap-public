using Moq;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Profiles.KS2;

[Collection("WebAppCollection")]
public class CurriculumAndExtraCurricularActivitiesPageTests : PageTestsBase
{
    private string _pageRoute = "/curriculum/primary";
    private string _urn = "143034";
    private string _schoolName = "St Paul's Church of England Academy";
    private string _schoolNameMultiPhase = "Abraham Moss Community School";
    private string _urnMultiPhase = "150009";
    private readonly EstablishmentServiceModel _establishment = new();
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;

    public CurriculumAndExtraCurricularActivitiesPageTests(WebAppFixture fixture) : base(fixture)
    {
        _mockEstablishmentService = UseMock<IEstablishmentService>();

        _establishment = new EstablishmentTestBuilder()
            .WithURN(_urn)
            .WithEstablishmentName(_schoolName)
            .WithIsKeyStage2(true)
            .WithIsKeyStage4(false)
            .BuildServiceModel();

        _mockEstablishmentService
            .Setup(a => a.GetEstablishmentAsync(_urn, It.IsAny<CancellationToken>()))
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
        Assert.Contains("Primary Curriculum", title.TextContent.Trim());
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

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_Displays_VerticalNavigation()
    {
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        Assert.NotNull(doc.QuerySelector(".moj-side-navigation"));
        Assert.Equal(5, doc.QuerySelectorAll(".moj-side-navigation__item").Length);
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
    public async Task CurriculumPage_DoesNotDisplay_SubNavigation_WhenOnlyKS2()
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
        var multiPhaseEstablishment = new EstablishmentTestBuilder()
            .WithURN(_urnMultiPhase)
            .WithEstablishmentName(_schoolNameMultiPhase)
            .WithIsKeyStage2(true)
            .WithIsKeyStage4(true)
            .BuildServiceModel();

        _mockEstablishmentService
            .Setup(a => a.GetEstablishmentAsync(_urnMultiPhase, It.IsAny<CancellationToken>()))
            .ReturnsAsync(multiPhaseEstablishment);

        var url = BuildUrl(_urnMultiPhase, _schoolNameMultiPhase, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var subNav = doc.QuerySelector("#sub-navigation-academic-performance");
        Assert.NotNull(subNav);
    }

    [Fact]
    public async Task CurriculumPage_SubNavigation_HasCorrectLinks_WhenMultiplePhases()
    {
        // Arrange
        var multiPhaseEstablishment = new EstablishmentTestBuilder()
            .WithURN(_urnMultiPhase)
            .WithEstablishmentName(_schoolNameMultiPhase)
            .WithIsKeyStage2(true)
            .WithIsKeyStage4(true)
            .BuildServiceModel();

        _mockEstablishmentService
            .Setup(a => a.GetEstablishmentAsync(_urnMultiPhase, It.IsAny<CancellationToken>()))
            .ReturnsAsync(multiPhaseEstablishment);

        var url = BuildUrl(_urnMultiPhase, _schoolNameMultiPhase, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var subNav = doc.QuerySelector("#sub-navigation-academic-performance");
        Assert.NotNull(subNav);

        var primaryLink = subNav.QuerySelector("a[aria-current='page']");
        var secondaryLink = subNav.QuerySelector("a:not([aria-current='page'])");

        Assert.NotNull(primaryLink);
        Assert.NotNull(secondaryLink);
        Assert.Contains("Primary Curriculum", primaryLink.TextContent.Trim());
        Assert.Contains("Secondary Curriculum", secondaryLink.TextContent.Trim());
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_Displays_Curriculum_Summary()
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var summaryCard = doc.QuerySelector("#current-curriculum-summary");
        Assert.NotNull(summaryCard);
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_Displays_Extra_Curriculum_Summary()
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var summaryCard = doc.QuerySelector("#current-extra-curricular-activities-offered-summary");
        Assert.NotNull(summaryCard);
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_CurrentCurriculum_ContactSchoolText()
    {
        // Arrange
        var schoolWithNoWebsite = new EstablishmentTestBuilder()
            .WithURN("100273")
            .WithEstablishmentName("Saint Paul Roman Catholic Infant School")
            .WithIsKeyStage2(true)
            .WithIsKeyStage4(false)
            .WithWebsite(null) // No website - triggers contact school message
            .BuildServiceModel();

        _mockEstablishmentService
            .Setup(a => a.GetEstablishmentAsync("100273", It.IsAny<CancellationToken>()))
            .ReturnsAsync(schoolWithNoWebsite);

        var url = BuildUrl("100273", "Saint Paul Roman Catholic Infant School", _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var contactSchoolInfo = doc.QuerySelector("[data-testid='contact-school-info-ks2']");
        Assert.NotNull(contactSchoolInfo);
        Assert.Contains("Contact the school", contactSchoolInfo.TextContent);
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_Displays_Extra_Curriculum_Summary_ContactSchoolText()
    {
        // Arrange
        var schoolWithNoWebsite = new EstablishmentTestBuilder()
            .WithURN("100273")
            .WithEstablishmentName("Saint Paul Roman Catholic Infant School")
            .WithIsKeyStage2(true)
            .WithIsKeyStage4(false)
            .WithWebsite(null) // No website - triggers contact school message
            .BuildServiceModel();

        _mockEstablishmentService
            .Setup(a => a.GetEstablishmentAsync("100273", It.IsAny<CancellationToken>()))
            .ReturnsAsync(schoolWithNoWebsite);

        var url = BuildUrl("100273", "Saint Paul Roman Catholic Infant School", _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var contactSchoolInfo = doc.QuerySelector("[data-testid='contact-school-info-extra']");
        Assert.NotNull(contactSchoolInfo);
        Assert.Contains("Contact the school", contactSchoolInfo.TextContent);
    }
}
