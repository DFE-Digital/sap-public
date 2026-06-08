using Dapper;
using Npgsql;
using SAPPub.Core.Entities;
using SAPPub.Core.Helpers;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.ServiceModels.Search.InputModels;

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


        public async Task<(IEnumerable<Establishment> Results, int TotalCount)> SearchAsync(SearchQuery query, int maxResults = 10, CancellationToken ct = default)
        {
            var parameters = new DynamicParameters();
            int pageSize = query.PageSize ?? maxResults;
            int offset = ((query.Page ?? 1) - 1) * pageSize;
            parameters.Add("pageSize", pageSize);
            parameters.Add("offset", offset);

            var whereClauses = new List<string>();
            string? orderBy = null;
            bool hasName = !string.IsNullOrWhiteSpace(query.Name);
            bool hasLocation = query.Latitude.HasValue && query.Longitude.HasValue && query.Distance.HasValue;

            if (hasName)
            {
                whereClauses.Add(@"""EstablishmentNameFTS"" @@ plainto_tsquery('english', normalize_text(@searchTerm))");
                parameters.Add("searchTerm", query.Name);
            }
            if (hasLocation)
            {
                whereClauses.Add(@"""geom"" IS NOT NULL");
                whereClauses.Add(@"ST_DWithin(""geom"", ST_SetSRID(ST_MakePoint(@lng, @lat), 4326)::geography, @distance)");
                parameters.Add("lat", query.Latitude);
                parameters.Add("lng", query.Longitude);
                parameters.Add("distance", MappingHelper.MilesToMeters(query.Distance!.Value));
                orderBy = @"""Distance"" ASC, ""EstablishmentName"" ASC";
            }
            else
            {
                orderBy = @"""EstablishmentName"" ASC";
            }

            string selectFields = @"""URN"", ""EstablishmentName"", ""AddressStreet"", ""AddressLocality"", ""AddressAddress3"", ""AddressTown"", ""AddressPostcode"", ""GenderName"", ""ReligiousCharacterName"", ""StatusCode"", ""ClosedDate""";
            if (hasLocation)
                selectFields += @", ST_Distance(""geom"", ST_SetSRID(ST_MakePoint(@lng, @lat), 4326)::geography) AS ""Distance""";

            string where = whereClauses.Count > 0 ? "WHERE " + string.Join(" AND ", whereClauses) : "";

            string sql = $@"
                SELECT {selectFields}
                FROM v_establishment
                {where}
                ORDER BY {orderBy}
                LIMIT @pageSize OFFSET @offset;";

            string countSql = $@"
                SELECT COUNT(*)
                FROM v_establishment
                {where};";

            await using var conn = await _dataSource.OpenConnectionAsync(ct).ConfigureAwait(false);
            var results = await conn.QueryAsync<Establishment>(sql, parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(countSql, parameters);

            return (results, totalCount);
        }
    }
}