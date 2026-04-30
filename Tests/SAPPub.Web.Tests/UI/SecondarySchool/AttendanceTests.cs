using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.SecondarySchool;

[Collection("Playwright Tests")]
public class AttendancePageTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private string _pageUrl = "school/105574/loreto-high-school-chorlton/secondary/attendance";

    private Dictionary<string, string> _schoolUrnToUrlMap = new Dictionary<string, string>
    {
        ["100273"] = "school/100273/saint-paul-roman-catholic-infant-school/secondary/attendance",
    };

    [Fact]
    public async Task AttendancePage_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(_pageUrl);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task AttendancePage_HasCorrectTitle()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var title = await Page.TitleAsync();

        // Assert
        Assert.Contains("Attendance", title);
    }

    [Fact]
    public async Task AttendancePage_DisplaysMainHeading()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var heading = await Page.Locator("h1").TextContentAsync();

        // Assert
        Assert.NotNull(heading);
        Assert.NotEmpty(heading!.Trim());
    }

    [Fact]
    public async Task AttendancePage_Displays_SchoolName_Caption()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var schoolNameCaptionLocator = Page.Locator("#school-name-caption");
        var isVisible = await schoolNameCaptionLocator.IsVisibleAsync();
        var schoolNameCaption = await schoolNameCaptionLocator.TextContentAsync();

        // Assert
        Assert.True(isVisible);
        Assert.NotNull(schoolNameCaption);
        Assert.Equal("Loreto High School Chorlton", schoolNameCaption);
    }

    [Fact]
    public async Task AttendancePage_Displays_VerticalNavigation()
    {
        var nav = new VerticalNavigationHelper(Page);
        await Page.GotoAsync(_pageUrl);
        
        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveItemsCountAsync(6);
        await nav.ShouldHaveOneActiveItemAsync();
        await nav.ShouldHaveActiveHrefAsync(_pageUrl);
    }

    [Fact]
    public async Task AttendancePage_Displays_AttendancePolicy_Summary()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var summaryCard = Page.Locator("#attendance-policy-summary");
        await summaryCard.WaitForAsync();

        var contactSchoolInfo = summaryCard.GetByTestId("contact-school-info");
        var schoolWebsiteLink = summaryCard.GetByTestId("school-website-link");
        var commissionerWebsiteLink = summaryCard.GetByTestId("school-website-link");
        var schoolWebsiteHref = await schoolWebsiteLink.GetAttributeAsync("href");
        var schoolWebsiteText = await schoolWebsiteLink.TextContentAsync();
        var commissionerWebsiteHref = await commissionerWebsiteLink.GetAttributeAsync("href");
        var commissionerWebsiteText = await commissionerWebsiteLink.TextContentAsync();


        // Assert
        Assert.True(await summaryCard.IsVisibleAsync());
        Assert.False(await contactSchoolInfo.IsVisibleAsync());
        Assert.True(await schoolWebsiteLink.IsVisibleAsync());
        Assert.True(await commissionerWebsiteLink.IsVisibleAsync());

        Assert.NotNull(schoolWebsiteHref);
        Assert.NotNull(schoolWebsiteText);
        Assert.NotNull(commissionerWebsiteHref);
        Assert.NotNull(commissionerWebsiteText);
    }

    [Fact]
    public async Task AttendancePage_DisplaysPagination()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#attendance-pagination").IsVisibleAsync();

        // Act
        var previousPaginationLink = Page.Locator("#attendance-pagination .govuk-pagination__prev a");
        var nextPaginationLink = Page.Locator("#attendance-pagination .govuk-pagination__next a");

        var previousPaginationText = await previousPaginationLink.TextContentAsync();
        var nextPaginationText = await nextPaginationLink.TextContentAsync();

        // Assert
        Assert.True(isVisible);
        Assert.Equal("Curriculum and extra-curricular activities", previousPaginationText?.Trim());
        Assert.Equal("Academic performance: Progress and attainment", nextPaginationText?.Trim());
    }

    [Fact]
    public async Task AttendancePage_Displays_ContactSchoolText()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["100273"]);

        // Act

        var contactSchoolInfo = Page.GetByTestId("contact-school-info");
        var isVisible = await contactSchoolInfo.IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

}
