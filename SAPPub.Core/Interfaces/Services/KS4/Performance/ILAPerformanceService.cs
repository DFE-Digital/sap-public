using SAPPub.Core.Entities.KS4.Performance;

namespace SAPPub.Core.Interfaces.Services.KS4.Performance
{
    public interface ILAPerformanceService
    {
        Task<IEnumerable<LAPerformance>> GetAllLAPerformanceAsync(CancellationToken ct = default);
        Task<LAPerformance> GetLAPerformanceAsync(string laCode, CancellationToken ct = default);
    }
}
