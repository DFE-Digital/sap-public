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

        public async Task<Establishment?> GetEstablishmentAsync(string urn, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(urn))
                return null;

            return await _repo.ReadAsync(urn, ct) ?? null;
        }

        public async Task<(IEnumerable<Establishment> Results, int TotalCount)> SearchByNameAsync(
            string searchTerm, int page, int pageSize, CancellationToken ct = default)
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
                WHERE ""EstablishmentNameFTS"" @@ plainto_tsquery('english', normalize_text(@searchTerm))
                ORDER BY ""EstablishmentName"" ASC
                LIMIT @pageSize OFFSET @offset;";

            const string countSql = @"
                SELECT COUNT(*)
                FROM v_establishment
                WHERE ""EstablishmentNameFTS"" @@ plainto_tsquery('english', normalize_text(@searchTerm));";


            int offset = (page - 1) * pageSize;

            await using var conn = await _dataSource.OpenConnectionAsync(ct).ConfigureAwait(false);
            var results = await conn.QueryAsync<Establishment>(sql, new { searchTerm, pageSize, offset });
            var totalCount = await conn.ExecuteScalarAsync<int>(countSql, new { searchTerm });

            return (results, totalCount);
        }

        public async Task<(IEnumerable<Establishment> Results, int TotalCount)> SearchByNameAndLocationAsync(
           string searchTerm, double latitude, double longitude, double distance, int page, int pageSize, CancellationToken ct = default)
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
            ""EstablishmentNameFTS"" @@ plainto_tsquery('english', normalize_text(@searchTerm))
            AND ""geom"" IS NOT NULL
            AND ST_DWithin(""geom"", ST_SetSRID(ST_MakePoint(@lng, @lat), 4326)::geography, @distance)
        ORDER BY ""Distance"" ASC, ""EstablishmentName"" ASC
        LIMIT @pageSize OFFSET @offset;";

            const string countSql = @"
        SELECT COUNT(*)
        FROM v_establishment
        WHERE
            ""EstablishmentNameFTS"" @@ plainto_tsquery('english', normalize_text(@searchTerm))
            AND ""geom"" IS NOT NULL
            AND ST_DWithin(""geom"", ST_SetSRID(ST_MakePoint(@lng, @lat), 4326)::geography, @distance);";

            int offset = (page - 1) * pageSize;
            double distanceMeters = MappingHelper.MilesToMeters(distance);

            await using var conn = await _dataSource.OpenConnectionAsync(ct).ConfigureAwait(false);
            var results = await conn.QueryAsync<Establishment>(sql, new { searchTerm, lat = latitude, lng = longitude, distance = distanceMeters, pageSize, offset });
            var totalCount = await conn.ExecuteScalarAsync<int>(countSql, new { searchTerm, lat = latitude, lng = longitude, distance = distanceMeters });

            return (results, totalCount);
        }

        public async Task<(IEnumerable<Establishment> Results, int TotalCount)> SearchByLocationAsync(
            double latitude, double longitude, double distance, int page, int pageSize, CancellationToken ct = default)
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
        LIMIT @pageSize OFFSET @offset;";

            const string countSql = @"
        SELECT COUNT(*)
        FROM v_establishment
        WHERE
            ""geom"" IS NOT NULL
            AND ST_DWithin(""geom"", ST_SetSRID(ST_MakePoint(@lng, @lat), 4326)::geography, @distance);";

            int offset = (page - 1) * pageSize;
            double distanceMeters = MappingHelper.MilesToMeters(distance);

            await using var conn = await _dataSource.OpenConnectionAsync(ct).ConfigureAwait(false);
            var results = await conn.QueryAsync<Establishment>(sql, new { lat = latitude, lng = longitude, distance = distanceMeters, pageSize, offset });
            var totalCount = await conn.ExecuteScalarAsync<int>(countSql, new { lat = latitude, lng = longitude, distance = distanceMeters });

            return (results, totalCount);
        }
    }
}
