using Microsoft.Playwright;
using System.Text.RegularExpressions;

namespace SAPPub.E2E.Tests.Helpers;

public static class PageHelpers
{
    public static Task<IResponse?> GotoPage(this IPage Page, string urlstring, string page)
    {
        const string marker = "/";
        var i = urlstring.LastIndexOf(marker);
        var urlStem = urlstring.Substring(0, i);
        return Page.GotoAsync($"{urlStem}/{page}");
    }

    public static Task<IResponse?> GotoAcademicPerformanceSelectedYearLink(this IPage Page, string urlstring, string year = "current")
    {
        const string marker = "school/";
        var i = urlstring.IndexOf(marker);
        var j = urlstring.LastIndexOf('/');
        var previousYearPerformanceUrl = urlstring.Substring(i, j - i);
        return Page.GotoAsync($"{previousYearPerformanceUrl}/{year}");
    }

    public static async Task ExpandElement(this IPage Page, string dataTestid)
    {
        var element = Page.Locator($"[data-testid='{dataTestid}']");
        Assert.Equal(1, await element.CountAsync());
        await element.Locator("summary.govuk-details__summary").ClickAsync();
    }

    public static async Task<IEnumerable<string>?> GetScoreFromParagraphAsync(this IPage Page, string dataTestid, string textString)
    {
        var section = Page.Locator($"[data-testid='{dataTestid}']");
        if(await section.CountAsync() == 0)
        {
            section = Page.Locator($"#{dataTestid}");
        }
        var p = section.Locator("p.govuk-body", new() { HasTextString = textString });
        var pCount = await p.CountAsync();
        Assert.True(pCount == 1, $"Paragraph count mismatch looking for paragraph text: {textString}. Actual count: {pCount}, expected 1");
        var input = await p.InnerTextAsync();
        var match = Regex.Matches(input, @"[+-]?\d+(?:\.\d+)?")
                 .Cast<Match>();

        return match.Select(m => m.Value);
    }
}
