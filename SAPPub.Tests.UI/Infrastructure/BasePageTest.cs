using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;

namespace SAPPub.Tests.UI.Infrastructure;

public class BasePageTest : PageTest
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

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        await Context.Tracing.StartAsync(new()
        {
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });
    }

    public override async Task DisposeAsync()
    {
        Directory.CreateDirectory("test-artifacts/screenshots");
        Directory.CreateDirectory("test-artifacts/traces");

        // Screenshot every time (small cost)
        await Page.ScreenshotAsync(new()
        {
            Path = $"test-artifacts/screenshots/{Guid.NewGuid()}.png",
            FullPage = true
        });

        // Stop tracing and save trace file
        await Context.Tracing.StopAsync(new()
        {
            Path = $"test-artifacts/traces/{Guid.NewGuid()}.zip"
        });

        await base.DisposeAsync();
    }


    protected async Task<IResponse?> GoToPageAysnc(string relativeUrl)
    {
        var baseUrl = Environment.GetEnvironmentVariable("BASE_URL")
            ?? "https://localhost:3000";

        return await Page.GotoAsync($"{baseUrl}/{relativeUrl}");
    }
}
