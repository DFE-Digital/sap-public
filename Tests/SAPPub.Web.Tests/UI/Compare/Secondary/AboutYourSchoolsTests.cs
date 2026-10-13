using Microsoft.Playwright;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.Compare.Secondary;

[Collection("Playwright Tests")]
public class AboutYourSchoolsTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private string _pageUrl = "compare/secondary/about-your-schools";

    [Fact]
    public async Task AboutYourSchoolsPage_LoadsSuccessfully()
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
    public async Task AboutYourSchoolsPage_AccessibilityNavigation()
    {
        // Arrange && Act
        var queryString = "urns=105574&urns=137020";
        await SkipContents_AccessibilityNavigation($"{_pageUrl}?{queryString}");

        // Assert
        var activeElement = await Page.EvaluateHandleAsync("document.activeElement");
        var prop = await activeElement.GetPropertyAsync("id");
        string id = await prop.JsonValueAsync<string>();
        Assert.Equal("map", id);
    }
}
