using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.KS4.Performance;

namespace SAPPub.Web.Models.SecondarySchool;

public class AcademicPerformanceSubjectsEnteredViewModel : SecondarySchoolBaseViewModel
{
    public List<SubjectsEnteredViewModel>? GcseSubjects { get; set; }

    public List<SubjectsEnteredViewModel>? VocationalSubjects { get; set; }

    public List<SubjectsEnteredViewModel>? OtherSubjects { get; set; }

    public static AcademicPerformanceSubjectsEnteredViewModel Map(EstablishmentServiceModel establishment, 
        IEnumerable<SubjectsEntered> gcseSubjectEntries, 
        IEnumerable<SubjectsEntered> vocationalSubjectEntries, 
        IEnumerable<SubjectsEntered> otherSubjectEntries)
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

    private static List<SubjectsEnteredViewModel> GetSubjectsEntered(IEnumerable<SubjectsEntered> subjectsEntered)
    { 
        return subjectsEntered.Select(se => new SubjectsEnteredViewModel
        {
            Subject = se.Subject ?? "Unknown Subject",
            Qualification = se.Qualification ?? "Unknown Qualification",
            NumberOfEntries = se.TotalNumberOfEntries.HasValue ? $"{se.TotalNumberOfEntries.Value:F0}" : "N/A",
        }).OrderBy(s => s.Subject).ToList();
    }
}
