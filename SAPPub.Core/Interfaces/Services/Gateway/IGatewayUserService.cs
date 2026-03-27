using SAPPub.Core.Entities.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Interfaces.Services.Gateway
{
    public interface IGatewayUserService
    {
        Task<IEnumerable<GatewayUser>> GetAllAsync();
        Task<GatewayUser?> GetByEmailAsync(string email);
        Task<GatewayUser?> GetById(Guid id);
        Task<Guid> InsertAsync(GatewayUser user);
        Task<bool> IsUserExpiredAsync(Guid id);
    }
}
