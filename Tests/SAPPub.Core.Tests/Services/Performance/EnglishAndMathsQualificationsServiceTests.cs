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
        KS5EstablishmentPerformance establishmentPerformance;
        KS5England5Performance englandPerformance;
        KS5LAPerformance laPerformance;
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

    private void SetupMocks(out KS5EstablishmentPerformance establishmentPerformance, out KS5England5Performance englandPerformance, out KS5LAPerformance laPerformance)
    {
        _mockEstablishmentService
            .Setup(r => r.GetEstablishmentAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeEstablishment);

        establishmentPerformance = new KS5EstablishmentPerformance
        {
            Id = fakeEstablishment.URN,
            T_SCOPEEX_E_Est_Current_Num_Coded = new ValueObjects.CodedDouble { Value = 10.10, Reason = string.Empty, Raw = "10.1011" },
            PROGEX_E_Est_Current_Num_Coded = new ValueObjects.CodedDouble { Value = 20.20, Reason = string.Empty, Raw = "20.20" },
            T_SCOPEEX_M_Est_Current_Num_Coded = new ValueObjects.CodedDouble { Value = 30.30, Reason = string.Empty, Raw = "30.30" },
            PROGEX_M_Est_Current_Num_Coded = new ValueObjects.CodedDouble { Value = 40.40, Reason = string.Empty, Raw = "40.40" },
            ENTRY_PER_E_Est_Current_Pct_Coded = new ValueObjects.CodedDouble { Value = 50.50, Reason = string.Empty, Raw = "50.50" },
            ENTRY_PER_M_Est_Current_Pct_Coded = new ValueObjects.CodedDouble { Value = 60.60, Reason = string.Empty, Raw = "14.60.60" },
        };
        englandPerformance = new KS5England5Performance
        {
            Id = fakeEstablishment.LAId,
            PROGEX_E_Eng_Current_Num_Coded = new ValueObjects.CodedDouble { Value = 11.11, Reason = string.Empty, Raw = "11.11" },
            PROGEX_M_Eng_Current_Num_Coded = new ValueObjects.CodedDouble { Value = 12.12, Reason = string.Empty, Raw = "12.12" },
            ENTRY_PER_E_Eng_Current_Pct_Coded = new ValueObjects.CodedDouble { Value = 13.13, Reason = string.Empty, Raw = "13.13" },
            ENTRY_PER_M_Eng_Current_Pct_Coded = new ValueObjects.CodedDouble { Value = 14.14, Reason = string.Empty, Raw = "14.14" },
        };
        laPerformance = new KS5LAPerformance
        {
            Id = fakeEstablishment.LAId,
            PROGEX_E_LA_Current_Num_Coded = new ValueObjects.CodedDouble { Value = 15.15, Reason = string.Empty, Raw = "15.15" },
            PROGEX_M_LA_Current_Num_Coded = new ValueObjects.CodedDouble { Value = 16.16, Reason = string.Empty, Raw = "16.16" },
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