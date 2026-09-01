using SAPPub.Core.ServiceModels.Performance;

namespace SAPPub.Core.Interfaces.Services.Performance;

public interface IKS2AdditionalMeasuresService
{
    Task<KS2AdditionalMeasuresModel> GetAdditionalMeasures(string urn, string laId, CancellationToken ct = default);
}
