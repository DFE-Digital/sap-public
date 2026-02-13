using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.ServiceModels.KS4.Performance;

namespace SAPPub.Core.Services.KS4.Performance;

public class EnglishAndMathsResultsService(
    IEstablishmentService establishmentService,
    IEstablishmentPerformanceService establishmentPerformanceService,
    ILAPerformanceService lAPerformanceService,
    IEnglandPerformanceService englandPerformanceService
    ) : IAcademicPerformanceEnglishAndMathsResultsService
{
    public EnglishAndMathsResultsModel GetEnglishAndMathsResults(string urn, int selectedGrade)
    {
        var establishment = establishmentService.GetEstablishment(urn);
        var establishmentPerformance = establishmentPerformanceService.GetEstablishmentPerformance(urn);
        var laPerformance = lAPerformanceService.GetLAPerformance(establishment.LAId);
        var englandPerformance = englandPerformanceService.GetEnglandPerformance();

        return new()
        {
            Urn = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            LAName = establishment.LAName,
            EstablishmentAll = new RelativeYearValues<double?>
            {
                CurrentYear = selectedGrade switch { 4 => establishmentPerformance.EngMaths49_Tot_Est_Current_Pct, 5 => establishmentPerformance.EngMaths59_Tot_Est_Current_Pct, _ => null },
                PreviousYear = selectedGrade switch { 4 => establishmentPerformance.EngMaths49_Tot_Est_Previous_Pct, 5 => establishmentPerformance.EngMaths59_Tot_Est_Previous_Pct, _ => null },
                TwoYearsAgo = selectedGrade switch { 4 => establishmentPerformance.EngMaths49_Tot_Est_Previous2_Pct, 5 => establishmentPerformance.EngMaths59_Tot_Est_Previous2_Pct, _ => null },
            },
            LocalAuthorityAll = new RelativeYearValues<double?>
            {
                CurrentYear = selectedGrade switch { 4 => laPerformance.EngMaths49_Tot_LA_Current_Pct, 5 => laPerformance.EngMaths59_Tot_LA_Current_Pct, _ => null },
                PreviousYear = selectedGrade switch { 4 => laPerformance.EngMaths49_Tot_LA_Previous_Pct, 5 => laPerformance.EngMaths59_Tot_LA_Previous_Pct, _ => null },
                TwoYearsAgo = selectedGrade switch { 4 => laPerformance.EngMaths49_Tot_LA_Previous2_Pct, 5 => laPerformance.EngMaths59_Tot_LA_Previous2_Pct, _ => null },
            },
            EnglandAll = new RelativeYearValues<double?>
            {
                CurrentYear = selectedGrade switch { 4 => englandPerformance.EngMaths49_Tot_Eng_Current_Pct, 5 => englandPerformance.EngMaths59_Tot_Eng_Current_Pct, _ => null },
                PreviousYear = selectedGrade switch { 4 => englandPerformance.EngMaths49_Tot_Eng_Previous_Pct, 5 => englandPerformance.EngMaths59_Tot_Eng_Previous_Pct, _ => null },
                TwoYearsAgo = selectedGrade switch { 4 => englandPerformance.EngMaths49_Tot_Eng_Previous2_Pct, 5 => englandPerformance.EngMaths59_Tot_Eng_Previous2_Pct, _ => null },
            },
            EstablishmentBoys = new RelativeYearValues<double?>
            {
                CurrentYear = selectedGrade switch { 4 => establishmentPerformance.EngMaths49_Boy_Est_Current_Pct, 5 => establishmentPerformance.EngMaths59_Boy_Est_Current_Pct, _ => null },
            },
            LocalAuthorityBoys = new RelativeYearValues<double?>
            {
                CurrentYear = selectedGrade switch { 4 => laPerformance.EngMaths49_Boy_LA_Current_Pct, 5 => laPerformance.EngMaths59_Boy_LA_Current_Pct, _ => null },
            },
            EnglandBoys = new RelativeYearValues<double?>
            {
                CurrentYear = selectedGrade switch { 4 => englandPerformance.EngMaths49_Boy_Eng_Current_Pct, 5 => englandPerformance.EngMaths59_Boy_Eng_Current_Pct, _ => null },
            },
            EstablishmentGirls = new RelativeYearValues<double?>
            {
                CurrentYear = selectedGrade switch { 4 => establishmentPerformance.EngMaths49_Grl_Est_Current_Pct, 5 => establishmentPerformance.EngMaths59_Grl_Est_Current_Pct, _ => null },
            },
            LocalAuthorityGirls = new RelativeYearValues<double?>
            {
                CurrentYear = selectedGrade switch { 4 => laPerformance.EngMaths49_Grl_LA_Current_Pct, 5 => laPerformance.EngMaths59_Grl_LA_Current_Pct, _ => null },
            },
            EnglandGirls = new RelativeYearValues<double?>
            {
                CurrentYear = selectedGrade switch { 4 => englandPerformance.EngMaths49_Grl_Eng_Current_Pct, 5 => englandPerformance.EngMaths59_Grl_Eng_Current_Pct, _ => null },
            },
        };
    }
}
