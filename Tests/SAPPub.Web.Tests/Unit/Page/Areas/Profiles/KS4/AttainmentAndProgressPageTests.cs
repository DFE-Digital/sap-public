using Moq;
using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.ServiceModels.KS4.Performance;
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
        var urn = "143034";
        var establishmentName = "Loreto High School Chorlton";
        _serviceMock
            .Setup(service => service.GetAttainmentAndProgressAsync(
                It.IsAny<string>(),
                It.IsAny<AcademicYearSelection>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AttainmentAndProgressModel()
            {
                Urn = urn,
                SchoolName = establishmentName,
                IsKS2 = false,
                IsKS4 = true,
                IsKS5 = false
            });


        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, $"{_pageRoute}/{AcademicYearSelection.Current}"));

        // Assert
        var title = doc.Title;
        Assert.Contains("Loreto High School Chorlton - Secondary Progress and attainment - School Profiles - GOV.UK", title);
    }

    [Fact]
    public async Task AcademicPerformanceAttainmentAndProgressPage_Displays_SchoolName_Caption()
    {
        // Arrange
        var urn = "123456";
        var establishmentName = "Loreto High School Chorlton";
        _serviceMock
            .Setup(service => service.GetAttainmentAndProgressAsync(
                It.IsAny<string>(),
                It.IsAny<AcademicYearSelection>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AttainmentAndProgressModel()
            {
                Urn = urn,
                SchoolName = establishmentName,
                IsKS2 = false,
                IsKS4 = true,
                IsKS5 = false
            });

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, $"{_pageRoute}/{AcademicYearSelection.Current}"));
        var schoolNameCaptionElement = doc.GetElementById("school-name-caption");
        var schoolNameCaption = schoolNameCaptionElement?.TextContent;

        // Assert
        Assert.Equal("Loreto High School Chorlton", schoolNameCaption);
    }

    [Fact]
    public async Task ShowsAttainmentValues()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "St Paul's Church of England Academy";
        var schoolAttainment = 10.0;

        _serviceMock
            .Setup(service => service.GetAttainmentAndProgressAsync(
                It.IsAny<string>(),
                It.IsAny<AcademicYearSelection>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AttainmentAndProgressModel()
            {
                Urn = urn,
                SchoolName = establishmentName,
                EstablishmentAttainment8Score = schoolAttainment,
                LocalAuthorityAttainment8Score = 15,
                EnglandAttainment8Score = 20,
                IsKS2 = false,
                IsKS4 = true,
                IsKS5 = false
            });

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, $"{_pageRoute}/{AcademicYearSelection.Current}"));

        // Assert
        var schoolAttainmentCard = doc.QuerySelector("[data-testid='attainment8-establishment-card']");
        var text = schoolAttainmentCard?.QuerySelector("p")?.TextContent.Trim();
        Assert.Contains(schoolAttainment.ToString(), text);
    }

    [Fact]
    public async Task ShowsProgress8Values()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "St Paul's Church of England Academy";
        var schoolProgress = _faker.Random.Double(-0.9, 0.9);

        _serviceMock
            .Setup(service => service.GetAttainmentAndProgressAsync(
                urn,
                AcademicYearSelection.Previous,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AttainmentAndProgressModel()
            {
                Urn = urn,
                SchoolName = establishmentName,
                EstablishmentProgress8Score = schoolProgress,
                LocalAuthorityProgress8Score = 15,
                IsKS2 = false,
                IsKS4 = true,
                IsKS5 = false
            });

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, $"{_pageRoute}/{AcademicYearSelection.Previous}"));

        // Assert
        var schoolProgressCard = doc.QuerySelector("[data-testid='progress8-establishment-card']");
        var text = schoolProgressCard?.QuerySelector("p")?.TextContent.Trim();
        Assert.Contains(schoolProgress.ToString(), text);
    }

    [Fact]
    public async Task NoProgress8DataForSchool_ShowsNoProgress8Content()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "St Paul's Church of England Academy";

        _serviceMock
            .Setup(service => service.GetAttainmentAndProgressAsync(
                urn,
                AcademicYearSelection.Previous,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AttainmentAndProgressModel()
            {
                Urn = urn,
                SchoolName = establishmentName,
                EstablishmentProgress8Score = null,
                LocalAuthorityProgress8Score = 0.1,
                IsKS2 = false,
                IsKS4 = true,
                IsKS5 = false
            });

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, $"{_pageRoute}/{AcademicYearSelection.Previous}"));

        // Assert
        Assert.NotNull(doc.QuerySelector("[data-testid='progress8-no-establishment-data-card']"));
        Assert.Null(doc.QuerySelector("[data-testid='progress8-custom-card']"));
    }

    [Fact]
    public async Task NoProgress8DataForCurrentYear_ShowsNoProgress8ForCurrentYearContent()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "St Paul's Church of England Academy";

        _serviceMock
            .Setup(service => service.GetAttainmentAndProgressAsync(
                urn,
                AcademicYearSelection.Current,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AttainmentAndProgressModel()
            {
                Urn = urn,
                SchoolName = establishmentName,
                EstablishmentProgress8Score = null,
                LocalAuthorityProgress8Score = null,
                IsKS2 = true,
                IsKS4 = true,
                IsKS5 = false
            });

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, $"{_pageRoute}/{AcademicYearSelection.Current}"));

        // Assert
        Assert.NotNull(doc.QuerySelector("[data-testid='progress8-custom-card']"));
        Assert.Null(doc.QuerySelector("[data-testid='progress8-no-establishment-data-card']"));
    }
}
