using Microsoft.Playwright;
using SAPPub.Integration.Tests;
using SAPPub.Playwright.Testing;

namespace SAPPub.IntegrationTests.SecondarySchoolTests;


[Collection("Integration Tests")]
public class DestinationsPageTests() : BasePageTest()
{
    private string BasePageUrl(string urn) => $"/school/{urn}";

    private string pageUnderTest => $"destinations/secondary";


    // TODO: replace School/LA/England values (percentages, current year 2022 to 2023) for each URN from source data.
    [Theory(Skip = "Until data populated")]
    [InlineData("105574", /* school */ "0%", /* LA */ "0%", /* England */ "0%")]
    public async Task DestinationsPage_StayedInEducationTable_ShowsExpectedData(string urn, string expectedSchool, string expectedLA, string expectedEngland)
    {
        // Arrange & Act
        var response = await Page.GotoAsync(BasePageUrl(urn));

        // Assert
        var row = await Page.GetTableRowValuesAsync("stayed-in-education-table", "Pupils who stayed in education");
        Assert.Equal(expectedSchool, row[0]);
        Assert.Equal(expectedLA, row[1]);
        Assert.Equal(expectedEngland, row[2]);
    }

    // TODO: replace School/LA/England values for each row using source fields
    [Theory(Skip = "Until data populated")]
    [InlineData("105574",
        /* further education: school, LA, England */ "0%", "0%", "0%",
        /* school sixth form: school, LA, England */ "0%", "0%", "0%",
        /* sixth form college: school, LA, England */ "0%", "0%", "0%",
        /* other education destinations: school, LA, England */ "0%", "0%", "0%")]
    public async Task DestinationsPage_WherePupilsStudiedTable_ShowsExpectedData(
        string urn,
        string furtherEdSchool, string furtherEdLA, string furtherEdEngland,
        string schoolSixthFormSchool, string schoolSixthFormLA, string schoolSixthFormEngland,
        string collegeSixthFormSchool, string collegeSixthFormLA, string collegeSixthFormEngland,
        string otherEdSchool, string otherEdLA, string otherEdEngland)
    {
        // Arrange & Act
        var response = await Page.GotoAsync(BasePageUrl(urn));

        // Assert
        var furtherEdRow = await Page.GetTableRowValuesAsync("where-pupils-studied-table", "Further education provider");
        Assert.Equal(furtherEdSchool, furtherEdRow[0]);
        Assert.Equal(furtherEdLA, furtherEdRow[1]);
        Assert.Equal(furtherEdEngland, furtherEdRow[2]);

        var schoolSixthFormRow = await Page.GetTableRowValuesAsync("where-pupils-studied-table", "School sixth form");
        Assert.Equal(schoolSixthFormSchool, schoolSixthFormRow[0]);
        Assert.Equal(schoolSixthFormLA, schoolSixthFormRow[1]);
        Assert.Equal(schoolSixthFormEngland, schoolSixthFormRow[2]);

        var collegeSixthFormRow = await Page.GetTableRowValuesAsync("where-pupils-studied-table", "Sixth form college");
        Assert.Equal(collegeSixthFormSchool, collegeSixthFormRow[0]);
        Assert.Equal(collegeSixthFormLA, collegeSixthFormRow[1]);
        Assert.Equal(collegeSixthFormEngland, collegeSixthFormRow[2]);

        var otherEdRow = await Page.GetTableRowValuesAsync("where-pupils-studied-table", "Other education destinations");
        Assert.Equal(otherEdSchool, otherEdRow[0]);
        Assert.Equal(otherEdLA, otherEdRow[1]);
        Assert.Equal(otherEdEngland, otherEdRow[2]);
    }

    // TODO: replace School/LA/England values for employment/apprenticeship rows using source fields
    [Theory(Skip = "Until data populated")]
    [InlineData("105574",
        /* employment: school, LA, England */ "0%", "0%", "0%",
        /* apprenticeship: school, LA, England */ "0%", "0%", "0%")]
    public async Task DestinationsPage_ApprenticeshipsOrEmploymentTable_ShowsExpectedData(
        string urn,
        string employmentSchool, string employmentLA, string employmentEngland,
        string apprenticeSchool, string apprenticeLA, string apprenticeEngland)
    {
        // Arrange & Act
        var response = await Page.GotoAsync(BasePageUrl(urn));

        // Assert
        var employmentRow = await Page.GetTableRowValuesAsync("apprenticeships-or-employment-table", "Pupils who stayed in employment for at least 2 terms");
        Assert.Equal(employmentSchool, employmentRow[0]);
        Assert.Equal(employmentLA, employmentRow[1]);
        Assert.Equal(employmentEngland, employmentRow[2]);

        var apprenticeRow = await Page.GetTableRowValuesAsync("apprenticeships-or-employment-table", "Pupils who stayed in an apprenticeship for at least 6 months");
        Assert.Equal(apprenticeSchool, apprenticeRow[0]);
        Assert.Equal(apprenticeLA, apprenticeRow[1]);
        Assert.Equal(apprenticeEngland, apprenticeRow[2]);
    }

    // TODO: replace School/LA/England values for not-sustained/unknown rows using source fields
    [Theory(Skip = "Until data populated")]
    [InlineData("105574",
        /* not sustained: school, LA, England */ "0%", "0%", "0%",
        /* unknown: school, LA, England */ "0%", "0%", "0%")]
    public async Task DestinationsPage_DidNotStayInEducationOrEmploymentTable_ShowsExpectedData(
        string urn,
        string notSustainedSchool, string notSustainedLA, string notSustainedEngland,
        string unknownSchool, string unknownLA, string unknownEngland)
    {
        // Arrange & Act
        var response = await Page.GotoAsync(BasePageUrl(urn));

        // Assert
        var notSustainedRow = await Page.GetTableRowValuesAsync("did-not-stay-in-education-or-employment-table", "Pupils who did not stay in education or employment for at least 2 terms");
        Assert.Equal(notSustainedSchool, notSustainedRow[0]);
        Assert.Equal(notSustainedLA, notSustainedRow[1]);
        Assert.Equal(notSustainedEngland, notSustainedRow[2]);

        var unknownRow = await Page.GetTableRowValuesAsync("did-not-stay-in-education-or-employment-table", "Destination unknown");
        Assert.Equal(unknownSchool, unknownRow[0]);
        Assert.Equal(unknownLA, unknownRow[1]);
        Assert.Equal(unknownEngland, unknownRow[2]);
    }
}
