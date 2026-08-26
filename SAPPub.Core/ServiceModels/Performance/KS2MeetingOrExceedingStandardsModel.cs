using SAPPub.Core.Entities;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.ServiceModels.Performance;

public class KS2MeetingOrExceedingStandardsModel
{
    public required string LAName { get; init; }
    public required RelativeYearValues<CodedDouble> EstablishmentPercentageMeetingOrExceeding { get; init; }
    public required RelativeYearValues<CodedDouble> LocalAuthorityPercentageMeetingOrExceeding { get; init; }
    public required RelativeYearValues<CodedDouble> EnglandPercentageMeetingOrExceeding { get; init; }
    public required RelativeYearValues<CodedDouble> EstablishmentPercentageExceeding { get; init; }
    public required RelativeYearValues<CodedDouble> LocalAuthorityPercentageExceeding { get; init; }
    public required RelativeYearValues<CodedDouble> EnglandPercentageExceeding { get; init; }

    /* Girls and boys */
    public required CodedDouble GirlsMeetingExpectedStandard { get; init; }
    public required CodedDouble GirlsExceedingExpectedStandard { get; init; }
    public required CodedDouble BoysMeetingExpectedStandard { get; init; }
    public required CodedDouble BoysExceedingExpectedStandard { get; init; }
    public required CodedDouble AllPupilsMeetingExpectedStandard { get; init; }
    public required CodedDouble AllPupilsExceedingExpectedStandard { get; init; }

    /* EAL */
    public required CodedDouble EALMeetingExpectedStandard { get; init; }
    public required CodedDouble EALExceedingExpectedStandard { get; init; }

    /* Non-mobile pupils */
    public required CodedDouble NonMobileMeetingExpectedStandard { get; init; }
    public required CodedDouble NonMobileExceedingExpectedStandard { get; init; }
   
    /* Disadvantaged pupils */
    public required CodedDouble EstablishmentDisadvantagedMeetingExpectedStandard { get; init; }
    public required CodedDouble EstablishmentDisadvantagedExceedingExpectedStandard { get; init; }
    public required CodedDouble LocalAuthorityDisadvantagedMeetingExpectedStandard { get; init; }
    public required CodedDouble LocalAuthorityDisadvantagedExceedingExpectedStandard { get; init; }
    public required CodedDouble EnglandDisadvantagedMeetingExpectedStandard { get; init; }
    public required CodedDouble EnglandDisadvantagedExceedingExpectedStandard { get; init; }
    
    /* Non-disadvantaged pupils */
    public required CodedDouble LocalAuthorityNonDisadvantagedMeetingExpectedStandard { get; init; }
    public required CodedDouble LocalAuthorityNonDisadvantagedExceedingExpectedStandard { get; init; }
    public required CodedDouble EnglandNonDisadvantagedMeetingExpectedStandard { get; init; }
    public required CodedDouble EnglandNonDisadvantagedExceedingExpectedStandard { get; init; }
}
