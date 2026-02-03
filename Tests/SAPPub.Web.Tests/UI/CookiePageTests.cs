using SAPPub.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI;

public class CookiePageTests : BasePageTest
{
    private string _pageUrl = "Cookies/Preferences";

    [Fact]
    public async Task CookiesBanner_LoadsSuccessfully()
    {
        // Arrange: Clear cookies before navigating to the page
        await Page.Context.ClearCookiesAsync();

        // Arrange && Act
        var response = await GoToPageAysnc(_pageUrl);

        // Assert
        Assert.NotNull(response);
        var cookiesNoRadio = Page.Locator("#cookies-analytics");
        bool isNoRadioChecked = await cookiesNoRadio.IsCheckedAsync();
        Assert.True(isNoRadioChecked, "The 'No' radio button should be selected.");

        // Act
        var cookiesYesRadio = Page.Locator("#cookies-analytics-2");
        await cookiesYesRadio.ClickAsync();

        // Act
        var saveButton = Page.Locator("button:has-text(\"Save cookie setting\")");
        await saveButton.ClickAsync();

        // Assert
        Assert.NotNull(response);
        cookiesYesRadio = Page.Locator("#cookies-analytics-2");
        bool isYesRadioChecked = await cookiesYesRadio.IsCheckedAsync();
        Assert.True(isYesRadioChecked, "The 'Yes' radio button should be selected.");

    }
}
