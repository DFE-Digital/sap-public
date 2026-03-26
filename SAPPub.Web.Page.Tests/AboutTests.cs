using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories.Generic;

namespace SAPPub.Web.Page.Tests;

[Collection("WebAppCollection")] // share the WebAppFixture across tests in this class so that we oly start the web app once for all tests in this collection

public class AboutTests : IDisposable
{
    private readonly MockAccessor<IGenericRepository<Establishment>> _accessor = new();
    private readonly WebAppFixture _fixture;

    private string Url => "/school/{urn}/{school-name}/secondary/about";

    public AboutTests(WebAppFixture fixture)
    {
        _fixture = fixture;
        //_accessor = fixture.Factory.Services
        //    .GetRequiredService<MockAccessor<IGenericRepository<Establishment>>>();
        var mock = new Mock<IGenericRepository<Establishment>>();
        _accessor.Set(mock);
    }

    public void Dispose()
    {
        _accessor.Clear();
    }

    [Fact]
    public async Task AboutPage_ShowsHeading()
    {
        // Arrange
        var urn = "112345";
        var establishmentGenericRepositoryMock = _accessor.GetOrDefault();
        establishmentGenericRepositoryMock?
            .Setup(repo => repo.ReadSingleAsync(
                It.IsAny<object>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Establishment());

        // Act
        var document = await _fixture.BrowseToPage("/about");
        // Assert
        var h1 = document.QuerySelector("h1");
        Assert.Contains("About this project", h1?.TextContent.Trim());
    }
}
