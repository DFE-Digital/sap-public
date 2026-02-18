using Microsoft.Playwright;
using SAPPub.Core.Enums;
using SAPPub.Web.Helpers;
using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.SecondarySchool;

[Collection("Playwright Tests")]
public class AcademicPerformanceEnglishAndMathsResults(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private string _pageUrl = "school/105574/Loreto%20High%20School%20Chorlton/secondary/academic-performance-english-and-maths-results";

    [Fact]
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(_pageUrl);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_HasCorrectTitle()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var title = await Page.TitleAsync();

        // Assert
        Assert.Contains("Academic Performance", title);
    }

    [Fact]
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_DisplaysMainHeading()
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
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_Displays_SchoolName_Caption()
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
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_Displays_VerticalNavigation()
    {
        var performancePage = "school/105574/Loreto%20High%20School%20Chorlton/secondary/academic-performance-pupil-progress";
        // We want to display the performance root page even when in a performance sub-page, hence need to check the active href is the root performance page

        var nav = new VerticalNavigationHelper(Page);
        await Page.GotoAsync(_pageUrl);

        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveItemsCountAsync(6);
        await nav.ShouldHaveOneActiveItemAsync();
        await nav.ShouldHaveActiveHrefAsync(performancePage);
    }

    [Fact]
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_Displays_Sub_Navigation()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#sub-navigation-academic-performance").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_Displays_Gcse_Grades_Explained()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#details-gcse-grades-explained").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_DisplaysPagination()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#academic-performance-english-and-maths-results-pagination").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_DisplaysGradeSelectorForDataDisplayed()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#gradeSelector").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_ChangeGradeSelected()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Assert
        var chartHeading = Page.Locator("#chartHeading");
        var chartHeadingText = await chartHeading.TextContentAsync();
        Assert.Contains("Grade 5 and above", chartHeadingText);

        // Act
        var gradeSelector = Page.Locator("#gradeSelector");
        await gradeSelector.SelectOptionAsync([GcseGradeDataSelection.Grade4AndAbove.GetDisplayName()!]);
        var buttonSelector = Page.Locator("button:has-text(\"Show results\")");
        await buttonSelector.ClickAsync();

        // Assert
        chartHeading = Page.Locator("#chartHeading");
        chartHeadingText = await chartHeading.TextContentAsync();
        Assert.Contains("Grade 4 and above", chartHeadingText);
    }

    [Fact]
    public async Task EnglishAndMathsResultsPage_Displays_AllGcse_CurrentYear_Chart()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var chart = Page.Locator("#all-gcse-chart");
        var table = Page.Locator("#all-gcse-current-year-table");
        var showAsTableBtn = Page.Locator("#all-gcse-current-year-show-btn");
        var showDataOverTimeBtn = Page.Locator("#all-gcse-show-data-over-time-btn");

        var isChartVisible = await chart.IsVisibleAsync();
        var isTableVisible = await table.IsVisibleAsync();
        var isShowAsTableBtnVisible = await showAsTableBtn.IsVisibleAsync();
        var isShowDataOverTimeBtnVisible = await showDataOverTimeBtn.IsVisibleAsync();
        var showAsTableBtnText = await showAsTableBtn.TextContentAsync();
        var showDataOverTimeBtnText = await showDataOverTimeBtn.TextContentAsync();

        // Assert
        Assert.False(isTableVisible);
        Assert.True(isChartVisible);
        Assert.True(isShowAsTableBtnVisible);
        Assert.True(isShowDataOverTimeBtnVisible);

        Assert.Equal("Show as a table", showAsTableBtnText);
        Assert.Equal("Show data over time", showDataOverTimeBtnText);
    }

    [Fact]
    public async Task EnglishAndMathsResultsPage_Displays_AllGcse_CurrentYear_Table()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        // Click Show as a table button
        await Page.ClickAsync("#all-gcse-current-year-show-btn");

        var showAsTableBtn = Page.Locator("#all-gcse-current-year-show-btn");
        var showDataOverTimeBtn = Page.Locator("#all-gcse-show-data-over-time-btn");
        var chart = Page.Locator("#all-gcse-chart");
        var table = Page.Locator("#all-gcse-current-year-table");

        var isChartVisible = await chart.IsVisibleAsync();
        var isTableVisible = await table.IsVisibleAsync();
        var isShowDataOverTimeBtnVisible = await showDataOverTimeBtn.IsVisibleAsync();
        var buttonText = await showAsTableBtn.TextContentAsync();
        var showDataOverTimeBtnText = await showDataOverTimeBtn.TextContentAsync();

        // Assert
        Assert.False(isChartVisible);
        Assert.True(isTableVisible);
        Assert.True(isShowDataOverTimeBtnVisible);
        Assert.Equal("Show as a chart", buttonText);
        Assert.Equal("Show data over time", showDataOverTimeBtnText);
    }

    [Fact]
    public async Task EnglishAndMathsResultsPage_Displays_AllGcse_DataOverTime_Chart()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        // Click Show data over time button
        await Page.ClickAsync("#all-gcse-show-data-over-time-btn");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var chart = Page.Locator("#all-gcse-data-overtime-chart");
        var table = Page.Locator("#all-gcse-data-overtime-table");
        var showAsTableBtn = Page.Locator("#all-gcse-data-over-time-show-btn");
        var showCurrentDataBtn = Page.Locator("#all-gcse-show-current-data-btn");

        var isChartVisible = await chart.IsVisibleAsync();
        var isTableVisible = await table.IsVisibleAsync();
        var isShowAsTableBtnVisible = await showAsTableBtn.IsVisibleAsync();
        var isShowCurrentDataBtnVisible = await showCurrentDataBtn.IsVisibleAsync();
        var showAsTableBtnText = await showAsTableBtn.TextContentAsync();
        var showCurrentDataBtnText = await showCurrentDataBtn.TextContentAsync();

        // Assert
        Assert.False(isTableVisible);
        Assert.True(isChartVisible);
        Assert.True(isShowAsTableBtnVisible);
        Assert.True(isShowCurrentDataBtnVisible);

        Assert.Equal("Show as a table", showAsTableBtnText);
        Assert.Equal("Show current data", showCurrentDataBtnText);
    }

    [Fact]
    public async Task EnglishAndMathsResultsPage_Displays_AllGcse_DataOverTime_Table()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        // Click Show data over time button
        await Page.ClickAsync("#all-gcse-show-data-over-time-btn");

        // and click Show as a table button
        await Page.ClickAsync("#all-gcse-data-over-time-show-btn");

        var chart = Page.Locator("#all-gcse-data-overtime-chart");
        var table = Page.Locator("#all-gcse-data-overtime-table");
        var showAsTableBtn = Page.Locator("#all-gcse-data-over-time-show-btn");
        var showCurrentDataBtn = Page.Locator("#all-gcse-show-current-data-btn");

        var isChartVisible = await chart.IsVisibleAsync();
        var isTableVisible = await table.IsVisibleAsync();
        var isShowAsTableBtnVisible = await showAsTableBtn.IsVisibleAsync();
        var isShowCurrentDataBtnVisible = await showCurrentDataBtn.IsVisibleAsync();
        var showAsTableBtnText = await showAsTableBtn.TextContentAsync();
        var showCurrentDataBtnText = await showCurrentDataBtn.TextContentAsync();

        // Assert
        Assert.False(isChartVisible);
        Assert.True(isTableVisible);
        Assert.True(isShowAsTableBtnVisible);
        Assert.True(isShowCurrentDataBtnVisible);

        Assert.Equal("Show as a chart", showAsTableBtnText);
        Assert.Equal("Show current data", showCurrentDataBtnText);
    }

    [Fact]
    public async Task EnglishAndMathsResultsPage_Displays_AllGcse_DataOverTime_Table_Click_On_ShowCurrentData()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        // Click Show data over time button
        await Page.ClickAsync("#all-gcse-show-data-over-time-btn");

        // and click Show current data button
        await Page.ClickAsync("#all-gcse-show-current-data-btn");

        var chart = Page.Locator("#all-gcse-chart");
        var showAsTableBtn = Page.Locator("#all-gcse-current-year-show-btn");
        var showDataOverTimeBtn = Page.Locator("#all-gcse-show-data-over-time-btn");

        var isChartVisible = await chart.IsVisibleAsync();
        var isShowAsTableBtnVisible = await showAsTableBtn.IsVisibleAsync();
        var isShowDataOverTimeBtnVisible = await showDataOverTimeBtn.IsVisibleAsync();
        var showAsTableBtnText = await showAsTableBtn.TextContentAsync();
        var showDataOverTimeBtnText = await showDataOverTimeBtn.TextContentAsync();

        // Assert
        Assert.True(isChartVisible);
        Assert.True(isShowAsTableBtnVisible);
        Assert.True(isShowDataOverTimeBtnVisible);

        Assert.Equal("Show as a table", showAsTableBtnText);
        Assert.Equal("Show data over time", showDataOverTimeBtnText);
    }
}
