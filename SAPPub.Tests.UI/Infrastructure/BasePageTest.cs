using System.Runtime.CompilerServices;
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;

namespace SAPPub.Tests.UI.Infrastructure
{
    [Collection("Playwright Tests")]
    public class BasePageTest : PageTest
    {
        // Will hold the current test method name (set when you call GoToPageAysnc)
        private string _currentTestMethodName = "UnknownTest";

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
            // Use the runtime type (derived test class) + method name + timestamp
            var className = GetType().Name;
            var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmssfff");
            var baseName = $"{className}.{_currentTestMethodName}_{timestamp}";

            // TRACE
            await Context.Tracing.StopAsync(new()
            {
                Path = $"SAPPub.Tests.UI/test-artifacts/traces/{baseName}.zip"
            });

            // SCREENSHOT
            try
            {
                await Page.ScreenshotAsync(new()
                {
                    Path = $"SAPPub.Tests.UI/test-artifacts/screenshots/{baseName}.png",
                    FullPage = true
                });
            }
            catch
            {
                // If Page is already closed, ignore
            }

            // VIDEO (rename underlying file if available)
            try
            {
                if (Page.Video is not null)
                {
                    var originalVideoPath = await Page.Video.PathAsync();
                    var newVideoPath =
                        $"SAPPub.Tests.UI/test-artifacts/videos/{baseName}.webm";

                    // Ensure directory exists (it should, but just in case)
                    Directory.CreateDirectory("SAPPub.Tests.UI/test-artifacts/videos");

                    File.Move(originalVideoPath, newVideoPath, true);
                }
            }
            catch
            {
                // If no video or cannot move, ignore – don't break the test run
            }

            await base.DisposeAsync();
        }

        // NOTE: only change is the extra optional parameter and the assignment
        protected Task<IResponse?> GoToPageAysnc(
            string relativeUrl,
            [CallerMemberName] string? testMethodName = null)
        {
            _currentTestMethodName = testMethodName ?? "UnknownTest";

            var baseUrl = Environment.GetEnvironmentVariable("BASE_URL")
                ?? "https://localhost:3000";

            return Page.GotoAsync($"{baseUrl}/{relativeUrl}");
        }
    }
}
