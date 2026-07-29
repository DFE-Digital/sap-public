using SAPPub.Core.Enums;
using SAPPub.Core.ServiceModels.KS4.Admissions;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Models.SecondarySchool;

public class AdmissionsViewModel : BaseViewModel
{
    public required DisplayField<string> SchoolWebsite { get; init; }

    public required string? LASecondarySchoolAdmissionsLinkUrl { get; init; }

    public required string LAName { get; init; }

    public bool IsSchoolClosed { get; init; }

    public static AdmissionsViewModel MapFrom(AdmissionsServiceModel serviceModel, string urn)
    {
        return new AdmissionsViewModel
        {
            URN = urn,
            SchoolName = serviceModel.SchoolName,
            SchoolWebsite = serviceModel.SchoolWebsite.ToDisplayField(),
            LASecondarySchoolAdmissionsLinkUrl = serviceModel.LASchoolAdmissionsUrl,
            LAName = serviceModel.LAName ?? "Local authority",
            IsSchoolClosed = serviceModel.EstablishmentStatus == EstablishmentStatus.Closed,
            IsKS2 = serviceModel.IsKS2,
            IsKS4 = serviceModel.IsKS4,
            IsKS5 = serviceModel.IsKS5
        };
    }
}
