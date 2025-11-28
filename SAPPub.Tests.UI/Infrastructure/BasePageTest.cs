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
        Console.WriteLine(">>> PageTest.InitializeAsync RUN");

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

    public override async Task DisposeAsync()
    {
        Console.WriteLine(">>> PageTest.DisposeAsync RUN");

        await Context.Tracing.StopAsync(new()
        {
            Path = $"test-artifacts/traces/{Guid.NewGuid()}.zip"
        });

        await Page.ScreenshotAsync(new()
        {
            Path = $"test-artifacts/screenshots/{Guid.NewGuid()}.png",
            FullPage = true
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
