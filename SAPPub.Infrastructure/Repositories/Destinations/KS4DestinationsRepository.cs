using SAPPub.Core.Entities.Destinations;
using SAPPub.Core.Interfaces.Repositories.Destinations;
using SAPPub.Core.Interfaces.Repositories.Generic;

namespace SAPPub.Infrastructure.Repositories.Destinations;

public class KS4DestinationsRepository(
    IGenericRepository<KS4EnglandDestinations> _englandRepo,
    IGenericRepository<KS4EstablishmentDestinations> _establishmentRepo,
    IGenericRepository<KS4LADestinations> _laRepo) : IKS4DestinationsRepository
{
    public async Task<KS4EnglandDestinations> GetEnglandDestinationsAsync(CancellationToken ct = default)
    {
        // v_england_destinations returns a single row
        return await _englandRepo.ReadSingleAsync(new { }, ct) ?? new KS4EnglandDestinations();
    }

    public Task<IEnumerable<KS4EstablishmentDestinations>> GetAllEstablishmentDestinationsAsync(CancellationToken ct = default)
    {
        return _establishmentRepo.ReadAllAsync(ct);
    }

    public async Task<KS4EstablishmentDestinations?> GetEstablishmentDestinationsAsync(string urn, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(urn))
            return null;

        return await _establishmentRepo.ReadAsync(urn, ct);
    }

    public async Task<IEnumerable<KS4EstablishmentDestinations>> GetEstablishmentsDestinationsAsync(
        IEnumerable<string> urns,
        CancellationToken ct = default)
    {
        if (urns is null || !urns.Any())
            return [];

        return await _establishmentRepo.ReadManyAsync(new { Ids = urns }, ct) ?? [];
    }

    public async Task<IEnumerable<KS4LADestinations>> GetAllLADestinationsAsync(CancellationToken ct = default)
    {
        return await _laRepo.ReadAllAsync(ct);
    }

    public async Task<KS4LADestinations> GetLADestinationsAsync(string laCode, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(laCode))
            return new KS4LADestinations();

        return await _laRepo.ReadAsync(laCode, ct) ?? new KS4LADestinations();
    }
}
