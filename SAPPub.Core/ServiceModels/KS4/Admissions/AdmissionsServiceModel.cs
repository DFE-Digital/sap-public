using SAPPub.Core.Enums;

namespace SAPPub.Core.ServiceModels.KS4.Admissions;

public record AdmissionsServiceModel
{
    public string? SchoolName { get; init; }

    public string? SchoolWebsite { get; init; }

    public string? LAName { get; init; }

    public string? LASchoolAdmissionsUrl { get; init; }

    public EstablishmentStatus? EstablishmentStatus { get; init; }

    public bool IsKS2 { get; init; }

    public bool IsKS4 { get; init; }

    public bool IsKS5 { get; init; }

    public bool IsIndependentSchool { get; init; }
}