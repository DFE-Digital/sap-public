using Microsoft.Extensions.DependencyInjection;
using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Interfaces.Repositories.Generic;

namespace SAPPub.Web.Page.Tests.Tests;

[Collection("WebAppCollection")]
public class AttainmentAndProgressPageTests : IDisposable
{
    private readonly MockAccessor<IGenericRepository<Establishment>> _establishmentMockAccessor;
    private readonly MockAccessor<IGenericRepository<EstablishmentPerformance>> _establishmentPerformanceMockAccessor;
    private readonly MockAccessor<IGenericRepository<EnglandPerformance>> _englandPerformanceMockAccessor;
    private readonly MockAccessor<IGenericRepository<LAPerformance>> _laPerformanceMockAccessor;
    private readonly WebAppFixture _fixture;

    private string BuildUrl(string urn, string schoolName)
    {
        var encodedSchoolName = Uri.EscapeDataString(schoolName);
        return $"/school/{urn}/{encodedSchoolName}/secondary/academic-performance-attainment-and-progress";
    }

    public AttainmentAndProgressPageTests(WebAppFixture fixture)
    {
        _fixture = fixture;
        _establishmentMockAccessor = fixture.Factory.Services
            .GetRequiredService<MockAccessor<IGenericRepository<Establishment>>>();
        var mockEstablishmentRepo = new Mock<IGenericRepository<Establishment>>();
        _establishmentMockAccessor.Set(mockEstablishmentRepo);

        _establishmentPerformanceMockAccessor = fixture.Factory.Services
            .GetRequiredService<MockAccessor<IGenericRepository<EstablishmentPerformance>>>();
        var mockPerformanceRepo = new Mock<IGenericRepository<EstablishmentPerformance>>();
        _establishmentPerformanceMockAccessor.Set(mockPerformanceRepo);

        _englandPerformanceMockAccessor = fixture.Factory.Services
            .GetRequiredService<MockAccessor<IGenericRepository<EnglandPerformance>>>();
        var mockEnglandPerformanceRepo = new Mock<IGenericRepository<EnglandPerformance>>();
        _englandPerformanceMockAccessor.Set(mockEnglandPerformanceRepo);

        _laPerformanceMockAccessor = fixture.Factory.Services
            .GetRequiredService<MockAccessor<IGenericRepository<LAPerformance>>>();
        var mockLAPerformanceRepo = new Mock<IGenericRepository<LAPerformance>>();
        _laPerformanceMockAccessor.Set(mockLAPerformanceRepo);
    }

    public void Dispose()
    {
        _establishmentMockAccessor.Clear();
        _establishmentPerformanceMockAccessor.Clear();
    }

    [Fact]
    public async Task ShowsAttainmentValues()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "St Paul's Church of England Academy";
        var schoolAttainment = 10.0;

        var establishmentGenericRepositoryMock = _establishmentMockAccessor.Get();
        establishmentGenericRepositoryMock?
            .Setup(repo => repo.ReadAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Establishment()
            {
                URN = urn,
                EstablishmentName = establishmentName
            });
        var establishmentPerformanceGenericRepositoryMock = _establishmentPerformanceMockAccessor.Get();
        establishmentPerformanceGenericRepositoryMock?
            .Setup(repo => repo.ReadAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentPerformance()
            {
                Attainment8_Tot_Est_Current_Num = schoolAttainment
            });
        var laPerformanceGenericRepositoryMock = _laPerformanceMockAccessor.Get();
        laPerformanceGenericRepositoryMock?
            .Setup(repo => repo.ReadAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new LAPerformance()
            {
                Attainment8_Tot_LA_Current_Num = 15
            });
        var englandPerformanceGenericRepositoryMock = _englandPerformanceMockAccessor.Get();
        englandPerformanceGenericRepositoryMock?
            .Setup(repo => repo.ReadAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EnglandPerformance()
            {
                Attainment8_Tot_Eng_Current_Num = 20
            });

        // Act
        var doc = await _fixture.BrowseToPage(BuildUrl(urn, establishmentName));

        // Assert
        var schoolAttainmentCard = doc.QuerySelector("[data-testid='attainment8-establishment-card']");
        var text = schoolAttainmentCard?.QuerySelector("p")?.TextContent.Trim();
        Assert.Contains(schoolAttainment.ToString(), text);
    }
}
