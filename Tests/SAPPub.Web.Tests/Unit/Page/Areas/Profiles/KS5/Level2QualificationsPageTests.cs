using Moq;
using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Constants;
using SAPPub.Web.Helpers;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Profiles.KS5;

[Collection("WebAppCollection")]
public class Level2QualificationsPageTests : PageTestsBase
{
    private string _pageRoute = "/16-to-19-performance/level-2-qualifications";
    private string _urn = "100279";
    private Level2 _qualificationType;
    private Level2QualificationModel _level2QualificationModel = null!;
    private readonly EstablishmentServiceModel _establishment = new();
    private readonly Mock<ILevel2QualificationsService> _level2QualificationsService = new();

    public Level2QualificationsPageTests(WebAppFixture fixture) : base(fixture)
    {
        _level2QualificationsService = UseMock<ILevel2QualificationsService>();
        _establishment = new EstablishmentTestBuilder()
            .WithURN(_urn)
            .WithEstablishmentName($"School{_urn}")
            .WithIsKeyStage5(true)
            .WithSixthForm(true)
            .BuildServiceModel();
    }

    private void SetupMocks(Level2 qualification = Level2.TechCert)
    {
        _qualificationType = qualification;
        _level2QualificationModel = new Level2QualificationsModelBuilder()
            .WithUrn(_urn)
            .WithQualificationType(_qualificationType)
            .WithKS5(true)
            .Build();

        _level2QualificationsService.Setup(s => s.GetLevel2QualificationDetailsAsync(_urn, _qualificationType, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_level2QualificationModel);
    }

    [Theory]
    [InlineData(Level2.TechCert)]
    public async Task Level2QualificationsPage_HasCorrectTitle(Level2 qualification)
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
            Level2.TechCert => PageTitleConstants.KS5SchoolPageTitles.Level2QualificationsTechCert,
            _ => null
        };
        var expectedTitle = $"{PageTitleConstants.KS5SchoolPageTitles.PhaseTitle} - {suffixTitle}";
        Assert.Contains(expectedTitle, title.TextContent.Trim());
    }

    [Theory]
    [InlineData(Level2.TechCert)]
    public async Task Level2QualificationsPage_DisplaysMainHeading(Level2 qualification)
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
    [InlineData(Level2.TechCert)]
    public async Task Level2QualificationsPage_DisplaysHeading(Level2 qualification)
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
    public async Task Level2QualificationsPage_Displays_VerticalNavigation()
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
    public async Task Level2QualificationsPage_Has_Correct_Sub_Navigation_Links()
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
    [InlineData(Level2.TechCert)]
    public async Task Level2QualificationsPage_Displays_Apprenticeships_Related_Links(Level2 qualification)
    {
        // Arrange
        SetupMocks(qualification);
        var pageRouteUrl = $"{_pageRoute}/{_qualificationType.ToString().ToLower()}";
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, pageRouteUrl);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var findStatisticsLink = doc.QuerySelector("#find-statistics-link");
        var findApprenticeshipLink = doc.QuerySelector("#find-apprenticeship-link");

        Assert.NotNull(findStatisticsLink);
        Assert.Contains("https://explore-education-statistics.service.gov.uk/find-statistics", findStatisticsLink.GetAttribute("href"));

        Assert.NotNull(findApprenticeshipLink);
        Assert.Contains("https://www.gov.uk/apply-apprenticeship", findApprenticeshipLink.GetAttribute("href"));
    }

    [Theory]
    [InlineData(Level2.TechCert)]
    public async Task Level2QualificationsPage_DisplaysProgressScoreHeading(Level2 qualification)
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
    [InlineData(Level2.TechCert)]
    public async Task Level2QualificationsPage_DisplaysTechnicalVocationalQualificationsLink(Level2 qualification)
    {
        // Arrange
        SetupMocks(qualification);
        var pageRouteUrl = $"{_pageRoute}/{_qualificationType.ToString().ToLower()}";
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, pageRouteUrl);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var techVocationalQualificationsLink = doc.QuerySelector("#tech-vocational-qualifications-link");

        if (qualification == Level2.TechCert)
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
    [InlineData(Level2.TechCert)]
    public async Task Level2QualificationsPage_Displays_Inset_Text(Level2 qualification)
    {
        // Arrange
        SetupMocks(qualification);
        var pageRouteUrl = $"{_pageRoute}/{_qualificationType.ToString().ToLower()}";
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, pageRouteUrl);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var techCertInsetText = doc.QuerySelector("#tech-cert-inset-text");
        Assert.NotNull(techCertInsetText);
    }

    [Theory]
    [InlineData(Level2.TechCert)]
    public async Task Level2QualificationsPage_Displays_ProgressScore(Level2 qualification)
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
        Assert.Equal($"Number of students from this school or college included in the measure: {_level2QualificationModel.TotalNoOfStudentCompletedQualification}", noOfStudentsInfo.TextContent.Trim());

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
        Assert.Contains($"Students at this school score {_level2QualificationModel.ProgressScore.Score}", progresScoreCard.QuerySelectorAll("p")[0].TextContent);
        Assert.Contains($"This is above average", progresScoreCard.QuerySelectorAll("p")[0].QuerySelector("span")?.TextContent);
        Assert.Contains($"The confidence interval is {_level2QualificationModel.ProgressScore.ConfidenceLevelLower} to {_level2QualificationModel.ProgressScore.ConfidenceLevelUpper}.", progresScoreCard.QuerySelectorAll("p")[1].TextContent);

        // Assert progress england average
        var averageProgresScoreNationalCard = doc.QuerySelector("#average-progress-score-national-card");
        Assert.NotNull(averageProgresScoreNationalCard);
        Assert.Contains($"Average progress score in England: {_level2QualificationModel.ProgressScore.EnglandAverageScore}", averageProgresScoreNationalCard.QuerySelector("p")?.TextContent);
    }

    [Theory]
    [InlineData(Level2.TechCert)]
    public async Task Level2QualificationsPage_Displays_AverageResult(Level2 qualification)
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
        Assert.Equal($"Number of students from this school or college included in the measure: {_level2QualificationModel.TotalNoOfStudentCompletedQualification}", noOfStudentsInfo.TextContent.Trim());

        // Assert performance points link
        var performancePointsLink = doc.QuerySelector("#performance-points-link");
        Assert.NotNull(performancePointsLink);
        Assert.Equal("https://www.gov.uk/government/publications/performance-points-a-practical-guide-to-key-stage-4-and-5-points", performancePointsLink.GetAttribute("href"));
        
        Assert.Contains("School or college", doc.GetTableHeaderContentByIdAndIndex("average-result-current-year-table", 1, 0));
        Assert.Contains(_level2QualificationModel.AverageResult.Establishment.Grade.ToString(), doc.GetTableCellContentByIdAndIndex("average-result-current-year-table", 1, 0));
        Assert.Contains(_level2QualificationModel.AverageResult.Establishment.Points.Value!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("average-result-current-year-table", 1, 1));

        Assert.Contains($"{_level2QualificationModel.LAName} average", doc.GetTableHeaderContentByIdAndIndex("average-result-current-year-table", 2, 0));
        Assert.Contains(_level2QualificationModel.AverageResult.LocalAuthority.Grade.ToString(), doc.GetTableCellContentByIdAndIndex("average-result-current-year-table", 2, 0));
        Assert.Contains(_level2QualificationModel.AverageResult.LocalAuthority.Points.Value!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("average-result-current-year-table", 2, 1));

        Assert.Contains("England average", doc.GetTableHeaderContentByIdAndIndex("average-result-current-year-table", 3, 0));
        Assert.Contains(_level2QualificationModel.AverageResult.England.Grade.ToString(), doc.GetTableCellContentByIdAndIndex("average-result-current-year-table", 3, 0));
        Assert.Contains(_level2QualificationModel.AverageResult.England.Points.Value!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("average-result-current-year-table", 3, 1));
    }

    [Theory]
    [InlineData(Level2.TechCert)]
    public async Task Level2QualificationsPage_Displays_StudentRetention_Info(Level2 qualification)
    {
        // Arrange
        SetupMocks(qualification);
        var pageRouteUrl = $"{_pageRoute}/{_qualificationType.ToString().ToLower()}";
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, pageRouteUrl);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var studentRetentionInsetText = doc.QuerySelector("#student-retention");

        Assert.NotNull(studentRetentionInsetText);
        Assert.Equal("Measures on student retention will be available shortly in a future release.", studentRetentionInsetText.TextContent.Trim());
    }
}