using Microsoft.Playwright;
using SAPPub.E2E.Tests.Helpers;
using SAPPub.E2E.Tests.Infrastructure;
using SAPPub.E2E.Tests.TestData.Models.KS4;
using SAPPub.Playwright.Testing;

namespace SAPPub.E2E.Tests.SecondarySchoolTests;

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

    private static readonly IDictionary<string, PerformanceTablesTestDataModel> _performanceGirlsCurrentYearTestData = TestDataLoader.Load<PerformanceTablesTestDataModel>(
    "KS4",
    "202425_performance_tables_schools_final_Girls_Current_Year").ToDictionary(x => x.SchoolUrn);

    private static readonly IDictionary<string, PerformanceTablesTestDataModel> _performanceBoysCurrentYearTestData = TestDataLoader.Load<PerformanceTablesTestDataModel>(
        "KS4",
        "202425_performance_tables_schools_final_Boys_Current_Year").ToDictionary(x => x.SchoolUrn);

    private static readonly IDictionary<string, PerformanceTablesTestDataModel> _performanceDisadvantagedCurrentYearTestData = TestDataLoader.Load<PerformanceTablesTestDataModel>(
        "KS4",
        "202425_performance_tables_schools_final_Disadvantaged_Current_Year").ToDictionary(x => x.SchoolUrn);

    private static readonly IDictionary<string, PerformanceTablesTestDataModel> _performanceDisadvantagedPreviousYearTestData = TestDataLoader.Load<PerformanceTablesTestDataModel>(
        "KS4",
        "202425_performance_tables_schools_final_Disadvantaged_Previous_Year").ToDictionary(x => x.SchoolUrn);

    private static readonly IDictionary<string, PerformanceTablesTestDataModel> _performanceDisadvantagedPrevious2YearTestData = TestDataLoader.Load<PerformanceTablesTestDataModel>(
        "KS4",
        "202425_performance_tables_schools_final_Disadvantaged_Previous2_Year").ToDictionary(x => x.SchoolUrn);

    private static readonly IDictionary<string, PerformanceTablesTestDataModel> _performanceEALCurrentYearTestData = TestDataLoader.Load<PerformanceTablesTestDataModel>(
        "KS4",
        "202425_performance_tables_schools_final_EAL_Current_Year").ToDictionary(x => x.SchoolUrn);

    private static readonly IDictionary<string, PerformanceTablesTestDataModel> _performanceNonMobileCurrentYearTestData = TestDataLoader.Load<PerformanceTablesTestDataModel>(
        "KS4",
        "202425_performance_tables_schools_final_NonMobile_Current_Year").ToDictionary(x => x.SchoolUrn);

    private static readonly IDictionary<string, AllStateFundedPupilsLaTestDataModel> _laPerformanceCurrentYearTestData = TestDataLoader.Load<AllStateFundedPupilsLaTestDataModel>(
        "KS4",
        "202425_all_state_funded_pupils_characteristics_la_Total_Current").ToDictionary(x => x.Urn);

    private static readonly IDictionary<string, AllStateFundedPupilsLaTestDataModel> _laPerformancePreviousYearTestData = TestDataLoader.Load<AllStateFundedPupilsLaTestDataModel>(
        "KS4",
        "202425_all_state_funded_pupils_characteristics_la_Total_Previous").ToDictionary(x => x.Urn);

    private static readonly IDictionary<string, AllStateFundedPupilsLaTestDataModel> _laPerformancePrevious2YearTestData = TestDataLoader.Load<AllStateFundedPupilsLaTestDataModel>(
        "KS4",
        "202425_all_state_funded_pupils_characteristics_la_Total_Previous2").ToDictionary(x => x.Urn);

    private static readonly IDictionary<string, AllStateFundedPupilsLaTestDataModel> _laPerformanceGirlsCurrentYearTestData = TestDataLoader.Load<AllStateFundedPupilsLaTestDataModel>(
        "KS4",
        "202425_all_state_funded_pupils_characteristics_la_Girls_Current").ToDictionary(x => x.Urn);

    private static readonly IDictionary<string, AllStateFundedPupilsLaTestDataModel> _laPerformanceBoysCurrentYearTestData = TestDataLoader.Load<AllStateFundedPupilsLaTestDataModel>(
        "KS4",
        "202425_all_state_funded_pupils_characteristics_la_Boys_Current").ToDictionary(x => x.Urn);

    private static readonly IDictionary<string, AllStateFundedPupilsLaTestDataModel> _laPerformanceDisadvantagedCurrentYearTestData = TestDataLoader.Load<AllStateFundedPupilsLaTestDataModel>(
        "KS4",
        "202425_all_state_funded_pupils_characteristics_la_Disadvantaged_Current").ToDictionary(x => x.Urn);
    private static readonly IDictionary<string, AllStateFundedPupilsLaTestDataModel> _laPerformanceDisadvantagedPreviousYearTestData = TestDataLoader.Load<AllStateFundedPupilsLaTestDataModel>(
        "KS4",
        "202425_all_state_funded_pupils_characteristics_la_Disadvantaged_Previous").ToDictionary(x => x.Urn);
    private static readonly IDictionary<string, AllStateFundedPupilsLaTestDataModel> _laPerformanceDisadvantagedPrevious2YearTestData = TestDataLoader.Load<AllStateFundedPupilsLaTestDataModel>(
        "KS4",
        "202425_all_state_funded_pupils_characteristics_la_Disadvantaged_Previous2").ToDictionary(x => x.Urn);

    private static readonly IDictionary<string, AllStateFundedPupilsLaTestDataModel> _laPerformanceNonDisadvantagedCurrentYearTestData = TestDataLoader.Load<AllStateFundedPupilsLaTestDataModel>(
        "KS4",
        "202425_all_state_funded_pupils_characteristics_la_NonDisadvantaged_Current").ToDictionary(x => x.Urn);

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

    public record Performance3YearData
    {
        public required string SchoolUrn { get; init; } = string.Empty;
        public required PerformanceTablesTestDataModel CurrentYearData { get; init; }
        public required PerformanceTablesTestDataModel PreviousYearData { get; init; }
        public required PerformanceTablesTestDataModel Previous2YearData { get; init; }
        public required AllStateFundedPupilsLaTestDataModel LaCurrentYearData { get; init; }
        public required AllStateFundedPupilsLaTestDataModel LaPreviousYearData { get; init; }
        public required AllStateFundedPupilsLaTestDataModel LaPrevious2YearData { get; init; }
    }

    public record AttainmentBreakdownsPageTestDataModel
    {
        public required string SchoolUrn { get; set; } = string.Empty;
        public required PerformanceTablesTestDataModel GirlsCurrentYear { get; set; }
        public required PerformanceTablesTestDataModel BoysCurrentYear { get; set; }
        public required PerformanceTablesTestDataModel DisadvantagedCurrentYear { get; set; }
        public required PerformanceTablesTestDataModel DisadvantagedPreviousYear { get; set; }
        public required PerformanceTablesTestDataModel DisadvantagedPrevious2Year { get; set; }
        public required PerformanceTablesTestDataModel EALCurrentYear { get; set; }
        public required PerformanceTablesTestDataModel NonMobileCurrentYear { get; set; }
        public required AllStateFundedPupilsLaTestDataModel LaDisadvantagedCurrentYear { get; set; }
        public required AllStateFundedPupilsLaTestDataModel LaDisadvantagedPreviousYear { get; set; }
        public required AllStateFundedPupilsLaTestDataModel LaDisadvantagedPrevious2Year { get; set; }
        public required AllStateFundedPupilsLaTestDataModel LaNonDisadvantagedCurrentYear { get; set; }
    }

    public static TheoryData<Performance3YearData> GetPerformance3YearTestData()
    {
        var urns = _performanceTotalsCurrentYearTestData.Keys
            .Intersect(_performanceTotalsPreviousYearTestData.Keys)
            .Intersect(_performanceTotalsPrevious2YearTestData.Keys)
            .Intersect(_laPerformancePreviousYearTestData.Keys)
            .Intersect(_laPerformancePrevious2YearTestData.Keys);
        if(!urns.Any())
        {
            throw new InvalidOperationException("No common urns found across the three years of test data.");
        }

        return new TheoryData<Performance3YearData>(urns.Select(urn => new Performance3YearData()
        {
            SchoolUrn = urn,
            CurrentYearData = _performanceTotalsCurrentYearTestData[urn],
            PreviousYearData = _performanceTotalsPreviousYearTestData[urn],
            Previous2YearData = _performanceTotalsPrevious2YearTestData[urn],
            LaCurrentYearData = _laPerformanceCurrentYearTestData[urn],
            LaPreviousYearData = _laPerformancePreviousYearTestData[urn],
            LaPrevious2YearData = _laPerformancePrevious2YearTestData[urn]
        }));
    }

    [Theory]
    [MemberData(nameof(GetPerformance3YearTestData))]
    public async Task ShowsExpectedSchoolAttainmentData(Performance3YearData testCase)
    {
        // Arrange && Act
        var _ = await Page.GotoAsync(PageUrl(testCase.SchoolUrn));
        var navigationHelper = new VerticalNavigationHelper(Page);
        _ = await navigationHelper.ClickSecondaryAcademicPerformanceAsync();

        await Page.Locator("#attainment8-previous-years-accordion").ClickAsync();

        // Assert
        await AssertSchoolAttainmentData(Page, testCase.CurrentYearData.Attainment8Average, "current");
        await AssertSchoolAttainmentData(Page, testCase.PreviousYearData.Attainment8Average, "prev");
        await AssertSchoolAttainmentData(Page, testCase.Previous2YearData.Attainment8Average, "prev2");
    }

    public static TheoryData<AttainmentBreakdownsPageTestDataModel> GetSchoolAttainmentBreakdownTestData()
    {
        var urns = _performanceGirlsCurrentYearTestData.Keys
            .Intersect(_performanceBoysCurrentYearTestData.Keys)
            .Intersect(_performanceEALCurrentYearTestData.Keys)
            .Intersect(_performanceNonMobileCurrentYearTestData.Keys)
            .Intersect(_performanceDisadvantagedCurrentYearTestData.Keys)
            .Intersect(_performanceDisadvantagedPreviousYearTestData.Keys)
            .Intersect(_performanceDisadvantagedPrevious2YearTestData.Keys)
            .Intersect(_laPerformanceNonDisadvantagedCurrentYearTestData.Keys);
        if (!urns.Any())
        {
            throw new InvalidOperationException("No common urns found across the three years of test data.");
        }

        return new TheoryData<AttainmentBreakdownsPageTestDataModel>(urns.Select(urn => new AttainmentBreakdownsPageTestDataModel()
        {
            SchoolUrn = urn,
            BoysCurrentYear = _performanceBoysCurrentYearTestData[urn],
            GirlsCurrentYear = _performanceGirlsCurrentYearTestData[urn],
            EALCurrentYear = _performanceEALCurrentYearTestData[urn],
            NonMobileCurrentYear = _performanceNonMobileCurrentYearTestData[urn],
            DisadvantagedCurrentYear = _performanceDisadvantagedCurrentYearTestData[urn],
            DisadvantagedPreviousYear = _performanceDisadvantagedPreviousYearTestData[urn],
            DisadvantagedPrevious2Year = _performanceDisadvantagedPrevious2YearTestData[urn],
            LaDisadvantagedCurrentYear = _laPerformanceDisadvantagedCurrentYearTestData[urn],
            LaNonDisadvantagedCurrentYear = _laPerformanceNonDisadvantagedCurrentYearTestData[urn],
            LaDisadvantagedPreviousYear = _laPerformanceDisadvantagedPreviousYearTestData[urn],
            LaDisadvantagedPrevious2Year = _laPerformanceDisadvantagedPrevious2YearTestData[urn]
        }
        ));
    }

    [Theory]
    [MemberData(nameof(GetSchoolAttainmentBreakdownTestData))]
    public async Task ShowsExpectedSchoolAttainmentBreakdownData(AttainmentBreakdownsPageTestDataModel testCase)
    {
        // Arrange && Act
        var _ = await Page.GotoAsync(PageUrl(testCase.SchoolUrn));
        var navigationHelper = new VerticalNavigationHelper(Page);
        _ = await navigationHelper.ClickSecondaryAcademicPerformanceAsync();

        await Page.ExpandAccordionByIdAsync("#other-pupil-characteristics-accordion");
        await Page.ExpandDetailsByIdAsync("#non-disadvantaged-details");
        await Page.ExpandDetailsByIdAsync("#disadvantaged-previous-years-details");

        await AssertSchoolAttainmentBreakdownData(Page, testCase);
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
    [MemberData(nameof(GetPerformance3YearTestData))]
    public async Task ShowsExpectedProgressData(Performance3YearData testData)
    {
        // Arrange && Act
        var _ = await Page.GotoAsync($"school/{testData.SchoolUrn}");
        var navigationHelper = new VerticalNavigationHelper(Page);
        _ = await navigationHelper.ClickSecondaryAcademicPerformanceAsync();

        await Page.ExpandAccordionByIdAsync("prog8-previous-years-accordion");

        // Assert
        // no current year data ATM
        await AssertSchoolProgressData(Page, testData.PreviousYearData, "prev");
        await AssertSchoolProgressData(Page, testData.Previous2YearData, "prev2");
        await AssertLaProgressData(Page, testData.LaPreviousYearData, "prev");
        await AssertLaProgressData(Page, testData.LaPrevious2YearData, "prev2");
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

    private static async Task AssertLaProgressData(IPage Page,
        AllStateFundedPupilsLaTestDataModel yearData,
        string year)
    {
        var laProgress8Selector = $"prog8-scores-{year}-localauthority-card";
        var laProgress8 = await Page.GetScoreFromParagraphAsync(laProgress8Selector, "local authority average");
        AssertHelpers.AssertNumericEqual(yearData.Progress8Average, laProgress8!.First());
    }

    private static async Task AssertSchoolAttainmentBreakdownData(IPage Page, AttainmentBreakdownsPageTestDataModel testCase)
    {
        // Assert
        var girlsScore = await Page.GetTableRowValuesAsync("characteristics-girlboy-table", 0);
        AssertHelpers.AssertNumericEqual(testCase.GirlsCurrentYear.Attainment8Average, girlsScore.First());
        var boysScore = await Page.GetTableRowValuesAsync("characteristics-girlboy-table", 1);
        AssertHelpers.AssertNumericEqual(testCase.BoysCurrentYear.Attainment8Average, boysScore.First());
        var EALScore = await Page.GetTableRowValuesAsync("characteristics-eal-table", 0);
        AssertHelpers.AssertNumericEqual(testCase.EALCurrentYear.Attainment8Average, EALScore.First());
        var nonMobileScore = await Page.GetTableRowValuesAsync("characteristics-nonmobile-table", 0);
        AssertHelpers.AssertNumericEqual(testCase.NonMobileCurrentYear.Attainment8Average, nonMobileScore.First());
        var disadvantagedScore = await Page.GetTableRowValuesAsync("breakdown-disadvantaged-table-0", 0);
        AssertHelpers.AssertNumericEqual(testCase.DisadvantagedCurrentYear.Attainment8Average, disadvantagedScore.First());
        var disadvantagedLaScore = await Page.GetTableRowValuesAsync("breakdown-disadvantaged-table-0", 1);
        AssertHelpers.AssertNumericEqual(testCase.LaDisadvantagedCurrentYear.Attainment8Average, disadvantagedLaScore.First());
        var nonDisadvantagedLaCurrentScore = await Page.GetTableRowValuesAsync("breakdown-non-disadvantaged-table", 0);
        AssertHelpers.AssertNumericEqual(testCase.LaNonDisadvantagedCurrentYear.Attainment8Average, nonDisadvantagedLaCurrentScore.First());
        disadvantagedScore = await Page.GetTableRowValuesAsync("breakdown-disadvantaged-table-1", 0);
        AssertHelpers.AssertNumericEqual(testCase.DisadvantagedPreviousYear.Attainment8Average, disadvantagedScore.First());
        disadvantagedLaScore = await Page.GetTableRowValuesAsync("breakdown-disadvantaged-table-1", 1);
        AssertHelpers.AssertNumericEqual(testCase.LaDisadvantagedPreviousYear.Attainment8Average, disadvantagedLaScore.First());
        disadvantagedScore = await Page.GetTableRowValuesAsync("breakdown-disadvantaged-table-2", 0);
        AssertHelpers.AssertNumericEqual(testCase.DisadvantagedPrevious2Year.Attainment8Average, disadvantagedScore.First());
        disadvantagedLaScore = await Page.GetTableRowValuesAsync("breakdown-disadvantaged-table-2", 1);
        AssertHelpers.AssertNumericEqual(testCase.LaDisadvantagedPrevious2Year.Attainment8Average, disadvantagedLaScore.First());
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
}
