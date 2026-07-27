using Moq;
using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Interfaces.Repositories.Performance;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.Services.Performance;

namespace SAPPub.Core.Tests.Services.Performance;

public class EnglishAndMathsQualificationsServiceTests
{
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;
    private readonly Mock<IKs5PerformanceRepository> _mockKs5PerformanceRepository;
    private readonly EnglishAndMathsQualificationsService _service;

    private readonly EstablishmentServiceModel fakeEstablishment = new()
    {
        URN = "123456",
        EstablishmentName = "Test Establishment",
        PhaseOfEducationName = "Secondary School",
        LAName = "Council",
        LAId = "E09000001"
    };

    public EnglishAndMathsQualificationsServiceTests()
    {
        _mockEstablishmentService = new();
        _mockKs5PerformanceRepository = new();

        _service = new EnglishAndMathsQualificationsService(
            _mockEstablishmentService.Object,
            _mockKs5PerformanceRepository.Object);
    }

    [Fact]
    public async Task GetEnglishAndMathsQualificationDetailsAsync_ShouldReturnData()
    {
        // Arrange
        EstablishmentKs5Performance establishmentPerformance;
        EnglandKs5Performance englandPerformance;
        LAKs5Performance laPerformance;
        SetupMocks(out establishmentPerformance, out englandPerformance, out laPerformance);

        // Act
        var result = await _service.GetEnglishAndMathsQualificationDetailsAsync(fakeEstablishment.URN, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fakeEstablishment.URN, result.Urn);
        Assert.Equal(fakeEstablishment.EstablishmentName, result.SchoolName);
        Assert.Equal(fakeEstablishment.IsKS2, result.IsKS2);
        Assert.Equal(fakeEstablishment.IsKS4, result.IsKS4);
        Assert.Equal(fakeEstablishment.LAName, result.LAName);
        Assert.Equal(establishmentPerformance.T_SCOPEEX_E_Est_Current_Num_Coded, result.AverageEnglishProgress!.NumberOfStudents);
        Assert.Equal(establishmentPerformance.PROGEX_E_Est_Current_Num_Coded, result.AverageEnglishProgress!.SchoolOrCollege);
        Assert.Equal(laPerformance.PROGEX_E_LA_Current_Num_Coded, result.AverageEnglishProgress!.LaAverage);
        Assert.Equal(englandPerformance.PROGEX_E_Eng_Current_Num_Coded, result.AverageEnglishProgress!.EnglandAverage);
        Assert.Equal(establishmentPerformance.T_SCOPEEX_M_Est_Current_Num_Coded, result.AverageMathsProgress!.NumberOfStudents);
        Assert.Equal(establishmentPerformance.PROGEX_M_Est_Current_Num_Coded, result.AverageMathsProgress!.SchoolOrCollege);
        Assert.Equal(laPerformance.PROGEX_M_LA_Current_Num_Coded, result.AverageMathsProgress!.LaAverage);
        Assert.Equal(englandPerformance.PROGEX_M_Eng_Current_Num_Coded, result.AverageMathsProgress!.EnglandAverage);
        Assert.Equal(establishmentPerformance.ENTRY_PER_E_Est_Current_Pct_Coded, result.EnteredForEnglishQualification!.SchoolOrCollege);
        Assert.Equal(laPerformance.ENTRY_PER_E_LA_Current_Pct_Coded, result.EnteredForEnglishQualification!.LaAverage);
        Assert.Equal(englandPerformance.ENTRY_PER_E_Eng_Current_Pct_Coded, result.EnteredForEnglishQualification!.EnglandAverage);
        Assert.Equal(establishmentPerformance.ENTRY_PER_M_Est_Current_Pct_Coded, result.EnteredForMathsQualification!.SchoolOrCollege);
        Assert.Equal(laPerformance.ENTRY_PER_M_LA_Current_Pct_Coded, result.EnteredForMathsQualification!.LaAverage);
        Assert.Equal(englandPerformance.ENTRY_PER_M_Eng_Current_Pct_Coded, result.EnteredForMathsQualification!.EnglandAverage);
    }


    [Fact]
    public async Task GetEnglishAndMathsQualificationDetailsAsync_ThrowsWhenCancellationRequested() //GetSubjectEntriesByUrnAsync_throws_when_cancellation_is_already_requested()
    {
        // Arrange
        SetupMocks(out _, out _, out _);
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        var urn = "123456";

        // Act
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => _service.GetEnglishAndMathsQualificationDetailsAsync(urn, cts.Token));

        // Assert
        _mockKs5PerformanceRepository.Verify(r => r.GetEstablishmentPerformanceAsync(urn, It.IsAny<CancellationToken>()), Times.Never);
        _mockKs5PerformanceRepository.Verify(r => r.GetEnglandPerformanceAsync(It.IsAny<CancellationToken>()), Times.Never);
        _mockKs5PerformanceRepository.Verify(r => r.GetLaPerformanceAsync(urn, It.IsAny<CancellationToken>()), Times.Never);
    }

    private void SetupMocks(out EstablishmentKs5Performance establishmentPerformance, out EnglandKs5Performance englandPerformance, out LAKs5Performance laPerformance)
    {
        _mockEstablishmentService
            .Setup(r => r.GetEstablishmentAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeEstablishment);

        establishmentPerformance = new EstablishmentKs5Performance
        {
            Id = fakeEstablishment.URN,
            T_SCOPEEX_E_Est_Current_Num = 10.10,
            PROGEX_E_Est_Current_Num = 20.20,
            T_SCOPEEX_M_Est_Current_Num = 30.30,
            PROGEX_M_Est_Current_Num = 40.40,
            ENTRY_PER_E_Est_Current_Pct = 50.50,
            ENTRY_PER_M_Est_Current_Pct = 60.60
        };
        englandPerformance = new EnglandKs5Performance
        {
            Id = fakeEstablishment.LAId,
            PROGEX_E_Eng_Current_Num = 11.11,
            PROGEX_M_Eng_Current_Num = 12.12,
            ENTRY_PER_E_Eng_Current_Pct = 13.13,
            ENTRY_PER_M_Eng_Current_Pct = 14.14
        };
        laPerformance = new LAKs5Performance
        {
            Id = fakeEstablishment.LAId,
            PROGEX_E_LA_Current_Num = 15.15,
            PROGEX_M_LA_Current_Num = 16.16
        };
        _mockKs5PerformanceRepository
            .Setup(r => r.GetEstablishmentPerformanceAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(establishmentPerformance);

        _mockKs5PerformanceRepository
            .Setup(r => r.GetEnglandPerformanceAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(englandPerformance);

        _mockKs5PerformanceRepository
            .Setup(r => r.GetLaPerformanceAsync(fakeEstablishment.LAId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(laPerformance);
    }

}