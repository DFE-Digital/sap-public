using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Compare.Secondary;

[Collection("WebAppCollection")]
public class NextStepsPageTests : PageTestsBase
{
    private string _pageUrl = "compare/secondary/next-steps?urns=119052&urns=124500";
    private readonly Mock<IEstablishmentService> _establishmentService;

    private string schoolName1 = "xyz School";
    private string schoolName2 = "abc School";
    private string website1 = "https://www.test.com";
    private string telephone1 = "101011";

    public NextStepsPageTests(WebAppFixture fixture) : base(fixture)
    {
        // set up the mock establishment service to return establishments for the URNs in the query string
        // this is used by the page validation filter to determine if the establishments are secondary and should be compared
        _establishmentService = UseMock<IEstablishmentService>();
        var establishmentList = (new List<Establishment>
        {
            new EstablishmentTestBuilder().WithURN("119052").WithEstablishmentName(schoolName1).WithTelephoneNum(telephone1).WithWebsite(website1).WithIsKeyStage4(true).Build(),
            new EstablishmentTestBuilder().WithURN("124500").WithEstablishmentName(schoolName2).WithIsKeyStage4(true).Build(),
        }).ToList();


        establishmentList.Select(e =>
            _establishmentService.Setup(s =>
            s.GetEstablishmentAsync(e.URN, It.IsAny<CancellationToken>())).ReturnsAsync(e)).ToList();

        _establishmentService
            .Setup(s => s.GetEstablishmentsAsync(It.IsAny<List<string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(establishmentList);
    }

    [Fact]
    public async Task NextSteps_Displays_VerticalNavigation()
    {
        // Act
        var doc = await Fixture.BrowseToPage(_pageUrl);

        // Assert
        Assert.NotNull(doc.QuerySelector(".moj-side-navigation"));
        Assert.Equal(4, doc.QuerySelectorAll(".moj-side-navigation__item").Length);
        Assert.Single(doc.QuerySelectorAll(".moj-side-navigation__item--active"));
    }

    [Fact]
    public async Task DisplaysPagination()
    {
        // Arrange
        var doc = await Fixture.BrowseToPage(_pageUrl);

        // Act
        var nav = doc.QuerySelector("#next-steps-pagination");
        var navNext = doc.QuerySelector("#next-steps-pagination .govuk-pagination__next a");
        var navPrevious = doc.QuerySelector("#next-steps-pagination .govuk-pagination__prev a");

        Assert.NotNull(nav);
        Assert.Null(navNext);
        Assert.NotNull(navPrevious);
        Assert.Contains("Destinations after year 11", navPrevious.TextContent);
    }

    [Fact]
    public async Task DisplaysCorrectH2Information()
    {
        // Act
        var doc = await Fixture.BrowseToPage(_pageUrl);
        var exploreMoreInformation = doc.GetElementsByTagName("h2")[1];
        var arrangeASchoolVisit = doc.GetElementsByTagName("h2")[2];
        var applyingToASchool = doc.GetElementsByTagName("h2")[3];

        // Assert
        Assert.NotNull(exploreMoreInformation);
        Assert.NotNull(arrangeASchoolVisit);
        Assert.NotNull(applyingToASchool);
        Assert.Equal("Explore more information", exploreMoreInformation.TextContent);
        Assert.Equal("Arrange a school visit", arrangeASchoolVisit.TextContent);
        Assert.Equal("Applying to a school", applyingToASchool.TextContent);
    }

    [Fact]
    public async Task ShowsSchoolInfoComparisonLists()
    {
        // Act
        var doc = await Fixture.BrowseToPage(_pageUrl);
        var summaryLists = doc.GetElementsByClassName("govuk-summary-list");

        // Assert
        Assert.Equal(2, summaryLists.Count());
        var summaryListOneValues = summaryLists[0].QuerySelectorAll(".govuk-summary-list__value");
        var summaryListTwoValues = summaryLists[1].QuerySelectorAll(".govuk-summary-list__value");

        Assert.Contains($"View {schoolName2}'s Profile", summaryListOneValues[0].TextContent);
        Assert.Contains($"Not available", summaryListOneValues[1].TextContent.Trim());
        Assert.Contains($"Not available", summaryListOneValues[2].TextContent.Trim());

        Assert.Contains($"View {schoolName1}'s Profile", summaryListTwoValues[0].TextContent);
        Assert.Contains($"https://www.test.com", summaryListTwoValues[1].TextContent);
        Assert.Contains($"101011", summaryListTwoValues[2].TextContent.Trim());
    }
}
