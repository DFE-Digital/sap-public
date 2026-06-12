using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.Compare.Secondary;

[Collection("Playwright Tests")]
public class AcademicPerformanceProgressAndAttainmentTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private string _pageUrl = "compare/secondary/pupil-performance-attainment-and-progress?urns=119052&urns=124500";

    [Fact]
    public async Task AcademicPerformanceProgressAndAttainmentTests_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(_pageUrl);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task AcademicPerformanceProgressAndAttainmentTests_Displays_VerticalNavigation()
    {
        var nav = new VerticalNavigationHelper(Page);
        await Page.GotoAsync(_pageUrl);

        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveItemsCountAsync(7);
        await nav.ShouldHaveOneActiveItemAsync();
    }

    [Fact]
    public async Task AcademicPerformanceProgressAndAttainmentTests_Displays_Sub_Navigation()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#sub-navigation-academic-performance").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AcademicPerformanceProgressAndAttainmentTests_Has_Correct_Sub_Navigation_Links()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var content = await Page.ContentAsync();
        var container = Page.Locator("#sub-navigation-academic-performance");
        var links = container.Locator(".moj-sub-navigation__link");
        var linkCount = await links.CountAsync();
        var firstListHref = await links.First.GetAttributeAsync("href");
        var secondLinkHref = await links.Last.GetAttributeAsync("href");

        // Assert
        Assert.Equal(2, linkCount);
        Assert.Equal("/compare/secondary/pupil-performance-attainment-and-progress?urns=119052&urns=124500", firstListHref);
        Assert.Equal("/compare/secondary/english-and-maths-results?urns=119052&urns=124500", secondLinkHref);
    }
}