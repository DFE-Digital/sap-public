using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI;

[Collection("Playwright Tests")]
public class CookiePageTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private string _pageUrl = "Cookies/Preferences";

    [Fact]
    public async Task CookiePage_UserSelectsCookieSettings()
    {
        // Arrange: Clear cookies before navigating to the page
        await Page.Context.ClearCookiesAsync();

        // Arrange && Act
        var response = await Page.GotoAsync(_pageUrl);

        // Assert
        Assert.NotNull(response);
        var cookiesYesRadio = Page.Locator("#cookies-analytics-yes");
        var cookiesNoRadio = Page.Locator("#cookies-analytics-no");
        bool isNoRadioChecked = await cookiesNoRadio.IsCheckedAsync();
        bool isYesRadioChecked = await cookiesNoRadio.IsCheckedAsync();
        Assert.False(isNoRadioChecked);
        Assert.False(isYesRadioChecked);

        // Act
        await cookiesNoRadio.ClickAsync();
        var saveButton = Page.Locator("button:has-text(\"Save cookie setting\")");
        await saveButton.ClickAsync();

        // Assert
        cookiesNoRadio = Page.Locator("#cookies-analytics-no");
        isNoRadioChecked = await cookiesNoRadio.IsCheckedAsync();
        Assert.True(isNoRadioChecked, "The 'No' radio button should be selected.");


        // Act
        await cookiesYesRadio.ClickAsync();
        saveButton = Page.Locator("button:has-text(\"Save cookie setting\")");
        await saveButton.ClickAsync();

        // Assert
        Assert.NotNull(response);
        cookiesYesRadio = Page.Locator("#cookies-analytics-yes");
        isYesRadioChecked = await cookiesYesRadio.IsCheckedAsync();
        Assert.True(isYesRadioChecked, "The 'Yes' radio button should be selected.");
    }
}
