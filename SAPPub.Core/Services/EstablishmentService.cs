using Microsoft.Extensions.Caching.Memory;
using SAPPub.Core.Entities;
using SAPPub.Core.Exceptions;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;

namespace SAPPub.Core.Services;

public sealed class EstablishmentService(
    IEstablishmentRepository establishmentRepository,
    IMemoryCache memoryCache
    ) : IEstablishmentService
{
    private readonly IEstablishmentRepository _establishmentRepository = establishmentRepository ?? throw new ArgumentNullException(nameof(establishmentRepository));
    private readonly IMemoryCache _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(establishmentRepository));

    public async Task<IEnumerable<EstablishmentServiceModel>> GetEstablishmentsAsync(int page, int take, CancellationToken ct = default)
    {
        var establishments = await _establishmentRepository.GetEstablishmentsAsync(page, take, ct);
        return establishments.Select(e => Establishment.MapToServiceModel(e));
    }

    public async Task<EstablishmentServiceModel> GetEstablishmentAsync(string urn, CancellationToken ct = default)
    {
        var establishment = await _establishmentRepository.GetEstablishmentAsync(urn, ct)
            ?? throw new NotFoundException($"Establishment not found with URN: {urn}");

        return Establishment.MapToServiceModel(establishment);
    }

    public async Task<IEnumerable<EstablishmentServiceModel>> GetEstablishmentsAsync(IEnumerable<string> urns, CancellationToken ct = default)
    {
        var establishments = await _establishmentRepository.GetEstablishmentsAsync(urns, ct);

        if (establishments is null || !establishments.Any())
        {
            throw new NotFoundException($"Establishments not found for the given URNs: {string.Join(", ", urns)}");
        }

        return establishments.Select(e => Establishment.MapToServiceModel(e));
    }

    public async Task<EstablishmentMinimumServiceModel> GetEstablishmentMinimumAsync(string urn, CancellationToken ct = default)
    {
        if (_memoryCache.TryGetValue(urn, out EstablishmentMinimumServiceModel? cacheValue) && cacheValue != null)
        {
            return cacheValue;
        }

        var establishment = await _establishmentRepository.GetEstablishmentAsync(urn, ct)
            ?? throw new NotFoundException($"Establishment not found with URN: {urn}");

        var cacheEntryOptions = new MemoryCacheEntryOptions();

        var establishmentModel = EstablishmentMinimum.MapToServiceModel(establishment);

        _memoryCache.Set(urn, establishmentModel, cacheEntryOptions);

        return establishmentModel;
    }
}