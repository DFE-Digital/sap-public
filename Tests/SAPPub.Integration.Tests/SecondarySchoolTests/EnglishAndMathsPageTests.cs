using Microsoft.Playwright;
using SAPPub.Integration.Tests.Helpers;
using SAPPub.Integration.Tests.Primary;
using SAPPub.Integration.Tests.TestData.Models.KS4;
using SAPPub.Playwright.Testing;

namespace SAPPub.Integration.Tests.SecondarySchoolTests;

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
    }

    public class EnglishMathsBreakdownsPageTestDataModel
    {
        public required string SchoolUrn { get; set; } = string.Empty;
        public required PerformanceTablesTestDataModel DisadvantagedCurrentYear { get; set; }
        public required PerformanceTablesTestDataModel EALCurrentYear { get; set; }
        public required PerformanceTablesTestDataModel NonMobileCurrentYear { get; set; }
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

    public static TheoryData<EnglishMathsPageTestDataModel> GetPerformanceData()
    {
        var urns = _performanceTotalsCurrentYearTestData.Keys
            .Intersect(_performanceTotalsPreviousYearTestData.Keys)
            .Intersect(_performanceTotalsPrevious2YearTestData.Keys)
            .Intersect(_performanceGirlsCurrentYearTestData.Keys)
            .Intersect(_performanceBoysCurrentYearTestData.Keys)
            .ToList();

        if (urns.Count == 0)
        {
            throw new InvalidOperationException("No matching URNs found across all test data sets.");
        }

        return new TheoryData<EnglishMathsPageTestDataModel>(_performanceTotalsCurrentYearTestData.Values.Select(currentYear => new EnglishMathsPageTestDataModel
        {
            SchoolUrn = currentYear.SchoolUrn,
            CurrentYear = currentYear,
            PreviousYear = _performanceTotalsPreviousYearTestData[currentYear.SchoolUrn],
            Previous2Year = _performanceTotalsPrevious2YearTestData[currentYear.SchoolUrn],
            GirlsCurrentYear = _performanceGirlsCurrentYearTestData[currentYear.SchoolUrn],
            BoysCurrentYear = _performanceBoysCurrentYearTestData[currentYear.SchoolUrn]
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

            // and click Show as a table button
            await Page.ClickAsync("#all-gcse-data-over-time-show-btn");

            await AssertPerformanceData(pageOption, testData);
        }
    }

    public static TheoryData<EnglishMathsBreakdownsPageTestDataModel> GetBreakdownPerformanceData()
    {
        var urns = _performanceDisadvantagedCurrentYearTestData.Keys
            .Intersect(_performanceEALCurrentYearTestData.Keys)
            .Intersect(_performanceNonMobileCurrentYearTestData.Keys)
            .ToList();

        if(urns.Count == 0)
        {
            throw new InvalidOperationException("No matching URNs found across all breakdown test data sets.");
        };

        return new TheoryData<EnglishMathsBreakdownsPageTestDataModel>(_performanceDisadvantagedCurrentYearTestData.Values.Select(currentYear => new EnglishMathsBreakdownsPageTestDataModel
        {
            SchoolUrn = currentYear.SchoolUrn,
            DisadvantagedCurrentYear = currentYear,
            EALCurrentYear = _performanceEALCurrentYearTestData[currentYear.SchoolUrn],
            NonMobileCurrentYear = _performanceNonMobileCurrentYearTestData[currentYear.SchoolUrn]
        }).ToArray());
    }

    [Theory]
    [MemberData(nameof(GetBreakdownPerformanceData))]
    public async Task BreakdownData_Expected(EnglishMathsBreakdownsPageTestDataModel testData)
    {
        var pageOptions = new string[] { "grade-5-and-above", "grade-4-and-above" };

        // Arrange && Act
        var response = await Page.GotoAsync(BasePageUrl(testData.SchoolUrn));
        Assert.NotNull(response);

        foreach (var pageOption in pageOptions)
        {
            var _ = await Page.GotoPage(response.Url, pageUnderTest + pageOption);

            // Act - expand the breakdowns section
            await Page.ExpandAccordionByIdAsync("other-pupil-characteristics-accordion");

            await AssertBreakdownPerformanceData(pageOption, testData);
        }
    }

    private async Task AssertBreakdownPerformanceData(string pageOption, EnglishMathsBreakdownsPageTestDataModel expected)
    {
        if (pageOption == "grade-5-and-above")
        {
            var schoolData = await Page.GetTableRowValuesAsync("#breakdown-eal-table", "Pupils with EAL");
            AssertHelpers.AssertNumericEqual(expected.EALCurrentYear.EngMath95Percent, schoolData[0]);
            schoolData = await Page.GetTableRowValuesAsync("#breakdown-non-mobile-table", "Non-mobile pupils");
            AssertHelpers.AssertNumericEqual(expected.NonMobileCurrentYear.EngMath95Percent, schoolData[0]);
            schoolData = await Page.GetTableRowValuesAsync("#breakdown-disadvantaged-table", "Disadvantaged pupils");
            AssertHelpers.AssertNumericEqual(expected.DisadvantagedCurrentYear.EngMath95Percent, schoolData[0]);
        }
        else if (pageOption == "grade-4-and-above")
        {
            var schoolData = await Page.GetTableRowValuesAsync("#breakdown-gcse-current-year-table", "School");
            AssertHelpers.AssertNumericEqual(expected.DisadvantagedCurrentYear.EngMath94Percent, schoolData[0]);
            AssertHelpers.AssertNumericEqual(expected.EALCurrentYear.EngMath94Percent, schoolData[1]);
            AssertHelpers.AssertNumericEqual(expected.NonMobileCurrentYear.EngMath94Percent, schoolData[2]);
        }
    }
    private async Task AssertPerformanceData(string pageOption, EnglishMathsPageTestDataModel expected)
    {
        if (pageOption == "grade-5-and-above")
        {
            var schoolData = await Page.GetTableRowValuesAsync("#all-gcse-data-overtime-table", "School");
            AssertHelpers.AssertNumericEqual(expected.Previous2Year.EngMath95Percent, schoolData[0]);
            AssertHelpers.AssertNumericEqual(expected.PreviousYear.EngMath95Percent, schoolData[1]);
            AssertHelpers.AssertNumericEqual(expected.CurrentYear.EngMath95Percent, schoolData[2]);
            var schoolDataByBreakdown = await Page.GetTableRowValuesAsync("#breakdown-gcse-current-year-table", "School");
            AssertHelpers.AssertNumericEqual(expected.GirlsCurrentYear.EngMath95Percent, schoolDataByBreakdown[0]);
            AssertHelpers.AssertNumericEqual(expected.BoysCurrentYear.EngMath95Percent, schoolDataByBreakdown[1]);
        }
        else if (pageOption == "grade-4-and-above")
        {
            var schoolData = await Page.GetTableRowValuesAsync("#all-gcse-data-overtime-table", "School");
            AssertHelpers.AssertNumericEqual(expected.Previous2Year.EngMath94Percent, schoolData[0]);
            AssertHelpers.AssertNumericEqual(expected.PreviousYear.EngMath94Percent, schoolData[1]);
            AssertHelpers.AssertNumericEqual(expected.CurrentYear.EngMath94Percent, schoolData[2]);
            var schoolDataByBreakdown = await Page.GetTableRowValuesAsync("#breakdown-gcse-current-year-table", "School");
            AssertHelpers.AssertNumericEqual(expected.GirlsCurrentYear.EngMath94Percent, schoolDataByBreakdown[0]);
            AssertHelpers.AssertNumericEqual(expected.BoysCurrentYear.EngMath94Percent, schoolDataByBreakdown[1]);
        }
    }
}
