using Microsoft.Extensions.DependencyInjection;
using Moq;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
using SAPPub.Core.ServiceModels.KS4.AboutSchool;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page;

// Share the WebAppFixture across tests in this class so that we oly start the web app once for all tests in this collection
// and run tests sequentially to avoid issues with shared state in the mock accessor.`
// The Mock accessor is cleared after each test, but if tests run in parallel, they could interfere with each other.
[Collection("WebAppCollection")]

public class AboutPageTests : PageTestsBase, IDisposable // implement IDisposable so can clear the mock accessor after each test
{
    private static string _pageRoute = "/secondary/about";
    private readonly MockAccessor<IAboutSchoolService> _accessor;
    private readonly WebAppFixture _fixture;

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
        var urn = "143032";
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
        var doc = await _fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));

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
        var doc = await _fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));

        // Assert
        var summaryList = doc.QuerySelector("#school-details-summary");
        var rows = summaryList?.QuerySelectorAll(".govuk-summary-list__row");

        var nameRow = rows?.FirstOrDefault(r =>
                    r.QuerySelector("dt")?.TextContent.Trim() == "Name");

        Assert.Contains(establishmentName, nameRow?.QuerySelector("dd")?.TextContent.Trim());
    }
}
