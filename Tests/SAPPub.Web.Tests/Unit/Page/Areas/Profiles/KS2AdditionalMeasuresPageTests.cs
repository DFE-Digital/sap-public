using Moq;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Profiles;

[Collection("WebAppCollection")]
public class KS2AdditionalMeasuresPageTests : PageTestsBase
{
    private readonly string _pageRoute = "/primary-performance/additional-measures";
    private readonly string _urn = "149976";
    private readonly string _laName = "Test LA";

    private readonly EstablishmentServiceModel _establishment = new();
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;

    private readonly KS2AdditionalMeasuresModel _ks2AdditionalMeasuresModel;
    private readonly Mock<IKS2AdditionalMeasuresService> _ks2AdditionalMeasuresService = new();


    public KS2AdditionalMeasuresPageTests(WebAppFixture fixture) : base(fixture)
    {
        _ks2AdditionalMeasuresService = UseMock<IKS2AdditionalMeasuresService>();
        _mockEstablishmentService = UseMock<IEstablishmentService>();
        _establishment = new EstablishmentTestBuilder()
            .WithURN(_urn)
            .WithEstablishmentName($"School{_urn}")
            .WithIsKeyStage2(true)
            .WithLAName(_laName)
            .BuildServiceModel();

        _ks2AdditionalMeasuresModel = GetKS2AdditionalMeasuresModel();

        _mockEstablishmentService
           .Setup(a => a.GetEstablishmentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
           .ReturnsAsync(_establishment);

        _ks2AdditionalMeasuresService
            .Setup(s => s.GetAdditionalMeasures(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_ks2AdditionalMeasuresModel);
    }

    [Fact]
    public async Task AdditionalMeasures_HasCorrectTitle()
    {
        // Arrange
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var title = doc.QuerySelector("title");
        Assert.NotNull(title);

        var expectedTitle = $"School149976 - Primary Additional measures - School Profiles - GOV.UK";
        Assert.Contains(expectedTitle, title.TextContent.Trim());
    }

    [Fact]
    public async Task AdditionalMeasures_DisplaysCorrectHeading()
    {
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var heading = doc.QuerySelectorAll("h2");
        Assert.NotNull(heading[1]);
        Assert.Contains("Additional measures", heading[1].TextContent.Trim());
    }

    [Fact]
    public async Task AdditionalMeasures_HasCorrectSubNavigationLinks()
    {
        // Arrange
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);
        var container = doc.QuerySelector("#sub-navigation-academic-performance");
        var links = container?.QuerySelectorAll(".moj-sub-navigation__link");

        Assert.NotNull(links);
        Assert.Equal(4, links.Length);
    }

    [Fact]
    public async Task AdditionalMeasures_ShowsFindOutMoreInformation()
    {
        // Arrange
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);
        var container = doc.QuerySelector("#details-additional-measures");

        Assert.NotNull(container);
    }

    [Fact]
    public async Task AdditionalMeasures_Displays_BreakdownOfNumbers()
    {
        // Arrange
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);
        var expectedModel = GetKS2AdditionalMeasuresModel();

        // Act
        var doc = await Fixture.BrowseToPage(url);

        Assert.Contains("Pupil group", doc.GetTableHeaderContentByIdAndIndex("additional-measures-breakdown-table", 0, 0));
        Assert.Contains("Grammar, punctuation, spelling at the expected standard", doc.GetTableHeaderContentByIdAndIndex("additional-measures-breakdown-table", 0, 1));
        Assert.Contains("Grammar, punctuation, spelling at the higher standard", doc.GetTableHeaderContentByIdAndIndex("additional-measures-breakdown-table", 0, 2));

        // Assert
        Assert.Equal("School", doc.GetTableHeaderContentByIdAndIndex("additional-measures-breakdown-table", 1, 0));
        Assert.Equal(expectedModel.EstablishmentGrammarAtExpectedStandard!.Value.ToString() + "%", doc.GetTableCellContentByIdAndIndex("additional-measures-breakdown-table", 1, 0));
        Assert.Equal(expectedModel.EstablishmentGrammarAtHigherStandard!.Value.ToString() + "%", doc.GetTableCellContentByIdAndIndex("additional-measures-breakdown-table", 1, 1));

        Assert.Equal($"{_laName} average", doc.GetTableHeaderContentByIdAndIndex("additional-measures-breakdown-table", 2, 0));
        Assert.Equal(expectedModel.LAGrammarAtExpectedStandard!.Value.ToString() + "%", doc.GetTableCellContentByIdAndIndex("additional-measures-breakdown-table",2, 0));
        Assert.Equal(expectedModel.LAGrammarAtHigherStandard!.Value.ToString() + "%", doc.GetTableCellContentByIdAndIndex("additional-measures-breakdown-table", 2, 1));

        Assert.Equal("England average", doc.GetTableHeaderContentByIdAndIndex("additional-measures-breakdown-table", 3, 0));
        Assert.Equal(expectedModel.EnglandGrammarAtExpectedStandard!.Value.ToString() + "%", doc.GetTableCellContentByIdAndIndex("additional-measures-breakdown-table", 3, 0));
        Assert.Equal(expectedModel.EnglandGrammarAtHigherStandard!.Value.ToString() + "%", doc.GetTableCellContentByIdAndIndex("additional-measures-breakdown-table", 3, 1));

    }

    [Fact]
    public async Task AdditionalMeasures_ShowsPupilPopulationAccordion()
    {
        // Arrange
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);
        var accordion = doc.QuerySelector("#pupil-population-accordion");
        var ehcpSection = doc.QuerySelector("#pupils-with-ehcp-section");
        var senSupportSection = doc.QuerySelector("#pupils-with-sen-support-section");

        //Assert
        Assert.NotNull(accordion);
        Assert.NotNull(ehcpSection);
        Assert.NotNull(senSupportSection);
    }

    private static KS2AdditionalMeasuresModel GetKS2AdditionalMeasuresModel()
    {
        return new KS2AdditionalMeasuresModel
        {
            EnglandGrammarAtExpectedStandard = new CodedDouble(1, string.Empty, "1"),
            EnglandGrammarAtHigherStandard = new CodedDouble(1, string.Empty, "2"),
            EstablishmentGrammarAtExpectedStandard  = new CodedDouble(1, string.Empty, "3"),
            EstablishmentGrammarAtHigherStandard = new CodedDouble(1, string.Empty, "4"),
            LAGrammarAtExpectedStandard = new CodedDouble(1, string.Empty, "5"),
            LAGrammarAtHigherStandard = new CodedDouble(1, string.Empty, "6")
        };
    }
}
