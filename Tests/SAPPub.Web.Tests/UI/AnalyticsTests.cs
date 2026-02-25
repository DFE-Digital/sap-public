using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Playwright;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI;

[Collection("Playwright Tests")]
public class AnalyticsTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{

    [Fact]
    public async Task GoogleTagManager_LoadsWhenAccepted()
    {
        // Arrange: Clear cookies before navigating to the page
        await Page.Context.ClearCookiesAsync();

        // Arrange && Act
        var response = await Page.GotoAsync("");

        // Assert
        Assert.NotNull(response);
        var cookieBanner = await Page.Locator("#full-cookie-banner").TextContentAsync();
        Assert.NotNull(cookieBanner);

        // Act
        var acceptCookiesButton = Page.Locator("button:has-text(\"Accept analytics cookies\")");
        await acceptCookiesButton.ClickAsync();

        var content = await Page.ContentAsync();

        Assert.Contains("googletagmanager.com/gtm.js", content, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task GoogleTagManager_DoesntLoadWhenDeclined()
    {
        // Arrange: Clear cookies before navigating to the page
        await Page.Context.ClearCookiesAsync();

        // Arrange && Act
        var response = await Page.GotoAsync("");

        // Assert
        Assert.NotNull(response);
        var cookieBanner = await Page.Locator("#full-cookie-banner").TextContentAsync();
        Assert.NotNull(cookieBanner);

        // Act
        var acceptCookiesButton = Page.Locator("button:has-text(\"Reject analytics cookies\")");
        await acceptCookiesButton.ClickAsync();

        var content = await Page.ContentAsync();

        Assert.DoesNotContain("googletagmanager.com/gtm.js", content, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task GoogleTagManager_DoesntLoadWhenIgnored()
    {
        // Arrange: Clear cookies before navigating to the page
        await Page.Context.ClearCookiesAsync();

        // Arrange && Act
        var response = await Page.GotoAsync("");

        // Assert
        var content = await Page.ContentAsync();

        Assert.DoesNotContain("googletagmanager.com/gtm.js", content, StringComparison.OrdinalIgnoreCase);
    }
}
