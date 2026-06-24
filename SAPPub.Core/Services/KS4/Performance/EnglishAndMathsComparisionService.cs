using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.ServiceModels.KS4.Performance;

namespace SAPPub.Core.Services.KS4.Performance;

public class EnglishAndMathsComparisionService(
    IEstablishmentService establishmentService,
    IEstablishmentPerformanceService establishmentPerformanceService,
    IEnglandPerformanceService englandPerformanceService
    ) : IEnglishAndMathsComparisionService
{
    public async Task<EnglishAndMathsComparisionResultsModel> GetComparisionResultsAsync(
        IEnumerable<string> urns,
        CancellationToken ct = default)
    {
        var establishments = await establishmentService.GetEstablishmentsAsync(urns, ct);
        var establishmentsPerformanceTask = establishmentPerformanceService.GetEstablishmentsPerformanceAsync(urns, ct);
        var englandPerformanceTask = englandPerformanceService.GetEnglandPerformanceAsync(ct);

        await Task.WhenAll(establishmentsPerformanceTask, englandPerformanceTask);

        var establishmentsPerformance = await establishmentsPerformanceTask;
        var englandPerformance = await englandPerformanceTask;

        var establishmentResults = (from establishment in establishments
                                    let establishmentPerformance = establishmentsPerformance.FirstOrDefault(p => p.Id == establishment.URN)
                                    let establishmentResultModel = new EnglishAndMathsComparisionResultModel
                                    {
                                        Urn = establishment.URN,
                                        SchoolName = establishment.EstablishmentName,
                                        EstablishmentData = new RelativeYearValues<double?>
                                        {
                                            CurrentYear = establishmentPerformance?.EngMaths59_Tot_Est_Current_Pct,
                                            PreviousYear = establishmentPerformance?.EngMaths59_Tot_Est_Previous_Pct,
                                            TwoYearsAgo = establishmentPerformance?.EngMaths59_Tot_Est_Previous2_Pct,
                                        },
                                    }
                                    select establishmentResultModel).ToList();

        return new EnglishAndMathsComparisionResultsModel
        {
            Establishments = establishmentResults,
            EnglandAverage = new RelativeYearValues<double?>
            {
                CurrentYear = englandPerformance?.EngMaths59_Tot_Eng_Current_Pct
            },
        };
    }
}
