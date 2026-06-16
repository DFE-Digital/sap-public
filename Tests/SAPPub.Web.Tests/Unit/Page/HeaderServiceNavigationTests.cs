using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page;

[Collection("WebAppCollection")]
public class HeaderServiceNavigationTests(WebAppFixture fixture) : PageTestsBase(fixture)
{
    private readonly string _pageUrl = "/";

    [Fact]
    public async Task ServiceTitle_IsCorrect_And_LinksToSearch()
    {
        // Arrange
        var doc = await Fixture.BrowseToPage(_pageUrl);

        // Act
        var serviceLink = doc.QuerySelector("span.govuk-service-navigation__service-name a");
        var text = serviceLink?.TextContent.Trim();
        var href = serviceLink?.GetAttribute("href");

        // Assert
        Assert.NotNull(serviceLink);
        Assert.Equal("School Profiles", text?.Trim());
        Assert.Equal("/search", href, ignoreCase: true);
    }

    [Fact]
    public async Task ServiceNavigation_IsCorrect_And_LinksToMySchoolsView()
    {
        // Arrange
        var doc = await Fixture.BrowseToPage(_pageUrl);

        // Act
        var mySchoolsViewLink = doc.QuerySelector("#my-schools-view-link");
        var text = mySchoolsViewLink?.TextContent.Trim();
        var href = mySchoolsViewLink?.GetAttribute("href");

        // Assert
        Assert.NotNull(mySchoolsViewLink);
        Assert.Equal("My schools list", text?.Trim());
        Assert.Equal("/my-schools/view", href, ignoreCase: true);
    }
}
