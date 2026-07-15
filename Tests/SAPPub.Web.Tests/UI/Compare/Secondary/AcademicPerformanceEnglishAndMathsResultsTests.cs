using Microsoft.Playwright;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.Compare.Secondary;

[Collection("Playwright Tests")]
public class AcademicPerformanceEnglishAndMathsResultsTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private string _pageUrl = "compare/secondary/english-and-maths-results";

    [Fact]
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_LoadsSuccessfully()
    {
        // Arrange
        var queryString = "urns=105574&urns=137020";

        // Act
        var response = await Page.GotoAsync($"{_pageUrl}?{queryString}");

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task EnglishAndMathsResultsPage_Displays_AllGcse_CurrentYear_Chart()
    {
        // Arrange
        var queryString = "urns=105574&urns=137020";

        // Act
        _ = await Page.GotoAsync($"{_pageUrl}?{queryString}");

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
        var queryString = "urns=105574&urns=137020";

        // Act
        _ = await Page.GotoAsync($"{_pageUrl}?{queryString}");

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
        var queryString = "urns=105574&urns=137020";

        // Act
        _ = await Page.GotoAsync($"{_pageUrl}?{queryString}");

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
        var queryString = "urns=105574&urns=137020";

        // Act
        _ = await Page.GotoAsync($"{_pageUrl}?{queryString}");

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
        var queryString = "urns=105574&urns=137020";

        // Act
        _ = await Page.GotoAsync($"{_pageUrl}?{queryString}");

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
    public async Task EnglishAndMathsResultsPage_KeyboardNavigation_CanReachAndFocus_ShowDataOverTimeButton()
    {
        // Arrange
        var queryString = "urns=105574&urns=137020";
        _ = await Page.GotoAsync($"{_pageUrl}?{queryString}");

        // Act
        var reachedShowAsTableButton = await FocusElementByTabAsync("all-gcse-current-year-show-btn");
        Assert.True(reachedShowAsTableButton);

        await Page.Keyboard.PressAsync("Tab");
        var focusedElementId = await Page.EvaluateAsync<string>("() => document.activeElement?.id ?? ''");
        var hasVisibleFocus = await HasVisibleFocusAsync("#all-gcse-show-data-over-time-btn");

        // Assert
        Assert.Equal("all-gcse-show-data-over-time-btn", focusedElementId);
        Assert.True(hasVisibleFocus);

        // Ensure reverse tab order is not trapped or skipped
        await Page.Keyboard.PressAsync("Shift+Tab");
        focusedElementId = await Page.EvaluateAsync<string>("() => document.activeElement?.id ?? ''");
        Assert.Equal("all-gcse-current-year-show-btn", focusedElementId);
    }

    [Fact]
    public async Task EnglishAndMathsResultsPage_KeyboardActivation_ShowAsTableButton_SupportsEnterAndSpace()
    {
        // Arrange
        var queryString = "urns=105574&urns=137020";
        _ = await Page.GotoAsync($"{_pageUrl}?{queryString}");

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
