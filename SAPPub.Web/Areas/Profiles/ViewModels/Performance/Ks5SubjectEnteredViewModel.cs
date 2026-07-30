using SAPPub.Core.Enums;
using SAPPub.Core.ServiceModels.KS4.AboutSchool;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models;

namespace SAPPub.Web.Areas.Profiles.ViewModels.Performance;

public class Ks5SubjectEnteredViewModel : BaseViewModel
{
    public QualificationType QualificationType { get; set; }
    
    public List<SubjectsEnteredDetailViewModel>? Subjects { get; set;  }

    public required DisplayField<string> EstablilshmentWebsite { get; set; }

    public static Ks5SubjectEnteredViewModel Map(AboutSchoolModel schoolDetails, IEnumerable<SubjectsEnteredModel> subjectsEntered)
    {
        return new Ks5SubjectEnteredViewModel
        {
            URN = schoolDetails.Urn,
            SchoolName = schoolDetails.SchoolName,
            IsKS2 = schoolDetails.IsKS2,
            IsKS4 = schoolDetails.IsKS4,
            IsKS5 = schoolDetails.IsKS5,
            EstablilshmentWebsite = schoolDetails.Website.ToDisplayField(),
            Subjects = subjectsEntered.Select(se => new SubjectsEnteredDetailViewModel
            {
                Subject = se.Subject ?? "Unknown Subject",
                Qualification = se.Qualification ?? "Unknown Qualification",
                NumberOfEntries = GetNumberOfEntries(se.TotalNumberOfEntries),
                Level = se.Level
            }).OrderBy(s => s.Subject).ToList()
        };
    }

    private static string GetNumberOfEntries(string? totalNumberOfEntries)
    {
        if (string.IsNullOrWhiteSpace(totalNumberOfEntries))
        {
            return "N/A"!;
        }

        if (int.TryParse(totalNumberOfEntries, out int numberOfEntries))
        {
            return numberOfEntries.ToString("F0");
        }

        return "N/A";
    }
}