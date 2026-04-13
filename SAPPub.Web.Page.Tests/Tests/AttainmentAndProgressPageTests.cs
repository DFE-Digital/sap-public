using Microsoft.Extensions.DependencyInjection;
using Moq;
using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.ServiceModels.KS4.Performance;

namespace SAPPub.Web.Page.Tests.Tests;

[Collection("WebAppCollection")]
public class AttainmentAndProgressPageTests : IDisposable
{
    private readonly MockAccessor<IAttainmentAndProgressService> _serviceMockAccessor;
    private readonly WebAppFixture _fixture;

    private string BuildUrl(string urn, string schoolName)
    {
        var encodedSchoolName = Uri.EscapeDataString(schoolName);
        return $"/school/{urn}/{encodedSchoolName}/secondary/academic-performance-attainment-and-progress";
    }

    public AttainmentAndProgressPageTests(WebAppFixture fixture)
    {
        _fixture = fixture;
        _serviceMockAccessor = fixture.Factory.Services
            .GetRequiredService<MockAccessor<IAttainmentAndProgressService>>();
        var mockService = new Mock<IAttainmentAndProgressService>();
        _serviceMockAccessor.Set(mockService);
    }

    public void Dispose()
    {
        _serviceMockAccessor.Clear();
    }

    [Fact]
    public async Task ShowsAttainmentValues()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "St Paul's Church of England Academy";
        var schoolAttainment = 10.0;

        var serviceMock = _serviceMockAccessor.Get();
        serviceMock?
            .Setup(service => service.GetAttainmentAndProgressAsync(
                It.IsAny<string>(),
                It.IsAny<AcademicYearSelection>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AttainmentAndProgressModel()
            {
                Urn = urn,
                SchoolName = establishmentName,
                EstablishmentAttainment8Score = schoolAttainment,
                LocalAuthorityAttainment8Score = 15,
                EnglandAttainment8Score = 20
            });

        // Act
        var doc = await _fixture.BrowseToPage(BuildUrl(urn, establishmentName));

        // Assert
        var schoolAttainmentCard = doc.QuerySelector("[data-testid='attainment8-establishment-card']");
        var text = schoolAttainmentCard?.QuerySelector("p")?.TextContent.Trim();
        Assert.Contains(schoolAttainment.ToString(), text);
    }
}
