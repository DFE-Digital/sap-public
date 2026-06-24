using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Compare.Secondary;

[Collection("WebAppCollection")]
public class ProgressAndAttainmentPageTests : PageTestsBase
{
    private string _pageUrl = "compare/secondary/pupil-attainment?urns=123456&urns=123457";
    private readonly Mock<IEstablishmentService> _establishmentService;

    public ProgressAndAttainmentPageTests(WebAppFixture fixture) : base(fixture)
    {
        // set up the mock establishment service to return establishments for the URNs in the query string
        // this is used by the page validation filter to determine if the establishments are secondary and should be compared
        _establishmentService = UseMock<IEstablishmentService>();
        var establishmentList = (new List<Establishment>
        {
            new EstablishmentTestBuilder().WithURN("123456").WithIsKeyStage4(true).Build(),
            new EstablishmentTestBuilder().WithURN("123457").WithIsKeyStage4(true).Build(),
        }).ToList();

        establishmentList.Select(e =>
            _establishmentService.Setup(s =>
            s.GetEstablishmentAsync(e.URN, It.IsAny<CancellationToken>())).ReturnsAsync(e)).ToList();
    }

    [Fact]
    public async Task AcademicPerformanceProgressAndAttainment_Displays_VerticalNavigation()
    {
        // Act
        var doc = await Fixture.BrowseToPage(_pageUrl);

        // Assert
        Assert.NotNull(doc.QuerySelector(".moj-side-navigation"));
        Assert.Equal(4, doc.QuerySelectorAll(".moj-side-navigation__item").Length);
        Assert.Single(doc.QuerySelectorAll(".moj-side-navigation__item--active"));
    }

    [Fact]
    public async Task AcademicPerformanceProgressAndAttainmentTests_Displays_Sub_Navigation()
    {
        // Act
        var doc = await Fixture.BrowseToPage(_pageUrl);

        // Assert
        Assert.NotNull(doc.QuerySelector("#sub-navigation-academic-performance"));
    }

    [Fact]
    public async Task AcademicPerformanceProgressAndAttainmentTests_Has_Correct_Sub_Navigation_Links()
    {
        // Act
        var doc = await Fixture.BrowseToPage(_pageUrl);
        var container = doc.QuerySelector("#sub-navigation-academic-performance");
        var links = container?.QuerySelectorAll(".moj-sub-navigation__link");

        Assert.NotNull(links);
        Assert.Equal(2, links.Length);
        Assert.Equal("/compare/secondary/pupil-attainment?urns=123456&urns=123457", links[0].GetAttribute("href"));
        Assert.Equal("/compare/secondary/english-and-maths-results?urns=123456&urns=123457", links[1].GetAttribute("href"));
    }

    [Fact]
    public async Task DisplaysPagination()
    {
        // Arrange
        var doc = await Fixture.BrowseToPage(_pageUrl);

        // Act
        var nav = doc.QuerySelector("#academic-performance-attainment-pagination");
        var navNext = doc.QuerySelector("#academic-performance-attainment-pagination .govuk-pagination__next a");
        var navPrevious = doc.QuerySelector("#academic-performance-attainment-pagination .govuk-pagination__prev a");

        Assert.NotNull(nav);
        Assert.NotNull(navNext);
        Assert.NotNull(navPrevious);
        Assert.Contains("About your schools", navPrevious.TextContent);
        Assert.Contains("Academic performance: English and maths results", navNext.TextContent);
    }
}
