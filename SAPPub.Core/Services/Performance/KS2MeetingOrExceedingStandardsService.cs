using SAPPub.Core.Entities;
using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Interfaces.Repositories.Performance;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Services.Performance;

public class KS2MeetingOrExceedingStandardsService(
    IEstablishmentService establishmentService,
    IKS2PerformanceRepository ks2PerformanceRepository) : IKS2MeetingOrExceedingStandardsService
{
    public async Task<KS2MeetingOrExceedingStandardsModel> GetMeetingOrExceedingStandardsPercentages(string urn, CancellationToken ct = default)
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

        return new KS2MeetingOrExceedingStandardsModel
        {
            EstablishmentPercentageMeetingOrExceeding = GetEstablishmentPercentageMeetingOrExceeding(establishmentPerformance),
            LocalAuthorityPercentageMeetingOrExceeding = GetLocalAuthorityPercentageMeetingOrExceeding(laPerformance),
            EnglandPercentageMeetingOrExceeding = GetEnglandPercentageMeetingOrExceeding(englandPerformance),
            EstablishmentPercentageExceeding = GetEstablishmentPercentageExceeding(establishmentPerformance),
            LocalAuthorityPercentageExceeding = GetLocalAuthorityPercentageExceeding(laPerformance),
            EnglandPercentageExceeding = GetEnglandPercentageExceeding(englandPerformance),

            GirlsMeetingExpectedStandard     = establishmentPerformance.PTRWM_EXP_G_Est_Current_Pct_Coded,
            GirlsExceedingExpectedStandard = establishmentPerformance.PTRWM_HIGH_G_Est_Current_Pct_Coded,
            BoysMeetingExpectedStandard = establishmentPerformance.PTRWM_EXP_B_Est_Current_Pct_Coded,
            BoysExceedingExpectedStandard = establishmentPerformance.PTRWM_HIGH_B_Est_Current_Pct_Coded,
            AllPupilsMeetingExpectedStandard     = establishmentPerformance.PTRWM_EXP_Est_Current_Pct_Coded,
            AllPupilsExceedingExpectedStandard = establishmentPerformance.PTRWM_HIGH_Est_Current_Pct_Coded,

            /* EAL */
            EALMeetingExpectedStandard = establishmentPerformance.PTRWM_EXP_EAL_Est_Current_Pct_Coded,
            EALExceedingExpectedStandard = establishmentPerformance.PTRWM_HIGH_EAL_Est_Current_Pct_Coded,
            AllEALPupilsMeetingExpectedStandard = establishmentPerformance.PTRWM_EXP_Est_Current_Pct_Coded,
            AllEALPupilsExceedingExpectedStandard = establishmentPerformance.PTRWM_HIGH_Est_Current_Pct_Coded,

            /* Non-mobile pupils */
            NonMobileMeetingExpectedStandard = establishmentPerformance.PTRWM_EXP_MOBN_Est_Current_Pct_Coded,
            NonMobileExceedingExpectedStandard = establishmentPerformance.PTRWM_HIGH_MOBN_Est_Current_Pct_Coded,
            AllNonMobilePupilsMeetingExpectedStandard = establishmentPerformance.PTRWM_EXP_Est_Current_Pct_Coded,
            AllNonMobilePupilsExceedingExpectedStandard = establishmentPerformance.PTRWM_HIGH_Est_Current_Pct_Coded,

            /* Disadvantaged pupils */
            EstablishmentDisadvantagedMeetingExpectedStandard = establishmentPerformance.PTRWM_EXP_FSM6CLA1A_Est_Current_Pct_Coded,
            EstablishmentDisadvantagedExceedingExpectedStandard = establishmentPerformance.PTRWM_HIGH_FSM6CLA1A_Est_Current_Pct_Coded,
            LocalAuthorityDisadvantagedMeetingExpectedStandard = laPerformance.PTRWM_EXP_FSM6CLA1A_LA_Current_Pct_Coded,
            LocalAuthorityDisadvantagedExceedingExpectedStandard = laPerformance.PTRWM_HIGH_FSM6CLA1A_LA_Current_Pct_Coded,
            EnglandAuthorityDisadvantagedMeetingExpectedStandard = englandPerformance.PTRWM_EXP_FSM6CLA1A_Eng_Current_Pct_Coded,
            EnglandAuthorityDisadvantagedExceedingExpectedStandard = englandPerformance.PTRWM_HIGH_FSM6CLA1A_Eng_Current_Pct,

            /* Non-disadvantaged pupils */
            LocalAuthorityNonDisadvantagedMeetingExpectedStandard = laPerformance.PTRWM_EXP_NOTFSM6CLA1A_LA_Current_Pct_Coded,
            LocalAuthorityNonDisadvantagedExceedingExpectedStandard = laPerformance.PTRWM_HIGH_NOTFSM6CLA1A_LA_Current_Pct_Coded,
            EnglandAuthorityNonDisadvantagedMeetingExpectedStandard = englandPerformance.PTRWM_EXP_NOTFSM6CLA1A_Eng_Current_Pct_Coded,
            EnglandAuthorityNonDisadvantagedExceedingExpectedStandard = englandPerformance.PTRWM_HIGH_NOTFSM6CLA1A_Eng_Current_Pct_Coded
        };

    }

    private static RelativeYearValues<CodedDouble> GetEnglandPercentageMeetingOrExceeding(KS2EnglandPerformance englandPerformance)
    {
        return new RelativeYearValues<CodedDouble>
        {
            CurrentYear = englandPerformance.PTRWM_EXP_Eng_Current_Pct_Coded,
            PreviousYear = englandPerformance.PTRWM_EXP_Eng_Previous_Pct_Coded,
            TwoYearsAgo = englandPerformance.PTRWM_EXP_Eng_Previous2_Pct_Coded
        };
    }

    private static RelativeYearValues<CodedDouble> GetLocalAuthorityPercentageMeetingOrExceeding(KS2LAPerformance laPerformance)
    {
        return new RelativeYearValues<CodedDouble>
        {
            CurrentYear = laPerformance.PTRWM_EXP_LA_Current_Pct_Coded,
            PreviousYear = laPerformance.PTRWM_EXP_LA_Previous_Pct_Coded,
            TwoYearsAgo = laPerformance.PTRWM_EXP_LA_Previous2_Pct_Coded
        };
    }

    private static RelativeYearValues<CodedDouble> GetEstablishmentPercentageMeetingOrExceeding(KS2EstablishmentPerformance establishmentPerformance)
    {
        return new RelativeYearValues<CodedDouble>
        {
            CurrentYear = establishmentPerformance.PTRWM_EXP_Est_Current_Pct_Coded,
            PreviousYear = establishmentPerformance.PTRWM_EXP_Est_Previous_Pct_Coded,
            TwoYearsAgo = establishmentPerformance.PTRWM_EXP_Est_Previous2_Pct_Coded
        };
    }

    private static RelativeYearValues<CodedDouble> GetEnglandPercentageExceeding(KS2EnglandPerformance englandPerformance)
    {
        return new RelativeYearValues<CodedDouble>
        {
            CurrentYear = englandPerformance.PTRWM_HIGH_Eng_Current_Pct_Coded,
            PreviousYear = englandPerformance.PTRWM_HIGH_Eng_Previous_Pct_Coded,
            TwoYearsAgo = englandPerformance.PTRWM_HIGH_Eng_Previous2_Pct_Coded
        };
    }

    private static RelativeYearValues<CodedDouble> GetLocalAuthorityPercentageExceeding(KS2LAPerformance laPerformance)
    {
        return new RelativeYearValues<CodedDouble>
        {
            CurrentYear = laPerformance.PTRWM_HIGH_LA_Current_Pct_Coded,
            PreviousYear = laPerformance.PTRWM_HIGH_LA_Previous_Pct_Coded,
            TwoYearsAgo = laPerformance.PTRWM_HIGH_LA_Previous2_Pct_Coded
        };
    }

    private static RelativeYearValues<CodedDouble> GetEstablishmentPercentageExceeding(KS2EstablishmentPerformance establishmentPerformance)
    {
        return new RelativeYearValues<CodedDouble>
        {
            CurrentYear = establishmentPerformance.PTRWM_HIGH_Est_Current_Pct_Coded,
            PreviousYear = establishmentPerformance.PTRWM_HIGH_Est_Previous_Pct_Coded,
            TwoYearsAgo = establishmentPerformance.PTRWM_HIGH_Est_Previous2_Pct_Coded
        };
    }
}
