using Microsoft.Playwright;
using SAPPub.Playwright.Testing;
using SAPPub.Playwright.Testing.KS2.Performance.MeetingOrExceedingStandards;

namespace SAPPub.Integration.Tests.Primary;

[Collection("Integration Tests")]
public class MeetingOrExceedingStandardsTests() : BasePageTest()
{
    private string BasePageUrl(string urn) => $"/school/{urn}";
    private string pageUnderTest => "primary-performance/meeting-or-exceeding-standards";

    // TODO :
    // England average for the tables
    [Theory]
    [InlineData("100019", "83", "15", "75", "20", "73", "25")]
    [InlineData("100241", "72", "16", "63", "19", "77", "14")]
    [InlineData("100353", "91", "44", "87", "27", "93", "46")]
    [InlineData("100448", "57", "4", "68", "11", "50", "0")]
    [InlineData("100500", "90", "14", "70", "3", "83", "17")]
    [InlineData("100674", "69", "9", "71", "9", "76", "8")]
    [InlineData("100684", "85", "24", "83", "15", "84", "18")]
    public async Task TableShowsYearData(
        string urn, 
        string expectedCurrent, string higherCurrent, 
        string expectedPrevious, string higherPrevious, 
        string expectedPrevious2, string higherPrevious2)
    {
        // Arrange && Act
        var response = await Page.GotoAsync(BasePageUrl(urn));
        Assert.NotNull(response);
        var _ = await Page.GotoPage(response.Url, pageUnderTest);

        // Act
        // Click Show data over time button
        await Page.ClickAsync(PageConstants.ShowDataOverTimeBtnId);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // and click Show as a table button
        await Page.ClickAsync(PageConstants.DataOverTimeShowAsTableBtnId);

        var schoolData = await Page.GetTableRowValuesAsync(PageConstants.DataOverTimeTableId, "School");
        Assert.Equal($"{expectedPrevious2}%", schoolData[0]);
        Assert.Equal($"{expectedPrevious}%", schoolData[1]);
        Assert.Equal($"{expectedCurrent}%", schoolData[2]);

        schoolData = await Page.GetTableRowValuesAsync(PageConstants.ExsDataOverTimeTableId, "School");
        Assert.Equal($"{higherPrevious2}%", schoolData[0]);
        Assert.Equal($"{higherPrevious}%", schoolData[1]);
        Assert.Equal($"{higherCurrent}%", schoolData[2]);
    }

    [Theory]
    [InlineData("100019", "70", "14", "72", "13", "73", "15")]
    [InlineData("100241", "70", "13", "74", "15", "76", "17")]
    [InlineData("100353", "74", "17", "75", "17", "77", "20")]
    [InlineData("100448", "65", "11", "67", "12", "67", "13")]
    [InlineData("100500", "74", "16", "72", "15", "75", "19")]
    [InlineData("100674", "61", "6", "63", "9", "68", "10")]
    [InlineData("100684", "61", "6", "63", "9", "68", "10")]
    public async Task TableShowsLaYearData(
        string urn, 
        string expectedPrevious2, string higherPrevious2,
        string expectedPrevious, string higherPrevious,
        string expectedCurrent, string higherCurrent)
    {
        // Arrange && Act
        var response = await Page.GotoAsync(BasePageUrl(urn));
        Assert.NotNull(response);
        var _ = await Page.GotoPage(response.Url, pageUnderTest);

        // Act
        // Click Show data over time button
        await Page.ClickAsync(PageConstants.ShowDataOverTimeBtnId);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // and click Show as a table button
        await Page.ClickAsync(PageConstants.DataOverTimeShowAsTableBtnId);

        var laData = await Page.GetTableRowValuesAsync(PageConstants.DataOverTimeTableId, 1);
        Assert.Equal($"{expectedPrevious2}%", laData[0]);
        Assert.Equal($"{expectedPrevious}%", laData[1]);
        Assert.Equal($"{expectedCurrent}%", laData[2]);

        laData = await Page.GetTableRowValuesAsync(PageConstants.ExsDataOverTimeTableId, 1);
        Assert.Equal($"{higherPrevious2}%", laData[0]);
        Assert.Equal($"{higherPrevious}%", laData[1]);
        Assert.Equal($"{higherCurrent}%", laData[2]);
    }

    // TODO :
    // England disadvantaged
    [Theory]
    [InlineData("100019", "78", "86", "6", "21", "89", "11", "80", "15", "80", "0")]
    [InlineData("100241", "80", "63", "23", "7", "79", "14", "77", "17", "58", "8")]
    [InlineData("100353", "83", "97", "46", "42", "95", "43", "91", "44", "67", "11")]
    [InlineData("100448", "58", "56", "0", "6", "83", "0", "56", "4", "50", "5")]
    [InlineData("100500", "100", "80", "21", "7", "100", "13", "89", "15", "77", "8")]
    [InlineData("100674", "85", "54", "11", "7", "60", "0", "71", "8", "56", "6")]
    [InlineData("100684", "82", "87", "21", "26", "88", "8", "85", "25", "67", "17")]
    public async Task TableShowsBreakdownsData(
        string urn, 
        string expectedBoys, string expectedGirls, string higherBoys, string higherGirls, 
        string expectedEAL, string higherEAL, string expectedNonMobile, string higherNonMobile,
        string expectedDisadvantaged, string higherDisadvantaged)
    {
        // each breakdown table has a 'All pupils at the school' row that replicates the data in the 
        // first 2 tables, so that check is not replicated here
        // Arrange && Act
        var response = await Page.GotoAsync(BasePageUrl(urn));
        Assert.NotNull(response);
        var _ = await Page.GotoPage(response.Url, pageUnderTest);

        // Act
        await Page.ExpandAccordionByIdAsync(PageConstants.MeetingOrExceedingStandardsByPupilCharacteristicAccordionId);
        await Page.ExpandDetailsAsync("Compare with non-disadvantaged pupils");

        var schoolData = await Page.GetTableRowValuesAsync(PageConstants.GirlsBoysTableId, "Girls");
        Assert.Equal($"{expectedGirls}%", schoolData[0]);
        Assert.Equal($"{higherGirls}%", schoolData[1]);

        schoolData = await Page.GetTableRowValuesAsync(PageConstants.GirlsBoysTableId, "Boys");
        Assert.Equal($"{expectedBoys}%", schoolData[0]);
        Assert.Equal($"{higherBoys}%", schoolData[1]);

        schoolData = await Page.GetTableRowValuesAsync(PageConstants.EalTableId, "Pupils with EAL");
        Assert.Equal($"{expectedEAL}%", schoolData[0]);
        Assert.Equal($"{higherEAL}%", schoolData[1]);

        schoolData = await Page.GetTableRowValuesAsync(PageConstants.NonMobileTableId, "Non-mobile pupils");
        Assert.Equal($"{expectedNonMobile}%", schoolData[0]);
        Assert.Equal($"{higherNonMobile}%", schoolData[1]);

        schoolData = await Page.GetTableRowValuesAsync(PageConstants.DisadvantagedPupilsTableId, "School");
        Assert.Equal($"{expectedDisadvantaged}%", schoolData[0]);
        Assert.Equal($"{higherDisadvantaged}%", schoolData[1]);
    }

    [Theory]
    [InlineData("100019", "82", "25")]
    [InlineData("100241", "83", "23")]
    [InlineData("100353", "85", "27")]
    [InlineData("100448", "79", "23")]
    [InlineData("100500", "83", "26")]
    [InlineData("100674", "77", "14")]
    [InlineData("100684", "77", "14")]
    public async Task NonDisadvantagedLa_Expected(string urn, string expected, string higher)
    {
        // Arrange && Act
        var response = await Page.GotoAsync(BasePageUrl(urn));
        Assert.NotNull(response);
        var _ = await Page.GotoPage(response.Url, pageUnderTest);

        // Act
        await Page.ExpandAccordionByIdAsync(PageConstants.MeetingOrExceedingStandardsByPupilCharacteristicAccordionId);
        await Page.ExpandDetailsAsync("Compare with non-disadvantaged pupils");

        var laData = await Page.GetTableRowValuesAsync(PageConstants.NonDisadvantagedPupilsTableId, 0);
        Assert.Equal($"{expected}%", laData[0]);
        Assert.Equal($"{higher}%", laData[1]);
    }
}
