using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.Services.KS4.Performance;

namespace SAPPub.Core.Tests.Services.KS4.Performance;

public class EnglishAndMathsComparisionServiceTests
{
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;
    private readonly Mock<IEstablishmentPerformanceService> _mockEstablishmentPerformanceService;
    private readonly Mock<IEnglandPerformanceService> _mockEnglandPerformanceService;
    private readonly EnglishAndMathsComparisionService _service;

    private readonly List<Establishment> fakeEstablishments = 
        [
            new()
            {
                URN = "123456",
                EstablishmentName = "Test Establishment 1",
                PhaseOfEducationName = "Secondary School",
                LAName = "Council",
                LAId = "E09000001"
            },
            new()
            {
                URN = "785694",
                EstablishmentName = "New Test Establishment 2",
                PhaseOfEducationName = "Secondary School",
                LAName = "New Council",
                LAId = "E12345001"
            },
            new()
            {
                URN = "587946",
                EstablishmentName = "New Test Establishment 3",
                PhaseOfEducationName = "Secondary School",
                LAName = "New Council",
                LAId = "E12345001"
            },
        ];

    public EnglishAndMathsComparisionServiceTests()
    {
        _mockEstablishmentService = new();
        _mockEstablishmentPerformanceService = new();
        _mockEnglandPerformanceService = new();

        _service = new EnglishAndMathsComparisionService(
            _mockEstablishmentService.Object,
            _mockEstablishmentPerformanceService.Object,
            _mockEnglandPerformanceService.Object);
    }

    [Fact]
    public async Task GetComparisionResultsAsync_ShouldReturnData()
    {
        // Arrange
        var urns = new List<string> { "123456", "785694", "587946" };

        var establishmentsPerformance = new List<EstablishmentPerformance>()
        {   
            new()
            {
                Id = fakeEstablishments[0].URN,
                EngMaths59_Tot_Est_Current_Pct = 90                
            },
            new()
            {
                Id = fakeEstablishments[1].URN,
                EngMaths59_Tot_Est_Current_Pct = 70
            },
        };

        var englandPerformance = new EnglandPerformance
        {
            EngMaths59_Tot_Eng_Current_Pct = 80,
            EngMaths59_Tot_Eng_Previous_Pct = 75.5,
            EngMaths59_Tot_Eng_Previous2_Pct = 70.9
        };

        _mockEstablishmentService
            .Setup(r => r.GetEstablishmentsAsync(urns, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeEstablishments);

        _mockEstablishmentPerformanceService
            .Setup(r => r.GetEstablishmentsPerformanceAsync(urns, It.IsAny<CancellationToken>()))
            .ReturnsAsync(establishmentsPerformance);

        _mockEnglandPerformanceService
            .Setup(r => r.GetEnglandPerformanceAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(englandPerformance);

        // Act
        var result = await _service.GetComparisionResultsAsync(urns, CancellationToken.None);

        // Assert (common)
        Assert.NotNull(result);

        foreach(var urn in urns)
        {
            var expectedEstablishment = fakeEstablishments.FirstOrDefault(x => x.URN == urn);
            Assert.NotNull(expectedEstablishment);

            Assert.Equal(englandPerformance.EngMaths59_Tot_Eng_Current_Pct, result.EnglandAverage.CurrentYear);
            Assert.Equal(englandPerformance.EngMaths59_Tot_Eng_Previous_Pct, result.EnglandAverage.PreviousYear);
            Assert.Equal(englandPerformance.EngMaths59_Tot_Eng_Previous2_Pct, result.EnglandAverage.TwoYearsAgo);

            var expectedEstablishmentComparisionResult = establishmentsPerformance.FirstOrDefault(x => x.Id == expectedEstablishment.URN);
            var actualEstablishmentComparisionResult = result.Establishments.FirstOrDefault(x => x.Urn == expectedEstablishment.URN);
            Assert.NotNull(actualEstablishmentComparisionResult);

            Assert.Equal(expectedEstablishment.EstablishmentName, actualEstablishmentComparisionResult.SchoolName);
            Assert.Equal(expectedEstablishmentComparisionResult?.EngMaths59_Tot_Est_Current_Pct, actualEstablishmentComparisionResult.EstablishmentData.CurrentYear);
        }       
    }
}
