using Moq;
using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Constants;
using SAPPub.Web.Helpers;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Profiles;

[Collection("WebAppCollection")]
public class Level3QualificationsPageTests : PageTestsBase
{
    private string _pageRoute = "/16-to-19-performance/level-3-qualifications";
    private string _urn = "100279";
    private Level3 _qualificationType;
    private Level3QualificationModel _level3QualificationModel = null!;
    private readonly EstablishmentServiceModel _establishment = new();    
    private readonly Mock<ILevel3QualificationsService> _level3QualificationsService = new();

    public Level3QualificationsPageTests(WebAppFixture fixture) : base(fixture)
    {
        _level3QualificationsService = UseMock<ILevel3QualificationsService>();
        _establishment = new EstablishmentTestBuilder()
            .WithURN(_urn)
            .WithEstablishmentName($"School{_urn}")
            .WithIsKeyStage5(true)
            .WithSixthForm(true)
            .BuildServiceModel();        
    }

    private void SetupMocks(Level3 qualification = Level3.ALevel)
    {
        _qualificationType = qualification;
        _level3QualificationModel = new Level3QualificationsModelBuilder()
            .WithUrn(_urn)
            .WithQualificationType(_qualificationType)
            .WithKS5(true)
            .Build();

        _level3QualificationsService.Setup(s => s.GetLevel3QualificationDetailsAsync(_urn, _qualificationType, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_level3QualificationModel);
    }

    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    [InlineData(Level3.AppliedGeneral)]
    public async Task Level3QualificationsPage_HasCorrectTitle(Level3 qualification)
    {
        // Arrange
        SetupMocks(qualification);
        var pageRouteUrl = $"{_pageRoute}/{_qualificationType.ToString().ToLower()}";
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, pageRouteUrl);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var title = doc.QuerySelector("title");
        Assert.NotNull(title);

        var suffixTitle = qualification switch
        {
            Level3.ALevel => PageTitleConstants.KS5SchoolPageTitles.Level3QualificationsAlevel,
            Level3.Academic => PageTitleConstants.KS5SchoolPageTitles.Level3QualificationsAcademic,
            Level3.AppliedGeneral => PageTitleConstants.KS5SchoolPageTitles.Level3QualificationsAppliedGeneral,
            _ => null
        };
        var expectedTitle = $"{PageTitleConstants.KS5SchoolPageTitles.PhaseTitle} - {suffixTitle}";
        Assert.Contains(expectedTitle, title.TextContent.Trim());
    }

    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    [InlineData(Level3.AppliedGeneral)]
    public async Task Level3QualificationsPage_DisplaysMainHeading(Level3 qualification)
    {
        // Arrange
        SetupMocks(qualification);
        var pageRouteUrl = $"{_pageRoute}/{_qualificationType.ToString().ToLower()}";
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, pageRouteUrl);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var heading = doc.QuerySelector("h1");
        Assert.NotNull(heading);
        Assert.Contains(PageTitleConstants.KS5SchoolPageTitles.Performance, heading.TextContent.Trim());
    }

    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    [InlineData(Level3.AppliedGeneral)]
    public async Task Level3QualificationsPage_DisplaysHeading(Level3 qualification)
    {
        // Arrange
        SetupMocks(qualification);
        var pageRouteUrl = $"{_pageRoute}/{_qualificationType.ToString().ToLower()}";
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, pageRouteUrl);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var heading = doc.GetElementsByTagName("h2")[1];
        Assert.NotNull(heading);
        Assert.Contains(qualification.GetDisplayName()!, heading.TextContent.Trim());
    }

    [Fact]
    public async Task Level3QualificationsPage_Displays_VerticalNavigation()
    {
        // Arrange
        SetupMocks();
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
    public async Task Level3QualificationsPage_Has_Correct_Sub_Navigation_Links()
    {
        // Arrange
        SetupMocks();
        var pageRouteUrl = $"{_pageRoute}/{_qualificationType.ToString().ToLower()}";
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, pageRouteUrl);

        // Act
        var doc = await Fixture.BrowseToPage(url);
        var container = doc.QuerySelector("#sub-navigation-academic-performance");
        var links = container?.QuerySelectorAll(".moj-sub-navigation__link");

        Assert.NotNull(links);
        Assert.Equal(4, links.Length);        
    }

    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    [InlineData(Level3.AppliedGeneral)]
    public async Task Level3QualificationsPage_DisplaysProgressScoreHeading(Level3 qualification)
    {
        // Arrange
        SetupMocks(qualification);
        var pageRouteUrl = $"{_pageRoute}/{_qualificationType.ToString().ToLower()}";
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, pageRouteUrl);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var heading = doc.GetElementsByTagName("h3")[0];
        Assert.NotNull(heading);
        Assert.Contains("Progress score", heading.TextContent.Trim());
    }

    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    [InlineData(Level3.AppliedGeneral)]
    public async Task Level3QualificationsPage_DisplaysTechnicalGuidanceLink(Level3 qualification)
    {
        // Arrange
        SetupMocks(qualification);
        var pageRouteUrl = $"{_pageRoute}/{_qualificationType.ToString().ToLower()}";
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, pageRouteUrl);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var techGuidanceLink = doc.QuerySelector("#tech-guidance-link");

        if (qualification == Level3.ALevel || qualification == Level3.AppliedGeneral)
        {
            Assert.NotNull(techGuidanceLink);
            Assert.Contains("https://www.gov.uk/government/publications/16-to-19-accountability-headline-measures-technical-guide", techGuidanceLink.GetAttribute("href"));
        }
        else
        {
            Assert.Null(techGuidanceLink);
        }
    }

    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    [InlineData(Level3.AppliedGeneral)]
    public async Task Level3QualificationsPage_DisplaysTechnicalVocationalQualificationsLink(Level3 qualification)
    {
        // Arrange
        SetupMocks(qualification);
        var pageRouteUrl = $"{_pageRoute}/{_qualificationType.ToString().ToLower()}";
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, pageRouteUrl);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var techVocationalQualificationsLink = doc.QuerySelector("#tech-vocational-qualifications-link");

        if (qualification == Level3.AppliedGeneral)
        {
            Assert.NotNull(techVocationalQualificationsLink);
            Assert.Contains("https://www.gov.uk/government/collections/performance-tables-technical-and-vocational-qualifications", techVocationalQualificationsLink.GetAttribute("href"));
        }
        else
        {
            Assert.Null(techVocationalQualificationsLink);
        }
    }

    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    [InlineData(Level3.AppliedGeneral)]
    public async Task Level3QualificationsPage_Displays_ProgressScore(Level3 qualification)
    {
        // Arrange
        SetupMocks(qualification);
        var pageRouteUrl = $"{_pageRoute}/{_qualificationType.ToString().ToLower()}";
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, pageRouteUrl);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert no of students completed qualification

        var noOfStudentsInfo = doc.QuerySelector("#no-of-students-completed-qulification-info");
        Assert.NotNull(noOfStudentsInfo);
        Assert.Equal($"Number of students from this school or college included in the measure: {_level3QualificationModel.TotalNoOfStudentCompletedQualification}", noOfStudentsInfo.TextContent.Trim());

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
        Assert.Contains($"Students at this school score {_level3QualificationModel.ProgressScore.Score}", progresScoreCard.QuerySelectorAll("p")[0].TextContent);
        Assert.Contains($"This is average", progresScoreCard.QuerySelectorAll("p")[0].QuerySelector("span")?.TextContent);
        Assert.Contains($"The confidence interval is {_level3QualificationModel.ProgressScore.ConfidenceLevelLower} to {_level3QualificationModel.ProgressScore.ConfidenceLevelUpper}.", progresScoreCard.QuerySelectorAll("p")[1].TextContent);

        // Assert progress england average
        var averageProgresScoreNationalCard = doc.QuerySelector("#average-progress-score-national-card");
        Assert.NotNull(averageProgresScoreNationalCard);
        Assert.Contains($"Average progress score in England: {_level3QualificationModel.ProgressScore.EnglandAverageScore}", averageProgresScoreNationalCard.QuerySelector("p")?.TextContent);
    }

    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    [InlineData(Level3.AppliedGeneral)]
    public async Task Level3QualificationsPage_Displays_AverageResult(Level3 qualification)
    {
        // Arrange
        SetupMocks(qualification);
        var pageRouteUrl = $"{_pageRoute}/{_qualificationType.ToString().ToLower()}";
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, pageRouteUrl);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert heading
        var heading = doc.GetElementsByTagName("h3")[1];
        Assert.NotNull(heading);
        Assert.Contains("Average result", heading.TextContent.Trim());

        // Assert no of students completed qualification
        var noOfStudentsInfo = doc.QuerySelector("#no-of-students-completed-qulification-info");
        Assert.NotNull(noOfStudentsInfo);
        Assert.Equal($"Number of students from this school or college included in the measure: {_level3QualificationModel.TotalNoOfStudentCompletedQualification}", noOfStudentsInfo.TextContent.Trim());

        // Assert performance points link
        var performancePointsLink = doc.QuerySelector("#performance-points-link");
        Assert.NotNull(performancePointsLink);
        Assert.Equal("https://www.gov.uk/government/publications/performance-points-a-practical-guide-to-key-stage-4-and-5-points", performancePointsLink.GetAttribute("href"));

        Assert.Contains("School or College", doc.GetTableHeaderContentByIdAndIndex("average-result-current-year-table", 1, 0));
        Assert.Contains(_level3QualificationModel.AverageResult.Establishment.Grade!, doc.GetTableCellContentByIdAndIndex("average-result-current-year-table", 1, 0));
        Assert.Contains(_level3QualificationModel.AverageResult.Establishment.Points.Value!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("average-result-current-year-table", 1, 1));

        Assert.Contains($"{_level3QualificationModel.LAName} average", doc.GetTableHeaderContentByIdAndIndex("average-result-current-year-table", 2, 0));
        Assert.Contains(_level3QualificationModel.AverageResult.LocalAuthority.Grade!, doc.GetTableCellContentByIdAndIndex("average-result-current-year-table", 2, 0));
        Assert.Contains(_level3QualificationModel.AverageResult.LocalAuthority.Points.Value!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("average-result-current-year-table", 2, 1));

        Assert.Contains("England average", doc.GetTableHeaderContentByIdAndIndex("average-result-current-year-table", 3, 0));
        Assert.Contains(_level3QualificationModel.AverageResult.England.Grade!, doc.GetTableCellContentByIdAndIndex("average-result-current-year-table", 3, 0));
        Assert.Contains(_level3QualificationModel.AverageResult.England.Points.Value!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("average-result-current-year-table", 3, 1));
    }
}
