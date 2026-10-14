using Microsoft.Playwright;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.Compare.Secondary;

[Collection("Playwright Tests")]
public class NextStepsTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private string _pageUrl = "compare/secondary/next-steps";

    [Fact]
    public async Task NextStepsPage_LoadsSuccessfully()
    {
        // Arrange
        var queryString = "urns=105574&urns=137020";

        // Act
        var response = await Page.GotoAsync($"{_pageUrl}?{queryString}");

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task NextStepsPage_AccessibilityNavigation()
    {
        // Arrange && Act
        var queryString = "urns=105574&urns=137020";
        await SkipContents_AccessibilityNavigation($"{_pageUrl}?{queryString}");

        // Assert
        var activeElement = await Page.EvaluateHandleAsync("document.activeElement");
        var prop = await activeElement.GetPropertyAsync("href");
        string href = await prop.JsonValueAsync<string>();
        Assert.Contains("/school/105574/SAPPub.Web.Areas.Compare.ViewModels.Secondary.CompareNextStepsModel", href);
    }
}
