using SAPPub.Integration.Tests.Helpers;
using SAPPub.Integration.Tests.Primary;
using SAPPub.Integration.Tests.TestData.Models.KS4;
using SAPPub.Playwright.Testing;

namespace SAPPub.Integration.Tests.SecondarySchoolTests;

public class AdditionalMeasuresPageTests : BasePageTest
{
    private string BasePageUrl(string urn) => $"/school/{urn}";
    private string pageUnderTest => "secondary-performance/additional-measures";

    public record AdditionalMeasuresTestData
    {
        public required PerformanceTablesTestDataModel Totals { get; init; }
        public required PerformanceTablesTestDataModel Disadvantaged { get; init; }
    }

    public static TheoryData<AdditionalMeasuresTestData> GetAdditionalMeasuresData()
    {
        var performanceTotalsData = TestDataLoader.Load<PerformanceTablesTestDataModel>(
            "KS4",
            "202425_performance_tables_schools_final_Total_Current_Year");

        var performanceDisadvantagedData = TestDataLoader.Load<PerformanceTablesTestDataModel>(
            "KS4",
            "202425_performance_tables_schools_final_Disadvantaged_Current_Year");

        var joinedData = performanceTotalsData.Join(
            performanceDisadvantagedData,
            totals => totals.SchoolUrn,
            disadvantaged => disadvantaged.SchoolUrn,
            (totals, disadvantaged) => new AdditionalMeasuresTestData()
            {
                Totals = totals,
                Disadvantaged = disadvantaged
            });

        if (!joinedData.Any())
        {
            throw new InvalidOperationException("No matching test data found for test.");
        }

        return new TheoryData<AdditionalMeasuresTestData>(joinedData.ToArray());
    }

    [Theory]
    [MemberData(nameof(GetAdditionalMeasuresData))]
    public async Task AdditionalMeasures_ShowsExpected(AdditionalMeasuresTestData testData)
    {
        // Arrange && Act
        var response = await Page.GotoAsync(BasePageUrl(testData.Totals.SchoolUrn));
        Assert.NotNull(response);
        var _ = await Page.GotoPage(response.Url, pageUnderTest);
        await Page.ExpandAccordionByIdAsync("average-numexams-entered-by-pupil-char");

        // Assert
        // only establishment-level data is checked so far
        await AssertAverageNumberOfExamsEnteredPerPupilSection(testData.Totals, testData.Disadvantaged);
        await AssertAdditionalEntryAndAchievementMeasuresSection(testData.Totals);
        await AssertNumberOfPupilsAtTheEndOfKS4Section(testData.Totals);
    }

    public static TheoryData<InformationAboutSchoolsTestDataModel> GetWholeSchoolData()
    {
        var wholeSchoolData = TestDataLoader.Load<InformationAboutSchoolsTestDataModel>(
            "KS4",
            "202425_information_about_schools_final"
            );
        return new TheoryData<InformationAboutSchoolsTestDataModel>(wholeSchoolData);
    }

    [Theory]
    [MemberData(nameof(GetWholeSchoolData))]
    public async Task NumberOfPupilsAtTheWholeSchoolSection_ShowsExpected(InformationAboutSchoolsTestDataModel testData)
    {
        // Arrange && Act
        var response = await Page.GotoAsync(BasePageUrl(testData.SchoolUrn));
        Assert.NotNull(response);
        var _ = await Page.GotoPage(response.Url, pageUnderTest);
        await Page.ExpandAccordionByIdAsync("further-pop-data-wholeschool");

        // Assert
        // only establishment-level data is checked so far
        // not testing table "num-pupil-wholsechool-table", "Number of pupils on roll" because it's GIAS data and should be E2E tested on About the school page eventually
        var pupilsWithSENSupport = await Page.GetTableRowValuesAsync("num-pupil-whole-school-sen-table", "Pupils with SEN support");
        AssertHelpers.AssertNumericEqual(testData.SenNoEhcpPupilPercent, pupilsWithSENSupport[0]);
        var pupilsWithEHCP = await Page.GetTableRowValuesAsync("num-pupil-whole-school-ehcp-table", "Pupils with EHCPs");
        AssertHelpers.AssertNumericEqual(testData.SenWithEhcpPupilPercent, pupilsWithEHCP[0]);
    }

    private async Task AssertAverageNumberOfExamsEnteredPerPupilSection(PerformanceTablesTestDataModel testCase1, PerformanceTablesTestDataModel testCase2)
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

    private async Task AssertAdditionalEntryAndAchievementMeasuresSection(PerformanceTablesTestDataModel testCase)
    {
        var tableContent = await Page.GetTableRowValuesAsync("additional-eanda-measures-table", "Pupils who achieved at least 1 qualification");
        AssertHelpers.AssertNumericEqual(testCase.Gcse91Percent, tableContent[0]);
        tableContent = await Page.GetTableRowValuesAsync("additional-eanda-measures-table", "Pupils entered for biology, chemistry and physics");
        AssertHelpers.AssertNumericEqual(testCase.SciTripleEnteringPercent, tableContent[0]);
        tableContent = await Page.GetTableRowValuesAsync("additional-eanda-measures-table", "Pupils entered for more than one foreign language");
        AssertHelpers.AssertNumericEqual(testCase.LanMultipleEnteringPercent, tableContent[0]);
    }

    private async Task AssertNumberOfPupilsAtTheEndOfKS4Section(PerformanceTablesTestDataModel testCase)
    {
        var tableContent = await Page.GetTableRowValuesAsync("num-pupil-eofks4-table", "Number of pupils at the end of KS4");
        AssertHelpers.AssertNumericEqual(testCase.PupilCount, tableContent[0]);

        // TODO check all the values in the breakdown tabel, which involves loading and joining data for additional files 
        // 202425_performance_tables_schools_final_Boys_Current_Year,
        // 202425_performance_tables_schools_final_Girls_Current_Year,
        // 202425_performance_tables_schools_final_EAL_Current_Year,
        // 202425_performance_tables_schools_final_NonMobile_Current_Year
    }
}
