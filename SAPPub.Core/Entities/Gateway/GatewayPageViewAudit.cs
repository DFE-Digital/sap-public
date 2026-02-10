using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Entities.Gateway
{
    public class GatewayPageViewAudit : GatewayMetadata
    {
        // Nullable as we may choose to not collect this
        public Guid? UserId { get; set; }
        public string Url { get; set; } = string.Empty;
    }
}
