using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Bogus;
using Moq;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Destinations;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.Compare;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Compare.Secondary;

[Collection("WebAppCollection")]
public class DestinationsPageTests : PageTestsBase
{
    private string _pageUrl = "compare/secondary/destinations-after-year-11";
    private List<string> _urns = ["100279", "145179"];
    private string QueryString => string.Join("&", _urns.Select(urn => $"urns={urn}"));

    private readonly List<EstablishmentServiceModel> _establishments = new();
    private readonly DestinationsComparisonResultModel _destinationsResult;
    private readonly Mock<IEstablishmentService> _establishmentService = new();
    private readonly Mock<IDestinationsComparisonService> _destinationsService = new();

    public DestinationsPageTests(WebAppFixture fixture) : base(fixture)
    {
        _establishmentService = UseMock<IEstablishmentService>();
        _destinationsService = UseMock<IDestinationsComparisonService>();
        var destinationsDetails = new List<SchoolDestinationDetails>();
        foreach (var urn in _urns)
        {
            var establishment = new EstablishmentTestBuilder()
                        .WithURN(urn)
                        .WithEstablishmentName($"School {urn}")
                        .WithIsKeyStage4(true)
                        .WithSixthForm(true)
                        .BuildServiceModel();
            _establishments.Add(establishment);
            _establishmentService.Setup(s => s.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new EstablishmentServiceModel
                {
                    URN = urn,
                    EstablishmentName = $"School {urn}",
                    IsKS4 = true
                });
        }
        _destinationsResult = new DestinationsComparisonResultModelBuilder()
            .WithEnglandPercentage(75.1)
            .WithSchoolUrns(_urns)
            .Build();

        _destinationsService.Setup(s => s.GetDestinationsDetailsAsync(_urns, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_destinationsResult);
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
    public async Task Page_WithNoSpecialSchool_DoesNotShowShowsNotificationBanner()
    {
        // Act
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{QueryString}");

        var banner = doc.QuerySelector("[data-testid='special-school-warning-banner']");
        Assert.Null(banner);
    }

    [Fact]
    public async Task Page_WithSpecialSchool_ShowsNotificationBanner()
    {
        // Arrange
        _establishmentService.Setup(s => s.GetEstablishmentAsync("100279", It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new EstablishmentTestBuilder()
                    .WithURN("100279")
                    .WithIsKeyStage4(true)
                    .WithTypeOfEstablishmentId("7")
                    .BuildServiceModel());

        // Act
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{QueryString}");

        // Assert
        var banner = doc.QuerySelector("[data-testid='special-school-warning-banner']");
        Assert.NotNull(banner);
        Assert.Contains("You're comparing a special school", banner.TextContent.Trim());
    }

    [Theory]
    [InlineData("1")]
    [InlineData("2")]
    [InlineData("0")]
    [InlineData("9")]
    public async Task DestinationsPage_DisplaysExpectedSixthFormAvailableContent(string SixthFormCode)
    {
        //Arrange
        foreach (var urn in _urns)
        {
            var establishment = new EstablishmentTestBuilder()
                        .WithURN(urn)
                        .WithEstablishmentName($"School {urn}")
                        .WithIsKeyStage4(true)
                        .WithOfficialSixthFormId(SixthFormCode)
                        .BuildServiceModel();
            _establishments.Add(establishment);
            _establishmentService.Setup(s => s.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync(establishment);
        }

        _destinationsService.Setup(s => s.GetDestinationsDetailsAsync(_urns, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DestinationsComparisonResultModelBuilder()
                .WithEnglandPercentage(57.1)
                .WithSchoolDetails(new List<Action<SchoolDestinationDetailsBuilder>>
                {
                    builder => builder.WithUrn(_urns[0]).WithPercentInEducationEmploymentOrTraining(new Faker().Random.Double(5.0, 100.0)),
                    builder => builder.WithUrn(_urns[1]).WithPercentInEducationEmploymentOrTraining(new Faker().Random.Double(5.0, 100.0))
                })
                .Build());

        var expectedSixthFormValue = SixthFormCode switch
        {
            "1" => "Yes",
            "2" => "No",
            "9" => "Not available",
            "0" => "Not available",
            _ => "Not available"
        };

        // Act
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{QueryString}");

        // Assert
        var table = doc.QuerySelector<IHtmlTableElement>("[data-testid='sixth-form-table']");
        Assert.NotNull(table);
        Assert.Equal(expectedSixthFormValue, table.GetTableValueByRowHeader(_establishments[0].EstablishmentName));
    }

    [Fact]
    public async Task DestinationsPage_DestinationsDataAvailable_DisplaysExpectedSchoolComparisonTableContent()
    {
        // Act
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{QueryString}");

        // Assert
        var table = doc.QuerySelector<IHtmlTableElement>("#all-destinations-current-year-table");
        Assert.NotNull(table);
        Assert.Equal($"{_destinationsResult.SchoolDetails.ToList()[0].PercentInEducationEmploymentOrTraining}%", table.GetTableValueByRowHeader(_establishments[0].EstablishmentName));
        Assert.Equal($"{_destinationsResult.SchoolDetails.ToList()[1].PercentInEducationEmploymentOrTraining}%", table.GetTableValueByRowHeader(_establishments[1].EstablishmentName));
        Assert.Equal("75.1%", table.GetTableValueByRowHeader("England average"));
    }

    [Fact]
    public async Task DestinationsPage_DestinationsEnglandPercentageDataNotAvailable_DisplaysExpectedSchoolComparisonTableContent()
    {
        // Arrange
        var destinationDetails = new DestinationsComparisonResultModelBuilder()
            .WithEnglandPercentage(null)
            .WithSchoolUrns(_urns)
            .Build();
        _destinationsService.Setup(s => s.GetDestinationsDetailsAsync(It.IsAny<List<string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(destinationDetails);

        // Act
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{QueryString}");

        // Assert
        var table = doc.QuerySelector<IHtmlTableElement>("#all-destinations-current-year-table");
        Assert.NotNull(table);
        Assert.Equal("Not available", table.GetTableValueByRowHeader("England average"));
    }

    [Fact]
    public async Task DestinationsPage_DestinationsPercentageDataNotAvailable_DisplaysExpectedSchoolComparisonTableContent()
    {
        // Arrange
        var destinationDetails = new DestinationsComparisonResultModelBuilder()
           .WithEnglandPercentage(null)
           .WithSchoolDetails(new List<Action<SchoolDestinationDetailsBuilder>>
           {
                builder => builder.WithUrn(_urns[0]).WithPercentInEducationEmploymentOrTraining(null),
                builder => builder.WithUrn(_urns[1]).WithPercentInEducationEmploymentOrTraining(null)
           })
           .Build();
        _destinationsService.Setup(s => s.GetDestinationsDetailsAsync(It.IsAny<List<string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(destinationDetails);

        // Act
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{QueryString}");

        // Assert
        var table = doc.QuerySelector<IHtmlTableElement>("#all-destinations-current-year-table");
        Assert.NotNull(table);
        Assert.Equal("Not available", table.GetTableValueByRowHeader(_establishments[0].EstablishmentName));
        Assert.Equal("Not available", table.GetTableValueByRowHeader(_establishments[1].EstablishmentName));
        Assert.Equal("Not available", table.GetTableValueByRowHeader("England average"));
    }
}
