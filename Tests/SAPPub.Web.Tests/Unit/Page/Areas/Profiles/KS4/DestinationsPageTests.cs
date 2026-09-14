using Moq;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Profiles.KS4;

[Collection("WebAppCollection")]
public class DestinationsPageTests : PageTestsBase
{
    private readonly string _urn = "105574";
    private readonly string _establishmentName = "Loreto High School Chorlton";
    private readonly string _laName = "Test LA";
    private readonly string _pageRoute = "/destinations/secondary";
    private readonly Mock<IDestinationsService> _mockDestinationsService;

    public DestinationsPageTests(WebAppFixture fixture) : base(fixture)
    {
        _mockDestinationsService = UseMock<IDestinationsService>();
    }

    private KS4DestinationsDetailsBuilder BuildDetails() =>
        new KS4DestinationsDetailsBuilder()
            .WithUrn(_urn)
            .WithEstablishmentName(_establishmentName)
            .WithLAName(_laName)
            .WithKS4(true);

    [Fact]
    public async Task Destinations_HasCorrectTitle()
    {
        // Arrange
        var destinationsDetails = BuildDetails().Build();
        _mockDestinationsService
            .Setup(s => s.GetKS4DestinationsDetailsAsync(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(destinationsDetails);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _establishmentName, _pageRoute));

        // Assert
        Assert.Contains($"{_establishmentName} - Destinations after year 11 - School Profiles - GOV.UK", doc.Title);
    }

    [Fact]
    public async Task Destinations_DisplaysMainHeading()
    {
        // Arrange
        var destinationsDetails = BuildDetails().Build();
        _mockDestinationsService
            .Setup(s => s.GetKS4DestinationsDetailsAsync(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(destinationsDetails);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _establishmentName, _pageRoute));

        // Assert
        var h2Elements = doc.GetElementsByTagName("h2");
        Assert.Contains(h2Elements, x => x.TextContent.Trim() == "Destinations");
    }

    [Fact]
    public async Task Destinations_Displays_School_Performance_Info()
    {
        // Arrange
        var destinationsDetails = BuildDetails().Build();
        _mockDestinationsService
            .Setup(s => s.GetKS4DestinationsDetailsAsync(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(destinationsDetails);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _establishmentName, _pageRoute));

        // Assert
        var info = doc.QuerySelector("[data-testId='looking-at-school-performance-info']");
        Assert.NotNull(info);
        Assert.NotEmpty(info!.TextContent.Trim());
    }

    [Fact]
    public async Task Destinations_Displays_AllDestinations_CurrentYear_Table()
    {
        // Arrange
        var destinationsDetails = BuildDetails().Build();
        _mockDestinationsService
            .Setup(s => s.GetKS4DestinationsDetailsAsync(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(destinationsDetails);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _establishmentName, _pageRoute));

        // Assert
        Assert.Contains(
            $"{destinationsDetails.SchoolAll.CurrentYear.Value}%",
            doc.GetTableCellContentByIdAndIndex("all-destinations-current-year-table", 0, 0));

        Assert.Contains(
            $"{destinationsDetails.LocalAuthorityAll.CurrentYear.Value}%",
            doc.GetTableCellContentByIdAndIndex("all-destinations-current-year-table", 1, 0));

        Assert.Contains(
            $"{destinationsDetails.EnglandAll.CurrentYear.Value}%",
            doc.GetTableCellContentByIdAndIndex("all-destinations-current-year-table", 2, 0));
    }

    [Fact]
    public async Task Destinations_Displays_DisadvantagedDestinationsAccordion()
    {
        // Arrange
        var destinationsDetails = BuildDetails().Build();
        _mockDestinationsService
            .Setup(s => s.GetKS4DestinationsDetailsAsync(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(destinationsDetails);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _establishmentName, _pageRoute));

        // Assert
        var accordion = doc.QuerySelector("#disadvantaged-destinations-accordion");
        Assert.NotNull(accordion);
        Assert.Contains("Staying in education, employment and apprenticeships for disadvantaged pupils", accordion!.TextContent);
    }

    [Fact]
    public async Task Destinations_Displays_DisadvantagedDestinationsTable()
    {
        // Arrange
        var destinationsDetails = BuildDetails().Build();
        _mockDestinationsService
            .Setup(s => s.GetKS4DestinationsDetailsAsync(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(destinationsDetails);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _establishmentName, _pageRoute));

        // Assert
        var table = doc.QuerySelector("#all-dest-disadvantaged-current-year-table");
        Assert.NotNull(table);
        Assert.Contains(
            $"{destinationsDetails.SchoolDisadvantagedAll.CurrentYear.Value}%",
            table!.GetTableCellContentByRowAndCellIndex(1, 0));
        Assert.Contains(
            $"{destinationsDetails.LocalAuthorityDisadvantagedAll.CurrentYear.Value}%",
            table.GetTableCellContentByRowAndCellIndex(2, 0));
        Assert.Contains(
            $"{destinationsDetails.EnglandDisadvantagedAll.CurrentYear.Value}%",
            table.GetTableCellContentByRowAndCellIndex(3, 0));
    }

    [Fact]
    public async Task Destinations_Displays_NonDisadvantagedDestinationsTable()
    {
        // Arrange
        var destinationsDetails = BuildDetails().Build();
        _mockDestinationsService
            .Setup(s => s.GetKS4DestinationsDetailsAsync(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(destinationsDetails);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _establishmentName, _pageRoute));

        // Assert
        var tables = doc.QuerySelectorAll("#all-dest-disadvantaged-current-year-table");
        Assert.Equal(2, tables.Length);

        var nonDisadvantagedTable = tables[1];
        Assert.Contains(
            $"{destinationsDetails.LocalAuthorityNonDisadvantagedAll.CurrentYear.Value}%",
            nonDisadvantagedTable.GetTableCellContentByRowAndCellIndex(1, 0));
        Assert.Contains(
            $"{destinationsDetails.EnglandNonDisadvantagedAll.CurrentYear.Value}%",
            nonDisadvantagedTable.GetTableCellContentByRowAndCellIndex(2, 0));
    }

    [Fact]
    public async Task Destinations_Displays_StayedInEducationTable()
    {
        // Arrange
        var destinationsDetails = BuildDetails().Build();
        _mockDestinationsService
            .Setup(s => s.GetKS4DestinationsDetailsAsync(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(destinationsDetails);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _establishmentName, _pageRoute));

        // Assert
        var table = doc.QuerySelector("#stayed-in-education-table");
        Assert.NotNull(table);
        Assert.Contains("Pupils who stayed in education", table!.TextContent);
        Assert.Contains($"{destinationsDetails.SchoolEducation.CurrentYear.Value}%", table.TextContent);
        Assert.Contains($"{destinationsDetails.LocalAuthorityEducation.CurrentYear.Value}%", table.TextContent);
        Assert.Contains($"{destinationsDetails.EnglandEducation.CurrentYear.Value}%", table.TextContent);
    }

    [Fact]
    public async Task Destinations_Displays_WherePupilsStudiedTable()
    {
        // Arrange
        var destinationsDetails = BuildDetails().Build();
        _mockDestinationsService
            .Setup(s => s.GetKS4DestinationsDetailsAsync(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(destinationsDetails);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _establishmentName, _pageRoute));

        // Assert
        var table = doc.QuerySelector("#where-pupils-studied-table");
        Assert.NotNull(table);
        Assert.Contains("Further education provider", table!.TextContent);
        Assert.Contains("School sixth form", table.TextContent);
        Assert.Contains("Sixth form college", table.TextContent);
        Assert.Contains("Other education destinations", table.TextContent);
    }

    [Fact]
    public async Task Destinations_Displays_ApprenticeshipsOrEmploymentTable()
    {
        // Arrange
        var destinationsDetails = BuildDetails().Build();
        _mockDestinationsService
            .Setup(s => s.GetKS4DestinationsDetailsAsync(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(destinationsDetails);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _establishmentName, _pageRoute));

        // Assert
        var table = doc.QuerySelector("#apprenticeships-or-employment-table");
        Assert.NotNull(table);
        Assert.Contains("employment", table!.TextContent);
        Assert.Contains("apprenticeship", table.TextContent);
    }

    [Fact]
    public async Task Destinations_Displays_DidNotStayInEducationOrEmploymentTable()
    {
        // Arrange
        var destinationsDetails = BuildDetails().Build();
        _mockDestinationsService
            .Setup(s => s.GetKS4DestinationsDetailsAsync(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(destinationsDetails);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _establishmentName, _pageRoute));

        // Assert
        var table = doc.QuerySelector("#did-not-stay-in-education-or-employment-table");
        Assert.NotNull(table);
        Assert.Contains("did not stay in education or employment", table!.TextContent);
        Assert.Contains("Destination unknown", table.TextContent);
    }

    [Fact]
    public async Task Destinations_ResultsNotAvailable_ShowsNotAvailableText()
    {
        // Arrange
        var destinationsDetails = BuildDetails().BuildResultsNotAvailable();
        _mockDestinationsService
            .Setup(s => s.GetKS4DestinationsDetailsAsync(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(destinationsDetails);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _establishmentName, _pageRoute));

        // Assert
        var chartContainer = doc.QuerySelector("#all-dest-current-year-data-container");
        Assert.Null(chartContainer);
    }
}
