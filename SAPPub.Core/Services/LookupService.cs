using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Services;

namespace SAPPub.Core.Services
{
    public sealed class LookupService : ILookupService
    {
        private readonly ILookupRepository _lookupRepository;

        public LookupService(ILookupRepository lookupRepository)
        {
            _lookupRepository = lookupRepository ?? throw new ArgumentNullException(nameof(lookupRepository));
        }

        public async Task<IEnumerable<Lookup>> GetAllLookupsAsync(CancellationToken ct = default)
        {
            return await _lookupRepository.GetAllLookupsAsync(ct);
        }

        public async Task<Lookup> GetLookupAsync(string id, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(id))
                return new Lookup();

            return await _lookupRepository.GetLookupAsync(id, ct) ?? new Lookup();
        }
    }
}
