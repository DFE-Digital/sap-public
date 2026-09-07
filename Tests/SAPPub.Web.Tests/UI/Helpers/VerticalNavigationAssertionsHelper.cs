using Microsoft.Playwright;
using SAPPub.Playwright.Testing;

namespace SAPPub.Web.Tests.UI.Helpers;

public static class VerticalNavigationAssertionsHelper
{
    public static async Task ShouldBeVisibleAsync(this VerticalNavigationHelper Nav)
    {
        Assert.True(await Nav.Nav.IsVisibleAsync(), "Vertical navigation should be visible");
    }

    public static async Task ShouldHaveItemsCountAsync(this VerticalNavigationHelper verticalNavigationHelper, int expectedCount)
    {
        Assert.Equal(expectedCount, await verticalNavigationHelper.Items.CountAsync());//, $"Vertical navigation should have {expectedCount} items");
    }

    public static async Task ShouldHaveOneActiveItemAsync(this VerticalNavigationHelper verticalNavigationHelper)
    {
        //(await ActiveItem.CountAsync()).Should().Be(1, $"Vertical navigation should have 1 active item");
        Assert.Equal(1, await verticalNavigationHelper.ActiveItem.CountAsync());
    }

    public static async Task ShouldHaveActiveHrefAsync(this VerticalNavigationHelper verticalNavigationHelper, string expectedHref)
    {
        //(await GetActiveHrefAsync()).Should().Be($"/{expectedHref}", $"Active item link href should match expected");
        Assert.Equal($"/{expectedHref}", await verticalNavigationHelper.GetActiveHrefAsync());
    }
}
