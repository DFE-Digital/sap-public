using SAPPub.Core.Entities;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.ServiceModels.Performance
{
    public class KS2ScaledScoreModel
    {
        public required RelativeYearValues<CodedDouble> Read_Average_Establishment { get; init; }
        public required RelativeYearValues<CodedDouble> Read_Average_LA { get; init; }
        public required RelativeYearValues<CodedDouble> Read_Average_England { get; init; }
    }
}
