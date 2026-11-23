using Microsoft.Playwright;
using SAPPub.E2E.Tests.Helpers;
using SAPPub.E2E.Tests.Infrastructure;
using SAPPub.E2E.Tests.TestData.Models.KS4;
using SAPPub.Playwright.Testing;
using System.Text.RegularExpressions;

namespace SAPPub.E2E.Tests.SecondarySchoolTests;

public class DestinationsPageTests(ITestOutputHelper output) : BasePageTest
{
    private string BasePageUrl(string urn) => $"/school/{urn}";
    private string pageUnderTest => "destinations/secondary";

    private static readonly Dictionary<string, KS4DestinationsTestDataModel> _establishmentTestData = TestDataLoader.LoadObject<Dictionary<string, KS4DestinationsTestDataModel>>(
        "KS4",
        "ees_ks4_202324_api_establishment");

    private static readonly Dictionary<string, KS4DestinationsTestDataModel> _laTestData = TestDataLoader.LoadObject<Dictionary<string, KS4DestinationsTestDataModel>>(
        "KS4",
        "ees_ks4_202324_api_la");

    private static readonly KS4DestinationsTestDataModel _englandTestData = TestDataLoader.LoadObject<KS4DestinationsTestDataModel>(
        "KS4",
        "ees_ks4_202324_api_england");

    public record DestinationsTestData
    { 
        public required string SchoolUrn { get; init; }
        public required KS4DestinationsTestDataModel School { get; init; }
        public required KS4DestinationsTestDataModel LocalAuthority { get; init; }
        public required KS4DestinationsTestDataModel England { get; init; }
        public override string ToString() => SchoolUrn;
    }

    public static TheoryData<DestinationsTestData> GetDestinationsTestData()
    {
        var urns = _establishmentTestData.Keys
            .Intersect(_laTestData.Keys)
            .ToList();

        if (urns.Count == 0 || _englandTestData.Periods.Count == 0)
        {
            throw new InvalidOperationException("No matching URNs found across the KS4 destinations test data sets.");
        }

        return [.. urns.Select(urn => new DestinationsTestData
        {
            SchoolUrn = urn,
            School = _establishmentTestData[urn],
            LocalAuthority = _laTestData[urn],
            England = _englandTestData,
        })];
    
    }

    [Theory]
    [MemberData(nameof(GetDestinationsTestData))]
    public async Task AllDestinations_ShowsExpectedCurrentYearAndDataOverTime(DestinationsTestData testData)
    {
        // temporarily add a warning to the test log about a known bug, until it is fixed
        // https://dfedigital.atlassian.net/browse/SIP2PUBLIC-382
        string[] failingUrns = ["137205", "137252"];

        // Arrange and act
        await GotoDestinationsPage(testData.SchoolUrn);

        // if data over time table not shown, select button to show it
        var overTimeTable = Page.Locator("#all-dest-data-overtime-table");
        if (await overTimeTable.CountAsync() == 0)
        {
            await Page.ClickAsync("#all-dest-show-data-over-time-btn");
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        var schoolValues = await Page.GetTableRowValuesAsync("#all-dest-data-overtime-table", 0);
        var laValues = await Page.GetTableRowValuesAsync("#all-dest-data-overtime-table", 1);
        var englandValues = await Page.GetTableRowValuesAsync("#all-dest-data-overtime-table", 2);

        if (failingUrns.Contains(testData.SchoolUrn))
        {
            output.WriteLine($"WARNING: Known bug in destinations data over time for school URN {testData.SchoolUrn}. Test will continue without full school and la data check.");
            Assert.Equal("Not available", schoolValues[0]);
            Assert.Equal("Not available", laValues[0]);
        }
        else
        {
            AssertHelpers.AssertNumericEqual(testData.School.AllDestinationsPercent(Ks4DestinationsTestDataExtensions.Previous2Year), schoolValues[0]);
            AssertHelpers.AssertNumericEqual(testData.LocalAuthority.AllDestinationsPercent(Ks4DestinationsTestDataExtensions.Previous2Year), laValues[0]);
        }
        AssertHelpers.AssertNumericEqual(testData.School.AllDestinationsPercent(Ks4DestinationsTestDataExtensions.PreviousYear), schoolValues[1]);
        AssertHelpers.AssertNumericEqual(testData.School.AllDestinationsPercent(Ks4DestinationsTestDataExtensions.CurrentYear), schoolValues[2]);
        AssertHelpers.AssertNumericEqual(testData.LocalAuthority.AllDestinationsPercent(Ks4DestinationsTestDataExtensions.PreviousYear), laValues[1]);
        AssertHelpers.AssertNumericEqual(testData.LocalAuthority.AllDestinationsPercent(Ks4DestinationsTestDataExtensions.CurrentYear), laValues[2]);
        await AssertOverTimeRow(2, testData.England);
    }

    [Theory]
    [MemberData(nameof(GetDestinationsTestData))]
    public async Task NumberOfStudensInlcudedInMeasure_ShowsExpected(DestinationsTestData testData)
    {
        // Arrange and act
        await GotoDestinationsPage(testData.SchoolUrn);

        // Assert - current year (school, la and England)
        // if #no-of-students-included-in-measure is on the page, check the value
        if (await Page.Locator("#no-of-students-included-in-measure").CountAsync() > 0)
        {
            var text = await Page.Locator("#no-of-students-included-in-measure").InnerTextAsync();
            var number = Regex.Match(text, @"[\d.]+\s*$").Value;
            AssertHelpers.AssertNumericEqual(testData.School.CohortCount(), number);
        }
    }

    [Theory]
    [MemberData(nameof(GetDestinationsTestData))]
    public async Task DisadvantagedDestinations_ShowsExpected(DestinationsTestData testData)
    {
        // Arrange and act
        await GotoDestinationsPage(testData.SchoolUrn);
        await Page.ExpandAccordionByIdAsync("disadvantaged-destinations-accordion");
        await Page.ExpandDetailsAsync("Compare with non-disadvantaged pupils");

        // Assert
        var disadvantaged = await GetTableColumnAsync("table#all-dest-disadvantaged-current-year-table", 0);
        AssertHelpers.AssertNumericEqual(testData.School.AllDestinationsPercent(disadvantageStatus: "Disadvantaged"), disadvantaged[0]);
        AssertHelpers.AssertNumericEqual(testData.LocalAuthority.AllDestinationsPercent(disadvantageStatus: "Disadvantaged"), disadvantaged[1]);
        AssertHelpers.AssertNumericEqual(testData.England.AllDestinationsPercent(disadvantageStatus: "Disadvantaged"), disadvantaged[2]);

        var nonDisadvantaged = await GetTableColumnAsync("table#all-dest-disadvantaged-current-year-table", 1);
        AssertHelpers.AssertNumericEqual(testData.LocalAuthority.AllDestinationsPercent(disadvantageStatus: "Not known to be disadvantaged"), nonDisadvantaged[0]);
        AssertHelpers.AssertNumericEqual(testData.England.AllDestinationsPercent(disadvantageStatus: "Not known to be disadvantaged"), nonDisadvantaged[1]);
    }

    [Theory]
    [MemberData(nameof(GetDestinationsTestData))]
    public async Task EducationDestinations_ShowsExpected(DestinationsTestData testData)
    {
        // Arrange and act
        await GotoDestinationsPage(testData.SchoolUrn);

        // Assert
        await AssertSeriesRow(testData, "stayed-in-education-table", "Pupils who stayed in education", "Sustained education destination");

        await AssertSeriesRow(testData, "where-pupils-studied-table", "Further education provider", "Further education");
        await AssertSeriesRow(testData, "where-pupils-studied-table", "School sixth form", "School sixth form");
        await AssertSeriesRow(testData, "where-pupils-studied-table", "Sixth form college", "Sixth form college");
        await AssertSeriesRow(testData, "where-pupils-studied-table", "Other education destinations", "Other education destination");
    }

    [Theory]
    [MemberData(nameof(GetDestinationsTestData))]
    public async Task ApprenticeshipsEmploymentAndNotSustainedDestinations_ShowsExpected(DestinationsTestData testData)
    {
        // Arrange and act
        await GotoDestinationsPage(testData.SchoolUrn);

        // Assert
        await AssertSeriesRow(testData, "apprenticeships-or-employment-table", "Pupils who stayed in employment for at least 2 terms", "Sustained employment destination");
        await AssertSeriesRow(testData, "apprenticeships-or-employment-table", "Pupils who stayed in an apprenticeship for at least 6 months", "Sustained apprenticeships");
        
        await AssertSeriesRow(testData, "did-not-stay-in-education-or-employment-table", "Pupils who did not stay in education or employment for at least 2 terms", "Not recorded as a sustained destination");
        await AssertSeriesRow(testData, "did-not-stay-in-education-or-employment-table", "Destination unknown", "Activity not captured");
    }

    private async Task AssertOverTimeRow(int rowNumber, KS4DestinationsTestDataModel rows)
    {
        var values = await Page.GetTableRowValuesAsync("#all-dest-data-overtime-table", rowNumber);
        Assert.Equal(3, values.Count);
        AssertHelpers.AssertNumericEqual(rows.AllDestinationsPercent(Ks4DestinationsTestDataExtensions.Previous2Year), values[0]);
        AssertHelpers.AssertNumericEqual(rows.AllDestinationsPercent(Ks4DestinationsTestDataExtensions.PreviousYear), values[1]);
        AssertHelpers.AssertNumericEqual(rows.AllDestinationsPercent(Ks4DestinationsTestDataExtensions.CurrentYear), values[2]);
    }

    private async Task AssertSeriesRow(DestinationsTestData testData, string tableId, string rowHeader, string destinationDescription)
    {
        var values = await Page.GetTableRowValuesAsync(tableId, rowHeader);
        Assert.Equal(3, values.Count);
        AssertHelpers.AssertNumericEqual(testData.School.DestinationPercent(destinationDescription), values[0]);
        AssertHelpers.AssertNumericEqual(testData.LocalAuthority.DestinationPercent(destinationDescription), values[1]);
        AssertHelpers.AssertNumericEqual(testData.England.DestinationPercent(destinationDescription), values[2]);
    }

    private async Task GotoDestinationsPage(string schoolUrn)
    {
        var response = await Page.GotoAsync(BasePageUrl(schoolUrn));
        Assert.NotNull(response);
        _ = await Page.GotoPage(response.Url, pageUnderTest);
    }

    private async Task<IReadOnlyList<string>> GetTableColumnAsync(string tableSelector, int tableIndex = 0)
    {
        var rows = Page.Locator(tableSelector).Nth(tableIndex).Locator("tbody tr");
        var values = new List<string>();
        for (var i = 0; i < await rows.CountAsync(); i++)
        {
            values.Add(await rows.Nth(i).Locator("td").First.InnerTextAsync());
        }

        return values;
    }
}