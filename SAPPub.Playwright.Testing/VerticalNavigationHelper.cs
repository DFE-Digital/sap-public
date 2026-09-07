using Microsoft.Playwright;

namespace SAPPub.Playwright.Testing;

public class VerticalNavigationHelper(IPage page)
{
    private readonly IPage _page = page;

    public ILocator Nav => _page.Locator(".moj-side-navigation");

    public ILocator Items => _page.Locator(".moj-side-navigation__item");

    public ILocator ActiveItem => _page.Locator(".moj-side-navigation__item--active");

    public ILocator ActiveLink => ActiveItem.Locator("a");

    public ILocator GetItem(string label) => Nav.Locator(".moj-side-navigation__item a").Filter(new() { HasText = label });

    public Task<string?> GetActiveHrefAsync() => ActiveLink.GetAttributeAsync("href");
}
