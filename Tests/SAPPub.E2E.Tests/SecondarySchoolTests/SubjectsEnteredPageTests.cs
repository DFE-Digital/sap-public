using Microsoft.Playwright;
using SAPPub.Integration.Tests.Helpers;
using SAPPub.Integration.Tests.Primary;
using SAPPub.Playwright.Testing;

namespace SAPPub.Integration.Tests.SecondarySchoolTests;

public class SubjectsEnteredPageTests : BasePageTest
{
    private string BasePageUrl(string urn) => $"/school/{urn}";
    private string pageUnderTest => "secondary-performance/subjects-entered";

    public record SubjectsEnteredTestCase(
        string Urn,
        List<(string, int)> ExpectedGcseSubjects,
        List<(string, int)> ExpectedTechLevelSubjects);


    public static TheoryData<SubjectsEnteredTestCase> GetSubjectsEnteredTestData()
    {
        var testdata =  TestDataLoader.GetTestData("KS4", "SubjectsEntered");
        var theoryData = new TheoryData<SubjectsEnteredTestCase>();
        if (testdata != null)
        {
            foreach (var row in testdata)
            {
                var testCase = new SubjectsEnteredTestCase(row[0].GetString() ?? string.Empty, new List<(string, int)>(), new List<(string, int)>());
                foreach (var subject in row[1].EnumerateArray())
                {
                    var subjectName = subject[0].GetString() ?? string.Empty;
                    var subjectCount = subject[1].GetInt32();
                    testCase.ExpectedGcseSubjects.Add((subjectName, subjectCount));
                }
                foreach (var subject in row[2].EnumerateArray())
                {
                    var subjectName = subject[0].GetString() ?? string.Empty;
                    var subjectCount = subject[1].GetInt32();
                    testCase.ExpectedTechLevelSubjects.Add((subjectName, subjectCount));
                }
                theoryData.Add(testCase);
            }
        }
        return theoryData;
    }

    [Theory]
    [MemberData(nameof(GetSubjectsEnteredTestData))]
    public async Task CurrentYearSelected_ShowsExpectedAttainmentData_Memberdata(
        SubjectsEnteredTestCase testCase)
    {
        // Arrange && Act
        var response = await Page.GotoAsync(BasePageUrl(testCase.Urn));
        Assert.NotNull(response);
        var _ = await Page.GotoPage(response.Url, pageUnderTest);

        // Assert
        await AssertSchoolSubjectsEnteredData(Page, "GCSE subjects entered", testCase.ExpectedGcseSubjects);
        await AssertSchoolSubjectsEnteredData(Page, "Technical Award subjects entered", testCase.ExpectedTechLevelSubjects);
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
            Assert.NotNull(subjectRow);
            Assert.Equal(expectedSubject.Item2.ToString(), subjectRow[2]);
        }
    }
}
