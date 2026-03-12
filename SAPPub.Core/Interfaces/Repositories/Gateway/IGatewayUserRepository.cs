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
        Task<bool> Insert(GatewayUser user);
        Task<GatewayUser?> GetById(Guid id, CancellationToken ct = default);
        Task<IEnumerable<GatewayUser>> GetAll(CancellationToken ct = default);

    }
}
