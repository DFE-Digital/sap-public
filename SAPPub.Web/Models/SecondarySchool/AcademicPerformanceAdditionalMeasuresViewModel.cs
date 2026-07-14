using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Models.SecondarySchool;

public class AcademicPerformanceAdditionalMeasuresViewModel : SecondarySchoolBaseViewModel
{
    public required IEnumerable<AcademicPerformanceAdditionalMeasureViewModel> MeasuresInTableFormat { get; set; }

    public static AcademicPerformanceAdditionalMeasuresViewModel MapToMeasuresInTableFormat(AdditionalMeasuresModel additionalMeasuresModel, EstablishmentServiceModel establishmentDetails)
    {
        return new AcademicPerformanceAdditionalMeasuresViewModel
        {
            URN = establishmentDetails.URN,
            SchoolName = establishmentDetails.EstablishmentName,
            IsKS2 = establishmentDetails.IsKS2,
            IsKS4 = establishmentDetails.IsKS4,
            IsKS5 = establishmentDetails.IsKS5,
            MeasuresInTableFormat = new List<AcademicPerformanceAdditionalMeasureViewModel>()
            {
                new AcademicPerformanceAdditionalMeasureViewModel{
                    MeasureName = "Pupils achieving at least 1 qualification",
                    MeasureFormat = MeasureFormat.Percent,
                    EstablishmentCurrentYear = additionalMeasuresModel.EstablishmentCurrentYear.PercentAchievingAtLeastOneQualification.ToDisplayField(),
                    LocalAuthorityCurrentYear = additionalMeasuresModel.LocalAuthorityCurrentYear.PercentAchievingAtLeastOneQualification.ToDisplayField(),
                    EnglandCurrentYear = additionalMeasuresModel.EnglandCurrentYear.PercentAchievingAtLeastOneQualification.ToDisplayField()
                },
                new AcademicPerformanceAdditionalMeasureViewModel
                {
                    MeasureName = "Pupils entering for biology, chemistry and physics",
                    MeasureFormat = MeasureFormat.Percent,
                    EstablishmentCurrentYear = additionalMeasuresModel.EstablishmentCurrentYear.PercentEnteredForTripleScience.ToDisplayField(),
                    LocalAuthorityCurrentYear = additionalMeasuresModel.LocalAuthorityCurrentYear.PercentEnteredForTripleScience.ToDisplayField(),
                    EnglandCurrentYear = additionalMeasuresModel.EnglandCurrentYear.PercentEnteredForTripleScience.ToDisplayField()
                },
                new AcademicPerformanceAdditionalMeasureViewModel
                {
                    MeasureName = "Pupils entering for more than one foreign language",
                    MeasureFormat = MeasureFormat.Percent,
                    EstablishmentCurrentYear = additionalMeasuresModel.EstablishmentCurrentYear.PercentEnteredMoreThanOneForeignLanguage.ToDisplayField(),
                    LocalAuthorityCurrentYear = additionalMeasuresModel.LocalAuthorityCurrentYear.PercentEnteredMoreThanOneForeignLanguage.ToDisplayField(),
                    EnglandCurrentYear = additionalMeasuresModel.EnglandCurrentYear.PercentEnteredMoreThanOneForeignLanguage.ToDisplayField()
                },
                new AcademicPerformanceAdditionalMeasureViewModel
                {
                    MeasureName = "Exam entries per pupil, GCSEs",
                    MeasureFormat = MeasureFormat.Average,
                    EstablishmentCurrentYear = additionalMeasuresModel.EstablishmentCurrentYear.AverageGCSEExamEntriesPerPupil.ToDisplayField(),
                    LocalAuthorityCurrentYear = additionalMeasuresModel.LocalAuthorityCurrentYear.AverageGCSEExamEntriesPerPupil.ToDisplayField(),
                    EnglandCurrentYear = additionalMeasuresModel.EnglandCurrentYear.AverageGCSEExamEntriesPerPupil.ToDisplayField()
                },
                new AcademicPerformanceAdditionalMeasureViewModel
                {
                    MeasureName = "Exam entries per pupil, all KS4 qualifications",
                    MeasureFormat = MeasureFormat.Average,
                    EstablishmentCurrentYear = additionalMeasuresModel.EstablishmentCurrentYear.AverageAllKS4QualificationsExamEntriesPerPupil.ToDisplayField(),
                    LocalAuthorityCurrentYear = additionalMeasuresModel.LocalAuthorityCurrentYear.AverageAllKS4QualificationsExamEntriesPerPupil.ToDisplayField(),
                    EnglandCurrentYear = additionalMeasuresModel.EnglandCurrentYear.AverageAllKS4QualificationsExamEntriesPerPupil.ToDisplayField()
                },
                new AcademicPerformanceAdditionalMeasureViewModel
                {
                    MeasureName = "Number of pupils at the end of KS4",
                    MeasureFormat = MeasureFormat.Int,
                    EstablishmentCurrentYear = additionalMeasuresModel.EstablishmentCurrentYear.NumberOfPupilsAtTheEndOfKS4.ToDisplayField(),
                    LocalAuthorityCurrentYear = additionalMeasuresModel.LocalAuthorityCurrentYear.NumberOfPupilsAtTheEndOfKS4.ToDisplayField(),
                    EnglandCurrentYear = additionalMeasuresModel.EnglandCurrentYear.NumberOfPupilsAtTheEndOfKS4.ToDisplayField()
                }
            }
        };
    }
}