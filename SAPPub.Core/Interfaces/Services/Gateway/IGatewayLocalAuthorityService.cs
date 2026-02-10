using SAPPub.Core.Entities.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Interfaces.Services.Gateway
{
    public interface IGatewayLocalAuthorityService
    {
        GatewayLocalAuthority? GetByName(string name);
        GatewayLocalAuthority? GetById(Guid Id);
        int CountSignUps(Guid Id);
        bool IsLAOpen(Guid Id);
    }
}
