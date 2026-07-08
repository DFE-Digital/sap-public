using AngleSharp.Dom;
using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Compare.Secondary;

[Collection("WebAppCollection")]
public class AcademicPerformanceEnglishAndMathsResultsTests : PageTestsBase
{
    private static readonly List<string> _urns = ["100279", "145179"];
    private readonly string _pageUrl = $"compare/secondary/english-and-maths-results?urns={_urns[0]}&urns={_urns[1]}";
    private readonly Mock<IEnglishAndMathsComparisionService> _englishAndMathsComparisionService = new();
    private readonly Mock<IEstablishmentService> _establishmentService = new();
    private readonly List<EstablishmentServiceModel> _establishmentList = [];

    public AcademicPerformanceEnglishAndMathsResultsTests(WebAppFixture fixture) : base(fixture)
    {
        _establishmentService = UseMock<IEstablishmentService>();
        _englishAndMathsComparisionService = UseMock<IEnglishAndMathsComparisionService>();

        _establishmentList =
        [
            new EstablishmentTestBuilder().WithURN(_urns[0]).WithEstablishmentName($"Test school {_urns[0]}").WithIsKeyStage4(true).BuildServiceModel(),
            new EstablishmentTestBuilder().WithURN(_urns[1]).WithEstablishmentName($"Test school {_urns[1]}").WithIsKeyStage4(true).BuildServiceModel(),
        ];

        _establishmentList.Select(e =>
            _establishmentService.Setup(s =>
            s.GetEstablishmentAsync(e.URN, It.IsAny<CancellationToken>())).ReturnsAsync(e)).ToList();

        var comparisionResults = new EnglishAndMathsComparisionResultsModel
        {
            Establishments =
                [
                    new()
                    {
                        Urn = _establishmentList[0].URN,
                        SchoolName = _establishmentList[0].EstablishmentName,
                        EstablishmentData = new RelativeYearValues<double?> { CurrentYear = 90.3, PreviousYear = 80.5, TwoYearsAgo = 78 }
                    },
                    new()
                    {
                        Urn = _establishmentList[1].URN,
                        SchoolName = _establishmentList[1].EstablishmentName,
                        EstablishmentData = new RelativeYearValues<double?> { CurrentYear = null, PreviousYear = 60, TwoYearsAgo = 95.1 }
                    }

                ],
            EnglandAverage = new RelativeYearValues<double?> { CurrentYear = 80, PreviousYear = 80.5, TwoYearsAgo = null }
        };

        _englishAndMathsComparisionService.Setup(s => s.GetComparisionResultsAsync(_urns, It.IsAny<CancellationToken>()))
                .ReturnsAsync(comparisionResults);
    }

    [Fact]
    public async Task EnglishAndMathsResultsPage_HasCorrectTitle()
    {

        // Act
        var doc = await Fixture.BrowseToPage(_pageUrl);

        // Assert
        var title = doc.QuerySelector("title");
        Assert.NotNull(title);
        Assert.Equal("Compare - English and maths results - School Profiles - GOV.UK", title.TextContent.Trim());
    }

    [Fact]
    public async Task EnglishAndMathsResultsPage_DisplaysMainHeading()
    {
        // Arrange && Act
        var doc = await Fixture.BrowseToPage(_pageUrl);

        // Assert
        var heading = doc.QuerySelector("h1");
        Assert.NotNull(heading);
        Assert.Contains("Academic Performance", heading.TextContent.Trim());
    }

    [Fact]
    public async Task EnglishAndMathsResultsPage_Displays_VerticalNavigation()
    {
        // Arrange && Act
        var doc = await Fixture.BrowseToPage(_pageUrl);

        // Assert
        Assert.NotNull(doc.QuerySelector(".moj-side-navigation"));
        Assert.Equal(4, doc.QuerySelectorAll(".moj-side-navigation__item").Length);
        Assert.Single(doc.QuerySelectorAll(".moj-side-navigation__item--active"));
    }

    [Fact]
    public async Task EnglishAndMathsResultsPage_Displays_Gcse_Grades_Explained()
    {
        // Arrange
        var doc = await Fixture.BrowseToPage(_pageUrl);

        // Act
        var gcseGradesExplained = doc.QuerySelector("#details-gcse-grades-explained");

        // Assert
        Assert.NotNull(gcseGradesExplained);
    }

    [Fact]
    public async Task EnglishAndMathsResultsPage_Displays_AllGcse_CurrentYear_Chart()
    {
        // Arrange
        var doc = await Fixture.BrowseToPage(_pageUrl);

        // Act
        var chart = doc.QuerySelector("#all-gcse-chart");
        var table = doc.QuerySelector("#all-gcse-current-year-table");
        var showAsTableBtn = doc.QuerySelector("#all-gcse-current-year-show-btn");

        var showAsTableBtnText = showAsTableBtn?.Text();

        // Assert
        Assert.NotNull(table);
        Assert.NotNull(chart);
        Assert.NotNull(showAsTableBtn);
        Assert.Equal("Show as a table", showAsTableBtnText);
    }

    [Fact]
    public async Task EnglishAndMathsResultsPage_Displays_AllGcse_CurrentYear_Table_Data()
    {
        // Arrange && Act
        var doc = await Fixture.BrowseToPage(_pageUrl);

        // Assert Gcse current year
        Assert.Contains(_establishmentList[0].EstablishmentName, doc.GetTableHeaderContentByIdAndIndex("all-gcse-current-year-table", 0, 0));
        Assert.Contains(_establishmentList[1].EstablishmentName, doc.GetTableHeaderContentByIdAndIndex("all-gcse-current-year-table", 1, 0));
        Assert.Contains("England average", doc.GetTableHeaderContentByIdAndIndex("all-gcse-current-year-table", 2, 0));

        Assert.Contains("90.3%", doc.GetTableCellContentByIdAndIndex("all-gcse-current-year-table", 0, 0));
        Assert.Contains("Not available", doc.GetTableCellContentByIdAndIndex("all-gcse-current-year-table", 1, 0));
        Assert.Contains("80%", doc.GetTableCellContentByIdAndIndex("all-gcse-current-year-table", 2, 0));

        // Assert Gcse DataOverTime
        Assert.Contains("2022 to 2023", doc.GetTableHeaderContentByIdAndIndex("all-gcse-data-overtime-table", 0, 1));
        Assert.Contains("2023 to 2024", doc.GetTableHeaderContentByIdAndIndex("all-gcse-data-overtime-table", 0, 2));
        Assert.Contains("2024 to 2025", doc.GetTableHeaderContentByIdAndIndex("all-gcse-data-overtime-table", 0, 3));

        Assert.Contains(_establishmentList[0].EstablishmentName, doc.GetTableHeaderContentByIdAndIndex("all-gcse-data-overtime-table", 1, 0));
        Assert.Contains("78%", doc.GetTableCellContentByIdAndIndex("all-gcse-data-overtime-table", 1, 0));
        Assert.Contains("80.5%", doc.GetTableCellContentByIdAndIndex("all-gcse-data-overtime-table", 1, 1));
        Assert.Contains("90.3%", doc.GetTableCellContentByIdAndIndex("all-gcse-data-overtime-table", 1, 2));

        Assert.Contains(_establishmentList[1].EstablishmentName, doc.GetTableHeaderContentByIdAndIndex("all-gcse-data-overtime-table", 2, 0));
        Assert.Contains("95.1%", doc.GetTableCellContentByIdAndIndex("all-gcse-data-overtime-table", 2, 0));
        Assert.Contains("60%", doc.GetTableCellContentByIdAndIndex("all-gcse-data-overtime-table", 2, 1));
        Assert.Contains("Not available", doc.GetTableCellContentByIdAndIndex("all-gcse-data-overtime-table", 2, 2));

        Assert.Contains("England average", doc.GetTableHeaderContentByIdAndIndex("all-gcse-data-overtime-table", 3, 0));
        Assert.Contains("Not available", doc.GetTableCellContentByIdAndIndex("all-gcse-data-overtime-table", 3, 0));
        Assert.Contains("80.5%", doc.GetTableCellContentByIdAndIndex("all-gcse-data-overtime-table", 3, 1));
        Assert.Contains("80%", doc.GetTableCellContentByIdAndIndex("all-gcse-data-overtime-table", 3, 2));
    }

    [Fact]
    public async Task DisplaysPagination()
    {
        // Arrange
        var doc = await Fixture.BrowseToPage(_pageUrl);

        // Act
        var nav = doc.QuerySelector("#academic-performance-english-maths-pagination");
        var navNext = doc.QuerySelector("#academic-performance-english-maths-pagination .govuk-pagination__next a");
        var navPrevious = doc.QuerySelector("#academic-performance-english-maths-pagination .govuk-pagination__prev a");

        // Assert
        Assert.NotNull(nav);
        Assert.NotNull(navNext);
        Assert.NotNull(navPrevious);
        Assert.Contains("Academic performance: Pupil attainment", navPrevious.TextContent);
        Assert.Contains("Destinations after year 11", navNext.TextContent);
    }

    [Fact]
    public async Task Page_WithNoSpecialSchool_DoesNotShowShowsNotificationBanner()
    {
        // Act
        var doc = await Fixture.BrowseToPage(_pageUrl);

        var banner = doc.QuerySelector("[data-testid='special-school-warning-banner']");
        Assert.Null(banner);
    }

    [Fact]
    public async Task Page_WithSpecialSchool_ShowsNotificationBanner()
    {
        // Arrange
        _establishmentService.Setup(s => s.GetEstablishmentAsync("100279", It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new EstablishmentTestBuilder()
                    .WithURN("100279")
                    .WithIsKeyStage4(true)
                    .WithTypeOfEstablishmentId(7)
                    .BuildServiceModel());

        // Act
        var doc = await Fixture.BrowseToPage(_pageUrl);

        // Assert
        var banner = doc.QuerySelector("[data-testid='special-school-warning-banner']");
        Assert.NotNull(banner);
        Assert.Contains("You're comparing a special school", banner.TextContent.Trim());
    }
}
