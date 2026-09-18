using Moq;
using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Areas.Profiles.Helpers;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Profiles.KS4;

[Collection("WebAppCollection")]
public class AttainmentAndProgressPageTests : PageTestsBase
{
    private static string _pageRoute = "/secondary-performance/progress-attainment";
    private readonly Mock<IAttainmentAndProgressService> _serviceMock;
    private readonly Mock<IEstablishmentService> _establishmentServiceMock;

    public AttainmentAndProgressPageTests(WebAppFixture fixture) : base(fixture)
    {
        _serviceMock = UseMock<IAttainmentAndProgressService>();
        _establishmentServiceMock = UseMock<IEstablishmentService>();
    }

    [Fact]
    public async Task AcademicPerformanceAttainmentAndProgressPage_HasCorrectTitle()
    {
        // Arrange
        var expected = new AttainmentAndProgressModelBuilder()
            .Build();
        var urn = expected.Urn;
        var establishmentName = expected.SchoolName;
        _establishmentServiceMock.Setup(service => service.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentServiceModel(){
                EstablishmentName = establishmentName!,
                URN = urn,
                IsKS4 = true
                });
        _serviceMock
            .Setup(service => service.GetAttainmentAndProgressAsync(
                It.IsAny<string>(),
                     It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);


        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName!,_pageRoute));

        // Assert
        var title = doc.Title;
        Assert.Contains($"{establishmentName} - Secondary Progress and attainment - Find and compare school and college profiles - GOV.UK", title);
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
        _establishmentServiceMock.Setup(service => service.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentServiceModel()
            {
                EstablishmentName = establishmentName!,
                URN = urn,
                IsKS4 = true
            });
        _serviceMock
            .Setup(service => service.GetAttainmentAndProgressAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName!, _pageRoute));

        // Assert
        var schoolAttainmentCard = doc.QuerySelector("[data-testid='attainment8-scores-current']");
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
        _establishmentServiceMock.Setup(service => service.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentServiceModel()
            {
                EstablishmentName = establishmentName!,
                URN = urn,
                IsKS4 = true
            });
        _serviceMock
            .Setup(service => service.GetAttainmentAndProgressAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName!, _pageRoute));

        // Assert
        var schoolProgressCard = doc.QuerySelector("[data-testid='prog8-scores-prev']");
        var text = schoolProgressCard?.QuerySelector("p")?.TextContent.Trim();
        Assert.Contains(expected.EstablishmentProgress8Score.CurrentYear.ToString(), text);
    }

    [Fact]
    public async Task ShowsPupilCharacteristicTableValues()
    {
        // Arrange
        var expected = new AttainmentAndProgressModelBuilder()
            .WithAttainment8Data()
            .WithAttainment8PupilCharacteristics()
            .Build();

        var urn = expected.Urn;
        var establishmentName = expected.SchoolName;
        _establishmentServiceMock.Setup(service => service.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentServiceModel()
            {
                EstablishmentName = establishmentName!,
                URN = urn,
                IsKS4 = true,
                LAName = "Test council"
            });
        _serviceMock
            .Setup(service => service.GetAttainmentAndProgressAsync(
                urn,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName!, _pageRoute));

        // Assert
        var tableSelectorGirlBoy = "characteristics-girlboy-table";
        Assert.Contains("Girls", doc.GetTableHeaderContentByIdAndIndex(tableSelectorGirlBoy, 1, 0));
        Assert.Contains("Boys", doc.GetTableHeaderContentByIdAndIndex(tableSelectorGirlBoy, 2, 0));
        Assert.Contains("All pupils at the school", doc.GetTableHeaderContentByIdAndIndex(tableSelectorGirlBoy, 3, 0));
        Assert.Equal(expected.EstablishmentAttainment8GirlsScore!.Value.ToString(), doc.GetTableCellContentByIdAndIndex(tableSelectorGirlBoy, 1, 0));
        Assert.Equal(expected.EstablishmentAttainment8BoysScore!.Value.ToString(), doc.GetTableCellContentByIdAndIndex(tableSelectorGirlBoy, 2, 0));
        Assert.Equal(expected.EstablishmentAttainment8Score.CurrentYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex(tableSelectorGirlBoy, 3, 0));

        var tableSelectorEal = "characteristics-eal-table";
        Assert.Contains("Pupils with EAL", doc.GetTableHeaderContentByIdAndIndex(tableSelectorEal, 1, 0));
        Assert.Contains("All pupils at the school", doc.GetTableHeaderContentByIdAndIndex(tableSelectorEal, 2, 0));
        Assert.Equal(expected.EstablishmentAttainment8EALScore!.Value.ToString(), doc.GetTableCellContentByIdAndIndex(tableSelectorEal, 1, 0));
        Assert.Equal(expected.EstablishmentAttainment8Score.CurrentYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex(tableSelectorEal, 2, 0));

        var tableSelectorNonMobile = "characteristics-nonmobile-table";
        Assert.Contains("Non-mobile pupils", doc.GetTableHeaderContentByIdAndIndex(tableSelectorNonMobile, 1, 0));
        Assert.Contains("All pupils at the school", doc.GetTableHeaderContentByIdAndIndex(tableSelectorNonMobile, 2, 0));
        Assert.Equal(expected.EstablishmentAttainment8NonMobileScore!.Value.ToString(), doc.GetTableCellContentByIdAndIndex(tableSelectorNonMobile, 1, 0));
        Assert.Equal(expected.EstablishmentAttainment8Score.CurrentYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex(tableSelectorNonMobile, 2, 0));
    }

    [Fact]
    public async Task ShowsDisadvantagedTableValues()
    {
        // Arrange
        var expected = new AttainmentAndProgressModelBuilder()
            .WithAttainment8Data()
            .WithAttainmentNonDisadvantaged8Data()
            .Build();
        var urn = expected.Urn;
        var establishmentName = expected.SchoolName;
        _establishmentServiceMock.Setup(service => service.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentServiceModel()
            {
                EstablishmentName = establishmentName!,
                URN = urn,
                IsKS4 = true,
                LAName = "Test council"
            });
        _serviceMock
            .Setup(service => service.GetAttainmentAndProgressAsync(
                urn,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName!, _pageRoute));

        // Assert
        Assert.Contains("School", doc.GetTableHeaderContentByIdAndIndex("breakdown-disadvantaged-table-0", 1, 0));
        Assert.Contains("Test council average", doc.GetTableHeaderContentByIdAndIndex("breakdown-disadvantaged-table-0", 2, 0));
        Assert.Contains("England average", doc.GetTableHeaderContentByIdAndIndex("breakdown-disadvantaged-table-0", 3, 0));
        Assert.Equal(expected.EstablishmentAttainment8DisadvantagedScore.CurrentYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("breakdown-disadvantaged-table-0", 1, 0));
        Assert.Equal(expected.LocalAuthorityAttainment8DisadvantagedScore.CurrentYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("breakdown-disadvantaged-table-0", 2, 0));
        Assert.Equal(expected.EnglandAttainment8DisadvantagedScore.CurrentYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("breakdown-disadvantaged-table-0", 3, 0));

        Assert.Equal(expected.EstablishmentAttainment8DisadvantagedScore.PreviousYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("breakdown-disadvantaged-table-1", 1, 0));
        Assert.Equal(expected.LocalAuthorityAttainment8DisadvantagedScore.PreviousYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("breakdown-disadvantaged-table-1", 2, 0));
        Assert.Equal(expected.EnglandAttainment8DisadvantagedScore.PreviousYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("breakdown-disadvantaged-table-1", 3, 0));

        Assert.Equal(expected.EstablishmentAttainment8DisadvantagedScore.TwoYearsAgo!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("breakdown-disadvantaged-table-2", 1, 0));
        Assert.Equal(expected.LocalAuthorityAttainment8DisadvantagedScore.TwoYearsAgo!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("breakdown-disadvantaged-table-2", 2, 0));
        Assert.Equal(expected.EnglandAttainment8DisadvantagedScore.TwoYearsAgo!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("breakdown-disadvantaged-table-2", 3, 0));
    }

    [Theory]
    [InlineData("prev")]
    [InlineData("prev2")]
    public async Task NoProgress8DataForSchool_ShowsNoProgress8Content(string year) // progress data not available for this school (non-covid year)
    {
        // Arrange
        var expected = new AttainmentAndProgressModelBuilder()
            .Build();
        var urn = expected.Urn;
        var establishmentName = expected.SchoolName;
        _establishmentServiceMock.Setup(service => service.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentServiceModel()
            {
                EstablishmentName = establishmentName!,
                URN = urn,
                IsKS4 = true
            });
        _serviceMock
            .Setup(service => service.GetAttainmentAndProgressAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName!, _pageRoute));

        // Assert
        Assert.NotNull(doc.QuerySelector($"[data-testid='no-data-prog8-scores-{year}']"));
        Assert.Null(doc.QuerySelector($"[data-testid='prog8-scores-{year}']"));
    }

    [Theory]
    [InlineData("current")]
    [InlineData("prev")]
    [InlineData("prev2")]
    public async Task NoAttainment8DataForSchool_ShowsNoAttainment8Content(string year) // progress data not available for this school (non-covid year)
    {
        // Arrange
        var expected = new AttainmentAndProgressModelBuilder()
            .Build();
        var urn = expected.Urn;
        var establishmentName = expected.SchoolName;
        _establishmentServiceMock.Setup(service => service.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentServiceModel()
            {
                EstablishmentName = establishmentName!,
                URN = urn,
                IsKS4 = true
            });
        _serviceMock
            .Setup(service => service.GetAttainmentAndProgressAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName!, _pageRoute));

        // Assert
        Assert.NotNull(doc.QuerySelector($"[data-testid='attainment8-scores-{year}-no-establishment-data-card']"));
        Assert.Null(doc.QuerySelector($"[data-testid='attainment8-scores-{year}']"));
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
        _establishmentServiceMock.Setup(service => service.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentServiceModel()
            {
                EstablishmentName = establishmentName!,
                URN = urn,
                IsKS4 = true
            });
        _serviceMock
            .Setup(service => service.GetAttainmentAndProgressAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName!, _pageRoute));

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

    [Theory]
    [InlineData(TypeOfEstablishment.UniversityTechnicalCollege, "11", true, false, false)]
    [InlineData(TypeOfEstablishment.StudioSchools, "11", false, true, false)]
    [InlineData(TypeOfEstablishment.FurtherEducation, "11", false, false, true)]
    [InlineData(TypeOfEstablishment.FurtherEducation, "12", false, false, true)]
    [InlineData(TypeOfEstablishment.CommunitySchool, "12", false, false, true)]
    [InlineData(TypeOfEstablishment.CommunitySchool, "11", false, false, false)]
    public async Task AcademicPerformanceAttainmentAndProgressPage_DisplaysCorrectProgress8Caveats(TypeOfEstablishment typeOfEstablishment, string ageRangeLow, bool expectedShowUTCCaveat, bool expectedShowStudioSchoolCaveat, bool expectedShowFECaveat)
    {
        // Arrange
        // Arrange
        var expected = new AttainmentAndProgressModelBuilder()
            .WithAttainment8Data()
            .Build();
        var urn = expected.Urn;
        var establishmentName = expected.SchoolName;
        
        _establishmentServiceMock
            .Setup(service => service.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentServiceModel()
            {
                EstablishmentName = establishmentName!,
                URN = urn,
                IsKS4 = true,
                TypeOfEstablishment = typeOfEstablishment,
                AgeRangeLow = ageRangeLow
            });

        _serviceMock
            .Setup(service => service.GetAttainmentAndProgressAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName!, _pageRoute));

        // Assert
        var utcCaveatElement = doc.QuerySelector("#utc-caveat-inset-text");
        var studioSchoolCaveatElement = doc.QuerySelector("#studioschool-caveat-inset-text");
        var feCaveatElement = doc.QuerySelector("#fe-caveat-inset-text");

        if (expectedShowUTCCaveat)
        {
            Assert.NotNull(utcCaveatElement);
        }
        if (expectedShowStudioSchoolCaveat)
        {
            Assert.NotNull(studioSchoolCaveatElement);
        }
        if (expectedShowFECaveat)
        {
            Assert.NotNull(feCaveatElement);
        }
    }
}
