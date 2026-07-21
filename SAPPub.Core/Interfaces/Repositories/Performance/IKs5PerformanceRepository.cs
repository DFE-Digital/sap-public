using SAPPub.Core.Entities.Performance;

namespace SAPPub.Core.Interfaces.Repositories.Performance;

public interface IKs5PerformanceRepository
{
    Task<EnglandKs5Performance> GetEnglandPerformanceAsync(CancellationToken ct = default);

    Task<EstablishmentKs5Performance> GetEstablishmentPerformanceAsync(
        string urn,
        CancellationToken ct = default);

    Task<LAKs5Performance> GetLaPerformanceAsync(
        string laCode,
        CancellationToken ct = default);
}
