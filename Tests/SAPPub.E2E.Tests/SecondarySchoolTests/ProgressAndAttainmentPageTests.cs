using Microsoft.Playwright;
using SAPPub.Integration.Tests.Helpers;
using SAPPub.Integration.Tests.TestData.Models.KS4;
using SAPPub.Playwright.Testing;
using System.Text.Json;

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

    private static readonly IDictionary<string, AllStateFundedPupilsLaTestDataModel> _laPerformanceCurrentYearTestData = TestDataLoader.Load<AllStateFundedPupilsLaTestDataModel>(
        "KS4",
        "202425_all_state_funded_pupils_characteristics_la_Total_Current").ToDictionary(x => x.Urn);

    private static readonly IDictionary<string, AllStateFundedPupilsLaTestDataModel> _laPerformancePreviousYearTestData = TestDataLoader.Load<AllStateFundedPupilsLaTestDataModel>(
        "KS4",
        "202425_all_state_funded_pupils_characteristics_la_Total_Previous").ToDictionary(x => x.Urn);

    private static readonly IDictionary<string, AllStateFundedPupilsLaTestDataModel> _laPerformancePrevious2YearTestData = TestDataLoader.Load<AllStateFundedPupilsLaTestDataModel>(
        "KS4",
        "202425_all_state_funded_pupils_characteristics_la_Total_Previous2").ToDictionary(x => x.Urn);

    private static readonly List<AllStateFundedPupilsTestDataModel> _englandPerformanceCurrentYearTestData = TestDataLoader.Load<AllStateFundedPupilsTestDataModel>(
        "KS4",
        "202425_all_state_funded_pupils_characteristics_England_Total_Current");

    private static readonly List<AllStateFundedPupilsTestDataModel> _englandPerformancePreviousYearTestData = TestDataLoader.Load<AllStateFundedPupilsTestDataModel>(
        "KS4",
        "202425_all_state_funded_pupils_characteristics_England_Total_Previous");

    private static readonly List<AllStateFundedPupilsTestDataModel> _englandPerformancePrevious2YearTestData = TestDataLoader.Load<AllStateFundedPupilsTestDataModel>(
        "KS4",
        "202425_all_state_funded_pupils_characteristics_England_Total_Previous2");

    public record AttainmentTestCase(
        string Urn,
        string ExpectedAttainmentCurrent,
        string ExpectedAttainmentPrevious,
        string ExpectedAttainmentPrevious2);

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

    public static TheoryData<AttainmentTestCase> GetLaAttainmentTestData()
    {
        var urns = _laPerformanceCurrentYearTestData.Keys
            .Intersect(_laPerformancePreviousYearTestData.Keys)
            .Intersect(_laPerformancePrevious2YearTestData.Keys);

        if (!urns.Any())
        {
            throw new InvalidOperationException("No common urns found across the three years of test data.");
        }

        var testData = urns.Select(urn => new AttainmentTestCase(Urn: urn,
            ExpectedAttainmentCurrent: _laPerformanceCurrentYearTestData[urn].Attainment8Average,
            ExpectedAttainmentPrevious: _laPerformancePreviousYearTestData[urn].Attainment8Average,
            ExpectedAttainmentPrevious2: _laPerformancePrevious2YearTestData[urn].Attainment8Average));

        return new TheoryData<AttainmentTestCase>(testData);
    }

    [Theory]
    [MemberData(nameof(GetLaAttainmentTestData))]
    public async Task ShowsExpectedLaAttainmentData(AttainmentTestCase testCase)
    {
        // Arrange && Act
        var _ = await Page.GotoAsync(PageUrl(testCase.Urn));
        var navigationHelper = new VerticalNavigationHelper(Page);
        _ = await navigationHelper.ClickSecondaryAcademicPerformanceAsync();

        await Page.Locator("#attainment8-previous-years-accordion").ClickAsync();

        // Assert
        await AssertLaAttainmentData(Page, testCase.ExpectedAttainmentCurrent, "current");
        await AssertLaAttainmentData(Page, testCase.ExpectedAttainmentPrevious, "prev");
        await AssertLaAttainmentData(Page, testCase.ExpectedAttainmentPrevious2, "prev2");
    }

    public static TheoryData<AttainmentTestCase> GetEnglandAttainmentTestData()
    {
        AttainmentTestCase testData = new(
            Urn: _performanceTotalsCurrentYearTestData.First().Value.SchoolUrn,
            ExpectedAttainmentCurrent: _englandPerformanceCurrentYearTestData.First().Attainment8Average,
            ExpectedAttainmentPrevious: _englandPerformancePreviousYearTestData.First().Attainment8Average,
            ExpectedAttainmentPrevious2: _englandPerformancePrevious2YearTestData.First().Attainment8Average);

        return new TheoryData<AttainmentTestCase>(testData);
    }

    [Theory]
    [MemberData(nameof(GetEnglandAttainmentTestData))]
    public async Task ShowsExpectedEnglandAttainmentData(AttainmentTestCase testCase)
    {
        // Arrange && Act
        var _ = await Page.GotoAsync(PageUrl(testCase.Urn));
        var navigationHelper = new VerticalNavigationHelper(Page);
        _ = await navigationHelper.ClickSecondaryAcademicPerformanceAsync();

        await Page.Locator("#attainment8-previous-years-accordion").ClickAsync();

        // Assert
        await AssertEnglandAttainmentData(Page, testCase.ExpectedAttainmentCurrent, "current");
        await AssertEnglandAttainmentData(Page, testCase.ExpectedAttainmentPrevious, "prev");
        await AssertEnglandAttainmentData(Page, testCase.ExpectedAttainmentPrevious2, "prev2");
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

    private async Task AssertLaAttainmentData(IPage Page, string expectedAttainmentLocalAuthority, string year)
    {
        //"attainment8-scores-prev"
        var localAuthorityAttainment8 = await Page.GetScoreFromParagraphAsync($"attainment8-scores-{year}-localauthority-and-national-card", "Attainment 8 score");
        Assert.NotNull(localAuthorityAttainment8);
        AssertHelpers.AssertNumericEqual(expectedAttainmentLocalAuthority, localAuthorityAttainment8.Last());
    }

    private async Task AssertEnglandAttainmentData(IPage Page, string expectedAttainmentEngland, string year)
    {
        var englandAttainment8 = await Page.GetScoreFromParagraphAsync($"attainment8-scores-{year}-localauthority-and-national-card", "the national average of");
        Assert.NotNull(englandAttainment8);
        AssertHelpers.AssertNumericEqual(expectedAttainmentEngland, englandAttainment8.ToList()[1]);
    }
    // "prog8-scores-prev2-localauthority-card"
}
