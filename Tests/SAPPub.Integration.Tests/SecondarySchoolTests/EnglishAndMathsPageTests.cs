using Microsoft.Playwright;
using SAPPub.Integration.Tests.Helpers;
using SAPPub.Integration.Tests.Primary;
using SAPPub.Integration.Tests.TestData.Models.KS4;
using SAPPub.Playwright.Testing;

namespace SAPPub.Integration.Tests.SecondarySchoolTests;

public class EnglishAndMathsPageTests : BasePageTest
{
    private string BasePageUrl(string urn) => $"/school/{urn}";
    private string pageUnderTest => "secondary-performance/english-and-maths/";

    private static readonly IDictionary<string, PerformanceTablesTestDataModel> _performanceTotalsCurrentYearTestData = TestDataLoader.Load<PerformanceTablesTestDataModel>(
        "KS4",
        "202425_performance_tables_schools_final_Total_Current_Year").ToDictionary(x => x.SchoolUrn);

    private static readonly IDictionary<string, PerformanceTablesTestDataModel> _performanceTotalsPreviousYearTestData = TestDataLoader.Load<PerformanceTablesTestDataModel>(
        "KS4",
        "202425_performance_tables_schools_final_Total_Previous_Year").ToDictionary(x => x.SchoolUrn);

    //private static readonly IDictionary<string, PerformanceTablesTestDataModel> _performanceTotalsPrevious2YearTestData = TestDataLoader.Load<PerformanceTablesTestDataModel>(
    //    "KS4",
    //    "202425_performance_tables_schools_final_Total_Previous_2_Year").ToDictionary(x => x.SchoolUrn);

    public static TheoryData<EnglishMathsPageTestDataModel> GetPerformanceData()
    {
        var urns = _performanceTotalsCurrentYearTestData.Keys
            .Intersect(_performanceTotalsPreviousYearTestData.Keys)
            .ToList();
        return new TheoryData<EnglishMathsPageTestDataModel>(_performanceTotalsCurrentYearTestData.Values.Select(currentYear => new EnglishMathsPageTestDataModel
        {
            SchoolUrn = currentYear.SchoolUrn,
            CurrentYear = currentYear,
            PreviousYear = _performanceTotalsPreviousYearTestData[currentYear.SchoolUrn]
            //Previous2Year = _performanceTotalsPrevious2YearTestData[currentYear.SchoolUrn] // Assuming you have a dictionary for Previous2Year
        }).ToArray());
    }

    public class EnglishMathsPageTestDataModel
    {
        public required string SchoolUrn { get; set; } = string.Empty;
        public required PerformanceTablesTestDataModel CurrentYear { get; set; }
        public required PerformanceTablesTestDataModel PreviousYear { get; set; }
        //public required PerformanceTablesTestDataModel Previous2Year { get; set; }
    }

    [Theory]
    [MemberData(nameof(GetPerformanceData))]
    public async Task Grade5AndAboveData_Expected(EnglishMathsPageTestDataModel testData)
    {
        var page_option = "grade-5-and-above";

        // Arrange && Act
        var response = await Page.GotoAsync(BasePageUrl(testData.SchoolUrn));
        Assert.NotNull(response);
        var _ = await Page.GotoPage(response.Url, pageUnderTest+page_option);

        // Act
        // Click Show data over time button
        await Page.ClickAsync("#all-gcse-show-data-over-time-btn");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // and click Show as a table button
        await Page.ClickAsync("#all-gcse-data-over-time-show-btn");

        var schoolData = await Page.GetTableRowValuesAsync("#all-gcse-data-overtime-table", "School");
        AssertHelpers.AssertNumericEqual(testData.PreviousYear.EngMath95Percent, schoolData[1]);
        AssertHelpers.AssertNumericEqual(testData.CurrentYear.EngMath95Percent, schoolData[2]);

        var schoolDataByBreakdown = await Page.GetTableRowValuesAsync("#breakdown-gcse-current-year-table", "School");
    }
}
