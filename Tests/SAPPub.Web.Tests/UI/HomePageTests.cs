using SAPPub.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI;

public class HomePageTests : BasePageTest
{
    [Fact]
    public async Task HomePage_LoadsSuccessfully()
    {
        // Arrange & Act
        var response = await GoToPageAysnc(string.Empty);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task HomePage_HasCorrectTitle()
    {
        // Arrange
        await GoToPageAysnc(string.Empty);

        // Act
        var title = await Page.TitleAsync();

        // Assert
        Assert.Contains("School Profile*", title);
    }

    [Fact]
    public async Task HomePage_DisplaysMainHeading()
    {
        // Arrange
        await GoToPageAysnc(string.Empty);

        // Act
        var heading = await Page.Locator("h1").TextContentAsync();

        // Assert
        Assert.NotNull(heading.Replace(" ",""));
    }

    [Fact]
    public async Task HomePage_DisplaysGovUkHeader()
    {
        // Arrange
        await GoToPageAysnc(string.Empty);

        // Act
        // Locate the GOV.UK header element
        var header = Page.Locator("header.govuk-header");
        var isVisible = await header.IsVisibleAsync();

        // Locate the GOV.UK logotype SVG 
        var govUkLogo = Page.Locator("header.govuk-header svg[aria-label='GOV.UK']");
        var logoVisible = await govUkLogo.IsVisibleAsync();

        // Assert
        Assert.True(isVisible, "GOV.UK header should be visible");
        Assert.True(logoVisible, "GOV.UK SVG logo should be visible in the header");
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
        await GoToPageAysnc(string.Empty);
        var heading = Page.Locator("h1");
        var isVisible = await heading.IsVisibleAsync();

        // Assert
        Assert.True(isVisible, $"Heading should be visible at {width}x{height}");
        //isVisible.Should().BeTrue($"Heading should be visible at {width}x{height}");
    }
}