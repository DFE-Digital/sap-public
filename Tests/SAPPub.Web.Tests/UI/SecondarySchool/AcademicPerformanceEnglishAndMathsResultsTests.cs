using Microsoft.Playwright;
using SAPPub.Core.Enums;
using SAPPub.Web.Helpers;
using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.SecondarySchool;

[Collection("Playwright Tests")]
public class AcademicPerformanceEnglishAndMathsResults(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private Dictionary<string, string> _schoolUrnToUrlMap = new Dictionary<string, string>
    {
        ["105574"] = "school/105574/loreto-high-school-chorlton/secondary-performance/english-and-maths",
        ["100273"] = "school/100273/saint-paul-roman-catholic-infant-school/secondary-performance/english-and-maths",
    };

    [Fact]
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_HasCorrectTitle()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var title = await Page.TitleAsync();

        // Assert
        Assert.Contains("Loreto High School Chorlton - Secondary English and maths results - School Profiles - GOV.UK", title);
    }

    [Fact]
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_DisplaysMainHeading()
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
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_Displays_SchoolName_Caption()
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
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_Displays_VerticalNavigation()
    {
        var performancePage = "school/105574/loreto-high-school-chorlton/secondary-performance/progress-attainment";
        // We want to display the performance root page even when in a performance sub-page, hence need to check the active href is the root performance page

        var nav = new VerticalNavigationHelper(Page);
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveOneActiveItemAsync();
        await nav.ShouldHaveActiveHrefAsync(performancePage);
    }

    [Fact]
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_Displays_Sub_Navigation()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var isVisible = await Page.Locator("#sub-navigation-academic-performance").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_Displays_Gcse_Grades_Explained()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var isVisible = await Page.Locator("#details-gcse-grades-explained").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_DisplaysPagination()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var isVisible = await Page.Locator("#academic-performance-english-and-maths-results-pagination").IsVisibleAsync();

        var previousPaginationLink = Page.Locator("#academic-performance-english-and-maths-results-pagination .govuk-pagination__prev a");
        var nextPaginationLink = Page.Locator("#academic-performance-english-and-maths-results-pagination .govuk-pagination__next a");

        var previousPaginationText = await previousPaginationLink.TextContentAsync();
        var nextPaginationText = await nextPaginationLink.TextContentAsync();

        // Assert
        Assert.True(isVisible);
        Assert.Contains("performance: Progress and attainment", previousPaginationText?.Trim());
        Assert.Contains("performance: Subjects entered", nextPaginationText?.Trim());
    }

    [Fact]
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_DisplaysGradeSelectorForDataDisplayed()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var isVisible = await Page.Locator("#gradeSelector").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_ChangeGradeSelected()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

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
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

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
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

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
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        // Click Show data over time button
        await Page.ClickAsync("#all-gcse-show-data-over-time-btn");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var chart = Page.Locator("#all-gcse-data-overtime-chart");
        var table = Page.Locator("#all-gcse-data-overtime-table");
        var chartLegend = Page.Locator("#all-gcse-data-overtime-chart-legend");
        var showAsTableBtn = Page.Locator("#all-gcse-data-over-time-show-btn");
        var showCurrentDataBtn = Page.Locator("#all-gcse-show-current-data-btn");

        var isChartVisible = await chart.IsVisibleAsync();
        var isTableVisible = await table.IsVisibleAsync();
        var isChartLegendVisible = await chartLegend.IsVisibleAsync();
        var isShowAsTableBtnVisible = await showAsTableBtn.IsVisibleAsync();
        var isShowCurrentDataBtnVisible = await showCurrentDataBtn.IsVisibleAsync();
        var showAsTableBtnText = await showAsTableBtn.TextContentAsync();
        var showCurrentDataBtnText = await showCurrentDataBtn.TextContentAsync();

        // Assert
        Assert.False(isTableVisible);
        Assert.True(isChartVisible);
        Assert.True(isChartLegendVisible);
        Assert.True(isShowAsTableBtnVisible);
        Assert.True(isShowCurrentDataBtnVisible);

        Assert.Equal("Show as a table", showAsTableBtnText);
        Assert.Equal("Show current data", showCurrentDataBtnText);
    }

    [Fact]
    public async Task EnglishAndMathsResultsPage_Displays_AllGcse_DataOverTime_Table()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        // Click Show data over time button
        await Page.ClickAsync("#all-gcse-show-data-over-time-btn");

        // and click Show as a table button
        await Page.ClickAsync("#all-gcse-data-over-time-show-btn");

        var chart = Page.Locator("#all-gcse-data-overtime-chart");
        var table = Page.Locator("#all-gcse-data-overtime-table");
        var chartLegend = Page.Locator("#all-gcse-data-overtime-chart-legend");
        var showAsTableBtn = Page.Locator("#all-gcse-data-over-time-show-btn");
        var showCurrentDataBtn = Page.Locator("#all-gcse-show-current-data-btn");

        var isChartVisible = await chart.IsVisibleAsync();
        var isTableVisible = await table.IsVisibleAsync();
        var isChartLegendVisible = await chartLegend.IsVisibleAsync();
        var isShowAsTableBtnVisible = await showAsTableBtn.IsVisibleAsync();
        var isShowCurrentDataBtnVisible = await showCurrentDataBtn.IsVisibleAsync();
        var showAsTableBtnText = await showAsTableBtn.TextContentAsync();
        var showCurrentDataBtnText = await showCurrentDataBtn.TextContentAsync();

        // Assert
        Assert.False(isChartVisible);
        Assert.False(isChartLegendVisible);
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
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

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

    [Fact]
    public async Task EnglishAndMathsResultsPage_Displays_AllGcse_DataOverTime_No_Chart_Only_Render_Table()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["100273"]);

        // Act       
        var gcseChart = Page.Locator("#all-gcse-chart");
        var gcseCurrentYearTable = Page.Locator("#all-gcse-current-year-table");
        var gcseCurrentYearShowBtn = Page.Locator("#all-gcse-current-year-show-btn");
        var gcseShowDataOverTimeBtn = Page.Locator("#all-gcse-show-data-over-time-btn");
        var gcseDataOverTimeChart = Page.Locator("#all-gcse-data-overtime-chart");
        var gcseDataOverTimeTable = Page.Locator("#all-gcse-data-overtime-table");
        var gcseDataOverTimeShowBtn = Page.Locator("#all-gcse-data-over-time-show-btn");
        var gcseShowCurrentDataBtn = Page.Locator("#all-gcse-show-current-data-btn");

        // Assert
        Assert.False(await gcseChart.CountAsync() > 0);
        Assert.False(await gcseCurrentYearTable.CountAsync() > 0);
        Assert.False(await gcseCurrentYearShowBtn.CountAsync() > 0);
        Assert.False(await gcseShowDataOverTimeBtn.CountAsync() > 0);
        Assert.False(await gcseDataOverTimeChart.CountAsync() > 0);
        Assert.False(await gcseDataOverTimeShowBtn.CountAsync() > 0);
        Assert.False(await gcseShowCurrentDataBtn.CountAsync() > 0);
        Assert.True(await gcseDataOverTimeTable.CountAsync() > 0);
    }

    [Fact]
    public async Task EnglishAndMathsResultsPage_Displays_BreakdownGcse_CurrentYear_Chart()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var chart = Page.Locator("#breakdown-gcse-chart");
        var table = Page.Locator("#breakdown-gcse-current-year-table");
        var chartLegend = Page.Locator("#breakdown-gcse-chart-legend");
        var showAsTableBtn = Page.Locator("#breakdown-gcse-current-year-show-btn");

        var isChartVisible = await chart.IsVisibleAsync();
        var isTableVisible = await table.IsVisibleAsync();
        var isChartLegendVisible = await chartLegend.IsVisibleAsync();
        var isShowAsTableBtnVisible = await showAsTableBtn.IsVisibleAsync();
        var showAsTableBtnText = await showAsTableBtn.TextContentAsync();

        // Assert
        Assert.False(isTableVisible);
        Assert.True(isChartVisible);
        Assert.True(isChartLegendVisible);
        Assert.True(isShowAsTableBtnVisible);

        Assert.Equal("Show as a table", showAsTableBtnText);
    }

    [Fact]
    public async Task EnglishAndMathsResultsPage_Displays_BreakdownGcse_CurrentYear_Table()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        // Click Show as a table button
        await Page.ClickAsync("#breakdown-gcse-current-year-show-btn");

        var showAsTableBtn = Page.Locator("#breakdown-gcse-current-year-show-btn");
        var chart = Page.Locator("#breakdown-gcse-chart");
        var table = Page.Locator("#breakdown-gcse-current-year-table");
        var chartLegend = Page.Locator("#breakdown-gcse-chart-legend");

        var isChartVisible = await chart.IsVisibleAsync();
        var isTableVisible = await table.IsVisibleAsync();
        var isChartLegendVisible = await chartLegend.IsVisibleAsync();
        var buttonText = await showAsTableBtn.TextContentAsync();

        // Assert
        Assert.False(isChartVisible);
        Assert.False(isChartLegendVisible);
        Assert.True(isTableVisible);
        Assert.Equal("Show as a chart", buttonText);
    }

    [Fact]
    public async Task EnglishAndMathsResultsPage_Displays_BreakdownGcse_No_Chart_Only_Render_Table()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["100273"]);

        // Act       
        var breakdownGcseChart = Page.Locator("#breakdown-gcse-chart");
        var breakdownGcseCurrentYearTable = Page.Locator("#breakdown-gcse-current-year-table");

        var breakdownGcseCurrentYearShowBtn = Page.Locator("#breakdown-gcse-current-year-show-btn");

        // Assert
        Assert.False(await breakdownGcseChart.CountAsync() > 0);
        Assert.False(await breakdownGcseCurrentYearShowBtn.CountAsync() > 0);
        Assert.True(await breakdownGcseCurrentYearTable.CountAsync() > 0);
    }

    [Fact]
    public async Task EnglishAndMathsResultsPage_KeyboardNavigation_CanReachAndFocus_ToggleButtons()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var reachedShowAsTableButton = await FocusElementByTabAsync("all-gcse-current-year-show-btn");
        Assert.True(reachedShowAsTableButton);

        var hasVisibleFocusOnShowAsTable = await HasVisibleFocusAsync("#all-gcse-current-year-show-btn");

        await Page.Keyboard.PressAsync("Tab");
        var focusedElementId = await Page.EvaluateAsync<string>("() => document.activeElement?.id ?? ''");
        var hasVisibleFocusOnShowDataOverTime = await HasVisibleFocusAsync("#all-gcse-show-data-over-time-btn");

        // Assert
        Assert.True(hasVisibleFocusOnShowAsTable);
        Assert.Equal("all-gcse-show-data-over-time-btn", focusedElementId);
        Assert.True(hasVisibleFocusOnShowDataOverTime);

        // Ensure reverse tab order is not trapped or skipped
        await Page.Keyboard.PressAsync("Shift+Tab");
        focusedElementId = await Page.EvaluateAsync<string>("() => document.activeElement?.id ?? ''");
        Assert.Equal("all-gcse-current-year-show-btn", focusedElementId);
    }

    [Fact]
    public async Task EnglishAndMathsResultsPage_KeyboardActivation_ShowAsTableButton_SupportsEnterAndSpace()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act - Enter switches to table view
        var reachedShowAsTableButton = await FocusElementByTabAsync("all-gcse-current-year-show-btn");
        Assert.True(reachedShowAsTableButton);

        await Page.Keyboard.PressAsync("Enter");

        var chart = Page.Locator("#all-gcse-chart");
        var table = Page.Locator("#all-gcse-current-year-table");
        Assert.False(await chart.IsVisibleAsync());
        Assert.True(await table.IsVisibleAsync());

        // Act - Space switches back to chart view
        await Page.Keyboard.PressAsync("Space");

        // Assert
        Assert.True(await chart.IsVisibleAsync());
        Assert.False(await table.IsVisibleAsync());
    }

    [Fact]
    public async Task EnglishAndMathsResultsPage_KeyboardActivation_ShowDataOverTimeAndShowCurrentDataButtons_SupportEnterAndSpace()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act - Enter on show data over time
        await Page.Locator("#all-gcse-show-data-over-time-btn").FocusAsync();
        await Page.Keyboard.PressAsync("Enter");

        // Assert
        Assert.True(await IsElementCheckedAsync("data-overtime-view"));
        Assert.False(await IsElementCheckedAsync("current-view"));

        // Act - Space on show current data
        await Page.Locator("#all-gcse-show-current-data-btn").FocusAsync();
        await Page.Keyboard.PressAsync("Space");

        // Assert
        Assert.True(await IsElementCheckedAsync("current-view"));
        Assert.False(await IsElementCheckedAsync("data-overtime-view"));

        // Act - Space on show data over time
        await Page.Locator("#all-gcse-show-data-over-time-btn").FocusAsync();
        await Page.Keyboard.PressAsync("Space");

        // Assert
        Assert.True(await IsElementCheckedAsync("data-overtime-view"));
        Assert.False(await IsElementCheckedAsync("current-view"));

        // Act - Enter on show current data
        await Page.Locator("#all-gcse-show-current-data-btn").FocusAsync();
        await Page.Keyboard.PressAsync("Enter");

        // Assert
        Assert.True(await IsElementCheckedAsync("current-view"));
        Assert.False(await IsElementCheckedAsync("data-overtime-view"));
    }

    [Fact]
    public async Task EnglishAndMathsResultsPage_KeyboardActivation_ShowDataOverTime_Enter_MovesFocusToShowCurrentData()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        var reachedShowAsTableButton = await FocusElementByTabAsync("all-gcse-current-year-show-btn");
        Assert.True(reachedShowAsTableButton);

        await Page.Keyboard.PressAsync("Tab");

        // Act
        await Page.Keyboard.PressAsync("Enter");

        // Assert
        var focusedElementMoved = await WaitForFocusedElementAsync("all-gcse-show-current-data-btn");
        Assert.True(focusedElementMoved);
    }

    [Fact]
    public async Task EnglishAndMathsResultsPage_KeyboardActivation_ShowCurrentData_Enter_TabSequenceCanReachShowDataOverTime()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        var reachedShowAsTableButton = await FocusElementByTabAsync("all-gcse-current-year-show-btn");
        Assert.True(reachedShowAsTableButton);

        await Page.Keyboard.PressAsync("Tab");
        await Page.Keyboard.PressAsync("Enter");

        var focusedOnShowCurrentData = await WaitForFocusedElementAsync("all-gcse-show-current-data-btn");
        Assert.True(focusedOnShowCurrentData);

        // Act
        await Page.Keyboard.PressAsync("Enter");

        // Assert
        var reachedShowDataOverTimeButton = await FocusElementByTabAsync("all-gcse-show-data-over-time-btn", 120);
        Assert.True(reachedShowDataOverTimeButton);
    }

    private async Task<bool> IsElementCheckedAsync(string elementId)
    {
        return await Page.EvaluateAsync<bool>("id => !!document.getElementById(id)?.checked", elementId);
    }

    private async Task<bool> WaitForFocusedElementAsync(string expectedElementId, int timeoutMs = 1000)
    {
        const int intervalMs = 50;
        var attempts = timeoutMs / intervalMs;

        for (var index = 0; index < attempts; index++)
        {
            var focusedElementId = await Page.EvaluateAsync<string>("() => document.activeElement?.id ?? ''");
            if (focusedElementId == expectedElementId)
            {
                return true;
            }

            await Page.WaitForTimeoutAsync(intervalMs);
        }

        return false;
    }

    private async Task<bool> FocusElementByTabAsync(string expectedElementId, int maxTabs = 60)
    {
        for (var index = 0; index < maxTabs; index++)
        {
            await Page.Keyboard.PressAsync("Tab");
            var focusedElementId = await Page.EvaluateAsync<string>("() => document.activeElement?.id ?? ''");
            if (focusedElementId == expectedElementId)
            {
                return true;
            }
        }

        return false;
    }

    private async Task<bool> HasVisibleFocusAsync(string selector)
    {
        return await Page.Locator(selector).EvaluateAsync<bool>("""
            element => {
                const styles = window.getComputedStyle(element);
                return styles.boxShadow !== 'none' || (styles.outlineStyle !== 'none' && styles.outlineWidth !== '0px');
            }
            """);
    }
}
