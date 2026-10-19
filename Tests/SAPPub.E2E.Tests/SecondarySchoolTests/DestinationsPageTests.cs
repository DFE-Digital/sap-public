using Microsoft.Playwright;
using SAPPub.E2E.Tests.Helpers;
using SAPPub.E2E.Tests.Infrastructure;
using SAPPub.E2E.Tests.TestData.Models.KS4;
using SAPPub.Playwright.Testing;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SAPPub.E2E.Tests.SecondarySchoolTests;

public class DestinationsPageTests : BasePageTest
{
    private string BasePageUrl(string urn) => $"/school/{urn}";
    private string pageUnderTest => "destinations/secondary";

    private static readonly ILookup<string, KS4DestinationsTestDataModel> _establishmentTestData = TestDataLoader.Load<KS4DestinationsTestDataModel>(
        "KS4",
        "ees_ks4_202324_api_establishment").ToLookup(x => x.SchoolUrn);

    private static readonly ILookup<string, KS4DestinationsTestDataModel> _laTestData = TestDataLoader.Load<KS4DestinationsTestDataModel>(
        "KS4",
        "ees_ks4_202324_api_la").ToLookup(x => x.SchoolUrn);

    private static readonly List<KS4DestinationsTestDataModel> _englandTestData = TestDataLoader.Load<KS4DestinationsTestDataModel>(
        "KS4",
        "ees_ks4_202324_api_england");

    public record DestinationsTestData
    { 
        public required string SchoolUrn { get; init; }
        public required IReadOnlyList<KS4DestinationsTestDataModel> School { get; init; }
        public required IReadOnlyList<KS4DestinationsTestDataModel> LocalAuthority { get; init; }
        public required IReadOnlyList<KS4DestinationsTestDataModel> England { get; init; }
        public override string ToString() => SchoolUrn;
    }

    public static TheoryData<DestinationsTestData> GetDestinationsTestData()
    {
        var urns = _establishmentTestData
            .Select(a => a.Key)
            .Intersect(_laTestData.Select(a => a.Key))
            .ToList();

        if (urns.Count == 0 || _englandTestData.Count == 0)
        {
            throw new InvalidOperationException("No matching URNs found across the KS4 destinations test data sets.");
        }

        return [.. urns.Select(urn => new DestinationsTestData
        {
            SchoolUrn = urn,
            School = _establishmentTestData[urn].ToList(),
            LocalAuthority = _laTestData[urn].ToList(),
            England = _englandTestData,
        })];
    
    }

    [Theory]
    [MemberData(nameof(GetDestinationsTestData))]
    public async Task AllDestinations_ShowsExpectedCurrentYearAndDataOverTime(DestinationsTestData testData)
    {
        // Arrange and act
        await GotoDestinationsPage(testData.SchoolUrn);

        // Assert - current year (school, la and England)
        var currentYear = await GetTableColumnAsync("#all-dest-current-year-table");
        AssertHelpers.AssertNumericEqual(testData.School.AllDestinationsPercent(), currentYear[0]);
        AssertHelpers.AssertNumericEqual(testData.LocalAuthority.AllDestinationsPercent(), currentYear[1]);
        AssertHelpers.AssertNumericEqual(testData.England.AllDestinationsPercent(), currentYear[2]);

        await Page.ClickAsync("#all-dest-show-data-over-time-btn");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await AssertOverTimeRow(0, testData.School);
        await AssertOverTimeRow(1, testData.LocalAuthority);
        await AssertOverTimeRow(2, testData.England);
    }

    [Theory]
    [MemberData(nameof(GetDestinationsTestData))]
    public async Task NumberOfStudensInlcudedInMeasure_ShowsExpected(DestinationsTestData testData)
    {
        // Arrange and act
        await GotoDestinationsPage(testData.SchoolUrn);

        // Assert - current year (school, la and England)
        var text = await Page.Locator("#no-of-students-included-in-measure").InnerTextAsync();
        var number = Regex.Match(text, @"[\d.]+\s*$").Value;
        AssertHelpers.AssertNumericEqual(testData.School.CohortCount(), number);
    }

    [Theory]
    [MemberData(nameof(GetDestinationsTestData))]
    public async Task DisadvantagedDestinations_ShowsExpected(DestinationsTestData testData)
    {
        await Page.ExpandAccordionByIdAsync("disadvantaged-destinations-accordion");
        await Page.ExpandDetailsAsync("Compare with non-disadvantaged pupils");

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


    private async Task AssertOverTimeRow(int rowNumber, IReadOnlyList<KS4DestinationsTestDataModel> rows)
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