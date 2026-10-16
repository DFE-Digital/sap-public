using SAPPub.E2E.Tests.Helpers;
using SAPPub.E2E.Tests.Infrastructure;
using SAPPub.E2E.Tests.TestData.Models.KS4;
using System.Text.RegularExpressions;

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

        return new TheoryData<DestinationsTestData>(urns.Select(urn => new DestinationsTestData
        {
            SchoolUrn = urn,
            School = _establishmentTestData[urn].ToList(),
            LocalAuthority = _laTestData[urn].ToList(),
            England = _englandTestData,
        }));
    
    }

    [Theory]
    [MemberData(nameof(GetDestinationsTestData))]
    public async Task NumberOfStudentsIncludedInMeasure_ShowsExpected(DestinationsTestData testData)
    {
        // Arrange and act
        await GotoDestinationsPage(testData.SchoolUrn);

        // Assert
        var text = await Page.Locator("#no-of-students-included-in-measure").InnerTextAsync();
        var number = Regex.Match(text, @"[\d.]+\s*$").Value;
        AssertHelpers.AssertNumericEqual(testData.School.CohortCount(), number);
    }

    private async Task GotoDestinationsPage(string schoolUrn)
    {
        var response = await Page.GotoAsync(BasePageUrl(schoolUrn));
        Assert.NotNull(response);
        _ = await Page.GotoPage(response.Url, pageUnderTest);
    }
}