using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.ServiceModels.Compare;

namespace SAPPub.Core.Services.KS4.Performance;

public class AttainmentAndProgressComparisionService(
    IEstablishmentPerformanceService establishmentPerformanceService,
    IEnglandPerformanceService englandPerformanceService) : IAttainmentAndProgressComparisionService
{
    public async Task<AttainmentAndProgressComparisonResultsModel> GetComparisionResultsAsync(IEnumerable<string> urns, CancellationToken ct = default)
    {
        var establishmentsPerformanceDetailsTask = establishmentPerformanceService.GetEstablishmentsPerformanceAsync(urns, ct);
        var englandPerformanceDetailsTask = englandPerformanceService.GetEnglandPerformanceAsync(ct);

        await Task.WhenAll(establishmentsPerformanceDetailsTask, englandPerformanceDetailsTask);

        var establishmentsPerformance = await establishmentsPerformanceDetailsTask;
        var englandPerformance = await englandPerformanceDetailsTask;

        var schoolDetails = urns.Select(urn => new SchoolAttainmentAndProgressDetails
        {
            Urn = urn,
            Attainment8Score = establishmentsPerformance.FirstOrDefault(x => x.Id == urn)?.Attainment8_Tot_Est_Previous_Num
        });

        return new AttainmentAndProgressComparisonResultsModel
        {
            SchoolDetails = schoolDetails,
            EnglandAverage = englandPerformance?.Attainment8_Tot_Eng_Previous_Num
        };
    }
}
