using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Core.ServiceModels.KS4.AboutSchool;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Web.Models;
using SAPPub.Web.Models.SecondarySchool;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS5
{
    public class Ks5SubjectEnteredViewModel : BaseViewModel
    {
        public Level3 Level3Qualification { get; set; }
        public Level2 Level2Qualification { get; set; }

        public List<SubjectsEnteredViewModel>? Subjects { get; set;  }

        public static Ks5SubjectEnteredViewModel Map(AboutSchoolModel schoolDetails, IEnumerable<SubjectsEntered> subjectsEntered)
        {
            return new Ks5SubjectEnteredViewModel
            {
                URN = schoolDetails.Urn,
                SchoolName = schoolDetails.SchoolName,
                IsKS2 = schoolDetails.IsKS2,
                IsKS4 = schoolDetails.IsKS4,
                IsKS5 = schoolDetails.IsKS5,
                Subjects = subjectsEntered.Select(se => new SubjectsEnteredViewModel
                {
                    Subject = se.Subject ?? "Unknown Subject",
                    Qualification = se.Qualification ?? "Unknown Qualification",
                    NumberOfEntries = se.TotalNumberOfEntries.HasValue ? $"{se.TotalNumberOfEntries.Value:F0}" : "N/A",
                }).OrderBy(s => s.Subject).ToList()
            };
        }
    }
}