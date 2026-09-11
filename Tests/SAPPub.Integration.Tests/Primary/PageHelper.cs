using Microsoft.Playwright;

namespace SAPPub.Integration.Tests.Primary;

public static class PageHelper
{
    public static Task<IResponse?> GotoAcademicPerformanceSelectedYearLink(this IPage Page, string urlstring, string year = "current")
    {
        const string marker = "school/";
        var i = urlstring.IndexOf(marker);
        var j = urlstring.LastIndexOf('/');
        var previousYearPerformanceUrl = urlstring.Substring(i, j - i);
        return Page.GotoAsync($"{previousYearPerformanceUrl}/{year}");
    }

    public static Task<IResponse?> GotoPage(this IPage Page, string urlstring, string page)
    {
        const string marker = "/";
        var i = urlstring.LastIndexOf(marker);
        var urlStem = urlstring.Substring(0, i);
        return Page.GotoAsync($"{urlStem}/{page}");
    }
}
