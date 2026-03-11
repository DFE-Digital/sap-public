using SAPPub.Core.Entities.KS4.Performance;

namespace SAPPub.Core.Interfaces.Services.KS4.Performance
{
    public interface IEstablishmentPerformanceService
    {
        Task<IEnumerable<EstablishmentPerformance>> GetAllEstablishmentPerformanceAsync(CancellationToken ct = default);
        Task<EstablishmentPerformance> GetEstablishmentPerformanceAsync(string urn, CancellationToken ct = default);
    }
}
