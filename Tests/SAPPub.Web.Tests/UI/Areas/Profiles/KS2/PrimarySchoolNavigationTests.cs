using Microsoft.Playwright;
using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;
using static SAPPub.Web.Constants.PageTitleConstants;

namespace SAPPub.Web.Tests.UI.Areas.Profiles.KS2;

[Collection("Playwright Tests")]
public class PrimarySchoolNavigationTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private Dictionary<string, string> _schoolUrnToUrlMap = new Dictionary<string, string>
    {
        ["143034"] = "school/143034/st-pauls-church-of-england-academy/about",
        ["100273"] = "school/100273/saint-paul-roman-catholic-infant-school/about"
    };

    [Fact]
    public async Task NavigateThroughLeftNav_ShowsExpectedPages()
    {
        // Act
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        var nav = new VerticalNavigationHelper(Page);
        var navItem = nav.GetItem("Admissions");
        await navItem.ClickAsync();

        //// Assert
        var title = await Page.TitleAsync();
        Assert.Contains(PrimarySchoolPageTitles.Admissions, title);

        // Act
        navItem = nav.GetItem("Curriculum and extra-curricular activities");
        await navItem.ClickAsync();

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains(PrimarySchoolPageTitles.Curriculum, title);

        // Act
        navItem = nav.GetItem("Attendance");
        await navItem.ClickAsync();

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains(PageTitles.Attendance, title);

        // Act
        navItem = nav.GetItem("Primary academic performance");
        await navItem.ClickAsync();

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains(PrimarySchoolPageTitles.PupilProgress, title);
    }

    [Fact]
    public async Task AcademicPerformanceAttainmentAndProgressPage_Displays_Sub_Navigation()
    {
        // Act
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        var nav = new VerticalNavigationHelper(Page);
        var navItem = nav.GetItem("Primary academic performance");
        await navItem.ClickAsync();

        // Act
        await ClickAcademicPerformanceNavItemAsync(Page, "Pupil progress");

        // Assert
        var title = await Page.TitleAsync();
        Assert.Contains(PrimarySchoolPageTitles.PupilProgress, title);

        // Act
        await ClickAcademicPerformanceNavItemAsync(Page, "Meeting or exceeding standards");

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains(PrimarySchoolPageTitles.MeetingOrExceedingStandards, title);

        // Act
        await ClickAcademicPerformanceNavItemAsync(Page, "Subject scaled scores");

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains(PrimarySchoolPageTitles.SubjectScaledScores, title);

        // Act
        await ClickAcademicPerformanceNavItemAsync(Page, "Additional measures");

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains(PrimarySchoolPageTitles.AdditionalMeasures, title);
    }
  
    private static Task ClickAcademicPerformanceNavItemAsync(
        IPage page,
        string itemName)
    {
        return page
            .Locator("#sub-navigation-academic-performance")
            .GetByRole(AriaRole.Link, new() { Name = itemName, Exact = true })
            .ClickAsync();
    }
}
