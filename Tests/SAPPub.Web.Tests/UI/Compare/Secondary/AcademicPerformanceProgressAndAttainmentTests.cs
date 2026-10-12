using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.Compare.Secondary;

[Collection("Playwright Tests")]
public class AcademicPerformanceProgressAndAttainmentTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private string _pageUrl = "compare/secondary/pupil-attainment";

    [Fact]
    public async Task AcademicPerformanceProgressAndAttainmentTests_LoadsSuccessfully()
    {
        // Arrange
        var queryString = "urns=100279&urns=145179";

        // Act
        var response = await Page.GotoAsync($"{_pageUrl}?{queryString}");

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task AcademicPerformanceProgressAndAttainmentPage_AccessibilityNavigation()
    {
        // Arrange && Act
        var queryString = "urns=100279&urns=145179";
        await SkipContents_AccessibilityNavigation($"{_pageUrl}?{queryString}");

        // Assert
        var activeElement = await Page.EvaluateHandleAsync("document.activeElement");
        var prop = await activeElement.GetPropertyAsync("href");
        string href = await prop.JsonValueAsync<string>();
        Assert.Contains("/compare/secondary/pupil-attainment?urns=100279&urns=145179", href);
    }

    [Fact]
    public async Task AcademicPerformanceProgressAndAttainmentTests_IgnoresInvalidUrnsAndLoadsSuccessfully()
    {
        // Arrange
        var queryString = "urns=100279&urns=145179&urns=000000";

        // Act
        var response = await Page.GotoAsync($"{_pageUrl}?{queryString}");

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }
}