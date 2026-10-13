using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.KS4;

[Collection("Playwright Tests")]
public class AdditionalMeasuresTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private Dictionary<string, string> _schoolUrnToUrlMap = new Dictionary<string, string>
    {
        ["105574"] = "school/105574/loreto-high-school-chorlton/secondary-performance/additional-measures",
        ["137020"] = "school/137020/west-hill-school/secondary-performance/additional-measures",
    };

    [Fact]
    public async Task AdditionalMeasuressPage_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task AdditionalMeasuressPage_KS4_AccessibilityNavigation()
    {
        // Arrange && Act
        await SkipContents_AccessibilityNavigation(_schoolUrnToUrlMap["105574"]);

        // Assert
        var activeElement = await Page.EvaluateHandleAsync("document.activeElement");
        var prop = await activeElement.GetPropertyAsync("href");
        string href = await prop.JsonValueAsync<string>();
        Assert.Contains("/school/105574/loreto-high-school-chorlton/secondary-performance/progress-attainment", href);
    }

    [Fact]
    public async Task AdditionalMeasuresPage_ShowsExpectedTableData()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Assert
        Assert.NotNull(response);
        var measuresData = Page.Locator("#additional-measures-table");
    }
}
