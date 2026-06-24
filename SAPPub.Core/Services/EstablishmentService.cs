using SAPPub.Core.Exceptions;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;

namespace SAPPub.Core.Services;

public sealed class EstablishmentService : IEstablishmentService
{
    private readonly IEstablishmentRepository _establishmentRepository;

    public EstablishmentService(
        IEstablishmentRepository establishmentRepository)
    {
        _establishmentRepository = establishmentRepository ?? throw new ArgumentNullException(nameof(establishmentRepository));
    }

    public async Task<IEnumerable<EstablishmentServiceModel>> GetEstablishmentsAsync(int page, int take, CancellationToken ct = default)
    {
        var establishments = await _establishmentRepository.GetEstablishmentsAsync(page, take, ct);
        return establishments.Select(e => EstablishmentServiceModel.Map(e));
    }

    public async Task<EstablishmentServiceModel> GetEstablishmentAsync(string urn, CancellationToken ct = default)
    {

        var establishment = await _establishmentRepository.GetEstablishmentAsync(urn, ct);

        if (establishment is null)
        {
            throw new NotFoundException($"Establishment not found with URN: {urn}");
        }

        return EstablishmentServiceModel.Map(establishment);
    }
}