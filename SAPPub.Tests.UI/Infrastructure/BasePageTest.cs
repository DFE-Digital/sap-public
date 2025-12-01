using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;

namespace SAPPub.Tests.UI.Infrastructure
{
    [Collection("Playwright Tests")]
    public class BasePageTest : PageTest
    {
        // One unique folder per test run
        private static readonly string RunId = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
        private static string Root => $"SAPPub.Tests.UI/test-artifacts/{RunId}";

        public override BrowserNewContextOptions ContextOptions()
        {
            return new BrowserNewContextOptions
            {
                IgnoreHTTPSErrors = true,
                RecordVideoDir = $"{Root}/videos",
                RecordVideoSize = new() { Width = 1280, Height = 720 }
            };
        }

        public override async Task InitializeAsync()
        {
            // Ensure folders exist
            Directory.CreateDirectory($"{Root}/screenshots");
            Directory.CreateDirectory($"{Root}/traces");
            Directory.CreateDirectory($"{Root}/videos");

            await base.InitializeAsync();

            // Start tracing (does not write a file yet)
            await Context.Tracing.StartAsync(new()
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });
        }

        public override async Task DisposeAsync()
        {
            string id = Guid.NewGuid().ToString();

            await Context.Tracing.StopAsync(new()
            {
                Path = $"{Root}/traces/{id}.zip"
            });

            await Page.ScreenshotAsync(new()
            {
                Path = $"{Root}/screenshots/{id}.png",
                FullPage = true
            });

            await base.DisposeAsync();
        }

        protected Task<IResponse?> GoToPageAysnc(string relativeUrl)
        {
            var baseUrl = Environment.GetEnvironmentVariable("BASE_URL")
                ?? "https://localhost:3000";

            return Page.GotoAsync($"{baseUrl}/{relativeUrl}");
        }
    }
}
