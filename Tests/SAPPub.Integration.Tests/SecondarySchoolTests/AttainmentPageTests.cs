using Microsoft.Playwright;
using SAPPub.Integration.Tests;
using SAPPub.IntegrationTests.Helpers;

namespace SAPPub.IntegrationTests.SecondarySchoolTests;

[Collection("Integration Tests")]
public class AttainmentPageTests() : BasePageTest()
{
    private string PageUrl(string urn) => $"/school/{urn}";

    [Theory]
    [InlineData("136745", 39.8, 44.1, 46.1)]
    [InlineData("137638", 44.2, 44.1, 46.1)]
    [InlineData("142894", 39.2, 44.1, 46.1)]
    [InlineData("144496", 49.6, 44.1, 46.1)]
    [InlineData("144991", 45.6, 44.1, 46.1)]
    public async Task SecondaryAcademicPerformanceProgressAndAttainment_Current_ShowsExpectedAttainmentData(string urn, double expectedAttainmentSchool, double expectedAttainmentLA, double expectedAttainmentEngland)
    {
        // Arrange && Act
        var _ = await Page.GotoAsync(PageUrl(urn));
        var response = await ClickAcademicPerformanceLinkAsync();

        // Assert
        await AssertSchoolAttainmentData(Page, expectedAttainmentSchool);
        await AssertLAAndEnglandAttainmentData(Page, expectedAttainmentLA, expectedAttainmentEngland);
    }

    [Theory]
    [InlineData("100054", 119, 65.4, 109, "0.62", "0.36", "0.89")]
    [InlineData("142894", 121, 36.1, 96, "-0.99", "-1.27", "-0.71")]
    [InlineData("114308", 136, 48.1, 129, "0.15", "-0.09", "0.4")]
    [InlineData("137228", 142, 44.6, 137, "-0.1", "-0.34", "0.14")]
    [InlineData("143362", 185, 43.4, 176, "-0.13", "-0.34", "0.08")]
    public async Task SecondaryAcademicPerformanceProgressAndAttainment_Previous_ShowsExpectedSchoolData(
        string urn,
        double totalPupils,
        double expectedAttainmentSchool,
        double pupilsInProgressMeasure,
        string expectedProgressSchool,
        string expectedBandingLower,
        string expectedBandingHigher
        )
    {
        // Arrange && Act
        var _ = await Page.GotoAsync($"school/{urn}");
        var response = await ClickAcademicPerformanceLinkAsync();
        _ = await GotoAcademicPerformanceLink(response!.Url, "previous");

        // Assert
        await AssertSchoolProgressData(Page, expectedProgressSchool, expectedBandingLower, expectedBandingHigher, totalPupils, pupilsInProgressMeasure);
        await AssertSchoolAttainmentData(Page, expectedAttainmentSchool);
    }

    [Theory]
    [InlineData("100054", 116, 65.8, 103, "0.77", "0.5", "1.04")]
    [InlineData("142894", 143, 33.7, 141, "-1.2", "-1.44", "-0.97")]
    [InlineData("114308", 152, 51.7, 148, "0.42", "0.19", "0.65")]
    [InlineData("137228", 154, 46.9, 154, "0.14", "-0.08", "0.37")]
    [InlineData("143362", 175, 40, 170, "-0.23", "-0.44", "-0.01")]
    public async Task SecondaryAcademicPerformanceProgressAndAttainment_Previous2_ShowsExpectedSchoolData(
    string urn,
    double totalPupils,
    double expectedAttainmentSchool,
    double pupilsInProgressMeasure,
    string expectedProgressSchool,
    string expectedBandingLower,
    string expectedBandingHigher
    )
    {
        // Arrange && Act
        var _ = await Page.GotoAsync($"school/{urn}");
        var response = await ClickAcademicPerformanceLinkAsync();
        _ = await GotoAcademicPerformanceLink(response!.Url, "previous2");

        // Assert
        await AssertSchoolProgressData(Page, expectedProgressSchool, expectedBandingLower, expectedBandingHigher, totalPupils, pupilsInProgressMeasure);
        await AssertSchoolAttainmentData(Page, expectedAttainmentSchool);
    }

    private Task<IResponse> ClickAcademicPerformanceLinkAsync()
    {
        var response = Page.RunAndWaitForResponseAsync(
            async () =>
            {
                await Page.GetByRole(AriaRole.Link, new() { Name = "Secondary academic performance" }).ClickAsync();
            },
            response => response.Url.Contains("/secondary-performance/progress-attainment/current") && response.Status == 200
        );
        return response;
    }

    private Task<IResponse?> GotoAcademicPerformanceLink(string urlstring, string year = "current")
    {
        const string marker = "school/";
        var i = urlstring.IndexOf(marker);
        var j = urlstring.LastIndexOf('/');
        var previousYearPerformanceUrl = urlstring.Substring(i, j - i);
        return Page.GotoAsync($"{previousYearPerformanceUrl}/{year}");
    }

    private async Task AssertSchoolProgressData(IPage Page, string expectedProgressSchool, string expectedBandingLower, string expectedBandingHigher, double expectedTotalPupils, double expectedPupilsInMeasure)
    {
        var schoolProgress8 = await Page.GetScoreFromParagraphAsync("progress8-establishment-card", "Pupils at this school score");
        Assert.NotNull(schoolProgress8);
        Assert.Equal(expectedProgressSchool, schoolProgress8.Last());

        var progress8Banding = await Page.GetScoreFromParagraphAsync("progress8-establishment-card", "The confidence interval is");
        Assert.NotNull(progress8Banding);
        Assert.Equal(expectedBandingLower, progress8Banding.First());
        Assert.Equal(expectedBandingHigher, progress8Banding.Last());

        await Page.ExpandElement("pupil-details-progress8");
        var pupilsInMeasure = await Page.GetScoreFromParagraphAsync("pupil-details-progress8", "pupils were included");
        Assert.NotNull(pupilsInMeasure);
        Assert.Equal(expectedPupilsInMeasure.ToString("F0"), pupilsInMeasure.First());
        Assert.Equal(expectedTotalPupils.ToString("F0"), pupilsInMeasure.Last());
    }

    private async Task AssertLAProgressData(IPage Page, double expectedProgressLA)
    {
        var laProgress8 = await Page.GetScoreFromParagraphAsync("progress8-localauthority-card", "The local authority average is");
        Assert.NotNull(laProgress8);
        Assert.Equal(expectedProgressLA.ToString("F2"), laProgress8.Last());
    }

    private async Task AssertSchoolAttainmentData(IPage Page, double expectedAttainmentSchool)
    {
        var schoolAttainment8 = await Page.GetScoreFromParagraphAsync("attainment8-establishment-card", "The attainment 8 score for this school is");
        Assert.NotNull(schoolAttainment8);
        Assert.Equal(expectedAttainmentSchool.ToString("F1"), schoolAttainment8.Last());
    }

    private async Task AssertLAAndEnglandAttainmentData(IPage Page, double expectedAttainmentLA, double expectedAttainmentEngland)
    {
        var englandAttainment8 = await Page.GetScoreFromParagraphAsync("attainment8-localauthority-and-national-card", "the national average of");
        Assert.NotNull(englandAttainment8);
        Assert.Equal(expectedAttainmentEngland.ToString("F1"), englandAttainment8.Last());
    }
}
