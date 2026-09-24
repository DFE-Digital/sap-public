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
    public async Task Level3QualificationsPage_LoadsSuccessfully(Level3 level3Qualification)
    {
        // Arrange && Act
        var response = await Page.GotoAsync(GetPageUrl(level3Qualification));

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Theory]
    [InlineData(Level3.ALevel, "A Level")]
    [InlineData(Level3.Academic, "Academic")]
    [InlineData(Level3.AppliedGeneral, "Applied General")]
    public async Task Level3QualificationsPage_HasCorrectTitle(Level3 level3Qualification, string qualTitle)
    {
        // Arrange
        await Page.GotoAsync(GetPageUrl(level3Qualification));

        // Act
        var title = await Page.TitleAsync();

        // Assert
        Assert.Equal($"{_schoolDisplayName} - 16 to 19 - Level 3 qualifications - {qualTitle} - Find and compare school and college profiles - GOV.UK", title);
    }

    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    [InlineData(Level3.AppliedGeneral)]
    public async Task Level3Qualifications_DisplaysMainHeading(Level3 level3Qualification)
    {
        // Arrange
        await Page.GotoAsync(GetPageUrl(level3Qualification));

        // Act
        var heading = await Page.Locator("h1").TextContentAsync();

        // Assert
        Assert.NotNull(heading);
        Assert.NotEmpty(heading.Trim());
    }

    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    [InlineData(Level3.AppliedGeneral)]
    public async Task Level3Qualifications_Displays_VerticalNavigation(Level3 level3Qualification)
    {
        var nav = new VerticalNavigationHelper(Page);
        await Page.GotoAsync(GetPageUrl(level3Qualification));

        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveOneActiveItemAsync();
    }

    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    [InlineData(Level3.AppliedGeneral)]
    public async Task Level3Qualifications_Displays_Sub_Navigation(Level3 level3Qualification)
    {
        // Arrange
        await Page.GotoAsync(GetPageUrl(level3Qualification));

        // Act
        var isVisible = await Page.Locator("#sub-navigation-academic-performance").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    [InlineData(Level3.AppliedGeneral)]
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
