using FluentAssertions;
using SAPPub.Tests.UI.Infrastructure;
using Xunit.Abstractions;

namespace SAPPub.Tests.UI;

public class HomePageTests : BasePageTest
{
    public HomePageTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task HomePage_LoadsSuccessfully()
    {
        // Arrange & Act
        var response = await GoToPageAysnc(string.Empty);

        // Assert
        response.Should().NotBeNull();
        response.Status.Should().Be(200);
    }

    [Fact]
    public async Task HomePage_HasCorrectTitle()
    {
        // Arrange
        await GoToPageAysnc(string.Empty);

        // Act
        var title = await Page.TitleAsync();

        // Assert
        title.Should().Match("School Profile*");
    }

    [Fact]
    public async Task HomePage_DisplaysMainHeading()
    {
        // Arrange
        await GoToPageAysnc(string.Empty);

        // Act
        var heading = await Page.Locator("h1").TextContentAsync();

        // Assert
        heading.Should().NotBeNullOrWhiteSpace();
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
        isVisible.Should().BeTrue("GOV.UK header should be visible");
        logoVisible.Should().BeTrue("GOV.UK SVG logo should be visible in the header");
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
        isVisible.Should().BeTrue($"Heading should be visible at {width}x{height}");
    }

    [Fact]
    public async Task HomePage_DeliberatelyFailingTestToCheckPlaywrightVideoAndScreenshotOutput()//To be removed after playwright functionality has been tested
    {
        // Arrange
        await GoToPageAysnc(string.Empty);

        // Act
        var title = await Page.TitleAsync();

        // Assert
        title.Should().Match("Text That Does Not Exist");
    }
}