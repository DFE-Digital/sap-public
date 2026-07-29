using SAPPub.Core.Interfaces.Repositories.Performance;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Services.Performance;

public class EnglishAndMathsQualificationsService(
    IEstablishmentService establishmentService,
    IKs5PerformanceRepository ks5PerformanceRepository) : IEnglishAndMathsQualificationsService
{
    public async Task<EnglishMathsQualificationModel> GetEnglishAndMathsQualificationDetailsAsync(string urn, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

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
                NumberOfStudents = establishmentPerformance.T_SCOPEEX_E_Est_Current_Num_Coded,
                SchoolOrCollege = establishmentPerformance.PROGEX_E_Est_Current_Num_Coded,
                LaAverage = laPerformance.PROGEX_E_LA_Current_Num_Coded,
                EnglandAverage = englandPerformance.PROGEX_E_Eng_Current_Num_Coded
            },
            AverageMathsProgress = new EnglishMathsScoreModel
            {
                NumberOfStudents = establishmentPerformance.T_SCOPEEX_M_Est_Current_Num_Coded,
                SchoolOrCollege = establishmentPerformance.PROGEX_M_Est_Current_Num_Coded,
                LaAverage = laPerformance.PROGEX_M_LA_Current_Num_Coded,
                EnglandAverage = englandPerformance.PROGEX_M_Eng_Current_Num_Coded
            },
            EnteredForEnglishQualification = new EnglishMathsScoreModel
            {
                NumberOfStudents = CodedDouble.Empty,
                SchoolOrCollege = establishmentPerformance.ENTRY_PER_E_Est_Current_Pct_Coded,
                LaAverage = laPerformance.ENTRY_PER_E_LA_Current_Pct_Coded,
                EnglandAverage = englandPerformance.ENTRY_PER_E_Eng_Current_Pct_Coded

            },
            EnteredForMathsQualification = new EnglishMathsScoreModel
            {
                NumberOfStudents = CodedDouble.Empty,
                SchoolOrCollege = establishmentPerformance.ENTRY_PER_M_Est_Current_Pct_Coded,
                LaAverage = laPerformance.ENTRY_PER_M_LA_Current_Pct_Coded,
                EnglandAverage = englandPerformance.ENTRY_PER_M_Eng_Current_Pct_Coded
            }
        };
    }
}
