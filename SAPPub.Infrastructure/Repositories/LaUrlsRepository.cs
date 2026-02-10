using Microsoft.Extensions.Logging;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Repositories.Generic;

namespace SAPPub.Infrastructure.Repositories;

public class LaUrlsRepository : ILaUrlsRepository
{
    private readonly IGenericRepository<LaUrls> _laUrlsRepository;
    private ILogger<Establishment> _logger;

    public LaUrlsRepository(
        IGenericRepository<LaUrls> repository,
        ILogger<Establishment> logger)
    {
        _laUrlsRepository = repository;
        _logger = logger;
    }


    public async Task<IEnumerable<LaUrls>> GetAllLAsAsync()
    {
        return _laUrlsRepository.ReadAll() ?? [];
    }


    public async Task<LaUrls?> GetLaAsync(string laGssCode)
    {
        return (await GetAllLAsAsync()).FirstOrDefault(x => x.Id == laGssCode);
    }
}
