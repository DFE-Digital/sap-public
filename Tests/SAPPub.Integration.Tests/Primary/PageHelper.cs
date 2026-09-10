using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Playwright;

namespace SAPPub.Integration.Tests.Primary;

public static class PageHelper
{
    public static Task<IResponse> ClickAcademicPerformanceLinkAsync(this IPage Page)
    {
        var response = Page.RunAndWaitForResponseAsync(
            async () =>
            {
                await Page.GetByRole(AriaRole.Link, new() { Name = "Primary academic performance" }).ClickAsync();
            },
            response => response.Url.Contains("/primary-performance/pupil-progress/current") && response.Status == 200
        );
        return response;
    }

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
        var previousYearPerformanceUrl = urlstring.Substring(0, i);
        return Page.GotoAsync($"{previousYearPerformanceUrl}/{page}");
    }
}
