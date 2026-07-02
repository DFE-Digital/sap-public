using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.ServiceModels.Search.InputModels;

namespace SAPPub.Web.Tests;

public sealed class FakeEstablishmentRepository : IEstablishmentRepository
{
    public static string? CurrentTestPostcode { get; set; }

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

    public Task<(IEnumerable<Establishment> Results, int TotalCount)> SearchAsync(
        SearchQuery query, int maxResults = 10, CancellationToken ct = default)
    {
        IEnumerable<Establishment> results = Establishments;

        // Filter by name if provided
        if (!string.IsNullOrWhiteSpace(query.Name))
        {
            results = results
                .Where(e => e.EstablishmentName != null &&
                            e.EstablishmentName.Contains(query.Name, StringComparison.OrdinalIgnoreCase));
        }

        // Filter by location if provided
        if (query.Latitude.HasValue && query.Longitude.HasValue && query.Distance.HasValue)
        {
            // For fake, just filter by postcode if provided in the AddressPostcode property
            results = results
                .Where(e => string.IsNullOrEmpty(CurrentTestPostcode) ||
                            string.Equals(e.AddressPostcode, CurrentTestPostcode, StringComparison.OrdinalIgnoreCase));
        }

        results = results.OrderBy(e => e.EstablishmentName);

        int page = query.Page ?? 1;
        int pageSize = query.PageSize ?? maxResults;
        int totalCount = results.Count();

        var paged = results.Skip((page - 1) * pageSize).Take(pageSize);

        return Task.FromResult((paged, totalCount));
    }
}