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

    private readonly EstablishmentMinimumServiceModel fakeEstablishment = new()
    {
        URN = "123456",
        EstablishmentName = "Test Establishment",
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

    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    [InlineData(Level3.AppliedGeneral)]
    [InlineData(Level3.TechLevel)]
    public async Task GetLevel3QualificationDetailsAsync_ShouldReturnEmptyModel_WhenNotFound(Level3 qualificationLevel)
    {
        // Arrange
        _mockEstablishmentService
            .Setup(r => r.GetEstablishmentMinimumAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
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
        var result = await _service.GetLevel3QualificationDetailsAsync(fakeEstablishment.URN, qualificationLevel, CancellationToken.None);

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

        Assert.Null(result.AdditionalData?.TotalNoOfStudentsIncludedInThisMeasure.Value);
        Assert.Null(result.AdditionalData?.Establishment.Grade.Value);
        Assert.Null(result.AdditionalData?.Establishment.Points.Value);
        Assert.Null(result.AdditionalData?.LocalAuthority.Grade.Value);
        Assert.Null(result.AdditionalData?.LocalAuthority.Points.Value);
        Assert.Null(result.AdditionalData?.England.Grade.Value);
        Assert.Null(result.AdditionalData?.England.Points.Value);

        Assert.Null(result.AdvancedLevelMathsQualificationData?.SchoolOrCollege.Value);
        Assert.Null(result.AdvancedLevelMathsQualificationData?.LocalAuthority.Value);
        Assert.Null(result.AdvancedLevelMathsQualificationData?.England.Value);

        Assert.Null(result.DisadvantagedStudentsData.Establishment!.NumberOfStudents.Value);
        Assert.Null(result.DisadvantagedStudentsData.Establishment!.ProgressScore.Value);
        Assert.Null(result.DisadvantagedStudentsData.Establishment!.ConfidenceLevelUpper.Value);
        Assert.Null(result.DisadvantagedStudentsData.Establishment!.ConfidenceLevelLower.Value);
        Assert.Null(result.DisadvantagedStudentsData.Establishment!.Result.Grade.Value);
        Assert.Null(result.DisadvantagedStudentsData.Establishment!.Result.Points.Value);

        Assert.Null(result.DisadvantagedStudentsData.LocalAuthority.NumberOfStudents.Value);
        Assert.Null(result.DisadvantagedStudentsData.LocalAuthority.ProgressScore.Value);
        Assert.Null(result.DisadvantagedStudentsData.LocalAuthority.ConfidenceLevelUpper.Value);
        Assert.Null(result.DisadvantagedStudentsData.LocalAuthority.ConfidenceLevelLower.Value);
        Assert.Null(result.DisadvantagedStudentsData.LocalAuthority.Result.Grade.Value);
        Assert.Null(result.DisadvantagedStudentsData.LocalAuthority.Result.Points.Value);

        Assert.Null(result.DisadvantagedStudentsData.England.NumberOfStudents.Value);
        Assert.Null(result.DisadvantagedStudentsData.England.ProgressScore.Value);
        Assert.Null(result.DisadvantagedStudentsData.England.ConfidenceLevelUpper.Value);
        Assert.Null(result.DisadvantagedStudentsData.England.ConfidenceLevelLower.Value);
        Assert.Null(result.DisadvantagedStudentsData.England.Result.Grade.Value);
        Assert.Null(result.DisadvantagedStudentsData.England.Result.Points.Value);

        Assert.Null(result.NonDisadvantagedStudentsData.Establishment);

        Assert.Null(result.NonDisadvantagedStudentsData.LocalAuthority.NumberOfStudents.Value);
        Assert.Null(result.NonDisadvantagedStudentsData.LocalAuthority.ProgressScore.Value);
        Assert.Null(result.NonDisadvantagedStudentsData.LocalAuthority.ConfidenceLevelUpper.Value);
        Assert.Null(result.NonDisadvantagedStudentsData.LocalAuthority.ConfidenceLevelLower.Value);
        Assert.Null(result.NonDisadvantagedStudentsData.LocalAuthority.Result.Grade.Value);
        Assert.Null(result.NonDisadvantagedStudentsData.LocalAuthority.Result.Points.Value);

        Assert.Null(result.NonDisadvantagedStudentsData.England.NumberOfStudents.Value);
        Assert.Null(result.NonDisadvantagedStudentsData.England.ProgressScore.Value);
        Assert.Null(result.NonDisadvantagedStudentsData.England.ConfidenceLevelUpper.Value);
        Assert.Null(result.NonDisadvantagedStudentsData.England.ConfidenceLevelLower.Value);
        Assert.Null(result.NonDisadvantagedStudentsData.England.Result.Grade.Value);
        Assert.Null(result.NonDisadvantagedStudentsData.England.Result.Points.Value);
    }

    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    [InlineData(Level3.AppliedGeneral)]
    [InlineData(Level3.TechLevel)]
    public async Task GetLevel3QualificationDetailsAsync_ShouldReturnData(Level3 qualificationLevel)
    {
        // Arrange
        var isAlevelQual = qualificationLevel == Level3.ALevel;
        var isAcademicQual = qualificationLevel == Level3.Academic;

        _mockEstablishmentService
            .Setup(r => r.GetEstablishmentMinimumAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeEstablishment);

        var establishmentPerformance = new KS5EstablishmentPerformance
        {
            Id = fakeEstablishment.URN,
            TALLPUP_ALEV_1618_Est_Current_Num_Coded = new CodedDouble(150, string.Empty, string.Empty),
            VA_INS_ALEV_Est_Current_Num_Coded = new CodedDouble(75.15, string.Empty, string.Empty),
            PROGRESS_BAND_ALEV_Est_Current = new CodedString("Average", string.Empty, string.Empty),
            UCI_INS_ALEV_Est_Current_Num_Coded = new CodedDouble(5, string.Empty, string.Empty),
            LCI_INS_ALEV_Est_Current_Num_Coded = new CodedDouble(1, string.Empty, string.Empty),
            TALLPPE_ALEV_1618_Est_Current_Num_Coded = new CodedDouble(15.57, string.Empty, string.Empty),
            TALLPPEGRD_ALEV_1618_Est_Current = new CodedString("A", string.Empty, string.Empty),

            TALLPUP_ACAD_1618_Est_Current_Num_Coded = new CodedDouble(125, string.Empty, string.Empty),
            VA_INS_ACAD_Est_Current_Num_Coded = new CodedDouble(50.55, string.Empty, string.Empty),
            PROGRESS_BAND_ACAD_Est_Current = new CodedString("Below Average", string.Empty, string.Empty),
            UCI_INS_ACAD_Est_Current_Num_Coded = new CodedDouble(3, string.Empty, string.Empty),
            LCI_INS_ACAD_Est_Current_Num_Coded = new CodedDouble(0.5, string.Empty, string.Empty),
            TALLPPE_ACAD_1618_Est_Current_Num_Coded = new CodedDouble(22.95, string.Empty, string.Empty),
            TALLPPEGRD_ACAD_1618_Est_Current = new CodedString("B", string.Empty, string.Empty),

            TALLPUP_AGEN_Est_Current_Num_Coded = new CodedDouble(45, string.Empty, string.Empty),
            VA_INS_AGEN_Est_Current_Num_Coded = new CodedDouble(66.29, string.Empty, string.Empty),
            PROGRESS_BAND_AGEN_Est_Current = new CodedString("Average", string.Empty, string.Empty),
            UCI_INS_AGEN_Est_Current_Num_Coded = new CodedDouble(3, string.Empty, string.Empty),
            LCI_INS_AGEN_Est_Current_Num_Coded = new CodedDouble(0.2, string.Empty, string.Empty),
            TALLPPE_AGEN_Est_Current_Num_Coded = new CodedDouble(22.77, string.Empty, string.Empty),
            TALLPPEGRD_AGEN_Est_Current = new CodedString("A", string.Empty, string.Empty),

            TALLPUP_TLEV_Est_Current_Num_Coded = new CodedDouble(55, string.Empty, string.Empty),
            VA_INS_TLEV_Est_Current_Num_Coded = new CodedDouble(61.55, string.Empty, string.Empty),
            PROGRESS_BAND_TLEV_Est_Current = new CodedString("Average", string.Empty, string.Empty),
            UCI_INS_TLEV_Est_Current_Num_Coded = new CodedDouble(1, string.Empty, string.Empty),
            LCI_INS_TLEV_Est_Current_Num_Coded = new CodedDouble(0.3, string.Empty, string.Empty),
            TALLPPE_TLEV_Est_Current_Num_Coded = new CodedDouble(15.23, string.Empty, string.Empty),
            TALLPPEGRD_TLEV_Est_Current = new CodedString("A", string.Empty, string.Empty),

            TINCLUDE_B3_Est_Current_Num_Coded = isAlevelQual ? new CodedDouble(100, string.Empty, string.Empty) : CodedDouble.Empty,
            TB3PTSE_Est_Current_Num_Coded = isAlevelQual ? new CodedDouble(85.27, string.Empty, string.Empty) : CodedDouble.Empty,
            TB3PTSE_GRD_Est_Current = isAlevelQual ? new CodedString("A", string.Empty, string.Empty) : CodedString.Empty,

            L3M_PER_Est_Current_Pct_Coded = isAcademicQual ? new CodedDouble(79.74, string.Empty, string.Empty) : CodedDouble.Empty,

            TALLPUP_ALEV_1618_DIS_Est_Current_Num_Coded = new CodedDouble(130, string.Empty, string.Empty),
            VA_INS_ALEV_DIS_Est_Current_Num_Coded = new CodedDouble(79.26, string.Empty, string.Empty),
            UCI_INS_ALEV_DIS_Est_Current_Num_Coded = new CodedDouble(3, string.Empty, string.Empty),
            LCI_INS_ALEV_DIS_Est_Current_Num_Coded = new CodedDouble(2, string.Empty, string.Empty),
            TALLPPE_ALEV_1618_DIS_Est_Current_Num_Coded = new CodedDouble(20.13, string.Empty, string.Empty),
            TALLPPEGRD_ALEV_DIS_Est_Current = new CodedString("A", string.Empty, string.Empty),
        };

        var englandPerformance = new KS5EnglandPerformance
        {
            Id = fakeEstablishment.LAId,
            VA_INS_ALEV_Eng_Current_Num_Coded = new CodedDouble(85.75, string.Empty, string.Empty),
            TALLPPE_ALEV_1618_Eng_Current_Num_Coded = new CodedDouble(25.79, string.Empty, string.Empty),
            TALLPPEGRD_ALEV_1618_Eng_Current = new CodedString("B", string.Empty, string.Empty),

            VA_INS_ACAD_Eng_Current_Num_Coded = new CodedDouble(67.35, string.Empty, string.Empty),
            TALLPPE_ACAD_1618_Eng_Current_Num_Coded = new CodedDouble(33.15, string.Empty, string.Empty),
            TALLPPEGRD_ACAD_1618_Eng_Current = new CodedString("C", string.Empty, string.Empty),

            VA_INS_AGEN_Eng_Current_Num_Coded = new CodedDouble(77.66, string.Empty, string.Empty),
            TALLPPE_AGEN_Eng_Current_Num_Coded = new CodedDouble(33.24, string.Empty, string.Empty),
            TALLPPEGRD_AGEN_Eng_Current = new CodedString("B", string.Empty, string.Empty),

            VA_INS_TLEV_Eng_Current_Num_Coded = new CodedDouble(59.56, string.Empty, string.Empty),
            TALLPPE_TLEV_Eng_Current_Num_Coded = new CodedDouble(35.11, string.Empty, string.Empty),
            TALLPPEGRD_TLEV_Eng_Current = new CodedString("C", string.Empty, string.Empty),

            TB3PTSE_Eng_Current_Num_Coded = isAlevelQual ? new CodedDouble(79.19, string.Empty, string.Empty) : CodedDouble.Empty,
            TB3PTSE_GRD_Eng_Current = isAlevelQual ? new CodedString("A", string.Empty, string.Empty) : CodedString.Empty,

            L3M_PER_Eng_Current_Pct_Coded = isAcademicQual ? new CodedDouble(67.58, string.Empty, string.Empty) : CodedDouble.Empty,

            TALLPUP_ALEV_1618_DIS_Eng_Current_Num_Coded = new CodedDouble(135, string.Empty, string.Empty),
            VA_INS_ALEV_DIS_Eng_Current_Num_Coded = new CodedDouble(67.78, string.Empty, string.Empty),
            UCI_INS_ALEV_DIS_Eng_Current_Num_Coded = new CodedDouble(2, string.Empty, string.Empty),
            LCI_INS_ALEV_DIS_Eng_Current_Num_Coded = new CodedDouble(3, string.Empty, string.Empty),
            TALLPPE_ALEV_1618_DIS_Eng_Current_Num_Coded = new CodedDouble(15.69, string.Empty, string.Empty),
            TALLPPEGRD_ALEV_DIS_Eng_Current = new CodedString("B", string.Empty, string.Empty),

            TALLPUP_ALEV_1618_NOTDIS_Eng_Current_Num_Coded = new CodedDouble(95, string.Empty, string.Empty),
            VA_INS_ALEV_NOTDIS_Eng_Current_Num_Coded = new CodedDouble(71.13, string.Empty, string.Empty),
            UCI_INS_ALEV_NOTDIS_Eng_Current_Num_Coded = new CodedDouble(3, string.Empty, string.Empty),
            LCI_INS_ALEV_NOTDIS_Eng_Current_Num_Coded = new CodedDouble(1, string.Empty, string.Empty),
            TALLPPE_ALEV_1618_NOTDIS_Eng_Current_Num_Coded = new CodedDouble(18.27, string.Empty, string.Empty),
            TALLPPEGRD_ALEV_NOTDIS_Eng_Current = new CodedString("A", string.Empty, string.Empty),
        };

        var laPerformance = new KS5LAPerformance
        {
            TALLPPE_ALEV_1618_LA_Current_Num_Coded = new CodedDouble(20.59, string.Empty, string.Empty),
            TALLPPEGRD_ALEV_1618_LA_Current = new CodedString("C", string.Empty, string.Empty),
            TALLPPE_ACAD_1618_LA_Current_Num_Coded = new CodedDouble(55.23, string.Empty, string.Empty),
            TALLPPEGRD_ACAD_1618_LA_Current = new CodedString("B", string.Empty, string.Empty),
            TALLPPE_AGEN_LA_Current_Num_Coded = new CodedDouble(47.53, string.Empty, string.Empty),
            TALLPPEGRD_AGEN_LA_Current = new CodedString("B", string.Empty, string.Empty),
            TALLPPE_TLEV_LA_Current_Num_Coded = new CodedDouble(21.85, string.Empty, string.Empty),
            TALLPPEGRD_TLEV_LA_Current = new CodedString("C", string.Empty, string.Empty),
            TB3PTSE_LA_Current_Num_Coded = isAlevelQual ? new CodedDouble(25.19, string.Empty, string.Empty) : CodedDouble.Empty,
            TB3PTSE_GRD_LA_Current = isAlevelQual ? new CodedString("C", string.Empty, string.Empty) : CodedString.Empty,
            L3M_PER_LA_Current_Pct_Coded = isAcademicQual ? new CodedDouble(67.58, string.Empty, string.Empty) : CodedDouble.Empty,

            TALLPUP_ALEV_1618_DIS_LA_Current_Num_Coded = new CodedDouble(110, string.Empty, string.Empty),
            VA_INS_ALEV_DIS_LA_Current_Num_Coded = new CodedDouble(55.11, string.Empty, string.Empty),
            UCI_INS_ALEV_DIS_LA_Current_Num_Coded = new CodedDouble(3, string.Empty, string.Empty),
            LCI_INS_ALEV_DIS_LA_Current_Num_Coded = new CodedDouble(2, string.Empty, string.Empty),
            TALLPPE_ALEV_1618_DIS_LA_Current_Num_Coded = new CodedDouble(16.19, string.Empty, string.Empty),
            TALLPPEGRD_ALEV_DIS_LA_Current = new CodedString("B", string.Empty, string.Empty),

            TALLPUP_ALEV_1618_NOTDIS_LA_Current_Num_Coded = new CodedDouble(80, string.Empty, string.Empty),
            VA_INS_ALEV_NOTDIS_LA_Current_Num_Coded = new CodedDouble(70.11, string.Empty, string.Empty),
            UCI_INS_ALEV_NOTDIS_LA_Current_Num_Coded = new CodedDouble(3, string.Empty, string.Empty),
            LCI_INS_ALEV_NOTDIS_LA_Current_Num_Coded = new CodedDouble(1, string.Empty, string.Empty),
            TALLPPE_ALEV_1618_NOTDIS_LA_Current_Num_Coded = new CodedDouble(19.71, string.Empty, string.Empty),
            TALLPPEGRD_ALEV_NOTDIS_LA_Current = new CodedString("B", string.Empty, string.Empty),
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

            // Establishment Additional data
            Assert.Equal(establishmentPerformance.TINCLUDE_B3_Est_Current_Num_Coded, result.AdditionalData!.TotalNoOfStudentsIncludedInThisMeasure);
            Assert.Equal(establishmentPerformance.TB3PTSE_Est_Current_Num_Coded, result.AdditionalData.Establishment.Points);
            Assert.Equal(establishmentPerformance.TB3PTSE_GRD_Est_Current, result.AdditionalData.Establishment.Grade);

            Assert.Equal(englandPerformance.VA_INS_ALEV_Eng_Current_Num_Coded, result.ProgressScore.EnglandAverageScore);
            Assert.Equal(englandPerformance.TALLPPE_ALEV_1618_Eng_Current_Num_Coded, result.AverageResult.England.Points);
            Assert.Equal(englandPerformance.TALLPPEGRD_ALEV_1618_Eng_Current, result.AverageResult.England.Grade);

            // England Additional data
            Assert.Equal(englandPerformance.TB3PTSE_Eng_Current_Num_Coded, result.AdditionalData.England.Points);
            Assert.Equal(englandPerformance.TB3PTSE_GRD_Eng_Current, result.AdditionalData.England.Grade);

            Assert.Equal(laPerformance.TALLPPE_ALEV_1618_LA_Current_Num_Coded, result.AverageResult.LocalAuthority.Points);
            Assert.Equal(laPerformance.TALLPPEGRD_ALEV_1618_LA_Current, result.AverageResult.LocalAuthority.Grade);

            // LA Additional data
            Assert.Equal(laPerformance.TB3PTSE_LA_Current_Num_Coded, result.AdditionalData.LocalAuthority.Points);
            Assert.Equal(laPerformance.TB3PTSE_GRD_LA_Current, result.AdditionalData.LocalAuthority.Grade);

            // AdvancedLevelMathsQualificationData
            Assert.Null(result.AdvancedLevelMathsQualificationData?.SchoolOrCollege);
            Assert.Null(result.AdvancedLevelMathsQualificationData?.LocalAuthority);
            Assert.Null(result.AdvancedLevelMathsQualificationData?.England);

            // Disadvantaged students - Establishment
            Assert.Equal(establishmentPerformance.TALLPUP_ALEV_1618_DIS_Est_Current_Num_Coded, result.DisadvantagedStudentsData.Establishment!.NumberOfStudents);
            Assert.Equal(establishmentPerformance.VA_INS_ALEV_DIS_Est_Current_Num_Coded, result.DisadvantagedStudentsData.Establishment!.ProgressScore);
            Assert.Equal(establishmentPerformance.UCI_INS_ALEV_DIS_Est_Current_Num_Coded, result.DisadvantagedStudentsData.Establishment!.ConfidenceLevelUpper);
            Assert.Equal(establishmentPerformance.LCI_INS_ALEV_DIS_Est_Current_Num_Coded, result.DisadvantagedStudentsData.Establishment!.ConfidenceLevelLower);
            Assert.Equal(establishmentPerformance.TALLPPE_ALEV_1618_DIS_Est_Current_Num_Coded, result.DisadvantagedStudentsData.Establishment!.Result.Points);
            Assert.Equal(establishmentPerformance.TALLPPEGRD_ALEV_DIS_Est_Current, result.DisadvantagedStudentsData.Establishment!.Result.Grade);

            // Disadvantaged students - LocalAuthority
            Assert.Equal(laPerformance.TALLPUP_ALEV_1618_DIS_LA_Current_Num_Coded, result.DisadvantagedStudentsData.LocalAuthority.NumberOfStudents);
            Assert.Equal(laPerformance.VA_INS_ALEV_DIS_LA_Current_Num_Coded, result.DisadvantagedStudentsData.LocalAuthority.ProgressScore);
            Assert.Equal(laPerformance.UCI_INS_ALEV_DIS_LA_Current_Num_Coded, result.DisadvantagedStudentsData.LocalAuthority.ConfidenceLevelUpper);
            Assert.Equal(laPerformance.LCI_INS_ALEV_DIS_LA_Current_Num_Coded, result.DisadvantagedStudentsData.LocalAuthority.ConfidenceLevelLower);
            Assert.Equal(laPerformance.TALLPPE_ALEV_1618_DIS_LA_Current_Num_Coded, result.DisadvantagedStudentsData.LocalAuthority.Result.Points);
            Assert.Equal(laPerformance.TALLPPEGRD_ALEV_DIS_LA_Current, result.DisadvantagedStudentsData.LocalAuthority.Result.Grade);

            // Disadvantaged students - England
            Assert.Equal(englandPerformance.TALLPUP_ALEV_1618_DIS_Eng_Current_Num_Coded, result.DisadvantagedStudentsData.England.NumberOfStudents);
            Assert.Equal(englandPerformance.VA_INS_ALEV_DIS_Eng_Current_Num_Coded, result.DisadvantagedStudentsData.England.ProgressScore);
            Assert.Equal(englandPerformance.UCI_INS_ALEV_DIS_Eng_Current_Num_Coded, result.DisadvantagedStudentsData.England.ConfidenceLevelUpper);
            Assert.Equal(englandPerformance.LCI_INS_ALEV_DIS_Eng_Current_Num_Coded, result.DisadvantagedStudentsData.England.ConfidenceLevelLower);
            Assert.Equal(englandPerformance.TALLPPE_ALEV_1618_DIS_Eng_Current_Num_Coded, result.DisadvantagedStudentsData.England.Result.Points);
            Assert.Equal(englandPerformance.TALLPPEGRD_ALEV_DIS_Eng_Current, result.DisadvantagedStudentsData.England.Result.Grade);

            // NonDisadvantaged students - LocalAuthority
            Assert.Equal(laPerformance.TALLPUP_ALEV_1618_NOTDIS_LA_Current_Num_Coded, result.NonDisadvantagedStudentsData.LocalAuthority.NumberOfStudents);
            Assert.Equal(laPerformance.VA_INS_ALEV_NOTDIS_LA_Current_Num_Coded, result.NonDisadvantagedStudentsData.LocalAuthority.ProgressScore);
            Assert.Equal(laPerformance.UCI_INS_ALEV_NOTDIS_LA_Current_Num_Coded, result.NonDisadvantagedStudentsData.LocalAuthority.ConfidenceLevelUpper);
            Assert.Equal(laPerformance.LCI_INS_ALEV_NOTDIS_LA_Current_Num_Coded, result.NonDisadvantagedStudentsData.LocalAuthority.ConfidenceLevelLower);
            Assert.Equal(laPerformance.TALLPPE_ALEV_1618_NOTDIS_LA_Current_Num_Coded, result.NonDisadvantagedStudentsData.LocalAuthority.Result.Points);
            Assert.Equal(laPerformance.TALLPPEGRD_ALEV_NOTDIS_LA_Current, result.NonDisadvantagedStudentsData.LocalAuthority.Result.Grade);

            // NonDisadvantaged students - England
            Assert.Equal(englandPerformance.TALLPUP_ALEV_1618_NOTDIS_Eng_Current_Num_Coded, result.NonDisadvantagedStudentsData.England.NumberOfStudents);
            Assert.Equal(englandPerformance.VA_INS_ALEV_NOTDIS_Eng_Current_Num_Coded, result.NonDisadvantagedStudentsData.England.ProgressScore);
            Assert.Equal(englandPerformance.UCI_INS_ALEV_NOTDIS_Eng_Current_Num_Coded, result.NonDisadvantagedStudentsData.England.ConfidenceLevelUpper);
            Assert.Equal(englandPerformance.LCI_INS_ALEV_NOTDIS_Eng_Current_Num_Coded, result.NonDisadvantagedStudentsData.England.ConfidenceLevelLower);
            Assert.Equal(englandPerformance.TALLPPE_ALEV_1618_NOTDIS_Eng_Current_Num_Coded, result.NonDisadvantagedStudentsData.England.Result.Points);
            Assert.Equal(englandPerformance.TALLPPEGRD_ALEV_NOTDIS_Eng_Current, result.NonDisadvantagedStudentsData.England.Result.Grade);
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

            // Establishment Additional data
            Assert.Null(result.AdditionalData?.TotalNoOfStudentsIncludedInThisMeasure);
            Assert.Null(result.AdditionalData?.Establishment.Points);
            Assert.Null(result.AdditionalData?.Establishment.Grade);

            Assert.Equal(englandPerformance.VA_INS_ACAD_Eng_Current_Num_Coded, result.ProgressScore.EnglandAverageScore);
            Assert.Equal(englandPerformance.TALLPPE_ACAD_1618_Eng_Current_Num_Coded, result.AverageResult.England.Points);
            Assert.Equal(englandPerformance.TALLPPEGRD_ACAD_1618_Eng_Current, result.AverageResult.England.Grade);

            // England Additional data
            Assert.Null(result.AdditionalData?.England.Points);
            Assert.Null(result.AdditionalData?.England.Grade);

            Assert.Equal(laPerformance.TALLPPE_ACAD_1618_LA_Current_Num_Coded, result.AverageResult.LocalAuthority.Points);
            Assert.Equal(laPerformance.TALLPPEGRD_ACAD_1618_LA_Current, result.AverageResult.LocalAuthority.Grade);

            // LA Additional data
            Assert.Null(result.AdditionalData?.LocalAuthority.Points);
            Assert.Null(result.AdditionalData?.LocalAuthority.Grade);

            // AdvancedLevelMathsQualificationData
            Assert.Equal(establishmentPerformance.L3M_PER_Est_Current_Pct_Coded, result.AdvancedLevelMathsQualificationData?.SchoolOrCollege);
            Assert.Equal(laPerformance.L3M_PER_LA_Current_Pct_Coded, result.AdvancedLevelMathsQualificationData?.LocalAuthority);
            Assert.Equal(englandPerformance.L3M_PER_Eng_Current_Pct_Coded, result.AdvancedLevelMathsQualificationData?.England);
        }
        else if (qualificationLevel == Level3.AppliedGeneral)
        {
            Assert.Equal(establishmentPerformance.TALLPUP_AGEN_Est_Current_Num_Coded, result.TotalNoOfStudentCompletedQualification);
            Assert.Equal(establishmentPerformance.VA_INS_AGEN_Est_Current_Num_Coded, result.ProgressScore.Score);
            Assert.Equal(establishmentPerformance.PROGRESS_BAND_AGEN_Est_Current, result.ProgressScore.BandingRating);
            Assert.Equal(establishmentPerformance.UCI_INS_AGEN_Est_Current_Num_Coded, result.ProgressScore.ConfidenceLevelUpper);
            Assert.Equal(establishmentPerformance.LCI_INS_AGEN_Est_Current_Num_Coded, result.ProgressScore.ConfidenceLevelLower);
            Assert.Equal(establishmentPerformance.TALLPPE_AGEN_Est_Current_Num_Coded, result.AverageResult.Establishment.Points);
            Assert.Equal(establishmentPerformance.TALLPPEGRD_AGEN_Est_Current, result.AverageResult.Establishment.Grade);

            // Establishment Additional data
            Assert.Null(result.AdditionalData?.TotalNoOfStudentsIncludedInThisMeasure);
            Assert.Null(result.AdditionalData?.Establishment.Points);
            Assert.Null(result.AdditionalData?.Establishment.Grade);

            Assert.Equal(englandPerformance.VA_INS_AGEN_Eng_Current_Num_Coded, result.ProgressScore.EnglandAverageScore);
            Assert.Equal(englandPerformance.TALLPPE_AGEN_Eng_Current_Num_Coded, result.AverageResult.England.Points);
            Assert.Equal(englandPerformance.TALLPPEGRD_AGEN_Eng_Current, result.AverageResult.England.Grade);

            // England Additional data
            Assert.Null(result.AdditionalData?.England.Points);
            Assert.Null(result.AdditionalData?.England.Grade);

            Assert.Equal(laPerformance.TALLPPE_AGEN_LA_Current_Num_Coded, result.AverageResult.LocalAuthority.Points);
            Assert.Equal(laPerformance.TALLPPEGRD_AGEN_LA_Current, result.AverageResult.LocalAuthority.Grade);

            // LA Additional data
            Assert.Null(result.AdditionalData?.LocalAuthority.Points);
            Assert.Null(result.AdditionalData?.LocalAuthority.Grade);

            // AdvancedLevelMathsQualificationData
            Assert.Null(result.AdvancedLevelMathsQualificationData?.SchoolOrCollege);
            Assert.Null(result.AdvancedLevelMathsQualificationData?.LocalAuthority);
            Assert.Null(result.AdvancedLevelMathsQualificationData?.England);
        }
        else if (qualificationLevel == Level3.TechLevel)
        {
            Assert.Equal(establishmentPerformance.TALLPUP_TLEV_Est_Current_Num_Coded, result.TotalNoOfStudentCompletedQualification);
            Assert.Equal(establishmentPerformance.VA_INS_TLEV_Est_Current_Num_Coded, result.ProgressScore.Score);
            Assert.Equal(establishmentPerformance.PROGRESS_BAND_TLEV_Est_Current, result.ProgressScore.BandingRating);
            Assert.Equal(establishmentPerformance.UCI_INS_TLEV_Est_Current_Num_Coded, result.ProgressScore.ConfidenceLevelUpper);
            Assert.Equal(establishmentPerformance.LCI_INS_TLEV_Est_Current_Num_Coded, result.ProgressScore.ConfidenceLevelLower);
            Assert.Equal(establishmentPerformance.TALLPPE_TLEV_Est_Current_Num_Coded, result.AverageResult.Establishment.Points);
            Assert.Equal(establishmentPerformance.TALLPPEGRD_TLEV_Est_Current, result.AverageResult.Establishment.Grade);

            // Establishment Additional data
            Assert.Null(result.AdditionalData?.TotalNoOfStudentsIncludedInThisMeasure);
            Assert.Null(result.AdditionalData?.Establishment.Points);
            Assert.Null(result.AdditionalData?.Establishment.Grade);

            Assert.Equal(englandPerformance.VA_INS_TLEV_Eng_Current_Num_Coded, result.ProgressScore.EnglandAverageScore);
            Assert.Equal(englandPerformance.TALLPPE_TLEV_Eng_Current_Num_Coded, result.AverageResult.England.Points);
            Assert.Equal(englandPerformance.TALLPPEGRD_TLEV_Eng_Current, result.AverageResult.England.Grade);

            // England Additional data
            Assert.Null(result.AdditionalData?.England.Points);
            Assert.Null(result.AdditionalData?.England.Grade);

            Assert.Equal(laPerformance.TALLPPE_TLEV_LA_Current_Num_Coded, result.AverageResult.LocalAuthority.Points);
            Assert.Equal(laPerformance.TALLPPEGRD_TLEV_LA_Current, result.AverageResult.LocalAuthority.Grade);

            // LA Additional data
            Assert.Null(result.AdditionalData?.LocalAuthority.Points);
            Assert.Null(result.AdditionalData?.LocalAuthority.Grade);

            // AdvancedLevelMathsQualificationData
            Assert.Null(result.AdvancedLevelMathsQualificationData?.SchoolOrCollege);
            Assert.Null(result.AdvancedLevelMathsQualificationData?.LocalAuthority);
            Assert.Null(result.AdvancedLevelMathsQualificationData?.England);
        }
    }
}
