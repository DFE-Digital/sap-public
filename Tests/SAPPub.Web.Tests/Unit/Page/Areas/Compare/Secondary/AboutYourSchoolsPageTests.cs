using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Compare.Secondary;

[Collection("WebAppCollection")]
public class AboutYourSchoolsPageTests(WebAppFixture fixture) : PageTestsBase(fixture)
{
    private string _pageUrl = "compare/secondary/about-your-schools?urns=119052&urns=124500";

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
        Assert.Equal(7, doc.QuerySelectorAll(".moj-side-navigation__item").Length);
        Assert.Single(doc.QuerySelectorAll(".moj-side-navigation__item--active"));
    }    
}
