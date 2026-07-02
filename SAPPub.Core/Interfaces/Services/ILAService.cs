using SAPPub.Core.Entities;

namespace SAPPub.Core.Interfaces.Services;

/// <summary>
/// Service interface for interacting with data for LAs (Local Authorities)
/// </summary>
public interface ILAService
{

    /// <summary>
    /// Gets a single LaUrl record for a single establishment
    /// </summary>
    /// <param name="establishment">Establishment to get LaUrl for</param>
    /// <param name="ct">Any valid cancellation token</param>
    /// <returns>A valid LaUrl record for the establishment, or null if nothing found</returns>
    Task<LaUrls?> GetLaUrlsAsync(Establishment establishment, CancellationToken ct);

    /// <summary>
    /// Gets a single LaUrl record for each establishment in the establishments list
    /// </summary>
    /// <param name="establishments">List of establishments to get LaUrls for</param>
    /// <param name="ct">Any valid cancellation token</param>
    /// <returns>Multiple LaUrls records for the establishments in the list, or null if nothing found</returns>
    Task<IEnumerable<LaUrls?>> GetLaUrlsListForEstablishmentsAsync(IEnumerable<Establishment> establishments, CancellationToken ct);
}
