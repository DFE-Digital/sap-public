using SAPPub.Core.Entities;

public interface ILaUrlsRepository
{
    Task<IEnumerable<LaUrls>> GetAllLAsAsync(CancellationToken ct = default);
    Task<LaUrls?> GetLaAsync(string laGssCode, CancellationToken ct = default);

    Task<IEnumerable<LaUrls?>> GetLaUrlsForEstablishmentsByGssLaCodeAsync(IEnumerable<string?> gssLaCodeList, CancellationToken ct);
}