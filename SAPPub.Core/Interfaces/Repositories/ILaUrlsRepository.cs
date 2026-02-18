using SAPPub.Core.Entities;

public interface ILaUrlsRepository
{
    Task<IEnumerable<LaUrls>> GetAllLAsAsync(CancellationToken ct = default);
    Task<LaUrls?> GetLaAsync(string laGssCode, CancellationToken ct = default);
}

