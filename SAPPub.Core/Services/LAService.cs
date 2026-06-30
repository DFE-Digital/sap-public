using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Services;

namespace SAPPub.Core.Services;

public class LAService(ILaUrlsRepository laUrlsRepository) : ILAService
{
    public async Task<LaUrls?> GetLaUrlsAsync(Establishment establishment, CancellationToken ct)
    {
        var laUrls = !string.IsNullOrWhiteSpace(establishment.GSSLACode) ? await laUrlsRepository.GetLaAsync(establishment.GSSLACode, ct) : null;
        laUrls ??= !string.IsNullOrWhiteSpace(establishment.DistrictAdministrativeId) ? await laUrlsRepository.GetLaAsync(establishment.DistrictAdministrativeId, ct) : null;
        return laUrls;
    }

    public async Task<IEnumerable<LaUrls?>> GetLaUrlsListForEstablishmentsAsync(IEnumerable<Establishment> establishments, CancellationToken ct)
    {
        if (establishments is null || !establishments.Any())
        {
            return [];
        }

        var gssLACodes = establishments.Select(a => a.GSSLACode).Where(a => !string.IsNullOrWhiteSpace(a)).ToList();
        var laUrlsList = await laUrlsRepository.GetLaUrlsForEstablishmentsByGssLaCodeAsync(gssLACodes, ct);

        if (laUrlsList.Count() == establishments.Count())
        {
            return laUrlsList;
        }

        // If we've reached here, some or all of the LaUrls for > 0 establishments have not been found.
        // Default to the backup search of looking for the records via DistrictAdministrativeId
        var l1Codes = new HashSet<string?>(laUrlsList!.Select(a => a!.Id));
        var missingList = establishments.Where(a => !l1Codes.Contains(a.GSSLACode)).ToList();

        var listMissing = new List<LaUrls?>();
        foreach (var missing in missingList.Where(a => !string.IsNullOrWhiteSpace(a.DistrictAdministrativeId)))
        {
            listMissing.Add(await laUrlsRepository.GetLaAsync(missing.DistrictAdministrativeId, ct));
        }

        return listMissing.Union(laUrlsList);
    }
}
