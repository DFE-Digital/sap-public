using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Services;

namespace SAPPub.Core.Services
{
    public class LookupService(ILookupRepository lookupRepository) : ILookupService
    {
        private readonly ILookupRepository _lookupRepository = lookupRepository;

        public IEnumerable<Lookup> GetAllLookups()
        {
            return _lookupRepository.GetAllLookups();
        }

        public Lookup GetLookup(string urn)
        {
            return _lookupRepository.GetLookup(urn) ?? new();
        }
    }
}
