using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Web.Areas.Profiles.ViewModels.Performance;

namespace SAPPub.Web.Models.SecondarySchool;

public class AcademicPerformanceSubjectsEnteredViewModel : BaseViewModel
{
    public List<SubjectsEnteredDetailViewModel>? GcseSubjects { get; set; }

    public List<SubjectsEnteredDetailViewModel>? VocationalSubjects { get; set; }

    public List<SubjectsEnteredDetailViewModel>? OtherSubjects { get; set; }

    public static AcademicPerformanceSubjectsEnteredViewModel Map(EstablishmentServiceModel establishment, 
        IEnumerable<SubjectsEnteredModel> gcseSubjectEntries, 
        IEnumerable<SubjectsEnteredModel> vocationalSubjectEntries, 
        IEnumerable<SubjectsEnteredModel> otherSubjectEntries)
    {
        var gcseSubjects = GetSubjectsEntered(gcseSubjectEntries);
        var vocationalSubjects = GetSubjectsEntered(vocationalSubjectEntries);
        var otherSubjects = GetSubjectsEntered(otherSubjectEntries);


        return new AcademicPerformanceSubjectsEnteredViewModel
        {
            URN = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            GcseSubjects = GetSubjectsEntered(gcseSubjectEntries),
            VocationalSubjects = GetSubjectsEntered(vocationalSubjectEntries),
            OtherSubjects = GetSubjectsEntered(otherSubjectEntries),
            IsKS2 = establishment.IsKS2,
            IsKS4 = establishment.IsKS4,
            IsKS5 = establishment.IsKS5
        };
    }

    private static List<SubjectsEnteredDetailViewModel> GetSubjectsEntered(IEnumerable<SubjectsEnteredModel> subjectsEntered)
    { 
        return subjectsEntered.Select(se => new SubjectsEnteredDetailViewModel
        {
            Subject = se.Subject ?? "Unknown Subject",
            Qualification = se.Qualification ?? "Unknown Qualification",
            NumberOfEntries = GetNumberOfEntries(se.TotalNumberOfEntries),
        }).OrderBy(s => s.Subject).ToList();
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
