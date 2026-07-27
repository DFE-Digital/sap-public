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
        var establishmentTask = establishmentService.GetEstablishmentAsync(urn, ct);
        var establishmentPerformanceTask = ks5PerformanceRepository.GetEstablishmentPerformanceAsync(urn, ct);
        var englandPerformanceTask = ks5PerformanceRepository.GetEnglandPerformanceAsync(ct);

        await Task.WhenAll(establishmentTask, establishmentPerformanceTask, englandPerformanceTask);

        var establishment = await establishmentTask;
        var establishmentPerformance = await establishmentPerformanceTask;
        var englandPerformance = await englandPerformanceTask;

        return new AdvancedLevelQualificationModel
        {
            Urn = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            IsKS2 = establishment.IsKS2,
            IsKS4 = establishment.IsKS4,
            IsKS5 = establishment.IsKS5,
            QualificationType = level3Qualification,
            TotalNoOfStudentCompletedQualification = establishmentPerformance.TALLPUP_ACAD_1618_Est_Current_Num,
            ProgressScore = GetProgressScoreModel(level3Qualification, establishmentPerformance, englandPerformance)
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
}
