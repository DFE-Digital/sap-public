using Microsoft.Extensions.Logging;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Repositories.Generic;

namespace SAPPub.Infrastructure.Repositories;

public sealed class LaUrlsRepository : ILaUrlsRepository
{
    private readonly IGenericRepository<LaUrls> _repo;
    private readonly ILogger<LaUrlsRepository> _logger;

    public LaUrlsRepository(
        IGenericRepository<LaUrls> repo,
        ILogger<LaUrlsRepository> logger)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<IEnumerable<LaUrls>> GetAllLAsAsync()
    {
        return Task.FromResult(_repo.ReadAll() ?? Array.Empty<LaUrls>());
    }

    public Task<LaUrls?> GetLaAsync(string laGssCode)
    {
        if (string.IsNullOrWhiteSpace(laGssCode))
            return Task.FromResult<LaUrls?>(null);

        return Task.FromResult(_repo.Read(laGssCode));
    }
}
