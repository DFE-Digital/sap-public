using Moq;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page;

[Collection("WebAppCollection")]
public class AdditionalMeasuresTests : PageTestsBase
{
    private static string _pageRoute = "/secondary/additional-measures";
    private readonly Mock<IAdditionalMeasuresService> _serviceMock;

    public AdditionalMeasuresTests(WebAppFixture fixture) : base(fixture)
    {
        _serviceMock = UseMock<IAdditionalMeasuresService>();
    }

    [Fact]
    public async Task AdditionalMeasuresPage_HasCorrectTitle()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "Loreto High School Chorlton";
        var additionalmeasuresModel = new AdditionalMeasuresModel
        {
            Urn = urn,
            EnglandCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            EstablishmentCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            LocalAuthorityCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            SchoolName = establishmentName
        };

        _serviceMock
            .Setup(service => service.GetAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(additionalmeasuresModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));

        // Assert
        var title = doc.Title;
        Assert.Contains("Loreto High School Chorlton - Additional Measures - School Profiles - GOV.UK", title);
    }

    [Fact]
    public async Task AdditionalMeasuresPage_Displays_VerticalNavigation()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "Loreto High School Chorlton";
        var additionalmeasuresModel = new AdditionalMeasuresModel
        {
            Urn = urn,
            SchoolName = establishmentName,
            EnglandCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            EstablishmentCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            LocalAuthorityCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build()
        };

        _serviceMock
            .Setup(service => service.GetAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(additionalmeasuresModel);

        // Act
        var pageUrl = BuildUrl(urn, establishmentName, _pageRoute);
        var doc = await Fixture.BrowseToPage(pageUrl);

        var nav = new VerticalNavigationAssertHelper(doc);

        nav.ShouldBeVisibleAsync();
        nav.ShouldHaveItemsCountAsync(6);
        nav.ShouldHaveOneActiveItemAsync();
        nav.ShouldHaveActiveHrefAsync(pageUrl);
    }

    [Fact]
    public async Task AdditionalMeasuresPage_DisplaysPagination()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "Loreto High School Chorlton";
        var additionalmeasuresModel = new AdditionalMeasuresModel
        {
            Urn = urn,
            SchoolName = establishmentName,
            EnglandCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            EstablishmentCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            LocalAuthorityCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build()
        };

        _serviceMock
            .Setup(service => service.GetAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(additionalmeasuresModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));

        // Act
        var bottomPagination = doc.GetElementById("bottom-pagination");

        // Act
        var previousPaginationLink = doc.QuerySelector("#bottom-pagination .govuk-pagination__prev a");
        var nextPaginationLink = doc.QuerySelector("#bottom-pagination .govuk-pagination__next a");
        var previousPaginationText = previousPaginationLink?.TextContent;
        var nextPaginationText = nextPaginationLink?.TextContent;

        // Assert
        Assert.NotNull(bottomPagination);
        Assert.Equal("Subjects entered", previousPaginationText?.Trim());
        Assert.Equal("Destinations", nextPaginationText?.Trim());
    }

    [Fact]
    public async Task AdditionalMeasuresPage_ShowsTableWithCorrectData()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "Loreto High School Chorlton";
        var additionalmeasuresModel = new AdditionalMeasuresModel
        {
            Urn = urn,
            EstablishmentCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            EnglandCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().WithPupilsAtTheEndOfKS4(32600).Build(),
            LocalAuthorityCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().WithPupilsAtTheEndOfKS4(3400).Build(),
            SchoolName = establishmentName
        };

        _serviceMock
            .Setup(service => service.GetAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(additionalmeasuresModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));

        // Assert
        Assert.Contains("Pupils achieving at least 1 qualification", doc.GetTableCellContentByIdAndIndex("#additional-measures-table", 0, 0));

        // establishment values display correctly in the table
        var establishmentValues = additionalmeasuresModel.EstablishmentCurrentYear;
        Assert.Contains(
            $"{establishmentValues.PercentAchievingAtLeastOneQualification.Value!.Value:F1}%",
            doc.GetTableCellContentByIdAndIndex("#additional-measures-table", 0, 1));
        Assert.Contains(
            $"{establishmentValues.PercentEnteredForTripleScience.Value!.Value:F1}%",
            doc.GetTableCellContentByIdAndIndex("#additional-measures-table", 1, 1));
        Assert.Contains(
            $"{establishmentValues.PercentEnteredMoreThanOneForeignLanguage.Value!.Value:F1}%",
            doc.GetTableCellContentByIdAndIndex("#additional-measures-table", 2, 1));
        Assert.Contains(
            $"{establishmentValues.AverageGCSEExamEntriesPerPupil.Value!.Value:F1}",
            doc.GetTableCellContentByIdAndIndex("#additional-measures-table", 3, 1));
        Assert.Contains(
            $"{establishmentValues.AverageAllKS4QualificationsExamEntriesPerPupil.Value!.Value:F1}",
            doc.GetTableCellContentByIdAndIndex("#additional-measures-table", 4, 1));
        Assert.Contains(
            $"{establishmentValues.NumberOfPupilsAtTheEndOfKS4.Value!.Value:F0}",
            doc.GetTableCellContentByIdAndIndex("#additional-measures-table", 5, 1));

        // local authority values display correctly in the table
        var laValues = additionalmeasuresModel.LocalAuthorityCurrentYear;
        Assert.Contains(
            $"{laValues.PercentAchievingAtLeastOneQualification.Value!.Value:F1}%",
            doc.GetTableCellContentByIdAndIndex("#additional-measures-table", 0, 2));
        Assert.Contains(
            $"{laValues.PercentEnteredForTripleScience.Value!.Value:F1}%",
            doc.GetTableCellContentByIdAndIndex("#additional-measures-table", 1, 2));
        Assert.Contains(
            $"{laValues.PercentEnteredMoreThanOneForeignLanguage.Value!.Value:F1}%",
            doc.GetTableCellContentByIdAndIndex("#additional-measures-table", 2, 2));
        Assert.Contains(
            $"{laValues.AverageGCSEExamEntriesPerPupil.Value!.Value:F1}",
            doc.GetTableCellContentByIdAndIndex("#additional-measures-table", 3, 2));
        Assert.Contains(
            $"{laValues.AverageAllKS4QualificationsExamEntriesPerPupil.Value!.Value:F1}",
            doc.GetTableCellContentByIdAndIndex("#additional-measures-table", 4, 2));
        Assert.Contains(
            $"{laValues.NumberOfPupilsAtTheEndOfKS4.Value!.Value:N0}",
            doc.GetTableCellContentByIdAndIndex("#additional-measures-table", 5, 2));

        // England values display correctly in the table
        var englandValues = additionalmeasuresModel.EnglandCurrentYear;
        Assert.Contains(
            $"{englandValues.PercentAchievingAtLeastOneQualification.Value!.Value:F1}%",
            doc.GetTableCellContentByIdAndIndex("#additional-measures-table", 0, 3));
        Assert.Contains(
            $"{englandValues.PercentEnteredForTripleScience.Value!.Value:F1}%",
            doc.GetTableCellContentByIdAndIndex("#additional-measures-table", 1, 3));
        Assert.Contains(
            $"{englandValues.PercentEnteredMoreThanOneForeignLanguage.Value!.Value:F1}%",
            doc.GetTableCellContentByIdAndIndex("#additional-measures-table", 2, 3));
        Assert.Contains(
            $"{englandValues.AverageGCSEExamEntriesPerPupil.Value!.Value:F1}",
            doc.GetTableCellContentByIdAndIndex("#additional-measures-table", 3, 3));
        Assert.Contains(
            $"{englandValues.AverageAllKS4QualificationsExamEntriesPerPupil.Value!.Value:F1}",
            doc.GetTableCellContentByIdAndIndex("#additional-measures-table", 4, 3));
        Assert.Contains(
            $"{englandValues.NumberOfPupilsAtTheEndOfKS4.Value!.Value:N0}",
            doc.GetTableCellContentByIdAndIndex("#additional-measures-table", 5, 3));
    }

    [Fact]
    public async Task AdditionalMeasuresPage_EstablishmentDataNotAvailable_ShowsTableValuesAsNotAvailable()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "Loreto High School Chorlton";
        var additionalmeasuresModel = new AdditionalMeasuresModel
        {
            Urn = urn,
            SchoolName = establishmentName,
            EstablishmentCurrentYear = new AdditionalMeasuresBuilder().Build(),
            EnglandCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().WithPupilsAtTheEndOfKS4(32600).Build(),
            LocalAuthorityCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().WithPupilsAtTheEndOfKS4(3400).Build()
        };

        _serviceMock
            .Setup(service => service.GetAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(additionalmeasuresModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));

        // Assert
        Assert.Contains("Pupils achieving at least 1 qualification", doc.GetTableCellContentByIdAndIndex("#additional-measures-table", 0, 0));

        // establishment values display correctly in the table
        var establishmentValues = additionalmeasuresModel.EstablishmentCurrentYear;
        Assert.Contains(
            "Not available",
            doc.GetTableCellContentByIdAndIndex("#additional-measures-table", 0, 1));
        Assert.Contains(
            "Not available",
            doc.GetTableCellContentByIdAndIndex("#additional-measures-table", 1, 1));
        Assert.Contains(
            "Not available",
            doc.GetTableCellContentByIdAndIndex("#additional-measures-table", 2, 1));
        Assert.Contains(
            "Not available",
            doc.GetTableCellContentByIdAndIndex("#additional-measures-table", 3, 1));
        Assert.Contains(
            "Not available",
            doc.GetTableCellContentByIdAndIndex("#additional-measures-table", 4, 1));
        Assert.Contains(
            "Not available",
            doc.GetTableCellContentByIdAndIndex("#additional-measures-table", 5, 1));
    }
}
