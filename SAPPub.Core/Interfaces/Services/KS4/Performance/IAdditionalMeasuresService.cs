using SAPPub.Core.ServiceModels.KS4.Performance;

namespace SAPPub.Core.Interfaces.Services.KS4.Performance;

public interface IAdditionalMeasuresService
{
    Task<AdditionalMeasuresModel> GetAsync(string urn, CancellationToken ct = default);
}
