using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
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
public class Level3QualificationsPageTests : PageTestsBase
{
    private string _pageRoute = "/16-to-19-performance/level-3-qualifications";
    private string _urn = "100279";
    private string _urnMultiPhase = "150009";
    private Level3 _qualificationType;
    private Level3QualificationModel _level3QualificationModel = null!;
    private readonly EstablishmentServiceModel _establishment = new();    
    private  EstablishmentServiceModel _multiPhaseEstablishment = new();
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
            .WithEstablishmentName($"School{_urn}")
            .WithQualificationType(_qualificationType)
            .WithKS5(true)
            .Build();

        _level3QualificationsService.Setup(s => s.GetLevel3QualificationDetailsAsync(_urn, _qualificationType, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_level3QualificationModel);
    }

    private void SetupMultiPhaseMocks(Level3 qualification = Level3.ALevel)
    {
        _qualificationType = qualification;
        _multiPhaseEstablishment = new EstablishmentTestBuilder()
            .WithURN(_urnMultiPhase)
            .WithEstablishmentName($"School{_urnMultiPhase}")
            .WithIsKeyStage4(true)
            .WithIsKeyStage5(true)
            .WithSixthForm(true)
            .BuildServiceModel();     

        var multiPhaseModel = new Level3QualificationsModelBuilder()
            .WithUrn(_urnMultiPhase)
            .WithEstablishmentName($"School{_urnMultiPhase}")
            .WithQualificationType(qualification)
            .WithKS4(true)
            .WithKS5(true)
            .Build();

        _level3QualificationsService
            .Setup(s => s.GetLevel3QualificationDetailsAsync(_urnMultiPhase, qualification, It.IsAny<CancellationToken>()))
            .ReturnsAsync(multiPhaseModel);
    }

    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    [InlineData(Level3.AppliedGeneral)]
    [InlineData(Level3.TechLevel)]
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
            Level3.TechLevel => PageTitleConstants.KS5SchoolPageTitles.Level3QualificationsTechLevel,
            _ => null
        };
        var expectedTitle = $"{PageTitleConstants.KS5SchoolPageTitles.PhaseTitle} - {suffixTitle}";
        Assert.Contains(expectedTitle, title.TextContent.Trim());
    }

    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    [InlineData(Level3.AppliedGeneral)]
    [InlineData(Level3.TechLevel)]
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
    [InlineData(Level3.TechLevel)]
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
    [InlineData(Level3.TechLevel)]
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
    [InlineData(Level3.TechLevel)]
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

        if (qualification == Level3.ALevel 
            || qualification == Level3.AppliedGeneral
            || qualification == Level3.TechLevel)
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
    [InlineData(Level3.TechLevel)]
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

        if (qualification == Level3.AppliedGeneral || qualification == Level3.TechLevel)
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
    [InlineData(Level3.TechLevel)]
    public async Task Level3QualificationsPage_Displays_ProgressScore(Level3 qualification)
    {
        // Arrange
        SetupMocks(qualification);
        var pageRouteUrl = $"{_pageRoute}/{_qualificationType.ToString().ToLower()}";
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, pageRouteUrl);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert no of students completed qualification

        var noOfStudentsInfo = doc.QuerySelector("#no-of-students-completed-qualification-info");
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
    [InlineData(Level3.TechLevel)]
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
        var noOfStudentsInfo = doc.QuerySelector("#no-of-students-completed-qualification-info");
        Assert.NotNull(noOfStudentsInfo);
        Assert.Equal($"Number of students from this school or college included in the measure: {_level3QualificationModel.TotalNoOfStudentCompletedQualification}", noOfStudentsInfo.TextContent.Trim());

        // Assert performance points link
        var performancePointsLink = doc.QuerySelector("#performance-points-link");
        Assert.NotNull(performancePointsLink);
        Assert.Equal("https://www.gov.uk/government/publications/performance-points-a-practical-guide-to-key-stage-4-and-5-points", performancePointsLink.GetAttribute("href"));

        if (qualification == Level3.TechLevel)
        {
            var measuresTechGuidanceLink = doc.QuerySelector("#measures-tech-guidance-link");
            Assert.NotNull(measuresTechGuidanceLink);
            Assert.Equal("https://www.gov.uk/government/publications/16-to-19-accountability-headline-measures-technical-guide", measuresTechGuidanceLink.GetAttribute("href"));

            var introTlevelsLink = doc.QuerySelector("#intro-t-levels-link");
            Assert.NotNull(introTlevelsLink);
            Assert.Equal("https://www.gov.uk/government/publications/introduction-of-t-levels/introduction-of-t-levels", introTlevelsLink.GetAttribute("href"));
        }

        Assert.Contains("School or college", doc.GetTableHeaderContentByIdAndIndex("average-result-current-year-table", 1, 0));
        Assert.Contains(_level3QualificationModel.AverageResult.Establishment.Grade.ToString(), doc.GetTableCellContentByIdAndIndex("average-result-current-year-table", 1, 0));
        Assert.Contains(_level3QualificationModel.AverageResult.Establishment.Points.Value!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("average-result-current-year-table", 1, 1));

        Assert.Contains($"{_level3QualificationModel.LAName} average", doc.GetTableHeaderContentByIdAndIndex("average-result-current-year-table", 2, 0));
        Assert.Contains(_level3QualificationModel.AverageResult.LocalAuthority.Grade.ToString(), doc.GetTableCellContentByIdAndIndex("average-result-current-year-table", 2, 0));
        Assert.Contains(_level3QualificationModel.AverageResult.LocalAuthority.Points.Value!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("average-result-current-year-table", 2, 1));

        Assert.Contains("England average", doc.GetTableHeaderContentByIdAndIndex("average-result-current-year-table", 3, 0));
        Assert.Contains(_level3QualificationModel.AverageResult.England.Grade.ToString(), doc.GetTableCellContentByIdAndIndex("average-result-current-year-table", 3, 0));
        Assert.Contains(_level3QualificationModel.AverageResult.England.Points.Value!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("average-result-current-year-table", 3, 1));
    }

    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    [InlineData(Level3.AppliedGeneral)]
    [InlineData(Level3.TechLevel)]
    public async Task Level3QualificationsPage_Displays_AdditionalData(Level3 qualification)
    {
        // Arrange
        SetupMocks(qualification);
        var pageRouteUrl = $"{_pageRoute}/{_qualificationType.ToString().ToLower()}";
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, pageRouteUrl);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var additionalDetails = doc.QuerySelector("#additional-data-details");
        var tableId = "additional-data-current-year-table";
        var additionalDetailsTable = doc.QuerySelector($"#{tableId}");

        if (qualification == Level3.ALevel)
        {
            // Assert additional details section
            Assert.NotNull(additionalDetails);

            // Assert additional details table
            Assert.NotNull(additionalDetailsTable);

            // Assert no of students included in this measure
            var noOfStudentsInfo = doc.QuerySelector("#no-of-students-included-in-measure-info");
            Assert.NotNull(noOfStudentsInfo);
            Assert.Equal($"Number of students included in these measures: {_level3QualificationModel.AdditionalData!.TotalNoOfStudentsIncludedInThisMeasure}", noOfStudentsInfo.TextContent.Trim());

            Assert.Contains("School or college", doc.GetTableHeaderContentByIdAndIndex(tableId, 1, 0));
            Assert.Contains(_level3QualificationModel.AdditionalData.Establishment.Grade.ToString(), doc.GetTableCellContentByIdAndIndex(tableId, 1, 0));
            Assert.Contains(_level3QualificationModel.AdditionalData.Establishment.Points.Value!.Value.ToString(), doc.GetTableCellContentByIdAndIndex(tableId, 1, 1));

            Assert.Contains($"{_level3QualificationModel.LAName} average", doc.GetTableHeaderContentByIdAndIndex(tableId, 2, 0));
            Assert.Contains(_level3QualificationModel.AdditionalData.LocalAuthority.Grade.ToString(), doc.GetTableCellContentByIdAndIndex(tableId, 2, 0));
            Assert.Contains(_level3QualificationModel.AdditionalData.LocalAuthority.Points.Value!.Value.ToString(), doc.GetTableCellContentByIdAndIndex(tableId, 2, 1));

            Assert.Contains("England average", doc.GetTableHeaderContentByIdAndIndex(tableId, 3, 0));
            Assert.Contains(_level3QualificationModel.AdditionalData.England.Grade.ToString(), doc.GetTableCellContentByIdAndIndex(tableId, 3, 0));
            Assert.Contains(_level3QualificationModel.AdditionalData.England.Points.Value!.Value.ToString(), doc.GetTableCellContentByIdAndIndex(tableId, 3, 1));
        }
        else
        {
            Assert.Null(additionalDetails);
            Assert.Null(additionalDetailsTable);
        }        
    }

    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    [InlineData(Level3.AppliedGeneral)]
    [InlineData(Level3.TechLevel)]
    public async Task Level3QualificationsPage_Displays_AdvancedLevelMathsQualificationData(Level3 qualification)
    {
        // Arrange
        SetupMocks(qualification);
        var pageRouteUrl = $"{_pageRoute}/{_qualificationType.ToString().ToLower()}";
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, pageRouteUrl);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var advanceLevelMathsQualDetails = doc.QuerySelector("#advanced-level-maths-qualifications-details");
        var tableId = "advanced-level-maths-qualifications-table";
        var advancedLevelMathsQualTable = doc.QuerySelector($"#{tableId}");

        if (qualification == Level3.Academic)
        {
            // Assert advance level maths qualification details section
            Assert.NotNull(advanceLevelMathsQualDetails);

            // Assert advance level maths qualification details table
            Assert.NotNull(advancedLevelMathsQualTable);


            Assert.Contains("School or college", doc.GetTableHeaderContentByIdAndIndex(tableId, 0, 0));
            Assert.Contains(_level3QualificationModel.AdvancedLevelMathsQualificationData!.SchoolOrCollege.ToString() + "%", doc.GetTableCellContentByIdAndIndex(tableId, 0, 0));

            Assert.Contains($"{_level3QualificationModel.LAName} average", doc.GetTableHeaderContentByIdAndIndex(tableId, 1, 0));
            Assert.Contains(_level3QualificationModel.AdvancedLevelMathsQualificationData!.LocalAuthority.ToString() + "%", doc.GetTableCellContentByIdAndIndex(tableId, 1, 0));

            Assert.Contains("England average", doc.GetTableHeaderContentByIdAndIndex(tableId, 2, 0));
            Assert.Contains(_level3QualificationModel.AdvancedLevelMathsQualificationData.England.ToString() + "%", doc.GetTableCellContentByIdAndIndex(tableId, 2, 0));
        }
        else
        {
            Assert.Null(advanceLevelMathsQualDetails);
            Assert.Null(advancedLevelMathsQualTable);
        }
    }

    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    [InlineData(Level3.AppliedGeneral)]
    [InlineData(Level3.TechLevel)]
    public async Task Level3QualificationsPage_Displays_StudentRetention_Info(Level3 qualification)
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

    [Fact]
    public async Task Level3QualificationsPage_DisplaysBottomPagination_WithCorrectDestinations()
    {
        // Arrange
        SetupMocks();
        var pageRouteUrl = $"{_pageRoute}/{_qualificationType.ToString().ToLower()}";
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, pageRouteUrl);

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
        Assert.Contains("/16-to-19-performance/level-2-qualifications", nextLink.GetAttribute("href"));
    }

    [Fact]
    public async Task Level3QualificationsPage_DisplaysBottomPagination_WithCorrectDestinations_WhenMultiplePhases()
    {
        // Arrange
        SetupMultiPhaseMocks();
        var pageRouteUrl = $"{_pageRoute}/{_qualificationType.ToString().ToLower()}";
        var url = BuildUrl(_multiPhaseEstablishment.URN, _multiPhaseEstablishment.EstablishmentName, pageRouteUrl);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var pagination = doc.QuerySelector("nav.govuk-pagination");
        Assert.NotNull(pagination);

        var previousLink = pagination.QuerySelector(".govuk-pagination__prev a");
        var nextLink = pagination.QuerySelector(".govuk-pagination__next a");

        Assert.NotNull(previousLink);
        Assert.Contains("/secondary-performance/additional-measures", previousLink.GetAttribute("href"));

        Assert.NotNull(nextLink);
        Assert.Contains("/16-to-19-performance/level-2-qualifications", nextLink.GetAttribute("href"));
    }
}
