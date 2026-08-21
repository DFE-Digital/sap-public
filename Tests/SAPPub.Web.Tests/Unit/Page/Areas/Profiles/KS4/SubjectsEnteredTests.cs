using Moq;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Profiles.KS4;

[Collection("WebAppCollection")]
public class SubjectsEnteredTests : PageTestsBase
{
    private static string _urn = "143034";
    private static string _establishmentName = "Loreto High School Chorlton";
    private static string _pageRoute = "/secondary-performance/subjects-entered";
    private readonly Mock<IKS4EstablishmentSubjectEntriesService> _mockEstablishmentSubjectEntriesService;
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;
    private EstablishmentMinimumServiceModel _establishment;

    public SubjectsEnteredTests(WebAppFixture fixture) : base(fixture)
    {
        _mockEstablishmentSubjectEntriesService = UseMock<IKS4EstablishmentSubjectEntriesService>();
        _mockEstablishmentService = UseMock<IEstablishmentService>();
        _establishment = new EstablishmentMinimumTestBuilder()
            .WithURN(_urn)
            .WithEstablishmentName(_establishmentName)
            .WithIsKeyStage4(true)
            .BuildServiceModel();

        _mockEstablishmentService
            .Setup(a => a.GetEstablishmentMinimumAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_establishment);
    }

    [Fact]
    public async Task AcademicPerformanceSubjectsEntered_HasCorrectTableCaptions()
    {
        // Arrange
        var returnValue = (new List<SubjectsEnteredModel>(), new List<SubjectsEnteredModel>(), new List<SubjectsEnteredModel>());

        _mockEstablishmentSubjectEntriesService
            .Setup(service => service.GetSubjectEntriesByUrnAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _establishmentName, _pageRoute));

        // Assert
        var captions = doc.QuerySelectorAll("caption");
        Assert.Equal("GCSE subjects entered", captions[0].TextContent.Trim());
        Assert.Equal("Technical Award subjects entered", captions[1].TextContent.Trim());
        Assert.Equal("Other subjects entered", captions[2].TextContent.Trim());
    }

    [Fact]
    public async Task AcademicPerformanceSubjectsEntered_DisplaysBottomPagination_WithCorrectDestinations()
    {
        // Arrange
        var returnValue = (new List<SubjectsEnteredModel>(), new List<SubjectsEnteredModel>(), new List<SubjectsEnteredModel>());

        _mockEstablishmentSubjectEntriesService
            .Setup(service => service.GetSubjectEntriesByUrnAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _establishmentName, _pageRoute));

        // Assert
        var pagination = doc.QuerySelector("nav.govuk-pagination");
        Assert.NotNull(pagination);

        var previousLink = pagination.QuerySelector(".govuk-pagination__prev a");
        var nextLink = pagination.QuerySelector(".govuk-pagination__next a");

        Assert.NotNull(previousLink);
        Assert.Contains("/secondary-performance/english-and-maths", previousLink.GetAttribute("href"));

        Assert.NotNull(nextLink);
        Assert.Contains("/secondary-performance/additional-measures", nextLink.GetAttribute("href"));
    }
}
