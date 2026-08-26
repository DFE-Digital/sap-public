using Microsoft.Playwright;
using System.Text.RegularExpressions;

namespace SAPPub.IntegrationTests.Helpers;

public static class PageHelpers
{
    public static async Task ExpandElement(this IPage Page, string dataTestid)
    {
        var element = Page.Locator($"[data-testid='{dataTestid}']");
        Assert.Equal(1, await element.CountAsync());
        await element.Locator("summary.govuk-details__summary").ClickAsync();
    }

    public static async Task<IEnumerable<string>?> GetScoreFromParagraphAsync(this IPage Page, string dataTestid, string textString)
    {
        var section = Page.Locator($"[data-testid='{dataTestid}']");
        var p = section.Locator("p.govuk-body", new() { HasTextString = textString });
        Assert.True(await p.CountAsync() == 1, $"Paragraph count mismatch: {textString}");
        var input = await p.InnerTextAsync();
        var match = Regex.Matches(input, @"[+-]?\d+(?:\.\d+)?")
                 .Cast<Match>();

        return match.Select(m => m.Value);
    }
}
