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
    public class GatewayUserRepository : IGatewayUserRepository
    {
        private readonly IGenericCRUDRepository<GatewayUser> _genericRepository;
        private ILogger<Establishment> _logger;

        public GatewayUserRepository(
            IGenericCRUDRepository<GatewayUser> genericRepository,
            ILogger<Establishment> logger)
        {
            _genericRepository = genericRepository;
            _logger = logger;
        }

        public GatewayUser? GetByEmail(string email)
        {
            return _genericRepository.ReadAll()?.FirstOrDefault(x => x.EmailAddress == email);
        }

        public GatewayUser? GetById(Guid? id)
        {
            return _genericRepository.ReadAll()?.FirstOrDefault(x => x.Id == id);
        }

        public bool Insert(GatewayUser user)
        {
            return _genericRepository.Create(user);
        }

        public IEnumerable<GatewayUser> GetAll()
        {
            return _genericRepository.ReadAll() ?? new List<GatewayUser>();
        }
    }
}
