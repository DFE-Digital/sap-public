using Microsoft.Playwright;
using SAPPub.Integration.Tests;
using System.Text.RegularExpressions;

namespace SAPPub.IntegrationTests.SecondarySchoolTests;

[Collection("Integration Tests")]
public class AttainmentPageTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private string PageUrl(string urn) => $"/school/{urn}";

    [Theory]
    [InlineData("136745", 39.8, 44.1, 46)]
    [InlineData("137638", 44.2, 44.1, 46)]
    [InlineData("142894", 39.2, 44.1, 46)]
    [InlineData("144496", 49.6, 44.1, 46)]
    [InlineData("144991", 45.6, 44.1, 46)]
    [InlineData("145564", 50.4, 44.1, 46)]
    [InlineData("147531", 49.1, 44.1, 46)]
    [InlineData("147894", 57.5, 44.1, 46)]
    [InlineData("147711", 45.8, 44.1, 46)]
    [InlineData("136971", 46.4, 44.1, 46)]
    [InlineData("137903", 46.9, 44.1, 46)]
    [InlineData("148109", 44.7, 44.1, 46)]
    [InlineData("145253", 41.5, 44.1, 46)]
    [InlineData("148706", 39.4, 44.1, 46)]
    [InlineData("138717", 48.2, 44.1, 46)]
    [InlineData("149962", 35.7, 44.1, 46)]
    [InlineData("136770", 43.9, 44.1, 46)]
    [InlineData("114308", 53.4, 44.1, 46)]
    [InlineData("137696", 45.2, 44.1, 46)]
    [InlineData("149251", 37.4, 44.1, 46)]
    [InlineData("114312", 55.2, 44.1, 46)]
    [InlineData("147122", 41.4, 44.1, 46)]
    [InlineData("136451", 52.2, 44.1, 46)]
    [InlineData("149739", 47.8, 44.1, 46)]
    [InlineData("147670", 51.5, 44.1, 46)]
    [InlineData("138075", 51.8, 44.1, 46)]
    [InlineData("137702", 46.4, 44.1, 46)]
    [InlineData("143583", 57.7, 44.1, 46)]
    [InlineData("148304", 44.5, 44.1, 46)]
    [InlineData("138172", 44.9, 44.1, 46)]
    public async Task AboutSchoolPage_LoadsSuccessfully(string urn, double expectedAttainmentSchool, double expectedAttainmentLA, double expectedAttainmentEngland)
    {
        // Arrange && Act
        var _ = await Page.GotoAsync(PageUrl(urn));
        var response = await ClickAcademicPerformanceLinkAsync();

        // Assert
        var schoolAttainment8 = await GetScoreAsync("attainment8-establishment-card", "The attainment 8 score for this school is");
        Assert.NotNull(schoolAttainment8);
        Assert.Equal(expectedAttainmentSchool.ToString("F1"), schoolAttainment8);
    }

    private Task<IResponse> ClickAcademicPerformanceLinkAsync()
    {
        var response = Page.RunAndWaitForResponseAsync(
            async () =>
            {
                await Page.GetByRole(AriaRole.Link, new() { Name = "Academic performance" }).ClickAsync();
            },
            response => response.Url.Contains("/academic-performance-attainment-and-progress") && response.Status == 200
        );
        return response;
    }

    public async Task<string> GetScoreAsync(string dataTestid, string textString)
    {
        var card = Page.Locator($"[data-testid='{dataTestid}']");
        var p = card.Locator("p.govuk-body", new() { HasTextString = textString });
        var input = await p.InnerTextAsync();
        var match = Regex.Matches(input, @"\d+(\.\d+)?")
                 .Cast<Match>()
                 .Last();

        return match.Value;
    }
}
