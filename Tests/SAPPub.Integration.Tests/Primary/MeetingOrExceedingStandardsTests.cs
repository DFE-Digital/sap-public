using Microsoft.Playwright;
using SAPPub.Playwright.Testing;
using SAPPub.Playwright.Testing.Primary.Performance;

namespace SAPPub.Integration.Tests.Primary;

[Collection("Integration Tests")]
public class MeetingOrExceedingStandardsTests() : BasePageTest()
{
    private string BasePageUrl(string urn) => $"/school/{urn}";
    private string ThisPage => "primary-performance/meeting-or-exceeding-standards";

    [Theory]
    [InlineData("100019", "83", "15", "75", "20", "73", "25")]
    [InlineData("100241", "72", "16", "63", "19", "77", "14")]
    [InlineData("100353", "91", "44", "87", "27", "93", "46")]
    [InlineData("100448", "57", "4", "68", "11", "50", "0")]
    [InlineData("100500", "90", "14", "70", "3", "83", "17")]
    [InlineData("100674", "69", "9", "71", "9", "76", "8")]
    [InlineData("100684", "85", "24", "83", "15", "84", "18")]
    public async Task TableShowsYearData(string urn, string expectedCurrent, string higherCurrent, string expectedPrevious, string higherPrevious, string expectedPrevious2, string higherPrevious2)
    {
        // Arrange && Act
        var response = await Page.GotoAsync(BasePageUrl(urn));
        Assert.NotNull(response);
        var _ = await Page.GotoPage(response.Url, ThisPage);

        // Act
        // Click Show data over time button
        await Page.ClickAsync(PageConstants.ContentIds["showDataOverTimeBtn"]);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // and click Show as a table button
        await Page.ClickAsync(PageConstants.ContentIds["dataOverTimeShowAsTableBtn"]);

        var table = Page.Locator(PageConstants.ContentIds["dataOverTimeTableContainer"]);

        var schoolData = await Page.GetTableRowValuesAsync(PageConstants.ContentIds["dataOverTimeTable"], "School");
        Assert.Equal($"{expectedPrevious2}%", schoolData[0]);
        Assert.Equal($"{expectedPrevious}%", schoolData[1]);
        Assert.Equal($"{expectedCurrent}%", schoolData[2]);

        schoolData = await Page.GetTableRowValuesAsync(PageConstants.ContentIds["exsDataOverTimeTable"], "School");
        Assert.Equal($"{higherPrevious2}%", schoolData[0]);
        Assert.Equal($"{higherPrevious}%", schoolData[1]);
        Assert.Equal($"{higherCurrent}%", schoolData[2]);
    }
}
