using Moq;
using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Web.Models.Charts;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;
using System.Text.Json;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Profiles.KS5;

[Collection("WebAppCollection")]
public class KS5DestinationsTests : PageTestsBase
{
    private readonly string _urn = "105574";
    private readonly string _pageRoute = "/destinations/16-to-19";
    private readonly string _establishmentName = "Loreto High School Chorlton";
    private readonly string _laName = "Test LA";
    private readonly double? _englandTotalOverall = 55;
    private readonly double? _establishmentTotalCohortFor = 1020;
    private readonly double? _establishmentTotalOverall = 66;
    private readonly double? _laTotalOverall = 77;
    private readonly Mock<IDestinationsService> _mockDestinationsService;

    public KS5DestinationsTests(WebAppFixture fixture) : base(fixture)
    {
        _mockDestinationsService = UseMock<IDestinationsService>();
        SetupMock();
    }


    [Fact]
    public async Task KS5Destinations_HasCorrectPageElements()
    {
        // Arrange / Act
        var url = BuildUrl(_urn, _establishmentName, _pageRoute);
        var doc = await Fixture.BrowseToPage(url);

        // Assert - headings
        var h1Elements = doc.GetElementsByTagName("h1");

        Assert.Contains(
            h1Elements,
            x => x.TextContent.Trim() == _establishmentName);

        var h2Elements = doc.GetElementsByTagName("h2");

        Assert.Contains(
            h2Elements,
            x => x.TextContent.Trim() ==
                 "Student destinations after 16 to 19 study (2023 leavers)");

        // Assert - page elements
        var tag =
            doc.QuerySelector(
                ".govuk-tag.govuk-tag--full-width.govuk-tag--grey");

        var chartContainer =
            doc.QuerySelector("#all-ks5-dest-data-chart-container");

        var tableContainer =
            doc.QuerySelector("#all-ks5-dest-data-table-container");

        Assert.NotNull(tag);
        Assert.NotNull(chartContainer);
        Assert.NotNull(tableContainer);

        Assert.Equal(
            $"Number of students from this school or college included in the measure: {_establishmentTotalCohortFor}",
            tag!.TextContent.Trim());

        Assert.Contains(
            $"{_establishmentTotalOverall}%",
            doc.GetTableCellContentByIdAndIndex(
                "all-ks5-dest-data-table-container",
                0,
                0));

        Assert.Contains(
            $"{_laTotalOverall}%",
            doc.GetTableCellContentByIdAndIndex(
                "all-ks5-dest-data-table-container",
                1,
                0));

        Assert.Contains(
            $"{_englandTotalOverall}%",
            doc.GetTableCellContentByIdAndIndex(
                "all-ks5-dest-data-table-container",
                2,
                0));

        // Assert - chart data
        var allDestData =
            JsonSerializer.Deserialize<DataViewModel>(
                chartContainer!.Children[0].GetAttribute("data-chart")!,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

        Assert.NotNull(allDestData);

        Assert.Equal(
            "School or College",
            allDestData!.Labels[0]);

        Assert.Equal(
            $"{_laName} average",
            allDestData.Labels[1]);

        Assert.Equal(
            "England average",
            allDestData.Labels[2]);

        Assert.Equal(66, allDestData.Data[0]);
        Assert.Equal(77, allDestData.Data[1]);
        Assert.Equal(55, allDestData.Data[2]);
    }

    [Fact]
    public async Task KS5Destinations_Has_Breakdown_Including_Disadvantaged_Students_Inset_Text()
    {
        // Arrange/Act
        var url = BuildUrl(_urn, _establishmentName, _pageRoute);
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var h2Elements = doc.GetElementsByTagName("h2");        
        var breakdownDisadvantagedStudentsInset = doc.QuerySelector("#breakdown-disadvantaged-students-inset-text");
        var findStatisticsLink = doc.QuerySelector("#find-statistics-link");

        Assert.Equal("Breakdown of all students, including disadvantaged students", h2Elements[3].InnerHtml.Trim());
        Assert.NotNull(breakdownDisadvantagedStudentsInset);

        Assert.NotNull(findStatisticsLink);
        Assert.Contains("https://explore-education-statistics.service.gov.uk/find-statistics", findStatisticsLink.GetAttribute("href"));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task KS5Destinations_Has_NumberOfStudentsIncludedInMeasure_Text(bool hasEstablishmentTotalCohort)
    {
        // Arrange
        SetupMock(hasEstablishmentTotalCohort);

        var url = BuildUrl(_urn, _establishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var noOfStudentsIncludedInMeasure = doc.QuerySelector("#no-of-students-included-in-measure");

        if (hasEstablishmentTotalCohort)
        {
            Assert.NotNull(noOfStudentsIncludedInMeasure);
            Assert.Equal($"Number of students from this school or college included in the measure: {_establishmentTotalCohortFor}", noOfStudentsIncludedInMeasure.TextContent.Trim());
        }
        else
        {
            Assert.Null(noOfStudentsIncludedInMeasure);
        }
    }

    

    [Fact]
    public async Task KS5Destinations_DisplaysBottomPagination_WithCorrectDestinations()
    {
        // Arrange
        var url = BuildUrl(_urn, _establishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var pagination = doc.QuerySelector("nav.govuk-pagination");
        Assert.NotNull(pagination);

        var previousLink = pagination.QuerySelector(".govuk-pagination__prev a");
        var nextLink = pagination.QuerySelector(".govuk-pagination__next a");

        Assert.NotNull(previousLink);
        Assert.Contains("/16-to-19-performance/subjects-entered", previousLink.GetAttribute("href"));

        Assert.NotNull(nextLink);
        Assert.Contains("/destinations/16-to-19-higher-level-study", nextLink.GetAttribute("href"));
    }

    [Fact]
    public async Task KS5Destinations_DisplaysBottomPagination_WithCorrectDestinations_WhenMultiplePhases()
    {
        _mockDestinationsService
            .Setup(a => a.GetKS5DestinationsDetailsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Core.ServiceModels.Destinations.KS5DestinationsDetails
            {
                SchoolName = _establishmentName,
                LocalAuthorityName = _laName,
                EnglandOverall = _englandTotalOverall,
                EstablishmentTotalCohortFor = _establishmentTotalCohortFor,
                EstablishmentTotalOverall = _establishmentTotalOverall,
                LATotalOverall = _laTotalOverall,
                Urn = "123456",
                IsKS2 = false,
                IsKS4 = true,
                IsKS5 = true

            });

        // Arrange
        var url = BuildUrl(_urn, _establishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var pagination = doc.QuerySelector("nav.govuk-pagination");
        Assert.NotNull(pagination);

        var previousLink = pagination.QuerySelector(".govuk-pagination__prev a");
        var nextLink = pagination.QuerySelector(".govuk-pagination__next a");

        Assert.NotNull(previousLink);
        Assert.Contains("/destinations/secondary", previousLink.GetAttribute("href"));

        Assert.NotNull(nextLink);
        Assert.Contains("/destinations/16-to-19-higher-level-study", nextLink.GetAttribute("href"));
    }

    private void SetupMock(bool hasEstablishmentTotalCohort = true)
    {
        _mockDestinationsService
                    .Setup(a => a.GetKS5DestinationsDetailsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new Core.ServiceModels.Destinations.KS5DestinationsDetails
                    {
                        SchoolName = _establishmentName,
                        LocalAuthorityName = _laName,
                        EnglandOverall = _englandTotalOverall,
                        EstablishmentTotalCohortFor = hasEstablishmentTotalCohort ? _establishmentTotalCohortFor : null,
                        EstablishmentTotalOverall = _establishmentTotalOverall,
                        LATotalOverall = _laTotalOverall,
                        Urn = "123456",
                        IsKS2 = false,
                        IsKS4 = false,
                        IsKS5 = true
                    });
    }
}
