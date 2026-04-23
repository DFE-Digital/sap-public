using Microsoft.Extensions.DependencyInjection;
using Moq;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
using SAPPub.Core.ServiceModels.KS4.AboutSchool;

namespace SAPPub.Web.Page.Tests.Tests;

[Collection("WebAppCollection")] // share the WebAppFixture across tests in this class so that we oly start the web app once for all tests in this collection

public class AboutPageTests : IDisposable // implement IDisposable so can clear the mock accessor after each test
{
    private readonly MockAccessor<IAboutSchoolService> _accessor;
    private readonly WebAppFixture _fixture;

    private string BuildUrl(string urn, string schoolName)
    {
        // CML TODO: use our slugify method here instead of just URL encoding the school name
        var encodedSchoolName = Uri.EscapeDataString(schoolName);
        return $"/school/{urn}/{encodedSchoolName}/secondary/about";
    }

    public AboutPageTests(WebAppFixture fixture)
    {
        _fixture = fixture;
        _accessor = fixture.Factory.Services
            .GetRequiredService<MockAccessor<IAboutSchoolService>>();
        var mock = new Mock<IAboutSchoolService>();
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
        var aboutSchoolServiceMock = _accessor.Get();
        aboutSchoolServiceMock?
            .Setup(service => service.GetAboutSchoolDetailsAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AboutSchoolModel()
            {
                Urn = urn,
                SchoolName = establishmentName
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
        var aboutSchoolServiceMock = _accessor.Get();
        aboutSchoolServiceMock?
            .Setup(service => service.GetAboutSchoolDetailsAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AboutSchoolModel()
            {
                Urn = urn,
                SchoolName = establishmentName
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
