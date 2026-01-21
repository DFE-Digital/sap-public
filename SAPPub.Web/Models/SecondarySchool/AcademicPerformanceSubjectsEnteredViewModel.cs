using SAPPub.Core.Entities;
using SAPPub.Core.Entities.KS4.SubjectEntries;

namespace SAPPub.Web.Models.SecondarySchool
{
    public class AcademicPerformanceSubjectsEnteredViewModel : SecondarySchoolBaseViewModel
    {
        public List<SubjectsEnteredViewModel>? CoreSubjects { get; set; }

        public List<SubjectsEnteredViewModel>? AdditionalSubjects { get; set; }

        public static AcademicPerformanceSubjectsEnteredViewModel Map(Establishment establishment, EstablishmentCoreSubjectEntries coreSubjectEntries, EstablishmentAdditionalSubjectEntries additionalSubjectEntries)
        {
            var coreSubjects = coreSubjectEntries.SubjectEntries.Select(se => new SubjectsEnteredViewModel
            {
                Subject = se.SubEntCore_Sub_Est_Current_Num ?? "Unknown Subject",
                Qualification = se.SubEntCore_Qual_Est_Current_Num ?? "Unknown Qualification",
                PercentageOfPupilsEntered = se.SubEntCore_Entr_Est_Current_Num.HasValue ? $"{se.SubEntCore_Entr_Est_Current_Num.Value}%" : "N/A",
            }).OrderByDescending(s => s.PercentageOfPupilsEntered).ToList();

            var additionalSubjects = additionalSubjectEntries.SubjectEntries.Select(se => new SubjectsEnteredViewModel
            {
                Subject = se.SubEntAdd_Sub_Est_Current_Num ?? "Unknown Subject",
                Qualification = se.SubEntAdd_Qual_Est_Current_Num ?? "Unknown Qualification",
                PercentageOfPupilsEntered = se.SubEntAdd_Entr_Est_Current_Num.HasValue ? $"{se.SubEntAdd_Entr_Est_Current_Num.Value}%" : "N/A",
            }).OrderBy(s => s.Subject).ToList();

            return new AcademicPerformanceSubjectsEnteredViewModel
            {
                URN = establishment.URN,
                SchoolName = establishment.EstablishmentName,
                CoreSubjects = coreSubjects,
                AdditionalSubjects = additionalSubjects
            };
        }
    }
}
