using Microsoft.Playwright;
using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;
using static SAPPub.Web.Constants.PageTitleConstants;

namespace SAPPub.Web.Tests.UI.Areas.KS2;

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

        // Assert
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
        Assert.Contains(PrimarySchoolPageTitles.Attendance, title);

        // Act
        navItem = nav.GetItem("Primary academic performance");
        await navItem.ClickAsync();

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains(PrimarySchoolPageTitles.ProgressAndAttainment, title);

        // Act
        navItem = nav.GetItem("Destinations");
        await navItem.ClickAsync();

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains(SecondarySchoolPageTitles.Destinations, title);
    }

    [Fact]
    public async Task AcademicPerformanceAttainmentAndProgressPage_Displays_Sub_Navigation()
    {
        throw new NotImplementedException("This test is not implemented yet.");
        // Act
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        var nav = new VerticalNavigationHelper(Page);
        var navItem = nav.GetItem("Academic performance");
        await navItem.ClickAsync();

        // Act
        await ClickAcademicPerformanceNavItemAsync(Page, "Progress and attainment");

        // Assert
        var title = await Page.TitleAsync();
        Assert.Contains("Progress and attainment", title);

        // Act
        await ClickAcademicPerformanceNavItemAsync(Page, "English and maths results");

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains("English and maths", title);

        // Act
        await ClickAcademicPerformanceNavItemAsync(Page, "Subjects entered");

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains("Subjects entered", title);

        // Act
        await ClickAcademicPerformanceNavItemAsync(Page, "Additional measures");

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains("Additional measures", title);
    }

    [Fact]
    public async Task NavigateThroughPaginationNav_ShowsExpectedPages()
    {
        throw new NotImplementedException("This test is not implemented yet.");
        // Act
        var response = await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);
        var nav = new PaginationNavigationHelper(Page);

        // Assert
        var title = await Page.TitleAsync();
        Assert.Contains("About", title);

        // Act
        await nav.ClickNextLinkAsync();

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains("Admissions", title);

        // Act
        await nav.ClickNextLinkAsync();

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains("Curriculum", title);

        // Act
        await nav.ClickNextLinkAsync();

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains("Attendance", title);

        // Act
        await nav.ClickNextLinkAsync();

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains("Secondary", title);
        Assert.Contains("Progress and attainment", title);

        // Act
        await nav.ClickNextLinkAsync();

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains("English and maths", title);

        // Act
        await nav.ClickNextLinkAsync();

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains("Subjects entered", title);

        // Act
        await nav.ClickNextLinkAsync();

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains("Additional measures", title);

        // Act
        await nav.ClickNextLinkAsync();

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains("Destinations", title);
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
