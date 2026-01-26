using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Repositories.KS4.Performance;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.ServiceModels.KS4.Performance;

namespace SAPPub.Core.Services.KS4.Performance;

public class EnglishAndMathsResultsService(
    IEstablishmentRepository establishmentRepository,
    IEnglandPerformanceRepository englandPerformanceRepository,
    IEstablishmentPerformanceRepository establishmentPerformanceRepository,
    ILAPerformanceRepository laPerformanceRepository
    ) : IAcademicPerformanceEnglishAndMathsResultsService
{
    public EnglishAndMathsResultsServiceModel ResultsOfSpecifiedGradeAndAbove(string urn, int selectedGrade)
    {
        var establishmentPerformance = establishmentPerformanceRepository.GetEstablishmentPerformance(urn);
        var establishment = establishmentRepository.GetEstablishment(urn);
        var laCode = establishment?.LAId;
        var laPerformance = laCode == null ? null : laPerformanceRepository.GetLAPerformance(laCode);
        var englandPerformance = englandPerformanceRepository.GetEnglandPerformance();
        return new()
        {
            LAName = establishment?.LAName,
            EnglandAverage = selectedGrade switch
            {
                4 => englandPerformance?.EngMaths49_Tot_Eng_Current_Pct,
                5 => englandPerformance?.EngMaths59_Tot_Eng_Current_Pct,
                _ => null
            },
            LocalAuthorityAverage = selectedGrade switch
            {
                4 => laPerformance?.EngMaths49_Tot_LA_Current_Pct,
                5 => laPerformance?.EngMaths59_Tot_LA_Current_Pct,
                _ => null
            },
            EstablishmentResult = selectedGrade switch
            {
                4 => establishmentPerformance?.EngMaths49_Tot_Est_Current_Pct,
                5 => establishmentPerformance?.EngMaths59_Tot_Est_Current_Pct,
                _ => null
            }
        };
    }
}
