using Microsoft.FeatureManagement;
using Moq;
using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Admissions;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.KS4.Admissions;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Profiles.KS4;

[Collection("WebAppCollection")]
public class AdmissionsPageTests : PageTestsBase
{
    private string _pageRoute = "/admissions/secondary";
    private string _urn = "143034";
    private string _schoolName = "St Paul's Church of England Academy";
    private string _schoolNameMultiPhase = "Abraham Moss Community School";
    private string _urnMultiPhase = "150009";
    private readonly EstablishmentServiceModel _establishment = new();
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;
    private readonly Mock<ITimeService> _mockTimeService;

    private readonly AdmissionsServiceModel _admissionsServiceModel;
    private readonly Mock<IAdmissionsService> _mockAdmissionsService;


    public AdmissionsPageTests(WebAppFixture fixture) : base(fixture)
    {
        _mockEstablishmentService = UseMock<IEstablishmentService>();
        _mockAdmissionsService = UseMock<IAdmissionsService>();
        _mockTimeService = UseMock<ITimeService>();

        _establishment = new EstablishmentTestBuilder()
            .WithURN(_urn)
            .WithEstablishmentName(_schoolName)
            .WithIsKeyStage2(false)
            .WithIsKeyStage4(true)
            .WithWebsite("https://www.stpaulsacademy.co.uk")
            .WithEstablishmentTypeGroupId((int)EstablishmentTypeGroup.Academies)
            .BuildServiceModel();

        _mockEstablishmentService
            .Setup(a => a.GetEstablishmentAsync(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_establishment);

        _admissionsServiceModel = GetAdmissionsServiceModel(_schoolName, isKs2: false, isKs4: true, _establishment.Website);

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
        Assert.Contains("Secondary Admissions", title.TextContent.Trim());
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
    [InlineData("143034", "St Paul's Church of England Academy", 7)]
    [InlineData("150009", "Abraham Moss Community School", 8)]
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
    public async Task AdmissionsPage_DoesNotDisplay_SubNavigation_WhenOnlyKS4()
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var subNav = doc.QuerySelector("#sub-navigation-admissions");
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
        var subNav = doc.QuerySelector("#sub-navigation-admissions");
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
        var subNav = doc.QuerySelector("#sub-navigation-admissions");
        Assert.NotNull(subNav);

        var secondaryLink = subNav.QuerySelector("a[aria-current='page']");
        var primaryLink = subNav.QuerySelector("a:not([aria-current='page'])");

        Assert.NotNull(primaryLink);
        Assert.NotNull(secondaryLink);
        Assert.Contains("Primary Admissions", primaryLink.TextContent.Trim());
        Assert.Contains("Secondary Admissions", secondaryLink.TextContent.Trim());
    }

    [Fact]
    public async Task AdmissionsPage_DoesNotDisplay_SubNavigation_WhenFeatureFlagDisabled()
    {
        // Arrange
        var featureManagerMock = UseMock<IFeatureManager>();
        featureManagerMock
            .Setup(f => f.IsEnabledAsync(Constants.Constants.EnablePrimary))
            .ReturnsAsync(false);

        ConfigureMultiPhaseSchool();
        var url = BuildUrl(_urnMultiPhase, _schoolNameMultiPhase, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var subNav = doc.QuerySelector("#sub-navigation-admissions");
        Assert.Null(subNav);
    }

    [Fact]
    public async Task AdmissionsPage_DisplaysBottomPagination_WithCorrectDestinations()
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
        Assert.Contains("/about", previousLink.GetAttribute("href"));

        Assert.NotNull(nextLink);
        Assert.Contains("/curriculum/secondary", nextLink.GetAttribute("href"));
    }

    [Fact]
    public async Task CurriculumPage_DisplaysBottomPagination_WithCorrectDestinations_WhenMultiplePhases()
    {
        // Arrange
        ConfigureMultiPhaseSchool();
        var url = BuildUrl(_urnMultiPhase, _schoolNameMultiPhase, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var pagination = doc.QuerySelector("nav.govuk-pagination");
        Assert.NotNull(pagination);

        var previousLink = pagination.QuerySelector(".govuk-pagination__prev a");
        var nextLink = pagination.QuerySelector(".govuk-pagination__next a");

        Assert.NotNull(previousLink);
        Assert.Contains("/admissions/primary", previousLink.GetAttribute("href"));

        Assert.NotNull(nextLink);
        Assert.Contains("/curriculum/primary", nextLink.GetAttribute("href"));
    }

    [Fact]
    public async Task AdmissionsPage_DisplaysStartingPrimarySchoolSummaryCard()
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var summaryCard = doc.QuerySelector("[data-testid='starting-secondary-school-summary']");
        Assert.NotNull(summaryCard);
        var independentSummaryCard = doc.QuerySelector("[data-testid='independent-school-summary']");
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
        var independentSummaryCard = doc.QuerySelector("[data-testid='independent-school-summary']");
        Assert.NotNull(independentSummaryCard);
        var summaryCard = doc.QuerySelector("[data-testid='starting-secondary-school-summary']");
        Assert.Null(summaryCard);
    }

    [Theory]
    [InlineData(2026)]
    [InlineData(2027)]
    [InlineData(2028)]
    [InlineData(2029)]
    public async Task AdmissionsPage_DisplaysCorrectAdmissionsContentForJulyToOctober(int year)
    {
        // Arrange
        int month = 7;
        var childYear = 6;

        var admissionsServiceModel = GetAdmissionsServiceModel(
            _schoolName,
            isKs2: false,
            isKs4: true,
            schoolWebsite: "https://www.independentsecondaryschool.co.uk",
            isIndependentSchool: false);

        _mockAdmissionsService
          .Setup(s => s.GetAdmissionsDetailsAsync(_urn, It.IsAny<CancellationToken>()))
          .ReturnsAsync(admissionsServiceModel);

        _mockTimeService
            .Setup(s => s.GetUTCTime())
            .Returns(new DateTime(year, month, 2));

        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var summaryCard = doc.QuerySelector("[data-testid='starting-secondary-school-summary']");
        var currentHeader = summaryCard!.QuerySelector("h3")!.InnerHtml;
        var currentPElements = summaryCard.QuerySelectorAll("p");

        var startingSecondaryNextContentBlock = doc.QuerySelector("[data-testid='starting-secondary-school-next']");
        var nextContentHeader = startingSecondaryNextContentBlock!.QuerySelector("h3")!.InnerHtml;
        var nextContentPElements = startingSecondaryNextContentBlock.QuerySelectorAll("p");

        var startingSecondaryAfterNextContentBlock = doc.QuerySelector("[data-testid='starting-secondary-school-afternext']");
        var afterNextContentHeader = startingSecondaryAfterNextContentBlock!.QuerySelector("h3")!.InnerHtml;
        var afterNextContentPElements = startingSecondaryAfterNextContentBlock.QuerySelectorAll("p");

        // Current
        Assert.Contains((year + 1).ToString(), currentHeader);
        Assert.Contains($"year {childYear}", currentPElements[0].InnerHtml);
        Assert.Contains($"September {year + 1}", currentPElements[0].InnerHtml);
        Assert.Contains($"The application window for starting secondary school in September {year + 1}", currentPElements[1].InnerHtml);
        Assert.Contains($"open in summer {year}", currentPElements[1].InnerHtml);
        Assert.Contains($"You'll need to apply for a place by 31 October {year}", currentPElements[1].InnerHtml);

        // Next
        Assert.Contains((year + 2).ToString(), nextContentHeader);
        Assert.Contains($"Admissions for starting secondary school in September {year + 2}", nextContentPElements[0].InnerHtml);
        Assert.Contains($"open in summer {year + 1}", nextContentPElements[0].InnerHtml);
        Assert.Contains($"year {childYear - 1}", nextContentPElements[1].InnerHtml);
        Assert.Contains($"start secondary school in September {year + 2}", nextContentPElements[1].InnerHtml);
        Assert.Contains($"The application window for starting secondary school in September {year + 2}", nextContentPElements[1].InnerHtml);
        Assert.Contains($"will open in summer {year + 1}", nextContentPElements[1].InnerHtml);
        Assert.Contains($"for a place by 31 October {year + 1}", nextContentPElements[1].InnerHtml);

        // AfterNext
        Assert.Contains((year + 3).ToString(), afterNextContentHeader);
        Assert.Contains($"year {childYear - 2}", afterNextContentPElements[0].InnerHtml);
        Assert.Contains($"start secondary school in September {year + 3}", afterNextContentPElements[0].InnerHtml);
        Assert.Contains($"starting secondary school in September {year + 3}", afterNextContentPElements[0].InnerHtml);
        Assert.Contains($"open in summer {year + 2}", afterNextContentPElements[0].InnerHtml);
        Assert.Contains($"apply for a place by 31 October {year + 2}", afterNextContentPElements[0].InnerHtml);
        Assert.Contains($"September {year + 3} by March {year + 2}", afterNextContentPElements[1].InnerHtml);
    }

    [Theory]
    [InlineData(2026)]
    [InlineData(2027)]
    [InlineData(2028)]
    [InlineData(2029)]
    public async Task AdmissionsPage_DisplaysCorrectAdmissionsContentForNovemberToDecember(int year)
    {
        // Arrange
        int month = 11;
        var childYear = 5;

        var admissionsServiceModel = GetAdmissionsServiceModel(
            _schoolName,
            isKs2: false,
            isKs4: true,
            schoolWebsite: "https://www.independentsecondaryschool.co.uk",
            isIndependentSchool: false);

        _mockAdmissionsService
          .Setup(s => s.GetAdmissionsDetailsAsync(_urn, It.IsAny<CancellationToken>()))
          .ReturnsAsync(admissionsServiceModel);

        _mockTimeService
            .Setup(s => s.GetUTCTime())
            .Returns(new DateTime(year, month, 2));

        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var summaryCard = doc.QuerySelector("[data-testid='starting-secondary-school-summary']");
        var currentHeader = summaryCard!.QuerySelector("h3")!.InnerHtml;
        var currentPElements = summaryCard.QuerySelectorAll("p");

        var startingSecondaryNextContentBlock = doc.QuerySelector("[data-testid='starting-secondary-school-next']");
        var nextContentHeader = startingSecondaryNextContentBlock!.QuerySelector("h3")!.InnerHtml;
        var nextContentPElements = startingSecondaryNextContentBlock.QuerySelectorAll("p");

        var startingSecondaryAfterNextContentBlock = doc.QuerySelector("[data-testid='starting-secondary-school-afternext']");
        var afterNextContentHeader = startingSecondaryAfterNextContentBlock!.QuerySelector("h3")!.InnerHtml;
        var afterNextContentPElements = startingSecondaryAfterNextContentBlock.QuerySelectorAll("p");

        // Current
        Assert.Contains((year + 2).ToString(), currentHeader);
        Assert.Contains($"year {childYear}", currentPElements[0].InnerHtml);
        Assert.Contains($"September {year + 2}", currentPElements[0].InnerHtml);
        Assert.Contains($"The application window for starting secondary school in September {year + 2}", currentPElements[1].InnerHtml);
        Assert.Contains($"open in summer {year + 1}", currentPElements[1].InnerHtml);
        Assert.Contains($"for a place by 31 October {year + 1}", currentPElements[1].InnerHtml);

        // Next
        Assert.Contains((year + 1).ToString(), nextContentHeader);
        Assert.Contains($"year {childYear + 1}", nextContentPElements[0].InnerHtml);
        Assert.Contains($"start secondary school in September {year + 1}", nextContentPElements[0].InnerHtml);
        Assert.Contains($"Applications for starting secondary school in September {year + 1}", nextContentPElements[0].InnerHtml);
        Assert.Contains($"closed on 31 October {year}", nextContentPElements[0].InnerHtml);

        // AfterNext
        Assert.Contains((year + 3).ToString(), afterNextContentHeader);
        Assert.Contains($"year {childYear - 1}", afterNextContentPElements[0].InnerHtml);
        Assert.Contains($"start secondary school in September {year + 3}", afterNextContentPElements[0].InnerHtml);
        Assert.Contains($"window for starting secondary school in September {year + 3}", afterNextContentPElements[0].InnerHtml);
        Assert.Contains($"open in summer {year + 2}", afterNextContentPElements[0].InnerHtml);
        Assert.Contains($"for a place by 31 October {year + 2}", afterNextContentPElements[0].InnerHtml);
    }

    [Theory]
    [InlineData(2027)]
    [InlineData(2028)]
    [InlineData(2029)]
    public async Task AdmissionsPage_DisplaysCorrectAdmissionsContentForJanuaryToJune(int year)
    {
        // Arrange
        int month = 1;
        var childYear = 5;

        var admissionsServiceModel = GetAdmissionsServiceModel(
            _schoolName,
            isKs2: false,
            isKs4: true,
            schoolWebsite: "https://www.independentsecondaryschool.co.uk",
            isIndependentSchool: false);

        _mockAdmissionsService
          .Setup(s => s.GetAdmissionsDetailsAsync(_urn, It.IsAny<CancellationToken>()))
          .ReturnsAsync(admissionsServiceModel);

        _mockTimeService
            .Setup(s => s.GetUTCTime())
            .Returns(new DateTime(year, month, 2));

        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var summaryCard = doc.QuerySelector("[data-testid='starting-secondary-school-summary']");
        var currentHeader = summaryCard!.QuerySelector("h3")!.InnerHtml;
        var currentPElements = summaryCard.QuerySelectorAll("p");

        var startingSecondaryNextContentBlock = doc.QuerySelector("[data-testid='starting-secondary-school-next']");
        var nextContentHeader = startingSecondaryNextContentBlock!.QuerySelector("h3")!.InnerHtml;
        var nextContentPElements = startingSecondaryNextContentBlock.QuerySelectorAll("p");

        var startingSecondaryAfterNextContentBlock = doc.QuerySelector("[data-testid='starting-secondary-school-afternext']");
        var afterNextContentHeader = startingSecondaryAfterNextContentBlock!.QuerySelector("h3")!.InnerHtml;
        var afterNextContentPElements = startingSecondaryAfterNextContentBlock.QuerySelectorAll("p");

        // Current
        Assert.Contains((year + 1).ToString(), currentHeader);
        Assert.Contains($"year {childYear}", currentPElements[0].InnerHtml);
        Assert.Contains($"September {year + 1}", currentPElements[0].InnerHtml);
        Assert.Contains($"The application window for starting secondary school in September {year + 1}", currentPElements[1].InnerHtml);
        Assert.Contains($"open in summer {year}", currentPElements[1].InnerHtml);
        Assert.Contains($"for a place by 31 October {year}", currentPElements[1].InnerHtml);

        // Next
        Assert.Contains(year.ToString(), nextContentHeader);
        Assert.Contains($"year {childYear + 1}", nextContentPElements[0].InnerHtml);
        Assert.Contains($"start secondary school in September {year}", nextContentPElements[0].InnerHtml);
        Assert.Contains($"Applications for starting secondary school in September {year}", nextContentPElements[0].InnerHtml);
        Assert.Contains($"closed on 31 October {year - 1}", nextContentPElements[0].InnerHtml);

        // AfterNext
        Assert.Contains((year + 2).ToString(), afterNextContentHeader);
        Assert.Contains($"year {childYear - 1}", afterNextContentPElements[0].InnerHtml);
        Assert.Contains($"start secondary school in September {year + 2}", afterNextContentPElements[0].InnerHtml);
        Assert.Contains($"window for starting secondary school in September {year + 2}", afterNextContentPElements[0].InnerHtml);
        Assert.Contains($"open in summer {year + 1}", afterNextContentPElements[0].InnerHtml);
        Assert.Contains($"for a place by 31 October {year + 1}", afterNextContentPElements[0].InnerHtml);
    }

    private void ConfigureMultiPhaseSchool()
    {
        var multiPhaseEstablishment = new EstablishmentTestBuilder()
            .WithURN(_urnMultiPhase)
            .WithEstablishmentName(_schoolNameMultiPhase)
            .WithIsKeyStage2(true)
            .WithIsKeyStage4(true)
            .BuildServiceModel();

        _mockEstablishmentService
            .Setup(a => a.GetEstablishmentAsync(_urnMultiPhase, It.IsAny<CancellationToken>()))
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
