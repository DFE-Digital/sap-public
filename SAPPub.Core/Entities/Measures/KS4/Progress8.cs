using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Entities.Measures.KS4
{
    public record Progress8
    {
        public string Id { get; init; } = string.Empty;
        public string LocalAuthorityCode { get; init; } = string.Empty;
        public double? Progress8_Score_Est_Current { get; init; }
        public double? Progress8_Score_Est_Previous { get; init; }
        public double? Progress8_Score_Est_Previous2 { get; init; }

        public double? Progress8_Score_LA_Current { get; init; }
        public double? Progress8_Score_LA_Previous { get; init; }
        public double? Progress8_Score_LA_Previous2 { get; init; }
    }
}
