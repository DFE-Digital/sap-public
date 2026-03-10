using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.ServiceModels.KS4.Performance;

namespace SAPPub.Core.Services.KS4.Performance;

public class AttainmentAndProgressService(
    IEstablishmentService establishmentService,
    IEstablishmentPerformanceService establishmentPerformanceService,
    ILAPerformanceService lAPerformanceService,
    IEnglandPerformanceService englandPerformanceService) : IAttainmentAndProgressService
{
    public async Task<AttainmentAndProgressModel> GetAttainmentAndProgressAsync(
        string urn,
        AcademicYearSelection selectedYear,
        CancellationToken ct = default)
    {
        // Need establishment first to get LAId/LAName (and to check if URN is valid)
        var establishment = await establishmentService.GetEstablishmentAsync(urn, ct);

        if (string.IsNullOrWhiteSpace(establishment.URN))
            return new AttainmentAndProgressModel { Urn = urn };

        // Now we can run the remaining calls concurrently
        var establishmentPerformance = await establishmentPerformanceService.GetEstablishmentPerformanceAsync(urn, ct);

        var laId = establishment.LAId ?? string.Empty;
        var laPerformance = await lAPerformanceService.GetLAPerformanceAsync(laId, ct);

        var englandPerformance = await englandPerformanceService.GetEnglandPerformanceAsync(ct);

        return new AttainmentAndProgressModel
        {
            Urn = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            EstablishmentProgress8Score = selectedYear switch
            {
                AcademicYearSelection.Previous => establishmentPerformance.Prog8_Tot_Est_Previous_Num,
                AcademicYearSelection.Previous2 => establishmentPerformance.Prog8_Tot_Est_Previous2_Num,
                _ => null,
            },
            LocalAuthorityProgress8Score = selectedYear switch
            {
                AcademicYearSelection.Previous => laPerformance.Prog8_Avg_LA_Previous_Num,
                AcademicYearSelection.Previous2 => laPerformance.Prog8_Avg_LA_Previous2_Num,
                _ => null,
            },
            EstablishmentAttainment8Score = selectedYear switch
            {
                AcademicYearSelection.Current => establishmentPerformance.Attainment8_Tot_Est_Current_Num,
                AcademicYearSelection.Previous => establishmentPerformance.Attainment8_Tot_Est_Previous_Num,
                AcademicYearSelection.Previous2 => establishmentPerformance.Attainment8_Tot_Est_Previous2_Num,
                _ => null,
            },
            LocalAuthorityAttainment8Score = selectedYear switch
            {
                AcademicYearSelection.Current => laPerformance.Attainment8_Tot_LA_Current_Num,
                AcademicYearSelection.Previous => laPerformance.Attainment8_Tot_LA_Previous_Num,
                AcademicYearSelection.Previous2 => laPerformance.Attainment8_Tot_LA_Previous2_Num,
                _ => null,
            },
            EnglandAttainment8Score = selectedYear switch
            {
                AcademicYearSelection.Current => englandPerformance.Attainment8_Tot_Eng_Current_Num,
                AcademicYearSelection.Previous => englandPerformance.Attainment8_Tot_Eng_Previous_Num,
                AcademicYearSelection.Previous2 => englandPerformance.Attainment8_Tot_Eng_Previous2_Num,
                _ => null,
            },
            EstablishmentProgress8TotalPupils = selectedYear switch
            {
                AcademicYearSelection.Previous => establishmentPerformance.Prog8_TotPup_Est_Previous_Num,
                AcademicYearSelection.Previous2 => establishmentPerformance.Prog8_TotPup_Est_Previous2_Num,
                _ => null,
            },
            EstablishmentTotalPupils = selectedYear switch
            {                
                AcademicYearSelection.Previous => establishmentPerformance.Pup_Tot_Est_Previous_Num,
                AcademicYearSelection.Previous2 => establishmentPerformance.Pup_Tot_Est_Previous2_Num,
                _ => null,
            },
        };
    }
}
