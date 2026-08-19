using SAPPub.Core.Entities;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.ServiceModels.Performance;

public class KS2MeetingOrExceedingStandardsModel
{
    public required RelativeYearValues<CodedDouble> EstablishmentPercentageMeetingOrExceeding { get; init; }
    public required RelativeYearValues<CodedDouble> LocalAuthorityPercentageMeetingOrExceeding { get; init; }
    public required RelativeYearValues<CodedDouble> EnglandPercentageMeetingOrExceeding { get; init; }
    public required RelativeYearValues<CodedDouble> EstablishmentPercentageExceeding { get; init; }
    public required RelativeYearValues<CodedDouble> LocalAuthorityPercentageExceeding { get; init; }
    public required RelativeYearValues<CodedDouble> EnglandPercentageExceeding { get; init; }
}
