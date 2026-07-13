using Moq;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.SubjectEntries;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page;

[Collection("WebAppCollection")]
public class SubjectsEnteredTests : PageTestsBase
{
    private static string _urn = "143034";
    private static string _establishmentName = "Loreto High School Chorlton";
    private static string _pageRoute = "/secondary/academic-performance-subjects-entered";
    private readonly Mock<IEstablishmentSubjectEntriesService> _mockEstablishmentSubjectEntriesService;
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;
    private EstablishmentServiceModel _establishment; 

    public SubjectsEnteredTests(WebAppFixture fixture) : base(fixture)
    {
        _mockEstablishmentSubjectEntriesService = UseMock<IEstablishmentSubjectEntriesService>();
        _mockEstablishmentService = UseMock<IEstablishmentService>();
        _establishment = new EstablishmentTestBuilder()
            .WithURN(_urn)
            .BuildServiceModel();

        _mockEstablishmentService
            .Setup(a => a.GetEstablishmentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_establishment);
    }

    [Fact]
    public async Task AcademicPerformanceSubjectsEntered_HasCorrectTableCaptions()
    {
        // Arrange
        var returnValue = (new List<SubjectsEntered>(), new List<SubjectsEntered>(), new List<SubjectsEntered>());
        
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
}
