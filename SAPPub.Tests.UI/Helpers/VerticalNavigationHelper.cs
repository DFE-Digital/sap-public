using FluentAssertions;
using Microsoft.Playwright;

namespace SAPPub.Tests.UI.Helpers;

public class VerticalNavigationHelper(IPage page)
{
    private readonly IPage _page = page;

    public ILocator Nav => _page.Locator(".dfe-vertical-nav");

    public ILocator Items => _page.Locator(".dfe-vertical-nav__section-item");

    public ILocator Links => _page.Locator(".dfe-vertical-nav__link");

    public ILocator ActiveItem => _page.Locator(".dfe-vertical-nav__section-item--current");

    public ILocator ActiveLink => ActiveItem.Locator("a");

    public async Task ShouldBeVisibleAsync()
    {
        (await Nav.IsVisibleAsync()).Should().BeTrue("Vertical navigation should be visible");
    }

    public async Task ShouldHaveItemsCountAsync(int expectedCount)
    {
        (await Items.CountAsync()).Should().Be(expectedCount, $"Vertical navigation should have {expectedCount} items");
    }

    public async Task ShouldHaveOneActiveItemAsync()
    {
        (await ActiveItem.CountAsync()).Should().Be(1, $"Vertical navigation should have 1 active item");
    }

    public async Task ShouldHaveActiveHrefAsync(string expectedHref)
    {
        (await GetActiveHrefAsync()).Should().Be($"/{expectedHref}", $"Active item link href should match expected");
    }

    public Task<string?> GetActiveHrefAsync() => ActiveLink.GetAttributeAsync("href");

    public Task<string> GetLinkTextAsync(int index) => Links.Nth(index).InnerTextAsync();
}
