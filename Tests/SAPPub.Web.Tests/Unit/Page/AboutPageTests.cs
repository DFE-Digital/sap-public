using Moq;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
using SAPPub.Core.ServiceModels.KS4.AboutSchool;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page;

// Share the WebAppFixture across tests in this class so that we only start the web app once for all tests in this collection
// and run tests sequentially to avoid issues with shared state in the mock accessor.`
// The Mock accessor is cleared after each test, but if tests run in parallel, they could interfere with each other.
[Collection("WebAppCollection")]
public class AboutPageTests : PageTestsBase
{
    private static string _pageRoute = "/secondary/about";
    private readonly Mock<IAboutSchoolService> _about;

    public AboutPageTests(WebAppFixture fixture) : base(fixture)
    {
        // access the mocks needed by the controller endpoint being used
        _about = UseMock<IAboutSchoolService>();
    }

    [Fact]
    public async Task AboutPage_ShowsHeading()
    {
        // Arrange
        var urn = "143032";
        var establishmentName = "St Paul's Church of England Academy";

        // set up mock
        _about
            .Setup(service => service.GetAboutSchoolDetailsAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AboutSchoolModel()
            {
                Urn = urn,
                SchoolName = establishmentName
            });

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));

        // Assert
        var h1 = doc.QuerySelector("h1");
        Assert.Contains("About the school", h1?.TextContent.Trim());
    }

    [Fact]
    public async Task AboutPage_ShowsDetails()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "St David's Church of England Academy";

        _about
            .Setup(service => service.GetAboutSchoolDetailsAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AboutSchoolModel()
            {
                Urn = urn,
                SchoolName = establishmentName
            });

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));

        // Assert
        var nameRowContent = doc.GetRowContentByIdAndKey("school-details-summary", "Name");
        Assert.Contains(establishmentName, nameRowContent);
    }
}
