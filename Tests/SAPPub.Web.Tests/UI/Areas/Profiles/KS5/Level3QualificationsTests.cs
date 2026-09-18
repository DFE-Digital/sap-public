using SAPPub.Playwright.Testing;
using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;
using PageConstants = SAPPub.Playwright.Testing.KS5.AcademicPerformanceLevel3QualificationsPageConstants;

namespace SAPPub.Web.Tests.UI.Areas.Profiles.KS5;

[Collection("Playwright Tests")]
public class Level3QualificationsTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private const string _urn = "130499";
    private const string _schoolName = "holy-cross-college";
    private const string _schoolDisplayName = "Holy Cross College";
    private const string _basePageUrl = $"school/{_urn}/{_schoolName}/16-to-19-performance/level-3-qualifications/alevel";

    [Fact]
    public async Task Level3QualificationsPage_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(_basePageUrl);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task Level3QualificationsPage_HasCorrectTitle()
    {
        // Arrange
        await Page.GotoAsync(_basePageUrl);

        // Act
        var title = await Page.TitleAsync();

        // Assert
        Assert.Equal($"{_schoolDisplayName} - 16 to 19 - Level 3 qualifications - A Level - School Profiles - GOV.UK", title);
    }

    [Fact]
    public async Task Level3Qualifications_DisplaysMainHeading()
    {
        // Arrange
        await Page.GotoAsync(_basePageUrl);

        // Act
        var heading = await Page.Locator("h1").TextContentAsync();

        // Assert
        Assert.NotNull(heading);
        Assert.NotEmpty(heading.Trim());
    }

    [Fact]
    public async Task Level3Qualifications_Displays_VerticalNavigation()
    {
        var nav = new VerticalNavigationHelper(Page);
        await Page.GotoAsync(_basePageUrl);

        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveOneActiveItemAsync();
    }

    [Fact]
    public async Task Level3Qualifications_Displays_Sub_Navigation()
    {
        // Arrange
        await Page.GotoAsync(_basePageUrl);

        // Act
        var isVisible = await Page.Locator("#sub-navigation-academic-performance").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task Level3Qualifications_Displays_CurrentYear_Table()
    {
        // Arrange
        await Page.GotoAsync(_basePageUrl);

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

    [Fact]
    public async Task Level3Qualifications_Displays_DataOverTime_Table()
    {
        // Arrange
        await Page.GotoAsync(_basePageUrl);

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
}
