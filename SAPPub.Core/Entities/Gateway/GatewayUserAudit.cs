using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Entities.Gateway
{
    public class GatewayUserAudit : GatewayMetadata
    {
        public Guid UserId { get; set; }
       public DateTime LoginDateTime { get; set; }
    }
}
