using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Infrastructure.Repositories.Generic;

namespace SAPPub.Infrastructure.Repositories;

public class EstablishmentLinksRepository(
            NpgsqlDataSource dataSource,
            ILogger<EstablishmentLinksRepository> logger) : IEstablishmentLinksRepository
{
    public async Task<IEnumerable<EstablishmentLinks>?> GetLinksAsync(string urn, CancellationToken ct)
    {
        await using var conn = await dataSource.OpenConnectionAsync(ct).ConfigureAwait(false);

        var sql = """
            select *
            from public.v_establishment_links
            where "urn" = @Urn;
            """;
        var cmd = new DapperCommandBuilder()
                .WithCommandText(sql)
                .WithParameters(new { Urn = urn })
                .Create(ct);

        var items = (await conn.QueryAsync<Core.Entities.EstablishmentLinks>(cmd).ConfigureAwait(false)).ToList();
        return items;
    }
}
