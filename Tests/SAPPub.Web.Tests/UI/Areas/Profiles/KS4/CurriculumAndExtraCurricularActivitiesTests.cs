using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.KS4;

[Collection("Playwright Tests")]
public class CurriculumAndExtraCurricularActivitiesTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{

    private Dictionary<string, string> _schoolUrnToUrlMap = new Dictionary<string, string>
    {
        ["105574"] = "school/105574/loreto-high-school-chorlton/curriculum/secondary",
        ["100273"] = "school/100273/saint-paul-roman-catholic-infant-school/curriculum/secondary",
        ["150009"] = "school/150009/abraham-moss-community-school/curriculum/secondary" // KS2 + KS4 school
    };

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_HasCorrectTitle()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var title = await Page.TitleAsync();

        // Assert
        Assert.Contains("Secondary Curriculum", title);
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_DisplaysMainHeading()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

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
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

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
    public async Task CurriculumAndExtraCurricularActivitiesPage_CurrentCurriculum_ContactSchoolText()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["100273"]);

        // Act
        var summaryCard = Page.GetByTestId("current-curriculum-summary");
        await summaryCard.WaitForAsync();

        var contactSchoolInfoKs3 = summaryCard.GetByTestId("contact-school-info-ks3");
        var contactSchoolInfoKs4 = summaryCard.GetByTestId("contact-school-info-ks4");

        // Assert
        Assert.True(await contactSchoolInfoKs3.IsVisibleAsync());
        Assert.True(await contactSchoolInfoKs4.IsVisibleAsync());
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_Displays_VerticalNavigation()
    {
        var nav = new VerticalNavigationHelper(Page);
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveOneActiveItemAsync();
        await nav.ShouldHaveActiveHrefAsync(_schoolUrnToUrlMap["105574"].Replace("/secondary", ""));
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_Displays_Curriculum_Summary()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var isVisible = await Page.Locator("#current-curriculum-summary").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_Displays_Extra_Curriculum_Summary()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

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
    public async Task CurriculumPage_DoesNotDisplay_SubNavigation_WhenOnlyKS4()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

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
        var primaryLink = Page.Locator("#sub-navigation-academic-performance a:not([aria-current='page'])");
        var secondaryLink = Page.Locator("#sub-navigation-academic-performance a[aria-current='page']");

        var primaryLinkText = await primaryLink.TextContentAsync();
        var secondaryLinkText = await secondaryLink.TextContentAsync();

        // Assert
        Assert.Equal("Primary Curriculum", primaryLinkText?.Trim());
        Assert.Equal("Secondary Curriculum", secondaryLinkText?.Trim());
    }
}
