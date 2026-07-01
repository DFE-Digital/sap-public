using Microsoft.Playwright;

namespace SAPPub.Web.Tests.UI.Helpers;

public class PaginationNavigationHelper(IPage page)
{
    private readonly IPage _page = page;

    public ILocator? Nav => _page.Locator(".govuk-pagination");

    public Task ClickNextLinkAsync() => _page.Locator(".govuk-pagination__next a.govuk-pagination__link").ClickAsync();

    public Task ClickPreviousLinkAsync() => _page.Locator(".govuk-pagination__prev a.govuk-pagination__link").ClickAsync();
}
