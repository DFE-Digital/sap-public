using SAPPub.Core.Entities.Destinations;
using SAPPub.Core.Interfaces.Repositories.Destinations;
using SAPPub.Core.Interfaces.Repositories.Generic;

namespace SAPPub.Infrastructure.Repositories.Destinations;

public class KS5DestinationsRepository(
    IGenericRepository<KS5EnglandDestinations> _englandRepo,
    IGenericRepository<KS5EstablishmentDestinations> _establishmentRepo,
    IGenericRepository<KS5LADestinations> _laRepo) : IKS5DestinationsRepository
{
    public async Task<KS5EstablishmentDestinations?> GetEstablishmentDestinationsAsync(string urn, CancellationToken ct = default)
    {
        // v_england_destinations returns a single row
        if (string.IsNullOrWhiteSpace(urn))
        {
            return null;
        }
        ct.ThrowIfCancellationRequested();

        return await _establishmentRepo.ReadAsync(urn, ct);
    }

    public async Task<KS5EnglandDestinations> GetEnglandDestinationsAsync(CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        return await _englandRepo.ReadSingleAsync(new { }, ct) ?? new KS5EnglandDestinations();
    }


    public async Task<KS5LADestinations> GetLADestinationsAsync(string laCode, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(laCode))
        {
            return new KS5LADestinations();
        }

        ct.ThrowIfCancellationRequested();

        return await _laRepo.ReadAsync(laCode, ct) ?? new KS5LADestinations();
    }
}
