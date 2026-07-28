using SAPPub.Core.ServiceModels.KS4.AboutSchool;
using SAPPub.Web.Models;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS2
{
    public class KS2ViewModel : BaseViewModel
    {
        public static KS2ViewModel Map(AboutSchoolModel schoolDetails)
        {
            return new KS2ViewModel
            {
                URN = schoolDetails.Urn,
                SchoolName = schoolDetails.SchoolName,
                IsKS2 = schoolDetails.IsKS2,
                IsKS4 = schoolDetails.IsKS4,
                IsKS5 = schoolDetails.IsKS5
            };
        }
    }
}
