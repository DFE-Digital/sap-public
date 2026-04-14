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
        Task<bool> InsertAsync(GatewayUser user);
        Task<GatewayUser?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IEnumerable<GatewayUser>> GetAllAsync(CancellationToken ct = default);
        Task<bool> UpdateAsync(GatewayUser user);

    }
}
