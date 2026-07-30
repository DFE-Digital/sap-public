using Moq;
using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Core.Interfaces.Repositories.Performance;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.Services.Performance;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Tests.Services.Performance;

public class Level3QualificationsServiceTests
{
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;
    private readonly Mock<IKs5PerformanceRepository> _mockKs5PerformanceRepository;
    private readonly Level3QualificationsService _service;

    private readonly EstablishmentServiceModel fakeEstablishment = new()
    {
        URN = "123456",
        EstablishmentName = "Test Establishment",
        PhaseOfEducationName = "Secondary School",
        LAName = "Council",
        LAId = "E09000001"
    };

    public Level3QualificationsServiceTests()
    {
        _mockEstablishmentService = new();
        _mockKs5PerformanceRepository = new();

        _service = new Level3QualificationsService(
            _mockEstablishmentService.Object,
            _mockKs5PerformanceRepository.Object);
    }

    [Fact]
    public async Task GetLevel3QualificationDetailsAsync_ShouldReturnEmptyModel_WhenNotFound()
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
            .ReturnsAsync(new KS5England5Performance());

        _mockKs5PerformanceRepository
            .Setup(r => r.GetLaPerformanceAsync(fakeEstablishment.LAId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS5LAPerformance());

        // Act
        var result = await _service.GetLevel3QualificationDetailsAsync(fakeEstablishment.URN, Level3.ALevel, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fakeEstablishment.URN, result.Urn);
        Assert.Equal(fakeEstablishment.EstablishmentName, result.SchoolName);

        Assert.Null(result.TotalNoOfStudentCompletedQualification.Value);
        Assert.Null(result.ProgressScore.Score.Value);
        Assert.Null(result.ProgressScore.BandingRating);
        Assert.Null(result.ProgressScore.ConfidenceLevelUpper.Value);
        Assert.Null(result.ProgressScore.ConfidenceLevelLower.Value);
        Assert.Null(result.ProgressScore.EnglandAverageScore.Value);

        Assert.Null(result.AverageResult.Establishment.Grade);
        Assert.Null(result.AverageResult.Establishment.Points.Value);
        Assert.Null(result.AverageResult.LocalAuthority.Grade);
        Assert.Null(result.AverageResult.LocalAuthority.Points.Value);
        Assert.Null(result.AverageResult.England.Grade);
        Assert.Null(result.AverageResult.England.Points.Value);
    }

    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    public async Task GetLevel3QualificationDetailsAsync_ShouldReturnData(Level3 qualificationLevel)
    {
        // Arrange
        _mockEstablishmentService
            .Setup(r => r.GetEstablishmentAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeEstablishment);

        var establishmentPerformance = new KS5EstablishmentPerformance
        {
            Id = fakeEstablishment.URN,
            TALLPUP_ALEV_1618_Est_Current_Num_Coded = new CodedDouble(150, string.Empty, string.Empty),
            VA_INS_ALEV_Est_Current_Num_Coded = new CodedDouble(75.15, string.Empty, string.Empty),
            PROGRESS_BAND_ALEV_Est_Current = "Average",
            UCI_INS_ALEV_Est_Current_Num_Coded = new CodedDouble(5, string.Empty, string.Empty),
            LCI_INS_ALEV_Est_Current_Num_Coded = new CodedDouble(1, string.Empty, string.Empty),
            TALLPPE_ALEV_1618_Est_Current_Num_Coded = new CodedDouble(15.57, string.Empty, string.Empty),
            TALLPPEGRD_ALEV_1618_Est_Current = "A",

            TALLPUP_ACAD_1618_Est_Current_Num_Coded = new CodedDouble(125, string.Empty, string.Empty),
            VA_INS_ACAD_Est_Current_Num_Coded = new CodedDouble(50.55, string.Empty, string.Empty),
            PROGRESS_BAND_ACAD_Est_Current = "Below Average",
            UCI_INS_ACAD_Est_Current_Num_Coded = new CodedDouble(3, string.Empty, string.Empty),
            LCI_INS_ACAD_Est_Current_Num_Coded = new CodedDouble(0.5, string.Empty, string.Empty),
            TALLPPE_ACAD_1618_Est_Current_Num_Coded = new CodedDouble(22.95, string.Empty, string.Empty),
            TALLPPEGRD_ACAD_1618_Est_Current = "B"
        };

        var englandPerformance = new KS5England5Performance
        {
            Id = fakeEstablishment.LAId,
            VA_INS_ALEV_Eng_Current_Num_Coded = new CodedDouble(85.75, string.Empty, string.Empty),
            TALLPPE_ALEV_1618_Eng_Current_Num_Coded = new CodedDouble(25.79, string.Empty, string.Empty),
            TALLPPEGRD_ALEV_1618_Eng_Current = "B",

            VA_INS_ACAD_Eng_Current_Num_Coded = new CodedDouble(67.35, string.Empty, string.Empty),
            TALLPPE_ACAD_1618_Eng_Current_Num_Coded = new CodedDouble(33.15, string.Empty, string.Empty),
            TALLPPEGRD_ACAD_1618_Eng_Current = "C"
        };

        var laPerformance = new KS5LAPerformance
        {
            TALLPPE_ALEV_1618_LA_Current_Num_Coded = new CodedDouble(20.59, string.Empty, string.Empty),
            TALLPPEGRD_ALEV_1618_LA_Current = "C",
            TALLPPE_ACAD_1618_LA_Current_Num_Coded = new CodedDouble(55.23, string.Empty, string.Empty),
            TALLPPEGRD_ACAD_1618_LA_Current = "B"
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
        var result = await _service.GetLevel3QualificationDetailsAsync(fakeEstablishment.URN, qualificationLevel, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fakeEstablishment.URN, result.Urn);
        Assert.Equal(fakeEstablishment.EstablishmentName, result.SchoolName);

        if (qualificationLevel == Level3.ALevel)
        {
            Assert.Equal(establishmentPerformance.TALLPUP_ALEV_1618_Est_Current_Num_Coded, result.TotalNoOfStudentCompletedQualification);
            Assert.Equal(establishmentPerformance.VA_INS_ALEV_Est_Current_Num_Coded, result.ProgressScore.Score);
            Assert.Equal(establishmentPerformance.PROGRESS_BAND_ALEV_Est_Current, result.ProgressScore.BandingRating);
            Assert.Equal(establishmentPerformance.UCI_INS_ALEV_Est_Current_Num_Coded, result.ProgressScore.ConfidenceLevelUpper);
            Assert.Equal(establishmentPerformance.LCI_INS_ALEV_Est_Current_Num_Coded, result.ProgressScore.ConfidenceLevelLower);
            Assert.Equal(establishmentPerformance.TALLPPE_ALEV_1618_Est_Current_Num_Coded, result.AverageResult.Establishment.Points);
            Assert.Equal(establishmentPerformance.TALLPPEGRD_ALEV_1618_Est_Current, result.AverageResult.Establishment.Grade);

            Assert.Equal(englandPerformance.VA_INS_ALEV_Eng_Current_Num_Coded, result.ProgressScore.EnglandAverageScore);
            Assert.Equal(englandPerformance.TALLPPE_ALEV_1618_Eng_Current_Num_Coded, result.AverageResult.England.Points);
            Assert.Equal(englandPerformance.TALLPPEGRD_ALEV_1618_Eng_Current, result.AverageResult.England.Grade);

            Assert.Equal(laPerformance.TALLPPE_ALEV_1618_LA_Current_Num_Coded, result.AverageResult.LocalAuthority.Points);
            Assert.Equal(laPerformance.TALLPPEGRD_ALEV_1618_LA_Current, result.AverageResult.LocalAuthority.Grade);
        }
        else if (qualificationLevel == Level3.Academic)
        {
            Assert.Equal(establishmentPerformance.TALLPUP_ACAD_1618_Est_Current_Num_Coded, result.TotalNoOfStudentCompletedQualification);
            Assert.Equal(establishmentPerformance.VA_INS_ACAD_Est_Current_Num_Coded, result.ProgressScore.Score);
            Assert.Equal(establishmentPerformance.PROGRESS_BAND_ACAD_Est_Current, result.ProgressScore.BandingRating);
            Assert.Equal(establishmentPerformance.UCI_INS_ACAD_Est_Current_Num_Coded, result.ProgressScore.ConfidenceLevelUpper);
            Assert.Equal(establishmentPerformance.LCI_INS_ACAD_Est_Current_Num_Coded, result.ProgressScore.ConfidenceLevelLower);
            Assert.Equal(establishmentPerformance.TALLPPE_ACAD_1618_Est_Current_Num_Coded, result.AverageResult.Establishment.Points);
            Assert.Equal(establishmentPerformance.TALLPPEGRD_ACAD_1618_Est_Current, result.AverageResult.Establishment.Grade);

            Assert.Equal(englandPerformance.VA_INS_ACAD_Eng_Current_Num_Coded, result.ProgressScore.EnglandAverageScore);
            Assert.Equal(englandPerformance.TALLPPE_ACAD_1618_Eng_Current_Num_Coded, result.AverageResult.England.Points);
            Assert.Equal(englandPerformance.TALLPPEGRD_ACAD_1618_Eng_Current, result.AverageResult.England.Grade);

            Assert.Equal(laPerformance.TALLPPE_ACAD_1618_LA_Current_Num_Coded, result.AverageResult.LocalAuthority.Points);
            Assert.Equal(laPerformance.TALLPPEGRD_ACAD_1618_LA_Current, result.AverageResult.LocalAuthority.Grade);
        }
    }
}
