using SAPPub.Core.Entities;
using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Interfaces.Repositories.Performance;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Services.Performance;

public class KS2ScaledScoresService(
    IEstablishmentService establishmentService,
    IKS2PerformanceRepository ks2PerformanceRepository) : IKS2ScaledScoreService
{
    public async Task<KS2ScaledScoreModel> GetScaledScoreModel(string urn, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(urn);
        ct.ThrowIfCancellationRequested();

        var establishment = await establishmentService.GetEstablishmentAsync(urn, ct);
        var establishmentPerformanceTask = ks2PerformanceRepository.GetEstablishmentPerformanceAsync(urn, ct);
        var localAuthorityPerformanceTask = ks2PerformanceRepository.GetLaPerformanceAsync(establishment.LAId, ct);
        var englandPerformanceTask = ks2PerformanceRepository.GetEnglandPerformanceAsync(ct);

        await Task.WhenAll(establishmentPerformanceTask, localAuthorityPerformanceTask, englandPerformanceTask);

        var establishmentPerformance = await establishmentPerformanceTask;
        var englandPerformance = await englandPerformanceTask;
        var laPerformance = await localAuthorityPerformanceTask;

        return new KS2ScaledScoreModel
        {
            LAName = establishment.LAName,
            ReadAverageEstablishment = GetEstablishmentReadAverage(establishmentPerformance),
            ReadAverageEngland = GetEngReadAverage(englandPerformance),
            ReadAverageLA = GetLAReadAverage(laPerformance),
            MathsAverageEstablishment = GetEstablishmentMathsAverage(establishmentPerformance),
            MathsAverageEngland = GetEngMathsAverage(englandPerformance),
            MathsAverageLA = GetLAMathsAverage(laPerformance),
            GirlsAverageReading = establishmentPerformance.READ_AVERAGE_G_Est_Current_Num_Coded,
            GirlsAverageMaths = establishmentPerformance.MAT_AVERAGE_G_Est_Current_Num_Coded,
            BoysAverageReading = establishmentPerformance.READ_AVERAGE_B_Est_Current_Num_Coded,
            BoysAverageMaths = establishmentPerformance.MAT_AVERAGE_B_Est_Current_Num_Coded,
            AllPupilsAverageReading = establishmentPerformance.READ_AVERAGE_Est_Current_Num_Coded,
            AllPupilsAverageMaths = establishmentPerformance.MAT_AVERAGE_Est_Current_Num_Coded,

            EALAverageReading = establishmentPerformance.READ_AVERAGE_EAL_Est_Current_Num_Coded,
            EALAverageMaths = establishmentPerformance.MAT_AVERAGE_EAL_Est_Current_Num_Coded,
            EALTotalAverageReading = establishmentPerformance.READ_AVERAGE_Est_Current_Num_Coded,
            EALTotalAverageMaths = establishmentPerformance.MAT_AVERAGE_Est_Current_Num_Coded,

            NonMobileAverageReading = establishmentPerformance.READ_AVERAGE_MOBN_Est_Current_Num_Coded,
            NonMobileAverageMaths = establishmentPerformance.MAT_AVERAGE_MOBN_Est_Current_Num_Coded,

            DisadvantagedAverageReadingEstablishment = establishmentPerformance.READ_AVERAGE_FSM6CLA1A_Est_Current_Num_Coded,
            DisadvantagedAverageMathsEstablishment = establishmentPerformance.MAT_AVERAGE_FSM6CLA1A_Est_Current_Num_Coded,
            DisadvantagedAverageReadingLA = laPerformance.READ_AVERAGE_FSM6CLA1A_LA_Current_Num_Coded,
            DisadvantagedAverageMathsLA = laPerformance.MAT_AVERAGE_FSM6CLA1A_LA_Current_Num_Coded,
            DisadvantagedAverageReadingEngland = englandPerformance.READ_AVERAGE_FSM6CLA1A_ENG_Current_Num_Coded,
            DisadvantagedAverageMathsEngland = englandPerformance.MAT_AVERAGE_FSM6CLA1A_ENG_Current_Num_Coded,

            NonDisadvantagedAverageReadingLA = laPerformance.READ_AVERAGE_NOTFSM6CLA1A_LA_Current_Num_Coded,
            NonDisadvantagedAverageMathsLA = laPerformance.MAT_AVERAGE_NOTFSM6CLA1A_LA_Current_Num_Coded,
            NonDisadvantagedAverageReadingEngland = englandPerformance.READ_AVERAGE_NOTFSM6CLA1A_Eng_Current_Num_Coded,
            NonDisadvantagedAverageMathsEngland = englandPerformance.MAT_AVERAGE_NOTFSM6CLA1A_Eng_Current_Num_Coded,
        };
    }

    private static RelativeYearValues<CodedDouble> GetEstablishmentReadAverage(KS2EstablishmentPerformance performance)
    {
        return new RelativeYearValues<CodedDouble>
        {
            CurrentYear = performance.READ_AVERAGE_Est_Current_Num_Coded,
            PreviousYear = performance.READ_AVERAGE_Est_Previous_Num_Coded,
            TwoYearsAgo = performance.READ_AVERAGE_Est_Previous2_Num_Coded,
        };
    }

    private static RelativeYearValues<CodedDouble> GetLAReadAverage(KS2LAPerformance performance)
    {
        return new RelativeYearValues<CodedDouble>
        {
            CurrentYear = performance.READ_AVERAGE_LA_Current_Num_Coded,
            PreviousYear = performance.READ_AVERAGE_LA_Previous_Num_Coded,
            TwoYearsAgo = performance.READ_AVERAGE_LA_Previous2_Num_Coded,
        };
    }

    private static RelativeYearValues<CodedDouble> GetEngReadAverage(KS2EnglandPerformance performance)
    {
        return new RelativeYearValues<CodedDouble>
        {
            CurrentYear = performance.READ_AVERAGE_Eng_Current_Num_Coded,
            PreviousYear = performance.READ_AVERAGE_Eng_Previous_Num_Coded,
            TwoYearsAgo = performance.READ_AVERAGE_Eng_Previous2_Num_Coded,
        };
    }

    private static RelativeYearValues<CodedDouble> GetEstablishmentMathsAverage(KS2EstablishmentPerformance performance)
    {
        return new RelativeYearValues<CodedDouble>
        {
            CurrentYear = performance.MAT_AVERAGE_Est_Current_Num_Coded,
            PreviousYear = performance.MAT_AVERAGE_Est_Previous_Num_Coded,
            TwoYearsAgo = performance.MAT_AVERAGE_Est_Previous2_Num_Coded,
        };
    }

    private static RelativeYearValues<CodedDouble> GetLAMathsAverage(KS2LAPerformance performance)
    {
        return new RelativeYearValues<CodedDouble>
        {
            CurrentYear = performance.MAT_AVERAGE_LA_Current_Num_Coded,
            PreviousYear = performance.MAT_AVERAGE_LA_Previous_Num_Coded,
            TwoYearsAgo = performance.MAT_AVERAGE_LA_Previous2_Num_Coded,
        };
    }

    private static RelativeYearValues<CodedDouble> GetEngMathsAverage(KS2EnglandPerformance performance)
    {
        return new RelativeYearValues<CodedDouble>
        {
            CurrentYear = performance.MAT_AVERAGE_Eng_Current_Num_Coded,
            PreviousYear = performance.MAT_AVERAGE_Eng_Previous_Num_Coded,
            TwoYearsAgo = performance.MAT_AVERAGE_Eng_Previous2_Num_Coded,
        };
    }
}
