using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.SecondarySchool;

[Collection("Playwright Tests")]
public class AcademicPerformanceSubjectsEnteredTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private string _pageUrl = "school/105574/Loreto%20High%20School%20Chorlton/secondary/academic-performance-subjects-entered";

    [Fact]
    public async Task AcademicPerformanceSubjectsEnteredPage_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(_pageUrl);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task AcademicPerformanceSubjectsEnteredPage_HasCorrectTitle()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var title = await Page.TitleAsync();

        // Assert
        Assert.Contains("Academic Performance", title);
    }

    [Fact]
    public async Task AcademicPerformanceSubjectsEnteredPage_DisplaysMainHeading()
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
    public async Task AcademicPerformanceSubjectsEnteredPage_Displays_SchoolName_Caption()
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
    public async Task AcademicPerformanceSubjectsEnteredPage_Displays_VerticalNavigation()
    {
        var performancePage = "school/105574/Loreto%20High%20School%20Chorlton/secondary/academic-performance-attainment-and-progress";
        // We want to display the performance root page even when in a performance sub-page, hence need to check the active href is the root performance page

        var nav = new VerticalNavigationHelper(Page);
        await Page.GotoAsync(_pageUrl);

        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveItemsCountAsync(6);
        await nav.ShouldHaveOneActiveItemAsync();
        await nav.ShouldHaveActiveHrefAsync(performancePage);
    }

    [Fact]
    public async Task AcademicPerformanceSubjectsEnteredPage_Displays_Sub_Navigation()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#sub-navigation-academic-performance").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AcademicPerformancSubjectsEnteredPage_DisplaysWhatDoTheQualificationsMean()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#details-academic-performance").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AcademicPerformanceSubjectsEnteredPage_DisplaysPagination()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#academic-performance-subjects-entered-pagination").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AcademicPerformanceSubjectsEnteredPage_Displays_CoreSubjects()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#core-subjects-entered-table").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AcademicPerformanceSubjectsEnteredPage_Displays_AdditionalSubjects()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#additional-subjects-entered-table").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }
}
