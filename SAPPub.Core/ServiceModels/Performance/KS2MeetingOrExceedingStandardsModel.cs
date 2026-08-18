using SAPPub.Core.Entities;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.ServiceModels.Performance;

public class KS2MeetingOrExceedingStandardsModel
{
    public required RelativeYearValues<CodedDouble> EstablishmentPercentage { get; init; }
    public required RelativeYearValues<CodedDouble> LocalAuthorityPercentage { get; init; }
    public required RelativeYearValues<CodedDouble> EnglandPercentage { get; init; }
}
