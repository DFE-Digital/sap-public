using Microsoft.Playwright;
using SAPPub.E2E.Tests.Helpers;
using SAPPub.E2E.Tests.Infrastructure;
using SAPPub.E2E.Tests.TestData.Models.KS2;
using SAPPub.Playwright.Testing;

namespace SAPPub.E2E.Tests.Primary;

/// <summary>
/// TODO: The test data in sap-public\Tests\SAPPub.E2E.Tests\TestData\2526\KS2\PupilProgress.json is currently out of date and needs to be updated when the new file is available.
/// </summary>

[Collection("Integration Tests")]
public class PupilProgressTests() : BasePageTest()
{
    private string PageUrl(string urn) => $"/school/{urn}";

    // Requires the EnableKS2PupilCharacteristicDropdown feature flag to be on
    private string pageUnderTest => "primary-performance/pupil-progress/all-pupils";

    private static readonly TheoryData<PupilProgressTestDataModel> _pupilProgressTestData =
        TestDataLoader.LoadTheoryData<PupilProgressTestDataModel>("KS2", "PupilProgress");

    public static TheoryData<PupilProgressTestDataModel> GetPupilProgressTestData() => _pupilProgressTestData;

    [Theory]
    [MemberData(nameof(GetPupilProgressTestData))]
    public async Task AllPupils_ShowsExpectedSchoolProgressData(PupilProgressTestDataModel testData)
    {
        // Arrange && Act
        await GotoAllPupilsPage(testData.SchoolUrn);

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
        await GotoAllPupilsPage(testData.SchoolUrn);

        // Assert
        await AssertComparisonData(Page, "reading", testData.LaReadingScore, testData.EnglandReadingScore);
        await AssertComparisonData(Page, "writing", testData.LaWritingScore, testData.EnglandWritingScore);
        await AssertComparisonData(Page, "maths", testData.LaMathsScore, testData.EnglandMathsScore);
    }

    private async Task GotoAllPupilsPage(string urn)
    {
        var response = await Page.GotoAsync(PageUrl(urn));
        Assert.NotNull(response);
        await Page.GotoPage(response.Url, pageUnderTest);
    }

    private static async Task AssertSchoolProgressData(IPage page, string subject, string expectedScore, string expectedBanding)
    {
        // columns: school name, progress score, banding tag and description
        var row = await page.GetTableRowValuesAsync($"{subject}-progress-table", 0);

        AssertHelpers.AssertNumericEqual(expectedScore, row[1]);
        Assert.Contains(expectedBanding, row[2], StringComparison.OrdinalIgnoreCase);
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
