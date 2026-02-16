using SAPPub.Core.Entities.KS4.Performance;

namespace SAPPub.Core.Interfaces.Services.KS4.Performance
{
    public interface IEnglandPerformanceService
    {
        Task<EnglandPerformance> GetEnglandPerformanceAsync(CancellationToken ct = default);
    }
}
