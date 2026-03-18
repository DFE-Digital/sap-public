using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI;

[Collection("Playwright Tests")]
public class HomePageTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    [Fact]
    public async Task HomePage_LoadsSuccessfully()
    {
        // Arrange & Act
        var response = await Page.GotoAsync(string.Empty);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task HomePage_HasCorrectTitle()
    {
        // Arrange
        await Page.GotoAsync(string.Empty);

        // Act
        var title = await Page.TitleAsync();

        // Assert
        Assert.Contains("School Profiles", title);
    }

    [Fact]
    public async Task HomePage_DisplaysMainHeading()
    {
        // Arrange
        await Page.GotoAsync(string.Empty);

        // Act
        var heading = await Page.Locator("h1").TextContentAsync();

        // Assert
        Assert.NotNull(heading);
        Assert.NotEmpty(heading!.Trim());
    }

    [Theory]
    [InlineData(1920, 1080)] // Desktop
    [InlineData(768, 1024)]  // Tablet
    [InlineData(375, 667)]   // Mobile
    public async Task HomePage_IsResponsive(int width, int height)
    {
        // Arrange        
        await Page.SetViewportSizeAsync(width, height);

        // Act
        await Page.GotoAsync(string.Empty);
        var heading = Page.Locator("h1");
        var isVisible = await heading.IsVisibleAsync();

        // Assert
        Assert.True(isVisible, $"Heading should be visible at {width}x{height}");
    }
}