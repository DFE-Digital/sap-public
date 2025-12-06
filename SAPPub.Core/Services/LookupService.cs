using Microsoft.Extensions.Logging;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Services
{
    public class LookupService : ILookupService
    {
        private readonly ILookupRepository _lookupRepository;


        public LookupService(
            ILookupRepository lookupRepository)
        {
            _lookupRepository = lookupRepository;
        }


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
