using SAPPub.Core.Entities;

namespace SAPPub.Web.Models.SecondarySchool
{
    public class AcademicPerformanceSubjectsEnteredViewModel : SecondarySchoolBaseViewModel
    {
        public List<SubjectsEnteredViewModel>? CoreSubjects { get; set; }

        public static AcademicPerformanceSubjectsEnteredViewModel Map(Establishment establishment)
        {
            return new AcademicPerformanceSubjectsEnteredViewModel
            {
                URN = establishment.URN,
                SchoolName = establishment.EstablishmentName,
                CoreSubjects =
                [
                    new() {
                        Subject = "English language",
                        Qualification = "GCSE",
                        PercentageOfPupilsEntered = "95%",
                    },
                    new() {
                        Subject = "English literature",
                        Qualification = "GCSE",
                        PercentageOfPupilsEntered = "90%",
                    },
                    new() {
                        Subject = "Mathematics",
                        Qualification = "GCSE",
                        PercentageOfPupilsEntered = "97%",
                    },
                    new() {
                        Subject = "Science: Double Award",
                        Qualification = "GCSE",
                        PercentageOfPupilsEntered = "55%",
                    },
                    new() {
                        Subject = "Biology",
                        Qualification = "GCSE",
                        PercentageOfPupilsEntered = "76%",
                    },
                ]
            };
        }
    }
}
