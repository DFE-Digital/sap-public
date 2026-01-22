using SAPPub.Tests.UI.Infrastructure;
using SAPPub.Web.Tests.UI.Helpers;

namespace SAPPub.Web.Tests.UI.SecondarySchool;

public class AcademicPerformanceSubjectsEnteredTests : BasePageTest
{
    private string _pageUrl = "school/105574/Loreto%20High%20School%20Chorlton/secondary/academic-performance-subjects-entered";

    [Fact]
    public async Task AcademicPerformanceSubjectsEnteredPage_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await GoToPageAysnc(_pageUrl);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task AcademicPerformanceSubjectsEnteredPage_HasCorrectTitle()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var title = await Page.TitleAsync();

        // Assert
        Assert.Contains("Academic Performance", title);
    }

    [Fact]
    public async Task AcademicPerformanceSubjectsEnteredPage_DisplaysMainHeading()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var heading = await Page.Locator("h1").TextContentAsync();

        // Assert
        Assert.NotNull(heading.Replace(" ", ""));
    }

    [Fact]
    public async Task AcademicPerformanceSubjectsEnteredPage_Displays_SchoolName_Caption()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

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
        var performancePage = "school/105574/Loreto%20High%20School%20Chorlton/secondary/academic-performance-pupil-progress";
        // We want to display the performance root page even when in a performance sub-page, hence need to check the active href is the root performance page

        var nav = new VerticalNavigationHelper(Page);
        await GoToPageAysnc(_pageUrl);

        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveItemsCountAsync(6);
        await nav.ShouldHaveOneActiveItemAsync();
        await nav.ShouldHaveActiveHrefAsync(performancePage);
    }

    [Fact]
    public async Task AcademicPerformanceSubjectsEnteredPage_Displays_Sub_Navigation()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#sub-navigation-academic-performance").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AcademicPerformancSubjectsEnteredPage_DisplaysWhatDoTheQualificationsMean()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#details-academic-performance").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AcademicPerformanceSubjectsEnteredPage_DisplaysPagination()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#academic-performance-subjects-entered-pagination").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AcademicPerformanceSubjectsEnteredPage_Displays_CoreSubjects()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#core-subjects-entered-table").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AcademicPerformanceSubjectsEnteredPage_Displays_AdditionalSubjects()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#additional-subjects-entered-table").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }
}
