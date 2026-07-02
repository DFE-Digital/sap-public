using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.Compare.Secondary;

[Collection("Playwright Tests")]
public class NavigateThroughComparePagesTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private string _pageUrl = "compare/secondary/about-your-schools";

    [Fact]
    public async Task NavigateThroughLeftNav_ShowsExpectedPages()
    {
        var queryString = "urns=100279&urns=105574";

        // Act
        var response = await Page.GotoAsync($"{_pageUrl}?{queryString}");
        var nav = new VerticalNavigationHelper(Page);
        var navItem = nav.GetItem("Academic Performance");
        await navItem.ClickAsync();

        // Assert
        var title = await Page.TitleAsync();
        Assert.Contains("Pupil attainment", title);

        // Act
        navItem = nav.GetItem("Destinations after year 11");
        await navItem.ClickAsync();

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains("Destinations after year 11", title);

        // Act
        navItem = nav.GetItem("Next Steps");
        await navItem.ClickAsync();

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains("Next Steps", title);
    }

    [Fact]
    public async Task NavigateThroughPaginationNav_ShowsExpectedPages()
    {
        var queryString = "urns=100279&urns=145179";

        // Act
        var response = await Page.GotoAsync($"{_pageUrl}?{queryString}");
        var nav = new PaginationNavigationHelper(Page);
        await nav.ClickNextLinkAsync();

        // Assert
        var title = await Page.TitleAsync();
        Assert.Contains("Pupil attainment", title);

        // Act
        await nav.ClickNextLinkAsync();

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains("English and maths", title);

        // Act
        await nav.ClickNextLinkAsync();

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains("Destinations after year 11", title);

        // Act
        await nav.ClickNextLinkAsync();

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains("Next Steps", title);

        // Act
        await nav.ClickPreviousLinkAsync();

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains("Destinations after year 11", title);

        // Act
        await nav.ClickPreviousLinkAsync();

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains("English and maths", title);

        // Act
        await nav.ClickPreviousLinkAsync();

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains("Pupil attainment", title);

        // Act
        await nav.ClickPreviousLinkAsync();

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains("About your schools", title);
    }
}
