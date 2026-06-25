using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Compare.Secondary;

[Collection("WebAppCollection")]
public class DestinationsPageTests : PageTestsBase
{
    private string _pageUrl = "compare/secondary/destinations-after-year-11";
    private List<string> _urns = ["100279", "145179"];
    private string QueryString => string.Join("&", _urns.Select(urn => $"urns={urn}"));

    private readonly List<Establishment> _establishments = new();
    private readonly Mock<IEstablishmentService> _establishmentService = new();
    private readonly List<DestinationsDetails> _destinationsDetails = new();
    private readonly Mock<IDestinationsService> _destinationsService = new();

    public DestinationsPageTests(WebAppFixture fixture) : base(fixture)
    {
        _establishmentService = UseMock<IEstablishmentService>();
        _destinationsService = UseMock<IDestinationsService>();
        foreach (var urn in _urns)
        {
            var establishment = new EstablishmentTestBuilder()
                        .WithURN(urn)
                        .WithEstablishmentName($"School {urn}")
                        .WithIsKeyStage4(true)
                        .WithSixthForm(true)
                        .Build();
            _establishments.Add(establishment);
            _establishmentService.Setup(s => s.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync(establishment);

            var destinationsDetails = new DestinationsDetailsBuilder()
                        .WithUrn(urn)
                        .WithEstablishmentName($"School {urn}")
                        .WithEnglandPercentage(75.1)
                        .Build();
            _destinationsDetails.Add(destinationsDetails);
            _destinationsService.Setup(s => s.GetDestinationsDetailsAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync(destinationsDetails);
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

    [Fact]
    public async Task DestinationsPage_DestinationsDataAvailable_DisplaysExpectedSchoolComparisonTableContent()
    {
        // Act
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{QueryString}");

        // Assert
        var table = doc.QuerySelector<IHtmlTableElement>("#all-destinations-current-year-table");
        Assert.NotNull(table);
        Assert.Equal($"{_destinationsDetails[0].SchoolAll.CurrentYear}%", table.GetTableValueByRowHeader(_destinationsDetails[0].SchoolName));
        Assert.Equal($"{_destinationsDetails[1].SchoolAll.CurrentYear}%", table.GetTableValueByRowHeader(_destinationsDetails[1].SchoolName));
        Assert.Equal("75.1%", table.GetTableValueByRowHeader("England average"));
    }

    [Fact]
    public async Task DestinationsPage_DestinationsPercentageDataNotAvailable_DisplaysExpectedSchoolComparisonTableContent()
    {
        // Arrange
        foreach (var urn in _urns)
        {
            var destinationsDetails = new DestinationsDetailsBuilder()
            .WithUrn(urn)
            .WithEstablishmentName($"School {urn}")
            .WithEnglandPercentage(null)
            .Build();
            _destinationsDetails.Add(destinationsDetails);
            _destinationsService.Setup(s => s.GetDestinationsDetailsAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync(destinationsDetails);
        }

        // Act
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{QueryString}");

        // Assert
        var table = doc.QuerySelector<IHtmlTableElement>("#all-destinations-current-year-table");
        Assert.NotNull(table);
        //Assert.Equal("Not available", table.GetTableValueByRowHeader(_destinationsDetails[0].SchoolName));
        //Assert.Equal("Not available", table.GetTableValueByRowHeader(_destinationsDetails[1].SchoolName));
        Assert.Equal("Not available", table.GetTableValueByRowHeader("England average"));
    }

    [Fact]
    public async Task DestinationsPage_DestinationsEnglandPercentageDataNotAvailable_DisplaysExpectedSchoolComparisonTableContent()
    {
        // Arrange
        foreach (var urn in _urns)
        {
            var destinationsDetails = new DestinationsDetailsBuilder()
            .WithUrn(urn)
            .WithEstablishmentName($"School {urn}")
            .BuildResultsNotAvailable();
            _destinationsDetails.Add(destinationsDetails);
            _destinationsService.Setup(s => s.GetDestinationsDetailsAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync(destinationsDetails);
        }

        // Act
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{QueryString}");

        // Assert
        var table = doc.QuerySelector<IHtmlTableElement>("#all-destinations-current-year-table");
        Assert.NotNull(table);
        Assert.Equal("Not available", table.GetTableValueByRowHeader(_destinationsDetails[0].SchoolName));
        Assert.Equal("Not available", table.GetTableValueByRowHeader(_destinationsDetails[1].SchoolName));
        Assert.Equal("Not available", table.GetTableValueByRowHeader("England average"));
    }
}
