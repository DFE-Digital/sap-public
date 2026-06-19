using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Compare.Secondary;

[Collection("WebAppCollection")]
public class AboutYourSchoolsPageTests : PageTestsBase
{
    private string _pageUrl = "compare/secondary/about-your-schools?urns=119052&urns=124500";
    private readonly Mock<IEstablishmentService> _establishmentService;

    public AboutYourSchoolsPageTests(WebAppFixture fixture) : base(fixture)
    {
        // set up the mock establishment service to return establishments for the URNs in the query string
        // this is used by the page validation filter to determine if the establishments are secondary and should be compared
        _establishmentService = UseMock<IEstablishmentService>();
        var establishmentList = (new List<Establishment>
        {
            new EstablishmentTestBuilder().WithURN("119052").WithIsKeyStage4(true).Build(),
            new EstablishmentTestBuilder().WithURN("124500").WithIsKeyStage4(true).Build(),
        }).ToList();

        establishmentList.Select(e =>
            _establishmentService.Setup(s =>
            s.GetEstablishmentAsync(e.URN, It.IsAny<CancellationToken>())).ReturnsAsync(e)).ToList();
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
}
