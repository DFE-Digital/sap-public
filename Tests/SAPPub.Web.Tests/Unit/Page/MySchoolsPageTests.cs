using AngleSharp.Dom;
using Moq;
using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Models.MySchools;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Tests.Unit.Page;

[Collection("WebAppCollection")]
public class MySchoolsPageTests : PageTestsBase
{
    private static string _pageRoute = "/my-schools/view";
    private readonly Mock<IMySchoolsListService> _comparisonService;
    private readonly Mock<IEstablishmentService> _establishmentService;

    public MySchoolsPageTests(WebAppFixture fixture) : base(fixture)
    {
        _comparisonService = UseMock<IMySchoolsListService>();
        _establishmentService = UseMock<IEstablishmentService>();
    }

    [Fact]
    public async Task MySchoolsListPage_ShowsHeading()
    {
        // Arrange
        _comparisonService.Setup(s => s.GetSavedEstablishments())
            .Returns(new List<string> { "123456" });
        _establishmentService.Setup(s => s.GetEstablishmentAsync("123456", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentServiceModel { URN = "123456", EstablishmentName = "Test School" });

        // Act
        var document = await Fixture.BrowseToPage(_pageRoute);

        // Assert
        var h1 = document.QuerySelector("h1");
        Assert.Contains("My schools list", h1?.TextContent.Trim());
    }

    [Fact]
    public async Task MySchoolsListPage_ShowsSchoolsListOrderedAlphabetically()
    {
        var establishmentList = new List<EstablishmentServiceModel>
        {
            new EstablishmentTestBuilder().WithURN("123456").WithEstablishmentName("Charlie").WithFullAddress().WithStatusCode(EstablishmentStatus.Open).BuildServiceModel(),
            new EstablishmentTestBuilder().WithURN("123457").WithEstablishmentName("Alpha").WithFullAddress().WithStatusCode(EstablishmentStatus.Closed).BuildServiceModel(),
            new EstablishmentTestBuilder().WithURN("123458").WithEstablishmentName("Bravo").WithFullAddress().WithStatusCode(EstablishmentStatus.Open).BuildServiceModel(),
            new EstablishmentTestBuilder().WithURN("123459").WithEstablishmentName("Delta").WithFullAddress().WithStatusCode(EstablishmentStatus.Open).BuildServiceModel(),
        }.ToList();

        var orderedUrns = establishmentList.OrderByDescending(e => e.EstablishmentName).Select(e => e.URN).ToList();
        _comparisonService.Setup(s => s.GetSavedEstablishments())
            .Returns(orderedUrns);

        foreach (var establishment in establishmentList)
        {
            _establishmentService.Setup(s => s.GetEstablishmentAsync(establishment.URN, It.IsAny<CancellationToken>()))
                .ReturnsAsync(establishment);
        }

        _establishmentService.Setup(s => s.GetEstablishmentsAsync(orderedUrns, It.IsAny<CancellationToken>()))
                .ReturnsAsync(establishmentList);

        // Act
        var document = await Fixture.BrowseToPage(_pageRoute);

        // Assert
        var schoolList = document.QuerySelectorAll(".govuk-checkboxes__item");
        Assert.Equal(establishmentList.Count, schoolList.Length);
        var items = ParseCheckboxElements(schoolList);

        Assert.Equal("Alpha", items[0].LabelText);
        Assert.Equal("Bravo", items[1].LabelText);
        Assert.Equal("Charlie", items[2].LabelText);
        Assert.Equal("Delta", items[3].LabelText);
    }

    [Fact]
    public async Task MySchoolsListPage_OpenSchool_ShowsSchoolInfo()
    {
        // Arrange
        var establishmentBuilder = new EstablishmentTestBuilder();
        var establishmentList = new List<EstablishmentServiceModel>
        {
            establishmentBuilder.WithURN("123457").WithFullAddress().WithStatusCode(EstablishmentStatus.Open).BuildServiceModel(),
        };

        _comparisonService.Setup(s => s.GetSavedEstablishments())
            .Returns(establishmentList.Select(e => e.URN).ToList());

        foreach (var establishment in establishmentList)
        {
            _establishmentService.Setup(s => s.GetEstablishmentAsync(establishment.URN, It.IsAny<CancellationToken>()))
                .ReturnsAsync(establishment);
        }

        var urns = establishmentList.Select(e => e.URN).ToList();

        _establishmentService.Setup(s => s.GetEstablishmentsAsync(urns, It.IsAny<CancellationToken>()))
                .ReturnsAsync(establishmentList.AsEnumerable());

        // Act
        var document = await Fixture.BrowseToPage(_pageRoute);

        // Assert
        var schoolList = document.QuerySelectorAll(".govuk-checkboxes__item");
        Assert.Equal(establishmentList.Count, schoolList.Length);
        var items = ParseCheckboxElements(schoolList);
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
        var establishmentList = new List<EstablishmentServiceModel>
        {
            new EstablishmentTestBuilder().WithURN("123457").WithFullAddress().WithStatusCode(EstablishmentStatus.Closed).BuildServiceModel(),
            new EstablishmentTestBuilder().WithURN("123458").WithFullAddress().WithStatusCode(EstablishmentStatus.Closed).WithClosedDate(new DateOnly(2020, 1, 1)).BuildServiceModel()
        }.OrderBy(e => e.EstablishmentName).ToList();

        _comparisonService.Setup(s => s.GetSavedEstablishments())
            .Returns(establishmentList.Select(e => e.URN).ToList());

        foreach (var establishment in establishmentList)
        {
            _establishmentService.Setup(s => s.GetEstablishmentAsync(establishment.URN, It.IsAny<CancellationToken>()))
                .ReturnsAsync(establishment);
        }

        var urns = establishmentList.Select(e => e.URN).ToList();

        _establishmentService.Setup(s => s.GetEstablishmentsAsync(urns, It.IsAny<CancellationToken>()))
                .ReturnsAsync(establishmentList.AsEnumerable());

        // Act
        var document = await Fixture.BrowseToPage(_pageRoute);

        // Assert
        var schoolList = document.QuerySelectorAll(".govuk-checkboxes__item");
        Assert.Equal(establishmentList.Count, schoolList.Length);
        var items = ParseCheckboxElements(schoolList);

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

    [Fact]
    public async Task MySchoolsListPage_UrnIsNotInSavedEstablishments_IgnoresUrn()
    {
        var establishmentBuilder = new EstablishmentTestBuilder();
        var establishmentsFromRepository = new List<EstablishmentServiceModel>
        {
            establishmentBuilder.WithURN("123456").WithFullAddress().WithStatusCode(EstablishmentStatus.Open).BuildServiceModel(),
            establishmentBuilder.WithURN("123457").WithFullAddress().WithStatusCode(EstablishmentStatus.Open).BuildServiceModel()
        }.OrderBy(e => e.EstablishmentName).ToList();

        var establishmentsStoredByUser = establishmentsFromRepository.Select(e => e.URN).ToList();
        establishmentsStoredByUser.Add("123458"); // Add an extra URN that won't be found in the repository

        _comparisonService.Setup(s => s.GetSavedEstablishments())
            .Returns(establishmentsStoredByUser);

        foreach (var establishment in establishmentsFromRepository)
        {
            _establishmentService.Setup(s => s.GetEstablishmentAsync(establishment.URN, It.IsAny<CancellationToken>()))
                .ReturnsAsync(establishment);
        }

        _establishmentService.Setup(s => s.GetEstablishmentsAsync(establishmentsStoredByUser, It.IsAny<CancellationToken>()))
                .ReturnsAsync(establishmentsFromRepository.AsEnumerable());

        // Act
        var document = await Fixture.BrowseToPage(_pageRoute);

        // Assert
        var schoolList = document.QuerySelectorAll(".govuk-checkboxes__item");
        Assert.Equal(establishmentsFromRepository.Count, schoolList.Length);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(7)]
    public async Task MySchoolsListPage_CompareSelection_InvalidAmountOfEstablishmentsSelected_ShowsErrorMessages(int selectedEstablishmentsCount)
    {
        var establishmentList = new List<EstablishmentServiceModel>
        {
            new EstablishmentTestBuilder().WithURN("123456").BuildServiceModel(),
            new EstablishmentTestBuilder().WithURN("123457").BuildServiceModel(),
            new EstablishmentTestBuilder().WithURN("123458").BuildServiceModel(),
            new EstablishmentTestBuilder().WithURN("123459").BuildServiceModel(),
            new EstablishmentTestBuilder().WithURN("123460").BuildServiceModel(),
            new EstablishmentTestBuilder().WithURN("123461").BuildServiceModel(),
            new EstablishmentTestBuilder().WithURN("123462").BuildServiceModel()
        }.ToList();

        _comparisonService.Setup(s => s.GetSavedEstablishments())
            .Returns(establishmentList.OrderByDescending(e => e.EstablishmentName).Select(e => e.URN).ToList());

        foreach (var establishment in establishmentList)
        {
            _establishmentService.Setup(s => s.GetEstablishmentAsync(establishment.URN, It.IsAny<CancellationToken>()))
                .ReturnsAsync(establishment);
        }

        var selectedEstablishments = establishmentList.Take(selectedEstablishmentsCount).Select(e => e.URN).ToList();
        var formData = new MySchoolsListViewModel
        {
            SelectedEstablishmentUrns = selectedEstablishments
        };

        // Act
        var response = await Fixture.PostToPage(_pageRoute, formData);
        var document = response.Document;

        // Assert
        Assert.NotNull(document);
        var summaryErrors = document.GetErrorSummaryErrors();
        var errorMessages = document.GetErrorMessages();
        Assert.Single(summaryErrors);
        Assert.Single(errorMessages);
    }

    [Fact]
    public async Task MySchoolsListPage_CompareSelection_TwoSelected_Redirects()
    {
        var establishmentList = new List<EstablishmentServiceModel>
        {
            new EstablishmentTestBuilder().WithURN("123456").BuildServiceModel(),
            new EstablishmentTestBuilder().WithURN("123457").BuildServiceModel(),
            new EstablishmentTestBuilder().WithURN("123458").BuildServiceModel(),
            new EstablishmentTestBuilder().WithURN("123459").BuildServiceModel(),
        }.ToList();

        _comparisonService.Setup(s => s.GetSavedEstablishments())
            .Returns(establishmentList.OrderByDescending(e => e.EstablishmentName).Select(e => e.URN).ToList());

        foreach (var establishment in establishmentList)
        {
            _establishmentService.Setup(s => s.GetEstablishmentAsync(establishment.URN, It.IsAny<CancellationToken>()))
                .ReturnsAsync(establishment);
        }

        var formData = new MySchoolsListViewModel
        {
            SelectedEstablishmentUrns = new List<string>() { "123456", "123457" }
        };

        // Act
        var response = await Fixture.PostToPage(_pageRoute, formData);
        var document = response.Document;

        // Assert
        Assert.Null(document);
        Assert.NotNull(response.RedirectionLocation);
    }

    [Fact]
    public async Task MySchoolsListPage_RemoveSelection_NoEstablishmentsSelected_ShowsErrorMessages()
    {
        var establishmentList = new List<EstablishmentServiceModel>
        {
            new EstablishmentTestBuilder().WithURN("123456").BuildServiceModel(),
            new EstablishmentTestBuilder().WithURN("123457").BuildServiceModel(),
            new EstablishmentTestBuilder().WithURN("123458").BuildServiceModel(),
            new EstablishmentTestBuilder().WithURN("123459").BuildServiceModel(),
            new EstablishmentTestBuilder().WithURN("123460").BuildServiceModel(),
            new EstablishmentTestBuilder().WithURN("123461").BuildServiceModel(),
            new EstablishmentTestBuilder().WithURN("123462").BuildServiceModel()
        }.ToList();

        _comparisonService.Setup(s => s.GetSavedEstablishments())
            .Returns(establishmentList.OrderByDescending(e => e.EstablishmentName).Select(e => e.URN).ToList());

        foreach (var establishment in establishmentList)
        {
            _establishmentService.Setup(s => s.GetEstablishmentAsync(establishment.URN, It.IsAny<CancellationToken>()))
                .ReturnsAsync(establishment);
        }

        var selectedEstablishments = new List<string>();
        var formData = new MySchoolsListViewModel
        {
            SelectedEstablishmentUrns = selectedEstablishments
        };

        // Act
        var response = await Fixture.PostToPage(_pageRoute, formData, Constants.Constants.ActionRemove);
        var document = response.Document;

        // Assert
        Assert.NotNull(document);
        var summaryErrors = document.GetErrorSummaryErrors();
        var errorMessages = document.GetErrorMessages();
        Assert.Single(summaryErrors);
        Assert.Single(errorMessages);
        Assert.Contains("Select at least one school to remove", errorMessages[0].Trim());
    }

    [Fact]
    public async Task MySchoolsListPage_RemoveSelection_Selected_Redirects()
    {
        var establishmentList = new List<EstablishmentServiceModel>
        {
            new EstablishmentTestBuilder().WithURN("123456").BuildServiceModel(),
            new EstablishmentTestBuilder().WithURN("123457").BuildServiceModel(),
            new EstablishmentTestBuilder().WithURN("123458").BuildServiceModel(),
            new EstablishmentTestBuilder().WithURN("123459").BuildServiceModel(),
        }.ToList();

        _comparisonService.Setup(s => s.GetSavedEstablishments())
            .Returns(establishmentList.OrderByDescending(e => e.EstablishmentName).Select(e => e.URN).ToList());

        foreach (var establishment in establishmentList)
        {
            _establishmentService.Setup(s => s.GetEstablishmentAsync(establishment.URN, It.IsAny<CancellationToken>()))
                .ReturnsAsync(establishment);
        }

        var formData = new MySchoolsListViewModel
        {
            SelectedEstablishmentUrns = new List<string>() { "123456", "123457" }
        };

        // Act
        var response = await Fixture.PostToPage(_pageRoute, formData, ActionRemove);
        var document = response.Document;

        // Assert
        Assert.Null(document);
        Assert.NotNull(response.RedirectionLocation);
        Assert.Equal("/my-schools/remove-confirm", response.RedirectionLocation);
    }

    private IReadOnlyList<CheckboxItemView> ParseCheckboxElements(IHtmlCollection<IElement> items)
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
