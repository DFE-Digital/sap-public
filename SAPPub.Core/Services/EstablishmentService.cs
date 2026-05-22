using SAPPub.Core.Entities;
using SAPPub.Core.Exceptions;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Services;

namespace SAPPub.Core.Services;

public sealed class EstablishmentService : IEstablishmentService
{
    private readonly IEstablishmentRepository _establishmentRepository;

    public EstablishmentService(
        IEstablishmentRepository establishmentRepository)
    {
        _establishmentRepository = establishmentRepository ?? throw new ArgumentNullException(nameof(establishmentRepository));
    }

    public Task<IEnumerable<Establishment>> GetEstablishmentsAsync(int page, int take, CancellationToken ct = default)
    {
        return _establishmentRepository.GetEstablishmentsAsync(page, take, ct);
    }

    public async Task<Establishment> GetEstablishmentAsync(string urn, CancellationToken ct = default)
    {

        var establishment = await _establishmentRepository.GetEstablishmentAsync(urn, ct);

        if (establishment is null)
        {
            throw new NotFoundException($"Establishment not found with URN: {urn}");
        }

            return establishment;
        }

        public Task<(IEnumerable<Establishment> Results, int TotalCount)> SearchByNameAsync(
            string searchTerm, int page, int pageSize, CancellationToken ct = default)
        {
            return _establishmentRepository.SearchByNameAsync(searchTerm, page, pageSize, ct);
        }

        public Task<(IEnumerable<Establishment> Results, int TotalCount)> SearchByNameAndLocationAsync(
            string searchTerm, double latitude, double longitude, double distance, int page, int pageSize, CancellationToken ct = default)
        {
            return _establishmentRepository.SearchByNameAndLocationAsync(searchTerm, latitude, longitude, distance, page, pageSize, ct);
        }

        public Task<(IEnumerable<Establishment> Results, int TotalCount)> SearchByLocationAsync(
            double latitude, double longitude, double distance, int page, int pageSize, CancellationToken ct = default)
        {
            return _establishmentRepository.SearchByLocationAsync(latitude, longitude, distance, page, pageSize, ct);
        }
    }

