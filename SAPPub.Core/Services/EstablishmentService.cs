using SAPPub.Core.Entities;
using SAPPub.Core.Exceptions;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;

namespace SAPPub.Core.Services;

public sealed class EstablishmentService(
    IEstablishmentRepository establishmentRepository) : IEstablishmentService
{
    private readonly IEstablishmentRepository _establishmentRepository = establishmentRepository ?? throw new ArgumentNullException(nameof(establishmentRepository));

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
}