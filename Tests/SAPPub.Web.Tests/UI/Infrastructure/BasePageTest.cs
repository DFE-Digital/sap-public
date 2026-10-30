using Microsoft.Playwright;
using Microsoft.Playwright.Xunit.v3;

namespace SAPPub.Web.Tests.UI.Infrastructure;

public abstract class BasePageTest : PageTest
{
    private readonly WebApplicationSetupFixture _fixture;
    protected string BaseUrl { get; set; }

    // ReSharper disable once ConvertToPrimaryConstructor
    protected BasePageTest(WebApplicationSetupFixture fixture)
    {
        _fixture = fixture;
        BaseUrl = _fixture.BaseUrl;
    }

    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions
        {
            BaseURL = _fixture.BaseUrl.TrimEnd('/'),
            IgnoreHTTPSErrors = true,
            ViewportSize = new() { Width = 1280, Height = 1080 },
            Locale = "en-GB",
            TimezoneId = "Europe/London",
            JavaScriptEnabled = true
        };
    }

    public override async ValueTask InitializeAsync()
    {
        await base.InitializeAsync();

        Page.SetDefaultTimeout((float)TimeSpan.FromSeconds(60).TotalMilliseconds);
        Page.SetDefaultNavigationTimeout((float)TimeSpan.FromSeconds(100).TotalMilliseconds);
    }

    public async Task WaitForSearchInputsAsync(int timeoutMs = 5000)
    {
        var selector = "input[name='__Query'], input[name='Query'][type='hidden'], input[name='Query']";
        await Page.WaitForSelectorAsync(selector, new() { Timeout = timeoutMs });
        await Page.WaitForTimeoutAsync(100);
    }

    public async Task<ILocator> GetQueryInputLocatorAsync(int checkTimeoutMs = 1000)
    {
        var jsLocator = Page.Locator("input[name='__Query']");
        try
        {
            if (await jsLocator.CountAsync() > 0)
            {
                var isVisible = await jsLocator.IsVisibleAsync();
                if (isVisible) return jsLocator;
            }

            var serverLocator = Page.Locator("input[name='Query']");
            if (await serverLocator.CountAsync() > 0) return serverLocator;

            var found = await Page.WaitForSelectorAsync("input[name='__Query'], input[name='Query']", new() { Timeout = checkTimeoutMs });
            if (found != null)
            {
                var nameAttr = await found.GetAttributeAsync("name");
                if (nameAttr == "__Query")
                    return Page.Locator("input[name='__Query']");
                return Page.Locator("input[name='Query']");
            }
            return Page.Locator("input[name='Query']");
        }
        catch
        {
            return Page.Locator("input[name='Query']");
        }
    }

    public async Task SkipContents_AccessibilityNavigation(string url)
    {
        // Arrange && Act
        var response = await Page.GotoAsync(url);

        //bypass cookies
        await Page.Keyboard.PressAsync("Tab");
        await Page.Keyboard.PressAsync("Tab");
        await Page.Keyboard.PressAsync("Tab");

        //start of page - _Layout page
        await Page.Keyboard.PressAsync("Tab");

        var activeElement = await Page.EvaluateHandleAsync("document.activeElement");
        var prop = await activeElement.GetPropertyAsync("textContent");
        string textContent = await prop.JsonValueAsync<string>();
        Assert.Equal("Skip to main content", textContent);

        //start of content - Default page
        await Page.Keyboard.PressAsync("Enter");
        await Page.Keyboard.PressAsync("Tab");

        activeElement = await Page.EvaluateHandleAsync("document.activeElement");
        prop = await activeElement.GetPropertyAsync("textContent");
        textContent = await prop.JsonValueAsync<string>();
        Assert.Equal("Skip contents", textContent);

        //start of right side content
        await Page.Keyboard.PressAsync("Enter");
        await Page.Keyboard.PressAsync("Tab");
    }
}
