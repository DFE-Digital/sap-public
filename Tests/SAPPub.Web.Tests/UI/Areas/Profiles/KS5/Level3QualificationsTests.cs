using Microsoft.Playwright;
using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Web.Tests.UI.Infrastructure;
using PageConstants = SAPPub.Playwright.Testing.KS5.AcademicPerformanceLevel3QualificationsPageConstants;

namespace SAPPub.Web.Tests.UI.Areas.Profiles.KS5;

[Collection("Playwright Tests")]
public class Level3QualificationsTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private const string _urn = "130499";
    private const string _schoolName = "holy-cross-college";
    private static string GetPageUrl(Level3 level3Qualification) => $"school/{_urn}/{_schoolName}/16-to-19-performance/level-3-qualifications/{level3Qualification.ToString().ToLower()}";

    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    [InlineData(Level3.AppliedGeneral)]
    [InlineData(Level3.TechLevel)]
    public async Task Level3QualificationsPage_LoadsSuccessfully(Level3 level3Qualification)
    {
        // Arrange && Act
        var response = await Page.GotoAsync(GetPageUrl(level3Qualification));

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    [InlineData(Level3.AppliedGeneral)]
    [InlineData(Level3.TechLevel)]
    public async Task Level3QualificationsPage_KS5_AccessibilityNavigation(Level3 level3Qualification)
    {
        // Arrange && Act
        await SkipContents_AccessibilityNavigation(GetPageUrl(level3Qualification));

        // Assert
        var activeElement = await Page.EvaluateHandleAsync("document.activeElement");
        var prop = await activeElement.GetPropertyAsync("href");
        string href = await prop.JsonValueAsync<string>();
        Assert.Equal("https://explore-education-statistics.service.gov.uk/find-statistics", href);
    }

    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    [InlineData(Level3.AppliedGeneral)]
    [InlineData(Level3.TechLevel)]
    public async Task Level3Qualifications_Displays_AverageResult_CurrentYear_Table(Level3 level3Qualification)
    {
        // Arrange
        await Page.GotoAsync(GetPageUrl(level3Qualification));

        // Act        
        var table = Page.Locator(PageConstants.AverageResultCurrentYearTableContainerId);
        var showCurrentDataTableBtn = Page.Locator(PageConstants.AverageResultShowCurrentDataBtnId);
        var showDataOverTimeBtn = Page.Locator(PageConstants.AverageResultShowDataOverTimeBtnId);

        var isTableVisible = await table.IsVisibleAsync();
        var isShowCurrentDataTableBtnVisible = await showCurrentDataTableBtn.IsVisibleAsync();
        var isShowDataOverTimeBtnVisible = await showDataOverTimeBtn.IsVisibleAsync();
        var showDataOverTimeBtnText = await showDataOverTimeBtn.TextContentAsync();

        // Assert
        Assert.True(isTableVisible);
        Assert.False(isShowCurrentDataTableBtnVisible);
        Assert.True(isShowDataOverTimeBtnVisible);

        Assert.Equal("Show data over time", showDataOverTimeBtnText);
    }

    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    [InlineData(Level3.AppliedGeneral)]
    [InlineData(Level3.TechLevel)]
    public async Task Level3Qualifications_Displays_AverageResult_DataOverTime_Table(Level3 level3Qualification)
    {
        // Arrange
        await Page.GotoAsync(GetPageUrl(level3Qualification));

        // Act
        // Click Show data over time button
        await Page.ClickAsync(PageConstants.AverageResultShowDataOverTimeBtnId);

        var table = Page.Locator(PageConstants.AverageResultDataOverTimeTableContainerId);
        var showCurrentDataBtn = Page.Locator(PageConstants.AverageResultShowCurrentDataBtnId);
        var showDataOverTimeBtn = Page.Locator(PageConstants.AverageResultShowDataOverTimeBtnId);

        var isTableVisible = await table.IsVisibleAsync();
        var isShowCurrentDataBtnVisible = await showCurrentDataBtn.IsVisibleAsync();
        var isShowDataOverTimeBtnVisible = await showDataOverTimeBtn.IsVisibleAsync();
        var showCurrentDataBtnText = await showCurrentDataBtn.TextContentAsync();

        // Assert
        Assert.True(isTableVisible);
        Assert.True(isShowCurrentDataBtnVisible);
        Assert.False(isShowDataOverTimeBtnVisible);

        Assert.Equal("Show current data", showCurrentDataBtnText);
    }

    [Theory]
    [InlineData(Level3.ALevel)]
    public async Task Level3Qualifications_Displays_RetentionMeasure_CurrentYear_Chart(Level3 level3Qualification)
    {
        // Arrange
        await Page.GotoAsync(GetPageUrl(level3Qualification));

        // Act
        var chart = Page.Locator("#all-retention-assessed-chart");
        var table = Page.Locator("#all-retention-assessed-current-year-table");
        var showAsTableBtn = Page.Locator("#all-retention-assessed-current-year-show-btn");
        var showDataOverTimeBtn = Page.Locator("#all-retention-assessed-show-data-over-time-btn");

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

    [Theory]
    [InlineData(Level3.ALevel)]
    public async Task Level3Qualifications_Displays_RetentionMeasure_CurrentYear_Table(Level3 level3Qualification)
    {
        // Arrange
        await Page.GotoAsync(GetPageUrl(level3Qualification));

        // Act
        // Click Show as a table button
        await Page.ClickAsync("#all-retention-assessed-current-year-show-btn");

        var showAsTableBtn = Page.Locator("#all-retention-assessed-current-year-show-btn");
        var showDataOverTimeBtn = Page.Locator("#all-retention-assessed-show-data-over-time-btn");
        var chart = Page.Locator("#all-retention-assessed-chart");
        var table = Page.Locator("#all-retention-assessed-current-year-table");

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

    [Theory]
    [InlineData(Level3.ALevel)]
    public async Task Level3Qualifications_Displays_RetentionMeasure_DataOverTime_Chart(Level3 level3Qualification)
    {
        // Arrange
        await Page.GotoAsync(GetPageUrl(level3Qualification));

        // Act
        // Click Show data over time button
        await Page.ClickAsync("#all-retention-assessed-show-data-over-time-btn");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var chart = Page.Locator("#all-retention-assessed-data-overtime-chart");
        var table = Page.Locator("#all-retention-assessed-data-overtime-table");
        var chartLegend = Page.Locator("#all-retention-assessed-data-overtime-chart-legend");
        var chartLegendExplainer = Page.Locator("#all-retention-assessed-data-over-time-legend-explainer");
        var showAsTableBtn = Page.Locator("#all-retention-assessed-data-over-time-show-btn");
        var showCurrentDataBtn = Page.Locator("#all-retention-assessed-show-current-data-btn");

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
        Assert.True(await chartLegendExplainer.IsVisibleAsync());

        Assert.Equal("Show as a table", showAsTableBtnText);
        Assert.Equal("Show current data", showCurrentDataBtnText);
    }

    [Theory]
    [InlineData(Level3.ALevel)]
    public async Task Level3Qualifications_Displays_RetentionMeasure_DataOverTime_Table(Level3 level3Qualification)
    {
        // Arrange
        await Page.GotoAsync(GetPageUrl(level3Qualification));

        // Act
        // Click Show data over time button
        await Page.ClickAsync("#all-retention-assessed-show-data-over-time-btn");

        // and click Show as a table button
        await Page.ClickAsync("#all-retention-assessed-data-over-time-show-btn");

        var chart = Page.Locator("#all-retention-assessed-data-overtime-chart");
        var table = Page.Locator("#all-retention-assessed-data-overtime-table");
        var chartLegend = Page.Locator("#all-retention-assessed-data-overtime-chart-legend");
        var showAsTableBtn = Page.Locator("#all-retention-assessed-data-over-time-show-btn");
        var showCurrentDataBtn = Page.Locator("#all-retention-assessed-show-current-data-btn");

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

    [Theory]
    [InlineData(Level3.ALevel)]
    public async Task EnglishAndMathsResultsPage_Displays_RetentionMeasure_DataOverTime_Table_Click_On_ShowCurrentData(Level3 level3Qualification)
    {
        // Arrange
        await Page.GotoAsync(GetPageUrl(level3Qualification));

        // Act
        // Click Show data over time button
        await Page.ClickAsync("#all-retention-assessed-show-data-over-time-btn");

        // and click Show current data button
        await Page.ClickAsync("#all-retention-assessed-show-current-data-btn");

        var chart = Page.Locator("#all-retention-assessed-chart");
        var showAsTableBtn = Page.Locator("#all-retention-assessed-current-year-show-btn");
        var showDataOverTimeBtn = Page.Locator("#all-retention-assessed-show-data-over-time-btn");

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
