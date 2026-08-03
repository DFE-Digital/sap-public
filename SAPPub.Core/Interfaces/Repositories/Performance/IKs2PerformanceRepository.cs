using SAPPub.Core.Entities.Performance;

namespace SAPPub.Core.Interfaces.Repositories.Performance;

public interface IKs2PerformanceRepository
{
    Task<KS2EnglandPerformance> GetEnglandPerformanceAsync(CancellationToken ct = default);

    Task<KS2EstablishmentPerformance> GetEstablishmentPerformanceAsync(
        string urn,
        CancellationToken ct = default);

    Task<KS2LAPerformance> GetLaPerformanceAsync(
        string laCode,
        CancellationToken ct = default);
}
