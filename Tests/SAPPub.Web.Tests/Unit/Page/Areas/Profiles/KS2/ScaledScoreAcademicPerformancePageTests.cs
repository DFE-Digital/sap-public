using Moq;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Profiles;

[Collection("WebAppCollection")]
public class ScaledScoresAcademicPerformacePageTests : PageTestsBase
{
    private string _pageRoute = "/primary-performance/subject-scaled-scores";
    private string _urn = "149976";
    private string _laName = "Test LA";
    private readonly EstablishmentMinimumServiceModel _establishment = new();
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;

    private readonly KS2ScaledScoreModel _scaledScoreModel;
    private readonly Mock<IKS2ScaledScoreService> _scaledScoreService  = new();
    
    public ScaledScoresAcademicPerformacePageTests(WebAppFixture fixture) : base(fixture)
    {
        _scaledScoreService = UseMock<IKS2ScaledScoreService>();
        _mockEstablishmentService = UseMock<IEstablishmentService>();
        _establishment = new EstablishmentMinimumTestBuilder()
            .WithURN(_urn)
            .WithEstablishmentName($"School{_urn}")
            .WithIsKeyStage2(true)
            .WithLAName(_laName)
            .BuildServiceModel();

        _scaledScoreModel = GetScaledScoreModel();

        _mockEstablishmentService
           .Setup(a => a.GetEstablishmentMinimumAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
           .ReturnsAsync(_establishment);

        _scaledScoreService
            .Setup(s => s.GetScaledScoreModel(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_scaledScoreModel);
    }

    [Fact]
    public async Task ScaledScore_HasCorrectTitle()
    {
        // Arrange
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var title = doc.QuerySelector("title");
        Assert.NotNull(title);

        var expectedTitle = $"School149976 - Primary Subject scaled scores - School Profiles - GOV.UK";
        Assert.Contains(expectedTitle, title.TextContent.Trim());
    }

    [Fact]
    public async Task ScaledScore_DisplaysHeading()
    {
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var heading = doc.QuerySelectorAll("h2");
        Assert.NotNull(heading[1]);
        Assert.Contains("Scaled scores", heading[1].TextContent.Trim());
    }

    [Fact]
    public async Task ScaledScore_Displays_VerticalNavigation()
    {
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        Assert.NotNull(doc.QuerySelector(".moj-side-navigation"));
        Assert.Equal(5, doc.QuerySelectorAll(".moj-side-navigation__item").Length);
        Assert.Single(doc.QuerySelectorAll(".moj-side-navigation__item--active"));
    }

    [Fact]
    public async Task ScaledScore_Has_Correct_Sub_Navigation_Links()
    {
        // Arrange
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);
        var container = doc.QuerySelector("#sub-navigation-academic-performance");
        var links = container?.QuerySelectorAll(".moj-sub-navigation__link");

        Assert.NotNull(links);
        Assert.Equal(4, links.Length);
    }

    [Fact]
    public async Task ScaledScore_Displays_Read_AverageScore()
    {
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        Assert.Contains("School", doc.GetTableHeaderContentByIdAndIndex("read-data-overtime-table", 1, 0));
        Assert.Contains($"{_laName} average", doc.GetTableHeaderContentByIdAndIndex("read-data-overtime-table", 2, 0));
        Assert.Contains("England average", doc.GetTableHeaderContentByIdAndIndex("read-data-overtime-table", 3, 0));

        var expectedModel = GetScaledScoreModel();

        Assert.Equal("2022 to 2023", doc.GetTableHeaderContentByIdAndIndex("read-data-overtime-table", 0, 1));
        Assert.Equal(expectedModel.ReadAverageEstablishment!.TwoYearsAgo!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("read-data-overtime-table", 1, 0));
        Assert.Equal(expectedModel.ReadAverageLA!.TwoYearsAgo!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("read-data-overtime-table", 2, 0));
        Assert.Equal(expectedModel.ReadAverageEngland!.TwoYearsAgo!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("read-data-overtime-table", 3, 0));

        Assert.Equal("2023 to 2024", doc.GetTableHeaderContentByIdAndIndex("read-data-overtime-table", 0, 2));
        Assert.Equal(expectedModel.ReadAverageEstablishment!.PreviousYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("read-data-overtime-table", 1, 1));
        Assert.Equal(expectedModel.ReadAverageLA!.PreviousYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("read-data-overtime-table", 2, 1));
        Assert.Equal(expectedModel.ReadAverageEngland!.PreviousYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("read-data-overtime-table", 3, 1));

        Assert.Equal("2024 to 2025", doc.GetTableHeaderContentByIdAndIndex("read-data-overtime-table", 0, 3));
        Assert.Equal(expectedModel.ReadAverageEstablishment!.CurrentYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("read-data-overtime-table", 1, 2));
        Assert.Equal(expectedModel.ReadAverageLA!.CurrentYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("read-data-overtime-table", 2, 2));
        Assert.Equal(expectedModel.ReadAverageEngland!.CurrentYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("read-data-overtime-table", 3, 2));
    }


    [Fact]
    public async Task ScaledScore_Displays_Maths_AverageScore()
    {
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        Assert.Contains("School", doc.GetTableHeaderContentByIdAndIndex("maths-data-overtime-table", 1, 0));
        Assert.Contains($"{_laName} average", doc.GetTableHeaderContentByIdAndIndex("maths-data-overtime-table", 2, 0));
        Assert.Contains("England average", doc.GetTableHeaderContentByIdAndIndex("maths-data-overtime-table", 3, 0));

        var expectedModel = GetScaledScoreModel();

        Assert.Equal("2022 to 2023", doc.GetTableHeaderContentByIdAndIndex("maths-data-overtime-table", 0, 1));
        Assert.Equal(expectedModel.MathsAverageEstablishment!.TwoYearsAgo!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("maths-data-overtime-table", 1, 0));
        Assert.Equal(expectedModel.MathsAverageLA!.TwoYearsAgo!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("maths-data-overtime-table", 2, 0));
        Assert.Equal(expectedModel.MathsAverageEngland!.TwoYearsAgo!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("maths-data-overtime-table", 3, 0));

        Assert.Equal("2023 to 2024", doc.GetTableHeaderContentByIdAndIndex("maths-data-overtime-table", 0, 2));
        Assert.Equal(expectedModel.MathsAverageEstablishment!.PreviousYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("maths-data-overtime-table", 1, 1));
        Assert.Equal(expectedModel.MathsAverageLA!.PreviousYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("maths-data-overtime-table", 2, 1));
        Assert.Equal(expectedModel.MathsAverageEngland!.PreviousYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("maths-data-overtime-table", 3, 1));

        Assert.Equal("2024 to 2025", doc.GetTableHeaderContentByIdAndIndex("maths-data-overtime-table", 0, 3));
        Assert.Equal(expectedModel.MathsAverageEstablishment!.CurrentYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("maths-data-overtime-table", 1, 2));
        Assert.Equal(expectedModel.MathsAverageLA!.CurrentYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("maths-data-overtime-table", 2, 2));
        Assert.Equal(expectedModel.MathsAverageEngland!.CurrentYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("maths-data-overtime-table", 3, 2));
    }

    [Fact]
    public async Task ScaledScore_ByPupilCharacteristic_DisplaysCorrectInformation()
    {
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        var accordion = doc.GetElementById("scaled-scores-by-pupil-characteristic-accordion");

        Assert.NotNull(accordion);

        var accordionSectionHeaders = accordion.GetElementsByTagName("h4");
        Assert.Contains("Girls and boys", accordionSectionHeaders[0].TextContent);
        Assert.Contains("English as an additional language (EAL)", accordionSectionHeaders[1].TextContent);
        Assert.Contains("Non-mobile pupils", accordionSectionHeaders[2].TextContent);
        Assert.Contains("Disadvantaged pupils", accordionSectionHeaders[3].TextContent);
        Assert.Contains("Non-disadvantaged pupils", accordionSectionHeaders[4].TextContent);

        Assert.Contains("Pupil group", doc.GetTableHeaderContentByIdAndIndex("girls-boys-table", 0, 0));
        Assert.Contains("Average score in reading", doc.GetTableHeaderContentByIdAndIndex("girls-boys-table", 0, 1));
        Assert.Contains("Average score in maths", doc.GetTableHeaderContentByIdAndIndex("girls-boys-table", 0,2 ));
        Assert.Contains("Girls", doc.GetTableHeaderContentByIdAndIndex("girls-boys-table", 1, 0));
        Assert.Contains("33", doc.GetTableCellContentByIdAndIndex("girls-boys-table", 1,0));
        Assert.Contains("32", doc.GetTableCellContentByIdAndIndex("girls-boys-table", 1, 1));
        Assert.Contains("Boys", doc.GetTableHeaderContentByIdAndIndex("girls-boys-table", 2, 0));
        Assert.Contains("22", doc.GetTableCellContentByIdAndIndex("girls-boys-table", 2, 0));
        Assert.Contains("21", doc.GetTableCellContentByIdAndIndex("girls-boys-table", 2, 1));
        Assert.Contains("All pupils at the school", doc.GetTableHeaderContentByIdAndIndex("girls-boys-table", 3, 0));
        Assert.Contains("35", doc.GetTableCellContentByIdAndIndex("girls-boys-table", 3, 0));
        Assert.Contains("19", doc.GetTableCellContentByIdAndIndex("girls-boys-table", 3, 1));

        Assert.Contains("Pupil group", doc.GetTableHeaderContentByIdAndIndex("eal-table", 0, 0));
        Assert.Contains("Average score in reading", doc.GetTableHeaderContentByIdAndIndex("eal-table", 0, 1));
        Assert.Contains("Average score in maths", doc.GetTableHeaderContentByIdAndIndex("eal-table", 0, 2));
        Assert.Contains("Pupils with EAL", doc.GetTableHeaderContentByIdAndIndex("eal-table", 1, 0));
        Assert.Contains("30", doc.GetTableCellContentByIdAndIndex("eal-table", 1, 0));
        Assert.Contains("29", doc.GetTableCellContentByIdAndIndex("eal-table", 1, 1));
        Assert.Contains("All pupils at the school", doc.GetTableHeaderContentByIdAndIndex("eal-table", 2, 0));
        Assert.Contains("20", doc.GetTableCellContentByIdAndIndex("eal-table", 2, 0));
        Assert.Contains("31", doc.GetTableCellContentByIdAndIndex("eal-table", 2, 1));

        Assert.Contains("Pupil group", doc.GetTableHeaderContentByIdAndIndex("nonmobile-pupils-table", 0, 0));
        Assert.Contains("Average score in reading", doc.GetTableHeaderContentByIdAndIndex("nonmobile-pupils-table", 0, 1));
        Assert.Contains("Average score in maths", doc.GetTableHeaderContentByIdAndIndex("nonmobile-pupils-table", 0, 2));
        Assert.Contains("Non-mobile pupils", doc.GetTableHeaderContentByIdAndIndex("nonmobile-pupils-table", 1, 0));
        Assert.Contains("40", doc.GetTableCellContentByIdAndIndex("nonmobile-pupils-table", 1, 0));
        Assert.Contains("39", doc.GetTableCellContentByIdAndIndex("nonmobile-pupils-table", 1, 1));
        Assert.Contains("All pupils at the school", doc.GetTableHeaderContentByIdAndIndex("nonmobile-pupils-table", 2, 0));
        Assert.Contains("35", doc.GetTableCellContentByIdAndIndex("nonmobile-pupils-table", 2, 0));
        Assert.Contains("19", doc.GetTableCellContentByIdAndIndex("nonmobile-pupils-table", 2, 1));

        Assert.Contains("Pupil group (Disadvantaged)", doc.GetTableHeaderContentByIdAndIndex("disadvantaged-pupils-table", 0, 0));
        Assert.Contains("Average score in reading", doc.GetTableHeaderContentByIdAndIndex("disadvantaged-pupils-table", 0, 1));
        Assert.Contains("Average score in maths", doc.GetTableHeaderContentByIdAndIndex("disadvantaged-pupils-table", 0, 2));
        Assert.Contains("School", doc.GetTableHeaderContentByIdAndIndex("disadvantaged-pupils-table", 1, 0));
        Assert.Contains("27", doc.GetTableCellContentByIdAndIndex("disadvantaged-pupils-table", 1, 0));
        Assert.Contains("24", doc.GetTableCellContentByIdAndIndex("disadvantaged-pupils-table", 1, 1));
        Assert.Contains("TEST LA average", doc.GetTableHeaderContentByIdAndIndex("disadvantaged-pupils-table", 2, 0));
        Assert.Contains("28", doc.GetTableCellContentByIdAndIndex("disadvantaged-pupils-table", 2, 0));
        Assert.Contains("25", doc.GetTableCellContentByIdAndIndex("disadvantaged-pupils-table", 2, 1));
        Assert.Contains("England average", doc.GetTableHeaderContentByIdAndIndex("disadvantaged-pupils-table", 3, 0));
        Assert.Contains("26", doc.GetTableCellContentByIdAndIndex("disadvantaged-pupils-table", 3, 0));
        Assert.Contains("23", doc.GetTableCellContentByIdAndIndex("disadvantaged-pupils-table", 3, 1));

        Assert.Contains("Pupil group (Non-disadvantaged)", doc.GetTableHeaderContentByIdAndIndex("non-disadvantaged-pupils-table", 0, 0));
        Assert.Contains("Average score in reading", doc.GetTableHeaderContentByIdAndIndex("non-disadvantaged-pupils-table", 0, 1));
        Assert.Contains("Average score in maths", doc.GetTableHeaderContentByIdAndIndex("non-disadvantaged-pupils-table", 0, 2));
        Assert.Contains("TEST LA average", doc.GetTableHeaderContentByIdAndIndex("non-disadvantaged-pupils-table", 1, 0));
        Assert.Contains("38", doc.GetTableCellContentByIdAndIndex("non-disadvantaged-pupils-table", 1, 0));
        Assert.Contains("36", doc.GetTableCellContentByIdAndIndex("non-disadvantaged-pupils-table", 1, 1));
        Assert.Contains("England average", doc.GetTableHeaderContentByIdAndIndex("non-disadvantaged-pupils-table", 2, 0));
        Assert.Contains("37", doc.GetTableCellContentByIdAndIndex("non-disadvantaged-pupils-table", 2, 0));
        Assert.Contains("34", doc.GetTableCellContentByIdAndIndex("non-disadvantaged-pupils-table", 2, 1));
    }

    [Fact]
    public async Task ScaledScorePage_DisplaysBottomPagination_WithCorrectDestinations()
    {
        // Arrange
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var pagination = doc.QuerySelector("nav.govuk-pagination");
        Assert.NotNull(pagination);

        var previousLink = pagination.QuerySelector(".govuk-pagination__prev a");
        var nextLink = pagination.QuerySelector(".govuk-pagination__next a");

        Assert.NotNull(previousLink);
        Assert.Contains("/primary-performance/meeting-or-exceeding-standards", previousLink.GetAttribute("href"));

        Assert.NotNull(nextLink);
        Assert.Contains("/primary-performance/additional-measures", nextLink.GetAttribute("href"));
    }

    private KS2ScaledScoreModel GetScaledScoreModel()
    {
        return new KS2ScaledScoreModel
        {
            LAName = "TEST LA",
            ReadAverageEstablishment = new Core.Entities.RelativeYearValues<CodedDouble>()
            {
                CurrentYear = new CodedDouble(1, string.Empty, "1"),
                PreviousYear = new CodedDouble(2, string.Empty, "2"),
                TwoYearsAgo = new CodedDouble(3, string.Empty, "3"),
            },
            ReadAverageLA = new Core.Entities.RelativeYearValues<CodedDouble>()
            {
                CurrentYear = new CodedDouble(1.1, string.Empty, "1.1"),
                PreviousYear = new CodedDouble(2.1, string.Empty, "2.1"),
                TwoYearsAgo = new CodedDouble(3.1, string.Empty, "3.1"),
            },
            ReadAverageEngland = new Core.Entities.RelativeYearValues<CodedDouble>()
            {
                CurrentYear = new CodedDouble(1.2, string.Empty, "1.2"),
                PreviousYear = new CodedDouble(2.2, string.Empty, "2.2"),
                TwoYearsAgo = new CodedDouble(3.2, string.Empty, "3.2"),
            },
            MathsAverageEstablishment = new Core.Entities.RelativeYearValues<CodedDouble>()
            {
                CurrentYear = new CodedDouble(1.3, string.Empty, "1.3"),
                PreviousYear = new CodedDouble(2.2, string.Empty, "2.3"),
                TwoYearsAgo = new CodedDouble(3.3, string.Empty, "3.3"),
            },
            MathsAverageLA = new Core.Entities.RelativeYearValues<CodedDouble>()
            {
                CurrentYear = new CodedDouble(1.4, string.Empty, "1.4"),
                PreviousYear = new CodedDouble(2.4, string.Empty, "2.4"),
                TwoYearsAgo = new CodedDouble(3.4, string.Empty, "3.4"),
            },
            MathsAverageEngland = new Core.Entities.RelativeYearValues<CodedDouble>()
            {
                CurrentYear = new CodedDouble(1.5, string.Empty, "1.5"),
                PreviousYear = new CodedDouble(2.5, string.Empty, "2.5"),
                TwoYearsAgo = new CodedDouble(3.5, string.Empty, "3.5"),
            },
            AllPupilsAverageMaths = new CodedDouble(19, string.Empty, "19"),
            EALTotalAverageReading = new CodedDouble(20, string.Empty, "20"),
            BoysAverageMaths = new CodedDouble(21, string.Empty, "21"),
            BoysAverageReading = new CodedDouble(22, string.Empty, "22"),
            DisadvantagedAverageMathsEngland = new CodedDouble(23, string.Empty, "23"),
            DisadvantagedAverageMathsEstablishment = new CodedDouble(24, string.Empty, "24"),
            DisadvantagedAverageMathsLA = new CodedDouble(25, string.Empty, "25"),
            DisadvantagedAverageReadingEngland = new CodedDouble(26, string.Empty, "26"),
            DisadvantagedAverageReadingEstablishment = new CodedDouble(27, string.Empty, "27"),
            DisadvantagedAverageReadingLA = new CodedDouble(28, string.Empty, "28"),
            EALAverageMaths = new CodedDouble(29, string.Empty, "29"),
            EALAverageReading = new CodedDouble(30, string.Empty, "30"),
            EALTotalAverageMaths = new CodedDouble(31, string.Empty, "31"),
            GirlsAverageMaths = new CodedDouble(32, string.Empty, "32"),
            GirlsAverageReading = new CodedDouble(33, string.Empty, "33"),
            NonDisadvantagedAverageMathsEngland = new CodedDouble(34, string.Empty, "34"),
            AllPupilsAverageReading = new CodedDouble(35, string.Empty, "35"),
            NonDisadvantagedAverageMathsLA = new CodedDouble(36, string.Empty, "36"),
            NonDisadvantagedAverageReadingEngland = new CodedDouble(37, string.Empty, "37"),
            NonDisadvantagedAverageReadingLA = new CodedDouble(38, string.Empty, "38"),
            NonMobileAverageMaths = new CodedDouble(39, string.Empty, "39"),
            NonMobileAverageReading = new CodedDouble(40, string.Empty, "40"),
        };
    }
}
