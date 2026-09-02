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
    public async Task Level3QualificationsPage_DisplaysExpectedMainHeadings(
        Level3 qualification)
    {
        // Arrange
        SetupMocks(qualification);

        var pageRouteUrl =
            $"{_pageRoute}/{_qualificationType.ToString().ToLower()}";

        var url = BuildUrl(
            _establishment.URN,
            _establishment.EstablishmentName,
            pageRouteUrl);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert - school name is H1
        var h1Elements = doc.GetElementsByTagName("h1");

        Assert.Contains(
            h1Elements,
            x => x.TextContent.Trim() ==
                 _establishment.EstablishmentName);

        // Assert - page title is H2
        var h2Elements = doc.GetElementsByTagName("h2");

        Assert.Contains(
            h2Elements,
            x => x.TextContent.Trim() ==
                 PageTitleConstants.KS5SchoolPageTitles.Performance);
    }
    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    [InlineData(Level3.AppliedGeneral)]
    [InlineData(Level3.TechLevel)]
    public async Task Level3QualificationsPage_DisplaysExpectedHeadings(
        Level3 qualification)
    {
        // Arrange
        SetupMocks(qualification);

        var pageRouteUrl =
            $"{_pageRoute}/{_qualificationType.ToString().ToLower()}";

        var url = BuildUrl(
            _establishment.URN,
            _establishment.EstablishmentName,
            pageRouteUrl);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert - school name is H1
        var h1Elements = doc.GetElementsByTagName("h1");

        Assert.Contains(
            h1Elements,
            x => x.TextContent.Trim() ==
                 _establishment.EstablishmentName);

        // Assert - page headings are H2
        var h2Elements = doc.GetElementsByTagName("h2");

        Assert.Contains(
            h2Elements,
            x => x.TextContent.Trim() ==
                 PageTitleConstants.KS5SchoolPageTitles.Performance);

        var qualificationHeading =
            qualification.GetDisplayName();

        Assert.NotNull(qualificationHeading);

        Assert.Contains(
            h2Elements,
            x => x.TextContent.Trim().Contains(
                qualificationHeading!));
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
        Assert.Equal(4, doc.QuerySelectorAll(".moj-side-navigation__item").Length);
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
    public async Task Level3QualificationsPage_Displays_Apprenticeships_Related_Links(Level3 qualification)
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

    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    [InlineData(Level3.AppliedGeneral)]
    [InlineData(Level3.TechLevel)]
    public async Task Level3QualificationsPage_Displays_Disadvantaged_Students_Info(Level3 qualification)
    {
        // Arrange
        SetupMocks(qualification);
        var pageRouteUrl = $"{_pageRoute}/{_qualificationType.ToString().ToLower()}";
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, pageRouteUrl);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var disadvantagedStudentsAccordion = doc.QuerySelector("#disadvantaged-students-info-accordion");
        var tableId = "disadvantaged-students-table";
        var disadavantagedStudentsTable = doc.QuerySelector($"#{tableId}");

        if (qualification == Level3.ALevel ||
            qualification == Level3.Academic ||
            qualification == Level3.AppliedGeneral)
        {
            // Assert disadvantaged students info accordion
            Assert.NotNull(disadvantagedStudentsAccordion);

            // Assert disadvantaged students table
            Assert.NotNull(disadavantagedStudentsTable);

            Assert.Contains("Number of students", doc.GetTableHeaderContentByIdAndIndex(tableId, 1, 0));
            Assert.Contains(_level3QualificationModel.DisadvantagedStudentsData.Establishment!.NumberOfStudents.ToString(), doc.GetTableCellContentByIdAndIndex(tableId, 1, 0));
            Assert.Contains(_level3QualificationModel.DisadvantagedStudentsData.LocalAuthority.NumberOfStudents.ToString(), doc.GetTableCellContentByIdAndIndex(tableId, 1, 1));
            Assert.Contains(_level3QualificationModel.DisadvantagedStudentsData.England.NumberOfStudents.ToString(), doc.GetTableCellContentByIdAndIndex(tableId, 1, 2));

            Assert.Contains("Progress score", doc.GetTableHeaderContentByIdAndIndex(tableId, 2, 0));
            Assert.Contains(_level3QualificationModel.DisadvantagedStudentsData.Establishment!.ProgressScore.ToString(), doc.GetTableCellContentByIdAndIndex(tableId, 2, 0));
            Assert.Contains(_level3QualificationModel.DisadvantagedStudentsData.LocalAuthority.ProgressScore.ToString(), doc.GetTableCellContentByIdAndIndex(tableId, 2, 1));
            Assert.Contains(_level3QualificationModel.DisadvantagedStudentsData.England.ProgressScore.ToString(), doc.GetTableCellContentByIdAndIndex(tableId, 2, 2));

            Assert.Contains("Confidence interval", doc.GetTableHeaderContentByIdAndIndex(tableId, 3, 0));
            Assert.Contains($"{_level3QualificationModel.DisadvantagedStudentsData.Establishment!.ConfidenceLevelLower.ToString()} to {_level3QualificationModel.DisadvantagedStudentsData.Establishment!.ConfidenceLevelUpper.ToString()}", doc.GetTableCellContentByIdAndIndex(tableId, 3, 0));
            Assert.Contains($"{_level3QualificationModel.DisadvantagedStudentsData.LocalAuthority.ConfidenceLevelLower.ToString()} to {_level3QualificationModel.DisadvantagedStudentsData.LocalAuthority.ConfidenceLevelUpper.ToString()}", doc.GetTableCellContentByIdAndIndex(tableId, 3, 1));
            Assert.Contains($"{_level3QualificationModel.DisadvantagedStudentsData.England.ConfidenceLevelLower.ToString()} to {_level3QualificationModel.DisadvantagedStudentsData.England.ConfidenceLevelUpper.ToString()}", doc.GetTableCellContentByIdAndIndex(tableId, 3, 2));

            Assert.Contains("Grade", doc.GetTableHeaderContentByIdAndIndex(tableId, 4, 0));
            Assert.Contains(_level3QualificationModel.DisadvantagedStudentsData.Establishment!.Result.Grade.ToString(), doc.GetTableCellContentByIdAndIndex(tableId, 4, 0));
            Assert.Contains(_level3QualificationModel.DisadvantagedStudentsData.LocalAuthority.Result.Grade.ToString(), doc.GetTableCellContentByIdAndIndex(tableId, 4, 1));
            Assert.Contains(_level3QualificationModel.DisadvantagedStudentsData.England.Result.Grade.ToString(), doc.GetTableCellContentByIdAndIndex(tableId, 4, 2));

            Assert.Contains("Points", doc.GetTableHeaderContentByIdAndIndex(tableId, 5, 0));
            Assert.Contains(_level3QualificationModel.DisadvantagedStudentsData.Establishment!.Result.Points.ToString(), doc.GetTableCellContentByIdAndIndex(tableId, 5, 0));
            Assert.Contains(_level3QualificationModel.DisadvantagedStudentsData.LocalAuthority.Result.Points.ToString(), doc.GetTableCellContentByIdAndIndex(tableId, 5, 1));
            Assert.Contains(_level3QualificationModel.DisadvantagedStudentsData.England.Result.Points.ToString(), doc.GetTableCellContentByIdAndIndex(tableId, 5, 2));
        }
        else
        {
            Assert.Null(disadvantagedStudentsAccordion);
            Assert.Null(disadavantagedStudentsTable);
        }
    }

    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    [InlineData(Level3.AppliedGeneral)]
    [InlineData(Level3.TechLevel)]
    public async Task Level3QualificationsPage_Displays_NonDisadvantaged_Students_Info(Level3 qualification)
    {
        // Arrange
        SetupMocks(qualification);
        var pageRouteUrl = $"{_pageRoute}/{_qualificationType.ToString().ToLower()}";
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, pageRouteUrl);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        var nonDisadvantagedStudentsDetails = doc.QuerySelector("#non-disadvantaged-students-details");
        var tableId = "non-disadvantaged-students-table";
        var nonDisadavantagedStudentsTable = doc.QuerySelector($"#{tableId}");

        if (qualification == Level3.ALevel ||
            qualification == Level3.Academic ||
            qualification == Level3.AppliedGeneral)
        {
            // Assert nondisadvantaged students info accordion
            Assert.NotNull(nonDisadvantagedStudentsDetails);

            // Assert nondisadvantaged students table
            Assert.NotNull(nonDisadavantagedStudentsTable);

            Assert.Contains("Number of students", doc.GetTableHeaderContentByIdAndIndex(tableId, 1, 0));
            Assert.Contains(_level3QualificationModel.NonDisadvantagedStudentsData.LocalAuthority.NumberOfStudents.ToString(), doc.GetTableCellContentByIdAndIndex(tableId, 1, 0));
            Assert.Contains(_level3QualificationModel.NonDisadvantagedStudentsData.England.NumberOfStudents.ToString(), doc.GetTableCellContentByIdAndIndex(tableId, 1, 1));

            Assert.Contains("Progress score", doc.GetTableHeaderContentByIdAndIndex(tableId, 2, 0));
            Assert.Contains(_level3QualificationModel.NonDisadvantagedStudentsData.LocalAuthority.ProgressScore.ToString(), doc.GetTableCellContentByIdAndIndex(tableId, 2, 0));
            Assert.Contains(_level3QualificationModel.NonDisadvantagedStudentsData.England.ProgressScore.ToString(), doc.GetTableCellContentByIdAndIndex(tableId, 2, 1));

            Assert.Contains("Confidence interval", doc.GetTableHeaderContentByIdAndIndex(tableId, 3, 0));
            Assert.Contains($"{_level3QualificationModel.NonDisadvantagedStudentsData.LocalAuthority.ConfidenceLevelLower.ToString()} to {_level3QualificationModel.NonDisadvantagedStudentsData.LocalAuthority.ConfidenceLevelUpper.ToString()}", doc.GetTableCellContentByIdAndIndex(tableId, 3, 0));
            Assert.Contains($"{_level3QualificationModel.NonDisadvantagedStudentsData.England.ConfidenceLevelLower.ToString()} to {_level3QualificationModel.NonDisadvantagedStudentsData.England.ConfidenceLevelUpper.ToString()}", doc.GetTableCellContentByIdAndIndex(tableId, 3, 1));

            Assert.Contains("Grade", doc.GetTableHeaderContentByIdAndIndex(tableId, 4, 0));
            Assert.Contains(_level3QualificationModel.NonDisadvantagedStudentsData.LocalAuthority.Result.Grade.ToString(), doc.GetTableCellContentByIdAndIndex(tableId, 4, 0));
            Assert.Contains(_level3QualificationModel.NonDisadvantagedStudentsData.England.Result.Grade.ToString(), doc.GetTableCellContentByIdAndIndex(tableId, 4, 1));

            Assert.Contains("Points", doc.GetTableHeaderContentByIdAndIndex(tableId, 5, 0));
            Assert.Contains(_level3QualificationModel.NonDisadvantagedStudentsData.LocalAuthority.Result.Points.ToString(), doc.GetTableCellContentByIdAndIndex(tableId, 5, 0));
            Assert.Contains(_level3QualificationModel.NonDisadvantagedStudentsData.England.Result.Points.ToString(), doc.GetTableCellContentByIdAndIndex(tableId, 5, 1));
        }
        else
        {
            Assert.Null(nonDisadvantagedStudentsDetails);
            Assert.Null(nonDisadavantagedStudentsTable);
        }
    }
}
