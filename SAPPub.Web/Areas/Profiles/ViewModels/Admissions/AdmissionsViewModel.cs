using SAPPub.Core.Enums;
using SAPPub.Core.ServiceModels.KS4.Admissions;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models;

namespace SAPPub.Web.Areas.Profiles.ViewModels.Admissions;

public class AdmissionsViewModel : BaseViewModel
{
    public required DisplayField<string> SchoolWebsite { get; init; }

    public string? LASchoolAdmissionsLinkUrl { get; init; }

    public required string LAName { get; init; }

    public bool IsSchoolClosed { get; init; }

    public bool IsIndependentSchool { get; init; }
    public bool IsPrimaryFeatureEnabled { get; set; }
    // Computed property - show sub-navigation only when school has both KS2 & KS4 AND primary feature is enabled
    public bool ShowSubNavigation => IsKS2 && IsKS4 && IsPrimaryFeatureEnabled;

    public static AdmissionsViewModel MapFrom(AdmissionsServiceModel serviceModel, string urn, bool isPrimaryFeatureEnabled)
    {
        return new AdmissionsViewModel
        {
            URN = urn,
            SchoolName = serviceModel.SchoolName,
            SchoolWebsite = serviceModel.SchoolWebsite.ToDisplayField(),
            LASchoolAdmissionsLinkUrl = serviceModel.LASchoolAdmissionsUrl,
            LAName = serviceModel.LAName ?? "Local authority",
            IsSchoolClosed = serviceModel.EstablishmentStatus == EstablishmentStatus.Closed,
            IsIndependentSchool = serviceModel.IsIndependentSchool,
            IsKS2 = serviceModel.IsKS2,
            IsKS4 = serviceModel.IsKS4,
            IsKS5 = serviceModel.IsKS5,
            IsPrimaryFeatureEnabled = isPrimaryFeatureEnabled
        };
    }
}
