using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Admissions;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.KS4.Admissions;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Core.ValueObjects;
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

    private readonly KS2MeetingOrExceedingStandardsModel _kS2MeetingOrExceedingStandardsModel;
    private readonly Mock<IKS2MeetingOrExceedingStandardsService> _mockKS2MeetingOrExceedingStandardsService;

    public MeetingOrExceedingStandardsPageTests(WebAppFixture fixture) : base(fixture)
    {
        _mockEstablishmentService = UseMock<IEstablishmentService>();
        _mockAdmissionsService = UseMock<IAdmissionsService>();
        _mockKS2MeetingOrExceedingStandardsService = UseMock<IKS2MeetingOrExceedingStandardsService>();

        _establishment = new EstablishmentTestBuilder()
            .WithURN(_urn)
            .WithEstablishmentName(_schoolName)
            .WithIsKeyStage2(true)
            .WithIsKeyStage4(false)
            .WithWebsite("https://www.stpaulsacademy.co.uk")
            .WithEstablishmentTypeGroupId((int)EstablishmentTypeGroup.Academies)
            .WithLAName("TEST LA")
            .BuildServiceModel();

        _establishmentMinimum = new EstablishmentMinimumTestBuilder()
            .WithURN(_urn)
            .WithEstablishmentName(_schoolName)
            .WithIsKeyStage2(true)
            .WithIsKeyStage4(false)
            .WithWebsite("https://www.stpaulsacademy.co.uk")
            .WithLAName("TEST LA")
            .BuildServiceModel();

        _mockEstablishmentService
            .Setup(a => a.GetEstablishmentMinimumAsync(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_establishmentMinimum);

        _admissionsServiceModel = GetAdmissionsServiceModel(_schoolName, isKs2: true, isKs4: false, _establishment.Website);

        _mockAdmissionsService
            .Setup(s => s.GetAdmissionsDetailsAsync(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_admissionsServiceModel);

        _kS2MeetingOrExceedingStandardsModel = GetMeetingOrExceedingStandardsModel();

        _mockKS2MeetingOrExceedingStandardsService
            .Setup(s => s.GetMeetingOrExceedingStandardsPercentages(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_kS2MeetingOrExceedingStandardsModel);
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
    public async Task MeetingOrExceedingStandardsPage_ByPupilCharacteristic_DisplaysCorrectInformation()
    {
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        var accordion = doc.GetElementById("meeting-or-exceeing-standards-by-pupil-characteristic-accordion");

        Assert.NotNull(accordion);

        var accordionSectionHeaders = accordion.GetElementsByTagName("h4");
        Assert.Contains("Girls and boys", accordionSectionHeaders[0].TextContent);
        Assert.Contains("English as an additional language (EAL)", accordionSectionHeaders[1].TextContent);
        Assert.Contains("Non-mobile pupils", accordionSectionHeaders[2].TextContent);
        Assert.Contains("Disadvantaged pupils", accordionSectionHeaders[3].TextContent);
        Assert.Contains("Non-disadvantaged pupils", accordionSectionHeaders[4].TextContent);

        Assert.Contains("Pupil group", doc.GetTableHeaderContentByIdAndIndex("girls-boys-table", 0, 0));
        Assert.Contains("Meeting the expected standard in reading, writing and maths", doc.GetTableHeaderContentByIdAndIndex("girls-boys-table", 0, 1));
        Assert.Contains("Achieving at a higher standard in reading, writing and maths", doc.GetTableHeaderContentByIdAndIndex("girls-boys-table", 0, 2));
        Assert.Contains("Girls", doc.GetTableHeaderContentByIdAndIndex("girls-boys-table", 1, 0));
        Assert.Contains("7%", doc.GetTableCellContentByIdAndIndex("girls-boys-table", 1, 0));
        Assert.Contains("8%", doc.GetTableCellContentByIdAndIndex("girls-boys-table", 1, 1));
        Assert.Contains("Boys", doc.GetTableHeaderContentByIdAndIndex("girls-boys-table", 2, 0));
        Assert.Contains("9%", doc.GetTableCellContentByIdAndIndex("girls-boys-table", 2, 0));
        Assert.Contains("10%", doc.GetTableCellContentByIdAndIndex("girls-boys-table", 2, 1));
        Assert.Contains("All pupils at the school", doc.GetTableHeaderContentByIdAndIndex("girls-boys-table", 3, 0));
        Assert.Contains("11%", doc.GetTableCellContentByIdAndIndex("girls-boys-table", 3, 0));
        Assert.Contains("12%", doc.GetTableCellContentByIdAndIndex("girls-boys-table", 3, 1));

        Assert.Contains("Pupil group", doc.GetTableHeaderContentByIdAndIndex("eal-table", 0, 0));
        Assert.Contains("Meeting the expected standard in reading, writing and maths", doc.GetTableHeaderContentByIdAndIndex("eal-table", 0, 1));
        Assert.Contains("Achieving at a higher standard in reading, writing and maths", doc.GetTableHeaderContentByIdAndIndex("eal-table", 0, 2));
        Assert.Contains("Pupils with EAL", doc.GetTableHeaderContentByIdAndIndex("eal-table", 1, 0));
        Assert.Contains("13%", doc.GetTableCellContentByIdAndIndex("eal-table", 1, 0));
        Assert.Contains("14%", doc.GetTableCellContentByIdAndIndex("eal-table", 1, 1));
        Assert.Contains("All pupils at the school", doc.GetTableHeaderContentByIdAndIndex("eal-table", 2, 0));
        Assert.Contains("11%", doc.GetTableCellContentByIdAndIndex("eal-table", 2, 0));
        Assert.Contains("12%", doc.GetTableCellContentByIdAndIndex("eal-table", 2, 1));

        Assert.Contains("Pupil group", doc.GetTableHeaderContentByIdAndIndex("nonmobile-pupils-table", 0, 0));
        Assert.Contains("Meeting the expected standard in reading, writing and maths", doc.GetTableHeaderContentByIdAndIndex("nonmobile-pupils-table", 0, 1));
        Assert.Contains("Achieving at a higher standard in reading, writing and maths", doc.GetTableHeaderContentByIdAndIndex("nonmobile-pupils-table", 0, 2));
        Assert.Contains("Non-mobile pupils", doc.GetTableHeaderContentByIdAndIndex("nonmobile-pupils-table", 1, 0));
        Assert.Contains("15%", doc.GetTableCellContentByIdAndIndex("nonmobile-pupils-table", 1, 0));
        Assert.Contains("16%", doc.GetTableCellContentByIdAndIndex("nonmobile-pupils-table", 1, 1));
        Assert.Contains("All pupils at the school", doc.GetTableHeaderContentByIdAndIndex("nonmobile-pupils-table", 2, 0));
        Assert.Contains("11%", doc.GetTableCellContentByIdAndIndex("nonmobile-pupils-table", 2, 0));
        Assert.Contains("12%", doc.GetTableCellContentByIdAndIndex("nonmobile-pupils-table", 2, 1));

        Assert.Contains("Pupil group (Disadvantaged)", doc.GetTableHeaderContentByIdAndIndex("disadvantaged-pupils-table", 0, 0));
        Assert.Contains("Meeting the expected standard in reading, writing and maths", doc.GetTableHeaderContentByIdAndIndex("disadvantaged-pupils-table", 0, 1));
        Assert.Contains("Achieving at a higher standard in reading, writing and maths", doc.GetTableHeaderContentByIdAndIndex("disadvantaged-pupils-table", 0, 2));
        Assert.Contains("School", doc.GetTableHeaderContentByIdAndIndex("disadvantaged-pupils-table", 1, 0));
        Assert.Contains("17%", doc.GetTableCellContentByIdAndIndex("disadvantaged-pupils-table", 1, 0));
        Assert.Contains("18%", doc.GetTableCellContentByIdAndIndex("disadvantaged-pupils-table", 1, 1));
        Assert.Contains("TEST LA average", doc.GetTableHeaderContentByIdAndIndex("disadvantaged-pupils-table", 2, 0));
        Assert.Contains("19%", doc.GetTableCellContentByIdAndIndex("disadvantaged-pupils-table", 2, 0));
        Assert.Contains("20%", doc.GetTableCellContentByIdAndIndex("disadvantaged-pupils-table", 2, 1));
        Assert.Contains("England average", doc.GetTableHeaderContentByIdAndIndex("disadvantaged-pupils-table", 3, 0));
        Assert.Contains("21%", doc.GetTableCellContentByIdAndIndex("disadvantaged-pupils-table", 3, 0));
        Assert.Contains("22%", doc.GetTableCellContentByIdAndIndex("disadvantaged-pupils-table", 3, 1));

        Assert.Contains("Pupil group (Non-disadvantaged)", doc.GetTableHeaderContentByIdAndIndex("non-disadvantaged-pupils-table", 0, 0));
        Assert.Contains("Meeting the expected standard in reading, writing and maths", doc.GetTableHeaderContentByIdAndIndex("non-disadvantaged-pupils-table", 0, 1));
        Assert.Contains("Achieving at a higher standard in reading, writing and maths", doc.GetTableHeaderContentByIdAndIndex("non-disadvantaged-pupils-table", 0, 2));
        Assert.Contains("TEST LA average", doc.GetTableHeaderContentByIdAndIndex("non-disadvantaged-pupils-table", 1, 0));
        Assert.Contains("23%", doc.GetTableCellContentByIdAndIndex("non-disadvantaged-pupils-table", 1, 0));
        Assert.Contains("24%", doc.GetTableCellContentByIdAndIndex("non-disadvantaged-pupils-table", 1, 1));
        Assert.Contains("England average", doc.GetTableHeaderContentByIdAndIndex("non-disadvantaged-pupils-table", 2, 0));
        Assert.Contains("25%", doc.GetTableCellContentByIdAndIndex("non-disadvantaged-pupils-table", 2, 0));
        Assert.Contains("26%", doc.GetTableCellContentByIdAndIndex("non-disadvantaged-pupils-table", 2, 1));
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

    private static KS2MeetingOrExceedingStandardsModel GetMeetingOrExceedingStandardsModel() => new()
    {
        EstablishmentPercentageMeetingOrExceeding = new RelativeYearValues<CodedDouble> { CurrentYear = GetCodedDouble(1) },
        LocalAuthorityPercentageMeetingOrExceeding = new RelativeYearValues<CodedDouble> { CurrentYear = GetCodedDouble(2) },
        EnglandPercentageMeetingOrExceeding = new RelativeYearValues<CodedDouble> { CurrentYear = GetCodedDouble(3) },
        EstablishmentPercentageExceeding = new RelativeYearValues<CodedDouble> { CurrentYear = GetCodedDouble(4) },
        LocalAuthorityPercentageExceeding = new RelativeYearValues<CodedDouble> { CurrentYear = GetCodedDouble(5) },
        EnglandPercentageExceeding = new RelativeYearValues<CodedDouble> { CurrentYear = GetCodedDouble(6) },
        GirlsMeetingExpectedStandard = GetCodedDouble(7),
        GirlsExceedingExpectedStandard = GetCodedDouble(8),
        BoysMeetingExpectedStandard = GetCodedDouble(9),
        BoysExceedingExpectedStandard = GetCodedDouble(10),
        AllPupilsMeetingExpectedStandard = GetCodedDouble(11),
        AllPupilsExceedingExpectedStandard = GetCodedDouble(12),
        EALMeetingExpectedStandard = GetCodedDouble(13),
        EALExceedingExpectedStandard = GetCodedDouble(14),
        NonMobileMeetingExpectedStandard = GetCodedDouble(15),
        NonMobileExceedingExpectedStandard = GetCodedDouble(16),
        EstablishmentDisadvantagedMeetingExpectedStandard = GetCodedDouble(17),
        EstablishmentDisadvantagedExceedingExpectedStandard = GetCodedDouble(18),
        LocalAuthorityDisadvantagedMeetingExpectedStandard = GetCodedDouble(19),
        LocalAuthorityDisadvantagedExceedingExpectedStandard = GetCodedDouble(20),
        EnglandDisadvantagedMeetingExpectedStandard = GetCodedDouble(21),
        EnglandDisadvantagedExceedingExpectedStandard = GetCodedDouble(22),
        LocalAuthorityNonDisadvantagedMeetingExpectedStandard = GetCodedDouble(23),
        LocalAuthorityNonDisadvantagedExceedingExpectedStandard = GetCodedDouble(24),
        EnglandNonDisadvantagedMeetingExpectedStandard = GetCodedDouble(25),
        EnglandNonDisadvantagedExceedingExpectedStandard = GetCodedDouble(26),
    };

    private static CodedDouble GetCodedDouble(double val)
    {
        return new CodedDouble(val, string.Empty, val.ToString());
    }
}
