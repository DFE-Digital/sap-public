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
    public Task<AdditionalMeasuresModel> GetAsync(string urn, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
