using SAPPub.Core.ServiceModels.KS4.Admissions;

namespace SAPPub.Web.Models.SecondarySchool;

public class AdmissionsViewModel : SecondarySchoolBaseViewModel
{
    public required string LASecondarySchoolAdmissionsLinkUrl { get; init; }

    public required string LAName { get; init; }

    public static AdmissionsViewModel MapFrom(AdmissionsServiceModel serviceModel, string urn, string schoolName)
    {
        return new AdmissionsViewModel
        {
            URN = urn,
            SchoolName = schoolName,
            LASecondarySchoolAdmissionsLinkUrl = serviceModel.LASchoolAdmissionsUrl,
            LAName = serviceModel.LAName
        };
    }
}
