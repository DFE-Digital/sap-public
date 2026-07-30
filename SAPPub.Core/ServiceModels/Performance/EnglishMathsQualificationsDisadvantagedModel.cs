using SAPPub.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.ServiceModels.Performance
{
    public class EnglishMathsQualificationsDisadvantagedModel //Since this is "just" a simple model of three properties, could make it generic in future?
    {
        public CodedDouble SchoolOrCollege { get; set; }
        public CodedDouble LocalAuthority { get; set; }
        public CodedDouble England { get; set; }
    }
}
