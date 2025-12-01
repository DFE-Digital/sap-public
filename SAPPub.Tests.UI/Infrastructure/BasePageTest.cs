using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using Xunit;

namespace SAPPub.Tests.UI.Infrastructure
{
    // Required to disable parallel test execution
    [Collection("Playwright Tests")]
    public class BasePageTest : PageTest, IAsyncLifetime
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

        // This is called by xUnit BEFORE each test
        public async Task InitializeAsync()
        {
            // Ensure artifact directories exist
            Directory.CreateDirectory("SAPPub.Tests.UI/test-artifacts/screenshots");
            Directory.CreateDirectory("SAPPub.Tests.UI/test-artifacts/traces");
            Directory.CreateDirectory("SAPPub.Tests.UI/test-artifacts/videos");

            // Let Playwright.PageTest initialize the browser/context/page
            await base.InitializeAsync();

            // Start Playwright tracing
            await Context.Tracing.StartAsync(new()
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });
        }

        // This is called by xUnit AFTER each test
        public async Task DisposeAsync()
        {
            string id = Guid.NewGuid().ToString();

            // Stop tracing and write trace file
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

            // Dispose Playwright.PageTest resources
            await base.DisposeAsync();
        }

        protected async Task<IResponse?> GoToPageAysnc(string relativeUrl)
        {
            var baseUrl = Environment.GetEnvironmentVariable("BASE_URL")
                ?? "https://localhost:3000";

            return await Page.GotoAsync($"{baseUrl}/{relativeUrl}");
        }
    }
}
