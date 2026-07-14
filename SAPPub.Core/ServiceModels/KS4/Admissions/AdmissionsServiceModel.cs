using SAPPub.Core.Enums;

namespace SAPPub.Core.ServiceModels.KS4.Admissions;

public record AdmissionsServiceModel(
    string SchoolName,
    string? SchoolWebsite,
    string? LAName,
    string? LASchoolAdmissionsUrl,
    EstablishmentStatus? EstablishmentStatus,
    bool IsKS2, 
    bool IsKS4, 
    bool IsKS5
);
