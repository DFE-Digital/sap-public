using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories.Generic;

namespace SAPPub.Infrastructure.Repositories
{
    public sealed class LaUrlsRepository : ILaUrlsRepository
    {
        private readonly IGenericRepository<LaUrls> _repo;

        public LaUrlsRepository(IGenericRepository<LaUrls> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<IEnumerable<LaUrls>> GetAllLAsAsync(CancellationToken ct = default)
        {
            return await _repo.ReadAllAsync(ct);
        }

        public async Task<LaUrls?> GetLaAsync(string laGssCode, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(laGssCode))
                return null;

            return await _repo.ReadAsync(laGssCode, ct);
        }

        public async Task<IEnumerable<LaUrls?>> GetLaUrlsForEstablishmentsAsync(IEnumerable<string?> gssLaCodeList, CancellationToken ct)
        {
            if (gssLaCodeList is null || !gssLaCodeList.Any())
            {
                return [];
            }

            var laUrlList = await _repo.ReadManyAsync(new { GSSLaCodes = gssLaCodeList });
            return laUrlList;
        }
    }
}
