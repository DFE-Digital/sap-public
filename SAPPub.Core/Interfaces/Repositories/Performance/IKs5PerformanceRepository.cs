using SAPPub.Core.Entities.Performance;

namespace SAPPub.Core.Interfaces.Repositories.Performance;

public interface IKs5PerformanceRepository
{
    Task<KS5England5Performance> GetEnglandPerformanceAsync(CancellationToken ct = default);

    Task<KS5EstablishmentPerformance> GetEstablishmentPerformanceAsync(
        string urn,
        CancellationToken ct = default);

    Task<KS5LAPerformance> GetLaPerformanceAsync(
        string laCode,
        CancellationToken ct = default);
}
