using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.SecondarySchool;

[Collection("Playwright Tests")]
public class CurriculumAndExtraCurricularActivitiesTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private string _pageUrl = "school/105574/loreto-high-school-chorlton/secondary/curriculum-and-extra-curricular-activities";

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(_pageUrl);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_HasCorrectTitle()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var title = await Page.TitleAsync();

        // Assert
        Assert.Contains("Curriculum and extra-curricular activities", title);
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_DisplaysMainHeading()
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
    public async Task CurriculumAndExtraCurricularActivitiesPage_Displays_SchoolName_Caption()
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
    public async Task CurriculumAndExtraCurricularActivitiesPage_CurrentCurriculum_ContactSchoolText()
    {
        // Arrange
        _pageUrl = "school/100273/saint-paul-roman-catholic-infant-school/secondary/curriculum-and-extra-curricular-activities";
        await Page.GotoAsync(_pageUrl);

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
        await Page.GotoAsync(_pageUrl);

        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveItemsCountAsync(5);
        await nav.ShouldHaveOneActiveItemAsync();
        await nav.ShouldHaveActiveHrefAsync(_pageUrl);
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_Displays_Curriculum_Summary()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#current-curriculum-summary").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_Displays_Extra_Curriculum_Summary()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

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
        _pageUrl = "school/100273/Saint%20Paul%20Roman%20Catholic%20Infant%20School/secondary/curriculum-and-extra-curricular-activities";
        await Page.GotoAsync(_pageUrl);

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
        await Page.GotoAsync(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#current-extra-curricular-activities-pagination").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }
}
