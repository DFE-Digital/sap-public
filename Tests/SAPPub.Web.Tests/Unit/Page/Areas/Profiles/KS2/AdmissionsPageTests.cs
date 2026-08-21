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
public class AdmissionsPageTests : PageTestsBase
{
    private string _pageRoute = "/admissions/primary";
    private string _urn = "143034";
    private string _schoolName = "St Paul's Church of England Academy";
    private string _schoolNameMultiPhase = "Abraham Moss Community School";
    private string _urnMultiPhase = "150009";
    private readonly EstablishmentServiceModel _establishment = new();
    private readonly EstablishmentMinimumServiceModel _establishmentMinimum = new();
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;

    private readonly AdmissionsServiceModel _admissionsServiceModel;
    private readonly Mock<IAdmissionsService> _mockAdmissionsService;

    public AdmissionsPageTests(WebAppFixture fixture) : base(fixture)
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
    public async Task AdmissionsPage_HasCorrectTitle()
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var title = doc.QuerySelector("title");
        Assert.NotNull(title);
        Assert.Contains("Primary Admissions", title.TextContent.Trim());
    }

    [Fact]
    public async Task AdmissionsPage_DisplaysMainHeading()
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
    [InlineData("143034", "St Paul's Church of England Academy", 5)]
    [InlineData("150009", "Abraham Moss Community School", 7)]
    public async Task AdmissionsPage_Displays_VerticalNavigation(string urn, string schoolName, int expectedItemCount)
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
    public async Task AdmissionsPage_Displays_SchoolName_Caption()
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
    public async Task AdmissionsPage_DoesNotDisplay_SubNavigation_WhenOnlyKS2()
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var subNav = doc.QuerySelector("#sub-navigation-academic-performance");
        Assert.Null(subNav);
    }

    [Fact]
    public async Task AdmissionsPage_Displays_SubNavigation_WhenMultiplePhases()
    {
        // Arrange
        ConfigureMultiPhaseSchool();
        var url = BuildUrl(_urnMultiPhase, _schoolNameMultiPhase, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var subNav = doc.QuerySelector("#sub-navigation-academic-performance");
        Assert.NotNull(subNav);
    }

    [Fact]
    public async Task AdmissionsPage_SubNavigation_HasCorrectLinks_WhenMultiplePhases()
    {
        // Arrange
        ConfigureMultiPhaseSchool();
        var url = BuildUrl(_urnMultiPhase, _schoolNameMultiPhase, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var subNav = doc.QuerySelector("#sub-navigation-academic-performance");
        Assert.NotNull(subNav);

        var primaryLink = subNav.QuerySelector("a[aria-current='page']");
        var secondaryLink = subNav.QuerySelector("a:not([aria-current='page'])");

        Assert.NotNull(primaryLink);
        Assert.NotNull(secondaryLink);
        Assert.Contains("Primary Admissions", primaryLink.TextContent.Trim());
        Assert.Contains("Secondary Admissions", secondaryLink.TextContent.Trim());
    }

    [Fact]
    public async Task AdmissionsPage_DisplaysStartingPrimarySchoolSummaryCard()
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var summaryCard = doc.QuerySelector("[data-testid='starting-primary-school-summary']");
        Assert.NotNull(summaryCard);
        var independentSummaryCard = doc.QuerySelector("[data-testid='independent-primary-school-summary']");
        Assert.Null(independentSummaryCard);

        var schoolWebsiteLink = summaryCard.QuerySelector("[data-testid='school-website-link']");
        var laWebsiteLink = summaryCard.QuerySelector("[data-testid='la-website-link']");

        Assert.NotNull(schoolWebsiteLink);
        Assert.NotNull(laWebsiteLink);
        Assert.NotNull(schoolWebsiteLink.GetAttribute("href"));
        Assert.NotNull(schoolWebsiteLink.TextContent.Trim());
        Assert.NotNull(laWebsiteLink.GetAttribute("href"));
        Assert.NotNull(laWebsiteLink.TextContent.Trim());
    }

    [Fact]
    public async Task AdmissionsPage_DisplaysIndependentPrimarySchoolSummaryCard()
    {
        // Arrange
        var independentPrimaryAdmissionsServiceModel = GetAdmissionsServiceModel(
            _schoolName,
            isKs2: true,
            isKs4: false,
            schoolWebsite: "https://www.independentprimaryschool.co.uk",
            isIndependentSchool: true);

        _mockAdmissionsService
          .Setup(s => s.GetAdmissionsDetailsAsync(_urn, It.IsAny<CancellationToken>()))
          .ReturnsAsync(independentPrimaryAdmissionsServiceModel);

        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var independentSummaryCard = doc.QuerySelector("[data-testid='independent-primary-school-summary']");
        Assert.NotNull(independentSummaryCard);
        var summaryCard = doc.QuerySelector("[data-testid='starting-primary-school-summary']");
        Assert.Null(summaryCard);
    }

    [Fact]
    public async Task AdmissionsPage_DisplaysMovingSchoolsDuringYearSummaryCard()
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var summaryCard = doc.QuerySelector("[data-testid='moving-schools-during-year-summary-card']");
        Assert.NotNull(summaryCard);

        var link = summaryCard.QuerySelector("[data-testid='link']");
        Assert.NotNull(link);
        Assert.NotNull(link.TextContent.Trim());
    }

    [Fact]
    public async Task AdmissionsPage_DisplaysMoreInfoOnPrimarySchoolAdmissionsAccordion()
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var admissionsAccordion = doc.QuerySelector("#more-info-on-primary-school-admissions-accordion");
        Assert.NotNull(admissionsAccordion);
    }

    [Fact]
    public async Task Admissions_DisplaysStartingPrimarySchool_Info()
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var summaryCard = doc.QuerySelector("[data-testid='starting-primary-school-summary']");
        Assert.NotNull(summaryCard);

        var contactSchoolInfo = summaryCard.QuerySelector("[data-testid='contact-school-info']");
        Assert.Null(contactSchoolInfo);
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
