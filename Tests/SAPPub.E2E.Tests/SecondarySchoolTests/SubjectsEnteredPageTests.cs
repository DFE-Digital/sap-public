using System.Text.Json;
using Microsoft.Playwright;
using SAPPub.E2E.Tests.Helpers;
using SAPPub.E2E.Tests.Infrastructure;
using SAPPub.E2E.Tests.TestData.Models.KS4;
using SAPPub.Playwright.Testing;

namespace SAPPub.E2E.Tests.SecondarySchoolTests;

public class SubjectsEnteredPageTests : BasePageTest
{
    private string BasePageUrl(string urn) => $"/school/{urn}";
    private string pageUnderTest => "secondary-performance/subjects-entered";

    public static TheoryData<SubjectEntriesDataModel> GetSubjectsEnteredTestData()
    {
        var testdata = TestDataLoader.Load<JsonElement>("KS4", "SubjectsEntered");
        var theoryData = new TheoryData<SubjectEntriesDataModel>();
        if (testdata != null)
        {
            foreach (var row in testdata)
            {
                var testCase = new SubjectEntriesDataModel { SchoolUrn = row.GetProperty("urn").GetString() ?? string.Empty };
                foreach (var subject in row.GetProperty("gcse").EnumerateArray())
                {
                    var subjectName = subject[0].GetString() ?? string.Empty;
                    var subjectCount = subject[1].GetInt32();
                    testCase.ExpectedGcseSubjects.Add((subjectName, subjectCount));
                }
                foreach (var subject in row.GetProperty("vocational").EnumerateArray())
                {
                    var subjectName = subject[0].GetString() ?? string.Empty;
                    var subjectCount = subject[1].GetInt32();
                    testCase.ExpectedTechLevelSubjects.Add((subjectName, subjectCount));
                }
                foreach (var subject in row.GetProperty("other").EnumerateArray())
                {
                    var subjectName = subject[0].GetString() ?? string.Empty;
                    var subjectCount = subject[1].GetInt32();
                    testCase.ExpectedOtherSubjects.Add((subjectName, subjectCount));
                }
                theoryData.Add(testCase);
            }
        }
        return theoryData;
    }

    [Theory]
    [MemberData(nameof(GetSubjectsEnteredTestData))]
    public async Task CurrentYearSelected_ShowsExpectedSubjectsEntered(
        SubjectEntriesDataModel testCase)
    {
        // Arrange && Act
        var response = await Page.GotoAsync(BasePageUrl(testCase.SchoolUrn));
        Assert.NotNull(response);
        var _ = await Page.GotoPage(response.Url, pageUnderTest);

        // Assert
        await AssertSchoolSubjectsEnteredData(Page, "GCSE subjects entered", testCase.ExpectedGcseSubjects);
        await AssertSchoolSubjectsEnteredData(Page, "Technical Award subjects entered", testCase.ExpectedTechLevelSubjects);
        await AssertSchoolSubjectsEnteredData(Page, "Other subjects entered", testCase.ExpectedOtherSubjects);
    }

    private async Task AssertSchoolSubjectsEnteredData(IPage Page, string tableCaption, List<(string, int)> expectedSubjects)
    {
        var tableValues = await Page.GetTableRowsValuesByTableCaptionAsync(tableCaption);

        Assert.NotNull(tableValues);

        foreach(var expectedSubject in expectedSubjects)
        {
            var subjectRows = tableValues.Where(row => row[0].Equals(expectedSubject.Item1, StringComparison.InvariantCultureIgnoreCase));
            if(subjectRows.Count() > 1)
            {
                // some subjects have multiple rows, so we need to check that at least one of them has the expected count
                Assert.NotNull(subjectRows.FirstOrDefault(r => r[2].Equals(expectedSubject.Item2.ToString())));
                continue;
            }
            var subjectRow = subjectRows.FirstOrDefault();
            Assert.True(subjectRow != null, $"Expected to find a row for subject '{expectedSubject.Item1}' with count '{expectedSubject.Item2}'");
            Assert.Equal(expectedSubject.Item2.ToString(), subjectRow[2]);
        }
    }
}
