using Microsoft.Playwright;
using SAPPub.Integration.Tests.Helpers;
using SAPPub.Integration.Tests.TestData.Models.KS4;
using SAPPub.Playwright.Testing;

namespace SAPPub.Integration.Tests.SecondarySchoolTests;

public class ProgressAndAttainmentPageTests : BasePageTest
{
    private string PageUrl(string urn) => $"/school/{urn}";

    private static readonly IDictionary<string, PerformanceTablesTestDataModel> _performanceTotalsCurrentYearTestData = TestDataLoader.Load<PerformanceTablesTestDataModel>(
        "KS4",
        "202425_performance_tables_schools_final_Total_Current_Year").ToDictionary(x => x.SchoolUrn);

    private static readonly IDictionary<string, PerformanceTablesTestDataModel> _performanceTotalsPreviousYearTestData = TestDataLoader.Load<PerformanceTablesTestDataModel>(
        "KS4",
        "202425_performance_tables_schools_final_Total_Previous_Year").ToDictionary(x => x.SchoolUrn);

    private static readonly IDictionary<string, PerformanceTablesTestDataModel> _performanceTotalsPrevious2YearTestData = TestDataLoader.Load<PerformanceTablesTestDataModel>(
        "KS4",
        "202425_performance_tables_schools_final_Total_Previous2_Year").ToDictionary(x => x.SchoolUrn);

    private static readonly IDictionary<string, AttainmentTestCase> _performanceLaAndEnglandAttainmentTestData = TestDataLoader.Load<AttainmentTestCase>(
        "KS4",
        "LaAndEnglandAttainment").ToDictionary(x => x.Urn);

    public record AttainmentTestCase(
        string Urn,
        double ExpectedAttainmentSchool,
        double ExpectedAttainmentLA,
        double ExpectedAttainmentEngland);

    public record SchoolPerformance3YearData(
        string urn,
        PerformanceTablesTestDataModel CurrentYearData,
        PerformanceTablesTestDataModel PreviousYearData,
        PerformanceTablesTestDataModel Previous2YearData
    );

    public static TheoryData<SchoolPerformance3YearData> GetSchoolPerformance3YearTestData()
    {
        var urns = _performanceTotalsCurrentYearTestData.Keys
            .Intersect(_performanceTotalsPreviousYearTestData.Keys)
            .Intersect(_performanceTotalsPrevious2YearTestData.Keys);
        if(!urns.Any())
        {
            throw new InvalidOperationException("No common urns found across the three years of test data.");
        }

        return new TheoryData<SchoolPerformance3YearData>(urns.Select(urn => new SchoolPerformance3YearData(
            urn,
            _performanceTotalsCurrentYearTestData[urn],
            _performanceTotalsPreviousYearTestData[urn],
            _performanceTotalsPrevious2YearTestData[urn]
        )).ToArray());
    }

    [Theory]
    [MemberData(nameof(GetSchoolPerformance3YearTestData))]
    public async Task ShowsExpectedSchoolAttainmentData(
        SchoolPerformance3YearData testCase)
    {
        // Arrange && Act
        var _ = await Page.GotoAsync(PageUrl(testCase.urn));
        var navigationHelper = new VerticalNavigationHelper(Page);
        _ = await navigationHelper.ClickSecondaryAcademicPerformanceAsync();

        await Page.Locator("#attainment8-previous-years-accordion").ClickAsync();

        // Assert
        await AssertSchoolAttainmentData(Page, testCase.CurrentYearData.Attainment8Average, "current");
        await AssertSchoolAttainmentData(Page, testCase.PreviousYearData.Attainment8Average, "prev");
        await AssertSchoolAttainmentData(Page, testCase.Previous2YearData.Attainment8Average, "prev2");
    }

    public static TheoryData<AttainmentTestCase> GetLaAndEnglandAttainmentTestData()
    {
        return new TheoryData<AttainmentTestCase>(_performanceLaAndEnglandAttainmentTestData.Values.ToArray());
    }

    [Theory]
    [MemberData(nameof(GetLaAndEnglandAttainmentTestData))]
    public async Task CurrentYearSelected_ShowsExpectedLaAndEnglandAttainmentData(AttainmentTestCase testCase)
    {
        // Arrange && Act
        var _ = await Page.GotoAsync(PageUrl(testCase.Urn));
        var navigationHelper = new VerticalNavigationHelper(Page);
        _ = await navigationHelper.ClickSecondaryAcademicPerformanceAsync();

        await Page.Locator("#prog8-previous-years-accordion").ClickAsync();
        await Page.Locator("#attainment8-previous-years-accordion").ClickAsync();

        // Assert
        await AssertLAAndEnglandAttainmentData(Page, testCase.ExpectedAttainmentLA, testCase.ExpectedAttainmentEngland, "current");
        await AssertLAAndEnglandAttainmentData(Page, testCase.ExpectedAttainmentLA, testCase.ExpectedAttainmentEngland, "current");
    }

    [Theory]
    [MemberData(nameof(GetSchoolPerformance3YearTestData))]
    public async Task ShowsExpectedProgressSchoolData(SchoolPerformance3YearData testData)
    {
        // Arrange && Act
        var _ = await Page.GotoAsync($"school/{testData.urn}");
        var navigationHelper = new VerticalNavigationHelper(Page);
        _ = await navigationHelper.ClickSecondaryAcademicPerformanceAsync();

        await Page.ExpandAccordionByIdAsync("prog8-previous-years-accordion");

        // Assert
        // no current year data ATM
        await AssertSchoolProgressData(Page, testData.PreviousYearData, "prev");
        await AssertSchoolProgressData(Page, testData.Previous2YearData, "prev2");
    }

    private static async Task AssertSchoolProgressData(
        IPage Page,
        PerformanceTablesTestDataModel yearData,
        string year)
    {
        var pupilDetailsProgress8Selector = $"pupil-details-prog8-scores-{year}";
        var prog8ScoreSelector = $"prog8-scores-{year}";

        var schoolProgress8 = await Page.GetScoreFromParagraphAsync(prog8ScoreSelector, "Pupils at this school score");
        Assert.NotNull(schoolProgress8);
        Assert.Equal(yearData.Progress8Average, schoolProgress8.Last());

        var progress8Banding = await Page.GetScoreFromParagraphAsync(prog8ScoreSelector, "The confidence interval is");
        Assert.NotNull(progress8Banding);
        AssertHelpers.AssertNumericEqual(yearData.Progress8Lower95Ci, progress8Banding.First());
        AssertHelpers.AssertNumericEqual(yearData.Progress8Upper95Ci, progress8Banding.Last());

        await Page.ExpandElement(pupilDetailsProgress8Selector);
        var pupilsInMeasure = await Page.GetScoreFromParagraphAsync(pupilDetailsProgress8Selector, "pupils were included");
        Assert.NotNull(pupilsInMeasure);
        AssertHelpers.AssertNumericEqual(yearData.Progress8PupilCount, pupilsInMeasure.First());
        AssertHelpers.AssertNumericEqual(yearData.PupilCount, pupilsInMeasure.Last());
    }

    private async Task AssertSchoolAttainmentData(IPage Page, string expectedAttainmentSchool, string year)
    {
        var schoolAttainment8 = await Page.GetScoreFromParagraphAsync($"attainment8-scores-{year}", "The Attainment 8 score for this school is");
        Assert.NotNull(schoolAttainment8);
        AssertHelpers.AssertNumericEqual(expectedAttainmentSchool, schoolAttainment8.Last());
    }

    private async Task AssertLAAndEnglandAttainmentData(IPage Page, double expectedAttainmentLA, double expectedAttainmentEngland, string year)
    {
        var englandAttainment8 = await Page.GetScoreFromParagraphAsync($"attainment8-scores-{year}-localauthority-and-national-card", "the national average of");
        Assert.NotNull(englandAttainment8);
        Assert.Equal(expectedAttainmentEngland.ToString("F1"), englandAttainment8.Last());
    }
}
