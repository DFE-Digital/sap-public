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
            EnglandPercentageExceeding = GetEnglandPercentageExceeding(englandPerformance)
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
