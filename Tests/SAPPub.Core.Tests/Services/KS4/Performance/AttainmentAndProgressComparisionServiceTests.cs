using Moq;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.Services.KS4.Performance;

namespace SAPPub.Core.Tests.Services.KS4.Performance;

public class AttainmentAndProgressComparisionServiceTests
{
    private readonly Mock<IEstablishmentPerformanceService> _mockEstablishmentPerformanceService;
    private readonly Mock<IEnglandPerformanceService> _mockEnglandPerformanceService;
    private readonly AttainmentAndProgressComparisionService _service;

    public AttainmentAndProgressComparisionServiceTests()
    {
        _mockEstablishmentPerformanceService = new();
        _mockEnglandPerformanceService = new();

        _service = new AttainmentAndProgressComparisionService(
            _mockEstablishmentPerformanceService.Object,
            _mockEnglandPerformanceService.Object);
    }

    [Fact]
    public async Task GetComparisionResultsAsync_ShouldReturnData()
    {
        // Arrange

        var urns = new List<string> { "123456", "785694"};

        var establishmentsPerformance = new List<EstablishmentPerformance>()
        {
            new()
            {
                Id = urns[0],
                Attainment8_Tot_Est_Current_Num = 85
            },
            new()
            {
                Id = urns[1],
                Attainment8_Tot_Est_Current_Num = 75
            },
        };



        var englandPerformance = new EnglandPerformance
        {
            Id = "E09000001",
            Attainment8_Tot_Eng_Current_Num = 80
        };

        _mockEstablishmentPerformanceService
            .Setup(r => r.GetEstablishmentsPerformanceAsync(urns, It.IsAny<CancellationToken>()))
            .ReturnsAsync(establishmentsPerformance);

        _mockEnglandPerformanceService
            .Setup(r => r.GetEnglandPerformanceAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(englandPerformance);

        // Act
        var result = await _service.GetComparisionResultsAsync(urns, CancellationToken.None);

        // Assert
        Assert.NotNull(result);

        foreach (var urn in urns)
        {
            Assert.Equal(englandPerformance.Attainment8_Tot_Eng_Current_Num, result.EnglandAverage);

            var expectedEstablishmentComparisionResult = establishmentsPerformance.FirstOrDefault(x => x.Id == urn);
            var actualEstablishmentComparisionResult = result.SchoolDetails.FirstOrDefault(x => x.Urn == urn);
            Assert.NotNull(actualEstablishmentComparisionResult);

            Assert.Equal(expectedEstablishmentComparisionResult?.Attainment8_Tot_Est_Current_Num, actualEstablishmentComparisionResult.Attainment8Score);
        }
    }
}
