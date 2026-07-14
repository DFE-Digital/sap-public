using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.ServiceModels.KS4.Performance;

namespace SAPPub.Core.Services.KS4.Performance;

public class AdditionalMeasuresService(
    IEstablishmentPerformanceService establishmentPerformanceService,
    ILAPerformanceService lAPerformanceService,
    IEnglandPerformanceService englandPerformanceService) : IAdditionalMeasuresService
{
    public async Task<AdditionalMeasuresModel> GetAsync(string urn, string lAId, CancellationToken ct = default)
    {
        var establishmentPerformanceTask = establishmentPerformanceService.GetEstablishmentPerformanceAsync(urn, ct);
        var englandPerformanceTask = englandPerformanceService.GetEnglandPerformanceAsync(ct);

        var laPerformanceTask = lAPerformanceService.GetLAPerformanceAsync(lAId, ct);

        await Task.WhenAll(establishmentPerformanceTask, laPerformanceTask, englandPerformanceTask);

        var additionalMeasuresModel = AdditionalMeasuresModel.Map(
            await establishmentPerformanceTask,
            await laPerformanceTask,
            await englandPerformanceTask);

        return additionalMeasuresModel;
    }
}
