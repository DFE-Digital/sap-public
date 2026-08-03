using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Core.ServiceModels.Performance;

namespace SAPPub.Core.Interfaces.Services.Performance;

public interface IKS2ScaledScoreService
{
    Task<KS2ScaledScoreModel> GetScaledScoreModel(
        string urn,
        CancellationToken ct = default);
}
