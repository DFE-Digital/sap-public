using Moq;
using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Areas.Profiles.Helpers;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Profiles.KS4;

[Collection("WebAppCollection")]
public class AttainmentEnglishAndMathsTests : PageTestsBase
{
    private static string _pageRoute = "/secondary-performance/english-and-maths";
    private readonly Mock<IAcademicPerformanceEnglishAndMathsResultsService> _serviceMock;

    public AttainmentEnglishAndMathsTests(WebAppFixture fixture) : base(fixture)
    {
        _serviceMock = UseMock<IAcademicPerformanceEnglishAndMathsResultsService>();
    }

    [Fact]
    public async Task DisadvantagedBreakdownTable_DataAvailable_ShowsExpectedValues()
    {
        // Arrange
        var urn = "143034";
        var gradeSelection = GcseGradeDataSelection.Grade5AndAbove;
        var grade = gradeSelection.ToGradeValue();
        var establishmentName = "St Paul's Church of England Academy";
        var expectedModel = new EnglishAndMathsResultsModelBuilder()
            .WithUrn(urn)
            .WithEstablishmentName(establishmentName)
            .WithLaName("Durham")
            .WithIsKS4(true)
            .WithCurrentYearData()
            .Build();

        _serviceMock
            .Setup(service => service.GetEnglishAndMathsResultsAsync(
                urn,
                grade,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, $"{_pageRoute}/{gradeSelection.ToRouteSegment()}"));

        // Assert
        Assert.Contains("School", doc.GetTableHeaderContentByIdAndIndex("breakdown-disadvantaged-table", 1, 0));
        Assert.Contains("Durham average", doc.GetTableHeaderContentByIdAndIndex("breakdown-disadvantaged-table", 2, 0));
        Assert.Contains("England average", doc.GetTableHeaderContentByIdAndIndex("breakdown-disadvantaged-table", 3, 0));
        Assert.Equal($"{expectedModel.EstablishmentDisadvantaged.CurrentYear!.Value.ToString()}%", doc.GetTableCellContentByIdAndIndex("breakdown-disadvantaged-table", 1, 0));
        Assert.Equal($"{expectedModel.LocalAuthorityDisadvantaged.CurrentYear!.Value.ToString()}%", doc.GetTableCellContentByIdAndIndex("breakdown-disadvantaged-table", 2, 0));
        Assert.Equal($"{expectedModel.EnglandDisadvantaged.CurrentYear!.Value.ToString()}%", doc.GetTableCellContentByIdAndIndex("breakdown-disadvantaged-table", 3, 0));
    }

    [Fact]
    public async Task DisadvantagedBreakdownTable_NoDataAvailable_ShowsExpectedValues()
    {
        // Arrange
        var urn = "143034";
        var gradeSelection = GcseGradeDataSelection.Grade5AndAbove;
        var grade = gradeSelection.ToGradeValue();
        var establishmentName = "St Paul's Church of England Academy";
        var expectedModel = new EnglishAndMathsResultsModelBuilder()
            .WithUrn(urn)
            .WithEstablishmentName(establishmentName)
            .WithLaName("Durham")
            .WithIsKS4(true)
            .Build();

        _serviceMock
            .Setup(service => service.GetEnglishAndMathsResultsAsync(
                urn,
                grade,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, $"{_pageRoute}/{gradeSelection.ToRouteSegment()}"));

        // Assert
        Assert.Contains("School", doc.GetTableHeaderContentByIdAndIndex("breakdown-disadvantaged-table", 1, 0));
        Assert.Contains("Durham average", doc.GetTableHeaderContentByIdAndIndex("breakdown-disadvantaged-table", 2, 0));
        Assert.Contains("England average", doc.GetTableHeaderContentByIdAndIndex("breakdown-disadvantaged-table", 3, 0));
        Assert.Equal("Not available", doc.GetTableCellContentByIdAndIndex("breakdown-disadvantaged-table", 1, 0));
        Assert.Equal("Not available", doc.GetTableCellContentByIdAndIndex("breakdown-disadvantaged-table", 2, 0));
        Assert.Equal("Not available", doc.GetTableCellContentByIdAndIndex("breakdown-disadvantaged-table", 3, 0));
    }

    [Fact]
    public async Task Year7Selected_TableShowsExpectedValues()
    {
        // Arrange
        var urn = "143034";
        var gradeSelection = GcseGradeDataSelection.Grade7AndAbove;
        var grade = gradeSelection.ToGradeValue();
        var establishmentName = "St Paul's Church of England Academy";
        var expectedModel = new EnglishAndMathsResultsModelBuilder()
            .WithUrn(urn)
            .WithEstablishmentName(establishmentName)
            .WithLaName("Durham")
            .WithIsKS4(true)
            .WithCurrentYearData()
            .Build();

        _serviceMock
            .Setup(service => service.GetEnglishAndMathsResultsAsync(
                urn,
                grade,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, $"{_pageRoute}/{gradeSelection.ToRouteSegment()}"));

        // Assert
        Assert.Contains("School", doc.GetTableHeaderContentByIdAndIndex("all-gcse-data-overtime-table", 1, 0));
        Assert.Contains($"{expectedModel.LAName} average", doc.GetTableHeaderContentByIdAndIndex("all-gcse-data-overtime-table", 2, 0));
        Assert.Contains($"England average", doc.GetTableHeaderContentByIdAndIndex("all-gcse-data-overtime-table", 3, 0));
        // current year data
        Assert.Equal($"{expectedModel.EstablishmentAll.CurrentYear!.Value.ToString()}%", doc.GetTableCellContentByIdAndIndex("all-gcse-data-overtime-table", 1, 2));
        Assert.Equal($"{expectedModel.LocalAuthorityAll.CurrentYear!.Value.ToString()}%", doc.GetTableCellContentByIdAndIndex("all-gcse-data-overtime-table", 2, 2));
        Assert.Equal($"{expectedModel.EnglandAll.CurrentYear!.Value.ToString()}%", doc.GetTableCellContentByIdAndIndex("all-gcse-data-overtime-table", 3, 2));
        // previous years data
        Assert.Equal("Not available", doc.GetTableCellContentByIdAndIndex("all-gcse-data-overtime-table", 1, 0));
        Assert.Equal("Not available", doc.GetTableCellContentByIdAndIndex("all-gcse-data-overtime-table", 1, 1));
        Assert.Equal("Not available", doc.GetTableCellContentByIdAndIndex("all-gcse-data-overtime-table", 2, 0));
        Assert.Equal("Not available", doc.GetTableCellContentByIdAndIndex("all-gcse-data-overtime-table", 2, 1));
        Assert.Equal("Not available", doc.GetTableCellContentByIdAndIndex("all-gcse-data-overtime-table", 3, 0));
        Assert.Equal("Not available", doc.GetTableCellContentByIdAndIndex("all-gcse-data-overtime-table", 3, 1));

        Assert.Contains($"School", doc.GetTableHeaderContentByIdAndIndex("breakdown-gcse-current-year-table", 1, 0));
        Assert.Contains($"{expectedModel.LAName} average", doc.GetTableHeaderContentByIdAndIndex("breakdown-gcse-current-year-table", 2, 0));
        Assert.Contains($"England average", doc.GetTableHeaderContentByIdAndIndex("breakdown-gcse-current-year-table", 3, 0));
        Assert.Equal($"{expectedModel.EstablishmentGirls.CurrentYear!.Value.ToString()}%", doc.GetTableCellContentByIdAndIndex("breakdown-gcse-current-year-table", 1, 0));
        Assert.Equal($"{expectedModel.EstablishmentBoys.CurrentYear!.Value.ToString()}%", doc.GetTableCellContentByIdAndIndex("breakdown-gcse-current-year-table", 1, 1));
        Assert.Equal($"{expectedModel.LocalAuthorityGirls.CurrentYear!.Value.ToString()}%", doc.GetTableCellContentByIdAndIndex("breakdown-gcse-current-year-table", 2, 0));
        Assert.Equal($"{expectedModel.LocalAuthorityBoys.CurrentYear!.Value.ToString()}%", doc.GetTableCellContentByIdAndIndex("breakdown-gcse-current-year-table", 2, 1));
        Assert.Equal($"{expectedModel.EnglandGirls.CurrentYear!.Value.ToString()}%", doc.GetTableCellContentByIdAndIndex("breakdown-gcse-current-year-table", 3, 0));
        Assert.Equal($"{expectedModel.EnglandBoys.CurrentYear!.Value.ToString()}%", doc.GetTableCellContentByIdAndIndex("breakdown-gcse-current-year-table", 3, 1));
    }
}
