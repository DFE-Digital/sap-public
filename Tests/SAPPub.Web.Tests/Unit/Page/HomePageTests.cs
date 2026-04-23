using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Web.Page.Tests.Tests.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page;

[Collection("WebAppCollection")] // share the WebAppFixture across tests in this class so that we oly start the web app once for all tests in this collection
public class HomePageTests : IDisposable //implement IDisposable so that we can clear the mock accessor after each test, to ensure a clean slate for the next test
{
    private readonly MockAccessor<IGenericRepository<Establishment>> _accessor = new();
    private readonly WebAppFixture _fixture;

    public HomePageTests(WebAppFixture fixture)
    {
        _fixture = fixture;
    }

    public void Dispose()
    {
    }

    [Fact]
    public async Task HomePage_ShowsHeading()
    {
        // Act
        var document = await _fixture.BrowseToPage("/");

        // Assert
        var h1 = document.QuerySelector("h1");
        Assert.Contains("School profiles", h1?.TextContent.Trim());
    }
}
