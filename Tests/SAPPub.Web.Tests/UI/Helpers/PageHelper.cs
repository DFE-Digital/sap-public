using Microsoft.Playwright;

namespace SAPPub.Web.Tests.UI.Helpers;

public static class PageHelper
{
    public static Task ClickButton(this IPage page, string buttonText)
    {
        var button = page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { NameString = buttonText }).First;
        return button.ClickAsync();
    }

    public static Task ClickLink(this IPage page, string linkText)
    {
        var link = page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { NameString = linkText }).First;
        return link.ClickAsync();
    }

    public static async Task<bool> HasErrorSummary(this IPage page)
    {
        return await page
            .Locator(".govuk-error-summary")
            .CountAsync() > 0;
    }

    public static Task<IReadOnlyList<string>> GetTableRowValuesAsync(
        this IPage page,
        string tableId,
        string rowHeader)
    {
        var row = page.Locator($"#{tableId} tbody tr")
            .Filter(new()
            {
                Has = page.Locator($"th:has-text('{rowHeader}')")
            });

        return row.Locator("td").AllInnerTextsAsync();
    }

    public static Task ExpandAccordionAsync(this IPage page, string label)
    {
        return page.GetByRole(AriaRole.Button, new()
        {
            Name = label
        }).ClickAsync();
    }

    public static Task ExpandDetailsAsync(this IPage page, string summaryText)
    {
        var summary = page
            .Locator("summary.govuk-details__summary")
            .Filter(new() { HasText = summaryText });

        return summary.ClickAsync();
    }

}
