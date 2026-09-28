using SAPPub.Integration.Tests.Helpers;
using SAPPub.Integration.Tests.Primary;
using SAPPub.Integration.Tests.TestData.Models.KS4;
using SAPPub.Playwright.Testing;

namespace SAPPub.Integration.Tests.SecondarySchoolTests;

public class AdditionalMeasuresPageTests : BasePageTest
{
    private string BasePageUrl(string urn) => $"/school/{urn}";
    private string pageUnderTest => "secondary-performance/additional-measures";

    private static readonly IDictionary<string, PerformanceTablesTestDataModel> _performanceTotalsTestData = TestDataLoader.Load<PerformanceTablesTestDataModel>(
        "KS4",
        "202425_performance_tables_schools_final_Total_Current_Year").ToDictionary(x => x.SchoolUrn);

    private static readonly IDictionary<string, PerformanceTablesTestDataModel> _performanceDisadvantagedData = TestDataLoader.Load<PerformanceTablesTestDataModel>(
        "KS4",
        "202425_performance_tables_schools_final_Disadvantaged_Current_Year").ToDictionary(x => x.SchoolUrn);

    private static readonly IDictionary<string, PerformanceTablesTestDataModel> _performanceBoysData = TestDataLoader.Load<PerformanceTablesTestDataModel>(
        "KS4",
        "202425_performance_tables_schools_final_Boys_Current_Year").ToDictionary(x => x.SchoolUrn);

    private static readonly IDictionary<string, PerformanceTablesTestDataModel> _performanceGirlsData = TestDataLoader.Load<PerformanceTablesTestDataModel>(
        "KS4",
        "202425_performance_tables_schools_final_Girls_Current_Year").ToDictionary(x => x.SchoolUrn);

    private static readonly IDictionary<string, PerformanceTablesTestDataModel> _performanceEALData = TestDataLoader.Load<PerformanceTablesTestDataModel>(
        "KS4",
        "202425_performance_tables_schools_final_EAL_Current_Year").ToDictionary(x => x.SchoolUrn);

    private static readonly IDictionary<string, PerformanceTablesTestDataModel> _performanceNonMobileData = TestDataLoader.Load<PerformanceTablesTestDataModel>(
        "KS4",
        "202425_performance_tables_schools_final_NonMobile_Current_Year").ToDictionary(x => x.SchoolUrn);

    private static readonly IDictionary<string, InformationAboutSchoolsTestDataModel> _wholeSchoolTestData = TestDataLoader.Load<InformationAboutSchoolsTestDataModel>(
        "KS4",
        "202425_information_about_schools_final").ToDictionary(x => x.SchoolUrn);

    public record AdditionalMeasuresTestData
    {
        public required PerformanceTablesTestDataModel Totals { get; init; }
        public required PerformanceTablesTestDataModel Boys { get; init; }
        public required PerformanceTablesTestDataModel Girls { get; init; }
        public required PerformanceTablesTestDataModel EAL { get; init; }
        public required PerformanceTablesTestDataModel NonMobile { get; init; }
        public required PerformanceTablesTestDataModel Disadvantaged { get; init; }
    }

    public static TheoryData<AdditionalMeasuresTestData> GetAdditionalMeasuresData()
    {
        var urns = _performanceTotalsTestData.Keys
            .Intersect(_performanceBoysData.Keys)
            .Intersect(_performanceGirlsData.Keys)
            .Intersect(_performanceEALData.Keys)
            .Intersect(_performanceNonMobileData.Keys)
            .Intersect(_performanceDisadvantagedData.Keys);

        if (!urns.Any())
        {
            throw new InvalidOperationException("No matching test data found for test.");
        }
        var joinedData = urns.Select(urn => new AdditionalMeasuresTestData()
            {
                Totals = _performanceTotalsTestData[urn],
                Boys = _performanceBoysData[urn],
                Girls = _performanceGirlsData[urn],
                EAL = _performanceEALData[urn],
                NonMobile = _performanceNonMobileData[urn],
                Disadvantaged = _performanceDisadvantagedData[urn]
            });

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
        await Page.ExpandAccordionByIdAsync("further-pop-data-eoks4");

        // Assert
        // only establishment-level data is checked so far
        await AssertAverageNumberOfExamsEnteredPerPupilSection(testData.Totals, testData.Disadvantaged);
        await AssertAdditionalEntryAndAchievementMeasuresSection(testData.Totals);
        await AssertNumberOfPupilsAtTheEndOfKS4Section(testData);
    }

    public static TheoryData<InformationAboutSchoolsTestDataModel> GetWholeSchoolData()
    {
        return new TheoryData<InformationAboutSchoolsTestDataModel>(_wholeSchoolTestData.Values.ToArray());
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
        await AssertNumberOfPupilsAtTheWholeSchoolSection(testData);
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

        // LA and England non-disadvantaged data is not yet tested here
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

    private async Task AssertNumberOfPupilsAtTheEndOfKS4Section(AdditionalMeasuresTestData testCase)
    {
        var tableContent = await Page.GetTableRowValuesAsync("num-pupil-eofks4-table", "Number of pupils at the end of KS4");
        AssertHelpers.AssertNumericEqual(testCase.Totals.PupilCount, tableContent[0]);

        var tableContentGirls = await Page.GetTableRowValuesAsync("num-pupil-eofks4-breakdown-table", "Girls");
        AssertHelpers.AssertNumericEqual(testCase.Girls.PupilCount, tableContentGirls[0]);
        var tableContentBoys = await Page.GetTableRowValuesAsync("num-pupil-eofks4-breakdown-table", "Boys");
        AssertHelpers.AssertNumericEqual(testCase.Boys.PupilCount, tableContentBoys[0]);
        var tableContentEAL = await Page.GetTableRowValuesAsync("num-pupil-eofks4-breakdown-table", "English as an additional language (EAL)");
        AssertHelpers.AssertNumericEqual(testCase.EAL.PupilCount, tableContentEAL[0]);
        var tableContentNonMobile = await Page.GetTableRowValuesAsync("num-pupil-eofks4-breakdown-table", "Non-mobile pupils");
        AssertHelpers.AssertNumericEqual(testCase.NonMobile.PupilCount, tableContentNonMobile[0]);
        var tableContentDisadvantaged = await Page.GetTableRowValuesAsync("num-pupil-eofks4-disadvantaged-table", "Number of disadvantaged pupils");
        AssertHelpers.AssertNumericEqual(testCase.Disadvantaged.PupilCount, tableContentDisadvantaged[0]);
    }

    private async Task AssertNumberOfPupilsAtTheWholeSchoolSection(InformationAboutSchoolsTestDataModel testCase)
    {
        // only establishment-level data is checked so far
        // not testing table "num-pupil-wholeschool-table", "Number of pupils on roll" because it's GIAS data and should be E2E tested on About the school page eventually
        var pupilsWithSENSupport = await Page.GetTableRowValuesAsync("num-pupil-whole-school-sen-table", "Pupils with SEN support");
        AssertHelpers.AssertNumericEqual(testCase.SenNoEhcpPupilPercent, pupilsWithSENSupport[0]);
        var pupilsWithEHCP = await Page.GetTableRowValuesAsync("num-pupil-whole-school-ehcp-table", "Pupils with EHCPs");
        AssertHelpers.AssertNumericEqual(testCase.SenWithEhcpPupilPercent, pupilsWithEHCP[0]);
    }
}
