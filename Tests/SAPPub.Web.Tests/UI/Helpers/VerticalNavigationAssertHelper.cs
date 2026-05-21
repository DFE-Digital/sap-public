using AngleSharp.Dom;

namespace SAPPub.Web.Tests.UI.Helpers;

public class VerticalNavigationAssertHelper(IDocument doc)
{
    private readonly IDocument _page = doc;

    public IElement? Nav => _page.QuerySelector(".moj-side-navigation");

    public IHtmlCollection<IElement> Items => _page.QuerySelectorAll(".moj-side-navigation__item");

    public IElement? ActiveItem => _page.QuerySelector(".moj-side-navigation__item--active");

    public IElement? ActiveLink => ActiveItem?.QuerySelector("a");

    public void ShouldBeVisibleAsync()
    {
        Assert.NotNull(Nav);
    }

    public void ShouldHaveItemsCountAsync(int expectedCount)
    {
        Assert.Equal(expectedCount, Items.Length);
    }

    public void ShouldHaveOneActiveItemAsync()
    {
        Assert.NotNull(ActiveItem);
    }

    public void ShouldHaveActiveHrefAsync(string expectedHref)
    {
        Assert.Equal($"{expectedHref}", GetActiveHref());
    }

    public string? GetActiveHref() => ActiveLink?.GetAttribute("href");
}
