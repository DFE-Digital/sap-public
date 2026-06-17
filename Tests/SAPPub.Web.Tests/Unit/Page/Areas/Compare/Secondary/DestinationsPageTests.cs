using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Compare.Secondary;

[Collection("WebAppCollection")]
public class DestinationsPageTests(WebAppFixture fixture) : PageTestsBase(fixture)
{
    private string _pageUrl = "compare/secondary/destinations-after-year-11?urns=119052&urns=124500";
    private readonly Mock<IEstablishmentService> _establishmentService = new();

    [Fact]
    public async Task DestinationsPage_HasCorrectTitle()
    {
        // Arrange
        var urns = new List<string> { "100279", "145179" };
        var queryString = string.Join("&", urns.Select(urn => $"urns={urn}"));
        foreach (var urn in urns)
        {
            _establishmentService.Setup(s => s.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Establishment { URN = urn, EstablishmentName = $"School {urn}" });
        }

        // Act
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{queryString}");

        // Assert
        var title = doc.QuerySelector("title");
        Assert.NotNull(title);
        Assert.Contains("Destinations after year 11", title.TextContent.Trim());
    }

    [Fact]
    public async Task DestinationsPage_DisplaysMainHeading()
    {
        // Arrange
        var urns = new List<string> { "100279", "145179" };
        var queryString = string.Join("&", urns.Select(urn => $"urns={urn}"));
        foreach (var urn in urns)
        {
            _establishmentService.Setup(s => s.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Establishment { URN = urn, EstablishmentName = $"School {urn}" });
        }

        // Act
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{queryString}");


        // Assert
        var heading = doc.QuerySelector("h1");
        Assert.NotNull(heading);
        Assert.Contains("Destinations after year 11", heading.TextContent.Trim());
    }

    [Fact]
    public async Task DestinationsPage_Displays_VerticalNavigation()
    {
        //Arrange
        var urns = new List<string> { "100279", "145179" };
        var queryString = string.Join("&", urns.Select(urn => $"urns={urn}"));
        foreach (var urn in urns)
        {
            _establishmentService.Setup(s => s.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Establishment { URN = urn, EstablishmentName = $"School {urn}" });
        }

        // Act
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{queryString}");

        // Assert
        Assert.NotNull(doc.QuerySelector(".moj-side-navigation"));
        Assert.Equal(7, doc.QuerySelectorAll(".moj-side-navigation__item").Length);
        Assert.Single(doc.QuerySelectorAll(".moj-side-navigation__item--active"));
    }
}
