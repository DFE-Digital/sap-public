using SAPPub.Core.Entities.KS4.Performance;

namespace SAPPub.Core.Interfaces.Repositories.KS4.Performance
{
    public interface IEnglandPerformanceRepository
    {
        Task<IEnumerable<EnglandPerformance>> GetAllEnglandPerformanceAsync(CancellationToken ct = default);
        Task<EnglandPerformance> GetEnglandPerformanceAsync(CancellationToken ct = default);
    }
}
