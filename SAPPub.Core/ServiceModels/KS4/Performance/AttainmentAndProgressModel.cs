using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.ServiceModels.KS4.Performance;

public class AttainmentAndProgressModel
{
    public required string Urn { get; init; }

    public required bool IsKS2 { get; set; }
    public required bool IsKS4 { get; set; }
    public required bool IsKS5 { get; set; }

    public string? SchoolName { get; init; }

    public double? EstablishmentProgress8Score { get; init; }

    public double? EstablishmentProgress8CILower { get; init; }

    public double? EstablishmentProgress8CIUpper { get; init; }

    public string? EstablishmentProgress8Banding { get; init; }

    public double? LocalAuthorityProgress8Score { get; init; }

    public double? EstablishmentAttainment8Score { get; init; }
    public CodedDouble EstablishmentAttainment8DisadvantagedScore { get; init; }

    public double? LocalAuthorityAttainment8Score { get; init; }
    public CodedDouble LocalAuthorityAttainment8DisadvantagedScore { get; init; }

    public double? EnglandAttainment8Score { get; init; }
    public CodedDouble EnglandAttainment8DisadvantagedScore { get; init; }

    public double? EstablishmentProgress8TotalPupils { get; init; }

    public double? EstablishmentTotalPupils { get; init; }

    public CodedDouble LocalAuthorityAttainment8NonDisadvantagedScore { get; init; }
    public CodedDouble EnglandAttainment8NonDisadvantagedScore { get; init; }
}
