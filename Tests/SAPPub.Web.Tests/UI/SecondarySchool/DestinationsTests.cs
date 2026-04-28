using Microsoft.Playwright;
using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.SecondarySchool;

[Collection("Playwright Tests")]
public class DestinationsPageTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private Dictionary<string, string> _schoolUrnToUrlMap = new Dictionary<string, string>
    {
        ["105574"] = "school/105574/loreto-high-school-chorlton/secondary/destinations",
        ["100273"] = "school/100273/saint-paul-roman-catholic-infant-school/secondary/destinations",
    };

    [Fact]
    public async Task Destinations_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task DestinationsPage_HasCorrectTitle()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var title = await Page.TitleAsync();

        // Assert
        Assert.Contains("Destinations", title);
    }

    [Fact]
    public async Task DestinationsPage_DisplaysMainHeading()
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
    public async Task DestinationsPage_Displays_SchoolName_Caption()
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
    public async Task DestinationsPage_Displays_VerticalNavigation()
    {
        var nav = new VerticalNavigationHelper(Page);
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveItemsCountAsync(5);
        await nav.ShouldHaveOneActiveItemAsync();
        await nav.ShouldHaveActiveHrefAsync(_schoolUrnToUrlMap["105574"]);
    }

    [Fact]
    public async Task DestinationsPage_Displays_School_Performance_Info()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var schoolPerformanceInfoLocator = Page.GetByTestId("looking-at-school-performance-info");
        var isVisible = await schoolPerformanceInfoLocator.IsVisibleAsync();
        var schoolPerformanceInfo = await schoolPerformanceInfoLocator.TextContentAsync();

        // Assert
        Assert.True(isVisible);
        Assert.NotNull(schoolPerformanceInfo);
        Assert.NotEmpty(schoolPerformanceInfo!.Trim());
    }

    [Fact]
    public async Task DestinationsPage_Displays_AllDestinations_CurrentYear_Chart()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

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
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

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
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

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
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

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
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

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
    public async Task DestinationsPage_Displays_AllDestinations__DataOverTime_No_Chart_Only_Render_Table()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["100273"]);

        // Act       
        var destinationsChart = Page.Locator("#all-destinations-chart");
        var destinationsCurrentYearTable = Page.Locator("#all-destinations-current-year-table");
        var destinationsCurrentYearShowBtn = Page.Locator("#all-dest-current-year-show-btn");
        var destinationsShowDataOverTimeBtn = Page.Locator("#all-dest-show-data-over-time-btn");

        var destinationsDataOverTimeChart = Page.Locator("#all-destinations-data-overtime-chart");
        var destinationsDataOverTimeTable = Page.Locator("#all-destinations-data-overtime-table");
        var destinationsDataOverTimeShowBtn = Page.Locator("#all-dest-data-over-time-show-btn");
        var destinationsShowCurrentDataBtn = Page.Locator("#all-dest-show-current-data-btn");

        // Assert
        Assert.False(await destinationsChart.CountAsync() > 0);
        Assert.False(await destinationsCurrentYearTable.CountAsync() > 0);
        Assert.False(await destinationsCurrentYearShowBtn.CountAsync() > 0);
        Assert.False(await destinationsShowDataOverTimeBtn.CountAsync() > 0);
        Assert.False(await destinationsDataOverTimeChart.CountAsync() > 0);
        Assert.False(await destinationsDataOverTimeShowBtn.CountAsync() > 0);
        Assert.False(await destinationsShowCurrentDataBtn.CountAsync() > 0);
        Assert.True(await destinationsDataOverTimeTable.CountAsync() > 0);
    }

    //[Fact]
    //public async Task DestinationsPage_Displays_BreakdownDestinations_CurrentYear_Chart()
    //{
    //    // Arrange
    //    await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

    //    // Act
    //    var chart = Page.Locator("#breakdown-destinations-chart");
    //    var table = Page.Locator("#breakdown-destinations-current-year-table");
    //    var chartLegend = Page.Locator("#breakdown-destinations-chart-legend");
    //    var showAsTableBtn = Page.Locator("#breakdown-dest-current-year-show-btn");

    //    var isChartVisible = await chart.IsVisibleAsync();
    //    var isTableVisible = await table.IsVisibleAsync();
    //    var isChartLegendVisible = await chartLegend.IsVisibleAsync();
    //    var isShowAsTableBtnVisible = await showAsTableBtn.IsVisibleAsync();
    //    var showAsTableBtnText = await showAsTableBtn.TextContentAsync();

    //    // Assert
    //    Assert.False(isTableVisible);
    //    Assert.True(isChartVisible);
    //    Assert.True(isChartLegendVisible);
    //    Assert.True(isShowAsTableBtnVisible);

    //    Assert.Equal("Show as a table", showAsTableBtnText);
    //}

    //[Fact]
    //public async Task DestinationsPage_Displays_BreakdownDestinations_CurrentYear_Table()
    //{
    //    // Arrange
    //    await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

    //    // Act
    //    // Click Show as a table button
    //    await Page.ClickAsync("#breakdown-dest-current-year-show-btn");
         
    //    var showAsTableBtn = Page.Locator("#breakdown-dest-current-year-show-btn");
    //    var chart = Page.Locator("#breakdown-destinations-chart");
    //    var table = Page.Locator("#breakdown-destinations-current-year-table");
    //    var chartLegend = Page.Locator("#breakdown-destinations-chart-legend");

    //    var isChartVisible = await chart.IsVisibleAsync();
    //    var isTableVisible = await table.IsVisibleAsync();
    //    var isChartLegendVisible = await chartLegend.IsVisibleAsync();
    //    var buttonText = await showAsTableBtn.TextContentAsync();

    //    // Assert
    //    Assert.False(isChartVisible);
    //    Assert.False(isChartLegendVisible);
    //    Assert.True(isTableVisible);
    //    Assert.Equal("Show as a chart", buttonText);
    //}

    //[Fact]
    //public async Task DestinationsPage_Displays_BreakdownDestinations_No_Chart_Only_Render_Table()
    //{
    //    // Arrange
    //    await Page.GotoAsync(_schoolUrnToUrlMap["100273"]);

    //    // Act       
    //    var breakdownDestinationsChart = Page.Locator("#breakdown-destinations-chart");
    //    var breakdownDestinationsCurrentYearTable = Page.Locator("#breakdown-destinations-current-year-table");

    //    var breakdownDestinationsCurrentYearShowBtn = Page.Locator("#breakdown-dest-current-year-show-btn");

    //    // Assert
    //    Assert.False(await breakdownDestinationsChart.CountAsync() > 0);
    //    Assert.False(await breakdownDestinationsCurrentYearShowBtn.CountAsync() > 0);
    //    Assert.True(await breakdownDestinationsCurrentYearTable.CountAsync() > 0);
    //}

    [Fact]
    public async Task DestinationsPage_DisplaysPagination()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var isVisible = await Page.Locator("#destinations-pagination").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }
}
