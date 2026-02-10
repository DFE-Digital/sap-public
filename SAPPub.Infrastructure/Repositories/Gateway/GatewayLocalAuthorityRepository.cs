using Microsoft.Extensions.Logging;
using SAPPub.Core.Entities;
using SAPPub.Core.Entities.Gateway;
using SAPPub.Core.Interfaces.Repositories.Gateway;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Services.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Infrastructure.Repositories.Gateway
{
    public class GatewayLocalAuthorityRepository : IGatewayLocalAuthorityRepository
    {
        private readonly IGenericCRUDRepository<GatewayLocalAuthority> _genericRepository;
        private ILogger<Establishment> _logger;

        public GatewayLocalAuthorityRepository(
            IGenericCRUDRepository<GatewayLocalAuthority> genericRepository,
            ILogger<Establishment> logger)
        {
            _genericRepository = genericRepository;
            _logger = logger;
        }

        public GatewayLocalAuthority? GetByName(string localAuthorityName)
        {
            return _genericRepository.ReadAll()?.FirstOrDefault(x => x.LocalAuthorityName.ToLower() == localAuthorityName.ToLower());
        }

        public GatewayLocalAuthority? GetById(Guid id)
        {
            return _genericRepository.Read(id);
        }

        public IEnumerable<GatewayLocalAuthority> GetAll()
        {
            return _genericRepository.ReadAll() ?? new List<GatewayLocalAuthority>();
        }
    }
}
