using Microsoft.Playwright;
using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.SecondarySchool;

[Collection("Playwright Tests")]
public class DestinationsPageTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private string _pageUrl = "school/105574/Loreto%20High%20School%20Chorlton/secondary/destinations";

    [Fact]
    public async Task Destinations_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(_pageUrl);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task DestinationsPage_HasCorrectTitle()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var title = await Page.TitleAsync();

        // Assert
        Assert.Contains("Destinations", title);
    }

    [Fact]
    public async Task DestinationsPage_DisplaysMainHeading()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var heading = await Page.Locator("h1").TextContentAsync();

        // Assert
        Assert.NotNull(heading.Replace(" ", ""));

    }

    [Fact]
    public async Task DestinationsPage_Displays_SchoolName_Caption()
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
    public async Task DestinationsPage_Displays_VerticalNavigation()
    {
        var nav = new VerticalNavigationHelper(Page);
        await Page.GotoAsync(_pageUrl);

        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveItemsCountAsync(6);
        await nav.ShouldHaveOneActiveItemAsync();
        await nav.ShouldHaveActiveHrefAsync(_pageUrl);
    }

    [Fact]
    public async Task DestinationsPage_Displays_AllDestinations_CurrentYear_Chart()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var chart = Page.Locator("#all-destinations-chart");
        var table = Page.Locator("#all-destinations-current-year-table");
        var showAsTableBtn = Page.Locator("#all-dest-current-year-show-btn");
        var showDataOverTimeBtn = Page.Locator("#all-dest-show-data-over-time-btn");
        
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
    public async Task DestinationsPage_Displays_AllDestinations_CurrentYear_Table()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        // Click Show as a table button
        await Page.ClickAsync("#all-dest-current-year-show-btn");

        var showAsTableBtn = Page.Locator("#all-dest-current-year-show-btn");
        var showDataOverTimeBtn = Page.Locator("#all-dest-show-data-over-time-btn");
        var chart = Page.Locator("#all-destinations-chart");
        var table = Page.Locator("#all-destinations-current-year-table");

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
    public async Task DestinationsPage_Displays_AllDestinations_DataOverTime_Chart()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        // Click Show data over time button
        await Page.ClickAsync("#all-dest-show-data-over-time-btn");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var chart = Page.Locator("#all-destinations-data-overtime-chart");
        var table = Page.Locator("#all-destinations-data-overtime-table");
        var showAsTableBtn = Page.Locator("#all-dest-data-over-time-show-btn");
        var showCurrentDataBtn = Page.Locator("#all-dest-show-current-data-btn");

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
    public async Task DestinationsPage_Displays_AllDestinations_DataOverTime_Table()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        // Click Show data over time button
        await Page.ClickAsync("#all-dest-show-data-over-time-btn");

        // and click Show as a table button
        await Page.ClickAsync("#all-dest-data-over-time-show-btn");

        var chart = Page.Locator("#all-destinations-data-overtime-chart");
        var table = Page.Locator("#all-destinations-data-overtime-table");        
        var showAsTableBtn = Page.Locator("#all-dest-data-over-time-show-btn");
        var showCurrentDataBtn = Page.Locator("#all-dest-show-current-data-btn");

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
    public async Task DestinationsPage_Displays_AllDestinations_DataOverTime_Table_Click_On_ShowCurrentData()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        // Click Show data over time button
        await Page.ClickAsync("#all-dest-show-data-over-time-btn");

        // and click Show current data button
        await Page.ClickAsync("#all-dest-show-current-data-btn");

        var chart = Page.Locator("#all-destinations-chart");
        var showAsTableBtn = Page.Locator("#all-dest-current-year-show-btn");
        var showDataOverTimeBtn = Page.Locator("#all-dest-show-data-over-time-btn");

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
    public async Task DestinationsPage_Displays_BreakdownDestinations_CurrentYear_Chart()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var chart = Page.Locator("#breakdown-destinations-chart");
        var table = Page.Locator("#breakdown-destinations-current-year-table");
        var chartLegend = Page.Locator("#breakdown-destinations-chart-legend");
        var showAsTableBtn = Page.Locator("#breakdown-dest-current-year-show-btn");

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
    public async Task DestinationsPage_Displays_BreakdownDestinations_CurrentYear_Table()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        // Click Show as a table button
        await Page.ClickAsync("#breakdown-dest-current-year-show-btn");
         
        var showAsTableBtn = Page.Locator("#breakdown-dest-current-year-show-btn");
        var chart = Page.Locator("#breakdown-destinations-chart");
        var table = Page.Locator("#breakdown-destinations-current-year-table");
        var chartLegend = Page.Locator("#breakdown-destinations-chart-legend");

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
    public async Task DestinationsPage_DisplaysPagination()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#destinations-pagination").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }
}
