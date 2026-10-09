using SAPPub.Playwright.Testing;
using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.Areas.Profiles;

[Collection("Playwright Tests")]
public class AttendancePageTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private string _pageUrl = "school/105574/loreto-high-school-chorlton/attendance";

    [Fact]
    public async Task AttendancePage_AccessibilityNavigation()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(_pageUrl);

        //bypass cookies
        await Page.Keyboard.PressAsync("Tab");
        await Page.Keyboard.PressAsync("Tab");
        await Page.Keyboard.PressAsync("Tab");

        //start of page - _Layout page
        await Page.Keyboard.PressAsync("Tab");

        var activeElement = await Page.EvaluateHandleAsync("document.activeElement");
        var prop = await activeElement.GetPropertyAsync("textContent");
        string textContent = await prop.JsonValueAsync<string>();
        Assert.Equal("Skip to main content", textContent);

        //start of content - Default page
        await Page.Keyboard.PressAsync("Enter");
        await Page.Keyboard.PressAsync("Tab");

        activeElement = await Page.EvaluateHandleAsync("document.activeElement");
        prop = await activeElement.GetPropertyAsync("textContent");
        textContent = await prop.JsonValueAsync<string>();
        Assert.Equal("Skip contents", textContent);

        //start of right side content - Attendance page
        await Page.Keyboard.PressAsync("Enter");
        await Page.Keyboard.PressAsync("Tab");

        activeElement = await Page.EvaluateHandleAsync("document.activeElement");
        prop = await activeElement.GetPropertyAsync("href");
        string href = await prop.JsonValueAsync<string>();
        Assert.Equal("http://www.test.co.uk/", href);
    }

    [Fact]
    public async Task AttendancePage_Displays_VerticalNavigation()
    {
        var nav = new VerticalNavigationHelper(Page);
        await Page.GotoAsync(_pageUrl);

        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveOneActiveItemAsync();
        await nav.ShouldHaveActiveHrefAsync(_pageUrl);
    }
}
