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
    public async Task<AdditionalMeasuresModel> GetAsync(string urn, string lAId, CancellationToken ct = default)
    {
        var establishmentServiceTask = establishmentService.GetEstablishmentAsync(urn, ct);
        var establishmentPerformanceTask = establishmentPerformanceService.GetEstablishmentPerformanceAsync(urn, ct);
        var englandPerformanceTask = englandPerformanceService.GetEnglandPerformanceAsync(ct);
        var laPerformanceTask = lAPerformanceService.GetLAPerformanceAsync(lAId, ct);

        await Task.WhenAll(establishmentServiceTask, establishmentPerformanceTask, laPerformanceTask, englandPerformanceTask);

        var additionalMeasuresModel = AdditionalMeasuresModel.Map(
            await establishmentServiceTask,
            await establishmentPerformanceTask,
            await laPerformanceTask,
            await englandPerformanceTask);

        return additionalMeasuresModel;
    }
}
