using Microsoft.Playwright;
using SAPPub.E2E.Tests.Helpers;
using SAPPub.E2E.Tests.Infrastructure;
using SAPPub.E2E.Tests.TestData.Models.KS2;
using SAPPub.Playwright.Testing;

namespace SAPPub.E2E.Tests.Primary;

/// <summary>
/// TODO: The test data in sap-public\Tests\SAPPub.E2E.Tests\TestData\2526\KS2\PupilProgress*.json is currently out of date and needs to be updated when the new file is available.
/// </summary>

[Collection("Integration Tests")]
public class PupilProgressTests() : BasePageTest()
{
    private string PageUrl(string urn) => $"/school/{urn}";

    private static readonly TheoryData<PupilProgressTestDataModel> _pupilProgressTestData =
        TestDataLoader.LoadTheoryData<PupilProgressTestDataModel>("KS2", "PupilProgress");

    private static readonly TheoryData<NonBandedPupilProgressTestDataModel> _disadvantagedPupilProgressTestData =
        TestDataLoader.LoadTheoryData<NonBandedPupilProgressTestDataModel>("KS2", "PupilProgressDisadvantaged");

    private static readonly TheoryData<NonBandedPupilProgressTestDataModel> _ealPupilProgressTestData =
    TestDataLoader.LoadTheoryData<NonBandedPupilProgressTestDataModel>("KS2", "PupilProgressEAL");

    public static TheoryData<PupilProgressTestDataModel> GetPupilProgressTestData() => _pupilProgressTestData;

    public static TheoryData<NonBandedPupilProgressTestDataModel> GetDisadvantagedPupilProgressTestData() => _disadvantagedPupilProgressTestData;

    public static TheoryData<NonBandedPupilProgressTestDataModel> GetEalPupilProgressTestData() => _ealPupilProgressTestData;

    [Theory]
    [MemberData(nameof(GetPupilProgressTestData))]
    public async Task AllPupils_ShowsExpectedSchoolProgressData(PupilProgressTestDataModel testData)
    {
        // Arrange && Act
        await GotoPupilProgressPage(testData.SchoolUrn, "all-pupils");

        // Assert
        await AssertSchoolProgressData(Page, "reading", testData.ReadingScore, testData.ReadingBanding);
        await AssertSchoolProgressData(Page, "writing", testData.WritingScore, testData.WritingBanding);
        await AssertSchoolProgressData(Page, "maths", testData.MathsScore, testData.MathsBanding);
    }

    [Theory]
    [MemberData(nameof(GetPupilProgressTestData))]
    public async Task AllPupils_ShowsExpectedLocalAuthorityAndEnglandComparisons(PupilProgressTestDataModel testData)
    {
        // Arrange && Act
        await GotoPupilProgressPage(testData.SchoolUrn, "all-pupils");

        // Assert
        await AssertComparisonData(Page, "reading", testData.LaReadingScore, testData.EnglandReadingScore);
        await AssertComparisonData(Page, "writing", testData.LaWritingScore, testData.EnglandWritingScore);
        await AssertComparisonData(Page, "maths", testData.LaMathsScore, testData.EnglandMathsScore);
    }

    [Theory]
    [MemberData(nameof(GetDisadvantagedPupilProgressTestData))]
    public async Task DisadvantagedPupil_ShowsExpectedSchoolProgressData(NonBandedPupilProgressTestDataModel testData)
    {
        // Arrange && Act
        await GotoPupilProgressPage(testData.SchoolUrn, "disadvantaged-pupils");

        // Assert
        await AssertNonBandedProgressRow(Page, "reading", 0, testData.ReadingScore);
        await AssertNonBandedProgressRow(Page, "writing", 0, testData.WritingScore);
        await AssertNonBandedProgressRow(Page, "maths", 0, testData.MathsScore);
    }

    [Theory]
    [MemberData(nameof(GetDisadvantagedPupilProgressTestData))]
    public async Task DisadvantagedPupil_ShowsExpectedLocalAuthorityAndEnglandComparisons(NonBandedPupilProgressTestDataModel testData)
    {
        // Arrange && Act
        await GotoPupilProgressPage(testData.SchoolUrn, "disadvantaged-pupils");

        // Assert
        await AssertNonBandedProgressRow(Page, "reading", 1, testData.LaReadingScore);
        await AssertNonBandedProgressRow(Page, "reading", 2, testData.EnglandReadingScore);
        await AssertNonBandedProgressRow(Page, "writing", 1, testData.LaWritingScore);
        await AssertNonBandedProgressRow(Page, "writing", 2, testData.EnglandWritingScore);
        await AssertNonBandedProgressRow(Page, "maths", 1, testData.LaMathsScore);
        await AssertNonBandedProgressRow(Page, "maths", 2, testData.EnglandMathsScore);
    }

    [Theory]
    [MemberData(nameof(GetEalPupilProgressTestData))]
    public async Task EalPupils_ShowsExpectedSchoolProgressData(NonBandedPupilProgressTestDataModel testData)
    {
        // Arrange && Act
        await GotoPupilProgressPage(testData.SchoolUrn, "english-as-additional-language");

        // Assert
        await AssertNonBandedProgressRow(Page, "reading", 0, testData.ReadingScore);
        await AssertNonBandedProgressRow(Page, "writing", 0, testData.WritingScore);
        await AssertNonBandedProgressRow(Page, "maths", 0, testData.MathsScore);
    }

    [Theory]
    [MemberData(nameof(GetEalPupilProgressTestData))]
    public async Task EalPupils_ShowsExpectedLocalAuthorityAndEnglandComparisons(NonBandedPupilProgressTestDataModel testData)
    {
        // Arrange && Act
        await GotoPupilProgressPage(testData.SchoolUrn, "english-as-additional-language");

        // Assert
        await AssertNonBandedProgressRow(Page, "reading", 1, testData.LaReadingScore);
        await AssertNonBandedProgressRow(Page, "reading", 2, testData.EnglandReadingScore);
        await AssertNonBandedProgressRow(Page, "writing", 1, testData.LaWritingScore);
        await AssertNonBandedProgressRow(Page, "writing", 2, testData.EnglandWritingScore);
        await AssertNonBandedProgressRow(Page, "maths", 1, testData.LaMathsScore);
        await AssertNonBandedProgressRow(Page, "maths", 2, testData.EnglandMathsScore);
    }

    private async Task GotoPupilProgressPage(string urn, string pupilCharacteristic)
    {
        var response = await Page.GotoAsync(PageUrl(urn));
        Assert.NotNull(response);
        await Page.GotoPage(response.Url, $"primary-performance/pupil-progress/{pupilCharacteristic}");
    }

    private static async Task AssertSchoolProgressData(IPage page, string subject, string expectedScore, string expectedBanding)
    {
        // columns: school name, progress score, banding tag and description
        var row = await page.GetTableRowValuesAsync($"{subject}-progress-table", 0);

        AssertHelpers.AssertNumericEqual(expectedScore, row[1]);
        Assert.Contains(expectedBanding, row[2], StringComparison.OrdinalIgnoreCase);
    }

    // Single table with no banding column; rows: disadvantaged pupils at school, local authority, England average
    private static async Task AssertNonBandedProgressRow(IPage page, string subject, int rowIndex, string expectedScore)
    {
        var row = await page.GetTableRowValuesAsync($"{subject}-progress-table", rowIndex);

        AssertHelpers.AssertNumericEqual(expectedScore, row[1]);
    }

    private static async Task AssertComparisonData(IPage page, string subject, string expectedLaScore, string expectedEnglandScore)
    {
        var tableId = $"{subject}-comparison-table";

        var laRow = await page.GetTableRowValuesAsync(tableId, 0);
        AssertHelpers.AssertNumericEqual(expectedLaScore, laRow[1]);

        var englandRow = await page.GetTableRowValuesAsync(tableId, "England average");
        AssertHelpers.AssertNumericEqual(expectedEnglandScore, englandRow[1]);
    }
}
