using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Core.ServiceModels.KS4.AboutSchool;
using SAPPub.Web.Constants;
using SAPPub.Web.Models.SecondarySchool;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS5
{
    public class KS5ViewModel : SecondarySchoolBaseViewModel
    {
        public Level3 Level3Qualification { get; set; }
        public Level2 Level2Qualification { get; set; }
        public string LevelPageTitle => GetPageTitle(Level3Qualification, Level2Qualification);
        

        public static KS5ViewModel Map(AboutSchoolModel schoolDetails)
        {

            return new KS5ViewModel
            {
                URN = schoolDetails.Urn,
                SchoolName = schoolDetails.SchoolName,
                IsKS2 = schoolDetails.IsKS2,
                IsKS4 = schoolDetails.IsKS4,
                IsKS5 = schoolDetails.IsKS5
            };
        }

        public static string GetPageTitle(Level3 level3, Level2 level2)
        {
            if (level3 != default)
            {
                switch (level3)
                {
                    case Level3.ALevel:
                        return PageTitleConstants.KS5SchoolPageTitles.Level3QualificationsAlevel;
                    case Level3.Academic:
                        return PageTitleConstants.KS5SchoolPageTitles.Level3QualificationsAcademic;
                    case Level3.AppliedGeneral:
                        return PageTitleConstants.KS5SchoolPageTitles.Level3QualificationsAppliedGeneral;
                    case Level3.TechLevel:
                        return PageTitleConstants.KS5SchoolPageTitles.Level3QualificationsTechLevel;
                    case Level3.Apprenticeship:
                        return PageTitleConstants.KS5SchoolPageTitles.Level3QualificationsApprenticeship;
                }
            }
            if (level2 != default)
            {
                switch (level2)
                {
                    case Level2.TechCert:
                        return PageTitleConstants.KS5SchoolPageTitles.Level2QualificationsTechCert;
                    case Level2.Apprenticeship:
                        return PageTitleConstants.KS5SchoolPageTitles.Level2QualificationsApprenticeship;
                }
            }
            return string.Empty;
        }

    }
}
