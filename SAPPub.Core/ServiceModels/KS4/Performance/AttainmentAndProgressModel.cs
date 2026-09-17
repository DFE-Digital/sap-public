using SAPPub.Core.Entities;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.ServiceModels.KS4.Performance;

public class AttainmentAndProgressModel
{
    public required string Urn { get; init; }

    public required bool IsKS2 { get; set; }
    public required bool IsKS4 { get; set; }
    public required bool IsKS5 { get; set; }

    public string? SchoolName { get; init; }

    public required RelativeYearValues<CodedDouble> EstablishmentProgress8Score { get; init; }

    public required RelativeYearValues<CodedDouble> EstablishmentProgress8CILower { get; init; }

    public required RelativeYearValues<CodedDouble> EstablishmentProgress8CIUpper { get; init; }

    public required RelativeYearValues<string?> EstablishmentProgress8Banding { get; init; }

    public required RelativeYearValues<CodedDouble> LocalAuthorityProgress8Score { get; init; }

    public required RelativeYearValues<CodedDouble> EstablishmentAttainment8Score { get; init; }
    public required RelativeYearValues<CodedDouble> EstablishmentAttainment8DisadvantagedScore { get; init; }

    public required RelativeYearValues<CodedDouble> LocalAuthorityAttainment8Score { get; init; }
    public required RelativeYearValues<CodedDouble> LocalAuthorityAttainment8DisadvantagedScore { get; init; }
    public required RelativeYearValues<CodedDouble> EnglandAttainment8Score { get; init; }
    public required RelativeYearValues<CodedDouble> EnglandAttainment8DisadvantagedScore { get; init; }
    public required RelativeYearValues<CodedDouble> EstablishmentProgress8TotalPupils { get; init; }
    public required RelativeYearValues<CodedDouble> EstablishmentTotalPupils { get; init; }

    public CodedDouble LocalAuthorityAttainment8NonDisadvantagedScore { get; init; }
    public CodedDouble EnglandAttainment8NonDisadvantagedScore { get; init; }
}
