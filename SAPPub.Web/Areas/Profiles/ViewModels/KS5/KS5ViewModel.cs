using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.KS4.AboutSchool;
using SAPPub.Web.Models;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS5
{
    public class KS5ViewModel : BaseViewModel
    {
        public Level3 Level3Qualification { get; set; }
        public Level2 Level2Qualification { get; set; }
        public string LevelPageTitle => GetPageTitle(Level3Qualification, Level2Qualification);
        

        public static KS5ViewModel Map(EstablishmentMinimumServiceModel schoolDetails)
        {

            return new KS5ViewModel
            {
                URN = schoolDetails.URN,
                SchoolName = schoolDetails.EstablishmentName,
                IsKS2 = schoolDetails.IsKS2,
                IsKS4 = schoolDetails.IsKS4,
                IsKS5 = schoolDetails.IsKS5
            };
        }
    }
}
