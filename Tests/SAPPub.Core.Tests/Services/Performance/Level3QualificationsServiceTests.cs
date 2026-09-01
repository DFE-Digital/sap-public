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
    private KS5EstablishmentPerformance _establishmentPerformance = null!;
    private KS5LAPerformance _laPerformance = null!;
    private KS5EnglandPerformance _englandPerformance = null!;

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

        SetupData(isAlevelQual, isAcademicQual);

        // Act
        var result = await _service.GetLevel3QualificationDetailsAsync(fakeEstablishment.URN, qualificationLevel, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fakeEstablishment.URN, result.Urn);
        Assert.Equal(fakeEstablishment.EstablishmentName, result.SchoolName);

        if (qualificationLevel == Level3.ALevel)
        {
            Assert.Equal(_establishmentPerformance.TALLPUP_ALEV_1618_Est_Current_Num_Coded, result.TotalNoOfStudentCompletedQualification);
            Assert.Equal(_establishmentPerformance.VA_INS_ALEV_Est_Current_Num_Coded, result.ProgressScore.Score);
            Assert.Equal(_establishmentPerformance.PROGRESS_BAND_ALEV_Est_Current, result.ProgressScore.BandingRating);
            Assert.Equal(_establishmentPerformance.UCI_INS_ALEV_Est_Current_Num_Coded, result.ProgressScore.ConfidenceLevelUpper);
            Assert.Equal(_establishmentPerformance.LCI_INS_ALEV_Est_Current_Num_Coded, result.ProgressScore.ConfidenceLevelLower);
            Assert.Equal(_establishmentPerformance.TALLPPE_ALEV_1618_Est_Current_Num_Coded, result.AverageResult.Establishment.Points);
            Assert.Equal(_establishmentPerformance.TALLPPEGRD_ALEV_1618_Est_Current, result.AverageResult.Establishment.Grade);

            // Establishment Additional data
            Assert.Equal(_establishmentPerformance.TINCLUDE_B3_Est_Current_Num_Coded, result.AdditionalData!.TotalNoOfStudentsIncludedInThisMeasure);
            Assert.Equal(_establishmentPerformance.TB3PTSE_Est_Current_Num_Coded, result.AdditionalData.Establishment.Points);
            Assert.Equal(_establishmentPerformance.TB3PTSE_GRD_Est_Current, result.AdditionalData.Establishment.Grade);

            Assert.Equal(_englandPerformance.VA_INS_ALEV_Eng_Current_Num_Coded, result.ProgressScore.EnglandAverageScore);
            Assert.Equal(_englandPerformance.TALLPPE_ALEV_1618_Eng_Current_Num_Coded, result.AverageResult.England.Points);
            Assert.Equal(_englandPerformance.TALLPPEGRD_ALEV_1618_Eng_Current, result.AverageResult.England.Grade);

            // England Additional data
            Assert.Equal(_englandPerformance.TB3PTSE_Eng_Current_Num_Coded, result.AdditionalData.England.Points);
            Assert.Equal(_englandPerformance.TB3PTSE_GRD_Eng_Current, result.AdditionalData.England.Grade);

            Assert.Equal(_laPerformance.TALLPPE_ALEV_1618_LA_Current_Num_Coded, result.AverageResult.LocalAuthority.Points);
            Assert.Equal(_laPerformance.TALLPPEGRD_ALEV_1618_LA_Current, result.AverageResult.LocalAuthority.Grade);

            // LA Additional data
            Assert.Equal(_laPerformance.TB3PTSE_LA_Current_Num_Coded, result.AdditionalData.LocalAuthority.Points);
            Assert.Equal(_laPerformance.TB3PTSE_GRD_LA_Current, result.AdditionalData.LocalAuthority.Grade);

            // AdvancedLevelMathsQualificationData
            Assert.Null(result.AdvancedLevelMathsQualificationData?.SchoolOrCollege);
            Assert.Null(result.AdvancedLevelMathsQualificationData?.LocalAuthority);
            Assert.Null(result.AdvancedLevelMathsQualificationData?.England);

            // Disadvantaged students - Establishment - A Levels
            Assert.Equal(_establishmentPerformance.TALLPUP_ALEV_1618_DIS_Est_Current_Num_Coded, result.DisadvantagedStudentsData.Establishment!.NumberOfStudents);
            Assert.Equal(_establishmentPerformance.VA_INS_ALEV_DIS_Est_Current_Num_Coded, result.DisadvantagedStudentsData.Establishment!.ProgressScore);
            Assert.Equal(_establishmentPerformance.UCI_INS_ALEV_DIS_Est_Current_Num_Coded, result.DisadvantagedStudentsData.Establishment!.ConfidenceLevelUpper);
            Assert.Equal(_establishmentPerformance.LCI_INS_ALEV_DIS_Est_Current_Num_Coded, result.DisadvantagedStudentsData.Establishment!.ConfidenceLevelLower);
            Assert.Equal(_establishmentPerformance.TALLPPE_ALEV_1618_DIS_Est_Current_Num_Coded, result.DisadvantagedStudentsData.Establishment!.Result.Points);
            Assert.Equal(_establishmentPerformance.TALLPPEGRD_ALEV_DIS_Est_Current, result.DisadvantagedStudentsData.Establishment!.Result.Grade);

            // Disadvantaged students - LocalAuthority  - A Levels
            Assert.Equal(_laPerformance.TALLPUP_ALEV_1618_DIS_LA_Current_Num_Coded, result.DisadvantagedStudentsData.LocalAuthority.NumberOfStudents);
            Assert.Equal(_laPerformance.VA_INS_ALEV_DIS_LA_Current_Num_Coded, result.DisadvantagedStudentsData.LocalAuthority.ProgressScore);
            Assert.Equal(_laPerformance.UCI_INS_ALEV_DIS_LA_Current_Num_Coded, result.DisadvantagedStudentsData.LocalAuthority.ConfidenceLevelUpper);
            Assert.Equal(_laPerformance.LCI_INS_ALEV_DIS_LA_Current_Num_Coded, result.DisadvantagedStudentsData.LocalAuthority.ConfidenceLevelLower);
            Assert.Equal(_laPerformance.TALLPPE_ALEV_1618_DIS_LA_Current_Num_Coded, result.DisadvantagedStudentsData.LocalAuthority.Result.Points);
            Assert.Equal(_laPerformance.TALLPPEGRD_ALEV_DIS_LA_Current, result.DisadvantagedStudentsData.LocalAuthority.Result.Grade);

            // Disadvantaged students - England  - A Levels
            Assert.Equal(_englandPerformance.TALLPUP_ALEV_1618_DIS_Eng_Current_Num_Coded, result.DisadvantagedStudentsData.England.NumberOfStudents);
            Assert.Equal(_englandPerformance.VA_INS_ALEV_DIS_Eng_Current_Num_Coded, result.DisadvantagedStudentsData.England.ProgressScore);
            Assert.Equal(_englandPerformance.UCI_INS_ALEV_DIS_Eng_Current_Num_Coded, result.DisadvantagedStudentsData.England.ConfidenceLevelUpper);
            Assert.Equal(_englandPerformance.LCI_INS_ALEV_DIS_Eng_Current_Num_Coded, result.DisadvantagedStudentsData.England.ConfidenceLevelLower);
            Assert.Equal(_englandPerformance.TALLPPE_ALEV_1618_DIS_Eng_Current_Num_Coded, result.DisadvantagedStudentsData.England.Result.Points);
            Assert.Equal(_englandPerformance.TALLPPEGRD_ALEV_DIS_Eng_Current, result.DisadvantagedStudentsData.England.Result.Grade);

            // NonDisadvantaged students - LocalAuthority - A Levels
            Assert.Equal(_laPerformance.TALLPUP_ALEV_1618_NOTDIS_LA_Current_Num_Coded, result.NonDisadvantagedStudentsData.LocalAuthority.NumberOfStudents);
            Assert.Equal(_laPerformance.VA_INS_ALEV_NOTDIS_LA_Current_Num_Coded, result.NonDisadvantagedStudentsData.LocalAuthority.ProgressScore);
            Assert.Equal(_laPerformance.UCI_INS_ALEV_NOTDIS_LA_Current_Num_Coded, result.NonDisadvantagedStudentsData.LocalAuthority.ConfidenceLevelUpper);
            Assert.Equal(_laPerformance.LCI_INS_ALEV_NOTDIS_LA_Current_Num_Coded, result.NonDisadvantagedStudentsData.LocalAuthority.ConfidenceLevelLower);
            Assert.Equal(_laPerformance.TALLPPE_ALEV_1618_NOTDIS_LA_Current_Num_Coded, result.NonDisadvantagedStudentsData.LocalAuthority.Result.Points);
            Assert.Equal(_laPerformance.TALLPPEGRD_ALEV_NOTDIS_LA_Current, result.NonDisadvantagedStudentsData.LocalAuthority.Result.Grade);

            // NonDisadvantaged students - England - A Levels
            Assert.Equal(_englandPerformance.TALLPUP_ALEV_1618_NOTDIS_Eng_Current_Num_Coded, result.NonDisadvantagedStudentsData.England.NumberOfStudents);
            Assert.Equal(_englandPerformance.VA_INS_ALEV_NOTDIS_Eng_Current_Num_Coded, result.NonDisadvantagedStudentsData.England.ProgressScore);
            Assert.Equal(_englandPerformance.UCI_INS_ALEV_NOTDIS_Eng_Current_Num_Coded, result.NonDisadvantagedStudentsData.England.ConfidenceLevelUpper);
            Assert.Equal(_englandPerformance.LCI_INS_ALEV_NOTDIS_Eng_Current_Num_Coded, result.NonDisadvantagedStudentsData.England.ConfidenceLevelLower);
            Assert.Equal(_englandPerformance.TALLPPE_ALEV_1618_NOTDIS_Eng_Current_Num_Coded, result.NonDisadvantagedStudentsData.England.Result.Points);
            Assert.Equal(_englandPerformance.TALLPPEGRD_ALEV_NOTDIS_Eng_Current, result.NonDisadvantagedStudentsData.England.Result.Grade);
        }
        else if (qualificationLevel == Level3.Academic)
        {
            Assert.Equal(_establishmentPerformance.TALLPUP_ACAD_1618_Est_Current_Num_Coded, result.TotalNoOfStudentCompletedQualification);
            Assert.Equal(_establishmentPerformance.VA_INS_ACAD_Est_Current_Num_Coded, result.ProgressScore.Score);
            Assert.Equal(_establishmentPerformance.PROGRESS_BAND_ACAD_Est_Current, result.ProgressScore.BandingRating);
            Assert.Equal(_establishmentPerformance.UCI_INS_ACAD_Est_Current_Num_Coded, result.ProgressScore.ConfidenceLevelUpper);
            Assert.Equal(_establishmentPerformance.LCI_INS_ACAD_Est_Current_Num_Coded, result.ProgressScore.ConfidenceLevelLower);
            Assert.Equal(_establishmentPerformance.TALLPPE_ACAD_1618_Est_Current_Num_Coded, result.AverageResult.Establishment.Points);
            Assert.Equal(_establishmentPerformance.TALLPPEGRD_ACAD_1618_Est_Current, result.AverageResult.Establishment.Grade);

            // Establishment Additional data
            Assert.Null(result.AdditionalData?.TotalNoOfStudentsIncludedInThisMeasure);
            Assert.Null(result.AdditionalData?.Establishment.Points);
            Assert.Null(result.AdditionalData?.Establishment.Grade);

            Assert.Equal(_englandPerformance.VA_INS_ACAD_Eng_Current_Num_Coded, result.ProgressScore.EnglandAverageScore);
            Assert.Equal(_englandPerformance.TALLPPE_ACAD_1618_Eng_Current_Num_Coded, result.AverageResult.England.Points);
            Assert.Equal(_englandPerformance.TALLPPEGRD_ACAD_1618_Eng_Current, result.AverageResult.England.Grade);

            // England Additional data
            Assert.Null(result.AdditionalData?.England.Points);
            Assert.Null(result.AdditionalData?.England.Grade);

            Assert.Equal(_laPerformance.TALLPPE_ACAD_1618_LA_Current_Num_Coded, result.AverageResult.LocalAuthority.Points);
            Assert.Equal(_laPerformance.TALLPPEGRD_ACAD_1618_LA_Current, result.AverageResult.LocalAuthority.Grade);

            // LA Additional data
            Assert.Null(result.AdditionalData?.LocalAuthority.Points);
            Assert.Null(result.AdditionalData?.LocalAuthority.Grade);

            // AdvancedLevelMathsQualificationData
            Assert.Equal(_establishmentPerformance.L3M_PER_Est_Current_Pct_Coded, result.AdvancedLevelMathsQualificationData?.SchoolOrCollege);
            Assert.Equal(_laPerformance.L3M_PER_LA_Current_Pct_Coded, result.AdvancedLevelMathsQualificationData?.LocalAuthority);
            Assert.Equal(_englandPerformance.L3M_PER_Eng_Current_Pct_Coded, result.AdvancedLevelMathsQualificationData?.England);

            // Disadvantaged students - Establishment - Academic Qualifications
            Assert.Equal(_establishmentPerformance.TALLPUP_ACAD_1618_DIS_Est_Current_Num_Coded, result.DisadvantagedStudentsData.Establishment!.NumberOfStudents);
            Assert.Equal(_establishmentPerformance.VA_INS_ACAD_DIS_Est_Current_Num_Coded, result.DisadvantagedStudentsData.Establishment!.ProgressScore);
            Assert.Equal(_establishmentPerformance.UCI_INS_ACAD_DIS_Est_Current_Num_Coded, result.DisadvantagedStudentsData.Establishment!.ConfidenceLevelUpper);
            Assert.Equal(_establishmentPerformance.LCI_INS_ACAD_DIS_Est_Current_Num_Coded, result.DisadvantagedStudentsData.Establishment!.ConfidenceLevelLower);
            Assert.Equal(_establishmentPerformance.TALLPPE_ACAD_1618_DIS_Est_Current_Num_Coded, result.DisadvantagedStudentsData.Establishment!.Result.Points);
            Assert.Equal(_establishmentPerformance.TALLPPEGRD_ACAD_DIS_Est_Current, result.DisadvantagedStudentsData.Establishment!.Result.Grade);

            // Disadvantaged students - LocalAuthority - Academic Qualifications
            Assert.Equal(_laPerformance.TALLPUP_ACAD_1618_DIS_LA_Current_Num_Coded, result.DisadvantagedStudentsData.LocalAuthority.NumberOfStudents);
            Assert.Equal(_laPerformance.VA_INS_ACAD_DIS_LA_Current_Num_Coded, result.DisadvantagedStudentsData.LocalAuthority.ProgressScore);
            Assert.Equal(_laPerformance.UCI_INS_ACAD_DIS_LA_Current_Num_Coded, result.DisadvantagedStudentsData.LocalAuthority.ConfidenceLevelUpper);
            Assert.Equal(_laPerformance.LCI_INS_ACAD_DIS_LA_Current_Num_Coded, result.DisadvantagedStudentsData.LocalAuthority.ConfidenceLevelLower);
            Assert.Equal(_laPerformance.TALLPPE_ACAD_1618_DIS_LA_Current_Num_Coded, result.DisadvantagedStudentsData.LocalAuthority.Result.Points);
            Assert.Equal(_laPerformance.TALLPPEGRD_ACAD_DIS_LA_Current, result.DisadvantagedStudentsData.LocalAuthority.Result.Grade);

            // Disadvantaged students - England - Academic Qualifications
            Assert.Equal(_englandPerformance.TALLPUP_ACAD_1618_DIS_Eng_Current_Num_Coded, result.DisadvantagedStudentsData.England.NumberOfStudents);
            Assert.Equal(_englandPerformance.VA_INS_ACAD_DIS_Eng_Current_Num_Coded, result.DisadvantagedStudentsData.England.ProgressScore);
            Assert.Equal(_englandPerformance.UCI_INS_ACAD_DIS_Eng_Current_Num_Coded, result.DisadvantagedStudentsData.England.ConfidenceLevelUpper);
            Assert.Equal(_englandPerformance.LCI_INS_ACAD_DIS_Eng_Current_Num_Coded, result.DisadvantagedStudentsData.England.ConfidenceLevelLower);
            Assert.Equal(_englandPerformance.TALLPPE_ACAD_1618_DIS_Eng_Current_Num_Coded, result.DisadvantagedStudentsData.England.Result.Points);
            Assert.Equal(_englandPerformance.TALLPPEGRD_ACAD_DIS_Eng_Current, result.DisadvantagedStudentsData.England.Result.Grade);

            // NonDisadvantaged students - LocalAuthority - Academic Qualifications
            Assert.Equal(_laPerformance.TALLPUP_ACAD_1618_NOTDIS_LA_Current_Num_Coded, result.NonDisadvantagedStudentsData.LocalAuthority.NumberOfStudents);
            Assert.Equal(_laPerformance.VA_INS_ACAD_NOTDIS_LA_Current_Num_Coded, result.NonDisadvantagedStudentsData.LocalAuthority.ProgressScore);
            Assert.Equal(_laPerformance.UCI_INS_ACAD_NOTDIS_LA_Current_Num_Coded, result.NonDisadvantagedStudentsData.LocalAuthority.ConfidenceLevelUpper);
            Assert.Equal(_laPerformance.LCI_INS_ACAD_NOTDIS_LA_Current_Num_Coded, result.NonDisadvantagedStudentsData.LocalAuthority.ConfidenceLevelLower);
            Assert.Equal(_laPerformance.TALLPPE_ACAD_1618_NOTDIS_LA_Current_Num_Coded, result.NonDisadvantagedStudentsData.LocalAuthority.Result.Points);
            Assert.Equal(_laPerformance.TALLPPEGRD_ACAD_NOTDIS_LA_Current, result.NonDisadvantagedStudentsData.LocalAuthority.Result.Grade);

            // NonDisadvantaged students - England - Academic Qualifications
            Assert.Equal(_englandPerformance.TALLPUP_ACAD_1618_NOTDIS_Eng_Current_Num_Coded, result.NonDisadvantagedStudentsData.England.NumberOfStudents);
            Assert.Equal(_englandPerformance.VA_INS_ACAD_NOTDIS_Eng_Current_Num_Coded, result.NonDisadvantagedStudentsData.England.ProgressScore);
            Assert.Equal(_englandPerformance.UCI_INS_ACAD_NOTDIS_Eng_Current_Num_Coded, result.NonDisadvantagedStudentsData.England.ConfidenceLevelUpper);
            Assert.Equal(_englandPerformance.LCI_INS_ACAD_NOTDIS_Eng_Current_Num_Coded, result.NonDisadvantagedStudentsData.England.ConfidenceLevelLower);
            Assert.Equal(_englandPerformance.TALLPPE_ACAD_1618_NOTDIS_Eng_Current_Num_Coded, result.NonDisadvantagedStudentsData.England.Result.Points);
            Assert.Equal(_englandPerformance.TALLPPEGRD_ACAD_NOTDIS_Eng_Current, result.NonDisadvantagedStudentsData.England.Result.Grade);
        }
        else if (qualificationLevel == Level3.AppliedGeneral)
        {
            Assert.Equal(_establishmentPerformance.TALLPUP_AGEN_Est_Current_Num_Coded, result.TotalNoOfStudentCompletedQualification);
            Assert.Equal(_establishmentPerformance.VA_INS_AGEN_Est_Current_Num_Coded, result.ProgressScore.Score);
            Assert.Equal(_establishmentPerformance.PROGRESS_BAND_AGEN_Est_Current, result.ProgressScore.BandingRating);
            Assert.Equal(_establishmentPerformance.UCI_INS_AGEN_Est_Current_Num_Coded, result.ProgressScore.ConfidenceLevelUpper);
            Assert.Equal(_establishmentPerformance.LCI_INS_AGEN_Est_Current_Num_Coded, result.ProgressScore.ConfidenceLevelLower);
            Assert.Equal(_establishmentPerformance.TALLPPE_AGEN_Est_Current_Num_Coded, result.AverageResult.Establishment.Points);
            Assert.Equal(_establishmentPerformance.TALLPPEGRD_AGEN_Est_Current, result.AverageResult.Establishment.Grade);

            // Establishment Additional data
            Assert.Null(result.AdditionalData?.TotalNoOfStudentsIncludedInThisMeasure);
            Assert.Null(result.AdditionalData?.Establishment.Points);
            Assert.Null(result.AdditionalData?.Establishment.Grade);

            Assert.Equal(_englandPerformance.VA_INS_AGEN_Eng_Current_Num_Coded, result.ProgressScore.EnglandAverageScore);
            Assert.Equal(_englandPerformance.TALLPPE_AGEN_Eng_Current_Num_Coded, result.AverageResult.England.Points);
            Assert.Equal(_englandPerformance.TALLPPEGRD_AGEN_Eng_Current, result.AverageResult.England.Grade);

            // England Additional data
            Assert.Null(result.AdditionalData?.England.Points);
            Assert.Null(result.AdditionalData?.England.Grade);

            Assert.Equal(_laPerformance.TALLPPE_AGEN_LA_Current_Num_Coded, result.AverageResult.LocalAuthority.Points);
            Assert.Equal(_laPerformance.TALLPPEGRD_AGEN_LA_Current, result.AverageResult.LocalAuthority.Grade);

            // LA Additional data
            Assert.Null(result.AdditionalData?.LocalAuthority.Points);
            Assert.Null(result.AdditionalData?.LocalAuthority.Grade);

            // AdvancedLevelMathsQualificationData
            Assert.Null(result.AdvancedLevelMathsQualificationData?.SchoolOrCollege);
            Assert.Null(result.AdvancedLevelMathsQualificationData?.LocalAuthority);
            Assert.Null(result.AdvancedLevelMathsQualificationData?.England);

            // Disadvantaged students - Establishment
            Assert.Equal(_establishmentPerformance.TALLPUP_AGEN_DIS_Est_Current_Num_Coded, result.DisadvantagedStudentsData.Establishment!.NumberOfStudents);
            Assert.Equal(_establishmentPerformance.VA_INS_AGEN_DIS_Est_Current_Num_Coded, result.DisadvantagedStudentsData.Establishment!.ProgressScore);
            Assert.Equal(_establishmentPerformance.UCI_INS_AGEN_DIS_Est_Current_Num_Coded, result.DisadvantagedStudentsData.Establishment!.ConfidenceLevelUpper);
            Assert.Equal(_establishmentPerformance.LCI_INS_AGEN_DIS_Est_Current_Num_Coded, result.DisadvantagedStudentsData.Establishment!.ConfidenceLevelLower);
            Assert.Equal(_establishmentPerformance.TALLPPE_AGEN_DIS_Est_Current_Num_Coded, result.DisadvantagedStudentsData.Establishment!.Result.Points);
            Assert.Equal(_establishmentPerformance.TALLPPEGRD_AGEN_DIS_Est_Current, result.DisadvantagedStudentsData.Establishment!.Result.Grade);

            // Disadvantaged students - LocalAuthority
            Assert.Equal(_laPerformance.TALLPUP_AGEN_DIS_LA_Current_Num_Coded, result.DisadvantagedStudentsData.LocalAuthority.NumberOfStudents);
            Assert.Equal(_laPerformance.VA_INS_AGEN_DIS_LA_Current_Num_Coded, result.DisadvantagedStudentsData.LocalAuthority.ProgressScore);
            Assert.Equal(_laPerformance.UCI_INS_AGEN_DIS_LA_Current_Num_Coded, result.DisadvantagedStudentsData.LocalAuthority.ConfidenceLevelUpper);
            Assert.Equal(_laPerformance.LCI_INS_AGEN_DIS_LA_Current_Num_Coded, result.DisadvantagedStudentsData.LocalAuthority.ConfidenceLevelLower);
            Assert.Equal(_laPerformance.TALLPPE_AGEN_DIS_LA_Current_Num_Coded, result.DisadvantagedStudentsData.LocalAuthority.Result.Points);
            Assert.Equal(_laPerformance.TALLPPEGRD_AGEN_DIS_LA_Current, result.DisadvantagedStudentsData.LocalAuthority.Result.Grade);

            // Disadvantaged students - England
            Assert.Equal(_englandPerformance.TALLPUP_AGEN_DIS_Eng_Current_Num_Coded, result.DisadvantagedStudentsData.England.NumberOfStudents);
            Assert.Equal(_englandPerformance.VA_INS_AGEN_DIS_Eng_Current_Num_Coded, result.DisadvantagedStudentsData.England.ProgressScore);
            Assert.Equal(_englandPerformance.UCI_INS_AGEN_DIS_Eng_Current_Num_Coded, result.DisadvantagedStudentsData.England.ConfidenceLevelUpper);
            Assert.Equal(_englandPerformance.LCI_INS_AGEN_DIS_Eng_Current_Num_Coded, result.DisadvantagedStudentsData.England.ConfidenceLevelLower);
            Assert.Equal(_englandPerformance.TALLPPE_AGEN_DIS_Eng_Current_Num_Coded, result.DisadvantagedStudentsData.England.Result.Points);
            Assert.Equal(_englandPerformance.TALLPPEGRD_AGEN_DIS_Eng_Current, result.DisadvantagedStudentsData.England.Result.Grade);

            // NonDisadvantaged students - LocalAuthority
            Assert.Equal(_laPerformance.TALLPUP_AGEN_NOTDIS_LA_Current_Num_Coded, result.NonDisadvantagedStudentsData.LocalAuthority.NumberOfStudents);
            Assert.Equal(_laPerformance.VA_INS_AGEN_NOTDIS_LA_Current_Num_Coded, result.NonDisadvantagedStudentsData.LocalAuthority.ProgressScore);
            Assert.Equal(_laPerformance.UCI_INS_AGEN_NOTDIS_LA_Current_Num_Coded, result.NonDisadvantagedStudentsData.LocalAuthority.ConfidenceLevelUpper);
            Assert.Equal(_laPerformance.LCI_INS_AGEN_NOTDIS_LA_Current_Num_Coded, result.NonDisadvantagedStudentsData.LocalAuthority.ConfidenceLevelLower);
            Assert.Equal(_laPerformance.TALLPPE_AGEN_NOTDIS_LA_Current_Num_Coded, result.NonDisadvantagedStudentsData.LocalAuthority.Result.Points);
            Assert.Equal(_laPerformance.TALLPPEGRD_AGEN_NOTDIS_LA_Current, result.NonDisadvantagedStudentsData.LocalAuthority.Result.Grade);

            // NonDisadvantaged students - England
            Assert.Equal(_englandPerformance.TALLPUP_AGEN_NOTDIS_Eng_Current_Num_Coded, result.NonDisadvantagedStudentsData.England.NumberOfStudents);
            Assert.Equal(_englandPerformance.VA_INS_AGEN_NOTDIS_Eng_Current_Num_Coded, result.NonDisadvantagedStudentsData.England.ProgressScore);
            Assert.Equal(_englandPerformance.UCI_INS_AGEN_NOTDIS_Eng_Current_Num_Coded, result.NonDisadvantagedStudentsData.England.ConfidenceLevelUpper);
            Assert.Equal(_englandPerformance.LCI_INS_AGEN_NOTDIS_Eng_Current_Num_Coded, result.NonDisadvantagedStudentsData.England.ConfidenceLevelLower);
            Assert.Equal(_englandPerformance.TALLPPE_AGEN_NOTDIS_Eng_Current_Num_Coded, result.NonDisadvantagedStudentsData.England.Result.Points);
            Assert.Equal(_englandPerformance.TALLPPEGRD_AGEN_NOTDIS_Eng_Current, result.NonDisadvantagedStudentsData.England.Result.Grade);
        }
        else if (qualificationLevel == Level3.TechLevel)
        {
            Assert.Equal(_establishmentPerformance.TALLPUP_TLEV_Est_Current_Num_Coded, result.TotalNoOfStudentCompletedQualification);
            Assert.Equal(_establishmentPerformance.VA_INS_TLEV_Est_Current_Num_Coded, result.ProgressScore.Score);
            Assert.Equal(_establishmentPerformance.PROGRESS_BAND_TLEV_Est_Current, result.ProgressScore.BandingRating);
            Assert.Equal(_establishmentPerformance.UCI_INS_TLEV_Est_Current_Num_Coded, result.ProgressScore.ConfidenceLevelUpper);
            Assert.Equal(_establishmentPerformance.LCI_INS_TLEV_Est_Current_Num_Coded, result.ProgressScore.ConfidenceLevelLower);
            Assert.Equal(_establishmentPerformance.TALLPPE_TLEV_Est_Current_Num_Coded, result.AverageResult.Establishment.Points);
            Assert.Equal(_establishmentPerformance.TALLPPEGRD_TLEV_Est_Current, result.AverageResult.Establishment.Grade);

            // Establishment Additional data
            Assert.Null(result.AdditionalData?.TotalNoOfStudentsIncludedInThisMeasure);
            Assert.Null(result.AdditionalData?.Establishment.Points);
            Assert.Null(result.AdditionalData?.Establishment.Grade);

            Assert.Equal(_englandPerformance.VA_INS_TLEV_Eng_Current_Num_Coded, result.ProgressScore.EnglandAverageScore);
            Assert.Equal(_englandPerformance.TALLPPE_TLEV_Eng_Current_Num_Coded, result.AverageResult.England.Points);
            Assert.Equal(_englandPerformance.TALLPPEGRD_TLEV_Eng_Current, result.AverageResult.England.Grade);

            // England Additional data
            Assert.Null(result.AdditionalData?.England.Points);
            Assert.Null(result.AdditionalData?.England.Grade);

            Assert.Equal(_laPerformance.TALLPPE_TLEV_LA_Current_Num_Coded, result.AverageResult.LocalAuthority.Points);
            Assert.Equal(_laPerformance.TALLPPEGRD_TLEV_LA_Current, result.AverageResult.LocalAuthority.Grade);

            // LA Additional data
            Assert.Null(result.AdditionalData?.LocalAuthority.Points);
            Assert.Null(result.AdditionalData?.LocalAuthority.Grade);

            // AdvancedLevelMathsQualificationData
            Assert.Null(result.AdvancedLevelMathsQualificationData?.SchoolOrCollege);
            Assert.Null(result.AdvancedLevelMathsQualificationData?.LocalAuthority);
            Assert.Null(result.AdvancedLevelMathsQualificationData?.England);
        }
    }

    private void SetupData(bool isAlevelQual, bool isAcademicQual)
    {
        _mockEstablishmentService
            .Setup(r => r.GetEstablishmentMinimumAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeEstablishment);

        _establishmentPerformance = new KS5EstablishmentPerformance
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

            // A Levels - Disadvantaged
            TALLPUP_ALEV_1618_DIS_Est_Current_Num_Coded = new CodedDouble(130, string.Empty, string.Empty),
            VA_INS_ALEV_DIS_Est_Current_Num_Coded = new CodedDouble(79.26, string.Empty, string.Empty),
            UCI_INS_ALEV_DIS_Est_Current_Num_Coded = new CodedDouble(3, string.Empty, string.Empty),
            LCI_INS_ALEV_DIS_Est_Current_Num_Coded = new CodedDouble(2, string.Empty, string.Empty),
            TALLPPE_ALEV_1618_DIS_Est_Current_Num_Coded = new CodedDouble(20.13, string.Empty, string.Empty),
            TALLPPEGRD_ALEV_DIS_Est_Current = new CodedString("A", string.Empty, string.Empty),

            // Academic Qualfications - Disadvantaged
            TALLPUP_ACAD_1618_DIS_Est_Current_Num_Coded = new CodedDouble(250, string.Empty, string.Empty),
            VA_INS_ACAD_DIS_Est_Current_Num_Coded = new CodedDouble(89.19, string.Empty, string.Empty),
            UCI_INS_ACAD_DIS_Est_Current_Num_Coded = new CodedDouble(2, string.Empty, string.Empty),
            LCI_INS_ACAD_DIS_Est_Current_Num_Coded = new CodedDouble(1, string.Empty, string.Empty),
            TALLPPE_ACAD_1618_DIS_Est_Current_Num_Coded = new CodedDouble(10.39, string.Empty, string.Empty),
            TALLPPEGRD_ACAD_DIS_Est_Current = new CodedString("A", string.Empty, string.Empty),

            // Applied general Qualfications - Disadvantaged
            TALLPUP_AGEN_DIS_Est_Current_Num_Coded = new CodedDouble(190, string.Empty, string.Empty),
            VA_INS_AGEN_DIS_Est_Current_Num_Coded = new CodedDouble(53.46, string.Empty, string.Empty),
            UCI_INS_AGEN_DIS_Est_Current_Num_Coded = new CodedDouble(3, string.Empty, string.Empty),
            LCI_INS_AGEN_DIS_Est_Current_Num_Coded = new CodedDouble(0.5, string.Empty, string.Empty),
            TALLPPE_AGEN_DIS_Est_Current_Num_Coded = new CodedDouble(11.87, string.Empty, string.Empty),
            TALLPPEGRD_AGEN_DIS_Est_Current = new CodedString("B", string.Empty, string.Empty),
        };

        _englandPerformance = new KS5EnglandPerformance
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

            // A Levels - Disadvantaged
            TALLPUP_ALEV_1618_DIS_Eng_Current_Num_Coded = new CodedDouble(135, string.Empty, string.Empty),
            VA_INS_ALEV_DIS_Eng_Current_Num_Coded = new CodedDouble(67.78, string.Empty, string.Empty),
            UCI_INS_ALEV_DIS_Eng_Current_Num_Coded = new CodedDouble(2, string.Empty, string.Empty),
            LCI_INS_ALEV_DIS_Eng_Current_Num_Coded = new CodedDouble(3, string.Empty, string.Empty),
            TALLPPE_ALEV_1618_DIS_Eng_Current_Num_Coded = new CodedDouble(15.69, string.Empty, string.Empty),
            TALLPPEGRD_ALEV_DIS_Eng_Current = new CodedString("B", string.Empty, string.Empty),

            // A Levels - Non-Disadvantaged
            TALLPUP_ALEV_1618_NOTDIS_Eng_Current_Num_Coded = new CodedDouble(95, string.Empty, string.Empty),
            VA_INS_ALEV_NOTDIS_Eng_Current_Num_Coded = new CodedDouble(71.13, string.Empty, string.Empty),
            UCI_INS_ALEV_NOTDIS_Eng_Current_Num_Coded = new CodedDouble(3, string.Empty, string.Empty),
            LCI_INS_ALEV_NOTDIS_Eng_Current_Num_Coded = new CodedDouble(1, string.Empty, string.Empty),
            TALLPPE_ALEV_1618_NOTDIS_Eng_Current_Num_Coded = new CodedDouble(18.27, string.Empty, string.Empty),
            TALLPPEGRD_ALEV_NOTDIS_Eng_Current = new CodedString("A", string.Empty, string.Empty),

            // Academic Qualfications - Disadvantaged
            TALLPUP_ACAD_1618_DIS_Eng_Current_Num_Coded = new CodedDouble(290, string.Empty, string.Empty),
            VA_INS_ACAD_DIS_Eng_Current_Num_Coded = new CodedDouble(57.89, string.Empty, string.Empty),
            UCI_INS_ACAD_DIS_Eng_Current_Num_Coded = new CodedDouble(3, string.Empty, string.Empty),
            LCI_INS_ACAD_DIS_Eng_Current_Num_Coded = new CodedDouble(1, string.Empty, string.Empty),
            TALLPPE_ACAD_1618_DIS_Eng_Current_Num_Coded = new CodedDouble(25.78, string.Empty, string.Empty),
            TALLPPEGRD_ACAD_DIS_Eng_Current = new CodedString("B", string.Empty, string.Empty),

            // Academic Qualfications - Non-Disadvantaged
            TALLPUP_ACAD_1618_NOTDIS_Eng_Current_Num_Coded = new CodedDouble(187, string.Empty, string.Empty),
            VA_INS_ACAD_NOTDIS_Eng_Current_Num_Coded = new CodedDouble(65.45, string.Empty, string.Empty),
            UCI_INS_ACAD_NOTDIS_Eng_Current_Num_Coded = new CodedDouble(2, string.Empty, string.Empty),
            LCI_INS_ACAD_NOTDIS_Eng_Current_Num_Coded = new CodedDouble(1, string.Empty, string.Empty),
            TALLPPE_ACAD_1618_NOTDIS_Eng_Current_Num_Coded = new CodedDouble(38.95, string.Empty, string.Empty),
            TALLPPEGRD_ACAD_NOTDIS_Eng_Current = new CodedString("B", string.Empty, string.Empty),

            // Applied general Qualfications - Disadvantaged
            TALLPUP_AGEN_DIS_Eng_Current_Num_Coded = new CodedDouble(195, string.Empty, string.Empty),
            VA_INS_AGEN_DIS_Eng_Current_Num_Coded = new CodedDouble(51.77, string.Empty, string.Empty),
            UCI_INS_AGEN_DIS_Eng_Current_Num_Coded = new CodedDouble(2, string.Empty, string.Empty),
            LCI_INS_AGEN_DIS_Eng_Current_Num_Coded = new CodedDouble(1, string.Empty, string.Empty),
            TALLPPE_AGEN_DIS_Eng_Current_Num_Coded = new CodedDouble(21.56, string.Empty, string.Empty),
            TALLPPEGRD_AGEN_DIS_Eng_Current = new CodedString("B", string.Empty, string.Empty),

            // Applied general Qualfications - Non-Disadvantaged
            TALLPUP_AGEN_NOTDIS_Eng_Current_Num_Coded = new CodedDouble(205, string.Empty, string.Empty),
            VA_INS_AGEN_NOTDIS_Eng_Current_Num_Coded = new CodedDouble(79.32, string.Empty, string.Empty),
            UCI_INS_AGEN_NOTDIS_Eng_Current_Num_Coded = new CodedDouble(2.5, string.Empty, string.Empty),
            LCI_INS_AGEN_NOTDIS_Eng_Current_Num_Coded = new CodedDouble(1, string.Empty, string.Empty),
            TALLPPE_AGEN_NOTDIS_Eng_Current_Num_Coded = new CodedDouble(31.74, string.Empty, string.Empty),
            TALLPPEGRD_AGEN_NOTDIS_Eng_Current = new CodedString("A", string.Empty, string.Empty),
        };

        _laPerformance = new KS5LAPerformance
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

            // A Levels - Disadvantaged
            TALLPUP_ALEV_1618_DIS_LA_Current_Num_Coded = new CodedDouble(110, string.Empty, string.Empty),
            VA_INS_ALEV_DIS_LA_Current_Num_Coded = new CodedDouble(55.11, string.Empty, string.Empty),
            UCI_INS_ALEV_DIS_LA_Current_Num_Coded = new CodedDouble(3, string.Empty, string.Empty),
            LCI_INS_ALEV_DIS_LA_Current_Num_Coded = new CodedDouble(2, string.Empty, string.Empty),
            TALLPPE_ALEV_1618_DIS_LA_Current_Num_Coded = new CodedDouble(16.19, string.Empty, string.Empty),
            TALLPPEGRD_ALEV_DIS_LA_Current = new CodedString("B", string.Empty, string.Empty),

            // A Levels - Non-Disadvantaged
            TALLPUP_ALEV_1618_NOTDIS_LA_Current_Num_Coded = new CodedDouble(80, string.Empty, string.Empty),
            VA_INS_ALEV_NOTDIS_LA_Current_Num_Coded = new CodedDouble(70.11, string.Empty, string.Empty),
            UCI_INS_ALEV_NOTDIS_LA_Current_Num_Coded = new CodedDouble(3, string.Empty, string.Empty),
            LCI_INS_ALEV_NOTDIS_LA_Current_Num_Coded = new CodedDouble(1, string.Empty, string.Empty),
            TALLPPE_ALEV_1618_NOTDIS_LA_Current_Num_Coded = new CodedDouble(19.71, string.Empty, string.Empty),
            TALLPPEGRD_ALEV_NOTDIS_LA_Current = new CodedString("B", string.Empty, string.Empty),

            // Academic Qualfications - Disadvantaged
            TALLPUP_ACAD_1618_DIS_LA_Current_Num_Coded = new CodedDouble(352, string.Empty, string.Empty),
            VA_INS_ACAD_DIS_LA_Current_Num_Coded = new CodedDouble(43.56, string.Empty, string.Empty),
            UCI_INS_ACAD_DIS_LA_Current_Num_Coded = new CodedDouble(3, string.Empty, string.Empty),
            LCI_INS_ACAD_DIS_LA_Current_Num_Coded = new CodedDouble(1, string.Empty, string.Empty),
            TALLPPE_ACAD_1618_DIS_LA_Current_Num_Coded = new CodedDouble(15.25, string.Empty, string.Empty),
            TALLPPEGRD_ACAD_DIS_LA_Current = new CodedString("B", string.Empty, string.Empty),

            // Academic Qualfications - Non-Disadvantaged
            TALLPUP_ACAD_1618_NOTDIS_LA_Current_Num_Coded = new CodedDouble(180, string.Empty, string.Empty),
            VA_INS_ACAD_NOTDIS_LA_Current_Num_Coded = new CodedDouble(81.54, string.Empty, string.Empty),
            UCI_INS_ACAD_NOTDIS_LA_Current_Num_Coded = new CodedDouble(3, string.Empty, string.Empty),
            LCI_INS_ACAD_NOTDIS_LA_Current_Num_Coded = new CodedDouble(0.5, string.Empty, string.Empty),
            TALLPPE_ACAD_1618_NOTDIS_LA_Current_Num_Coded = new CodedDouble(17.32, string.Empty, string.Empty),
            TALLPPEGRD_ACAD_NOTDIS_LA_Current = new CodedString("A", string.Empty, string.Empty),

            // Applied general Qualfications - Disadvantaged
            TALLPUP_AGEN_DIS_LA_Current_Num_Coded = new CodedDouble(245, string.Empty, string.Empty),
            VA_INS_AGEN_DIS_LA_Current_Num_Coded = new CodedDouble(33.15, string.Empty, string.Empty),
            UCI_INS_AGEN_DIS_LA_Current_Num_Coded = new CodedDouble(2, string.Empty, string.Empty),
            LCI_INS_AGEN_DIS_LA_Current_Num_Coded = new CodedDouble(1, string.Empty, string.Empty),
            TALLPPE_AGEN_DIS_LA_Current_Num_Coded = new CodedDouble(17.25, string.Empty, string.Empty),
            TALLPPEGRD_AGEN_DIS_LA_Current = new CodedString("C", string.Empty, string.Empty),

            // Applied general Qualfications - Non-Disadvantaged
            TALLPUP_AGEN_NOTDIS_LA_Current_Num_Coded = new CodedDouble(160, string.Empty, string.Empty),
            VA_INS_AGEN_NOTDIS_LA_Current_Num_Coded = new CodedDouble(83.12, string.Empty, string.Empty),
            UCI_INS_AGEN_NOTDIS_LA_Current_Num_Coded = new CodedDouble(3, string.Empty, string.Empty),
            LCI_INS_AGEN_NOTDIS_LA_Current_Num_Coded = new CodedDouble(1, string.Empty, string.Empty),
            TALLPPE_AGEN_NOTDIS_LA_Current_Num_Coded = new CodedDouble(15.33, string.Empty, string.Empty),
            TALLPPEGRD_AGEN_NOTDIS_LA_Current = new CodedString("A", string.Empty, string.Empty),
        };
        _mockKs5PerformanceRepository
            .Setup(r => r.GetEstablishmentPerformanceAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_establishmentPerformance);

        _mockKs5PerformanceRepository
            .Setup(r => r.GetEnglandPerformanceAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_englandPerformance);

        _mockKs5PerformanceRepository
            .Setup(r => r.GetLaPerformanceAsync(fakeEstablishment.LAId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_laPerformance);
    }
}
