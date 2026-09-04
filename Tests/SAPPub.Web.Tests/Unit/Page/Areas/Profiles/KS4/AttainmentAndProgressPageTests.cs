using Moq;
using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Areas.Profiles.Helpers;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Profiles.KS4;

[Collection("WebAppCollection")]
public class AttainmentAndProgressPageTests : PageTestsBase
{
    private static string _pageRoute = "/secondary-performance/progress-attainment";
    private readonly Mock<IAttainmentAndProgressService> _serviceMock;

    public AttainmentAndProgressPageTests(WebAppFixture fixture) : base(fixture)
    {
        _serviceMock = UseMock<IAttainmentAndProgressService>();
    }

    [Fact]
    public async Task AcademicPerformanceAttainmentAndProgressPage_HasCorrectTitle()
    {
        // Arrange
        var expected = new AttainmentAndProgressModelBuilder()
            .Build();
        var urn = expected.Urn;
        var establishmentName = expected.SchoolName;
        _serviceMock
            .Setup(service => service.GetAttainmentAndProgressAsync(
                It.IsAny<string>(),
                     It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);


        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName!, $"{_pageRoute}/{AcademicYearSelection.Current.ToRouteSegment()}"));

        // Assert
        var title = doc.Title;
        Assert.Contains($"{establishmentName} - Secondary Progress and attainment - School Profiles - GOV.UK", title);
    }

    [Fact]
    public async Task ShowsAttainmentValues()
    {
        // Arrange
        var expected = new AttainmentAndProgressModelBuilder()
            .WithAttainment8Data()
            .Build();
        var urn = expected.Urn;
        var establishmentName = expected.SchoolName;
        _serviceMock
            .Setup(service => service.GetAttainmentAndProgressAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName!, $"{_pageRoute}/{AcademicYearSelection.Current.ToRouteSegment()}"));

        // Assert
        var schoolAttainmentCard = doc.QuerySelector("[data-testid='attainment8-establishment-card']");
        var text = schoolAttainmentCard?.QuerySelector("p")?.TextContent.Trim();
        Assert.Contains(expected.EstablishmentAttainment8Score.CurrentYear.ToString(), text);
    }

    [Fact]
    public async Task ShowsProgress8Values()
    {
        // Arrange
        var expected = new AttainmentAndProgressModelBuilder()
            .WithAttainment8Data()
            .WithEstablishmentProgress8Data()
            .WithLaProgressData()
            .Build();
        var urn = expected.Urn;
        var establishmentName = expected.SchoolName;
        _serviceMock
            .Setup(service => service.GetAttainmentAndProgressAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName!, $"{_pageRoute}/{AcademicYearSelection.Previous.ToRouteSegment()}"));

        // Assert
        var schoolProgressCard = doc.QuerySelector("[data-testid='progress8-establishment-card']");
        var text = schoolProgressCard?.QuerySelector("p")?.TextContent.Trim();
        Assert.Contains(expected.EstablishmentProgress8Score.CurrentYear.ToString(), text);
    }

    [Theory]
    [InlineData(AcademicYearSelection.Previous)]
    [InlineData(AcademicYearSelection.Previous2)]
    public async Task NoProgress8DataForSchool_ShowsNoProgress8Content(AcademicYearSelection yearSelection) // progress data not available for this school (non-covid year)
    {
        // Arrange
        var expected = new AttainmentAndProgressModelBuilder()
            .Build();
        var urn = expected.Urn;
        var establishmentName = expected.SchoolName;
        _serviceMock
            .Setup(service => service.GetAttainmentAndProgressAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName!, $"{_pageRoute}/{yearSelection.ToRouteSegment()}"));

        // Assert
        Assert.NotNull(doc.QuerySelector("[data-testid='progress8-no-establishment-data-card']"));
        Assert.Null(doc.QuerySelector("[data-testid='progress8-custom-card']"));
    }

    [Theory]
    [InlineData(AcademicYearSelection.Current)]
    [InlineData(AcademicYearSelection.Previous)]
    [InlineData(AcademicYearSelection.Previous2)]
    public async Task NoAttainment8DataForSchool_ShowsNoAttainment8Content(AcademicYearSelection yearSelection) // progress data not available for this school (non-covid year)
    {
        // Arrange
        var expected = new AttainmentAndProgressModelBuilder()
            .Build();
        var urn = expected.Urn;
        var establishmentName = expected.SchoolName;
        _serviceMock
            .Setup(service => service.GetAttainmentAndProgressAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName!, $"{_pageRoute}/{yearSelection.ToRouteSegment()}"));

        // Assert
        Assert.NotNull(doc.QuerySelector("[data-testid='attainment8-no-establishment-data-card']"));
        Assert.Null(doc.QuerySelector("[data-testid='attainment8-establishment-card']"));
    }

    [Fact]
    public async Task NoProgress8DataForCurrentYear_ShowsNoProgress8ForCurrentYearContent() // content for covid years
    {
        // Arrange
        var expected = new AttainmentAndProgressModelBuilder()
            .WithAttainment8Data()
            .Build();
        var urn = expected.Urn;
        var establishmentName = expected.SchoolName;
        _serviceMock
            .Setup(service => service.GetAttainmentAndProgressAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName!, $"{_pageRoute}/{AcademicYearSelection.Current.ToRouteSegment()}"));

        // Assert
        Assert.NotNull(doc.QuerySelector("[data-testid='progress8-custom-card']"));
        Assert.Null(doc.QuerySelector("[data-testid='progress8-no-establishment-data-card']"));
    }

    [Fact]
    public async Task AcademicPerformanceAttainmentAndProgressPage_DisplaysBottomPagination_WithCorrectDestinations()
    {
        // Arrange
        var expected = new AttainmentAndProgressModelBuilder()
            .WithAttainment8Data()
            .Build();
        var urn = expected.Urn;
        var establishmentName = expected.SchoolName;
        _serviceMock
            .Setup(service => service.GetAttainmentAndProgressAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName!, $"{_pageRoute}/{AcademicYearSelection.Current.ToRouteSegment()}"));

        // Assert
        var pagination = doc.QuerySelector("nav.govuk-pagination");
        Assert.NotNull(pagination);

        var previousLink = pagination.QuerySelector(".govuk-pagination__prev a");
        var nextLink = pagination.QuerySelector(".govuk-pagination__next a");

        Assert.NotNull(previousLink);
        Assert.Contains("/attendance", previousLink.GetAttribute("href"));

        Assert.NotNull(nextLink);
        Assert.Contains("/secondary-performance/english-and-maths", nextLink.GetAttribute("href"));
    }
}
