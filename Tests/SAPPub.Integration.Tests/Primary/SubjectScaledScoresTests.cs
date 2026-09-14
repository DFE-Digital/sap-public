using Microsoft.Playwright;
using SAPPub.Playwright.Testing;
using SAPPub.Playwright.Testing.KS2.Performance.SubjectScaledScores;
using SubjectScaledScores = SAPPub.Playwright.Testing.KS2.Performance.SubjectScaledScores;

namespace SAPPub.Integration.Tests.Primary;

[Collection("Integration Tests")]
public class SubjectScaledScoresTests() : BasePageTest()
{
    private string BasePageUrl(string urn) => $"/school/{urn}";
    private string pageUnderTest => "primary-performance/subject-scaled-scores";

    [Theory]
    [InlineData("100019", "110", "107", "109", "109", "110", "109")]
    [InlineData("100241", "107", "107", "105", "104", "105", "104")]
    [InlineData("100353", "115", "111", "113", "111", "114", "111")]
    [InlineData("100448", "102", "101", "103", "105", "105", "101")]
    [InlineData("100500", "109", "108", "110", "107", "109", "106")]
    [InlineData("100674", "108", "107", "108", "106", "109", "106")]
    [InlineData("100684", "109", "107", "109", "107", "109", "108")]
    public async Task ShowsExpectedSchoolData(
        string urn,
        string readingPrevious2, string mathsPrevious2,
        string readingPrevious1, string mathsPrevious1,
        string readingCurrent, string mathsCurrent)
    {
        // Arrange && Act
        var response = await Page.GotoAsync(BasePageUrl(urn));
        Assert.NotNull(response);
        var _ = await Page.GotoPage(response.Url, pageUnderTest);

        // Act
        // 'reading' table
        await Page.ClickAsync(SubjectScaledScores.PageConstants.ReadShowDataOverTimeBtnId);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.ClickAsync(SubjectScaledScores.PageConstants.ReadDataOverTimeShowAsTableBtnId);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        // 'maths' table
        await Page.ClickAsync(SubjectScaledScores.PageConstants.MathsShowDataOverTimeBtnId);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.ClickAsync(SubjectScaledScores.PageConstants.MathsDataOverTimeShowAsTableBtnId);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var schoolData = await Page.GetTableRowValuesAsync(SubjectScaledScores.PageConstants.ReadDataOverTimeTableId, "School");
        Assert.Equal(readingPrevious2, schoolData[0]);
        Assert.Equal(readingPrevious1, schoolData[1]);
        Assert.Equal(readingCurrent, schoolData[2]);

        schoolData = await Page.GetTableRowValuesAsync(SubjectScaledScores.PageConstants.MathsDataOverTimeTableId, "School");
        Assert.Equal(mathsPrevious2, schoolData[0]);
        Assert.Equal(mathsPrevious1, schoolData[1]);
        Assert.Equal(mathsCurrent, schoolData[2]);
    }

    [Theory]
    [InlineData("100019", "108", "110", "109", "110", "110", "110", "109", "110", "105", "108")]
    [InlineData("100241", "106", "102", "107", "103", "105", "105", "105", "106", "102", "103")]
    [InlineData("100353", "110", "111", "113", "114", "111", "114", "111", "114", "106", "110")]
    [InlineData("100448", "103", "100", "104", "106", "105", "106", "101", "104", "101", "105")]
    [InlineData("100500", "109", "103", "110", "108", "107", "109", "106", "109", "104", "107")]
    [InlineData("100674", "108", "103", "110", "108", "106", "109", "106", "109", "105", "108")]
    [InlineData("100684", "108", "108", "110", "108", "107", "106", "108", "109", "103", "105")]
    public async Task ShowsExpectedSchoolBreakdownData(
        string urn,
        string mathsBoys, string mathsGirls, string readingBoys, string readingGirls,
        string mathsEAL, string readingEAL,
        string mathsNonMobile, string readingNonMobile,
        string mathsDisadvantaged, string readingDisadvantaged)
    {
        // Arrange && Act
        var response = await Page.GotoAsync(BasePageUrl(urn));
        Assert.NotNull(response);
        var _ = await Page.GotoPage(response.Url, pageUnderTest);

        // Act
        await Page.ExpandAccordionByIdAsync(SubjectScaledScores.PageConstants.ScaledScoresByPupilCharacteristicAccordionId);
        await Page.ExpandDetailsAsync("Compare with non-disadvantaged pupils");

        var schoolData = await Page.GetTableRowValuesAsync(SubjectScaledScores.PageConstants.GirlsBoysTableId, "Girls");
        Assert.Equal(readingGirls, schoolData[0]);
        Assert.Equal(mathsGirls, schoolData[1]);

        schoolData = await Page.GetTableRowValuesAsync(SubjectScaledScores.PageConstants.GirlsBoysTableId, "Boys");
        Assert.Equal(readingBoys, schoolData[0]);
        Assert.Equal(mathsBoys, schoolData[1]);

        schoolData = await Page.GetTableRowValuesAsync(SubjectScaledScores.PageConstants.EalTableId, "Pupils with EAL");
        Assert.Equal(readingEAL, schoolData[0]);
        Assert.Equal(mathsEAL, schoolData[1]);

        schoolData = await Page.GetTableRowValuesAsync(SubjectScaledScores.PageConstants.NonMobileTableId, "Non-mobile pupils");
        Assert.Equal(readingNonMobile, schoolData[0]);
        Assert.Equal(mathsNonMobile, schoolData[1]);

        schoolData = await Page.GetTableRowValuesAsync(SubjectScaledScores.PageConstants.DisadvantagedPupilsTableId, "School");
        Assert.Equal(readingDisadvantaged, schoolData[0]);
        Assert.Equal(mathsDisadvantaged, schoolData[1]);
    }
}
