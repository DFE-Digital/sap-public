using Deque.AxeCore.Commons;
using Deque.AxeCore.Playwright;
using Microsoft.Playwright;
using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI;

[Collection("Playwright Tests")]
public class AccessibilityTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private List<string> _pageUrls = new List<string>
    {
        //"school/105574/loreto-high-school-chorlton/secondary/about",
        //"school/137552/stewards-academy-science-specialist-harlow/secondary/about",
        //"school/100273/saint-paul-roman-catholic-infant-school/secondary/about",
        //"school/107564/todmorden-high-school/secondary/about",
        //"school/145744/abbey-park-school/secondary/about",
        //"school/105574/loreto-high-school-chorlton/secondary/academic-performance-attainment-and-progress",
        //"school/105574/loreto-high-school-chorlton/secondary/academic-performance-attainment-and-progress?SelectedAcademicYear=Current",
        //"school/105574/loreto-high-school-chorlton/secondary/academic-performance-english-and-maths-results",
        //"school/105574/loreto-high-school-chorlton/secondary/academic-performance-english-and-maths-results?SelectedGrade=Grade5AndAbove",
        //"school/100273/saint-paul-roman-catholic-infant-school/secondary/academic-performance-english-and-maths-results",
        //"school/105574/loreto-high-school-chorlton/secondary/academic-performance-subjects-entered",
        //"school/105574/loreto-high-school-chorlton/secondary/admissions",
        //"school/100273/saint-paul-roman-catholic-infant-school/secondary/admissions",
        //"school/107564/todmorden-high-school/secondary/admissions",
        //"school/105574/loreto-high-school-chorlton/secondary/attendance",
        //"school/105574/loreto-high-school-chorlton/secondary/curriculum-and-extra-curricular-activities",
        //"school/100273/saint-paul-roman-catholic-infant-school/secondary/curriculum-and-extra-curricular-activities",
        //"school/105574/loreto-high-school-chorlton/secondary/destinations",
        //"school/100273/saint-paul-roman-catholic-infant-school/secondary/destinations",
        //"search",
        //"search/results?NameSearchTerm=school&Distance=3&PageNumber=1",
        //"search/results?NameSearchTerm=xyz&Distance=3&PageNumber=1",
        //"",
        //"Cookies/Preferences",
        "my-schools/view"
    };

    [Fact]
    public async Task AboutSchoolPage_LoadsSuccessfully()
    {
        var cookieListOfUrns = new List<string> { "105574", "107564" };

        await Page.Context.ClearCookiesAsync();
        await Page.Context.AddCookiesAsync([new Cookie { Name = "MySchoolsList", Value = String.Join(",", cookieListOfUrns), Domain = "127.0.0.1", Path = "/", SameSite = SameSiteAttribute.Lax, Secure = true }]);

        var violationCount = 0;

        foreach (var pageUrl in _pageUrls)
        {
            await Page.GotoAsync(pageUrl);
            violationCount += await WriteAccessibilityReport(pageUrl);
        }

        // Future TODO if required. Uncomment this line if we want to fail the tests on Accessibility issues. For now, any violations are simply logged
        // Assert.Equal(0, violationCount);
    }

    private async Task<int> WriteAccessibilityReport(string pageName)
    {
        await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        var axeResult = await Page.RunAxe(new AxeRunOptions
        {
            RunOnly = new RunOnlyOptions { Type = "tag", Values = ["wcag2a", "wcag2aa"] }
        });

        AccessibilityReportHelper.AddViolations(pageName, Page.Url, axeResult.Violations);

        return axeResult.Violations.Length;
    }
}