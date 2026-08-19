using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Core.Interfaces.Repositories.Performance;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels.Common;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Services.Performance;

public class Level3QualificationsService(
    IEstablishmentService establishmentService,
    IKs5PerformanceRepository ks5PerformanceRepository) : ILevel3QualificationsService
{
    public async Task<Level3QualificationModel> GetLevel3QualificationDetailsAsync(
        string urn,
        Level3 level3Qualification,
        CancellationToken ct = default)
    {
        var establishment = await establishmentService.GetEstablishmentMinimumAsync(urn, ct);
        var establishmentPerformanceTask = ks5PerformanceRepository.GetEstablishmentPerformanceAsync(urn, ct);
        var englandPerformanceTask = ks5PerformanceRepository.GetEnglandPerformanceAsync(ct);
        var laPerformanceTask = ks5PerformanceRepository.GetLaPerformanceAsync(establishment.LAId, ct);

        await Task.WhenAll(establishmentPerformanceTask, englandPerformanceTask, laPerformanceTask);

        var establishmentPerformance = await establishmentPerformanceTask;
        var englandPerformance = await englandPerformanceTask;
        var laPerformance = await laPerformanceTask;

        return new Level3QualificationModel
        {
            Urn = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            LAName = establishment.LAName,
            IsKS2 = establishment.IsKS2,
            IsKS4 = establishment.IsKS4,
            IsKS5 = establishment.IsKS5,
            QualificationType = level3Qualification,
            TotalNoOfStudentCompletedQualification = GetTotalNoOfStudentsCompletedQualification(level3Qualification, establishmentPerformance),
            ProgressScore = GetProgressScoreModel(level3Qualification, establishmentPerformance, englandPerformance),
            AverageResult = GetAverageResultModel(level3Qualification, establishmentPerformance, laPerformance, englandPerformance),
            AdditionalData = GetAdditionalData(level3Qualification, establishmentPerformance, laPerformance, englandPerformance),
            AdvancedLevelMathsQualificationData = AdvancedLevelMathsQualificationData(level3Qualification, establishmentPerformance, laPerformance, englandPerformance),
        };
    }

    private static CodedDouble GetTotalNoOfStudentsCompletedQualification(
        Level3 level3Qualification,
        KS5EstablishmentPerformance establishmentPerformance)
    {
        return level3Qualification switch
        {
            Level3.ALevel => establishmentPerformance.TALLPUP_ALEV_1618_Est_Current_Num_Coded,
            Level3.Academic => establishmentPerformance.TALLPUP_ACAD_1618_Est_Current_Num_Coded,
            Level3.AppliedGeneral => establishmentPerformance.TALLPUP_AGEN_Est_Current_Num_Coded,
            Level3.TechLevel => establishmentPerformance.TALLPUP_TLEV_Est_Current_Num_Coded,
            _ => CodedDouble.Empty,
        };
    }

    private static ProgressScoreModel GetProgressScoreModel(
        Level3 level3Qualification,
        KS5EstablishmentPerformance establishmentPerformance,
        KS5EnglandPerformance englandPerformance)
    {
        return new ProgressScoreModel
        {
            Score = level3Qualification switch
            {
                Level3.ALevel => establishmentPerformance.VA_INS_ALEV_Est_Current_Num_Coded,
                Level3.Academic => establishmentPerformance.VA_INS_ACAD_Est_Current_Num_Coded,
                Level3.AppliedGeneral => establishmentPerformance.VA_INS_AGEN_Est_Current_Num_Coded,
                Level3.TechLevel => establishmentPerformance.VA_INS_TLEV_Est_Current_Num_Coded,
                _ => CodedDouble.Empty,
            },
            BandingRating = level3Qualification switch
            {
                Level3.ALevel => establishmentPerformance.PROGRESS_BAND_ALEV_Est_Current,
                Level3.Academic => establishmentPerformance.PROGRESS_BAND_ACAD_Est_Current,
                Level3.AppliedGeneral => establishmentPerformance.PROGRESS_BAND_AGEN_Est_Current,
                Level3.TechLevel => establishmentPerformance.PROGRESS_BAND_TLEV_Est_Current,
                _ => CodedString.Empty,
            },
            ConfidenceLevelUpper = level3Qualification switch
            {
                Level3.ALevel => establishmentPerformance.UCI_INS_ALEV_Est_Current_Num_Coded,
                Level3.Academic => establishmentPerformance.UCI_INS_ACAD_Est_Current_Num_Coded,
                Level3.AppliedGeneral => establishmentPerformance.UCI_INS_AGEN_Est_Current_Num_Coded,
                Level3.TechLevel => establishmentPerformance.UCI_INS_TLEV_Est_Current_Num_Coded,
                _ => CodedDouble.Empty,
            },
            ConfidenceLevelLower = level3Qualification switch
            {
                Level3.ALevel => establishmentPerformance.LCI_INS_ALEV_Est_Current_Num_Coded,
                Level3.Academic => establishmentPerformance.LCI_INS_ACAD_Est_Current_Num_Coded,
                Level3.AppliedGeneral => establishmentPerformance.LCI_INS_AGEN_Est_Current_Num_Coded,
                Level3.TechLevel => establishmentPerformance.LCI_INS_TLEV_Est_Current_Num_Coded,
                _ => CodedDouble.Empty,
            },
            EnglandAverageScore = level3Qualification switch
            {
                Level3.ALevel => englandPerformance.VA_INS_ALEV_Eng_Current_Num_Coded,
                Level3.Academic => englandPerformance.VA_INS_ACAD_Eng_Current_Num_Coded,
                Level3.AppliedGeneral => englandPerformance.VA_INS_AGEN_Eng_Current_Num_Coded,
                Level3.TechLevel => englandPerformance.VA_INS_TLEV_Eng_Current_Num_Coded,
                _ => CodedDouble.Empty,
            },
        };
    }

    private static AverageResultModel GetAverageResultModel(
        Level3 level3Qualification,
        KS5EstablishmentPerformance establishmentPerformance,        
        KS5LAPerformance laPerformance,
        KS5EnglandPerformance englandPerformance)
    {
        return new AverageResultModel
        {
            Establishment = new PerformanceResult
            {
                Points = level3Qualification switch
                {
                    Level3.ALevel => establishmentPerformance.TALLPPE_ALEV_1618_Est_Current_Num_Coded,
                    Level3.Academic => establishmentPerformance.TALLPPE_ACAD_1618_Est_Current_Num_Coded,
                    Level3.AppliedGeneral => establishmentPerformance.TALLPPE_AGEN_Est_Current_Num_Coded,
                    Level3.TechLevel => establishmentPerformance.TALLPPE_TLEV_Est_Current_Num_Coded,
                    _ => CodedDouble.Empty,
                },
                Grade = level3Qualification switch
                {
                    Level3.ALevel => establishmentPerformance.TALLPPEGRD_ALEV_1618_Est_Current,
                    Level3.Academic => establishmentPerformance.TALLPPEGRD_ACAD_1618_Est_Current,
                    Level3.AppliedGeneral => establishmentPerformance.TALLPPEGRD_AGEN_Est_Current,
                    Level3.TechLevel => establishmentPerformance.TALLPPEGRD_TLEV_Est_Current,
                    _ => CodedString.Empty,
                },                
            },
            LocalAuthority = new PerformanceResult
            {
                Points = level3Qualification switch
                {
                    Level3.ALevel => laPerformance.TALLPPE_ALEV_1618_LA_Current_Num_Coded,
                    Level3.Academic => laPerformance.TALLPPE_ACAD_1618_LA_Current_Num_Coded,
                    Level3.AppliedGeneral => laPerformance.TALLPPE_AGEN_LA_Current_Num_Coded,
                    Level3.TechLevel => laPerformance.TALLPPE_TLEV_LA_Current_Num_Coded,
                    _ => CodedDouble.Empty,
                },
                Grade = level3Qualification switch
                {
                    Level3.ALevel => laPerformance.TALLPPEGRD_ALEV_1618_LA_Current,
                    Level3.Academic => laPerformance.TALLPPEGRD_ACAD_1618_LA_Current,
                    Level3.AppliedGeneral => laPerformance.TALLPPEGRD_AGEN_LA_Current,
                    Level3.TechLevel => laPerformance.TALLPPEGRD_TLEV_LA_Current,
                    _ => CodedString.Empty,
                },                
            },
            England = new PerformanceResult
            {
                Points = level3Qualification switch
                {
                    Level3.ALevel => englandPerformance.TALLPPE_ALEV_1618_Eng_Current_Num_Coded,
                    Level3.Academic => englandPerformance.TALLPPE_ACAD_1618_Eng_Current_Num_Coded,
                    Level3.AppliedGeneral => englandPerformance.TALLPPE_AGEN_Eng_Current_Num_Coded,
                    Level3.TechLevel => englandPerformance.TALLPPE_TLEV_Eng_Current_Num_Coded,
                    _ => CodedDouble.Empty,
                },
                Grade = level3Qualification switch
                {
                    Level3.ALevel => englandPerformance.TALLPPEGRD_ALEV_1618_Eng_Current,
                    Level3.Academic => englandPerformance.TALLPPEGRD_ACAD_1618_Eng_Current,
                    Level3.AppliedGeneral => englandPerformance.TALLPPEGRD_AGEN_Eng_Current,
                    Level3.TechLevel => englandPerformance.TALLPPEGRD_TLEV_Eng_Current,
                    _ => CodedString.Empty,
                },                
            }
        };
    }

    private static AdditionalDataModel? GetAdditionalData(
        Level3 level3Qualification,
        KS5EstablishmentPerformance establishmentPerformance,        
        KS5LAPerformance laPerformance,
        KS5EnglandPerformance englandPerformance)
    {
        if (level3Qualification != Level3.ALevel)
            return null;

        return new AdditionalDataModel
        {
            TotalNoOfStudentsIncludedInThisMeasure = establishmentPerformance.TINCLUDE_B3_Est_Current_Num_Coded,
            Establishment = new PerformanceResult
            {
                Points = establishmentPerformance.TB3PTSE_Est_Current_Num_Coded,
                Grade = establishmentPerformance.TB3PTSE_GRD_Est_Current,
            },
            LocalAuthority = new PerformanceResult
            {
                Points = laPerformance.TB3PTSE_LA_Current_Num_Coded,
                Grade = laPerformance.TB3PTSE_GRD_LA_Current,
            },
            England = new PerformanceResult
            {
                Points = englandPerformance.TB3PTSE_Eng_Current_Num_Coded,
                Grade = englandPerformance.TB3PTSE_GRD_Eng_Current,
            }
        };
    }

    private static SimpleCodedDoubleTableModel? AdvancedLevelMathsQualificationData(
        Level3 level3Qualification,
        KS5EstablishmentPerformance establishmentPerformance,        
        KS5LAPerformance laPerformance,
        KS5EnglandPerformance englandPerformance)
    {
        if (level3Qualification != Level3.Academic)
            return null;

        return new SimpleCodedDoubleTableModel
        {
            SchoolOrCollege = establishmentPerformance.L3M_PER_Est_Current_Pct_Coded,
            LocalAuthority = laPerformance.L3M_PER_LA_Current_Pct_Coded,
            England = englandPerformance.L3M_PER_Eng_Current_Pct_Coded
        };
    }
}
