using Microsoft.Playwright;
using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.Areas.Profiles.KS2;

[Collection("Playwright Tests")]
public class MeetingOrExceedingStandardsTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private Dictionary<string, string> _schoolUrnToUrlMap = new Dictionary<string, string>
    {
        ["143034"] = "school/143034/st-pauls-church-of-england-academy/primary-performance/meeting-or-exceeding-standards",
    };

    private Dictionary<string, string> _pageElements = new Dictionary<string, string>
    {
        ["currentYearShowAsTableBtn"] = "#mes-current-year-show-btn",
        ["currentYearChartContainer"] = "#mes-current-year-chart-container",
        ["currentYearTableContainer"] = "#mes-current-year-table-container",
        ["showDataOverTimeBtn"] = "#mes-show-data-over-time-btn",
        ["dataOverTimeChartContainer"] = "#mes-data-over-time-chart-container",
        ["dataOverTimeTableContainer"] = "#mes-data-over-time-table-container",
        ["dataOverTimeChartLegend"] = "#mes-data-overtime-chart-legend",
        ["dataOverTimeShowAsTableBtn"] = "#mes-data-over-time-show-btn",
        ["showCurrentDataBtn"] = "#mes-show-current-data-btn"
    };

    [Fact]
    public async Task MeetingOrExceedingStandardsResultsPage_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task MeetingOrExceedingStandardsResultsPage_HasCorrectTitle()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        var title = await Page.TitleAsync();

        // Assert
        Assert.Contains("St Paul's Church of England Academy - Primary Meeting or exceeding standards - School Profiles - GOV.UK", title);
    }

    [Fact]
    public async Task MeetingOrExceedingStandardsResultsPage_DisplaysMainHeading()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        var heading = await Page.Locator("h1").TextContentAsync();

        // Assert
        Assert.NotNull(heading);
        Assert.NotEmpty(heading!.Trim());
    }

    [Fact]
    public async Task MeetingOrExceedingStandardsResultsPage_Displays_VerticalNavigation()
    {
        var performancePage = "school/143034/st-pauls-church-of-england-academy/primary-performance/pupil-progress";
        // We want to display the performance root page even when in a performance sub-page, hence need to check the active href is the root performance page

        var nav = new VerticalNavigationHelper(Page);
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveOneActiveItemAsync();
        await nav.ShouldHaveActiveHrefAsync(performancePage);
    }

    [Fact]
    public async Task MeetingOrExceedingStandardsResultsPage_Displays_Sub_Navigation()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        var isVisible = await Page.Locator("#sub-navigation-academic-performance").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task MeetingOrExceedingStandardsResultsPage_Displays_CurrentYear_Chart()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        var chart = Page.Locator(_pageElements["currentYearChartContainer"]);
        var table = Page.Locator(_pageElements["currentYearTableContainer"]);
        var showAsTableBtn = Page.Locator(_pageElements["currentYearShowAsTableBtn"]);
        var showDataOverTimeBtn = Page.Locator(_pageElements["showDataOverTimeBtn"]);

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
    public async Task MeetingOrExceedingStandardsResultsPage_Displays_CurrentYear_Table()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        // Click Show as a table button
        await Page.ClickAsync(_pageElements["currentYearShowAsTableBtn"]);

        var showAsTableBtn = Page.Locator(_pageElements["currentYearShowAsTableBtn"]);
        var showDataOverTimeBtn = Page.Locator(_pageElements["showDataOverTimeBtn"]);
        var chart = Page.Locator(_pageElements["currentYearChartContainer"]);
        var table = Page.Locator(_pageElements["currentYearTableContainer"]);

        var isChartVisible = await chart.IsVisibleAsync();
        var isTableVisible = await table.IsVisibleAsync();
        var isShowDataOverTimeBtnVisible = await showDataOverTimeBtn.IsVisibleAsync();
        var buttonText = await showAsTableBtn.TextContentAsync();
        var showDataOverTimeBtnText = await showDataOverTimeBtn.TextContentAsync();
        var firstTableCellText = await table.Locator("td").First.TextContentAsync();

        // Assert
        Assert.False(isChartVisible);
        Assert.True(isTableVisible);
        Assert.True(isShowDataOverTimeBtnVisible);
        Assert.Equal("Show as a chart", buttonText);
        Assert.Equal("Show data over time", showDataOverTimeBtnText);
        Assert.Contains("Not available", firstTableCellText);
    }

    [Fact]
    public async Task MeetingOrExceedingStandardsResultsPage_Displays_DataOverTime_Chart()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        // Click Show data over time button
        await Page.ClickAsync(_pageElements["showDataOverTimeBtn"]);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var chart = Page.Locator(_pageElements["dataOverTimeChartContainer"]);
        var table = Page.Locator(_pageElements["dataOverTimeTableContainer"]);
        var chartLegend = Page.Locator(_pageElements["dataOverTimeChartLegend"]);
        var showAsTableBtn = Page.Locator(_pageElements["dataOverTimeShowAsTableBtn"]);
        var showCurrentDataBtn = Page.Locator(_pageElements["showCurrentDataBtn"]);

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
    public async Task MeetingOrExceedingStandardsResultsPage_Displays_DataOverTime_Table()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        // Click Show data over time button
        await Page.ClickAsync(_pageElements["showDataOverTimeBtn"]);

        // and click Show as a table button
        await Page.ClickAsync(_pageElements["dataOverTimeShowAsTableBtn"]);

        var chart = Page.Locator(_pageElements["dataOverTimeChartContainer"]);
        var table = Page.Locator(_pageElements["dataOverTimeTableContainer"]);
        var chartLegend = Page.Locator(_pageElements["dataOverTimeChartLegend"]);
        var showAsTableBtn = Page.Locator(_pageElements["dataOverTimeShowAsTableBtn"]);
        var showCurrentDataBtn = Page.Locator(_pageElements["showCurrentDataBtn"]);

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
    public async Task MeetingOrExceedingStandardsResultsPage_Displays_DataOverTime_Table_Click_On_ShowCurrentData()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        // Click Show data over time button
        await Page.ClickAsync(_pageElements["showDataOverTimeBtn"]);

        // and click Show current data button
        await Page.ClickAsync(_pageElements["showCurrentDataBtn"]);

        var chart = Page.Locator(_pageElements["currentYearChartContainer"]);
        var showAsTableBtn = Page.Locator(_pageElements["currentYearShowAsTableBtn"]);
        var showDataOverTimeBtn = Page.Locator(_pageElements["showDataOverTimeBtn"]);

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
    public async Task MeetingOrExceedingStandardsResultsPage_KeyboardNavigation_CanReachAndFocus_ToggleButtons()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        var reachedShowAsTableButton = await FocusElementByTabAsync(_pageElements["currentYearShowAsTableBtn"].Replace("#", ""));
        Assert.True(reachedShowAsTableButton);

        var hasVisibleFocusOnShowAsTable = await HasVisibleFocusAsync(_pageElements["currentYearShowAsTableBtn"]);

        await Page.Keyboard.PressAsync("Tab");
        var focusedElementId = await Page.EvaluateAsync<string>("() => document.activeElement?.id ?? ''");
        var hasVisibleFocusOnShowDataOverTime = await HasVisibleFocusAsync(_pageElements["showDataOverTimeBtn"]);

        // Assert
        Assert.True(hasVisibleFocusOnShowAsTable);
        Assert.Equal("mes-show-data-over-time-btn", focusedElementId);
        Assert.True(hasVisibleFocusOnShowDataOverTime);

        // Ensure reverse tab order is not trapped or skipped
        await Page.Keyboard.PressAsync("Shift+Tab");
        focusedElementId = await Page.EvaluateAsync<string>("() => document.activeElement?.id ?? ''");
        Assert.Equal("mes-current-year-show-btn", focusedElementId);
    }

    [Fact]
    public async Task MeetingOrExceedingStandardsResultsPage_KeyboardActivation_ShowAsTableButton_SupportsEnterAndSpace()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act - Enter switches to table view
        var reachedShowAsTableButton = await FocusElementByTabAsync(_pageElements["currentYearShowAsTableBtn"].Replace("#", ""));
        Assert.True(reachedShowAsTableButton);

        await Page.Keyboard.PressAsync("Enter");

        var chart = Page.Locator(_pageElements["currentYearChartContainer"]);
        var table = Page.Locator(_pageElements["currentYearTableContainer"]);
        Assert.False(await chart.IsVisibleAsync());
        Assert.True(await table.IsVisibleAsync());

        // Act - Space switches back to chart view
        await Page.Keyboard.PressAsync("Space");

        // Assert
        Assert.True(await chart.IsVisibleAsync());
        Assert.False(await table.IsVisibleAsync());
    }

    [Fact]
    public async Task MeetingOrExceedingStandardsResultsPage_KeyboardActivation_ShowDataOverTimeAndShowCurrentDataButtons_SupportEnterAndSpace()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act - Enter on show data over time
        await Page.Locator(_pageElements["showDataOverTimeBtn"]).FocusAsync();
        await Page.Keyboard.PressAsync("Enter");

        // Assert
        Assert.True(await IsElementCheckedAsync("mes-data-overtime-view"));
        Assert.False(await IsElementCheckedAsync("mes-current-view"));

        // Act - Space on show current data
        await Page.Locator(_pageElements["showCurrentDataBtn"]).FocusAsync();
        await Page.Keyboard.PressAsync("Space");

        // Assert
        Assert.True(await IsElementCheckedAsync("mes-current-view"));
        Assert.False(await IsElementCheckedAsync("mes-data-overtime-view"));

        // Act - Space on show data over time
        await Page.Locator(_pageElements["showDataOverTimeBtn"]).FocusAsync();
        await Page.Keyboard.PressAsync("Space");

        // Assert
        Assert.True(await IsElementCheckedAsync("mes-data-overtime-view"));
        Assert.False(await IsElementCheckedAsync("mes-current-view"));

        // Act - Enter on show current data
        await Page.Locator(_pageElements["showCurrentDataBtn"]).FocusAsync();
        await Page.Keyboard.PressAsync("Enter");

        // Assert
        Assert.True(await IsElementCheckedAsync("mes-current-view"));
        Assert.False(await IsElementCheckedAsync("mes-data-overtime-view"));
    }

    [Fact]
    public async Task MeetingOrExceedingStandardsResultsPage_KeyboardActivation_ShowDataOverTime_Enter_MovesFocusToShowCurrentData()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        var reachedShowAsTableButton = await FocusElementByTabAsync(_pageElements["currentYearShowAsTableBtn"].Replace("#", ""));
        Assert.True(reachedShowAsTableButton);

        await Page.Keyboard.PressAsync("Tab");

        // Act
        await Page.Keyboard.PressAsync("Enter");

        // Assert
        var focusedElementMoved = await WaitForFocusedElementAsync(_pageElements["showCurrentDataBtn"].Replace("#", ""));
        Assert.True(focusedElementMoved);
    }

    [Fact]
    public async Task MeetingOrExceedingStandardsResultsPage_KeyboardActivation_ShowCurrentData_Enter_TabSequenceCanReachShowDataOverTime()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        var reachedShowAsTableButton = await FocusElementByTabAsync(_pageElements["currentYearShowAsTableBtn"].Replace("#", ""));
        Assert.True(reachedShowAsTableButton);

        await Page.Keyboard.PressAsync("Tab");
        await Page.Keyboard.PressAsync("Enter");

        var focusedOnShowCurrentData = await WaitForFocusedElementAsync(_pageElements["showCurrentDataBtn"].Replace("#", ""));
        Assert.True(focusedOnShowCurrentData);

        // Act
        await Page.Keyboard.PressAsync("Enter");

        // Assert
        var reachedShowDataOverTimeButton = await FocusElementByTabAsync(_pageElements["showDataOverTimeBtn"].Replace("#", ""), 120);
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
