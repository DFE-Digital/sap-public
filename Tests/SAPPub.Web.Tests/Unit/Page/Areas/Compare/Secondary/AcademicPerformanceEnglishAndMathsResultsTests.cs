using AngleSharp.Dom;
using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Compare.Secondary;

[Collection("WebAppCollection")]
public class AcademicPerformanceEnglishAndMathsResultsTests : PageTestsBase
{
    private string _pageUrl = "compare/secondary/english-and-maths-results";
    private List<string> _urns = ["100279", "145179"];
    private string QueryString => string.Join("&", _urns.Select(urn => $"urns={urn}"));
    private readonly Mock<IEnglishAndMathsComparisionService> _englishAndMathsComparisionService = new();
    private readonly Mock<IEstablishmentService> _establishmentService = new();

    public AcademicPerformanceEnglishAndMathsResultsTests(WebAppFixture fixture) : base(fixture)
    {
        _establishmentService = UseMock<IEstablishmentService>();
        _englishAndMathsComparisionService = UseMock<IEnglishAndMathsComparisionService>();

        foreach (var urn in _urns)
        {
            _establishmentService.Setup(s => s.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Establishment
                {
                    URN = urn,
                    EstablishmentName = $"School {urn}",
                    IsKS4 = true
                });
        }

        var comparisionResults = new EnglishAndMathsComparisionResultsModel
        {
            Establishments =
                [
                    new()
                    {
                        Urn = _urns[0],
                        SchoolName = $"Test School {_urns[0]}",
                        EstablishmentData = new RelativeYearValues<double?> { CurrentYear = 90.3 }
                    },
                    new()
                    {
                        Urn = _urns[1],
                        SchoolName = $"Test School {_urns[1]}",
                        EstablishmentData = new RelativeYearValues<double?> { CurrentYear = null }
                    }

                ],
            EnglandAverage = new RelativeYearValues<double?> { CurrentYear = 80 }
        };

        _englishAndMathsComparisionService.Setup(s => s.GetComparisionResultsAsync(_urns, It.IsAny<CancellationToken>()))
                .ReturnsAsync(comparisionResults);
    }

    [Fact]
    public async Task EnglishAndMathsResultsPage_HasCorrectTitle()
    {
        // Arrange

        // Act
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{QueryString}");

        // Assert
        var title = doc.QuerySelector("title");
        Assert.NotNull(title);
        Assert.Equal("Compare - English and maths results - School Profiles - GOV.UK", title.TextContent.Trim());
    }

    [Fact]
    public async Task EnglishAndMathsResultsPage_DisplaysMainHeading()
    {
        // Arrange

        // Act
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{QueryString}");


        // Assert
        var heading = doc.QuerySelector("h1");
        Assert.NotNull(heading);
        Assert.Contains("Academic Perfomance", heading.TextContent.Trim());
    }

    [Fact]
    public async Task EnglishAndMathsResultsPage_Displays_VerticalNavigation()
    {
        //Arrange

        // Act
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{QueryString}");

        // Assert
        Assert.NotNull(doc.QuerySelector(".moj-side-navigation"));
        Assert.Equal(4, doc.QuerySelectorAll(".moj-side-navigation__item").Length);
        Assert.Single(doc.QuerySelectorAll(".moj-side-navigation__item--active"));
    }

    [Fact]
    public async Task EnglishAndMathsResultsPage_Displays_Gcse_Grades_Explained()
    {
        // Arrange
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{QueryString}");

        // Act
        var gcseGradesExplained = doc.QuerySelector("#details-gcse-grades-explained");

        // Assert
        Assert.NotNull(gcseGradesExplained);
    }

    [Fact]
    public async Task EnglishAndMathsResultsPage_Displays_AllGcse_CurrentYear_Chart()
    {
        // Arrange
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{QueryString}");

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
        var doc = await Fixture.BrowseToPage($"{_pageUrl}?{QueryString}");

        Assert.Contains($"Test School {_urns[0]}", doc.GetTableHeaderContentByIdAndIndex("all-gcse-current-year-table", 0, 0));
        Assert.Contains($"Test School {_urns[1]}", doc.GetTableHeaderContentByIdAndIndex("all-gcse-current-year-table", 1, 0));
        Assert.Contains("England average", doc.GetTableHeaderContentByIdAndIndex("all-gcse-current-year-table", 2, 0));

        Assert.Contains("90.3%", doc.GetTableCellContentByIdAndIndex("all-gcse-current-year-table", 0, 0));
        Assert.Contains("Not available", doc.GetTableCellContentByIdAndIndex("all-gcse-current-year-table", 1, 0));
        Assert.Contains("80%", doc.GetTableCellContentByIdAndIndex("all-gcse-current-year-table", 2, 0));

    }
}
