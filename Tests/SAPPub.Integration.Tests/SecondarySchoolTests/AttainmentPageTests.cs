using Microsoft.Playwright;
using SAPPub.Integration.Tests.Helpers;
using SAPPub.Playwright.Testing;

namespace SAPPub.Integration.Tests.SecondarySchoolTests;

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
        var navigationHelper = new VerticalNavigationHelper(Page);
        var response = await navigationHelper.ClickSecondaryAcademicPerformanceAsync();

        // Assert
        await AssertSchoolAttainmentData(Page, expectedAttainmentSchool, "current");
        await AssertLAAndEnglandAttainmentData(Page, expectedAttainmentLA, expectedAttainmentEngland, "current");
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
        var content = await Page.ContentAsync();
        var navigationHelper = new VerticalNavigationHelper(Page);
        var response = await navigationHelper.ClickSecondaryAcademicPerformanceAsync();

        await Page.Locator("#prog8-previous-years-accordion").ClickAsync();
        await Page.Locator("#attainment8-previous-years-accordion").ClickAsync();

        // Assert
        await AssertSchoolProgressData(Page, expectedProgressSchool, expectedBandingLower, expectedBandingHigher, totalPupils, pupilsInProgressMeasure, "prev");
        await AssertSchoolAttainmentData(Page, expectedAttainmentSchool, "prev");
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
        var navItem = new VerticalNavigationHelper(Page);

        var response = await navItem.ClickSecondaryAcademicPerformanceAsync();
        await Page.Locator("#prog8-previous-years-accordion").ClickAsync();
        await Page.Locator("#attainment8-previous-years-accordion").ClickAsync();

        // Assert
        await AssertSchoolProgressData(Page, expectedProgressSchool, expectedBandingLower, expectedBandingHigher, totalPupils, pupilsInProgressMeasure, "prev2");
        await AssertSchoolAttainmentData(Page, expectedAttainmentSchool, "prev2");
    }

    private static async Task AssertSchoolProgressData(
        IPage Page, 
        string expectedProgressSchool, 
        string expectedBandingLower, 
        string expectedBandingHigher, 
        double expectedTotalPupils, 
        double expectedPupilsInMeasure,
        string year)
    {
        var pupilDetailsProgress8Selector = $"pupil-details-prog8-scores-{year}";
        var prog8ScoreSelector = $"prog8-scores-{year}";

        var schoolProgress8 = await Page.GetScoreFromParagraphAsync(prog8ScoreSelector, "Pupils at this school score");
        Assert.NotNull(schoolProgress8);
        Assert.Equal(expectedProgressSchool, schoolProgress8.Last());

        var progress8Banding = await Page.GetScoreFromParagraphAsync(prog8ScoreSelector, "The confidence interval is");
        Assert.NotNull(progress8Banding);
        Assert.Equal(expectedBandingLower, progress8Banding.First());
        Assert.Equal(expectedBandingHigher, progress8Banding.Last());

        await Page.ExpandElement(pupilDetailsProgress8Selector);
        var pupilsInMeasure = await Page.GetScoreFromParagraphAsync(pupilDetailsProgress8Selector, "pupils were included");
        Assert.NotNull(pupilsInMeasure);
        Assert.Equal(expectedPupilsInMeasure.ToString("F0"), pupilsInMeasure.First());
        Assert.Equal(expectedTotalPupils.ToString("F0"), pupilsInMeasure.Last());
    }

    private async Task AssertSchoolAttainmentData(IPage Page, double expectedAttainmentSchool, string year)
    {
        var schoolAttainment8 = await Page.GetScoreFromParagraphAsync($"attainment8-scores-{year}", "The Attainment 8 score for this school is");
        Assert.NotNull(schoolAttainment8);
        Assert.Equal(expectedAttainmentSchool.ToString("F1"), schoolAttainment8.Last());
    }

    private async Task AssertLAAndEnglandAttainmentData(IPage Page, double expectedAttainmentLA, double expectedAttainmentEngland, string year)
    {
        var englandAttainment8 = await Page.GetScoreFromParagraphAsync($"attainment8-scores-{year}-localauthority-and-national-card", "the national average of");
        Assert.NotNull(englandAttainment8);
        Assert.Equal(expectedAttainmentEngland.ToString("F1"), englandAttainment8.Last());
    }
}
