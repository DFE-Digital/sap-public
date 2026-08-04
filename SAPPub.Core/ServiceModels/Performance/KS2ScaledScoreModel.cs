using SAPPub.Core.Entities;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.ServiceModels.Performance
{
    public class KS2ScaledScoreModel
    {
        public required RelativeYearValues<CodedDouble> ReadAverageEstablishment { get; init; }
        public required RelativeYearValues<CodedDouble> ReadAverageLA { get; init; }
        public required RelativeYearValues<CodedDouble> ReadAverageEngland { get; init; }
    }
}
