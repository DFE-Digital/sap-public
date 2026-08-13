using Microsoft.Playwright;
using SAPPub.Integration.Tests;
using System.Text.RegularExpressions;

namespace SAPPub.IntegrationTests.SecondarySchoolTests;

[Collection("Integration Tests")]
public class AttainmentPageTests() : BasePageTest()
{
    private string PageUrl(string urn) => $"/school/{urn}";

    [Theory]
    [InlineData("136745", 39.8, 44.1, 46.1)]
    [InlineData("137638", 44.2, 44.1, 46.1)]
    [InlineData("142894", 39.2, 44.1, 46.1)]
    [InlineData("144496", 49.6, 44.1, 46.1)]
    [InlineData("144991", 45.6, 44.1, 46.1)]
    public async Task SecondaryAcademicPerformanceProgressAndAttainment_ShowsExpectedAttainmentData(string urn, double expectedAttainmentSchool, double expectedAttainmentLA, double expectedAttainmentEngland)
    {
        // Arrange && Act
        var _ = await Page.GotoAsync(PageUrl(urn));
        var response = await ClickAcademicPerformanceLinkAsync();

        // Assert
        var schoolAttainment8 = await GetScoreAsync("attainment8-establishment-card", "The attainment 8 score for this school is");
        Assert.NotNull(schoolAttainment8);
        Assert.Equal(expectedAttainmentSchool.ToString("F1"), schoolAttainment8.Last());

        var laAttainment8 = await GetScoreAsync("attainment8-localauthority-and-national-card", "the local council average of");
        Assert.NotNull(laAttainment8);
        Assert.Equal(expectedAttainmentLA.ToString("F1"), laAttainment8.Last());

        var englandAttainment8 = await GetScoreAsync("attainment8-localauthority-and-national-card", "the national average of");
        Assert.NotNull(englandAttainment8);
        Assert.Equal(expectedAttainmentEngland.ToString("F1"), englandAttainment8.Last());
    }

    [Theory]
    [InlineData("142894", -0.99, -1.27, -0.71, -0.14, 96, 121)]
    public async Task SecondaryAcademicPerformanceProgressAndAttainment_ShowsExpectedProgressData(
        string urn,
        double expectedProgressSchool,
        double expectedBandingLower,
        double expectedBandingHigher,
        double expectedProgressLA,
        double expectedPupilsInMeasure,
        double expectedPupilsInMeasureOutOf)
    {
        // Arrange && Act
        var _ = await Page.GotoAsync($"school/{urn}");
        var response = await ClickAcademicPerformanceLinkAsync();
        _ = await GotoAcademicPerformanceLink(response!.Url, "previous");

        // Assert
        var schoolProgress8 = await GetScoreAsync("progress8-establishment-card", "Pupils at this school score");
        Assert.NotNull(schoolProgress8);
        Assert.Equal(expectedProgressSchool.ToString("F2"), schoolProgress8.Last());

        var progress8Banding = await GetScoreAsync("progress8-establishment-card", "The confidence interval is");
        Assert.NotNull(progress8Banding);
        Assert.Equal(expectedBandingLower.ToString("F2"), progress8Banding.First());
        Assert.Equal(expectedBandingHigher.ToString("F2"), progress8Banding.Last());

        // TODO  - pupils in this measure

        var laProgress8 = await GetScoreAsync("progress8-localauthority-card", "The local authority average is");
        Assert.NotNull(laProgress8);
        Assert.Equal(expectedProgressLA.ToString("F2"), laProgress8.Last());
    }

    private Task<IResponse> ClickAcademicPerformanceLinkAsync()
    {
        var response = Page.RunAndWaitForResponseAsync(
            async () =>
            {
                await Page.GetByRole(AriaRole.Link, new() { Name = "Secondary academic performance" }).ClickAsync();
            },
            response => response.Url.Contains("/secondary-performance/progress-attainment/current") && response.Status == 200
        );
        return response;
    }

    private Task<IResponse?> GotoAcademicPerformanceLink(string urlstring, string year = "current")
    {
        const string marker = "school/";
        var i = urlstring.IndexOf(marker);
        var j = urlstring.LastIndexOf('/');
        var previousYearPerformanceUrl = urlstring.Substring(i, j - i);
        return Page.GotoAsync($"{previousYearPerformanceUrl}/{year}");
    }

    private async Task<IEnumerable<string>?> GetScoreAsync(string dataTestid, string textString)
    {
        var card = Page.Locator($"[data-testid='{dataTestid}']");
        var p = card.Locator("p.govuk-body", new() { HasTextString = textString });
        var input = await p.InnerTextAsync();
        var match = Regex.Matches(input, @"[+-]?\d+(?:\.\d+)?")
                 .Cast<Match>();

        return match.Select(m => m.Value);
    }
}
