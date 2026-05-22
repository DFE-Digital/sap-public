using SAPPub.Core.Entities;

namespace SAPPub.Core.Interfaces.Services
{
    public interface IEstablishmentService
    {
        Task<IEnumerable<Establishment>> GetEstablishmentsAsync(int page, int take, CancellationToken ct = default);

        Task<Establishment> GetEstablishmentAsync(string urn, CancellationToken ct = default);

        Task<(IEnumerable<Establishment> Results, int TotalCount)> SearchByNameAsync(string searchTerm, int page, int pageSize, CancellationToken ct = default);

        Task<(IEnumerable<Establishment> Results, int TotalCount)> SearchByNameAndLocationAsync(string searchTerm, double latitude, double longitude, double distance, int page, int pageSize, CancellationToken ct = default);

        Task<(IEnumerable<Establishment> Results, int TotalCount)> SearchByLocationAsync(double latitude, double longitude, double distance, int page, int pageSize, CancellationToken ct = default);
    }
}
