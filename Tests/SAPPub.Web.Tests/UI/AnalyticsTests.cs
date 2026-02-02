using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Playwright;
using SAPPub.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI;

public class AnalyticsTests : BasePageTest
{
    [Fact]
    public async Task GoogleTagManager_LoadsWhenItShould()
    {
        // Arrange: Clear cookies before navigating to the page
        var cookies = new Cookie[]
        {
            new Cookie
            {
                Name = "analytics_preference",
                Value = "true",
                Domain = "localhost",
                Path = "/",
                HttpOnly = true,
                Secure = true
            }
        };

        await Page.Context.AddCookiesAsync(cookies);

        // Arrange && Act
        var response = await GoToPageAysnc("");

        // Assert
        Assert.NotNull(response);

        string html = await response.TextAsync();

        Assert.Contains("googletagmanager.com/gtm.js", html, StringComparison.OrdinalIgnoreCase);
    }
}
