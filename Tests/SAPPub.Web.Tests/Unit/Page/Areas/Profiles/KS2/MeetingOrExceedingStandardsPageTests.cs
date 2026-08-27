using Moq;
using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Admissions;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.KS4.Admissions;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Profiles.KS2;

[Collection("WebAppCollection")]
public class MeetingOrExceedingStandardsPageTests : PageTestsBase
{
    private readonly string _pageRoute = "/primary-performance/meeting-or-exceeding-standards";
    private readonly string _urn = "143034";
    private readonly string _schoolName = "St Paul's Church of England Academy";
    private readonly string _schoolNameMultiPhase = "Abraham Moss Community School";
    private readonly string _urnMultiPhase = "150009";
    private readonly EstablishmentServiceModel _establishment = new();
    private readonly EstablishmentMinimumServiceModel _establishmentMinimum = new();
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;

    private readonly AdmissionsServiceModel _admissionsServiceModel;
    private readonly Mock<IAdmissionsService> _mockAdmissionsService;

    public MeetingOrExceedingStandardsPageTests(WebAppFixture fixture) : base(fixture)
    {
        _mockEstablishmentService = UseMock<IEstablishmentService>();
        _mockAdmissionsService = UseMock<IAdmissionsService>();

        _establishment = new EstablishmentTestBuilder()
            .WithURN(_urn)
            .WithEstablishmentName(_schoolName)
            .WithIsKeyStage2(true)
            .WithIsKeyStage4(false)
            .WithWebsite("https://www.stpaulsacademy.co.uk")
            .WithEstablishmentTypeGroupId((int)EstablishmentTypeGroup.Academies)
            .BuildServiceModel();

        _establishmentMinimum = new EstablishmentMinimumTestBuilder()
            .WithURN(_urn)
            .WithEstablishmentName(_schoolName)
            .WithIsKeyStage2(true)
            .WithIsKeyStage4(false)
            .WithWebsite("https://www.stpaulsacademy.co.uk")
            .BuildServiceModel();

        _mockEstablishmentService
            .Setup(a => a.GetEstablishmentAsync(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_establishment);

        _mockEstablishmentService
            .Setup(a => a.GetEstablishmentMinimumAsync(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_establishmentMinimum);

        _admissionsServiceModel = GetAdmissionsServiceModel(_schoolName, isKs2: true, isKs4: false, _establishment.Website);

        _mockAdmissionsService
            .Setup(s => s.GetAdmissionsDetailsAsync(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_admissionsServiceModel);
    }

    [Fact]
    public async Task MeetingOrExceedingStandardsPage_HasCorrectTitle()
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var title = doc.QuerySelector("title");
        Assert.NotNull(title);
        Assert.Contains("Primary Meeting or exceeding standards", title.TextContent.Trim());
    }

    [Fact]
    public async Task MeetingOrExceedingStandardsPage_DisplaysMainHeading()
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var heading = doc.QuerySelector("h1");
        Assert.NotNull(heading);
        Assert.NotEmpty(heading.TextContent.Trim());
    }

    [Theory]
    [InlineData("143034", "St Paul's Church of England Academy", 6)]
    [InlineData("150009", "Abraham Moss Community School", 8)]
    public async Task MeetingOrExceedingStandardsPage_Displays_VerticalNavigation(string urn, string schoolName, int expectedItemCount)
    {
        // Arrange
        if (urn == _urnMultiPhase)
        {
            ConfigureMultiPhaseSchool();
        }

        var url = BuildUrl(urn, schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        Assert.NotNull(doc.QuerySelector(".moj-side-navigation"));
        Assert.Equal(expectedItemCount, doc.QuerySelectorAll(".moj-side-navigation__item").Length);
        Assert.Single(doc.QuerySelectorAll(".moj-side-navigation__item--active"));
    }

    [Fact]
    public async Task MeetingOrExceedingStandardsPage_Displays_SchoolName_Caption()
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var schoolNameCaption = doc.QuerySelector("#school-name-caption");
        Assert.NotNull(schoolNameCaption);
        Assert.Equal(_schoolName, schoolNameCaption.TextContent.Trim());
    }

    [Fact]
    public async Task MeetingOrExceedingStandardsPage_Displays_SubNavigation()
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var subNav = doc.QuerySelector("#sub-navigation-academic-performance");
        Assert.NotNull(subNav);
    }

    [Fact]
    public async Task MeetingOrExceedingStandardsPage_Displays_MeetingExpectedStandardData()
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var chartDataCurrent = doc.QuerySelector("#mes-current-year-chart-container");
        var tableDataCurrent = doc.QuerySelector("#mes-current-year-table-container");
        var chartDataOverTime = doc.QuerySelector("#mes-data-over-time-chart-container");
        var tableDataOverTime = doc.QuerySelector("#mes-data-over-time-table-container");
        Assert.NotNull(chartDataCurrent);
        Assert.NotNull(tableDataCurrent);
        Assert.NotNull(chartDataOverTime);
        Assert.NotNull(tableDataOverTime);
    }

    [Fact]
    public async Task MeetingOrExceedingStandardsPage_Displays_ExceedingExpectedStandardData()
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var chartDataCurrent = doc.QuerySelector("#exs-current-year-chart-container");
        var tableDataCurrent = doc.QuerySelector("#exs-current-year-table-container");
        var chartDataOverTime = doc.QuerySelector("#exs-data-over-time-chart-container");
        var tableDataOverTime = doc.QuerySelector("#exs-data-over-time-table-container");
        Assert.NotNull(chartDataCurrent);
        Assert.NotNull(tableDataCurrent);
        Assert.NotNull(chartDataOverTime);
        Assert.NotNull(tableDataOverTime);
    }

    [Fact]
    public async Task MeetingOrExceedingStandardsPage_DisplaysBottomPagination_WithCorrectDestinations()
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var pagination = doc.QuerySelector("nav.govuk-pagination");
        Assert.NotNull(pagination);

        var previousLink = pagination.QuerySelector(".govuk-pagination__prev a");
        var nextLink = pagination.QuerySelector(".govuk-pagination__next a");

        Assert.NotNull(previousLink);
        Assert.Contains("/primary-performance/pupil-progress", previousLink.GetAttribute("href"));

        Assert.NotNull(nextLink);
        Assert.Contains("/primary-performance/subject-scaled-scores", nextLink.GetAttribute("href"));
    }  

    private void ConfigureMultiPhaseSchool()
    {
        var multiPhaseEstablishment = new EstablishmentMinimumTestBuilder()
            .WithURN(_urnMultiPhase)
            .WithEstablishmentName(_schoolNameMultiPhase)
            .WithIsKeyStage2(true)
            .WithIsKeyStage4(true)
            .BuildServiceModel();

        _mockEstablishmentService
            .Setup(a => a.GetEstablishmentMinimumAsync(_urnMultiPhase, It.IsAny<CancellationToken>()))
            .ReturnsAsync(multiPhaseEstablishment);

        _mockAdmissionsService
            .Setup(s => s.GetAdmissionsDetailsAsync(_urnMultiPhase, It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetAdmissionsServiceModel(_schoolNameMultiPhase, isKs2: true, isKs4: true, multiPhaseEstablishment.Website));
    }

    private AdmissionsServiceModel GetAdmissionsServiceModel(
        string schoolName,
        bool isKs2,
        bool isKs4,
        string? schoolWebsite,
        bool isIndependentSchool = false)
    {
        return new AdmissionsServiceModel
        {
            SchoolName = schoolName,
            IsKS2 = isKs2,
            IsKS4 = isKs4,
            IsKS5 = false,
            LAName = "Test LA",
            EstablishmentStatus = EstablishmentStatus.Open,
            IsIndependentSchool = isIndependentSchool,
            SchoolWebsite = schoolWebsite,
            LASchoolAdmissionsUrl = "https://www.testla.gov.uk/admissions"
        };
    }
}
