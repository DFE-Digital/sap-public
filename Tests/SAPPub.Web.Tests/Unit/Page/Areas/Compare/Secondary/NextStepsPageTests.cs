using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Compare.Secondary;

[Collection("WebAppCollection")]
public class NextStepsPageTests : PageTestsBase
{
    private string _pageUrl = "compare/secondary/next-steps?urns=119052&urns=124500";
    private readonly Mock<IEstablishmentService> _establishmentService;

    public NextStepsPageTests(WebAppFixture fixture) : base(fixture)
    {
        // set up the mock establishment service to return establishments for the URNs in the query string
        // this is used by the page validation filter to determine if the establishments are secondary and should be compared
        _establishmentService = UseMock<IEstablishmentService>();
        var establishmentList = (new List<Establishment>
        {
            new EstablishmentTestBuilder().WithURN("119052").WithIsKeyStage4(true).Build(),
            new EstablishmentTestBuilder().WithURN("124500").WithIsKeyStage4(true).Build(),
        }).ToList();

        establishmentList.Select(e =>
            _establishmentService.Setup(s =>
            s.GetEstablishmentAsync(e.URN, It.IsAny<CancellationToken>())).ReturnsAsync(e)).ToList();
    }

    [Fact]
    public async Task NextSteps_Displays_VerticalNavigation()
    {
        // Act
        var doc = await Fixture.BrowseToPage(_pageUrl);

        // Assert
        Assert.NotNull(doc.QuerySelector(".moj-side-navigation"));
        Assert.Equal(4, doc.QuerySelectorAll(".moj-side-navigation__item").Length);
        Assert.Single(doc.QuerySelectorAll(".moj-side-navigation__item--active"));
    }

    [Fact]
    public async Task NextSteps_Show_ExploreMoreInformation_Header()
    {
        // Act
        var doc = await Fixture.BrowseToPage(_pageUrl);
        var h2Header = doc.QuerySelector("h2");

        Assert.NotNull(h2Header);
    }
}
