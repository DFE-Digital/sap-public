using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories.Generic;

namespace SAPPub.Web.Page.Tests;

[Collection("WebAppCollection")] // share the WebAppFixture across tests in this class so that we oly start the web app once for all tests in this collection

public class AboutTests : IDisposable
{
    private readonly MockAccessor<IGenericRepository<Establishment>> _accessor = new();
    private readonly WebAppFixture _fixture;

    private string BuildUrl(string urn, string schoolName)
    {
        var encodedSchoolName = Uri.EscapeDataString(schoolName);
        return $"/school/{urn}/{encodedSchoolName}/secondary/about";
    }

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
        var establishmentGenericRepositoryMock = _accessor.Get();
        establishmentGenericRepositoryMock?
            .Setup(repo => repo.ReadAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Establishment()
            {
                URN = urn,
                EstablishmentName = "Test School"
            });

        // Act
        var document = await _fixture.BrowseToPage(BuildUrl(urn, "Test School"));

        // Assert
        var h1 = document.QuerySelector("h1");
        Assert.Contains("About this project", h1?.TextContent.Trim());
    }
}
