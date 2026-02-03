using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Playwright;
using SAPPub.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI;

public class AnalyticsTests : BasePageTest
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    [InlineData(null)]
    public async Task GoogleTagManager_LoadsWhenItShould(bool? acceptCookies)
    {
        // Arrange: Clear cookies before navigating to the page
        var cookies = new Cookie[]
        {
            new Cookie
            {
                Name = "analytics_preference",
                Value = acceptCookies?.ToString().ToLower() ?? string.Empty,
                Domain = "localhost",
                Path = "/",
                HttpOnly = true,
                Secure = true
            }
        };

        if (acceptCookies.HasValue)
        {
            await Page.Context.AddCookiesAsync(cookies);
        }


        // Arrange && Act
        var response = await GoToPageAysnc("");

        // Assert
        Assert.NotNull(response);

        string html = await response.TextAsync();
        if (acceptCookies == true)
        {
            Assert.Contains("googletagmanager.com/gtm.js", html, StringComparison.OrdinalIgnoreCase);
        }
        else
        {
            Assert.DoesNotContain("googletagmanager.com/gtm.js", html, StringComparison.OrdinalIgnoreCase);
        }

    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    [InlineData(null)]
    public async Task MicrosoftClarity_LoadsWhenItShould(bool? acceptCookies)
    {
        // Arrange: Clear cookies before navigating to the page
        var cookies = new Cookie[]
        {
            new Cookie
            {
                Name = "analytics_preference",
                Value = acceptCookies?.ToString().ToLower() ?? string.Empty,
                Domain = "localhost",
                Path = "/",
                HttpOnly = true,
                Secure = true
            }
        };

        if (acceptCookies.HasValue)
        {
            await Page.Context.AddCookiesAsync(cookies);
        }


        // Arrange && Act
        var response = await GoToPageAysnc("");

        // Assert
        Assert.NotNull(response);

        string html = await response.TextAsync();
        if (acceptCookies == true)
        {
            Assert.Contains("www.clarity.ms", html, StringComparison.OrdinalIgnoreCase);
        }
        else
        {
            Assert.DoesNotContain("www.clarity.ms", html, StringComparison.OrdinalIgnoreCase);
        }

    }
}
