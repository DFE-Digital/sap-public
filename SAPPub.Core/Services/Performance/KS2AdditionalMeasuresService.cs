using SAPPub.Core.Interfaces.Repositories.Performance;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels.Performance;

namespace SAPPub.Core.Services.Performance;

public class KS2AdditionalMeasuresService(
    IEstablishmentService establishmentService, 
    IKS2PerformanceRepository ks2PerformanceRepository) : IKS2AdditionalMeasuresService
{

    public async Task<KS2AdditionalMeasuresModel> GetAdditionalMeasures(string urn, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(urn);
        ct.ThrowIfCancellationRequested();

        var establishment = await establishmentService.GetEstablishmentMinimumAsync(urn, ct);
        var establishmentPerformanceTask = ks2PerformanceRepository.GetEstablishmentPerformanceAsync(urn, ct);
        var localAuthorityPerformanceTask = ks2PerformanceRepository.GetLaPerformanceAsync(establishment.LAId, ct);
        var englandPerformanceTask = ks2PerformanceRepository.GetEnglandPerformanceAsync(ct);

        await Task.WhenAll(establishmentPerformanceTask, localAuthorityPerformanceTask, englandPerformanceTask);

        var establishmentPerformance = await establishmentPerformanceTask;
        var englandPerformance = await englandPerformanceTask;
        var laPerformance = await localAuthorityPerformanceTask;

        return new KS2AdditionalMeasuresModel
        {
            EstablishmentGrammarAtExpectedStandard = establishmentPerformance.PTGPS_EXP_Est_Current_Pct_Coded,
            EstablishmentGrammarAtHigherStandard = establishmentPerformance.PTGPS_HIGH_Est_Current_Pct_Coded,
            EstablishmentEHCPPopulation = establishmentPerformance.PSENELE_Est_Current_Pct_Coded,
            EstablishmentSENSupportPopulation = establishmentPerformance.PSENELK_Est_Current_Pct_Coded,
            LAGrammarAtExpectedStandard = laPerformance.PTGPS_EXP_LA_Current_Pct_Coded,
            LAGrammarAtHigherStandard = laPerformance.PTGPS_HIGH_LA_Current_Pct_Coded,
            EnglandGrammarAtExpectedStandard = englandPerformance.PTGPS_EXP_Eng_Current_Pct_Coded,
            EnglandGrammarAtHigherStandard = englandPerformance.PTGPS_HIGH_Eng_Current_Pct_Coded,
            EnglandEHCPPopulation = englandPerformance.PSENELE_Eng_Current_Pct_Coded,
            EnglandSENSupportPopulation = englandPerformance.PSENELK_Eng_Current_Pct_Coded
        };
    }
}
