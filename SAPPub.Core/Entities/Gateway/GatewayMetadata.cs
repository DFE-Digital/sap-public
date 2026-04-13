using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Entities.Gateway
{
    public class GatewayMetadata
    {
        public Guid Id { get; set; }    
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
