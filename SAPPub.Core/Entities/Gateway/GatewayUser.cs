using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Entities.Gateway
{
    public class GatewayUser : GatewayMetadata
    {
        public string EmailAddress { get; set; } = string.Empty;
        public Guid LocalAuthorityId { get; set; }
        public bool CookiePrefs { get; set; }
        public DateTime RegisteredOn { get; set; }
    }
}
