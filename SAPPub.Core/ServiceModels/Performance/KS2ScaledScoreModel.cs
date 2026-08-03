using SAPPub.Core.Entities;
using SAPPub.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.ServiceModels.Performance
{
    public class KS2ScaledScoreModel
    {
        public required string Urn { get; init; }

        public required bool IsKS2 { get; init; }

        public required bool IsKS4 { get; init; }

        public required bool IsKS5 { get; init; }

        public required string SchoolName { get; init; }

        public required string LAName { get; init; }

        public required RelativeYearValues<CodedDouble> Read_Average_Establishment { get; init; }
        public required RelativeYearValues<CodedDouble> Read_Average_LA { get; init; }
        public required RelativeYearValues<CodedDouble> Read_Average_England { get; init; }
    }
}
