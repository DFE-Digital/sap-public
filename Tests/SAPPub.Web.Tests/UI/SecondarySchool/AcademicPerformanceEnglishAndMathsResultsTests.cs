using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.SecondarySchool;

public class AcademicPerformanceEnglishAndMathsResults : BasePageTest
{
    private string _pageUrl = "school/105574/Loreto-High-School-Chorlton/secondary/academic-performance-english-and-maths-results";

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
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_Displays_VerticalNavigation()
    {
        var nav = new VerticalNavigationHelper(Page);
        await GoToPageAysnc(_pageUrl);

        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveItemsCountAsync(6);
        await nav.ShouldHaveOneActiveItemAsync();
        await nav.ShouldHaveActiveHrefAsync(_pageUrl);
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
}
