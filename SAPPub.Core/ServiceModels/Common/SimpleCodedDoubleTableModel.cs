using SAPPub.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.ServiceModels.Common
{
    public class SimpleCodedDoubleTableModel
    {
        public CodedDouble SchoolOrCollege { get; set; }
        public CodedDouble LocalAuthority { get; set; }
        public CodedDouble England { get; set; }
    }
}
