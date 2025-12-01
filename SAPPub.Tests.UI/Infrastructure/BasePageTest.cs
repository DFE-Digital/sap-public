using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;

namespace SAPPub.Tests.UI.Infrastructure;

[Collection("Playwright Tests")]
public class BasePageTest : PageTest
{
    private bool _testFailed = false;

    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true,
            RecordVideoDir = "SAPPub.Tests.UI/test-artifacts/videos",
            RecordVideoSize = new() { Width = 1280, Height = 720 }
        };
    }

    public override async Task InitializeAsync()
    {
        Directory.CreateDirectory("SAPPub.Tests.UI/test-artifacts/screenshots");
        Directory.CreateDirectory("SAPPub.Tests.UI/test-artifacts/traces");
        Directory.CreateDirectory("SAPPub.Tests.UI/test-artifacts/videos");

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
        var id = Guid.NewGuid().ToString();

        try
        {
            // Try to stop tracing (will succeed only on passed tests)
            await Context.Tracing.StopAsync(new()
            {
                Path = $"SAPPub.Tests.UI/test-artifacts/traces/{id}.zip"
            });

            // Passed test → save nothing else
        }
        catch
        {
            // Failed test → mark flag & save screenshot
            _testFailed = true;

            await Page.ScreenshotAsync(new()
            {
                Path = $"SAPPub.Tests.UI/test-artifacts/screenshots/{id}.png",
                FullPage = true
            });
        }

        await base.DisposeAsync();
    }

    protected async Task<IResponse?> GoToPageAysnc(string relativeUrl)
    {
        var baseUrl = Environment.GetEnvironmentVariable("BASE_URL")
            ?? "https://localhost:3000";

        return await Page.GotoAsync($"{baseUrl}/{relativeUrl}");
    }
}
