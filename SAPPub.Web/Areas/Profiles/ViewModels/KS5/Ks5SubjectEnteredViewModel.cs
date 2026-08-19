using SAPPub.Core.Enums;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.KS4.AboutSchool;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Web.Areas.Profiles.ViewModels.Performance;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS5;

public class Ks5SubjectEnteredViewModel : SubjectsEnteredBaseModel
{
    public QualificationType QualificationType { get; set; }
    
    public List<SubjectsEnteredDetailViewModel>? Subjects { get; set;  }

    public required DisplayField<string> EstablilshmentWebsite { get; set; }

    public static Ks5SubjectEnteredViewModel Map(EstablishmentMinimumServiceModel schoolDetails, IEnumerable<SubjectsEnteredModel> subjectsEntered)
    {
        return new Ks5SubjectEnteredViewModel
        {
            URN = schoolDetails.URN,
            SchoolName = schoolDetails.EstablishmentName,
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
}