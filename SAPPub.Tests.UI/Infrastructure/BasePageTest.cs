using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;

namespace SAPPub.Tests.UI.Infrastructure;

[Collection("Playwright Tests")]
public class BasePageTest : PageTest
{
    public BasePageTest() : base()
    {
    }

    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true,
            // Playwright will drop videos here
            RecordVideoDir = "SAPPub.Tests.UI/test-artifacts/videos",
            RecordVideoSize = new() { Width = 1280, Height = 720 }
        };
    }

    public override async Task InitializeAsync()
    {
        // Make sure folders exist
        Directory.CreateDirectory("SAPPub.Tests.UI/test-artifacts/screenshots");
        Directory.CreateDirectory("SAPPub.Tests.UI/test-artifacts/traces");
        Directory.CreateDirectory("SAPPub.Tests.UI/test-artifacts/videos");

        // Let PageTest do its thing (creates Context + Page)
        await base.InitializeAsync();

        // Start tracing for every test
        await Context.Tracing.StartAsync(new()
        {
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });
    }

    public override async Task DisposeAsync()
    {
        var id = Guid.NewGuid().ToString();

        // Stop tracing and save a trace for every test
        try
        {
            await Context.Tracing.StopAsync(new()
            {
                Path = $"SAPPub.Tests.UI/test-artifacts/traces/{id}.zip"
            });
        }
        catch
        {
            // If tracing is already stopped, ignore
        }

        // Save a full-page screenshot for every test
        try
        {
            await Page.ScreenshotAsync(new()
            {
                Path = $"SAPPub.Tests.UI/test-artifacts/screenshots/{id}.png",
                FullPage = true
            });
        }
        catch
        {
            // If the page is already closed, ignore
        }

        await base.DisposeAsync();
    }

    // Backwards-compatible helper with the original typo
    protected Task<IResponse?> GoToPageAysnc(string relativeUrl)
        => GoToPageAsync(relativeUrl);

    // Correctly spelled helper – you can migrate tests to this over time
    protected async Task<IResponse?> GoToPageAsync(string relativeUrl)
    {
        var baseUrl = Environment.GetEnvironmentVariable("BASE_URL")
            ?? "https://localhost:3000";

        return await Page.GotoAsync($"{baseUrl}/{relativeUrl}");
    }
}
