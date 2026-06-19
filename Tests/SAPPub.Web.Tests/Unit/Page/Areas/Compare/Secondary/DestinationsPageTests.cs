using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Compare.Secondary;

[Collection("WebAppCollection")]
public class DestinationsPageTests : PageTestsBase
{
    private string _pageUrl = "compare/secondary/destinations-after-year-11";
    private List<string> _urns = new List<string> { "100279", "145179" };
    private string QueryString => string.Join("&", _urns.Select(urn => $"urns={urn}"));
    private readonly Mock<IEstablishmentService> _establishmentService = new();

    public DestinationsPageTests(WebAppFixture fixture) : base(fixture)
    {
        _establishmentService = UseMock<IEstablishmentService>();
        foreach (var urn in _urns)
        {
            _establishmentService.Setup(s => s.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Establishment
                {
                    URN = urn,
                    EstablishmentName = $"School {urn}",
                    IsKS4 = true
                });
        }
    }

    [Fact]
    public async Task DestinationsPage_HasCorrectTitle()
    {
        // Arrange

        // Act
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{QueryString}");

        // Assert
        var title = doc.QuerySelector("title");
        Assert.NotNull(title);
        Assert.Contains("Destinations after year 11", title.TextContent.Trim());
    }

    [Fact]
    public async Task DestinationsPage_DisplaysMainHeading()
    {
        // Arrange

        // Act
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{QueryString}");


        // Assert
        var heading = doc.QuerySelector("h1");
        Assert.NotNull(heading);
        Assert.Contains("Destinations after year 11", heading.TextContent.Trim());
    }

    [Fact]
    public async Task DestinationsPage_Displays_VerticalNavigation()
    {
        //Arrange

        // Act
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{QueryString}");

        // Assert
        Assert.NotNull(doc.QuerySelector(".moj-side-navigation"));
        Assert.Equal(4, doc.QuerySelectorAll(".moj-side-navigation__item").Length);
        Assert.Single(doc.QuerySelectorAll(".moj-side-navigation__item--active"));
    }
}
