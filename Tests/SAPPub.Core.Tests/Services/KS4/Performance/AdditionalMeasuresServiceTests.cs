using Moq;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.Services.KS4.Performance;

namespace SAPPub.Core.Tests.Services.KS4.Performance;

public class AdditionalMeasuresServiceTests
{
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;
    private readonly Mock<IEstablishmentPerformanceService> _mockEstablishmentPerformanceService;
    private readonly Mock<ILAPerformanceService> _mockLAPerformanceService;
    private readonly Mock<IEnglandPerformanceService> _mockEnglandPerformanceService;
    private readonly AdditionalMeasuresService _service;

    private readonly EstablishmentServiceModel fakeEstablishment = new()
    {
        URN = "123456",
        EstablishmentName = "Test Establishment",
        PhaseOfEducationName = "Secondary School",
        LAName = "Council",
        LAId = "E09000001"
    };

    public AdditionalMeasuresServiceTests()
    {
        _mockEstablishmentService = new();
        _mockEstablishmentPerformanceService = new();
        _mockLAPerformanceService = new();
        _mockEnglandPerformanceService = new();

        _service = new AdditionalMeasuresService(
            _mockEstablishmentService.Object,
            _mockEstablishmentPerformanceService.Object,
            _mockLAPerformanceService.Object,
            _mockEnglandPerformanceService.Object);
    }

    [Fact]
    public async Task DoesSomething()
    {
        // Act
        var result = _service.GetAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>());

        // Assert
        Assert.NotNull(result);
    }
}
