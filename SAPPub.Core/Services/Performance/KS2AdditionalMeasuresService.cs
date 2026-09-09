using SAPPub.Core.Interfaces.Repositories.Performance;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Services.Performance;

public class KS2AdditionalMeasuresService(IKS2PerformanceRepository ks2PerformanceRepository, IAboutSchoolService aboutSchoolService) : IKS2AdditionalMeasuresService
{

    public async Task<KS2AdditionalMeasuresModel> GetAdditionalMeasures(string urn, string laId,  CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(urn);
        ct.ThrowIfCancellationRequested();

        var aboutSchoolServiceTask = aboutSchoolService.GetAboutSchoolDetailsAsync(urn, ct);
        var establishmentPerformanceTask = ks2PerformanceRepository.GetEstablishmentPerformanceAsync(urn, ct);
        var localAuthorityPerformanceTask = ks2PerformanceRepository.GetLaPerformanceAsync(laId, ct);
        var englandPerformanceTask = ks2PerformanceRepository.GetEnglandPerformanceAsync(ct);

        await Task.WhenAll(aboutSchoolServiceTask, establishmentPerformanceTask, localAuthorityPerformanceTask, englandPerformanceTask);

        var aboutSchool = await aboutSchoolServiceTask;
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
            EnglandSENSupportPopulation = englandPerformance.PSENELK_Eng_Current_Pct_Coded,

            EstablishmentNumPupilsEndOfKS2 = establishmentPerformance.TELIG_Est_Current_Num_Coded,
            LANumPupilsEndOfKS2 = CodedDouble.Empty,                // TODO in ticket 8c.i (needs to be mapped)
            EnglandNumPupilsEndOfKS2 = CodedDouble.Empty,           // TODO in ticket 8c.i (needs to be mapped)
            EstablishmentNumGirlsEndOfKS2 = establishmentPerformance.GELIG_Est_Current_Num_Coded,
            EstablishmentNumBoysEndOfKS2 = establishmentPerformance.BELIG_Est_Current_Num_Coded,
            EstablishmentNumEALEndOfKS2 = establishmentPerformance.TEALGRP2_Est_Current_Num_Coded,
            EstablishmentNumNonMobileEndOfKS2 = establishmentPerformance.TMOBN_Est_Current_Num_Coded,

            EstablishmentNumDisadvantagedEndOfKS2 = establishmentPerformance.TFSM6CLA1A_Est_Current_Num_Coded,
            LANumDisadvantagedEndOfKS2 = laPerformance.TFSM6CLA1A_LA_Current_Num_Coded,
            EnglandNumDisadvantagedEndOfKS2 = englandPerformance.TFSM6CLA1A_Eng_Current_Num_Coded,

            LANumNonDisadvantagedEndOfKS2 = laPerformance.TNOTFSM6CLA1A_LA_Current_Num_Coded,
            EnglandNumNonDisadvantagedEndOfKS2 = englandPerformance.TNOTFSM6CLA1A_Eng_Current_Num_Coded,

            EstablishmentPupilTotal = aboutSchool.NumberOfPupils,
            EnglandPupilTotal = CodedDouble.Empty                   //  TODO in ticket 8c.i (needs to be mapped)
        };
    }
}
