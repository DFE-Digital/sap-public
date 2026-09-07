using Moq;
using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Profiles.KS2;

[Collection("WebAppCollection")]
public class PupilProgressPageTests : PageTestsBase
{
    private readonly string _pageRoute = "/primary-performance/pupil-progress/current";
    private readonly string _urn = "143034";
    private readonly string _schoolName = "St Paul's Church of England Academy";
    private readonly EstablishmentMinimumServiceModel _establishment = new();
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;

    private readonly KS2PupilPerformance _pupilPerformanceModel;
    private readonly Mock<IKS2PupilProgressService> _mockPupilProgressService;

    public PupilProgressPageTests(WebAppFixture fixture) : base(fixture)
    {
        _mockEstablishmentService = UseMock<IEstablishmentService>();
        _mockPupilProgressService = UseMock<IKS2PupilProgressService>();

        _establishment = new EstablishmentMinimumTestBuilder()
            .WithURN(_urn)
            .WithEstablishmentName(_schoolName)
            .WithIsKeyStage2(true)
            .WithIsKeyStage4(false)
            .BuildServiceModel();

        _mockEstablishmentService
            .Setup(a => a.GetEstablishmentMinimumAsync(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_establishment);

        _pupilPerformanceModel = GetPupilPerformanceModel(_urn);

        _mockPupilProgressService
            .Setup(s => s.GetPupilProgressAsync(_urn, It.IsAny<AcademicYearSelection>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_pupilPerformanceModel);
    }

    [Fact]
    public async Task PupilProgressPage_HasCorrectTitle()
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var title = doc.QuerySelector("title");
        Assert.NotNull(title);
        Assert.Contains("Primary Pupil progress", title.TextContent.Trim());
    }

    [Fact]
    public async Task PupilProgressPage_DisplaysMainHeading()
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var h2Elements = doc.GetElementsByTagName("h2");

        Assert.Contains(
            h2Elements,
            x => x.TextContent.Trim() == "Academic performance");
    }

    [Fact]
    public async Task PupilProgressPage_Displays_VerticalNavigation()
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        Assert.NotNull(doc.QuerySelector(".moj-side-navigation"));
        Assert.Equal(6, doc.QuerySelectorAll(".moj-side-navigation__item").Length);
        Assert.Single(doc.QuerySelectorAll(".moj-side-navigation__item--active"));
    }

    [Fact]
    public async Task PupilProgressPage_Has_Correct_Sub_Navigation_Links()
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);
        var container = doc.QuerySelector("#sub-navigation-academic-performance");
        var links = container?.QuerySelectorAll(".moj-sub-navigation__link");

        // Assert
        Assert.NotNull(links);
        Assert.Equal(4, links.Length);
    }

    [Fact]
    public async Task PupilProgressPage_Displays_AcademicYearSelector()
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var academicYearSelector = doc.QuerySelector("#academicYearSelector");
        var academicYearInfo = doc.QuerySelector("#academic-year-info");

        Assert.NotNull(academicYearSelector);
        Assert.NotNull(academicYearInfo);
    }

    [Fact]
    public async Task PupilProgressPage_ShowsDataNotAvailable_ForCurrentAcademicYear()
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var dataNotAvailable = doc.QuerySelector("#data-not-available-custom-card");
        Assert.NotNull(dataNotAvailable);

        Assert.Null(doc.QuerySelector("#reading-establishment-card"));
        Assert.Null(doc.QuerySelector("#writing-establishment-card"));
        Assert.Null(doc.QuerySelector("#maths-establishment-card"));
    }

    [Fact]
    public async Task PupilProgressPage_Displays_ProgressScores_ForPreviousAcademicYear()
    {
        // Arrange
        var url = BuildUrl(_urn, _schoolName, "/primary-performance/pupil-progress/previous2");

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var readingCard = doc.QuerySelector("#reading-establishment-card");
        var writingCard = doc.QuerySelector("#writing-establishment-card");
        var mathsCard = doc.QuerySelector("#maths-establishment-card");

        Assert.NotNull(readingCard);
        Assert.NotNull(writingCard);
        Assert.NotNull(mathsCard);

        Assert.Contains("Pupils at this school score 1.", readingCard.TextContent);
        Assert.Contains("Pupils at this school score 2.", writingCard.TextContent);
        Assert.Contains("Pupils at this school score 3.", mathsCard.TextContent);
    }

    [Fact]
    public async Task PupilProgressPage_DisplaysBottomPagination_WithCorrectDestinations()
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
        Assert.Contains("/attendance", previousLink.GetAttribute("href"));

        Assert.NotNull(nextLink);
        Assert.Contains("/primary-performance/meeting-or-exceeding-standards", nextLink.GetAttribute("href"));
    }

    private static KS2PupilPerformance GetPupilPerformanceModel(string urn)
    {
        return new KS2PupilPerformance
        {
            Urn = urn,
            EstablishmentReadingScore = new CodedDouble(1, string.Empty, "1"),
            EstablishmentReadingConfidenceUpper = new CodedDouble(1.5, string.Empty, "1.5"),
            EstablishmentReadingConfidenceLower = new CodedDouble(0.5, string.Empty, "0.5"),
            LaReadingScore = new CodedDouble(1.1, string.Empty, "1.1"),
            EstablishmentWritingScore = new CodedDouble(2, string.Empty, "2"),
            EstablishmentWritingConfidenceUpper = new CodedDouble(2.5, string.Empty, "2.5"),
            EstablishmentWritingConfidenceLower = new CodedDouble(1.5, string.Empty, "1.5"),
            LaWritingScore = new CodedDouble(2.1, string.Empty, "2.1"),
            EstablishmentMathsScore = new CodedDouble(3, string.Empty, "3"),
            EstablishmentMathsConfidenceUpper = new CodedDouble(3.5, string.Empty, "3.5"),
            EstablishmentMathsConfidenceLower = new CodedDouble(2.5, string.Empty, "2.5"),
            LaMathsScore = new CodedDouble(3.1, string.Empty, "3.1")
        };
    }
}
