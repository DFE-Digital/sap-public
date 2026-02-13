using SAPPub.Core.Entities.KS4.Performance;

namespace SAPPub.Core.Interfaces.Repositories.KS4.Performance
{
    public interface ILAPerformanceRepository
    {
        Task<IEnumerable<LAPerformance>> GetAllLAPerformanceAsync(CancellationToken ct = default);
        Task<LAPerformance> GetLAPerformanceAsync(string laCode, CancellationToken ct = default);
    }
}
