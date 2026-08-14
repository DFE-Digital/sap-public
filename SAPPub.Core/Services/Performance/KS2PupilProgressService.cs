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

        await Task.WhenAll(ks2EstablishPerformanceTask, ks2LAPerformanceTask);

        var establishmentPerformance = await ks2EstablishPerformanceTask;
        var laPerformance = await ks2LAPerformanceTask;

        return new KS2PupilPerformance
        {
            Urn = establishment.URN,
            EstablishmentReadingScore = selectedYear switch
            {
                AcademicYearSelection.Previous2 => establishmentPerformance.READPROG_Est_Previous2_Num_Coded,
                _ => CodedDouble.Empty
            },
            EstablishmentReadingDescription = selectedYear switch
            { 
                AcademicYearSelection.Previous2 => establishmentPerformance.READPROG_DESCR_Est_Previous2_Num_Coded,
                _ => CodedString.Empty
            },
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
            EstablishmentWritingScore = selectedYear switch
            {
                AcademicYearSelection.Previous2 => establishmentPerformance.WRITPROG_Est_Previous2_Num_Coded,
                _ => CodedDouble.Empty
            },
            EstablishmentWritingDescription = selectedYear switch
            {
                AcademicYearSelection.Previous2 => establishmentPerformance.WRITPROG_DESCR_Est_Previous2_Num_Coded,
                _ => CodedString.Empty
            },
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
            EstablishmentMathsScore = selectedYear switch
            {
                AcademicYearSelection.Previous2 => establishmentPerformance.MATPROG_Est_Previous2_Num_Coded,
                _ => CodedDouble.Empty
            },
            EstablishmentMathsDescription = selectedYear switch
            {
                AcademicYearSelection.Previous2 => establishmentPerformance.MATPROG_DESCR_Est_Previous2_Num_Coded,
                _ => CodedString.Empty
            },
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
            }
        };
    }
}