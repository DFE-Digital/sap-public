using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using System.Text;
using System.Text.Json;

namespace SAPPub.Web.Tests.UI.Infrastructure;

public abstract class BasePageTest : PageTest
{
    private readonly WebApplicationSetupFixture _fixture;
    private static string? _axeScriptPath;

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
        var axeScriptPath = GetAxeScriptPath();

        await Page.AddScriptTagAsync(new PageAddScriptTagOptions { Path = axeScriptPath });

        var json = await Page.EvaluateAsync<string>("async () => JSON.stringify(await axe.run(document, {runonly:{ {type:'tag', values: ['wcag2a','wcag2aa']}}))");

        using var document = JsonDocument.Parse(json);
        var violations = document.RootElement.GetProperty("violations");

        var reportDirectory = Environment.GetEnvironmentVariable("ACCESSIBILITY_REPORT_DIR");
        if (string.IsNullOrWhiteSpace(reportDirectory))
        {
            reportDirectory = Path.Combine(AppContext.BaseDirectory, "accessibility-results");
        }

        Directory.CreateDirectory(reportDirectory);
        var reportPath = Path.Combine(reportDirectory, SanitizeFilename(pageName) + ".md");
        await File.WriteAllTextAsync(reportPath, BuildReport(pageName, violations));
    }

    private static string BuildReport(string pageName, JsonElement violations)
    {
        var builder = new StringBuilder();
        builder.AppendLine($"Accessibility report for {pageName}");

        if (violations.GetArrayLength() == 0)
        {
            builder.AppendLine("No violations found.");
            return builder.ToString();
        }

        foreach (var violation in violations.EnumerateArray()) {
            var violationId = violation.GetProperty("id").GetString();
            var violationHelp = violation.GetProperty("help").GetString();  
            builder.AppendLine($"* {violationId} {violationHelp}");
            
            if (!violations.TryGetProperty("nodes", out var nodes))
            {
                continue;
            }

            foreach (var node in nodes.EnumerateArray())
            {
                if (node.TryGetProperty("target", out var target))
                {
                    builder.AppendLine($"Target: {string.Join(", ", target.EnumerateArray().Select(a => a.GetString()))}");
                }
                if (node.TryGetProperty("failureSummary", out var failureSummary))
                { 
                    builder.AppendLine($"{failureSummary.ToString()}");
                }
            }
        }

        return builder.ToString();

    }

    private string SanitizeFilename(string value)
    {
        foreach (var invalidChar in Path.GetInvalidFileNameChars())
        {
            value = value.Replace(invalidChar, '-');
        }

        return value.Replace('/', '-').Replace('\\', '-').Replace(' ', '-');
    }

    private static string GetAxeScriptPath()
    {
        if (_axeScriptPath is not null)
        {
            return _axeScriptPath;
        }

        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory != null && !directory.GetFiles("*.sln").Any())
        {
            directory = directory.Parent;
        }

        if (directory is null)
        {
            throw new InvalidOperationException();      // todo message
        }

        _axeScriptPath = Path.Combine(directory.FullName, "SAPPub.Web", "node_modules", "axe-core", "axe.min.js");

        if (!File.Exists(_axeScriptPath))
        {
            throw new FileNotFoundException();
        }

        return _axeScriptPath;
    }
}