using SAPPub.Core.Enums;

namespace SAPPub.Core.ServiceModels.KS4.Admissions;

public record AdmissionsServiceModel(
    string SchoolName,
    string? SchoolWebsite,
    string? LAName,
    string? LASchoolAdmissionsUrl,
    EstablishmentStatus? EstablishmentStatus
);
