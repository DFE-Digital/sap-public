using SAPPub.Core.Entities.Gateway;
using SAPPub.Core.Interfaces.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Interfaces.Repositories.Gateway
{
    public interface IGatewaySettingsRepository : IGenericCRUDRepository<GatewaySettings>
    {
    }
}
