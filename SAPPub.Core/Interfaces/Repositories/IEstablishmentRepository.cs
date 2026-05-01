using SAPPub.Core.Entities;

namespace SAPPub.Core.Interfaces.Repositories
{
    public interface IEstablishmentRepository
    {
        Task<IEnumerable<Establishment>> GetAllEstablishmentsAsync(CancellationToken ct = default);
        Task<IEnumerable<Establishment>> GetEstablishmentsAsync(int page, int take, CancellationToken ct = default);
        Task<Establishment?> GetEstablishmentAsync(string urn, CancellationToken ct = default);
        Task<IEnumerable<Establishment>> SearchByNameAsync(string searchTerm, int limit = 20, CancellationToken ct = default);
        Task<IEnumerable<Establishment>> SearchByNameAndLocationAsync(string searchTerm, double latitude, double longitude, double distance, int limit = 20, CancellationToken ct = default);
        Task<IEnumerable<Establishment>> SearchByLocationAsync(double latitude, double longitude, double distance, int limit = 20, CancellationToken ct = default);
    }
}
