using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;

namespace SAPPub.Tests.UI.Infrastructure;

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
            RecordVideoDir = "SAPPub.Tests.UI/test-artifacts/videos",
            RecordVideoSize = new() { Width = 1280, Height = 720 }
        };
    }

    // Called automatically before EACH test
    public override async Task InitializeAsync()
    {
        // Ensure directories exist
        Directory.CreateDirectory("SAPPub.Tests.UI/test-artifacts/screenshots");
        Directory.CreateDirectory("SAPPub.Tests.UI/test-artifacts/traces");
        Directory.CreateDirectory("SAPPub.Tests.UI/test-artifacts/videos");

        await base.InitializeAsync();

        // Start tracing
        await Context.Tracing.StartAsync(new()
        {
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });
    }

    // Called automatically AFTER each test
    public override async Task DisposeAsync()
    {
        var id = Guid.NewGuid().ToString();

        // Save trace
        await Context.Tracing.StopAsync(new()
        {
            Path = $"SAPPub.Tests.UI/test-artifacts/traces/{id}.zip"
        });

        // Save screenshot
        await Page.ScreenshotAsync(new()
        {
            Path = $"SAPPub.Tests.UI/test-artifacts/screenshots/{id}.png",
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
