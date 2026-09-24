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
            EstablishmentMathsContextDescription = establishmentMathsDescription.GetBandingDescription(mathsBandingDescriptions)
        };
    }
}