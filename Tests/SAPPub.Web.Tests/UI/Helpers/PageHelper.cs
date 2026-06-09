using Microsoft.Playwright;

namespace SAPPub.Web.Tests.UI.Helpers;

public static class PageHelper
{
    public static Task ClickButton(this IPage page, string buttonText)
    {
        var button = page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { NameString = buttonText }).First;
        return button.ClickAsync();
    }

    public static async Task<bool> HasErrorSummary(this IPage page)
    {
        return await page
            .Locator(".govuk-error-summary")
            .CountAsync() > 0;
    }
}
