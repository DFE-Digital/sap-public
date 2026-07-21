using Moq;
using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Constants;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Profiles;

[Collection("WebAppCollection")]
public class AdvancedLevelPageTests : PageTestsBase
{
    private string _pageRoute = "/16-to-19-performance/advanced-level";
    private string _urn = "100279";
    private Level3 _qualificationType;
    private readonly EstablishmentServiceModel _establishment = new();
    private readonly AdvancedLevelQualificationModel _advancedLevelQualificationModel;
    private readonly Mock<IAdvancedLevelQualificationsService> _advancedLevelQualificationsService = new();

    public AdvancedLevelPageTests(WebAppFixture fixture) : base(fixture)
    {
        _qualificationType = Level3.ALevel;
        _advancedLevelQualificationsService = UseMock<IAdvancedLevelQualificationsService>();
        _establishment = new EstablishmentTestBuilder()
            .WithURN(_urn)
            .WithEstablishmentName($"School{_urn}")
            .WithIsKeyStage5(true)
            .WithSixthForm(true)
            .BuildServiceModel();

        _advancedLevelQualificationModel = new AdvancedLevelQualificationModelBuilder()
            .WithUrn(_urn)
            .WithQualificationType(_qualificationType)
            .WithKS5(true)
            .Build();

        _advancedLevelQualificationsService.Setup(s => s.GetAdvancedLevelQualificationDetailsAsync(_urn, _qualificationType, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_advancedLevelQualificationModel);
    }

    [Fact]
    public async Task AdvancedLevelPage_Tlevel_HasCorrectTitle()
    {
        // Arrange
        var pageRouteUrl = $"{_pageRoute}/{_qualificationType.ToString().ToLower()}";
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, pageRouteUrl);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var title = doc.QuerySelector("title");
        Assert.NotNull(title);

        var expectedTitle = $"{PageTitleConstants.KS5SchoolPageTitles.PhaseTitle} - {PageTitleConstants.KS5SchoolPageTitles.Level3QualificationsAlevel}";
        Assert.Contains(expectedTitle, title.TextContent.Trim());
    }

    [Fact]
    public async Task AdvancedLevelPage_Tlevel_DisplaysMainHeading()
    {
        var pageRouteUrl = $"{_pageRoute}/{_qualificationType.ToString().ToLower()}";
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, pageRouteUrl);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var heading = doc.QuerySelector("h1");
        Assert.NotNull(heading);
        Assert.Contains(PageTitleConstants.KS5SchoolPageTitles.Performance, heading.TextContent.Trim());
    }

    [Fact]
    public async Task AdvancedLevelPage_Tlevel_DisplaysHeading()
    {
        var pageRouteUrl = $"{_pageRoute}/{_qualificationType.ToString().ToLower()}";
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, pageRouteUrl);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var heading = doc.GetElementsByTagName("h2")[1];
        Assert.NotNull(heading);
        Assert.Contains("A level", heading.TextContent.Trim());
    }

    [Fact]
    public async Task AdvancedLevelPage_Tlevel_Displays_VerticalNavigation()
    {
        var pageRouteUrl = $"{_pageRoute}/{_qualificationType.ToString().ToLower()}";
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, pageRouteUrl);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        Assert.NotNull(doc.QuerySelector(".moj-side-navigation"));
        Assert.Equal(3, doc.QuerySelectorAll(".moj-side-navigation__item").Length);
        Assert.Single(doc.QuerySelectorAll(".moj-side-navigation__item--active"));
    }

    [Fact]
    public async Task AdvancedLevelPage_Has_Correct_Sub_Navigation_Links()
    {
        // Arrange
        var pageRouteUrl = $"{_pageRoute}/{_qualificationType.ToString().ToLower()}";
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, pageRouteUrl);

        // Act
        var doc = await Fixture.BrowseToPage(url);
        var container = doc.QuerySelector("#sub-navigation-academic-performance");
        var links = container?.QuerySelectorAll(".moj-sub-navigation__link");

        Assert.NotNull(links);
        Assert.Equal(4, links.Length);        
    }

    [Fact]
    public async Task AdvancedLevelPage_Tlevel_DisplaysProgressScoreHeading()
    {
        var pageRouteUrl = $"{_pageRoute}/{_qualificationType.ToString().ToLower()}";
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, pageRouteUrl);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var heading = doc.GetElementsByTagName("h3")[0];
        Assert.NotNull(heading);
        Assert.Contains("Progress score", heading.TextContent.Trim());
    }

    [Fact]
    public async Task AdvancedLevelPage_Tlevel_DisplaysTechnicalGuidanceLink()
    {
        var pageRouteUrl = $"{_pageRoute}/{_qualificationType.ToString().ToLower()}";
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, pageRouteUrl);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var techGuidanceLink = doc.QuerySelector("#tech-guidance-link");
        Assert.NotNull(techGuidanceLink);
        Assert.Contains("https://www.gov.uk/government/publications/16-to-19-accountability-headline-measures-technical-guide", techGuidanceLink.GetAttribute("href"));
    }

    [Fact]
    public async Task AdvancedLevelPage_Tlevel_Displays_ProgressScore()
    {
        var pageRouteUrl = $"{_pageRoute}/{_qualificationType.ToString().ToLower()}";
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, pageRouteUrl);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert no of students completed qualification

        var noOfStudentsInfo = doc.QuerySelector("#no-of-students-completed-qulification-info");
        Assert.NotNull(noOfStudentsInfo);
        Assert.Equal($"Number of students from this school or college included in the measure: {_advancedLevelQualificationModel.TotalNoOfStudentCompletedQualification}", noOfStudentsInfo.TextContent.Trim());

        // Assert progress score confidence level
        var progressScoreConfidenceIntervalsDetails = doc.QuerySelector("#details-progress-score-confidence-intervals");
        Assert.NotNull(progressScoreConfidenceIntervalsDetails);

        // Assert accountability guidance link
        var accountabilityGuidanceLink = progressScoreConfidenceIntervalsDetails.QuerySelector("a");
        Assert.NotNull(accountabilityGuidanceLink);
        Assert.Equal("https://www.gov.uk/government/publications/16-to-19-accountability-headline-measures-technical-guide", accountabilityGuidanceLink.GetAttribute("href"));

        // Assert progress data
        var progresScoreCard = doc.QuerySelector("#progress-score-card");
        Assert.NotNull(progresScoreCard);
        Assert.Contains($"Pupils at this school score {_advancedLevelQualificationModel.ProgressScore.Score}", progresScoreCard.QuerySelectorAll("p")[0].TextContent);
        Assert.Contains($"This is average", progresScoreCard.QuerySelectorAll("p")[0].QuerySelector("span")?.TextContent);
        Assert.Contains($"The confidence interval is {_advancedLevelQualificationModel.ProgressScore.ConfidenceLevelLower} to {_advancedLevelQualificationModel.ProgressScore.ConfidenceLevelUpper}.", progresScoreCard.QuerySelectorAll("p")[1].TextContent);

        // Assert progress england average
        var averageProgresScoreNationalCard = doc.QuerySelector("#average-progress-score-national-card");
        Assert.NotNull(averageProgresScoreNationalCard);
        Assert.Contains($"Average progress score in England: {_advancedLevelQualificationModel.ProgressScore.EnglandAverageScore}", averageProgresScoreNationalCard.QuerySelector("p")?.TextContent);
    }
}
