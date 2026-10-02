using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Web.Tests.UI.Infrastructure;
using PageConstants = SAPPub.Playwright.Testing.KS5.AcademicPerformanceLevel3QualificationsPageConstants;

namespace SAPPub.Web.Tests.UI.Areas.Profiles.KS5;

[Collection("Playwright Tests")]
public class Level2QualificationsTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private const string _urn = "130499";
    private const string _schoolName = "holy-cross-college";

    private static string GetPageUrl(Level2 level2Qualification) => $"school/{_urn}/{_schoolName}/16-to-19-performance/level-2-qualifications/{level2Qualification.ToString().ToLower()}";

    [Theory]
    [InlineData(Level2.TechCert)]    
    public async Task Level2QualificationsPage_LoadsSuccessfully(Level2 level2Qualification)
    {
        // Arrange && Act
        var response = await Page.GotoAsync(GetPageUrl(level2Qualification));

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Theory]
    [InlineData(Level2.TechCert)]    
    public async Task Level2Qualifications_Displays_CurrentYear_Table(Level2 level2Qualification)
    {
        // Arrange
        await Page.GotoAsync(GetPageUrl(level2Qualification));

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
    [InlineData(Level2.TechCert)]    
    public async Task Level2Qualifications_Displays_DataOverTime_Table(Level2 level2Qualification)
    {
        // Arrange
        await Page.GotoAsync(GetPageUrl(level2Qualification));

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

