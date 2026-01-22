using Microsoft.Playwright;

namespace SAPPub.Web.Tests.UI.Helpers;

public class VerticalNavigationHelper(IPage page)
{
    private readonly IPage _page = page;

    public ILocator Nav => _page.Locator(".moj-side-navigation");

    public ILocator Items => _page.Locator(".moj-side-navigation__item");

    public ILocator ActiveItem => _page.Locator(".moj-side-navigation__item--active");

    public ILocator ActiveLink => ActiveItem.Locator("a");

    public async Task ShouldBeVisibleAsync()
    {
        Assert.True(await Nav.IsVisibleAsync(), "Vertical navigation should be visible");
    }

    public async Task ShouldHaveItemsCountAsync(int expectedCount)
    {
        Assert.Equal(expectedCount, await Items.CountAsync());//, $"Vertical navigation should have {expectedCount} items");
    }

    public async Task ShouldHaveOneActiveItemAsync()
    {
        //(await ActiveItem.CountAsync()).Should().Be(1, $"Vertical navigation should have 1 active item");
        Assert.Equal(1, await ActiveItem.CountAsync());
    }

    public async Task ShouldHaveActiveHrefAsync(string expectedHref)
    {
        //(await GetActiveHrefAsync()).Should().Be($"/{expectedHref}", $"Active item link href should match expected");
        Assert.Equal($"/{expectedHref}", await GetActiveHrefAsync());
    }

    public Task<string?> GetActiveHrefAsync() => ActiveLink.GetAttributeAsync("href");
    
}
