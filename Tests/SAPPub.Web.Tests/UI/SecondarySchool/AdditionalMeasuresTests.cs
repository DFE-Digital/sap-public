using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.SecondarySchool;

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
    public async Task AdditionalMeasuresPage_ShowsExpectedTableData()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Assert
        Assert.NotNull(response);
        var measuresData = Page.Locator("#additional-measures-table");
    }
}
