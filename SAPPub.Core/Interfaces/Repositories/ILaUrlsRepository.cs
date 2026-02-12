using SAPPub.Core.Entities;

namespace SAPPub.Core.Interfaces.Repositories;

public interface ILaUrlsRepository
{
    public Task<IEnumerable<LaUrls>> GetAllLAsAsync();

    public Task<LaUrls?> GetLaAsync(string laGssCode);
}
