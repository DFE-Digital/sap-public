using SAPPub.Core.Entities.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Interfaces.Repositories.Gateway
{
    public interface IGatewayUserRepository
    {
        bool Insert(GatewayUser user);
        GatewayUser? GetByEmail (string email);
        GatewayUser? GetById(Guid? id);
        IEnumerable<GatewayUser> GetAll();
    }
}
