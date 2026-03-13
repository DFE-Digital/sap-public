using SAPPub.Core.Entities.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Interfaces.Services.Gateway
{
    public interface IGatewayUserAuditService
    {
        void Insert(Guid userId);
    }
}
