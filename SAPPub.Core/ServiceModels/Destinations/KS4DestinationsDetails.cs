using SAPPub.Core.Entities;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.ServiceModels.Destinations;

public record KS4DestinationsDetails
{
    public required string Urn { get; init; }

    public required string SchoolName { get; init; }

    public required bool IsKS2 { get; set; }
    public required bool IsKS4 { get; set; }
    public required bool IsKS5 { get; set; }

    public required string LocalAuthorityName { get; init; }

    public required RelativeYearValues<CodedDouble> SchoolAll { get; init; }

    public required RelativeYearValues<CodedDouble> LocalAuthorityAll { get; init; }

    public required RelativeYearValues<CodedDouble> EnglandAll { get; init; }

    public required RelativeYearValues<CodedDouble> SchoolDisadvantagedAll { get; init; }

    public required RelativeYearValues<CodedDouble> LocalAuthorityDisadvantagedAll { get; init; }

    public required RelativeYearValues<CodedDouble> EnglandDisadvantagedAll { get; init; }
    public required RelativeYearValues<CodedDouble> LocalAuthorityNonDisadvantagedAll { get; init; }

    public required RelativeYearValues<CodedDouble> EnglandNonDisadvantagedAll { get; init; }

    public required RelativeYearValues<CodedDouble> SchoolEducation { get; init; }

    public required RelativeYearValues<CodedDouble> LocalAuthorityEducation { get; init; }

    public required RelativeYearValues<CodedDouble> EnglandEducation { get; init; }

    public required RelativeYearValues<CodedDouble> SchoolEmployment { get; init; }

    public required RelativeYearValues<CodedDouble> LocalAuthorityEmployment { get; init; }

    public required RelativeYearValues<CodedDouble> EnglandEmployment { get; init; }

    public required RelativeYearValues<CodedDouble> SchoolApprentice { get; init; }

    public required RelativeYearValues<CodedDouble> LocalAuthorityApprentice { get; init; }

    public required RelativeYearValues<CodedDouble> EnglandApprentice { get; init; }
}
