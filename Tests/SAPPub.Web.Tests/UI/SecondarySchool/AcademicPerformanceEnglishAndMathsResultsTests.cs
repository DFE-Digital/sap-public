using SAPPub.Tests.UI.Infrastructure;
using SAPPub.Web.Helpers;
using SAPPub.Web.Tests.UI.Helpers;
using static SAPPub.Web.Models.SecondarySchool.AcademicPerformanceEnglishAndMathsResultsViewModel;

namespace SAPPub.Web.Tests.UI.SecondarySchool;

public class AcademicPerformanceEnglishAndMathsResults : BasePageTest
{
    private string _pageUrl = "school/105574/Loreto%20High%20School%20Chorlton/secondary/academic-performance-english-and-maths-results";

    [Fact]
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await GoToPageAysnc(_pageUrl);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_HasCorrectTitle()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var title = await Page.TitleAsync();

        // Assert
        Assert.Contains("Academic Performance", title);
    }

    [Fact]
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_DisplaysMainHeading()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var heading = await Page.Locator("h1").TextContentAsync();

        // Assert
        Assert.NotNull(heading.Replace(" ", ""));
    }

    [Fact]
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_Displays_SchoolName_Caption()
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
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_Displays_VerticalNavigation()
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
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_Displays_Sub_Navigation()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#sub-navigation-academic-performance").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_Displays_Gcse_Grades_Explained()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#details-gcse-grades-explained").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_DisplaysPagination()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#academic-performance-english-and-maths-results-pagination").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_DisplaysGradeSelectorForDataDisplayed()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#gradeSelector").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_ChangeGradeSelected()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Assert
        var chartHeading = Page.Locator("#chartHeading");
        var chartHeadingText = await chartHeading.TextContentAsync();
        Assert.Contains("Grade 5 and above", chartHeadingText);

        // Act
        var gradeSelector = Page.Locator("#gradeSelector");
        await gradeSelector.SelectOptionAsync(new[] { GcseGradeDataSelection.Grade4AndAbove.GetDisplayName()! });
        var buttonSelector = Page.Locator("button:has-text(\"Show results\")");
        await buttonSelector.ClickAsync();

        // Assert
        chartHeading = Page.Locator("#chartHeading");
        chartHeadingText = await chartHeading.TextContentAsync();
        Assert.Contains("Grade 4 and above", chartHeadingText);
    }
}
