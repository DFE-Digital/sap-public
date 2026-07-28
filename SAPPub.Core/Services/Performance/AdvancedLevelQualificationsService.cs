using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Core.Interfaces.Repositories.Performance;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels.Performance;

namespace SAPPub.Core.Services.Performance;

public class AdvancedLevelQualificationsService(
    IEstablishmentService establishmentService,
    IKs5PerformanceRepository ks5PerformanceRepository) : IAdvancedLevelQualificationsService
{
    public async Task<AdvancedLevelQualificationModel> GetAdvancedLevelQualificationDetailsAsync(
        string urn,
        Level3 level3Qualification,
        CancellationToken ct = default)
    {
        var establishment = await establishmentService.GetEstablishmentAsync(urn, ct);
        var establishmentPerformanceTask = ks5PerformanceRepository.GetEstablishmentPerformanceAsync(urn, ct);
        var englandPerformanceTask = ks5PerformanceRepository.GetEnglandPerformanceAsync(ct);
        var laPerformanceTask = ks5PerformanceRepository.GetLaPerformanceAsync(establishment.LAId, ct);

        await Task.WhenAll(establishmentPerformanceTask, englandPerformanceTask, laPerformanceTask);

        var establishmentPerformance = await establishmentPerformanceTask;
        var englandPerformance = await englandPerformanceTask;
        var laPerformance = await laPerformanceTask;

        return new AdvancedLevelQualificationModel
        {
            Urn = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            LAName = establishment.LAName,
            IsKS2 = establishment.IsKS2,
            IsKS4 = establishment.IsKS4,
            IsKS5 = establishment.IsKS5,
            QualificationType = level3Qualification,
            TotalNoOfStudentCompletedQualification = establishmentPerformance.TALLPUP_ACAD_1618_Est_Current_Num,
            ProgressScore = GetProgressScoreModel(level3Qualification, establishmentPerformance, englandPerformance),
            AverageResult = GetAverageResultModel(level3Qualification, establishmentPerformance, englandPerformance, laPerformance),
        };
    }

    private static ProgressScoreModel GetProgressScoreModel(
        Level3 level3Qualification,
        EstablishmentKs5Performance establishmentPerformance,
        EnglandKs5Performance englandPerformance)
    {
        return new ProgressScoreModel
        {
            Score = level3Qualification switch
            {
                Level3.ALevel => establishmentPerformance.VA_INS_ALEV_Est_Current_Num,
                _ => null,
            },
            BandingRating = level3Qualification switch
            {
                Level3.ALevel => establishmentPerformance.PROGRESS_BAND_ALEV_Est_Current,
                _ => null,
            },
            ConfidenceLevelUpper = level3Qualification switch
            {
                Level3.ALevel => establishmentPerformance.UCI_INS_ALEV_Est_Current_Num,
                _ => null,
            },
            ConfidenceLevelLower = level3Qualification switch
            {
                Level3.ALevel => establishmentPerformance.LCI_INS_ALEV_Est_Current_Num,
                _ => null,
            },
            EnglandAverageScore = level3Qualification switch
            {
                Level3.ALevel => englandPerformance.VA_INS_ALEV_Eng_Current_Num,
                _ => null,
            },
        };
    }

    private static AverageResultModel GetAverageResultModel(
        Level3 level3Qualification,
        EstablishmentKs5Performance establishmentPerformance,
        EnglandKs5Performance englandPerformance,
        LAKs5Performance laPerformance)
    {
        return new AverageResultModel
        {
            Establishment = new AverageResult
            {
                Points = level3Qualification switch
                {
                    Level3.ALevel => establishmentPerformance.TALLPPE_ALEV_1618_Est_Current_Num,
                    _ => null,
                },
                Grade = level3Qualification switch
                {
                    Level3.ALevel => establishmentPerformance.TALLPPEGRD_ALEV_1618_Est_Current,
                    _ => null,
                },                
            },
            LocalAuthority = new AverageResult
            {
                Points = level3Qualification switch
                {
                    Level3.ALevel => laPerformance.TALLPPE_ALEV_1618_LA_Current_Num,
                    _ => null,
                },
                Grade = level3Qualification switch
                {
                    Level3.ALevel => laPerformance.TALLPPEGRD_ALEV_1618_LA_Current,
                    _ => null,
                },                
            },
            England = new AverageResult
            {
                Points = level3Qualification switch
                {
                    Level3.ALevel => englandPerformance.TALLPPE_ALEV_1618_Eng_Current_Num,
                    _ => null,
                },
                Grade = level3Qualification switch
                {
                    Level3.ALevel => englandPerformance.TALLPPEGRD_ALEV_1618_Eng_Current,
                    _ => null,
                },                
            }
        };
    }
}
