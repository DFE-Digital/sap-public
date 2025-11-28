using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using Xunit;

namespace SAPPub.Tests.UI.Infrastructure;

public class BasePageTest : PageTest, IAsyncLifetime
{
    public BasePageTest() : base() { }

    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true,
            RecordVideoDir = "test-artifacts/videos",
            RecordVideoSize = new() { Width = 1280, Height = 720 }
        };
    }

    // ⭐ IMPORTANT: use `new` (NOT override)
    public new async Task InitializeAsync()
    {
        Console.WriteLine(">>> IAsyncLifetime.InitializeAsync called");

        // Run Playwright’s built-in init
        await base.InitializeAsync();

        Directory.CreateDirectory("test-artifacts/screenshots");
        Directory.CreateDirectory("test-artifacts/traces");
        Directory.CreateDirectory("test-artifacts/videos");

        await Context.Tracing.StartAsync(new()
        {
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });
    }

    // ⭐ IMPORTANT: use `new` (NOT override)
    public new async Task DisposeAsync()
    {
        Console.WriteLine(">>> IAsyncLifetime.DisposeAsync called");

        // Write trace
        await Context.Tracing.StopAsync(new()
        {
            Path = $"test-artifacts/traces/{Guid.NewGuid()}.zip"
        });

        // Write screenshot
        await Page.ScreenshotAsync(new()
        {
            Path = $"test-artifacts/screenshots/{Guid.NewGuid()}.png",
            FullPage = true
        });

        // Run Playwright internal cleanup
        await base.DisposeAsync();
    }

    protected async Task<IResponse?> GoToPageAysnc(string relativeUrl)
    {
        var baseUrl = Environment.GetEnvironmentVariable("BASE_URL")
            ?? "https://localhost:3000";

        return await Page.GotoAsync($"{baseUrl}/{relativeUrl}");
    }
}
