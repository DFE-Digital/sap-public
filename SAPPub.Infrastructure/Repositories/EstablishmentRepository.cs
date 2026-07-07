using Dapper;
using Npgsql;
using SAPPub.Core.Entities;
using SAPPub.Core.Helpers;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Services.Search;
using SAPPub.Core.ServiceModels.Search.InputModels;

namespace SAPPub.Infrastructure.Repositories
{
    public sealed class EstablishmentRepository : IEstablishmentRepository
    {
        private readonly IGenericRepository<Establishment> _repo;
        private readonly NpgsqlDataSource _dataSource;
        private readonly ISearchVisibilityPolicy _searchVisibilityPolicy;

        internal const string Ks5VisibilityPredicate = @"(""ISKS5"" IS NOT TRUE OR ""ISKS4"" IS TRUE)";

        internal sealed record SearchSqlParts(
            string SelectFields,
            string WhereClause,
            string OrderBy,
            DynamicParameters Parameters);

        public EstablishmentRepository(
            IGenericRepository<Establishment> repo,
            NpgsqlDataSource dataSource,
            ISearchVisibilityPolicy searchVisibilityPolicy)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
            _searchVisibilityPolicy = searchVisibilityPolicy ?? throw new ArgumentNullException(nameof(searchVisibilityPolicy));
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

        internal static SearchSqlParts BuildSearchSqlParts(SearchQuery query, int maxResults, bool includeKs5)
        {
            ArgumentNullException.ThrowIfNull(query);

            var parameters = new DynamicParameters();
            int requestedPageSize = query.PageSize ?? maxResults;
            int pageSize = Math.Clamp(requestedPageSize, 1, maxResults);
            int offset = ((query.Page ?? 1) - 1) * pageSize;
            parameters.Add("pageSize", pageSize);
            parameters.Add("offset", offset);

            var whereClauses = new List<string>();
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
            }

            if (!includeKs5)
            {
                whereClauses.Add(Ks5VisibilityPredicate);
            }

            string orderBy = hasLocation
                ? @"""Distance"" ASC, ""EstablishmentName"" ASC"
                : @"""EstablishmentName"" ASC";

            string selectFields = @"""URN"", ""EstablishmentName"", ""AddressStreet"", ""AddressLocality"", ""AddressAddress3"", ""AddressTown"", ""AddressPostcode"", ""GenderName"", ""ReligiousCharacterName"", ""StatusCode"", ""ClosedDate"", ""ISKS4""";

            if (hasLocation)
            {
                selectFields += @", ST_Distance(""geom"", ST_SetSRID(ST_MakePoint(@lng, @lat), 4326)::geography) AS ""Distance""";
            }

            string whereClause = whereClauses.Count > 0
                ? "WHERE " + string.Join(" AND ", whereClauses)
                : string.Empty;

            return new SearchSqlParts(selectFields, whereClause, orderBy, parameters);
        }

        public async Task<(IEnumerable<Establishment> Results, int TotalCount)> SearchAsync(SearchQuery query, int maxResults = 10, CancellationToken ct = default)
        {
            bool includeKs5 = await _searchVisibilityPolicy.IncludeKs5Async(ct);
            var parts = BuildSearchSqlParts(query, maxResults, includeKs5);

            string sql = $@"
                SELECT {parts.SelectFields}
                FROM v_establishment
                {parts.WhereClause}
                ORDER BY {parts.OrderBy}
                LIMIT @pageSize OFFSET @offset;";

            string countSql = $@"
                SELECT COUNT(*)
                FROM v_establishment
                {parts.WhereClause};";

            await using var conn = await _dataSource.OpenConnectionAsync(ct).ConfigureAwait(false);
            var results = await conn.QueryAsync<Establishment>(sql, parts.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(countSql, parts.Parameters);

            return (results, totalCount);
        }
    }
}