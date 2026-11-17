using Dapper;
using Npgsql;
using SAPPub.Core.Entities;
using SAPPub.Core.Helpers;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Services.Search;
using SAPPub.Core.ServiceModels.Search.InputModels;
using SAPPub.Core.Specifications;
using SAPPub.Infrastructure.Repositories.Generic;

namespace SAPPub.Infrastructure.Repositories;

public sealed class EstablishmentRepository : IEstablishmentRepository
{
    private readonly IGenericRepository<Establishment> _repo;

    internal sealed record SearchSqlParts(
        string SelectFields,
        string WhereClause,
        string OrderBy,
        DynamicParameters Parameters);

    public EstablishmentRepository(
        IGenericRepository<Establishment> repo)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
    }

    public Task<IEnumerable<Establishment>> GetEstablishmentsAsync(int page, int take, CancellationToken ct = default)
    {
        return _repo.ReadPageAsync(page, take, ct);
    }

    public async Task<Establishment?> GetEstablishmentAsync(string urn, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(urn))
            return null;

        return await _repo.ReadAsync(urn, ct) ?? null;
    }

    public async Task<IEnumerable<Establishment>?> GetEstablishmentsAsync(IEnumerable<string> urns, CancellationToken ct = default)
    {
        if (urns is null || !urns.Any())
            return null;

        return await _repo.ReadManyAsync(new { Urns = urns }, ct) ?? null;
    }


}