using Moq;
using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Core.Interfaces.Repositories.Performance;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.Services.Performance;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Tests.Services.Performance;

public class Level2QualificationsServiceTests
{
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;
    private readonly Mock<IKs5PerformanceRepository> _mockKs5PerformanceRepository;
    private readonly Level2QualificationsService _service;

    private readonly EstablishmentServiceModel fakeEstablishment = new()
    {
        URN = "123456",
        EstablishmentName = "Test Establishment",
        PhaseOfEducationName = "Secondary School",
        LAName = "Council",
        LAId = "E09000001"
    };

    public Level2QualificationsServiceTests()
    {
        _mockEstablishmentService = new();
        _mockKs5PerformanceRepository = new();

        _service = new Level2QualificationsService(
            _mockEstablishmentService.Object,
            _mockKs5PerformanceRepository.Object);
    }

    [Theory]
    [InlineData(Level2.TechCert)]
    public async Task GetLevel2QualificationDetailsAsync_ShouldReturnEmptyModel_WhenNotFound(Level2 qualificationLevel)
    {
        // Arrange
        _mockEstablishmentService
            .Setup(r => r.GetEstablishmentAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeEstablishment);

        _mockKs5PerformanceRepository
            .Setup(r => r.GetEstablishmentPerformanceAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS5EstablishmentPerformance());

        _mockKs5PerformanceRepository
            .Setup(r => r.GetEnglandPerformanceAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS5EnglandPerformance());

        _mockKs5PerformanceRepository
            .Setup(r => r.GetLaPerformanceAsync(fakeEstablishment.LAId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS5LAPerformance());

        // Act
        var result = await _service.GetLevel2QualificationDetailsAsync(fakeEstablishment.URN, qualificationLevel, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fakeEstablishment.URN, result.Urn);
        Assert.Equal(fakeEstablishment.EstablishmentName, result.SchoolName);

        Assert.Null(result.TotalNoOfStudentCompletedQualification.Value);
        Assert.Null(result.ProgressScore.Score.Value);
        Assert.Null(result.ProgressScore.BandingRating.Value);
        Assert.Null(result.ProgressScore.ConfidenceLevelUpper.Value);
        Assert.Null(result.ProgressScore.ConfidenceLevelLower.Value);
        Assert.Null(result.ProgressScore.EnglandAverageScore.Value);

        Assert.Null(result.AverageResult.Establishment.Grade.Value);
        Assert.Null(result.AverageResult.Establishment.Points.Value);
        Assert.Null(result.AverageResult.LocalAuthority.Grade.Value);
        Assert.Null(result.AverageResult.LocalAuthority.Points.Value);
        Assert.Null(result.AverageResult.England.Grade.Value);
        Assert.Null(result.AverageResult.England.Points.Value);
    }

    [Theory]
    [InlineData(Level2.TechCert)]
    public async Task GetLevel2QualificationDetailsAsync_ShouldReturnData(Level2 qualificationLevel)
    {
        // Arrange
        _mockEstablishmentService
            .Setup(r => r.GetEstablishmentAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeEstablishment);

        var establishmentPerformance = new KS5EstablishmentPerformance
        {
            Id = fakeEstablishment.URN,
            TALLPUP_TECHCERT_Est_Current_Num_Coded = new CodedDouble(55, string.Empty, string.Empty),
            VA_INS_TECHCERT_Est_Current_Num_Coded = new CodedDouble(61.55, string.Empty, string.Empty),
            PROGRESS_BAND_TECHCERT_Est_Current = new CodedString("Average", string.Empty, string.Empty),
            UCI_INS_TECHCERT_Est_Current_Num_Coded = new CodedDouble(1, string.Empty, string.Empty),
            LCI_INS_TECHCERT_Est_Current_Num_Coded = new CodedDouble(0.3, string.Empty, string.Empty),
            TALLPPE_TECHCERT_Est_Current_Num_Coded = new CodedDouble(15.23, string.Empty, string.Empty),
            TALLPPEGRD_TECHCERT_Est_Current = new CodedString("A", string.Empty, string.Empty),
        };

        var englandPerformance = new KS5EnglandPerformance
        {
            Id = fakeEstablishment.LAId,
            VA_INS_TECHCERT_Eng_Current_Num_Coded = new CodedDouble(59.56, string.Empty, string.Empty),
            TALLPPE_TECHCERT_Eng_Current_Num_Coded = new CodedDouble(35.11, string.Empty, string.Empty),
            TALLPPEGRD_TECHCERT_Eng_Current = new CodedString("C", string.Empty, string.Empty),
        };

        var laPerformance = new KS5LAPerformance
        {
            TALLPPE_TECHCERT_LA_Current_Num_Coded = new CodedDouble(21.85, string.Empty, string.Empty),
            TALLPPEGRD_TECHCERT_LA_Current = new CodedString("C", string.Empty, string.Empty),
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

        // Act
        var result = await _service.GetLevel2QualificationDetailsAsync(fakeEstablishment.URN, qualificationLevel, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fakeEstablishment.URN, result.Urn);
        Assert.Equal(fakeEstablishment.EstablishmentName, result.SchoolName);

        if (qualificationLevel == Level2.TechCert)
        {
            Assert.Equal(establishmentPerformance.TALLPUP_TECHCERT_Est_Current_Num_Coded, result.TotalNoOfStudentCompletedQualification);
            Assert.Equal(establishmentPerformance.VA_INS_TECHCERT_Est_Current_Num_Coded, result.ProgressScore.Score);
            Assert.Equal(establishmentPerformance.PROGRESS_BAND_TECHCERT_Est_Current, result.ProgressScore.BandingRating);
            Assert.Equal(establishmentPerformance.UCI_INS_TECHCERT_Est_Current_Num_Coded, result.ProgressScore.ConfidenceLevelUpper);
            Assert.Equal(establishmentPerformance.LCI_INS_TECHCERT_Est_Current_Num_Coded, result.ProgressScore.ConfidenceLevelLower);
            Assert.Equal(establishmentPerformance.TALLPPE_TECHCERT_Est_Current_Num_Coded, result.AverageResult.Establishment.Points);
            Assert.Equal(establishmentPerformance.TALLPPEGRD_TECHCERT_Est_Current, result.AverageResult.Establishment.Grade);

            Assert.Equal(englandPerformance.VA_INS_TECHCERT_Eng_Current_Num_Coded, result.ProgressScore.EnglandAverageScore);
            Assert.Equal(englandPerformance.TALLPPE_TECHCERT_Eng_Current_Num_Coded, result.AverageResult.England.Points);
            Assert.Equal(englandPerformance.TALLPPEGRD_TECHCERT_Eng_Current, result.AverageResult.England.Grade);

            Assert.Equal(laPerformance.TALLPPE_TECHCERT_LA_Current_Num_Coded, result.AverageResult.LocalAuthority.Points);
            Assert.Equal(laPerformance.TALLPPEGRD_TECHCERT_LA_Current, result.AverageResult.LocalAuthority.Grade);
        }
    }
}
