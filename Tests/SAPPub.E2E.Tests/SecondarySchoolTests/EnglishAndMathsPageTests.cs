using Microsoft.Playwright;
using SAPPub.E2E.Tests.Helpers;
using SAPPub.E2E.Tests.Infrastructure;
using SAPPub.E2E.Tests.Primary;
using SAPPub.E2E.Tests.TestData.Models.KS4;
using SAPPub.Playwright.Testing;

namespace SAPPub.E2E.Tests.SecondarySchoolTests;

public class EnglishAndMathsPageTests : BasePageTest
{
    public class EnglishMathsPageTestDataModel
    {
        public required string SchoolUrn { get; set; } = string.Empty;
        public required PerformanceTablesTestDataModel CurrentYear { get; set; }
        public required PerformanceTablesTestDataModel PreviousYear { get; set; }
        public required PerformanceTablesTestDataModel Previous2Year { get; set; }
        public required PerformanceTablesTestDataModel GirlsCurrentYear { get; set; }
        public required PerformanceTablesTestDataModel BoysCurrentYear { get; set; }
        public required AllStateFundedPupilsLaTestDataModel laCurrentYear { get; set; }
        public required AllStateFundedPupilsLaTestDataModel laPreviousYear { get; set; }
        public required AllStateFundedPupilsLaTestDataModel laPrevious2Year { get; set; }
        public required AllStateFundedPupilsLaTestDataModel laGirlsCurrentYear { get; set; }
        public required AllStateFundedPupilsLaTestDataModel laBoysCurrentYear { get; set; }
    }

    public class EnglishMathsBreakdownsPageTestDataModel
    {
        public required string SchoolUrn { get; set; } = string.Empty;
        public required PerformanceTablesTestDataModel DisadvantagedCurrentYear { get; set; }
        public required PerformanceTablesTestDataModel EALCurrentYear { get; set; }
        public required PerformanceTablesTestDataModel NonMobileCurrentYear { get; set; }
        public required AllStateFundedPupilsLaTestDataModel LaNonDisadvantagedCurrentYear { get; set; }
        public required AllStateFundedPupilsLaTestDataModel LaDisadvantagedCurrentYear { get; set; }
    }

    private string BasePageUrl(string urn) => $"/school/{urn}";
    private string pageUnderTest => "secondary-performance/english-and-maths/";

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

    private static readonly IDictionary<string, AllStateFundedPupilsLaTestDataModel> _laPerformanceNonDisadvantagedCurrentYearTestData = TestDataLoader.Load<AllStateFundedPupilsLaTestDataModel>(
        "KS4",
        "202425_all_state_funded_pupils_characteristics_la_NonDisadvantaged_Current").ToDictionary(x => x.Urn);

    public static TheoryData<EnglishMathsPageTestDataModel> GetPerformanceData()
    {
        var urns = _performanceTotalsCurrentYearTestData.Keys
            .Intersect(_performanceTotalsPreviousYearTestData.Keys)
            .Intersect(_performanceTotalsPrevious2YearTestData.Keys)
            .Intersect(_performanceGirlsCurrentYearTestData.Keys)
            .Intersect(_performanceBoysCurrentYearTestData.Keys)
            .Intersect(_laPerformanceCurrentYearTestData.Keys)
            .Intersect(_laPerformancePreviousYearTestData.Keys)
            .Intersect(_laPerformancePrevious2YearTestData.Keys)
            .Intersect(_laPerformanceGirlsCurrentYearTestData.Keys)
            .Intersect(_laPerformanceBoysCurrentYearTestData.Keys)
            .ToList();

        if (urns.Count == 0)
        {
            throw new InvalidOperationException("No matching URNs found across all test data sets.");
        }

        return new TheoryData<EnglishMathsPageTestDataModel>(_performanceTotalsCurrentYearTestData.Values.Select(
            currentYear => new EnglishMathsPageTestDataModel
            {
                SchoolUrn = currentYear.SchoolUrn,
                CurrentYear = currentYear,
                PreviousYear = _performanceTotalsPreviousYearTestData[currentYear.SchoolUrn],
                Previous2Year = _performanceTotalsPrevious2YearTestData[currentYear.SchoolUrn],
                laCurrentYear = _laPerformanceCurrentYearTestData[currentYear.SchoolUrn],
                laPreviousYear = _laPerformancePreviousYearTestData[currentYear.SchoolUrn],
                laPrevious2Year = _laPerformancePrevious2YearTestData[currentYear.SchoolUrn],
                GirlsCurrentYear = _performanceGirlsCurrentYearTestData[currentYear.SchoolUrn],
                BoysCurrentYear = _performanceBoysCurrentYearTestData[currentYear.SchoolUrn],
                laGirlsCurrentYear = _laPerformanceGirlsCurrentYearTestData[currentYear.SchoolUrn],
                laBoysCurrentYear = _laPerformanceBoysCurrentYearTestData[currentYear.SchoolUrn]
            }).ToArray());
    }

    [Theory]
    [MemberData(nameof(GetPerformanceData))]
    public async Task TotalsAndGirlsBoysData_Expected(EnglishMathsPageTestDataModel testData)
    {
        var pageOptions = new string[] { "grade-5-and-above", "grade-4-and-above" };

        // Arrange && Act
        var response = await Page.GotoAsync(BasePageUrl(testData.SchoolUrn));
        Assert.NotNull(response);

        foreach (var pageOption in pageOptions)
        {
            var _ = await Page.GotoPage(response.Url, pageUnderTest + pageOption);

            // Act
            // Click Show data over time button
            await Page.ClickAsync("#all-gcse-show-data-over-time-btn");
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            await AssertGradeXAndAboveTotalsPerformanceData(pageOption, testData);
            await AssertGradeXAndAboveGirlsBoysPerformanceData(pageOption, testData);
        }
    }

    public static TheoryData<EnglishMathsBreakdownsPageTestDataModel> GetBreakdownPerformanceData()
    {
        var urns = _performanceDisadvantagedCurrentYearTestData.Keys
            .Intersect(_performanceEALCurrentYearTestData.Keys)
            .Intersect(_performanceNonMobileCurrentYearTestData.Keys)
            .Intersect(_laPerformanceNonDisadvantagedCurrentYearTestData.Keys)
            .Intersect(_laPerformanceDisadvantagedCurrentYearTestData.Keys)
            .ToList();

        if (urns.Count == 0)
        {
            throw new InvalidOperationException("No matching URNs found across all breakdown test data sets.");
        }

        return new TheoryData<EnglishMathsBreakdownsPageTestDataModel>(_performanceDisadvantagedCurrentYearTestData.Values.Select(currentYear => new EnglishMathsBreakdownsPageTestDataModel
        {
            SchoolUrn = currentYear.SchoolUrn,
            DisadvantagedCurrentYear = currentYear,
            EALCurrentYear = _performanceEALCurrentYearTestData[currentYear.SchoolUrn],
            NonMobileCurrentYear = _performanceNonMobileCurrentYearTestData[currentYear.SchoolUrn],
            LaDisadvantagedCurrentYear = _laPerformanceDisadvantagedCurrentYearTestData[currentYear.SchoolUrn],
            LaNonDisadvantagedCurrentYear = _laPerformanceNonDisadvantagedCurrentYearTestData[currentYear.SchoolUrn]
        }).ToArray());
    }

    [Theory]
    [MemberData(nameof(GetBreakdownPerformanceData))]
    public async Task BreakdownData_Expected(EnglishMathsBreakdownsPageTestDataModel testData)
    {
        var pageOptions = new string[] { /* grade-7-and-above", */ "grade-5-and-above", "grade-4-and-above" };

        // Arrange && Act
        var response = await Page.GotoAsync(BasePageUrl(testData.SchoolUrn));
        Assert.NotNull(response);

        foreach (var pageOption in pageOptions)
        {
            var _ = await Page.GotoPage(response.Url, pageUnderTest + pageOption);

            // Act - expand the breakdowns section
            await Page.ExpandAccordionByIdAsync("other-pupil-characteristics-accordion");
            await Page.ExpandDetailsByIdAsync("non-disadvantaged-details");

            await AssertBreakdownPerformanceData(pageOption, testData);
        }
    }

    private async Task AssertBreakdownPerformanceData(string pageOption, EnglishMathsBreakdownsPageTestDataModel expected)
    {
        // TODO  for new data release (pageOption == "grade-7-and-above")
        var expectedValuesForGrade = pageOption switch
        {
            "grade-5-and-above" => new
            {
                DisadvantagedCurrentYear = expected.DisadvantagedCurrentYear.EngMath95Percent,
                EALCurrentYear = expected.EALCurrentYear.EngMath95Percent,
                NonMobileCurrentYear = expected.NonMobileCurrentYear.EngMath95Percent,
                LaDisadvantagedCurrentYear = expected.LaDisadvantagedCurrentYear.EngMath95Percent,
                LaNonDisadvantagedCurrentYear = expected.LaNonDisadvantagedCurrentYear.EngMath95Percent
            },
            "grade-4-and-above" => new
            {
                DisadvantagedCurrentYear = expected.DisadvantagedCurrentYear.EngMath94Percent,
                EALCurrentYear = expected.EALCurrentYear.EngMath94Percent,
                NonMobileCurrentYear = expected.NonMobileCurrentYear.EngMath94Percent,
                LaDisadvantagedCurrentYear = expected.LaDisadvantagedCurrentYear.EngMath94Percent,
                LaNonDisadvantagedCurrentYear = expected.LaNonDisadvantagedCurrentYear.EngMath94Percent
            },
            _ => throw new ArgumentException($"Invalid page option: {pageOption}")
        };

        var schoolData = await Page.GetTableRowValuesAsync("#breakdown-eal-table", 0);
        AssertHelpers.AssertNumericEqual(expectedValuesForGrade.EALCurrentYear, schoolData[0]);
        schoolData = await Page.GetTableRowValuesAsync("#breakdown-non-mobile-table", 0);
        AssertHelpers.AssertNumericEqual(expectedValuesForGrade.NonMobileCurrentYear, schoolData[0]);
        schoolData = await Page.GetTableRowValuesAsync("#breakdown-disadvantaged-table", 0);
        AssertHelpers.AssertNumericEqual(expectedValuesForGrade.DisadvantagedCurrentYear, schoolData[0]);
        var laData = await Page.GetTableRowValuesAsync("#breakdown-disadvantaged-table", 1);
        AssertHelpers.AssertNumericEqual(expectedValuesForGrade.LaDisadvantagedCurrentYear, laData[0]);
        laData = await Page.GetTableRowValuesAsync("#breakdown-non-disadvantaged-table", 0);
        AssertHelpers.AssertNumericEqual(expectedValuesForGrade.LaNonDisadvantagedCurrentYear, laData[0]);
    }

    private async Task AssertGradeXAndAboveTotalsPerformanceData(string pageOption, EnglishMathsPageTestDataModel expected)
    {
        // TODO  for new data release (pageOption == "grade-7-and-above")
        var expectedValuesForGrade = pageOption switch
        {
            "grade-5-and-above" => new
            {
                SchoolCurrentYear = expected.CurrentYear.EngMath95Percent,
                SchoolPreviousYear = expected.PreviousYear.EngMath95Percent,
                SchoolPrevious2Year = expected.Previous2Year.EngMath95Percent,
                LaCurrentYear = expected.laCurrentYear.EngMath95Percent,
                LaPreviousYear = expected.laPreviousYear.EngMath95Percent,
                LaPrevious2Year = expected.laPrevious2Year.EngMath95Percent,
            },
            "grade-4-and-above" => new
            {
                SchoolCurrentYear = expected.CurrentYear.EngMath94Percent,
                SchoolPreviousYear = expected.PreviousYear.EngMath94Percent,
                SchoolPrevious2Year = expected.Previous2Year.EngMath94Percent,
                LaCurrentYear = expected.laCurrentYear.EngMath94Percent,
                LaPreviousYear = expected.laPreviousYear.EngMath94Percent,
                LaPrevious2Year = expected.laPrevious2Year.EngMath94Percent,
            },
            _ => throw new ArgumentException($"Invalid page option: {pageOption}")
        };

        var schoolData = await Page.GetTableRowValuesAsync("#all-gcse-data-overtime-table", 0);
        AssertHelpers.AssertNumericEqual(expectedValuesForGrade.SchoolPrevious2Year, schoolData[0]);
        AssertHelpers.AssertNumericEqual(expectedValuesForGrade.SchoolPreviousYear, schoolData[1]);
        AssertHelpers.AssertNumericEqual(expectedValuesForGrade.SchoolCurrentYear, schoolData[2]);

        var laData = await Page.GetTableRowValuesAsync("#all-gcse-data-overtime-table", 1);
        AssertHelpers.AssertNumericEqual(expectedValuesForGrade.LaPrevious2Year, laData[0]);
        AssertHelpers.AssertNumericEqual(expectedValuesForGrade.LaPreviousYear, laData[1]);
        AssertHelpers.AssertNumericEqual(expectedValuesForGrade.LaCurrentYear, laData[2]);
    }

    private async Task AssertGradeXAndAboveGirlsBoysPerformanceData(string pageOption, EnglishMathsPageTestDataModel expected)
    {
        var expectedValuesForGrade = pageOption switch
        {
            // TODO  for new data release if (pageOption == "grade-7-and-above")
            "grade-5-and-above" => new
            {
                GirlsCurrentYear = expected.GirlsCurrentYear.EngMath95Percent,
                BoysCurrentYear = expected.BoysCurrentYear.EngMath95Percent,
                laGirlsCurrentYear = expected.laGirlsCurrentYear.EngMath95Percent,
                laBoysCurrentYear = expected.laBoysCurrentYear.EngMath95Percent,
            },
            "grade-4-and-above" => new
            {
                GirlsCurrentYear = expected.GirlsCurrentYear.EngMath94Percent,
                BoysCurrentYear = expected.BoysCurrentYear.EngMath94Percent,
                laGirlsCurrentYear = expected.laGirlsCurrentYear.EngMath94Percent,
                laBoysCurrentYear = expected.laBoysCurrentYear.EngMath94Percent,
            },
            _ => throw new ArgumentException($"Invalid page option: {pageOption}")
        };

        var schoolDataByBreakdown = await Page.GetTableRowValuesAsync("#breakdown-gcse-current-year-table", 0);
        AssertHelpers.AssertNumericEqual(expectedValuesForGrade.GirlsCurrentYear, schoolDataByBreakdown[0]);
        AssertHelpers.AssertNumericEqual(expectedValuesForGrade.BoysCurrentYear, schoolDataByBreakdown[1]);
        var laDataByBreakdown = await Page.GetTableRowValuesAsync("#breakdown-gcse-current-year-table", 1);
        AssertHelpers.AssertNumericEqual(expectedValuesForGrade.laGirlsCurrentYear, laDataByBreakdown[0]);
        AssertHelpers.AssertNumericEqual(expectedValuesForGrade.laBoysCurrentYear, laDataByBreakdown[1]);
     }
}
