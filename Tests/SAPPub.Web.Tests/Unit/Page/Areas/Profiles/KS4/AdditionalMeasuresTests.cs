using Moq;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Helpers;
using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Profiles.KS4;

[Collection("WebAppCollection")]
public class AdditionalMeasuresTests : PageTestsBase
{
    private static string _pageRoute = "/secondary-performance/additional-measures";
    private readonly Mock<IAdditionalMeasuresService> _serviceMock;
    private readonly Mock<IEstablishmentService> _establishmentServiceMock;
    private readonly string _urn = "143034";
    private readonly string _establishmentName = "Loreto High School Chorlton";
    private readonly string _laId = "E08";

    public AdditionalMeasuresTests(WebAppFixture fixture) : base(fixture)
    {
        _serviceMock = UseMock<IAdditionalMeasuresService>();
        _establishmentServiceMock = UseMock<IEstablishmentService>();
        _establishmentServiceMock
            .Setup(service => service.GetEstablishmentMinimumAsync(
                _urn,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new EstablishmentMinimumTestBuilder()
                    .WithURN(_urn)
                    .WithEstablishmentName(_establishmentName)
                    .WithIsKeyStage4(true)
                    .WithLAId(_laId)
                    .BuildServiceModel());
    }

    [Fact]
    public async Task AdditionalMeasuresPage_HasCorrectTitle()
    {
        // Arrange
        var additionalmeasuresModel = GetAdditionalMeasuresModel(
            establishment: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            localAuthority: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            england: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build());

        _serviceMock
            .Setup(service => service.GetAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(additionalmeasuresModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _establishmentName, _pageRoute));

        // Assert
        var title = doc.Title;
        Assert.Contains("Loreto High School Chorlton - Secondary Additional measures - School Profiles - GOV.UK", title);
    }

    [Fact]
    public async Task AdditionalMeasuresPage_Displays_VerticalNavigation()
    {
        // Arrange
        var additionalmeasuresModel = GetAdditionalMeasuresModel(
            establishment: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            localAuthority: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            england: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build());

        _serviceMock
            .Setup(service => service.GetAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(additionalmeasuresModel);

        // Act
        var pageUrl = BuildUrl(_urn, _establishmentName, _pageRoute);
        var doc = await Fixture.BrowseToPage(pageUrl);

        var nav = new VerticalNavigationAssertHelper(doc);

        nav.ShouldBeVisibleAsync();
        nav.ShouldHaveItemsCountAsync(7);
        nav.ShouldHaveOneActiveItemAsync();
    }

    [Fact]
    public async Task AdditionalMeasuresPage_DisplaysPagination()
    {
        // Arrange
        var additionalmeasuresModel = GetAdditionalMeasuresModel(
            establishment: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            localAuthority: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            england: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build());

        _serviceMock
            .Setup(service => service.GetAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(additionalmeasuresModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _establishmentName, _pageRoute));

        // Act
        var bottomPagination = doc.QuerySelector("nav.govuk-pagination");
        Assert.NotNull(bottomPagination);


        // Act
        var previousPaginationLink = bottomPagination.QuerySelector(".govuk-pagination__prev a");
        var nextPaginationLink = bottomPagination.QuerySelector(".govuk-pagination__next a");
        var previousPaginationText = previousPaginationLink?.TextContent;
        var nextPaginationText = nextPaginationLink?.TextContent;

        // Assert
        var previousLink = bottomPagination.QuerySelector(".govuk-pagination__prev a");
        var nextLink = bottomPagination.QuerySelector(".govuk-pagination__next a");

        Assert.NotNull(previousLink);
        Assert.Contains("/secondary-performance/subjects-entered", previousLink.GetAttribute("href"));

        Assert.NotNull(nextLink);
        Assert.Contains("/destinations/secondary", nextLink.GetAttribute("href"));

    }

    [Fact]
    public async Task AdditionalMeasuresPage_ShowsTableWithCorrectData()
    {
        // Arrange
        var additionalmeasuresModel = GetAdditionalMeasuresModel(
            establishment: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            localAuthority: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().WithPupilsAtTheEndOfKS4(3400).Build(),
            england: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().WithPupilsAtTheEndOfKS4(32600).Build());

        _serviceMock
            .Setup(service => service.GetAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(additionalmeasuresModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _establishmentName, _pageRoute));

        // Assert
        var tableId = "additional-eanda-measures-table";

        // establishment values display correctly in the table
        var estVals = additionalmeasuresModel.EstablishmentCurrentYear;
        Assert.Contains($"{estVals.PercentAchievingAtLeastOneQualification.Value!.Value}%", doc.GetTableCellContentByIdAndIndex(tableId, 1, 0));
        Assert.Contains($"{estVals.PercentEnteredForTripleScience.Value!.Value}%", doc.GetTableCellContentByIdAndIndex(tableId, 2, 0));
        Assert.Contains($"{estVals.PercentEnteredMoreThanOneForeignLanguage.Value!.Value}%", doc.GetTableCellContentByIdAndIndex(tableId, 3, 0));


        // local authority values display correctly in the table
        var laValues = additionalmeasuresModel.LocalAuthorityCurrentYear;
        Assert.Contains($"{laValues.PercentAchievingAtLeastOneQualification.Value!.Value}%", doc.GetTableCellContentByIdAndIndex(tableId, 1, 1));
        Assert.Contains($"{laValues.PercentEnteredForTripleScience.Value!.Value}%", doc.GetTableCellContentByIdAndIndex(tableId, 2, 1));
        Assert.Contains($"{laValues.PercentEnteredMoreThanOneForeignLanguage.Value!.Value}%", doc.GetTableCellContentByIdAndIndex(tableId, 3, 1));


        // England values display correctly in the table
        var englandValues = additionalmeasuresModel.EnglandCurrentYear;
        Assert.Contains($"{englandValues.PercentAchievingAtLeastOneQualification.Value!.Value}%", doc.GetTableCellContentByIdAndIndex(tableId, 1, 2));
        Assert.Contains($"{englandValues.PercentEnteredForTripleScience.Value!.Value}%", doc.GetTableCellContentByIdAndIndex(tableId, 2, 2));
        Assert.Contains($"{englandValues.PercentEnteredMoreThanOneForeignLanguage.Value!.Value}%", doc.GetTableCellContentByIdAndIndex(tableId, 3, 2));

    }

    [Fact]
    public async Task AdditionalMeasuresPage_EstablishmentDataNotAvailable_ShowsTableValuesAsNotAvailable()
    {
        // Arrange
        var additionalmeasuresModel = GetAdditionalMeasuresModel(
            establishment: new AdditionalMeasuresBuilder().Build(),
            localAuthority: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().WithPupilsAtTheEndOfKS4(3400).Build(),
            england: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().WithPupilsAtTheEndOfKS4(32600).Build());

        _serviceMock
            .Setup(service => service.GetAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(additionalmeasuresModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _establishmentName, _pageRoute));

        // Assert
        var tableId = "additional-eanda-measures-table";

        var establishmentDataCellIndex = 0;
        Assert.Contains("Not available", doc.GetTableCellContentByIdAndIndex(tableId, 1, establishmentDataCellIndex));
        Assert.Contains("Not available", doc.GetTableCellContentByIdAndIndex(tableId, 2, establishmentDataCellIndex));
        Assert.Contains("Not available", doc.GetTableCellContentByIdAndIndex(tableId, 3, establishmentDataCellIndex));
    }

    [Fact]
    public async Task AdditionalMeasuresPage_LocalAuthorityDataNotAvailable_ShowsTableValuesAsNotAvailable()
    {
        // Arrange
        var additionalmeasuresModel = GetAdditionalMeasuresModel(
            england: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            establishment: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().WithPupilsAtTheEndOfKS4(32600).Build(),
            localAuthority: new AdditionalMeasuresBuilder().Build());


        _serviceMock
            .Setup(service => service.GetAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(additionalmeasuresModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _establishmentName, _pageRoute));

        // Assert
        var tableId = "additional-eanda-measures-table";

        var localAuthorityDataCellIndex = 1;
        Assert.Contains("Not available", doc.GetTableCellContentByIdAndIndex(tableId, 1, localAuthorityDataCellIndex));
        Assert.Contains("Not available", doc.GetTableCellContentByIdAndIndex(tableId, 2, localAuthorityDataCellIndex));
        Assert.Contains("Not available", doc.GetTableCellContentByIdAndIndex(tableId, 3, localAuthorityDataCellIndex));
    }

    [Fact]
    public async Task AdditionalMeasuresPage_EnglandDataNotAvailable_ShowsTableValuesAsNotAvailable()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "Loreto High School Chorlton";

        var additionalmeasuresModel = GetAdditionalMeasuresModel(
            establishment: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            localAuthority: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            england: new AdditionalMeasuresBuilder().Build());

        _serviceMock
            .Setup(service => service.GetAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(additionalmeasuresModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));

        // Assert
        var tableId = "additional-eanda-measures-table";

        var englandDataCellIndex = 2;
        Assert.Contains("Not available", doc.GetTableCellContentByIdAndIndex(tableId, 1, englandDataCellIndex));
        Assert.Contains("Not available", doc.GetTableCellContentByIdAndIndex(tableId, 2, englandDataCellIndex));
        Assert.Contains("Not available", doc.GetTableCellContentByIdAndIndex(tableId, 3, englandDataCellIndex));
    }

    [Fact]
    public async Task AdditionalMeasuresPage_ShowsAverageExamsEnteredTableWithCorrectData()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "Loreto High School Chorlton";

        var additionalMeasuresModel = GetAdditionalMeasuresModel(
            establishment: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            localAuthority: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            england: new AdditionalMeasuresBuilder().Build());

        _serviceMock
            .Setup(a => a.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(additionalMeasuresModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));

        // Assert
        var tableId = "additional-measures-exams-entered-table";

        Assert.Contains("GCSE qualifications", doc.GetTableCellContentByIdAndIndex(tableId, 1, 0));
        Assert.Contains(FormatNumber(additionalMeasuresModel.EstablishmentCurrentYear.AverageGCSEExamEntriesPerPupil,"N1"), doc.GetTableCellContentByIdAndIndex(tableId, 1, 1));
        Assert.Contains(FormatNumber(additionalMeasuresModel.LocalAuthorityCurrentYear.AverageGCSEExamEntriesPerPupil, "N1"), doc.GetTableCellContentByIdAndIndex(tableId, 1, 2));
        Assert.Contains(FormatNumber(additionalMeasuresModel.EnglandCurrentYear.AverageGCSEExamEntriesPerPupil, "N1"), doc.GetTableCellContentByIdAndIndex(tableId, 1, 3));

        Assert.Contains("All KS4 qualifications", doc.GetTableCellContentByIdAndIndex(tableId, 2, 0));
        Assert.Contains(FormatNumber(additionalMeasuresModel.EstablishmentCurrentYear.AverageAllKS4QualificationsExamEntriesPerPupil, "N1"), doc.GetTableCellContentByIdAndIndex(tableId,2 , 1));
        Assert.Contains(FormatNumber(additionalMeasuresModel.LocalAuthorityCurrentYear.AverageAllKS4QualificationsExamEntriesPerPupil, "N1"), doc.GetTableCellContentByIdAndIndex(tableId, 2, 2));
        Assert.Contains(FormatNumber(additionalMeasuresModel.EnglandCurrentYear.AverageAllKS4QualificationsExamEntriesPerPupil, "N1"), doc.GetTableCellContentByIdAndIndex(tableId, 2, 3));
    }

    [Fact]
    public async Task AdditionalMeasuresPage_ShowsAverageExamsEnteredDisadvantagedTableWithCorrectData()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "Loreto High School Chorlton";

        var additionalMeasuresModel = GetAdditionalMeasuresModel(
            establishment: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            localAuthority: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            england: new AdditionalMeasuresBuilder().Build());

        _serviceMock
            .Setup(a => a.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(additionalMeasuresModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));

        // Assert
        var tableId = "additional-measures-exams-entered-disadvantaged-table";

        Assert.Contains("GCSE qualifications", doc.GetTableHeaderContentByIdAndIndex(tableId, 1, 0));
        Assert.Contains(FormatNumber(additionalMeasuresModel.EstablishmentCurrentYear.AverageGCSEExamEntriesPerDisadvantagedPupil, "N1"), doc.GetTableCellContentByIdAndIndex(tableId, 1, 0));
        Assert.Contains(FormatNumber(additionalMeasuresModel.LocalAuthorityCurrentYear.AverageGCSEExamEntriesPerDisadvantagedPupil, "N1"), doc.GetTableCellContentByIdAndIndex(tableId, 1, 1));
        Assert.Contains(FormatNumber(additionalMeasuresModel.EnglandCurrentYear.AverageGCSEExamEntriesPerDisadvantagedPupil, "N1"), doc.GetTableCellContentByIdAndIndex(tableId, 1, 2));

        Assert.Contains("All KS4 qualifications", doc.GetTableHeaderContentByIdAndIndex(tableId, 2, 0));
        Assert.Contains(FormatNumber(additionalMeasuresModel.EstablishmentCurrentYear.AverageAllKS4QualificationsExamEntriesPerDisadvantagedPupil, "N1"), doc.GetTableCellContentByIdAndIndex(tableId, 2, 0));
        Assert.Contains(FormatNumber(additionalMeasuresModel.LocalAuthorityCurrentYear.AverageAllKS4QualificationsExamEntriesPerDisadvantagedPupil, "N1"), doc.GetTableCellContentByIdAndIndex(tableId, 2, 1));
        Assert.Contains(FormatNumber(additionalMeasuresModel.EnglandCurrentYear.AverageAllKS4QualificationsExamEntriesPerDisadvantagedPupil, "N1"), doc.GetTableCellContentByIdAndIndex(tableId, 2, 2));
    }

    [Fact]
    public async Task AdditionalMeasuresPage_ShowsAverageExamsEnteredNonDisadvantagedTableWithCorrectData()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "Loreto High School Chorlton";

        var additionalMeasuresModel = GetAdditionalMeasuresModel(
            establishment: new AdditionalMeasuresBuilder().Build(),
            localAuthority: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            england: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build());

        _serviceMock
            .Setup(a => a.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(additionalMeasuresModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));

        // Assert
        var tableId = "additional-measures-exams-entered-non-disadvantaged-table";

        Assert.Contains("GCSE qualifications", doc.GetTableHeaderContentByIdAndIndex(tableId, 1, 0));
        Assert.Contains(FormatNumber(additionalMeasuresModel.LocalAuthorityCurrentYear.AverageGCSEExamEntriesPerNonDisadvantagedPupil, "N1"), doc.GetTableCellContentByIdAndIndex(tableId, 1, 0));
        Assert.Contains(FormatNumber(additionalMeasuresModel.EnglandCurrentYear.AverageGCSEExamEntriesPerNonDisadvantagedPupil, "N1"), doc.GetTableCellContentByIdAndIndex(tableId, 1, 1));
        Assert.Contains("All KS4 qualifications", doc.GetTableHeaderContentByIdAndIndex(tableId, 2, 0));
        Assert.Contains(FormatNumber(additionalMeasuresModel.LocalAuthorityCurrentYear.AverageAllKS4QualificationsExamEntriesPerNonDisadvantagedPupil, "N1"), doc.GetTableCellContentByIdAndIndex(tableId, 2, 0));
        Assert.Contains(FormatNumber(additionalMeasuresModel.EnglandCurrentYear.AverageAllKS4QualificationsExamEntriesPerNonDisadvantagedPupil, "N1"), doc.GetTableCellContentByIdAndIndex(tableId, 2, 1));
    }

    [Fact]
    public async Task AdditionalMeasuresPage_ShowsNumberOfPupilsEndOfKs4TableWithCorrectData()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "Loreto High School Chorlton";

        var additionalMeasuresModel = GetAdditionalMeasuresModel(
            establishment: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            localAuthority: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            england: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build());

        _serviceMock
            .Setup(a => a.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(additionalMeasuresModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));

        // Assert
        var tableId = "num-pupil-eofks4-table";

        Assert.Contains("Number of pupils at the end of KS4", doc.GetTableHeaderContentByIdAndIndex(tableId, 1, 0));
        Assert.Contains(FormatNumber(additionalMeasuresModel.EstablishmentCurrentYear.NumberOfPupilsAtTheEndOfKS4, "N0"), doc.GetTableCellContentByIdAndIndex(tableId, 1, 0));
        Assert.Contains(FormatNumber(additionalMeasuresModel.LocalAuthorityCurrentYear.NumberOfPupilsAtTheEndOfKS4, "N0"), doc.GetTableCellContentByIdAndIndex(tableId, 1, 1));
        Assert.Contains(FormatNumber(additionalMeasuresModel.EnglandCurrentYear.NumberOfPupilsAtTheEndOfKS4, "N0"), doc.GetTableCellContentByIdAndIndex(tableId, 1, 2));
    }

    [Fact]
    public async Task AdditionalMeasuresPage_ShowsNumberOfPupilsEndOfKs4BreadkdownTableWithCorrectData()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "Loreto High School Chorlton";

        var additionalMeasuresModel = GetAdditionalMeasuresModel(
            establishment: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            localAuthority: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            england: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build());

        _serviceMock
            .Setup(a => a.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(additionalMeasuresModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));

        // Assert
        var tableId = "num-pupil-eofks4-breakdown-table";

        Assert.Contains("Girls", doc.GetTableHeaderContentByIdAndIndex(tableId, 1, 0));
        Assert.Contains("Boys", doc.GetTableHeaderContentByIdAndIndex(tableId, 2, 0));
        Assert.Contains("English as an additional language (EAL)", doc.GetTableHeaderContentByIdAndIndex(tableId, 3, 0));
        Assert.Contains("Non-mobile pupils", doc.GetTableHeaderContentByIdAndIndex(tableId, 4, 0));
        Assert.Contains(FormatNumber(additionalMeasuresModel.EstablishmentGirlsEndOfKS4), doc.GetTableCellContentByIdAndIndex(tableId, 1, 0));
        Assert.Contains(FormatNumber(additionalMeasuresModel.EstablishmentBoysEndOfKS4), doc.GetTableCellContentByIdAndIndex(tableId, 2, 0));
        Assert.Contains(FormatNumber(additionalMeasuresModel.EstablishmentEALEndOfKS4), doc.GetTableCellContentByIdAndIndex(tableId, 3, 0));
        Assert.Contains(FormatNumber(additionalMeasuresModel.EstablishmentNonMobilePupilsEndOfKS4), doc.GetTableCellContentByIdAndIndex(tableId, 4, 0));
    }

    [Fact]
    public async Task AdditionalMeasuresPage_ShowsNumberOfDisadvantagedPupilsEndOfKs4TableWithCorrectData()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "Loreto High School Chorlton";

        var additionalMeasuresModel = GetAdditionalMeasuresModel(
            establishment: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            localAuthority: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            england: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build());

        _serviceMock
            .Setup(a => a.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(additionalMeasuresModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));

        // Assert
        var tableId = "num-pupil-eofks4-disadvantaged-table";

        Assert.Contains("Number of disadvantaged pupils", doc.GetTableHeaderContentByIdAndIndex(tableId, 1, 0));
        Assert.Contains(FormatNumber(additionalMeasuresModel.EstablishmentDisadvantagedPupilsEndOfKS4), doc.GetTableCellContentByIdAndIndex(tableId, 1, 0));
        Assert.Contains(FormatNumber(additionalMeasuresModel.LocalAuthorityDisadvantagedPupilsEndOfKS4), doc.GetTableCellContentByIdAndIndex(tableId, 1, 1));
        Assert.Contains(FormatNumber(additionalMeasuresModel.EnglandDisadvantagedPupilsEndOfKS4), doc.GetTableCellContentByIdAndIndex(tableId, 1, 2));
    }

    [Fact]
    public async Task AdditionalMeasuresPage_ShowsNumberOfNonDisadvantagedPupilsEndOfKs4TableWithCorrectData()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "Loreto High School Chorlton";

        var additionalMeasuresModel = GetAdditionalMeasuresModel(
            establishment: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            localAuthority: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            england: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build());

        _serviceMock
            .Setup(a => a.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(additionalMeasuresModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));

        // Assert
        var tableId = "num-pupil-eofks4-non-disadvantaged-table";

        Assert.Contains("Number of non-disadvantaged pupils", doc.GetTableHeaderContentByIdAndIndex(tableId, 1, 0));
        Assert.Contains(FormatNumber(additionalMeasuresModel.LocalAuthorityNonDisadvantagedPupilsEndOfKS4), doc.GetTableCellContentByIdAndIndex(tableId, 1, 0));
        Assert.Contains(FormatNumber(additionalMeasuresModel.EnglandNonDisadvantagedPupilsEndOfKS4), doc.GetTableCellContentByIdAndIndex(tableId, 1, 1));
    }

    [Fact]
    public async Task AdditionalMeasuresPage_ShowsNumberOfPupilsWholseSchoolTableWithCorrectData()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "Loreto High School Chorlton";

        var additionalMeasuresModel = GetAdditionalMeasuresModel(
            establishment: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            localAuthority: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            england: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build());

        _serviceMock
            .Setup(a => a.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(additionalMeasuresModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));

        // Assert
        var tableId = "num-pupil-wholsechool-table";

        Assert.Contains("Number of pupils on roll", doc.GetTableHeaderContentByIdAndIndex(tableId, 1, 0));
        Assert.Contains(FormatNumber(additionalMeasuresModel.EstablishmentTotalPupils), doc.GetTableCellContentByIdAndIndex(tableId, 1, 0));
        Assert.Contains(FormatNumber(additionalMeasuresModel.EnglandTotalPupils), doc.GetTableCellContentByIdAndIndex(tableId, 1, 1));
    }

    [Fact]
    public async Task AdditionalMeasuresPage_ShowsSenNumberOfPupilsWholseSchoolTableWithCorrectData()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "Loreto High School Chorlton";

        var additionalMeasuresModel = GetAdditionalMeasuresModel(
            establishment: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            localAuthority: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            england: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build());

        _serviceMock
            .Setup(a => a.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(additionalMeasuresModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));

        // Assert
        var tableId = "num-pupil-whole-school-sen-table";

        Assert.Contains("Pupils with SEN support", doc.GetTableHeaderContentByIdAndIndex(tableId, 1, 0));
        Assert.Contains(FormatPercentage(additionalMeasuresModel.EstablishmentTotalSENPupils), doc.GetTableCellContentByIdAndIndex(tableId, 1, 0));
        Assert.Contains(FormatPercentage(additionalMeasuresModel.EnglandTotalSENPupils), doc.GetTableCellContentByIdAndIndex(tableId, 1, 1));
    }

    [Fact]
    public async Task AdditionalMeasuresPage_ShowsEhcpNumberOfPupilsWholseSchoolTableWithCorrectData()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "Loreto High School Chorlton";

        var additionalMeasuresModel = GetAdditionalMeasuresModel(
            establishment: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            localAuthority: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            england: new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build());

        _serviceMock
            .Setup(a => a.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(additionalMeasuresModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));

        // Assert
        var tableId = "num-pupil-whole-school-ehcp-table";

        Assert.Contains("Pupils with EHCPs", doc.GetTableHeaderContentByIdAndIndex(tableId, 1, 0));
        Assert.Contains(FormatPercentage(additionalMeasuresModel.EstablishmentTotalEHCPPupils), doc.GetTableCellContentByIdAndIndex(tableId, 1, 0));
        Assert.Contains(FormatPercentage(additionalMeasuresModel.EnglandTotalEHCPPupils), doc.GetTableCellContentByIdAndIndex(tableId, 1, 1));
    }

    private static AdditionalMeasuresModel GetAdditionalMeasuresModel(
        AdditionalMeasures establishment,
        AdditionalMeasures localAuthority,
        AdditionalMeasures england
        )
    {
        return new AdditionalMeasuresModel
        {
            EstablishmentCurrentYear = establishment,
            LocalAuthorityCurrentYear = localAuthority,
            EnglandCurrentYear = england,
            EstablishmentGirlsEndOfKS4 = GetCodedDouble(1),
            EstablishmentBoysEndOfKS4 = GetCodedDouble(2),
            EstablishmentEALEndOfKS4 = GetCodedDouble(3),
            EstablishmentNonMobilePupilsEndOfKS4 = GetCodedDouble(4),
            EstablishmentDisadvantagedPupilsEndOfKS4 = GetCodedDouble(5),
            LocalAuthorityDisadvantagedPupilsEndOfKS4 = GetCodedDouble(6),
            EnglandDisadvantagedPupilsEndOfKS4 = GetCodedDouble(7),
            LocalAuthorityNonDisadvantagedPupilsEndOfKS4 = GetCodedDouble(8),
            EnglandNonDisadvantagedPupilsEndOfKS4 = GetCodedDouble(9),
            EstablishmentTotalPupils = GetCodedDouble(10),
            EnglandTotalPupils = GetCodedDouble(11),
            EstablishmentTotalSENPupils = GetCodedDouble(12),
            EstablishmentTotalEHCPPupils = GetCodedDouble(13),
            EnglandTotalSENPupils = GetCodedDouble(14),
            EnglandTotalEHCPPupils = GetCodedDouble(15)
        };
    }

    private static CodedDouble GetCodedDouble(double val)
    {
        return new CodedDouble(val, string.Empty, val.ToString());
    }

    private static string FormatNumber(CodedDouble value, string? format = null) => value.ToDisplayField().DisplayNumber(format);
    private static string FormatPercentage(CodedDouble value) => value.ToDisplayField().DisplayPercentage();
}
