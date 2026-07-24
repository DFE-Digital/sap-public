using Moq;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Core.Tests.TestBuilders;
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
            .Setup(service => service.GetEstablishmentAsync(
                _urn,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new EstablishmentTestBuilder()
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
        var additionalmeasuresModel = new AdditionalMeasuresModel
        {
            EnglandCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            EstablishmentCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            LocalAuthorityCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),

        };

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
        var additionalmeasuresModel = new AdditionalMeasuresModel
        {
            EnglandCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            EstablishmentCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            LocalAuthorityCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build()
        };

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
        nav.ShouldHaveItemsCountAsync(6);
        nav.ShouldHaveOneActiveItemAsync();
    }

    [Fact]
    public async Task AdditionalMeasuresPage_DisplaysPagination()
    {
        // Arrange
        var additionalmeasuresModel = new AdditionalMeasuresModel
        {
            EnglandCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            EstablishmentCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            LocalAuthorityCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build()
        };

        _serviceMock
            .Setup(service => service.GetAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(additionalmeasuresModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _establishmentName, _pageRoute));

        // Act
        var bottomPagination = doc.GetElementById("bottom-pagination");

        // Act
        var previousPaginationLink = doc.QuerySelector("#bottom-pagination .govuk-pagination__prev a");
        var nextPaginationLink = doc.QuerySelector("#bottom-pagination .govuk-pagination__next a");
        var previousPaginationText = previousPaginationLink?.TextContent;
        var nextPaginationText = nextPaginationLink?.TextContent;

        // Assert
        Assert.NotNull(bottomPagination);
        Assert.Equal("Secondary academic performance: Subjects entered", previousPaginationText?.Trim());
        Assert.Equal("Destinations", nextPaginationText?.Trim());
    }

    [Fact]
    public async Task AdditionalMeasuresPage_ShowsTableWithCorrectData()
    {
        // Arrange
        var additionalmeasuresModel = new AdditionalMeasuresModel
        {
            EstablishmentCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            EnglandCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().WithPupilsAtTheEndOfKS4(32600).Build(),
            LocalAuthorityCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().WithPupilsAtTheEndOfKS4(3400).Build(),
        };

        _serviceMock
            .Setup(service => service.GetAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(additionalmeasuresModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _establishmentName, _pageRoute));

        // Assert
        // establishment values display correctly in the table
        var establishmentValues = additionalmeasuresModel.EstablishmentCurrentYear;
        Assert.Contains(
            $"{establishmentValues.PercentAchievingAtLeastOneQualification.Value!.Value:F1}%",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 1, 0));
        Assert.Contains(
            $"{establishmentValues.PercentEnteredForTripleScience.Value!.Value:F1}%",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 2, 0));
        Assert.Contains(
            $"{establishmentValues.PercentEnteredMoreThanOneForeignLanguage.Value!.Value:F1}%",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 3, 0));
        Assert.Contains(
            $"{establishmentValues.AverageGCSEExamEntriesPerPupil.Value!.Value:F1}",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 4, 0));
        Assert.Contains(
            $"{establishmentValues.AverageAllKS4QualificationsExamEntriesPerPupil.Value!.Value:F1}",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 5, 0));
        Assert.Contains(
            $"{establishmentValues.NumberOfPupilsAtTheEndOfKS4.Value!.Value:F0}",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 6, 0));

        // local authority values display correctly in the table
        var laValues = additionalmeasuresModel.LocalAuthorityCurrentYear;
        Assert.Contains(
            $"{laValues.PercentAchievingAtLeastOneQualification.Value!.Value:F1}%",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 1, 1));
        Assert.Contains(
            $"{laValues.PercentEnteredForTripleScience.Value!.Value:F1}%",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 2, 1));
        Assert.Contains(
            $"{laValues.PercentEnteredMoreThanOneForeignLanguage.Value!.Value:F1}%",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 3, 1));
        Assert.Contains(
            $"{laValues.AverageGCSEExamEntriesPerPupil.Value!.Value:F1}",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 4, 1));
        Assert.Contains(
            $"{laValues.AverageAllKS4QualificationsExamEntriesPerPupil.Value!.Value:F1}",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 5, 1));
        Assert.Contains(
            $"{laValues.NumberOfPupilsAtTheEndOfKS4.Value!.Value:N0}",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 6, 1));

        // England values display correctly in the table
        var englandValues = additionalmeasuresModel.EnglandCurrentYear;
        Assert.Contains(
            $"{englandValues.PercentAchievingAtLeastOneQualification.Value!.Value:F1}%",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 1, 2));
        Assert.Contains(
            $"{englandValues.PercentEnteredForTripleScience.Value!.Value:F1}%",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 2, 2));
        Assert.Contains(
            $"{englandValues.PercentEnteredMoreThanOneForeignLanguage.Value!.Value:F1}%",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 3, 2));
        Assert.Contains(
            $"{englandValues.AverageGCSEExamEntriesPerPupil.Value!.Value:F1}",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 4, 2));
        Assert.Contains(
            $"{englandValues.AverageAllKS4QualificationsExamEntriesPerPupil.Value!.Value:F1}",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 5, 2));
        Assert.Contains(
            $"{englandValues.NumberOfPupilsAtTheEndOfKS4.Value!.Value:N0}",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 6, 2));
    }

    [Fact]
    public async Task AdditionalMeasuresPage_EstablishmentDataNotAvailable_ShowsTableValuesAsNotAvailable()
    {
        // Arrange
        var additionalmeasuresModel = new AdditionalMeasuresModel
        {
            EstablishmentCurrentYear = new AdditionalMeasuresBuilder().Build(),
            EnglandCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().WithPupilsAtTheEndOfKS4(32600).Build(),
            LocalAuthorityCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().WithPupilsAtTheEndOfKS4(3400).Build()
        };

        _serviceMock
            .Setup(service => service.GetAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(additionalmeasuresModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _establishmentName, _pageRoute));

        // Assert
        var establishmentDataCellIndex = 0;
        Assert.Contains(
            "Not available",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 1, establishmentDataCellIndex));
        Assert.Contains(
            "Not available",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 2, establishmentDataCellIndex));
        Assert.Contains(
            "Not available",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 3, establishmentDataCellIndex));
        Assert.Contains(
            "Not available",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 4, establishmentDataCellIndex));
        Assert.Contains(
            "Not available",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 5, establishmentDataCellIndex));
        Assert.Contains(
            "Not available",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 6, establishmentDataCellIndex));
    }

    [Fact]
    public async Task AdditionalMeasuresPage_LocalAuthorityDataNotAvailable_ShowsTableValuesAsNotAvailable()
    {
        // Arrange
        var additionalmeasuresModel = new AdditionalMeasuresModel
        {
            EstablishmentCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            EnglandCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().WithPupilsAtTheEndOfKS4(32600).Build(),
            LocalAuthorityCurrentYear = new AdditionalMeasuresBuilder().Build()
        };

        _serviceMock
            .Setup(service => service.GetAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(additionalmeasuresModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _establishmentName, _pageRoute));

        // Assert
        var localAuthorityDataCellIndex = 1;
        Assert.Contains(
            "Not available",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 1, localAuthorityDataCellIndex));
        Assert.Contains(
            "Not available",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 2, localAuthorityDataCellIndex));
        Assert.Contains(
            "Not available",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 3, localAuthorityDataCellIndex));
        Assert.Contains(
            "Not available",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 4, localAuthorityDataCellIndex));
        Assert.Contains(
            "Not available",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 5, localAuthorityDataCellIndex));
        Assert.Contains(
            "Not available",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 6, localAuthorityDataCellIndex));
    }

    [Fact]
    public async Task AdditionalMeasuresPage_EnglandDataNotAvailable_ShowsTableValuesAsNotAvailable()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "Loreto High School Chorlton";
        var additionalmeasuresModel = new AdditionalMeasuresModel
        {
            EstablishmentCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            EnglandCurrentYear = new AdditionalMeasuresBuilder().Build(),
            LocalAuthorityCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build()
        };

        _serviceMock
            .Setup(service => service.GetAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(additionalmeasuresModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));

        // Assert
        var englandDataCellIndex = 2;
        Assert.Contains(
            "Not available",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 1, englandDataCellIndex));
        Assert.Contains(
            "Not available",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 2, englandDataCellIndex));
        Assert.Contains(
            "Not available",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 3, englandDataCellIndex));
        Assert.Contains(
            "Not available",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 4, englandDataCellIndex));
        Assert.Contains(
            "Not available",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 5, englandDataCellIndex));
        Assert.Contains(
            "Not available",
            doc.GetTableCellContentByIdAndIndex("additional-measures-table", 6, englandDataCellIndex));
    }
}
