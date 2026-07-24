using SAPPub.Core.Interfaces.Repositories.Performance;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels.Performance;

namespace SAPPub.Core.Services.Performance;

public class EnglishAndMathsQualificationsService(
    IEstablishmentService establishmentService,
    IKs5PerformanceRepository ks5PerformanceRepository) : IEnglishAndMathsQualificationsService
{
    public async Task<EnglishMathsQualificationModel> GetAdvancedLevelQualificationDetailsAsync(string urn, CancellationToken ct = default)
    {
        var establishment = await establishmentService.GetEstablishmentAsync(urn, ct);
        
        var establishmentPerformanceTask = ks5PerformanceRepository.GetEstablishmentPerformanceAsync(urn, ct);
        var englandPerformanceTask = ks5PerformanceRepository.GetEnglandPerformanceAsync(ct);
        var localAuthorityPerformanceTask = ks5PerformanceRepository.GetLaPerformanceAsync(establishment.LAId, ct);

        await Task.WhenAll(establishmentPerformanceTask, englandPerformanceTask, localAuthorityPerformanceTask);
      
        var establishmentPerformance = await establishmentPerformanceTask;
        var englandPerformance = await englandPerformanceTask;
        var laPerformance = await localAuthorityPerformanceTask;

        return new EnglishMathsQualificationModel
        {
            Urn = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            IsKS2 = establishment.IsKS2,
            IsKS4 = establishment.IsKS4,
            IsKS5 = establishment.IsKS5,
            LAName = establishment.LAName,
            AverageEnglishProgress = new EnglishMathsScoreModel 
            {
                NumberOfStudents = establishmentPerformance.T_SCOPEEX_E_Est_Current_Num,
                SchoolOrCollege = establishmentPerformance.PROGEX_E_Est_Current_Num,
                LaAverage = laPerformance.PROGEX_E_LA_Current_Num,
                EnglandAverage = englandPerformance.PROGEX_E_Eng_Current_Num
            },
            AverageMathsProgress = new EnglishMathsScoreModel 
            {
                NumberOfStudents = establishmentPerformance.T_SCOPEEX_M_Est_Current_Num,
                SchoolOrCollege = establishmentPerformance.PROGEX_M_Est_Current_Num,
                LaAverage = laPerformance.PROGEX_M_LA_Current_Num,
                EnglandAverage = englandPerformance.PROGEX_M_Eng_Current_Num
            },
            EnteredForEnglishQualification = new EnglishMathsScoreModel
            { 
                SchoolOrCollege = establishmentPerformance.ENTRY_PER_E_Est_Current_Pct,
                LaAverage = laPerformance.ENTRY_PER_E_LA_Current_Pct,
                EnglandAverage = englandPerformance.ENTRY_PER_E_Eng_Current_Pct

            },
            EnteredForMathsQualification = new EnglishMathsScoreModel
            {
                SchoolOrCollege = establishmentPerformance.ENTRY_PER_M_Est_Current_Pct,
                LaAverage = laPerformance.ENTRY_PER_M_LA_Current_Pct,
                EnglandAverage = englandPerformance.ENTRY_PER_M_Eng_Current_Pct
            }
        };
    }
}
