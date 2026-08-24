using SAPPub.Core.Enums;
using SAPPub.Core.ServiceModels.KS4.Admissions;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models;

namespace SAPPub.Web.Areas.Profiles.ViewModels.Admissions;

public class AdmissionsViewModel : ProfileBaseViewModel
{
    public required DisplayField<string> SchoolWebsite { get; init; }

    public string? LASchoolAdmissionsLinkUrl { get; init; }

    public required string LAName { get; init; }

    public bool IsSchoolClosed { get; init; }

    public bool IsIndependentSchool { get; init; }  

    public static AdmissionsViewModel MapFrom(AdmissionsServiceModel serviceModel, string urn)
    {
        return new AdmissionsViewModel
        {
            URN = urn,
            SchoolName = serviceModel.SchoolName ?? string.Empty,
            SchoolWebsite = serviceModel.SchoolWebsite.ToDisplayField(),
            LASchoolAdmissionsLinkUrl = serviceModel.LASchoolAdmissionsUrl,
            LAName = serviceModel.LAName ?? "Local authority",
            IsSchoolClosed = serviceModel.EstablishmentStatus == EstablishmentStatus.Closed,
            IsIndependentSchool = serviceModel.IsIndependentSchool,
            IsKS2 = serviceModel.IsKS2,
            IsKS4 = serviceModel.IsKS4,
            IsKS5 = serviceModel.IsKS5
        };
    }
}
