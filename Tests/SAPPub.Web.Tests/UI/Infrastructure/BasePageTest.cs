using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Models;
using System.Text;
using System.Text.Json;

namespace SAPPub.Web.Tests.UI.Infrastructure;

public abstract class BasePageTest : PageTest
{
    private readonly WebApplicationSetupFixture _fixture;
    private static readonly Lazy<string> AxeScriptPath = new(() => AccessibilityReportHelper.GetAxeScriptPath(null));

    // ReSharper disable once ConvertToPrimaryConstructor
    protected BasePageTest(WebApplicationSetupFixture fixture)
    {
        _fixture = fixture;
    }

    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions
        {
            BaseURL = _fixture.BaseUrl.TrimEnd('/'),
            IgnoreHTTPSErrors = true,
            ViewportSize = new() { Width = 1280, Height = 720 },
            Locale = "en-GB",
            TimezoneId = "Europe/London",
            JavaScriptEnabled = true,
            BypassCSP = true,               // we can't run axe for accessibility testing if CSP is active 
        };
    }

    public override async Task InitializeAsync()
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

    protected async Task WriteAccessibilityReport(string pageName)
    {
        await Page.AddScriptTagAsync(new PageAddScriptTagOptions { Path = AxeScriptPath.Value });

        var json = await Page.EvaluateAsync<string>(
            @"async () => {
                const results = await axe.run(document, {runOnly: {type: 'tag',values: ['wcag2aa']}});
                return JSON.stringify(results);
        }");

        var axeResult = JsonSerializer.Deserialize<AxeResults>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (axeResult is null)
        {
            return;
        }

        AccessibilityReportHelper.AddViolations(pageName, axeResult.Violations);

        // TODO: Wire in config to fail tests based on config switch. At the moment, we want them to
        // still pass, but a little config switch here would be handy.
        // Assert.False(axeResult.Violations.Any());
    }
}