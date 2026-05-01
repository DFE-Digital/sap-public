using Dapper;
using Npgsql;
using SAPPub.Core.Entities;
using SAPPub.Core.Helpers;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Repositories.Generic;

namespace SAPPub.Infrastructure.Repositories
{
    public sealed class EstablishmentRepository : IEstablishmentRepository
    {
        private readonly IGenericRepository<Establishment> _repo;
        private readonly NpgsqlDataSource _dataSource;

        public EstablishmentRepository(
            IGenericRepository<Establishment> repo,
            NpgsqlDataSource dataSource)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
        }

        public Task<IEnumerable<Establishment>> GetEstablishmentsAsync(int page, int take, CancellationToken ct = default)
        {
            return _repo.ReadPageAsync(page, take, ct);
        }

        public Task<IEnumerable<Establishment>> GetAllEstablishmentsAsync(CancellationToken ct = default)
        {
            // Keep only while we genuinely need to list; LIMIT 100 is already in DapperHelpers
            return _repo.ReadAllAsync(ct);
        }

        public async Task<Establishment?> GetEstablishmentAsync(string urn, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(urn))
                return null;

            return await _repo.ReadAsync(urn, ct) ?? null;
        }

        public async Task<IEnumerable<Establishment>> SearchByNameAsync(string searchTerm, int limit = 20, CancellationToken ct = default)
        {
            const string sql = @"
                SELECT 
                    ""URN"", 
                    ""EstablishmentName"",
                    ""AddressStreet"",
                    ""AddressLocality"",
                    ""AddressAddress3"",
                    ""AddressTown"",
                    ""AddressPostcode"",
                    ""GenderName"",
                    ""ReligiousCharacterName"",
                    ""StatusCode"",
                    ""ClosedDate""
                FROM v_establishment
                WHERE ""EstablishmentNameFTS"" @@ plainto_tsquery('english', @searchTerm)
                ORDER BY ""EstablishmentName"" ASC
                LIMIT @limit;";

            await using var conn = await _dataSource.OpenConnectionAsync(ct).ConfigureAwait(false);
            return await conn.QueryAsync<Establishment>(sql, new { searchTerm, limit });
        }

        public async Task<IEnumerable<Establishment>> SearchByNameAndLocationAsync(
    string searchTerm, double latitude, double longitude, double distance, int limit = 20, CancellationToken ct = default)
        {
            const string sql = @"
                SELECT
                    ""URN"",
                    ""EstablishmentName"",
                    ""AddressStreet"",
                    ""AddressLocality"",
                    ""AddressAddress3"",
                    ""AddressTown"",
                    ""AddressPostcode"",
                    ""GenderName"",
                    ""ReligiousCharacterName"",
                    ""StatusCode"",
                    ""ClosedDate"",
                    ST_Distance(""geom"", ST_SetSRID(ST_MakePoint(@lng, @lat), 4326)::geography) AS ""Distance""
                FROM v_establishment
                WHERE
                    ""EstablishmentNameFTS"" @@ plainto_tsquery('english', @searchTerm)
                    AND ""geom"" IS NOT NULL
                    AND ST_DWithin(""geom"", ST_SetSRID(ST_MakePoint(@lng, @lat), 4326)::geography, @distance)
                ORDER BY ""Distance"" ASC, ""EstablishmentName"" ASC
                LIMIT @limit;";

            await using var conn = await _dataSource.OpenConnectionAsync(ct).ConfigureAwait(false);
            return await conn.QueryAsync<Establishment>(sql, new { searchTerm, lat = latitude, lng = longitude, distance = MappingHelper.MilesToMeters(distance), limit });
        }

        public async Task<IEnumerable<Establishment>> SearchByLocationAsync(
    double latitude, double longitude, double distance, int limit = 20, CancellationToken ct = default)
        {
            const string sql = @"
                SELECT
                    ""URN"",
                    ""EstablishmentName"",
                    ""AddressStreet"",
                    ""AddressLocality"",
                    ""AddressAddress3"",
                    ""AddressTown"",
                    ""AddressPostcode"",
                    ""GenderName"",
                    ""ReligiousCharacterName"",
                    ""StatusCode"",
                    ""ClosedDate"",
                    ST_Distance(""geom"", ST_SetSRID(ST_MakePoint(@lng, @lat), 4326)::geography) AS ""Distance""
                FROM v_establishment
                WHERE
                    ""geom"" IS NOT NULL
                    AND ST_DWithin(""geom"", ST_SetSRID(ST_MakePoint(@lng, @lat), 4326)::geography, @distance)
                ORDER BY ""Distance"" ASC, ""EstablishmentName"" ASC
                LIMIT @limit;";

            await using var conn = await _dataSource.OpenConnectionAsync(ct).ConfigureAwait(false);
            return await conn.QueryAsync<Establishment>(sql, new { lat = latitude, lng = longitude, distance = MappingHelper.MilesToMeters(distance), limit });
        }

    }
}
