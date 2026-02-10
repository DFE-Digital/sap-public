using SAPPub.Core.Entities.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Interfaces.Repositories.Gateway
{
    public interface IGatewayPageViewAuditRepository
    {
        bool Insert(GatewayPageViewAudit audit);
    }
}
