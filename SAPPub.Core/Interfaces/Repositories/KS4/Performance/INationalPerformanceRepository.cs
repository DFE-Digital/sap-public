using SAPPub.Core.Entities.KS4.Performance;

namespace SAPPub.Core.Interfaces.Repositories.KS4.Performance
{
    public interface IEnglandPerformanceRepository
    {
        IEnumerable<EnglandPerformance> GetAllEnglandPerformance();
        EnglandPerformance? GetEnglandPerformance();
    }
}
