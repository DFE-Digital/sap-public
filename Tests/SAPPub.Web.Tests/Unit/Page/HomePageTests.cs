using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page;

[Collection("WebAppCollection")] // share the WebAppFixture across tests in this class so that we start the web app once for all tests in this collection
public class HomePageTests : PageTestsBase
{
    public HomePageTests(WebAppFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task HomePage_ShowsHeading()
    {
        // Act
        var document = await Fixture.BrowseToPage("/");

        // Assert
        var h1 = document.QuerySelector("h1");
        Assert.Contains("School Profiles", h1?.TextContent.Trim());
    }
}
