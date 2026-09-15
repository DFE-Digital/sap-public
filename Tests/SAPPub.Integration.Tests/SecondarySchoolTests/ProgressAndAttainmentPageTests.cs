using Microsoft.Playwright;
using SAPPub.Integration.Tests.Helpers;
using SAPPub.Playwright.Testing;

namespace SAPPub.Integration.Tests.SecondarySchoolTests;

[Collection("Integration Tests")]
public class ProgressAndAttainmentPageTests : BasePageTest
{
    private string PageUrl(string urn) => $"/school/{urn}";

    public record AttainmentTestCase(
        string Urn,
        double ExpectedAttainmentSchool,
        double ExpectedAttainmentLA,
        double ExpectedAttainmentEngland);


    public static TheoryData<AttainmentTestCase> GetAttainmentTestData()
    {
        var testdata =  TestDataLoader.GetTestData("KS4", "EstablishmentAttainment");
        var theoryData = new TheoryData<AttainmentTestCase>();
        if (testdata != null)
        {
            foreach (var row in testdata)
            {
                theoryData.Add(new AttainmentTestCase(
                    row[0].GetString() ?? string.Empty,
                    row[1].GetDouble(),
                    row[2].GetDouble(),
                    row[3].GetDouble()
                ));
            }
        }
        return theoryData;
    }

    [Theory]
    [MemberData(nameof(GetAttainmentTestData))]
    public async Task CurrentYearSelected_ShowsExpectedAttainmentData_Memberdata(
        AttainmentTestCase testCase)
    {
        // Arrange && Act
        var _ = await Page.GotoAsync(PageUrl(testCase.Urn));
        var navigationHelper = new VerticalNavigationHelper(Page);
        _ = await navigationHelper.ClickSecondaryAcademicPerformanceAsync();

        await Page.Locator("#prog8-previous-years-accordion").ClickAsync();
        await Page.Locator("#attainment8-previous-years-accordion").ClickAsync();

        // Assert
        await AssertSchoolAttainmentData(Page, testCase.ExpectedAttainmentSchool, "current");
        await AssertLAAndEnglandAttainmentData(Page, testCase.ExpectedAttainmentLA, testCase.ExpectedAttainmentEngland, "current");
        await AssertSchoolAttainmentData(Page, testCase.ExpectedAttainmentSchool, "current");
        await AssertLAAndEnglandAttainmentData(Page, testCase.ExpectedAttainmentLA, testCase.ExpectedAttainmentEngland, "current");
    }

    public record ProgressAndAttainmentTestCase(
        string urn,
        double totalPupils,
        double expectedAttainmentSchool,
        double pupilsInProgressMeasure,
        string expectedProgressSchool,
        string expectedBandingLower,
        string expectedBandingHigher);

    public static TheoryData<ProgressAndAttainmentTestCase> GetProgressAndAttainmentTestData(string filename)
    {
        var testdata = TestDataLoader.GetTestData("KS4", filename);
        var theoryData = new TheoryData<ProgressAndAttainmentTestCase>();
        if (testdata != null)
        {
            foreach (var row in testdata)
            {
                theoryData.Add(new ProgressAndAttainmentTestCase(
                    row[0].GetString() ?? string.Empty,
                    row[1].GetDouble(),
                    row[2].GetDouble(),
                    row[3].GetDouble(),
                    row[4].GetString() ?? string.Empty,
                    row[5].GetString() ?? string.Empty,
                    row[6].GetString() ?? string.Empty
                ));
            }
        }
        return theoryData;
    }

    [Theory]
    [MemberData(nameof(GetProgressAndAttainmentTestData), "EstablishmentPreviousYearProgressAndAttainment")]
    public async Task PreviousYearSelected_ShowsExpectedAttainmentAndProgressSchoolData(ProgressAndAttainmentTestCase testData)
    {
        // Arrange && Act
        var _ = await Page.GotoAsync($"school/{testData.urn}");
        var navigationHelper = new VerticalNavigationHelper(Page);
        _ = await navigationHelper.ClickSecondaryAcademicPerformanceAsync();

        await Page.ExpandAccordionByIdAsync("prog8-previous-years-accordion");
        await Page.ExpandAccordionByIdAsync("attainment8-previous-years-accordion");

        // Assert
        await AssertSchoolProgressData(Page, testData.expectedProgressSchool, testData.expectedBandingLower, testData.expectedBandingHigher, testData.totalPupils, testData.pupilsInProgressMeasure, "prev");
        await AssertSchoolAttainmentData(Page, testData.expectedAttainmentSchool, "prev");
    }

    [Theory]
    [MemberData(nameof(GetProgressAndAttainmentTestData), "EstablishmentPrevious2YearProgressAndAttainment")]
    public async Task Previous2YearSelected_ShowsExpectedAttainmentAndProgressSchoolData(ProgressAndAttainmentTestCase testData)
    {
        // Arrange && Act
        var _ = await Page.GotoAsync($"school/{testData.urn}");
        var navItem = new VerticalNavigationHelper(Page);
        _ = await navItem.ClickSecondaryAcademicPerformanceAsync();
        
        await Page.ExpandAccordionByIdAsync("prog8-previous-years-accordion");
        await Page.ExpandAccordionByIdAsync("attainment8-previous-years-accordion");

        // Assert
        await AssertSchoolProgressData(Page, testData.expectedProgressSchool, testData.expectedBandingLower, testData.expectedBandingHigher, testData.totalPupils, testData.pupilsInProgressMeasure, "prev2");
        await AssertSchoolAttainmentData(Page, testData.expectedAttainmentSchool, "prev2");
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
