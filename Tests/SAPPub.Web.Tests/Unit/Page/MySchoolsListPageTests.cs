using AngleSharp.Dom;
using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page;

[Collection("WebAppCollection")]
public class MySchoolsListPageTests : PageTestsBase
{
    private static string _pageRoute = "/myschools";
    private readonly Mock<IEstablishmentComparisonService> _comparisonService;
    private readonly Mock<IEstablishmentService> _establishmentService;

    public MySchoolsListPageTests(WebAppFixture fixture) : base(fixture)
    {
        _comparisonService = UseMock<IEstablishmentComparisonService>();
        _establishmentService = UseMock<IEstablishmentService>();
    }

    [Fact]
    public async Task MySchoolsListPage_ShowsHeading()
    {
        // Arrange
        _comparisonService.Setup(s => s.GetSavedEstablishments())
            .Returns(new List<string> { "123456" });
        _establishmentService.Setup(s => s.GetEstablishmentAsync("123456", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Establishment { URN = "123456", EstablishmentName = "Test School" });

        // Act
        var document = await Fixture.BrowseToPage(_pageRoute);

        // Assert
        var h1 = document.QuerySelector("h1");
        Assert.Contains("My schools list", h1?.TextContent.Trim());
    }

    [Fact]
    public async Task MySchoolsListPage_ShowsSchoolsList()
    {
        var establishmentBuilder = new EstablishmentTestBuilder();
        var establishmentList = (new List<Establishment>
        {
            establishmentBuilder.WithURN("123456").WithFullAddress().WithStatusCode(EstablishmentStatus.Open).Build(),
            establishmentBuilder.WithURN("123457").WithFullAddress().WithStatusCode(EstablishmentStatus.Closed).Build(),
            establishmentBuilder.WithURN("123458").WithFullAddress().WithStatusCode(EstablishmentStatus.Open).Build(),
            establishmentBuilder.WithURN("123459").WithFullAddress().WithStatusCode(EstablishmentStatus.Open).Build(),
        }).OrderBy(e => e.EstablishmentName).ToList();

        _comparisonService.Setup(s => s.GetSavedEstablishments())
            .Returns(establishmentList.Select(e => e.URN).ToList());

        foreach (var establishment in establishmentList)
        {
            _establishmentService.Setup(s => s.GetEstablishmentAsync(establishment.URN, It.IsAny<CancellationToken>()))
                .ReturnsAsync(establishment);
        }

        // Act
        var document = await Fixture.BrowseToPage(_pageRoute);

        // Assert
        var schoolList = document.QuerySelectorAll(".govuk-checkboxes__item");
        Assert.Equal(establishmentList.Count, schoolList.Length);
    }

    [Fact]
    public async Task MySchoolsListPage_OpenSchool_ShowsSchoolInfo()
    {
        // Arrange
        var establishmentBuilder = new EstablishmentTestBuilder();
        var establishmentList = new List<Establishment>
        {
            establishmentBuilder.WithURN("123457").WithFullAddress().WithStatusCode(EstablishmentStatus.Open).Build(),
        };

        _comparisonService.Setup(s => s.GetSavedEstablishments())
            .Returns(establishmentList.Select(e => e.URN).ToList());

        foreach (var establishment in establishmentList)
        {
            _establishmentService.Setup(s => s.GetEstablishmentAsync(establishment.URN, It.IsAny<CancellationToken>()))
                .ReturnsAsync(establishment);
        }

        // Act
        var document = await Fixture.BrowseToPage(_pageRoute);

        // Assert
        var schoolList = document.QuerySelectorAll(".govuk-checkboxes__item");
        Assert.Equal(establishmentList.Count, schoolList.Length);
        var items = ParseElements(schoolList);
        Assert.Collection(items,
            item =>
            {
                Assert.Equal(establishmentList[0].URN, item.Urn);
                Assert.Equal(establishmentList[0].EstablishmentName, item.LabelText);
                Assert.Equal(establishmentList[0].Address, item.HintParts?.Address);
                Assert.Null(item.HintParts?.StatusTag);
            });
    }

    [Fact]
    public async Task MySchoolsListPage_ClosedSchool_ShowsClosedInfo()
    {
        // Arrange
        var establishmentBuilder = new EstablishmentTestBuilder();
        var establishmentList = (new List<Establishment>
        {
            new EstablishmentTestBuilder().WithURN("123457").WithFullAddress().WithStatusCode(EstablishmentStatus.Closed).Build(),
            new EstablishmentTestBuilder().WithURN("123458").WithFullAddress().WithStatusCode(EstablishmentStatus.Closed).WithClosedDate(new DateOnly(2020, 1, 1)).Build()
        }).OrderBy(e => e.EstablishmentName).ToList();

        _comparisonService.Setup(s => s.GetSavedEstablishments())
            .Returns(establishmentList.Select(e => e.URN).ToList());

        foreach (var establishment in establishmentList)
        {
            _establishmentService.Setup(s => s.GetEstablishmentAsync(establishment.URN, It.IsAny<CancellationToken>()))
                .ReturnsAsync(establishment);
        }

        // Act
        var document = await Fixture.BrowseToPage(_pageRoute);

        // Assert
        var schoolList = document.QuerySelectorAll(".govuk-checkboxes__item");
        Assert.Equal(establishmentList.Count, schoolList.Length);
        var items = ParseElements(schoolList);

        var itemWithClosedDate = items.Single(i => i.Urn == "123458");
        var expectedWithClosedDate = establishmentList.Single(e => e.URN == "123458");
        Assert.Equal(expectedWithClosedDate.URN, itemWithClosedDate.Urn);
        Assert.Equal(expectedWithClosedDate.EstablishmentName, itemWithClosedDate.LabelText);
        Assert.Equal(expectedWithClosedDate.Address, itemWithClosedDate.HintParts?.Address);
        Assert.Equal("Closed in January 2020", itemWithClosedDate.HintParts?.StatusTag);

        var itemWithoutClosedDate = items.Single(i => i.Urn == "123457");
        var expectedWithoutClosedDate = establishmentList.Single(e => e.URN == "123457");
        Assert.Equal(expectedWithoutClosedDate.URN, itemWithoutClosedDate.Urn);
        Assert.Equal(expectedWithoutClosedDate.EstablishmentName, itemWithoutClosedDate.LabelText);
        Assert.Equal(expectedWithoutClosedDate.Address, itemWithoutClosedDate.HintParts?.Address);
        Assert.Equal("Closed", itemWithoutClosedDate.HintParts?.StatusTag);
    }

    private IReadOnlyList<CheckboxItemView> ParseElements(IHtmlCollection<IElement> items)
    {
        var results = new List<CheckboxItemView>(items.Length);

        for (var index = 0; index < items.Length; index++)
        {
            var item = items[index];
            var input = item.QuerySelector("input.govuk-checkboxes__input");
            var label = item.QuerySelector("label.govuk-checkboxes__label");
            var hint = item.QuerySelector(".govuk-checkboxes__hint");

            var address = hint?.QuerySelector(":scope > div")?.TextContent.Trim() ?? string.Empty;
            var statusTag = hint?.QuerySelector("strong.govuk-tag")?.TextContent.Trim();

            results.Add(new CheckboxItemView(
                Urn: input?.GetAttribute("value") ?? string.Empty,
                LabelText: label?.TextContent.Trim() ?? string.Empty,
                HintParts: new CheckboxHintParts(address, statusTag)));
        }

        return results;
    }

    public sealed record CheckboxItemView(string Urn, string LabelText, CheckboxHintParts? HintParts);
    public sealed record CheckboxHintParts(string Address, string? StatusTag);
}
