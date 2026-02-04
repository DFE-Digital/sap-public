using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI;

[Collection("Playwright Tests")]
public class CookieBannerTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private string _pageUrl = "school/105574/Loreto%20High%20School%20Chorlton/secondary/about";

    [Fact]
    public async Task CookiesBanner_LoadsSuccessfully()
    {
        // Arrange: Clear cookies before navigating to the page
        await Page.Context.ClearCookiesAsync();

        // Arrange && Act
        var response = await Page.GotoAsync(_pageUrl);

        // Assert
        Assert.NotNull(response);
        var cookieBanner = await Page.Locator("#full-cookie-banner").TextContentAsync();
        Assert.NotNull(cookieBanner);

        // Act
        var acceptCookiesButton = Page.Locator("button:has-text(\"Accept analytics cookies\")");
        await acceptCookiesButton.ClickAsync();

        // Assert
        Assert.NotNull(response);
        cookieBanner = await Page.Locator("#selection-made-cookie-banner").TextContentAsync();
        Assert.NotNull(cookieBanner);

        // Act
        var hideMessageButton = Page.Locator("button:has-text(\"Hide this message\")");
        await hideMessageButton.ClickAsync();

        // Assert
        Assert.False(await Page.Locator("#selection-made-cookie-banner").IsVisibleAsync());
        Assert.False(await Page.Locator("#full-cookie-banner").IsVisibleAsync());
    }
}
