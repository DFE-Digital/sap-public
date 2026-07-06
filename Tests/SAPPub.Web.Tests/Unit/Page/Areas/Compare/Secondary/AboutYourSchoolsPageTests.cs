using Moq;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.KS4.AboutSchool;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Compare.Secondary;

[Collection("WebAppCollection")]
public class AboutYourSchoolsPageTests : PageTestsBase
{
    private static readonly List<string> _urns = ["119052", "124500"];
    private string _pageUrl = $"compare/secondary/about-your-schools?urns={_urns[0]}&urns={_urns[1]}";
    private readonly Mock<IAboutSchoolService> _mockAboutSchoolService = new();
    private readonly Mock<IEstablishmentService> _establishmentService;

    public AboutYourSchoolsPageTests(WebAppFixture fixture) : base(fixture)
    {
        _mockAboutSchoolService = UseMock<IAboutSchoolService>();
        _establishmentService = UseMock<IEstablishmentService>();
        var establishmentList = (new List<EstablishmentServiceModel>
        {
            new EstablishmentTestBuilder().WithURN("119052").WithIsKeyStage4(true).BuildServiceModel(),
            new EstablishmentTestBuilder().WithURN("124500").WithIsKeyStage4(true).BuildServiceModel(),
        }).ToList();

        establishmentList.Select(e =>
            _establishmentService.Setup(s =>
            s.GetEstablishmentAsync(e.URN, It.IsAny<CancellationToken>())).ReturnsAsync(e)).ToList();

        var aboutSchools = new List<AboutSchoolComparisonModel>
        {
            new(){ SchoolName ="Test School", Address="Test Street", Website = "www.test-school.com", Urn = "119052", Easting="532301", Northing="181746" },
            new(){ SchoolName ="Test School1", Urn = "124500" }
        };

        _mockAboutSchoolService
            .Setup(a => a.GetAboutSchoolForComparisonAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(aboutSchools);
    }

    [Fact]
    public async Task AboutYourSchoolsPage_HasCorrectTitle()
    {
        // Arrange
        var doc = await Fixture.BrowseToPage(_pageUrl);

        // Act
        var title = doc.QuerySelector("title");

        // Assert
        Assert.NotNull(title);
        Assert.Contains("About your schools", title.TextContent.Trim());
    }

    [Fact]
    public async Task AboutYourSchoolsPage_DisplaysMainHeading()
    {
        // Arrange
        var doc = await Fixture.BrowseToPage(_pageUrl);

        // Act
        var heading = doc.QuerySelector("h1");

        // Assert
        Assert.NotNull(heading);
        Assert.Contains("About your schools", heading.TextContent.Trim());
    }

    [Fact]
    public async Task AboutYourSchools_Displays_VerticalNavigation()
    {
        // Act
        var doc = await Fixture.BrowseToPage(_pageUrl);

        // Assert
        Assert.NotNull(doc.QuerySelector(".moj-side-navigation"));
        Assert.Equal(4, doc.QuerySelectorAll(".moj-side-navigation__item").Length);
        Assert.Single(doc.QuerySelectorAll(".moj-side-navigation__item--active"));
    }

    [Fact]
    public async Task DisplaysPagination()
    {
        // Arrange
        var doc = await Fixture.BrowseToPage(_pageUrl);

        // Act
        var nav = doc.QuerySelector("#about-the-school-pagination");
        var navNext = doc.QuerySelector("#about-the-school-pagination .govuk-pagination__next a");
        var navPrevious = doc.QuerySelector("#about-the-school-pagination .govuk-pagination__prev a");

        Assert.NotNull(nav);
        Assert.NotNull(navNext);
        Assert.Null(navPrevious);
        Assert.Contains("Academic Performance", navNext.TextContent);
    }

    [Fact]
    public async Task Page_WithNoSpecialSchool_DoesNotShowShowsNotificationBanner()
    {
        // Act
        var doc = await Fixture.BrowseToPage(_pageUrl);

        var banner = doc.QuerySelector("[data-testid='special-school-warning-banner']");
        Assert.Null(banner);
    }

    [Fact]
    public async Task Page_WithSpecialSchool_ShowsNotificationBanner()
    {
        // Arrange
        _establishmentService.Setup(s => s.GetEstablishmentAsync("119052", It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new EstablishmentTestBuilder()
                    .WithURN("119052")
                    .WithIsKeyStage4(true)
                    .WithTypeOfEstablishmentId("7")
                    .BuildServiceModel());

        // Act
        var doc = await Fixture.BrowseToPage(_pageUrl);

        // Assert
        var banner = doc.QuerySelector("[data-testid='special-school-warning-banner']");
        Assert.NotNull(banner);
        Assert.Contains("You're comparing a special school", banner.TextContent.Trim());
    }

    [Fact]
    public async Task DisplaysLocationSection()
    {
        // Arrange && Act
        var doc = await Fixture.BrowseToPage(_pageUrl);

        // Assert
        var heading = doc.QuerySelector(".govuk-grid-column-three-quarters > h2");
        Assert.NotNull(heading);
        Assert.Contains("Location", heading.TextContent.Trim());
    }

    [Fact]
    public async Task DisplaysLocationTable()
    {
        // Arrange && Act
        var doc = await Fixture.BrowseToPage(_pageUrl);

        // Assert
        var locationTable = doc.QuerySelector("#schoolLocationTable");
        Assert.NotNull(locationTable);

        var tableRows = locationTable.QuerySelectorAll(".govuk-table__row");
        Assert.NotNull(tableRows);

        var row1Col1Text = tableRows[0].Children[0].TextContent;
        var row1Col2Text = tableRows[0].Children[1].TextContent;
        var row2Col1Text = tableRows[1].Children[0].TextContent;
        var row2Col2Text = tableRows[1].Children[1].TextContent;
        var row3Col1Text = tableRows[2].Children[0].TextContent;
        var row3Col2Text = tableRows[2].Children[1].TextContent;

        Assert.Equal("School", row1Col1Text);
        Assert.Equal("Address", row1Col2Text);
        Assert.Equal("Test School", row2Col1Text);
        Assert.Equal("Test Street", row2Col2Text);
        Assert.Equal("Test School1", row3Col1Text);
        Assert.Equal("Not available", row3Col2Text);
    }

    [Fact]
    public async Task DisplaysMapWithLegend()
    {
        // Arrange && Act
        var doc = await Fixture.BrowseToPage(_pageUrl);

        // Assert
        var mapElement = doc.QuerySelector("#map");
        var legendElement = doc.QuerySelector(".legend");
        Assert.NotNull(mapElement);
        Assert.NotNull(legendElement);
        var datapoints = mapElement.Attributes[3];
        Assert.Equal("data-points", datapoints?.Name);
        Assert.Contains("lat", datapoints?.Value);
        Assert.Equal(1, legendElement.ChildElementCount);

    }
    [Fact]
    public async Task DisplaysFurtherInformationSection()
    {
        // Arrange && Act
        var doc = await Fixture.BrowseToPage(_pageUrl);

        // Assert
        var summaryCard = doc.QuerySelector("#schoolInformationSummary");
        var summaryTitle = doc.QuerySelector("#schoolInformationSummary .govuk-summary-card__title");

        Assert.NotNull(summaryCard);
        Assert.NotNull(summaryTitle);
        Assert.Equal("Further information", summaryTitle.TextContent.Trim());
    }

    [Fact]
    public async Task DisplaysFurtherInformationLinks()
    {
        // Arrange && Act
        var doc = await Fixture.BrowseToPage(_pageUrl);

        // Assert - School 1
        var school1Heading = doc.QuerySelector("#school-details-heading-119052");
        var school1ProfileLink = doc.QuerySelector("#school-profile-link-119052");
        var school1OfstedLink = doc.QuerySelector("#ofsted-report-link-119052");

        Assert.NotNull(school1Heading);
        Assert.Equal("Test School", school1Heading.TextContent.Trim());

        Assert.NotNull(school1ProfileLink);
        Assert.Contains("View Test School's School Profile", school1ProfileLink.TextContent);
        Assert.Contains("school/119052", school1ProfileLink.GetAttribute("href"));

        Assert.NotNull(school1OfstedLink);
        Assert.Contains("View the latest Ofsted report", school1OfstedLink.TextContent);
        Assert.Contains("/ELS/119052", school1OfstedLink.GetAttribute("href"));

        // Assert - School 2
        var school2Heading = doc.QuerySelector("#school-details-heading-124500");
        var school2ProfileLink = doc.QuerySelector("#school-profile-link-124500");
        var school2OfstedLink = doc.QuerySelector("#ofsted-report-link-124500");

        Assert.NotNull(school2Heading);
        Assert.Equal("Test School1", school2Heading.TextContent.Trim());

        Assert.NotNull(school2ProfileLink);
        Assert.Contains("View Test School1's School Profile", school2ProfileLink.TextContent);
        Assert.Contains("school/124500", school2ProfileLink.GetAttribute("href"));

        Assert.NotNull(school2OfstedLink);
        Assert.Contains("View the latest Ofsted report", school2OfstedLink.TextContent);
        Assert.Contains("/ELS/124500", school2OfstedLink.GetAttribute("href"));
    }

    [Fact]
    public async Task DisplaysWebsiteAsLinkWhenAvailable_AndNotAvailableWhenMissing()
    {
        // Arrange && Act
        var doc = await Fixture.BrowseToPage(_pageUrl);

        // Available website (school 1)
        var school1WebsiteValue = doc.QuerySelector("#school-website-value-119052");
        var school1WebsiteLink = doc.QuerySelector("#school-website-link-119052");

        Assert.NotNull(school1WebsiteValue);
        Assert.NotNull(school1WebsiteLink);
        Assert.Contains("www.test-school.com", school1WebsiteLink.TextContent);
        Assert.Equal("https://www.test-school.com", school1WebsiteLink.GetAttribute("href"));

        // Missing website (school 2)
        var school2WebsiteValue = doc.QuerySelector("#school-website-value-124500");
        var school2WebsiteLink = doc.QuerySelector("#school-website-link-124500");

        Assert.NotNull(school2WebsiteValue);
        Assert.Null(school2WebsiteLink);
        Assert.Equal("Not available", school2WebsiteValue.TextContent.Trim());
    }
}