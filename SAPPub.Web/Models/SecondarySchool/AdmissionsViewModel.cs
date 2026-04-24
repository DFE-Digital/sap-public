using SAPPub.Core.ServiceModels.KS4.Admissions;
using SAPPub.Web.Helpers;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Models.SecondarySchool;

public class AdmissionsViewModel : SecondarySchoolBaseViewModel
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
            SchoolName = serviceModel?.SchoolName,
            SchoolWebsite = serviceModel.SchoolWebsite.ToDisplayField(),
            LASecondarySchoolAdmissionsLinkUrl = serviceModel?.LASchoolAdmissionsUrl,
            LAName = serviceModel?.LAName ?? "Local authority",
            IsSchoolClosed = serviceModel?.StatusCode == SchoolClosedStatusCode
        };
    }
}
