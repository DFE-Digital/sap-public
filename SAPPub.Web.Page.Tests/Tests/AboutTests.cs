using Microsoft.Extensions.DependencyInjection;
using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories.Generic;

namespace SAPPub.Web.Page.Tests.Tests;

[Collection("WebAppCollection")] // share the WebAppFixture across tests in this class so that we oly start the web app once for all tests in this collection

public class AboutTests : IDisposable // implement IDisposable so can clear the mock accessor after each test
{
    private readonly MockAccessor<IGenericRepository<Establishment>> _accessor;
    private readonly WebAppFixture _fixture;

    private string BuildUrl(string urn, string schoolName)
    {
        var encodedSchoolName = Uri.EscapeDataString(schoolName);
        return $"/school/{urn}/{encodedSchoolName}/secondary/about";
    }

    public AboutTests(WebAppFixture fixture)
    {
        _fixture = fixture;
        _accessor = fixture.Factory.Services
            .GetRequiredService<MockAccessor<IGenericRepository<Establishment>>>();
        var mock = new Mock<IGenericRepository<Establishment>>();
        _accessor.Set(mock);
    }

    public void Dispose()
    {
        _accessor.Clear();
    }

    [Fact]
    public async Task AboutPage_ShowsHeadingAndDetails()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "St Paul's Church of England Academy";
        var establishmentGenericRepositoryMock = _accessor.Get();
        establishmentGenericRepositoryMock?
            .Setup(repo => repo.ReadAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Establishment()
            {
                URN = urn,
                EstablishmentName = establishmentName
            });

        // Act
        var doc = await _fixture.BrowseToPage(BuildUrl(urn, establishmentName));

        // Assert
        var h1 = doc.QuerySelector("h1");
        Assert.Contains("About the school", h1?.TextContent.Trim());
        var summaryList = doc.QuerySelector("#school-details-summary");
        var rows = summaryList?.QuerySelectorAll(".govuk-summary-list__row");

        var nameRow = rows?.FirstOrDefault(r =>
                    r.QuerySelector("dt")?.TextContent.Trim() == "Name");

        Assert.Contains(establishmentName, nameRow?.QuerySelector("dd")?.TextContent.Trim());
    }

    [Fact]
    public async Task AboutPage_ShowsDetails()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "St David's Church of England Academy";
        var establishmentGenericRepositoryMock = _accessor.Get();
        establishmentGenericRepositoryMock?
            .Setup(repo => repo.ReadAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Establishment()
            {
                URN = urn,
                EstablishmentName = establishmentName
            });

        // Act
        var doc = await _fixture.BrowseToPage(BuildUrl(urn, establishmentName));

        // Assert
        var summaryList = doc.QuerySelector("#school-details-summary");
        var rows = summaryList?.QuerySelectorAll(".govuk-summary-list__row");

        var nameRow = rows?.FirstOrDefault(r =>
                    r.QuerySelector("dt")?.TextContent.Trim() == "Name");

        Assert.Contains(establishmentName, nameRow?.QuerySelector("dd")?.TextContent.Trim());
    }
}
