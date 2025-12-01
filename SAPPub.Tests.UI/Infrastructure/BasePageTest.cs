using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;

namespace SAPPub.Tests.UI.Infrastructure;

[Collection("Playwright Tests")]
public class BasePageTest : PageTest, IAsyncLifetime
{
    private bool _testFailed = false;
    private string _tracePath = "";
    private string _videoDir = "SAPPub.Tests.UI/test-artifacts/videos";

    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true,
            RecordVideoDir = _videoDir,
            RecordVideoSize = new() { Width = 1280, Height = 720 }
        };
    }

    public override async Task InitializeAsync()
    {
        Directory.CreateDirectory("SAPPub.Tests.UI/test-artifacts/screenshots");
        Directory.CreateDirectory("SAPPub.Tests.UI/test-artifacts/traces");
        Directory.CreateDirectory(_videoDir);

        await base.InitializeAsync();

        // Start tracing and remember path
        _tracePath = $"SAPPub.Tests.UI/test-artifacts/traces/{Guid.NewGuid()}.zip";

        await Context.Tracing.StartAsync(new()
        {
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });
    }

    public override async Task DisposeAsync()
    {
        try
        {
            // Let Playwright dispose first — if the test failed, this throws
            await base.DisposeAsync();
        }
        catch
        {
            _testFailed = true;
        }

        // We must now stop tracing — not part of failure detection
        try
        {
            await Context.Tracing.StopAsync(new() { Path = _tracePath });
        }
        catch
        {
            // tracing might already be stopped, ignore
        }

        if (_testFailed)
        {
            // Save screenshot for failed test
            await Page.ScreenshotAsync(new()
            {
                Path = $"SAPPub.Tests.UI/test-artifacts/screenshots/{Guid.NewGuid()}.png",
                FullPage = true
            });
        }
        else
        {
            // Clean up trace for passed tests
            if (File.Exists(_tracePath))
                File.Delete(_tracePath);

            // Clean up video for passed tests
            foreach (var file in Directory.GetFiles(_videoDir, "*.webm"))
                File.Delete(file);
        }
    }

    protected async Task<IResponse?> GoToPageAysnc(string relativeUrl)
    {
        var baseUrl = Environment.GetEnvironmentVariable("BASE_URL")
            ?? "https://localhost:3000";

        return await Page.GotoAsync($"{baseUrl}/{relativeUrl}");
    }
}
