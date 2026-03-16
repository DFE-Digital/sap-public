using SAPPub.Web.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.Error;

[Collection("Playwright Tests")]

public class ErrorPageTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{

    [Fact]
    public async Task Should_Show_PageNotFound_Page()
    {
        // Arrange
        var pageUrl = "xyz";
        var response = await Page.GotoAsync(pageUrl);

        // Act
        var heading = await Page.Locator("h1").TextContentAsync();
        var link = Page.GetByTestId("link");

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(heading);
        Assert.Contains("Page not found", heading);

        Assert.True(await link.IsVisibleAsync());

        var text = await link.TextContentAsync();
        Assert.NotNull(text);

        var href = await link.GetAttributeAsync("href");
        Assert.Equal($"/{RouteConstants.Search.ToLower()}", href);
    }

    [Fact]
    public async Task Should_Show_404_Page()
    {
        // Arrange
        var pageUrl = "/error/404";

        // Act
        var response = await Page.GotoAsync(pageUrl);

        var heading = await Page.Locator("h1").TextContentAsync();

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(heading);
        Assert.NotEmpty(heading!.Trim());
        Assert.Contains("Page not found", heading);
    }

    [Fact]
    public async Task Show_Show_ProblemWithService_Page()
    {
        // Arrange
        var pageUrl = "error/throw";

        // Act
        var response = await Page.GotoAsync(pageUrl);

        var heading = await Page.Locator("h1").TextContentAsync();

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(heading);
        Assert.NotEmpty(heading!.Trim());
        Assert.Contains("Sorry, there is a problem with the service", heading);
    }
}
