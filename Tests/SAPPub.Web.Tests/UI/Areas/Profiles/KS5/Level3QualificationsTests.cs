using SAPPub.Core.Enums.KS5Qualifications;
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
        var response = await Page.GotoAsync(GetPageUrl(level3Qualification));

        //bypass cookies
        await Page.Keyboard.PressAsync("Tab");
        await Page.Keyboard.PressAsync("Tab");
        await Page.Keyboard.PressAsync("Tab");

        //start of page - _Layout page
        await Page.Keyboard.PressAsync("Tab");

        var activeElement = await Page.EvaluateHandleAsync("document.activeElement");
        var prop = await activeElement.GetPropertyAsync("textContent");
        string textContent = await prop.JsonValueAsync<string>();
        Assert.Equal("Skip to main content", textContent);

        //start of content - Default page
        await Page.Keyboard.PressAsync("Enter");
        await Page.Keyboard.PressAsync("Tab");

        activeElement = await Page.EvaluateHandleAsync("document.activeElement");
        prop = await activeElement.GetPropertyAsync("textContent");
        textContent = await prop.JsonValueAsync<string>();
        Assert.Equal("Skip contents", textContent);

        //start of right side content - PupilProgress KS2 page
        await Page.Keyboard.PressAsync("Enter");
        await Page.Keyboard.PressAsync("Tab");

        activeElement = await Page.EvaluateHandleAsync("document.activeElement");
        prop = await activeElement.GetPropertyAsync("href");
        string href = await prop.JsonValueAsync<string>();
        Assert.Equal("https://explore-education-statistics.service.gov.uk/find-statistics", href);
    }

    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    [InlineData(Level3.AppliedGeneral)]
    [InlineData(Level3.TechLevel)]
    public async Task Level3Qualifications_Displays_CurrentYear_Table(Level3 level3Qualification)
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
    public async Task Level3Qualifications_Displays_DataOverTime_Table(Level3 level3Qualification)
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
}
