using Microsoft.Playwright;

namespace SAPPub.Playwright.Testing;

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

    public static async Task<List<IReadOnlyList<string>>> GetTableRowsValuesByTableCaptionAsync(
    this IPage page,
    string captionText)
    {
        var table = page.Locator("table")
            .Filter(new()
            {
                Has = page.Locator($"caption:text-is('{captionText}')")
            });

        var rows = table.Locator("tbody tr");
        var rowCount = await rows.CountAsync();
        var result = new List<IReadOnlyList<string>>();
        for (var i = 0; i < rowCount; i++)
        {
            var row = rows.Nth(i);
            result.Add(await rows.Nth(i).Locator("td").AllInnerTextsAsync());
        }

        return result;
    }

    public static Task<IReadOnlyList<string>> GetTableRowValuesAsync(
        this IPage page,
        string tableId,
        string rowHeader)
    {
        var id = tableId.StartsWith("#") ? tableId : $"#{tableId}";
        var row = page.Locator($"{id} tbody tr")
            .Filter(new()
            {
                HasText = rowHeader
            });

        return row.Locator("td").AllInnerTextsAsync();
    }

    public static Task<IReadOnlyList<string>> GetTableRowValuesAsync(
        this IPage page,
        string tableId,
        int rowNumber)
    {
        var id = tableId.StartsWith("#") ? tableId : $"#{tableId}";
        var row = page.Locator($"{id} tbody tr")
            .Nth(rowNumber);

        return row.Locator("td").AllInnerTextsAsync();
    }

    public static async Task ExpandAccordionByIdAsync(this IPage page, string id)
    {
        id = id.StartsWith("#") ? id : $"#{id}";
        var sectionLocator = page.Locator($"{id}");
        var button = sectionLocator.Locator(".govuk-accordion__show-all");
        var isExpanded = await button.GetAttributeAsync("aria-expanded");
        if (isExpanded != "true")
        {
            await button.ClickAsync();
        }
    }

    public static Task ExpandDetailsAsync(this IPage page, string summaryText)
    {
        var summary = page
            .Locator("summary.govuk-details__summary")
            .Filter(new() { HasText = summaryText });

        return summary.ClickAsync();
    }

    public static async Task ExpandDetailsByIdAsync(this IPage page, string id)
    {
        id = id.StartsWith("#") ? id : $"#{id}";
        var sectionLocator = page.Locator($"details{id}");

        if (!await sectionLocator.GetAttributeAsync("open").ContinueWith(t => t.Result != null))
        {
            await sectionLocator.Locator("summary").ClickAsync();
        }
    }
}
