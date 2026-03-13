namespace SAPPub.Core.ServiceModels.KS4.Performance;

public class AttainmentAndProgressModel
{
    public required string Urn { get; init; }

    public string? SchoolName { get; init; }

    public double? EstablishmentProgress8Score { get; init; }

    public double? LocalAuthorityProgress8Score { get; init; }

    public double? EstablishmentAttainment8Score { get; init; }

    public double? LocalAuthorityAttainment8Score { get; init; }

    public double? EnglandAttainment8Score { get; init; }

    public double? EstablishmentProgress8TotalPupils { get; init; }

    public double? EstablishmentTotalPupils { get; init; }
}
