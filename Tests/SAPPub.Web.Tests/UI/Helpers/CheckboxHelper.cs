using Microsoft.Playwright;

namespace SAPPub.Web.Tests.UI.Helpers;

public static class CheckboxHelper
{
    public static async Task<ILocator> CheckboxStrictAsync(
    this IPage page,
    string fieldsetHeading,
    string labelText)
    {
        var container = page.Locator("fieldset", new()
        {
            HasText = fieldsetHeading
        });

        var item = container
            .Locator(".govuk-checkboxes__item")
            .Filter(new() { HasText = labelText });

        var checkbox = item.Locator("input[type='checkbox']");

        var label = item.Locator("label");

        // enforce label is actually connected (GDS requirement)
        var id = await checkbox.GetAttributeAsync("id");
        var forAttr = await label.GetAttributeAsync("for");

        if (id == null || forAttr != id)
            throw new Exception($"GDS violation: label not linked to checkbox for '{labelText}'");

        return checkbox;
    }
}
