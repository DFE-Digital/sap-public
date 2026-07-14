using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.ServiceModels.KS4.Performance;

namespace SAPPub.Core.Services.KS4.Performance;

public class AdditionalMeasuresService(
    IEstablishmentService establishmentService,
    IEstablishmentPerformanceService establishmentPerformanceService,
    ILAPerformanceService lAPerformanceService,
    IEnglandPerformanceService englandPerformanceService) : IAdditionalMeasuresService
{
    public async Task<AdditionalMeasuresModel> GetAsync(string urn, CancellationToken ct = default)
    {
        var establishmentTask = establishmentService.GetEstablishmentAsync(urn, ct);
        var establishmentPerformanceTask = establishmentPerformanceService.GetEstablishmentPerformanceAsync(urn, ct);
        var englandPerformanceTask = englandPerformanceService.GetEnglandPerformanceAsync(ct);

        var establishment = await establishmentTask;
        var laPerformanceTask = lAPerformanceService.GetLAPerformanceAsync(establishment.LAId, ct);

        await Task.WhenAll(establishmentPerformanceTask, laPerformanceTask, englandPerformanceTask);

        var additionalMeasuresModel = AdditionalMeasuresModel.Map(
            establishmentTask.Result,
            await establishmentPerformanceTask,
            await laPerformanceTask,
            await englandPerformanceTask);

        return additionalMeasuresModel;
    }
}
