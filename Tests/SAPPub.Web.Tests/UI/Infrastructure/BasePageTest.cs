using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using SAPPub.Web.Tests.UI.Models;
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
        var axeScriptPath = GetAxeScriptPath();

        await Page.AddScriptTagAsync(new PageAddScriptTagOptions { Path = axeScriptPath });

        var json = await Page.EvaluateAsync<string>(
            @"async () => JSON.stringify(await axe.run(document, {
            runOnly: {
                type: 'tag',
                values: ['wcag2a', 'wcag2aa']
            }
        }))");

        var axeResult = JsonSerializer.Deserialize<AxeResults>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (axeResult is null)
        {
            return;
        }

        string reportPath = GetReportPath(pageName);
        await File.WriteAllTextAsync(reportPath, BuildReport(pageName, axeResult.Violations));
       
        // TODO: Wire in config to fail tests based on config switch. At the moment, we want them to
        // still pass, but a little config switch here would be handy.
        Assert.False(axeResult.Violations.Any());
    }

    private static string GetReportPath(string pageName)
    {
        var reportDirectory = Environment.GetEnvironmentVariable("ACCESSIBILITY_REPORT_DIR");
        if (string.IsNullOrWhiteSpace(reportDirectory))
        {
            reportDirectory = Path.Combine(GetSolutionPath(), "Tests", "accessibility-results");
        }

        Directory.CreateDirectory(reportDirectory);
        var reportPath = Path.Combine(reportDirectory, SanitizeFilename(pageName) + ".md");
        return reportPath;
    }

    private static string BuildReport(string pageName, IList<AxeResult> violations)
    {
        if (!violations.Any())
        {
            return $"{pageName} violations - 0";
        }

        var builder = new StringBuilder(100);

        builder.AppendLine($"{pageName} violations - {violations.Count}");

        foreach (var violation in violations)
        {
            builder.AppendLine($" * {violation.Id} {violation.Help}");

            foreach (var node in violation.Nodes)
            {
                builder.AppendLine($"  Target: {string.Join(", ", node.Target.Select(a => a))}");
                builder.AppendLine($"  {node.FailureSummary}");
            }
        }

        return builder.ToString();
    }

    private static string SanitizeFilename(string value)
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

        var solutionPath = GetSolutionPath();

        _axeScriptPath = Path.Combine(solutionPath, "SAPPub.Web", "node_modules", "axe-core", "axe.min.js");

        if (!File.Exists(_axeScriptPath))
        {
            throw new FileNotFoundException();
        }

        return _axeScriptPath;
    }

    private static string GetSolutionPath()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory != null && !directory.GetFiles("*.sln").Any())
        {
            directory = directory.Parent;
        }

        if (directory is null)
        {
            throw new InvalidOperationException();      // todo message
        }

        return directory.FullName;
    }
}