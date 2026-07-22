using Moq;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Enums;
using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Core.Interfaces.Repositories.Performance;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.Services.Performance;

namespace SAPPub.Core.Tests.Services.Performance;

public class AdvancedLevelQualificationsServiceTests
{
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;
    private readonly Mock<IKs5PerformanceRepository> _mockKs5PerformanceRepository;
    private readonly AdvancedLevelQualificationsService _service;

    private readonly EstablishmentServiceModel fakeEstablishment = new()
    {
        URN = "123456",
        EstablishmentName = "Test Establishment",
        PhaseOfEducationName = "Secondary School",
        LAName = "Council",
        LAId = "E09000001"
    };

    public AdvancedLevelQualificationsServiceTests()
    {
        _mockEstablishmentService = new();
        _mockKs5PerformanceRepository = new();

        _service = new AdvancedLevelQualificationsService(
            _mockEstablishmentService.Object,
            _mockKs5PerformanceRepository.Object);
    }

    [Fact]
    public async Task GetAdvancedLevelQualificationDetailsAsync_ShouldReturnEmptyModel_WhenNotFound()
    {
        // Arrange
        _mockEstablishmentService
            .Setup(r => r.GetEstablishmentAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeEstablishment);

        _mockKs5PerformanceRepository
            .Setup(r => r.GetEstablishmentPerformanceAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentKs5Performance());

        _mockKs5PerformanceRepository
            .Setup(r => r.GetEnglandPerformanceAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EnglandKs5Performance());

        // Act
        var result = await _service.GetAdvancedLevelQualificationDetailsAsync(fakeEstablishment.URN, Level3.ALevel, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fakeEstablishment.URN, result.Urn);
        Assert.Equal(fakeEstablishment.EstablishmentName, result.SchoolName);

        Assert.Null(result.TotalNoOfStudentCompletedQualification);
        Assert.Null(result.ProgressScore.Score);
        Assert.Null(result.ProgressScore.BandingRating);
        Assert.Null(result.ProgressScore.ConfidenceLevelUpper);
        Assert.Null(result.ProgressScore.ConfidenceLevelLower);
        Assert.Null(result.ProgressScore.EnglandAverageScore);
    }

    [Theory]
    [InlineData(Level3.ALevel)]
    public async Task GetAdvancedLevelQualificationDetailsAsync_ShouldReturnData(Level3 qualificationLevel)
    {
        // Arrange
        _mockEstablishmentService
            .Setup(r => r.GetEstablishmentAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeEstablishment);

        var establishmentPerformance = new EstablishmentKs5Performance
        {
            Id = fakeEstablishment.URN,
            TALLPUP_ACAD_1618_Est_Current_Num = 150,
            VA_INS_ALEV_Est_Current_Num = 75.15,
            PROGRESS_BAND_ALEV_Est_Current = "Average",
            UCI_INS_ALEV_Est_Current_Num = 5,
            LCI_INS_ALEV_Est_Current_Num = 1            
        };

        var englandPerformance = new EnglandKs5Performance
        {
            Id = fakeEstablishment.LAId,
            VA_INS_ALEV_Eng_Current_Num = 85.75,
        };

        _mockKs5PerformanceRepository
            .Setup(r => r.GetEstablishmentPerformanceAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(establishmentPerformance);

        _mockKs5PerformanceRepository
            .Setup(r => r.GetEnglandPerformanceAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(englandPerformance);

        // Act
        var result = await _service.GetAdvancedLevelQualificationDetailsAsync(fakeEstablishment.URN, qualificationLevel, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fakeEstablishment.URN, result.Urn);
        Assert.Equal(fakeEstablishment.EstablishmentName, result.SchoolName);

        if (qualificationLevel == Level3.ALevel)
        {
            Assert.Equal(establishmentPerformance.TALLPUP_ACAD_1618_Est_Current_Num, result.TotalNoOfStudentCompletedQualification);
            Assert.Equal(establishmentPerformance.VA_INS_ALEV_Est_Current_Num, result.ProgressScore.Score);
            Assert.Equal(establishmentPerformance.PROGRESS_BAND_ALEV_Est_Current, result.ProgressScore.BandingRating);
            Assert.Equal(establishmentPerformance.UCI_INS_ALEV_Est_Current_Num, result.ProgressScore.ConfidenceLevelUpper);
            Assert.Equal(establishmentPerformance.LCI_INS_ALEV_Est_Current_Num, result.ProgressScore.ConfidenceLevelLower);

            Assert.Equal(englandPerformance.VA_INS_ALEV_Eng_Current_Num, result.ProgressScore.EnglandAverageScore);
        }
    }
}
