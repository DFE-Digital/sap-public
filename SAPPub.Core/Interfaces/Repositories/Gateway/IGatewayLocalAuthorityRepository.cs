using SAPPub.Core.Entities.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Interfaces.Repositories.Gateway
{
    public interface IGatewayLocalAuthorityRepository
    {
        GatewayLocalAuthority? GetById(Guid id, CancellationToken ct = default);
        IEnumerable<GatewayLocalAuthority> GetAll(CancellationToken ct = default);
    }
}
