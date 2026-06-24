using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Compare.Secondary;

[Collection("WebAppCollection")]
public class EnglishAndMathsResultsTests : PageTestsBase
{
    private string _pageUrl = "compare/secondary/english-and-maths-results?urns=123456&urns=123457";
    private readonly Mock<IEstablishmentService> _establishmentService;

    public EnglishAndMathsResultsTests(WebAppFixture fixture) : base(fixture)
    {
        // set up the mock establishment service to return establishments for the URNs in the query string
        // this is used by the page validation filter to determine if the establishments are secondary and should be compared
        _establishmentService = UseMock<IEstablishmentService>();
        var establishmentList = (new List<EstablishmentServiceModel>
        {
            new EstablishmentTestBuilder().WithURN("123456").WithIsKeyStage4(true).BuildServiceModel(),
            new EstablishmentTestBuilder().WithURN("123457").WithIsKeyStage4(true).BuildServiceModel(),
        }).ToList();

        establishmentList.Select(e =>
                                _establishmentService.Setup(s =>
            s.GetEstablishmentAsync(e.URN, It.IsAny<CancellationToken>())).ReturnsAsync(e)).ToList();
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

        Assert.NotNull(nav);
        Assert.NotNull(navNext);
        Assert.NotNull(navPrevious);
        Assert.Contains("Academic performance: Pupil attainment", navPrevious.TextContent);
        Assert.Contains("Destinations after year 11", navNext.TextContent);
    }
}
