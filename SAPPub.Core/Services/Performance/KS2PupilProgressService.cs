using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Repositories.Performance;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Services.Performance;

public class KS2PupilProgressService(
    IEstablishmentService establishmentService,
    IKS2PerformanceRepository ks2PerformanceRepository) : IKS2PupilProgressService
{
    public async Task<KS2PupilPerformance> GetPupilProgressAsync(string urn, AcademicYearSelection selectedYear, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(urn);
        ct.ThrowIfCancellationRequested();

        var establishment = await establishmentService.GetEstablishmentAsync(urn, ct);

        if (string.IsNullOrWhiteSpace(establishment.URN))
        {
            return new KS2PupilPerformance { Urn = urn };
        }

        var ks2EstablishPerformanceTask = ks2PerformanceRepository.GetEstablishmentPerformanceAsync(urn, ct);
        var ks2LAPerformanceTask = ks2PerformanceRepository.GetLaPerformanceAsync(establishment.LAId, ct);
        var ks2EnglandPerformanceTask = ks2PerformanceRepository.GetEnglandPerformanceAsync(ct);

        await Task.WhenAll(ks2EstablishPerformanceTask, ks2LAPerformanceTask, ks2EnglandPerformanceTask);

        var establishmentPerformance = await ks2EstablishPerformanceTask;
        var laPerformance = await ks2LAPerformanceTask;
        var englandPerformance = await ks2EnglandPerformanceTask;

        var establishmentReadingDescription = selectedYear switch
        {
            AcademicYearSelection.Previous2 => establishmentPerformance.READPROG_DESCR_Est_Previous2_Num_Coded,
            _ => CodedString.Empty
        };
        var readingBandingDescriptions = selectedYear switch
        {
            AcademicYearSelection.Previous2 => new ProgressBandingDescriptions(
                englandPerformance.ProgBand_Read_Band1_Eng_Previous2_Desc,
                englandPerformance.ProgBand_Read_Band2_Eng_Previous2_Desc,
                englandPerformance.ProgBand_Read_Band3_Eng_Previous2_Desc,
                englandPerformance.ProgBand_Read_Band4_Eng_Previous2_Desc,
                englandPerformance.ProgBand_Read_Band5_Eng_Previous2_Desc),
            _ => ProgressBandingDescriptions.Empty
        };

        var establishmentWritingDescription = selectedYear switch
        {
            AcademicYearSelection.Previous2 => establishmentPerformance.WRITPROG_DESCR_Est_Previous2_Num_Coded,
            _ => CodedString.Empty
        };
        var writingBandingDescriptions = selectedYear switch
        {
            AcademicYearSelection.Previous2 => new ProgressBandingDescriptions(
                englandPerformance.ProgBand_Writ_Band1_Eng_Previous2_Desc,
                englandPerformance.ProgBand_Writ_Band2_Eng_Previous2_Desc,
                englandPerformance.ProgBand_Writ_Band3_Eng_Previous2_Desc,
                englandPerformance.ProgBand_Writ_Band4_Eng_Previous2_Desc,
                englandPerformance.ProgBand_Writ_Band5_Eng_Previous2_Desc),
            _ => ProgressBandingDescriptions.Empty
        };

        var establishmentMathsDescription = selectedYear switch
        {
            AcademicYearSelection.Previous2 => establishmentPerformance.MATPROG_DESCR_Est_Previous2_Num_Coded,
            _ => CodedString.Empty
        };
        var mathsBandingDescriptions = selectedYear switch
        {
            AcademicYearSelection.Previous2 => new ProgressBandingDescriptions(
                englandPerformance.ProgBand_Math_Band1_Eng_Previous2_Desc,
                englandPerformance.ProgBand_Math_Band2_Eng_Previous2_Desc,
                englandPerformance.ProgBand_Math_Band3_Eng_Previous2_Desc,
                englandPerformance.ProgBand_Math_Band4_Eng_Previous2_Desc,
                englandPerformance.ProgBand_Math_Band5_Eng_Previous2_Desc),
            _ => ProgressBandingDescriptions.Empty
        };

        var readingProgressBandingPercentages = selectedYear switch
        {

            AcademicYearSelection.Previous2 => new CodedDouble[]
            {
                englandPerformance.ProgBand_Read_Band1_Eng_Previous2_Pct_Coded,
                englandPerformance.ProgBand_Read_Band2_Eng_Previous2_Pct_Coded,
                englandPerformance.ProgBand_Read_Band3_Eng_Previous2_Pct_Coded,
                englandPerformance.ProgBand_Read_Band4_Eng_Previous2_Pct_Coded,
                englandPerformance.ProgBand_Read_Band5_Eng_Previous2_Pct_Coded
            },
            _ => []
        };
        var writingProgressBandingPercentages = selectedYear switch
        {

            AcademicYearSelection.Previous2 => new CodedDouble[]
            {
                englandPerformance.ProgBand_Writ_Band1_Eng_Previous2_Pct_Coded,
                englandPerformance.ProgBand_Writ_Band2_Eng_Previous2_Pct_Coded,
                englandPerformance.ProgBand_Writ_Band3_Eng_Previous2_Pct_Coded,
                englandPerformance.ProgBand_Writ_Band4_Eng_Previous2_Pct_Coded,
                englandPerformance.ProgBand_Writ_Band5_Eng_Previous2_Pct_Coded
            },
            _ => []
        };
        var mathsProgressBandingPercentages = selectedYear switch
        {

            AcademicYearSelection.Previous2 => new CodedDouble[]
            {
                englandPerformance.ProgBand_Math_Band1_Eng_Previous2_Pct_Coded,
                englandPerformance.ProgBand_Math_Band2_Eng_Previous2_Pct_Coded,
                englandPerformance.ProgBand_Math_Band3_Eng_Previous2_Pct_Coded,
                englandPerformance.ProgBand_Math_Band4_Eng_Previous2_Pct_Coded,
                englandPerformance.ProgBand_Math_Band5_Eng_Previous2_Pct_Coded
            },
            _ => []
        };


        return new KS2PupilPerformance
        {
            Urn = establishment.URN,
            EstablishmentReadingScore = selectedYear switch
            {
                AcademicYearSelection.Previous2 => establishmentPerformance.READPROG_Est_Previous2_Num_Coded,
                _ => CodedDouble.Empty
            },
            EstablishmentReadingDescription = establishmentReadingDescription,
            EstablishmentReadingConfidenceUpper = selectedYear switch
            {
                AcademicYearSelection.Previous2 => establishmentPerformance.READPROG_UPPER_Est_Previous2_Num_Coded,
                _ => CodedDouble.Empty
            },
            EstablishmentReadingConfidenceLower = selectedYear switch
            {
                AcademicYearSelection.Previous2 => establishmentPerformance.READPROG_LOWER_Est_Previous2_Num_Coded,
                _ => CodedDouble.Empty
            },
            LaReadingScore = selectedYear switch
            {
                AcademicYearSelection.Previous2 => laPerformance.READPROG_LA_Previous2_Num_Coded,
                _ => CodedDouble.Empty
            },
            EstablishmentReadingContextDescription = establishmentReadingDescription.GetBandingDescription(readingBandingDescriptions),
            EstablishmentWritingScore = selectedYear switch
            {
                AcademicYearSelection.Previous2 => establishmentPerformance.WRITPROG_Est_Previous2_Num_Coded,
                _ => CodedDouble.Empty
            },
            EstablishmentWritingDescription = establishmentWritingDescription,
            EstablishmentWritingConfidenceUpper = selectedYear switch
            {
                AcademicYearSelection.Previous2 => establishmentPerformance.WRITPROG_UPPER_Est_Previous2_Num_Coded,
                _ => CodedDouble.Empty
            },
            EstablishmentWritingConfidenceLower = selectedYear switch
            {
                AcademicYearSelection.Previous2 => establishmentPerformance.WRITPROG_LOWER_Est_Previous2_Num_Coded,
                _ => CodedDouble.Empty
            },
            LaWritingScore = selectedYear switch
            {
                AcademicYearSelection.Previous2 => laPerformance.WRITPROG_LA_Previous2_Num_Coded,
                _ => CodedDouble.Empty
            },
            EstablishmentWritingContextDescription = establishmentWritingDescription.GetBandingDescription(writingBandingDescriptions),
            EstablishmentMathsScore = selectedYear switch
            {
                AcademicYearSelection.Previous2 => establishmentPerformance.MATPROG_Est_Previous2_Num_Coded,
                _ => CodedDouble.Empty
            },
            EstablishmentMathsDescription = establishmentMathsDescription,
            EstablishmentMathsConfidenceUpper = selectedYear switch
            {
                AcademicYearSelection.Previous2 => establishmentPerformance.MATPROG_UPPER_Est_Previous2_Num_Coded,
                _ => CodedDouble.Empty
            },
            EstablishmentMathsConfidenceLower = selectedYear switch
            {
                AcademicYearSelection.Previous2 => establishmentPerformance.MATPROG_LOWER_Est_Previous2_Num_Coded,
                _ => CodedDouble.Empty
            },
            LaMathsScore = selectedYear switch
            {
                AcademicYearSelection.Previous2 => laPerformance.MATPROG_LA_Previous2_Num_Coded,
                _ => CodedDouble.Empty
            },
            EstablishmentMathsContextDescription = establishmentMathsDescription.GetBandingDescription(mathsBandingDescriptions),
            ReadingProgressBandingPercentages = readingProgressBandingPercentages,
            WritingProgressBandingPercentages = writingProgressBandingPercentages,
            MathsProgressBandingPercentages = mathsProgressBandingPercentages,
        };
    }

    public async Task<KS2PupilPerformanceBandings> GetPupilProgressAsync(string urn, CancellationToken ct = default)
    {
        var performance = await GetPerformanceAsync(urn, ct);

        if (performance is null)
        {
            return new KS2PupilPerformanceBandings { Urn = urn };
        }

        var (establishmentPerformance, laPerformance, englandPerformance) = performance.Value;

        return new KS2PupilPerformanceBandings
        {
            Urn = urn,
            EstablishmentReadingScore = establishmentPerformance?.READPROG_Est_Current_Num_Coded ?? CodedDouble.Empty,
            EstablishmentWritingScore = establishmentPerformance?.WRITPROG_Est_Current_Num_Coded ?? CodedDouble.Empty,
            EstablishmentMathsScore = establishmentPerformance?.MATPROG_Est_Current_Num_Coded ?? CodedDouble.Empty,
            EstablishmentReadingProgressBanding = establishmentPerformance?.READPROG_DESCR_Est_Current_Num_Coded,
            EstablishmentWritingProgressBanding = establishmentPerformance?.WRITPROG_DESCR_Est_Current_Num_Coded,
            EstablishmentMathsProgressBanding = establishmentPerformance?.MATPROG_DESCR_Est_Current_Num_Coded,
            EstablishmentReadingProgressSignificance = establishmentPerformance?.READPROG_SIGNIFICANCE_Est_Current_Num_Coded,
            EstablishmentWritingProgressSignificance = establishmentPerformance?.WRITPROG_SIGNIFICANCE_Est_Current_Num_Coded,
            EstablishmentMathsProgressSignificance = establishmentPerformance?.MATPROG_SIGNIFICANCE_Est_Current_Num_Coded,

            LocalAuthorityReadingScore = laPerformance?.READPROG_LA_Current_Num_Coded ?? CodedDouble.Empty,
            LocalAuthorityWritingScore = laPerformance?.WRITPROG_LA_Current_Num_Coded ?? CodedDouble.Empty,
            LocalAuthorityMathsScore = laPerformance?.MATPROG_LA_Current_Num_Coded ?? CodedDouble.Empty,

            EnglandAverageReadingScore = englandPerformance?.READPROG_Eng_Current_Num_Coded,
            EnglandAverageWritingScore = englandPerformance?.WRITPROG_Eng_Current_Num_Coded,
            EnglandAverageMathsScore = englandPerformance?.MATPROG_Eng_Current_Num_Coded
        };
    }

    public async Task<KS2PupilPerformanceBandings> GetDisadvantagedPupilProgressAsync(string urn, CancellationToken ct = default)
    {
        var performance = await GetPerformanceAsync(urn, ct);

        if (performance is null)
        {
            return new KS2PupilPerformanceBandings { Urn = urn };
        }

        var (establishmentPerformance, laPerformance, englandPerformance) = performance.Value;

        return new KS2PupilPerformanceBandings
        {
            Urn = urn,
            EstablishmentReadingScore = establishmentPerformance?.READPROG_FSM6CLA1A_Est_Current_Num_Coded ?? CodedDouble.Empty,
            EstablishmentWritingScore = establishmentPerformance?.WRITPROG_FSM6CLA1A_Est_Current_Num_Coded ?? CodedDouble.Empty,
            EstablishmentMathsScore = establishmentPerformance?.MATPROG_FSM6CLA1A_Est_Current_Num_Coded ?? CodedDouble.Empty,
            LocalAuthorityReadingScore = laPerformance?.READPROG_FSM6CLA1A_LA_Current_Num_Coded ?? CodedDouble.Empty,
            LocalAuthorityWritingScore = laPerformance?.WRITPROG_FSM6CLA1A_LA_Current_Num_Coded ?? CodedDouble.Empty,
            LocalAuthorityMathsScore = laPerformance?.MATPROG_FSM6CLA1A_LA_Current_Num_Coded ?? CodedDouble.Empty,
            EnglandAverageReadingScore = englandPerformance?.READPROG_FSM6CLA1A_Eng_Current_Num_Coded,
            EnglandAverageWritingScore = englandPerformance?.WRITPROG_FSM6CLA1A_Eng_Current_Num_Coded,
            EnglandAverageMathsScore = englandPerformance?.MATPROG_FSM6CLA1A_Eng_Current_Num_Coded
        };
    }

    /// <inheritdoc/>
    public async Task<KS2PupilPerformanceBandings> GetEalPupilProgressAsync(string urn, CancellationToken ct = default)
    {
        var performance = await GetPerformanceAsync(urn, ct);

        if (performance is null)
        {
            return new KS2PupilPerformanceBandings { Urn = urn };
        }

        var (establishmentPerformance, laPerformance, englandPerformance) = performance.Value;

        return new KS2PupilPerformanceBandings
        {
            Urn = urn,
            EstablishmentReadingScore = establishmentPerformance?.READPROG_EAL_Est_Current_Num_Coded ?? CodedDouble.Empty,
            EstablishmentWritingScore = establishmentPerformance?.WRITPROG_EAL_Est_Current_Num_Coded ?? CodedDouble.Empty,
            EstablishmentMathsScore = establishmentPerformance?.MATPROG_EAL_Est_Current_Num_Coded ?? CodedDouble.Empty,
            LocalAuthorityReadingScore = laPerformance?.READPROG_EAL_LA_Current_Num_Coded ?? CodedDouble.Empty,
            LocalAuthorityWritingScore = laPerformance?.WRITPROG_EAL_LA_Current_Num_Coded ?? CodedDouble.Empty,
            LocalAuthorityMathsScore = laPerformance?.MATPROG_EAL_LA_Current_Num_Coded ?? CodedDouble.Empty,
            EnglandAverageReadingScore = englandPerformance?.READPROG_EAL_Eng_Current_Num_Coded,
            EnglandAverageWritingScore = englandPerformance?.WRITPROG_EAL_Eng_Current_Num_Coded,
            EnglandAverageMathsScore = englandPerformance?.MATPROG_EAL_Eng_Current_Num_Coded
        };
    }

    public async Task<KS2PupilPerformanceBandings[]> GetGirlBoyPupilProgressAsync(string urn, CancellationToken ct = default)
    {
        var performance = await GetPerformanceAsync(urn, ct);

        if (performance is null)
        {
            return [new() { Urn = urn }];
        }

        var (establishmentPerformance, laPerformance, englandPerformance) = performance.Value;

        return [
            new ()
            {
                Urn = urn,
                CharacteristicName = "Girls",
                EstablishmentReadingScore = establishmentPerformance?.READPROG_G_Est_Current_Num_Coded ?? CodedDouble.Empty,
                EstablishmentWritingScore = establishmentPerformance?.WRITPROG_G_Est_Current_Num_Coded ?? CodedDouble.Empty,
                EstablishmentMathsScore = establishmentPerformance?.MATPROG_G_Est_Current_Num_Coded ?? CodedDouble.Empty,
                LocalAuthorityReadingScore = laPerformance?.READPROG_G_LA_Current_Num_Coded ?? CodedDouble.Empty,
                LocalAuthorityWritingScore = laPerformance?.WRITPROG_G_LA_Current_Num_Coded ?? CodedDouble.Empty,
                LocalAuthorityMathsScore = laPerformance?.MATPROG_G_LA_Current_Num_Coded ?? CodedDouble.Empty,
                EnglandAverageReadingScore = englandPerformance?.READPROG_G_Eng_Current_Num_Coded,
                EnglandAverageWritingScore = englandPerformance?.WRITPROG_G_Eng_Current_Num_Coded,
                EnglandAverageMathsScore = englandPerformance?.MATPROG_G_Eng_Current_Num_Coded
            },
            new()
            {
                Urn = urn,
                CharacteristicName = "Boys",
                EstablishmentReadingScore = establishmentPerformance?.READPROG_B_Est_Current_Num_Coded ?? CodedDouble.Empty,
                EstablishmentWritingScore = establishmentPerformance?.WRITPROG_B_Est_Current_Num_Coded ?? CodedDouble.Empty,
                EstablishmentMathsScore = establishmentPerformance?.MATPROG_B_Est_Current_Num_Coded ?? CodedDouble.Empty,
                LocalAuthorityReadingScore = laPerformance?.READPROG_B_LA_Current_Num_Coded ?? CodedDouble.Empty,
                LocalAuthorityWritingScore = laPerformance?.WRITPROG_B_LA_Current_Num_Coded ?? CodedDouble.Empty,
                LocalAuthorityMathsScore = laPerformance?.MATPROG_B_LA_Current_Num_Coded ?? CodedDouble.Empty,
                EnglandAverageReadingScore = englandPerformance?.READPROG_B_Eng_Current_Num_Coded,
                EnglandAverageWritingScore = englandPerformance?.WRITPROG_B_Eng_Current_Num_Coded,
                EnglandAverageMathsScore = englandPerformance?.MATPROG_B_Eng_Current_Num_Coded
            }
        ];
    }

    private async Task<(KS2EstablishmentPerformance? Establishment, KS2LAPerformance? LocalAuthority, KS2EnglandPerformance? England)?> GetPerformanceAsync(string urn, CancellationToken ct)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(urn);
        ct.ThrowIfCancellationRequested();

        var establishment = await establishmentService.GetEstablishmentMinimumAsync(urn, ct);

        if (string.IsNullOrWhiteSpace(establishment?.URN))
        {
            return null;
        }

        var ks2EstablishPerformanceTask = ks2PerformanceRepository.GetEstablishmentPerformanceAsync(urn, ct);
        var ks2LAPerformanceTask = ks2PerformanceRepository.GetLaPerformanceAsync(establishment.LAId, ct);
        var ks2EnglandPerformanceTask = ks2PerformanceRepository.GetEnglandPerformanceAsync(ct);

        await Task.WhenAll(ks2EstablishPerformanceTask, ks2LAPerformanceTask, ks2EnglandPerformanceTask);

        return (await ks2EstablishPerformanceTask, await ks2LAPerformanceTask, await ks2EnglandPerformanceTask);
    }


}