using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.Help;

[Collection("Playwright Tests")]
public class TermsAndConditionsPageTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private string _pageUrl = "terms-and-conditions";

    [Fact]
    public async Task TermsAndConditions_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(_pageUrl);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task AboutSchoolPage_HasCorrectTitle()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var title = await Page.TitleAsync();

        // Assert
        Assert.Contains("Terms and conditions", title);
    }

    [Fact]
    public async Task AboutSchoolPage_DisplaysMainHeading()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var heading = await Page.Locator("h1").TextContentAsync();

        // Assert
        Assert.NotNull(heading);
        Assert.NotEmpty(heading!.Trim());
    }
}
