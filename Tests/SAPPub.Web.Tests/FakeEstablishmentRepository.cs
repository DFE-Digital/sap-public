using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories;

namespace SAPPub.Web.Tests;

public sealed class FakeEstablishmentRepository : IEstablishmentRepository
{
    public static string? CurrentTestPostcode { get; set; }

    private static readonly List<Establishment> Establishments = FakeGenericRepository<Establishment>
        .GetAllEstablishments();

    public Task<IEnumerable<Establishment>> GetEstablishmentsAsync(int page, int take, CancellationToken ct = default)
        => Task.FromResult(Establishments.Skip((page - 1) * take).Take(take));

    public Task<IEnumerable<Establishment>> GetAllEstablishmentsAsync(CancellationToken ct = default)
        => Task.FromResult(Establishments.AsEnumerable());

    public Task<Establishment?> GetEstablishmentAsync(string urn, CancellationToken ct = default)
        => Task.FromResult(Establishments.FirstOrDefault(e => e.URN == urn));

    public Task<(IEnumerable<Establishment> Results, int TotalCount)> SearchByNameAsync(
        string searchTerm, int page, int pageSize, CancellationToken ct = default)
    {
        var results = Establishments
            .Where(e => e.EstablishmentName != null && e.EstablishmentName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .OrderBy(e => e.EstablishmentName)
            .ToList();

        var paged = results.Skip((page - 1) * pageSize).Take(pageSize);
        return Task.FromResult((paged, results.Count));
    }

    public Task<(IEnumerable<Establishment> Results, int TotalCount)> SearchByLocationAsync(
        double latitude, double longitude, double distance, int page, int pageSize, CancellationToken ct = default)
    {

        // For fake, just filter by postcode if provided in the AddressPostcode property
        var results = Establishments
            .Where(e => string.IsNullOrEmpty(CurrentTestPostcode) ||
                        string.Equals(e.AddressPostcode, CurrentTestPostcode, StringComparison.OrdinalIgnoreCase))
            .OrderBy(e => e.EstablishmentName)
            .ToList();

        var paged = results.Skip((page - 1) * pageSize).Take(pageSize);
        return Task.FromResult((paged, results.Count));
    }

    public Task<(IEnumerable<Establishment> Results, int TotalCount)> SearchByNameAndLocationAsync(
        string searchTerm, double latitude, double longitude, double distance, int page, int pageSize, CancellationToken ct = default)
    {
        // For fake, just filter by name and postcode if provided in the AddressPostcode property
        var results = Establishments
            .Where(e => e.EstablishmentName != null && e.EstablishmentName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .Where(e => string.IsNullOrEmpty(CurrentTestPostcode) ||
                        string.Equals(e.AddressPostcode, CurrentTestPostcode, StringComparison.OrdinalIgnoreCase))
            .OrderBy(e => e.EstablishmentName)
            .ToList();

        var paged = results.Skip((page - 1) * pageSize).Take(pageSize);
        return Task.FromResult((paged, results.Count));
    }
}