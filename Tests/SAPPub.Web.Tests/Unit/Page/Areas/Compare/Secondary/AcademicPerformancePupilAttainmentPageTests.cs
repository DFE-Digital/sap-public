using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Moq;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.Compare;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Compare.Secondary;

[Collection("WebAppCollection")]
public class AcademicPerformancePupilAttainmentPageTests : PageTestsBase
{
    private string _pageUrl = "compare/secondary/pupil-attainment";
    private List<string> _urns = ["100279", "145179"];
    private string QueryString => string.Join("&", _urns.Select(urn => $"urns={urn}"));
    private readonly double _englandAverage = 65.7;
    private readonly List<EstablishmentServiceModel> _establishments = new();
    private readonly AttainmentAndProgressComparisonResultsModel _attainmentsResult;
    private readonly Mock<IEstablishmentService> _establishmentService = new();
    private readonly Mock<IAttainmentAndProgressComparisionService> _attainmentAndProgressComparisionServiceService = new();

    public AcademicPerformancePupilAttainmentPageTests(WebAppFixture fixture) : base(fixture)
    {
        _establishmentService = UseMock<IEstablishmentService>();
        _attainmentAndProgressComparisionServiceService = UseMock<IAttainmentAndProgressComparisionService>();
        var attainmentDetails = new List<SchoolAttainmentAndProgressDetails>();
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
                .ReturnsAsync(establishment);
        }
        _attainmentsResult = new AttainmentAndProgressComparisonResultModelBuilder()
            .WithEnglandPercentage(_englandAverage)
            .WithSchoolUrns(_urns)
            .Build();

        _attainmentAndProgressComparisionServiceService.Setup(s => s.GetComparisionResultsAsync(_urns, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_attainmentsResult);
    }

    [Fact]
    public async Task AcademicPerformancePupilAttainmentPage_HasCorrectTitle()
    {
        // Arrange

        // Act
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{QueryString}");

        // Assert
        var title = doc.QuerySelector("title");
        Assert.NotNull(title);
        Assert.Contains("Pupil attainment", title.TextContent.Trim());
    }

    [Fact]
    public async Task AcademicPerformancePupilAttainmentPage_DisplaysMainHeading()
    {
        // Arrange

        // Act
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{QueryString}");


        // Assert
        var heading = doc.QuerySelector("h1");
        Assert.NotNull(heading);
        Assert.Contains("Academic Performance", heading.TextContent.Trim());
    }

    [Fact]
    public async Task AcademicPerformancePupilAttainmentPage_Displays_VerticalNavigation()
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
    public async Task AcademicPerformancePupilAttainmentPage_Displays_Sub_Navigation()
    {
        // Act
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{QueryString}");

        // Assert
        Assert.NotNull(doc.QuerySelector("#sub-navigation-academic-performance"));
    }

    [Fact]
    public async Task AcademicPerformancePupilAttainmentPage_Has_Correct_Sub_Navigation_Links()
    {
        // Act
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{QueryString}");
        var container = doc.QuerySelector("#sub-navigation-academic-performance");
        var links = container?.QuerySelectorAll(".moj-sub-navigation__link");

        Assert.NotNull(links);
        Assert.Equal(2, links.Length);
        Assert.Equal($"/compare/secondary/pupil-attainment?urns={_urns[0]}&urns={_urns[1]}", links[0].GetAttribute("href"));
        Assert.Equal($"/compare/secondary/english-and-maths-results?urns={_urns[0]}&urns={_urns[1]}", links[1].GetAttribute("href"));
    }

    [Fact]
    public async Task AcademicPerformancePupilAttainmentPage_Displays_Attainment8_Details()
    {
        // Arrange & Act
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{QueryString}");

        var attainmemtDetails = doc.QuerySelector("#details-attainment8");

        // Assert
        Assert.NotNull(attainmemtDetails);
    }

    [Fact]
    public async Task AcademicPerformancePupilAttainmentPage_AttainmentsDataAvailable_DisplaysExpectedSchoolComparisonTableContent()
    {
        // Act
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{QueryString}");

        // Assert
        var table = doc.QuerySelector<IHtmlTableElement>("#attainment-table");
        Assert.NotNull(table);
        Assert.Equal(_attainmentsResult.SchoolDetails.ToList()[0].Attainment8Score.ToString(), table.GetTableValueByRowHeader(_establishments[0].EstablishmentName));
        Assert.Equal(_attainmentsResult.SchoolDetails.ToList()[1].Attainment8Score.ToString(), table.GetTableValueByRowHeader(_establishments[1].EstablishmentName));
        Assert.Equal(_englandAverage.ToString(), table.GetTableValueByRowHeader("England average"));
    }

    [Fact]
    public async Task AcademicPerformancePupilAttainmentPage_AttainmentsEnglandPercentageDataNotAvailable_DisplaysExpectedSchoolComparisonTableContent()
    {
        // Arrange
        var attainmentDetails = new AttainmentAndProgressComparisonResultModelBuilder()
            .WithEnglandPercentage(null)
            .WithSchoolUrns(_urns)
            .Build();

        _attainmentAndProgressComparisionServiceService.Setup(s => s.GetComparisionResultsAsync(It.IsAny<List<string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(attainmentDetails);

        // Act
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{QueryString}");

        // Assert
        var table = doc.QuerySelector<IHtmlTableElement>("#attainment-table");
        Assert.NotNull(table);
        Assert.Equal("Not available", table.GetTableValueByRowHeader("England average"));
    }

    [Fact]
    public async Task AcademicPerformancePupilAttainmentPage_AttainmentsPercentageDataNotAvailable_DisplaysExpectedSchoolComparisonTableContent()
    {
        // Arrange
        var attainmentDetails = new AttainmentAndProgressComparisonResultModelBuilder()
           .WithEnglandPercentage(null)
           .WithSchoolDetails(new List<Action<SchoolAttainmentAndProgressDetailsBuilder>>
           {
                builder => builder.WithUrn(_urns[0]).WithAttainment8Score(null),
                builder => builder.WithUrn(_urns[1]).WithAttainment8Score(null)
           })
           .Build();

        _attainmentAndProgressComparisionServiceService.Setup(s => s.GetComparisionResultsAsync(It.IsAny<List<string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(attainmentDetails);

        // Act
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{QueryString}");

        // Assert
        var table = doc.QuerySelector<IHtmlTableElement>("#attainment-table");
        Assert.NotNull(table);
        Assert.Equal("Not available", table.GetTableValueByRowHeader(_establishments[0].EstablishmentName));
        Assert.Equal("Not available", table.GetTableValueByRowHeader(_establishments[1].EstablishmentName));
        Assert.Equal("Not available", table.GetTableValueByRowHeader("England average"));
    }

    [Fact]
    public async Task AcademicPerformancePupilAttainmentPage_DisplaysPagination()
    {
        // Arrange
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{QueryString}");

        // Act
        var nav = doc.QuerySelector("#academic-performance-attainment-pagination");
        var navNext = doc.QuerySelector("#academic-performance-attainment-pagination .govuk-pagination__next a");
        var navPrevious = doc.QuerySelector("#academic-performance-attainment-pagination .govuk-pagination__prev a");

        Assert.NotNull(nav);
        Assert.NotNull(navNext);
        Assert.NotNull(navPrevious);
        Assert.Contains("About your schools", navPrevious.TextContent);
        Assert.Contains("Academic performance: English and maths results", navNext.TextContent);
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
    public async Task Page_WithSpecialSchoolAndNonSpecialSchool_ShowsNotificationBanner()
    {
        // Arrange
        _establishmentService.Setup(s => s.GetEstablishmentAsync("100279", It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new EstablishmentTestBuilder()
                    .WithURN("100279")
                    .WithIsKeyStage4(true)
                    .WithTypeOfEstablishmentId(7)
                    .BuildServiceModel());

        // Act
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{QueryString}");

        // Assert
        var banner = doc.QuerySelector("[data-testid='special-school-warning-banner']");
        Assert.NotNull(banner);
        Assert.Contains("You're comparing a special school", banner.TextContent.Trim());
    }

    [Fact]
    public async Task Page_WithOnlySpecialSchools_DoesNotShowNotificationBanner()
    {
        // Arrange
        _establishmentService.Setup(s => s.GetEstablishmentAsync("100279", It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new EstablishmentTestBuilder()
                    .WithURN("100279")
                    .WithIsKeyStage4(true)
                    .WithTypeOfEstablishmentId(7)
                    .BuildServiceModel());
        _establishmentService.Setup(s => s.GetEstablishmentAsync("145179", It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new EstablishmentTestBuilder()
                    .WithURN("145179")
                    .WithIsKeyStage4(true)
                    .WithTypeOfEstablishmentId(7)
                    .BuildServiceModel());

        // Act
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{QueryString}");

        // Assert
        var banner = doc.QuerySelector("[data-testid='special-school-warning-banner']");
        Assert.Null(banner);
    }
}
