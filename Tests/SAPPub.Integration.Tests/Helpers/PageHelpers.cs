using Microsoft.Playwright;
using System.Text.RegularExpressions;

namespace SAPPub.IntegrationTests.Helpers;

public static class PageHelpers
{
    public static async Task<IEnumerable<string>?> GetScoreFromParagraphAsync(this IPage Page, string dataTestid, string textString)
    {
        var card = Page.Locator($"[data-testid='{dataTestid}']");
        var p = card.Locator("p.govuk-body", new() { HasTextString = textString });
        var input = await p.InnerTextAsync();
        var match = Regex.Matches(input, @"[+-]?\d+(?:\.\d+)?")
                 .Cast<Match>();

        return match.Select(m => m.Value);
    }
}
