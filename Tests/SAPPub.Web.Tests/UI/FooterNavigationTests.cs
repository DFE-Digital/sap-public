using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI;

[Collection("Playwright Tests")]
public class FooterNavigationTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    [Fact]
    public async Task FooterNavigation_Displays_Accessibility_Link()
    {
        // Arrange
        await Page.GotoAsync(string.Empty);

        // Act
        var accessibilityLink = Page.Locator("#accessibility-link a");
        var isVisible = await accessibilityLink.IsVisibleAsync();
        var href = await accessibilityLink.GetAttributeAsync("href");

        // Assert
        Assert.True(isVisible, "Accessibility link should be visible");
        Assert.Equal("https://accessibility-statements.education.gov.uk/s/76", href);
    }

    [Fact]
    public async Task FooterNavigation_Displays_Cookies_Link()
    {
        // Arrange
        await Page.GotoAsync(string.Empty);

        // Act
        var accessibilityLink = Page.Locator("#cookies-link a");
        var isVisible = await accessibilityLink.IsVisibleAsync();
        var href = await accessibilityLink.GetAttributeAsync("href");

        // Assert
        Assert.True(isVisible, "Cookies link should be visible");
        Assert.Equal("/Cookies/Preferences", href);
    }

    [Fact]
    public async Task FooterNavigation_Displays_Privacy_Link()
    {
        // Arrange
        await Page.GotoAsync(string.Empty);

        // Act
        var accessibilityLink = Page.Locator("#privacy-link a");
        var isVisible = await accessibilityLink.IsVisibleAsync();
        var href = await accessibilityLink.GetAttributeAsync("href");

        // Assert
        Assert.True(isVisible, "Privacy link should be visible");
        Assert.Equal("https://www.gov.uk/government/organisations/department-for-education/about/personal-information-charter", href);
    }

    [Fact]
    public async Task FooterNavigation_Displays_TermsAndConditions_Link()
    {
        // Arrange
        await Page.GotoAsync(string.Empty);

        // Act
        var accessibilityLink = Page.Locator("#termsandconditions-link a");
        var isVisible = await accessibilityLink.IsVisibleAsync();
        var href = await accessibilityLink.GetAttributeAsync("href");

        // Assert
        Assert.True(isVisible, "TermsAndConditions link should be visible");
        Assert.Equal("/terms-and-conditions", href);
    }
}
