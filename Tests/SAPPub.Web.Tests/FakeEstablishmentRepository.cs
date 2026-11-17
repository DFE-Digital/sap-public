using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.ServiceModels.Search.InputModels;

namespace SAPPub.Web.Tests;

public sealed class FakeEstablishmentRepository : IEstablishmentRepository
{
    private static readonly List<Establishment> Establishments = FakeGenericRepository<Establishment>
        .GetAllEstablishments();

    public Task<IEnumerable<Establishment>> GetEstablishmentsAsync(int page, int take, CancellationToken ct = default)
        => Task.FromResult(Establishments.Skip((page - 1) * take).Take(take));

    public Task<Establishment?> GetEstablishmentAsync(string urn, CancellationToken ct = default)
        => Task.FromResult(Establishments.FirstOrDefault(e => e.URN == urn));

    public Task<IEnumerable<Establishment>?> GetEstablishmentsAsync(IEnumerable<string> urns, CancellationToken ct = default)
    {       
        if (urns is null || !urns.Any())
            return Task.FromResult<IEnumerable<Establishment>?>(null);

        var filteredEstablishments = Establishments.Where(e => urns.Contains(e.URN));

        return filteredEstablishments.Any() ? Task.FromResult((IEnumerable<Establishment>?)filteredEstablishments) : Task.FromResult<IEnumerable<Establishment>?>(null);
    }

    
}