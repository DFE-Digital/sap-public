using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.Areas.KS2;

[Collection("Playwright Tests")]
public class CurriculumAndExtraCurricularActivitiesTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private string _pageUrl = "school/143034/st-pauls-church-of-england-academy/curriculum/primary";

    private Dictionary<string, string> _schoolUrnToUrlMap = new Dictionary<string, string>
    {
        ["143034"] = "school/143034/st-pauls-church-of-england-academy/curriculum/primary",
        ["100273"] = "school/100273/saint-paul-roman-catholic-infant-school/curriculum/primary",
        ["150009"] = "school/150009/abraham-moss-community-school/curriculum/primary" // KS2 + KS4 school
    };

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_HasCorrectTitle()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        var title = await Page.TitleAsync();

        // Assert
        Assert.Contains("Primary Curriculum", title);
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_DisplaysMainHeading()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        var heading = await Page.Locator("h1").TextContentAsync();

        // Assert
        Assert.NotNull(heading);
        Assert.NotEmpty(heading!.Trim());
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_Displays_SchoolName_Caption()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        var schoolNameCaptionLocator = Page.Locator("#school-name-caption");
        var isVisible = await schoolNameCaptionLocator.IsVisibleAsync();
        var schoolNameCaption = await schoolNameCaptionLocator.TextContentAsync();

        // Assert
        Assert.True(isVisible);
        Assert.NotNull(schoolNameCaption);
        Assert.Equal("St Paul's Church of England Academy", schoolNameCaption);
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_Displays_VerticalNavigation()
    {
        var nav = new VerticalNavigationHelper(Page);
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveOneActiveItemAsync();
        await nav.ShouldHaveActiveHrefAsync(_pageUrl);
    }

    [Fact]
    public async Task CurriculumPage_DoesNotDisplay_SubNavigation_WhenOnlyKS2()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        var subNav = Page.Locator("#sub-navigation-academic-performance");
        var isVisible = await subNav.IsVisibleAsync();

        // Assert
        Assert.False(isVisible);
    }

    [Fact]
    public async Task CurriculumPage_Displays_SubNavigation_WhenMultiplePhases()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["150009"]);

        // Act
        var subNav = Page.Locator("#sub-navigation-academic-performance");
        var isVisible = await subNav.IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task CurriculumPage_SubNavigation_HasCorrectLinks_WhenMultiplePhases()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["150009"]);

        // Act
        var primaryLink = Page.Locator("#sub-navigation-academic-performance a[aria-current='page']");
        var secondaryLink = Page.Locator("#sub-navigation-academic-performance a:not([aria-current='page'])");

        var primaryLinkText = await primaryLink.TextContentAsync();
        var secondaryLinkText = await secondaryLink.TextContentAsync();

        // Assert
        Assert.Equal("Primary Curriculum", primaryLinkText?.Trim());
        Assert.Equal("Secondary Curriculum", secondaryLinkText?.Trim());
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_CurrentCurriculum_ContactSchoolText()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["100273"]);

        // Act
        var summaryCard = Page.GetByTestId("current-curriculum-summary");
        await summaryCard.WaitForAsync();

        var contactSchoolInfoKs2 = summaryCard.GetByTestId("contact-school-info-ks2");

        // Assert
        Assert.True(await contactSchoolInfoKs2.IsVisibleAsync());
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_Displays_Curriculum_Summary()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        var summaryCard = Page.Locator("#current-curriculum-summary");

        // Assert
        Assert.True(await summaryCard.IsVisibleAsync());

        var contactSchoolInfo = summaryCard.GetByTestId("contact-school-info-ks2");
        Assert.False(await contactSchoolInfo.IsVisibleAsync());
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_Displays_Extra_Curriculum_Summary()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        var summaryCard = Page.Locator("#current-extra-curricular-activities-offered-summary");

        // Assert
        Assert.True(await summaryCard.IsVisibleAsync());

        var contactSchoolInfo = summaryCard.GetByTestId("contact-school-info-extra");
        Assert.False(await contactSchoolInfo.IsVisibleAsync());
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_Displays_Extra_Curriculum_Summary_ContactSchoolText()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["100273"]);

        // Act
        var summaryCard = Page.Locator("#current-extra-curricular-activities-offered-summary");
        await summaryCard.WaitForAsync();

        var contactSchoolInfo = summaryCard.GetByTestId("contact-school-info-extra");

        // Assert
        Assert.True(await contactSchoolInfo.IsVisibleAsync());

    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_DisplaysPagination()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        var isVisible = await Page.Locator("#current-extra-curricular-activities-pagination").IsVisibleAsync();
        var previousPaginationLink = Page.Locator("#current-extra-curricular-activities-pagination .govuk-pagination__prev a");
        var nextPaginationLink = Page.Locator("#current-extra-curricular-activities-pagination .govuk-pagination__next a");

        var previousPaginationText = await previousPaginationLink.TextContentAsync();
        var nextPaginationText = await nextPaginationLink.TextContentAsync();

        // Assert
        Assert.True(isVisible);
        Assert.Equal("Admissions", previousPaginationText?.Trim());
        Assert.Equal("Attendance", nextPaginationText?.Trim());
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_DisplaysPagination_WhenMultiplePhases()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["150009"]);

        // Act
        var isVisible = await Page.Locator("#current-extra-curricular-activities-pagination").IsVisibleAsync();
        var previousPaginationLink = Page.Locator("#current-extra-curricular-activities-pagination .govuk-pagination__prev a");
        var nextPaginationLink = Page.Locator("#current-extra-curricular-activities-pagination .govuk-pagination__next a");

        var previousPaginationText = await previousPaginationLink.TextContentAsync();
        var nextPaginationText = await nextPaginationLink.TextContentAsync();

        // Assert
        Assert.True(isVisible);
        Assert.Equal("Secondary admissions", previousPaginationText?.Trim());
        Assert.Equal("Secondary curriculum", nextPaginationText?.Trim());
    }
}
