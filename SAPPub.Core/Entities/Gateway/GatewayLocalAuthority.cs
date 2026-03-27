using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Entities.Gateway
{
    public class GatewayLocalAuthority : GatewayMetadata
    {
        public string LocalAuthorityName { get; set; } = string.Empty;
        public int MaxSessions { get; set; }
    }
}
