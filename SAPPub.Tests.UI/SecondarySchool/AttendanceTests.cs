using Microsoft.Playwright;

namespace SAPPub.Tests.UI.SecondarySchool;

public class AttendancePageTests : IAsyncLifetime
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private readonly string _baseUrl;
    private string _pageUrl = null!;

    public AttendancePageTests()
    {
        _baseUrl = Environment.GetEnvironmentVariable("BASE_URL")
                   ?? "https://localhost:3000";
    }
    public async Task InitializeAsync()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });

        _pageUrl = $"{_baseUrl}/school/1/kes/secondary/attendance";
    }

    public async Task DisposeAsync()
    {
        if (_browser != null)
        {
            await _browser.CloseAsync();
        }
        _playwright?.Dispose();
    }

    [Fact]
    public async Task AttendancePage_LoadsSuccessfully()
    {
        // Arrange
        var page = await _browser!.NewPageAsync();

        // Act
        var response = await page.GotoAsync(_pageUrl);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);

        await page.CloseAsync();
    }

    [Fact]
    public async Task AttendancePage_HasCorrectTitle()
    {
        // Arrange
        var page = await _browser!.NewPageAsync();
        await page.GotoAsync(_pageUrl);

        // Act
        var title = await page.TitleAsync();

        // Assert
        Assert.Contains("Attendance", title);

        await page.CloseAsync();
    }

    [Fact]
    public async Task Attendance_DisplaysMainHeading()
    {
        // Arrange
        var page = await _browser!.NewPageAsync();
        await page.GotoAsync(_pageUrl);

        // Act
        var heading = await page.Locator("h1").TextContentAsync();

        // Assert
        Assert.NotNull(heading);
        Assert.NotEmpty(heading);

        await page.CloseAsync();
    }
}
