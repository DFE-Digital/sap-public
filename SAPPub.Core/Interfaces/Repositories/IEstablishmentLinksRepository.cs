using SAPPub.Core.Entities;

namespace SAPPub.Core.Interfaces.Repositories;

public interface IEstablishmentLinksRepository
{
    public Task<List<EstablishmentLinks>?> GetLinksAsync(string urn, CancellationToken ct = default);
}
