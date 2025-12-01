using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using Xunit.Abstractions;

namespace SAPPub.Tests.UI.Infrastructure;

[Collection("Playwright Tests")]
public class BasePageTest : PageTest
{
    private readonly string _testName;

    public BasePageTest(ITestOutputHelper output)
        : base()
    {
        // Extract fully qualified test name from xUnit output helper
        _testName = ExtractTestName(output);
    }

    private string ExtractTestName(ITestOutputHelper output)
    {
        var type = output.GetType();
        var testMember = type.GetProperty("TestMember")?.GetValue(output);

        if (testMember == null)
            return $"UnknownTest_{Guid.NewGuid()}";

        var testDisplayName = testMember.GetType().GetProperty("DisplayName")?.GetValue(testMember) as string;

        // Clean invalid filename chars
        foreach (var c in Path.GetInvalidFileNameChars())
            testDisplayName = testDisplayName?.Replace(c, '_');

        return testDisplayName ?? $"UnknownTest_{Guid.NewGuid()}";
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
        var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");

        // TRACE
        await Context.Tracing.StopAsync(new()
        {
            Path = $"SAPPub.Tests.UI/test-artifacts/traces/{_testName}_{timestamp}.zip"
        });

        // SCREENSHOT (rename)
        await Page.ScreenshotAsync(new()
        {
            Path = $"SAPPub.Tests.UI/test-artifacts/screenshots/{_testName}_{timestamp}.png",
            FullPage = true
        });

        // VIDEO (Playwright will auto-generate .webm files)
        // We rename the video file AFTER close
        try
        {
            var videoPath = await Page.Video.PathAsync();
            var newVideoPath =
                $"SAPPub.Tests.UI/test-artifacts/videos/{_testName}_{timestamp}.webm";

            File.Move(videoPath, newVideoPath, true);
        }
        catch
        {
            // If page/video is null or missing, safely ignore
        }

        await base.DisposeAsync();
    }

    protected Task<IResponse?> GoToPageAysnc(string relativeUrl)
    {
        var baseUrl = Environment.GetEnvironmentVariable("BASE_URL")
            ?? "https://localhost:3000";

        return Page.GotoAsync($"{baseUrl}/{relativeUrl}");
    }
}
