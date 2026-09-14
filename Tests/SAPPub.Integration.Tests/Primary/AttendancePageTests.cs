using SAPPub.Playwright.Testing;

namespace SAPPub.Integration.Tests.Primary;

public class AttendancePageTests : BasePageTest
{
    private string BasePageUrl(string urn) => $"/school/{urn}";
    private string pageUnderTest => "attendance";

    [Theory]
    [InlineData("100019", 4.28844, 8.09859)]
    [InlineData("100241", 4.51361, 10.02786)]
    [InlineData("100353", 3.9508, 5.0)]
    [InlineData("100448", 6.1856, 20.33898)]
    [InlineData("100500", 6.44356, 19.87952)]
    [InlineData("100674", 5.16147, 16.90141)]
    [InlineData("100684", 3.79325, 7.70677)]
    public async Task AttendanceData_Expected(
        string urn, 
        double sess_overall_percent, 
        double enrolments_pa_10_exact_percent)
    {
        // Arrange && Act
        var response = await Page.GotoAsync(BasePageUrl(urn));
        Assert.NotNull(response);
        var _ = await Page.GotoPage(response.Url, pageUnderTest);

        var attendanceDataSchool = await Page.GetTableRowValuesAsync("attendance-table", "School");
        Assert.Equal($"{(100.0 - sess_overall_percent):0.#}%", attendanceDataSchool[0]);

        var persistentAbsenceDataSchool = await Page.GetTableRowValuesAsync("persistent-absence-table", "School");
        Assert.Equal($"{enrolments_pa_10_exact_percent:0.#}%", persistentAbsenceDataSchool[0]);
    }
}
