using SAPPub.Integration.Tests.Helpers;
using SAPPub.Integration.Tests.Primary;
using SAPPub.Integration.Tests.TestData.Models.KS4;
using SAPPub.Playwright.Testing;
using System.Net.WebSockets;
using System.Runtime.ConstrainedExecution;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SAPPub.Integration.Tests.SecondarySchoolTests;

public class AdditionalMeasuresPageTests : BasePageTest
{
    private string BasePageUrl(string urn) => $"/school/{urn}";
    private string pageUnderTest => "secondary-performance/additional-measures";

    public static TheoryData<PerformanceTablesTestDataModel, PerformanceTablesTestDataModel> GetAdditionalMeasuresBreakdownTotalCurrentYearData() =>
        TestDataLoader.LoadTheoryData<PerformanceTablesTestDataModel, PerformanceTablesTestDataModel>(
        "KS4",
        "202425_performance_tables_schools_final_Total_Current_Year",
        "202425_performance_tables_schools_final_Disadvantaged_Current_Year");

    [Theory]
    [MemberData(nameof(GetAdditionalMeasuresBreakdownTotalCurrentYearData))]
    public async Task BreakdownTotalCurrentYearData_ShowsExpected(PerformanceTablesTestDataModel breakdownTotalData, PerformanceTablesTestDataModel breakdownDisadvantagedData)
    {
        // Arrange && Act
        var response = await Page.GotoAsync(BasePageUrl(breakdownTotalData.SchoolUrn));
        Assert.NotNull(response);
        var _ = await Page.GotoPage(response.Url, pageUnderTest);
        await Page.ExpandAccordionByIdAsync("average-numexams-entered-by-pupil-char");

        // Assert
        // only establishment-level data is checked so far
        await AssertAverageNumberOfExamsEnteredPerPupil(breakdownTotalData, breakdownDisadvantagedData);
        await AssertAdditionalEntryAndAchievementMeasuresData(breakdownTotalData);
        await AssertNumberOfPupilsAtTheEndOfKS4Data(breakdownTotalData);
    }


    private async Task AssertAverageNumberOfExamsEnteredPerPupil(PerformanceTablesTestDataModel testCase1, PerformanceTablesTestDataModel testCase2)
    {
        var gcseData = await Page.GetTableRowValuesAsync("additional-measures-exams-entered-table", "GCSE qualifications");
        AssertHelpers.AssertNumericEqual(testCase1.GcseEntriesAverage, gcseData[0]);
        var allQualificationsData = await Page.GetTableRowValuesAsync("additional-measures-exams-entered-table", "All KS4 qualifications");
        AssertHelpers.AssertNumericEqual(testCase1.QualEntriesAverage, allQualificationsData[0]);

        var gcseDataDisadvantaged = await Page.GetTableRowValuesAsync("additional-measures-exams-entered-disadvantaged-table", "GCSE qualifications");
        AssertHelpers.AssertNumericEqual(testCase2.GcseEntriesAverage, gcseDataDisadvantaged[0]);
        var allQualificationsDataDisadvantaged = await Page.GetTableRowValuesAsync("additional-measures-exams-entered-disadvantaged-table", "All KS4 qualifications");
        AssertHelpers.AssertNumericEqual(testCase2.QualEntriesAverage, allQualificationsDataDisadvantaged[0]);
    }

    private async Task AssertAdditionalEntryAndAchievementMeasuresData(PerformanceTablesTestDataModel testCase)
    {
        var tableContent = await Page.GetTableRowValuesAsync("additional-eanda-measures-table", "Pupils who achieved at least 1 qualification");
        AssertHelpers.AssertNumericEqual(testCase.Gcse91Percent, tableContent[0]);
        tableContent = await Page.GetTableRowValuesAsync("additional-eanda-measures-table", "Pupils entered for biology, chemistry and physics");
        AssertHelpers.AssertNumericEqual(testCase.SciTripleEnteringPercent, tableContent[0]);
        tableContent = await Page.GetTableRowValuesAsync("additional-eanda-measures-table", "Pupils entered for more than one foreign language");
        AssertHelpers.AssertNumericEqual(testCase.LanMultipleEnteringPercent, tableContent[0]);
    }

    private async Task AssertNumberOfPupilsAtTheEndOfKS4Data(PerformanceTablesTestDataModel testCase)
    {
        var tableContent = await Page.GetTableRowValuesAsync("num-pupil-eofks4-table", "Number of pupils at the end of KS4");
        AssertHelpers.AssertNumericEqual(testCase.PupilCount, tableContent[0]);
    }
}
