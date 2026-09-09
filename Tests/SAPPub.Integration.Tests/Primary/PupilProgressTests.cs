using Microsoft.Playwright;
using SAPPub.Integration.Tests;
using SAPPub.Integration.Tests.Primary;
using SAPPub.IntegrationTests.Helpers;

namespace SAPPub.Integration.Tests.Primary;

[Collection("Integration Tests")]
public class PupilProgressTests() : BasePageTest()
{
    private string PageUrl(string urn) => $"/school/{urn}";

    [Theory]
    [InlineData("100019", "1.9", "-0.4", "4.3", "4.4", "1.9", "6.8", "3.5", "1.2", "5.9")]
    [InlineData("100241", "-0.1", "-1.7", "1.5", "-0.3", "-2", "1.4", "1.2", "-0.4", "2.8")]
    [InlineData("100353", "4.2", "2.5", "5.8", "6.8", "5.1", "8.5", "3.4", "1.8", "5.1")]
    [InlineData("100448", "-5.5", "-7.7", "-3.4", "-5.1", "-7.3", "-2.8", "-2.9", "-5", "-0.7")]
    [InlineData("100500", "4", "1.5", "6.5", "4.1", "1.4", "6.8", "2", "-0.6", "4.5")]
    [InlineData("100674", "0.8", "-0.9", "2.5", "0.9", "-0.9", "2.7", "0.3", "-1.4", "2.1")]
    [InlineData("100684", "1.8", "0.5", "3.1", "2.7", "1.4", "4.1", "1.4", "0.1", "2.8")]
    public async Task SecondaryAcademicPerformanceProgressAndAttainment_Previous2_ShowsExpectedAttainmentData(
        string urn, 
        string expectedMathsScore, string expectedMathsLowerBand, string expectedMathsHigherBand,
        string expectedReadingScore, string expectedReadingLowerBand, string expectedReadingHigherBand,
        string expectedWritingScore, string expectedWritingLowerBand, string expectedWritingHigherBand)
    {
        // Arrange && Act
        var _ = await Page.GotoAsync(PageUrl(urn));
        var response = await Page.ClickAcademicPerformanceLinkAsync();
        _ = await Page.GotoAcademicPerformanceSelectedYearLink(response!.Url, "previous2");

        // Assert
        await AssertSchoolProgressData(Page, "maths-establishment-card", expectedMathsScore, expectedMathsLowerBand, expectedMathsHigherBand);
        await AssertSchoolProgressData(Page, "reading-establishment-card", expectedReadingScore, expectedReadingLowerBand, expectedReadingHigherBand);
        await AssertSchoolProgressData(Page, "writing-establishment-card", expectedWritingScore, expectedWritingLowerBand, expectedWritingHigherBand);
    }

    private async Task AssertSchoolProgressData(IPage Page, string cardId, string expectedSchoolScore, string expectedLowerBanding, string expectedUpperBanding)
    {
        var schoolScore = await Page.GetScoreFromParagraphAsync(cardId, "Pupils at this school score");
        Assert.NotNull(schoolScore);
        Assert.Equal(expectedSchoolScore, schoolScore.First());

        var banding = await Page.GetScoreFromParagraphAsync(cardId, "The confidence interval is");
        Assert.NotNull(banding);
        Assert.Equal(expectedLowerBanding, banding.First());
        Assert.Equal(expectedUpperBanding, banding.Last());
    }
}
