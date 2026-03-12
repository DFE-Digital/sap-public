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
        Task<GatewayLocalAuthority?> GetByName(string laName);
        Task<GatewayLocalAuthority?> GetById(Guid Id);
        int CountSignUps(Guid Id);
        Task<bool> IsLAOpen(Guid Id);
    }
}
