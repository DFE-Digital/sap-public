using Microsoft.Playwright;
using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;
using static SAPPub.Web.Constants.PageTitleConstants;

namespace SAPPub.Web.Tests.UI.KS4;

[Collection("Playwright Tests")]
public class SecondarySchoolNavigationTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private Dictionary<string, string> _schoolUrnToUrlMap = new Dictionary<string, string>
    {
        ["105574"] = "school/105574/loreto-high-school-chorlton/about",
        ["149328"] = "school/149328/king-edward-vi-high-school/about"
    };

    [Fact]
    public async Task NavigateThroughLeftNav_ShowsExpectedPages()
    {
        // Act
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        var nav = new VerticalNavigationHelper(Page);
        var navItem = nav.GetItem("Admissions");
        await navItem.ClickAsync();

        // Assert
        var title = await Page.TitleAsync();
        Assert.Contains(SecondarySchoolPageTitles.Admissions, title);

        // Act
        navItem = nav.GetItem("Curriculum and extra-curricular activities");
        await navItem.ClickAsync();

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains(SecondarySchoolPageTitles.Curriculum, title);

        // Act
        navItem = nav.GetItem("Attendance");
        await navItem.ClickAsync();

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains(SecondarySchoolPageTitles.Attendance, title);

        // Act
        navItem = nav.GetItem("Academic performance");
        await navItem.ClickAsync();

        // Assert
        title = await Page.TitleAsync();
        Assert.Contains(SecondarySchoolPageTitles.ProgressAndAttainment, title);

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
        // Act
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

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
        // Act
        var response = await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);
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
        Assert.Contains("Progress and attainment", title); // CML TODO this and others after - assert it's secondary

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

    [Fact(Skip = "Not implemented yet")]
    public async Task NavigateThroughPaginationNav_SchoolIsKS4AndKS5_ShowsExpectedPages()
    {
        // Act - navigate to last tab for Academic performance
        var response = await Page.GotoAsync(_schoolUrnToUrlMap["149328"]);
        var nav = new PaginationNavigationHelper(Page);
        var title = await Page.TitleAsync();
        await ClickAcademicPerformanceNavItemAsync(Page, "Additional measures");

        // Assert
        Assert.Contains("Additional measures", title);

        // Act
        await nav.ClickNextLinkAsync();

        // Assert
        Assert.Contains("16 to 19", title);
        Assert.Contains("performance", title);
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
