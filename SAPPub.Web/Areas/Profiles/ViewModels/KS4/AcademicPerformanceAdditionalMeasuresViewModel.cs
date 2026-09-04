using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models;
using SAPPub.Web.Models.SecondarySchool;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS4;

public class AcademicPerformanceAdditionalMeasuresViewModel : BaseViewModel
{
    public required IEnumerable<AcademicPerformanceAdditionalMeasureViewModel> MeasuresInTableFormat { get; set; }

    public required IEnumerable<AcademicPerformanceAdditionalMeasureViewModel> AverageExamsEnteredPerPupil { get; set; }

    public required IEnumerable<AcademicPerformanceAdditionalMeasureViewModel> AverageExamsEnteredPerDisadvantagedPupil { get; set; }

    public required IEnumerable<AcademicPerformanceAdditionalMeasureViewModel> AverageExamsEnteredPerNonDisadvantagedPupil { get; set; }

    public required IEnumerable<AcademicPerformanceAdditionalMeasureViewModel> NumberOfPupilsEndOfKS4 { get; set; }

    public required string LAName { get; set; }

    public required DisplayField<CodedDouble> EstablishmentGirlsEndOfKS4 { get; set; }
    public required DisplayField<CodedDouble> EstablishmentBoysEndOfKS4 { get; set; }
    public required DisplayField<CodedDouble> EstablishmentEALEndOfKS4 { get; set; }
    public required DisplayField<CodedDouble> EstablishmentNonMobilePupilsEndOfKS4 { get; set; }
    public required DisplayField<CodedDouble> EstablishmentDisadvantagedPupilsEndOfKS4 { get; set; }

    public required DisplayField<CodedDouble> LocalAuthorityDisadvantagedPupilsEndOfKS4 { get; set; }
    public required DisplayField<CodedDouble> EnglandDisadvantagedPupilsEndOfKS4 { get; set; }

    public required DisplayField<CodedDouble> LocalAuthorityNonDisadvantagedPupilsEndOfKS4 { get; set; }
    public required DisplayField<CodedDouble> EnglandNonDisadvantagedPupilsEndOfKS4 { get; set; }


    public required DisplayField<CodedDouble> EstablishmentTotalPupils { get; set; }
    public required DisplayField<CodedDouble> EnglandTotalPupils { get; set; }

    public required DisplayField<CodedDouble> EstablishmentTotalSENPupils { get; set; }
    public required DisplayField<CodedDouble> EstablishmentTotalEHCPPupils { get; set; }

    public static AcademicPerformanceAdditionalMeasuresViewModel MapToMeasuresInTableFormat(AdditionalMeasuresModel additionalMeasuresModel, EstablishmentMinimumServiceModel establishmentDetails)
    {
        return new AcademicPerformanceAdditionalMeasuresViewModel
        {
            URN = establishmentDetails.URN,
            SchoolName = establishmentDetails.EstablishmentName,
            IsKS2 = establishmentDetails.IsKS2,
            IsKS4 = establishmentDetails.IsKS4,
            IsKS5 = establishmentDetails.IsKS5,
            LAName = establishmentDetails.LAName,
            EstablishmentGirlsEndOfKS4 = additionalMeasuresModel.EstablishmentGirlsEndOfKS4.ToDisplayField(),
            EstablishmentBoysEndOfKS4 = additionalMeasuresModel.EstablishmentBoysEndOfKS4.ToDisplayField(),
            EstablishmentEALEndOfKS4 = additionalMeasuresModel.EstablishmentEALEndOfKS4.ToDisplayField(),
            EstablishmentNonMobilePupilsEndOfKS4 = additionalMeasuresModel.EstablishmentNonMobilePupilsEndOfKS4.ToDisplayField(),
            EstablishmentDisadvantagedPupilsEndOfKS4 = additionalMeasuresModel.EstablishmentDisadvantagedPupilsEndOfKS4.ToDisplayField(),
            LocalAuthorityDisadvantagedPupilsEndOfKS4 = additionalMeasuresModel.LocalAuthorityDisadvantagedPupilsEndOfKS4.ToDisplayField(),
            EnglandDisadvantagedPupilsEndOfKS4 = additionalMeasuresModel.EnglandDisadvantagedPupilsEndOfKS4.ToDisplayField(),
            LocalAuthorityNonDisadvantagedPupilsEndOfKS4 = additionalMeasuresModel.LocalAuthorityNonDisadvantagedPupilsEndOfKS4.ToDisplayField(),
            EnglandNonDisadvantagedPupilsEndOfKS4 = additionalMeasuresModel.EnglandNonDisadvantagedPupilsEndOfKS4.ToDisplayField(),
            EstablishmentTotalPupils = additionalMeasuresModel.EstablishmentTotalPupils.ToDisplayField(),
            EnglandTotalPupils = additionalMeasuresModel.EnglandTotalPupils.ToDisplayField(),
            EstablishmentTotalSENPupils = additionalMeasuresModel.EstablishmentTotalSENPupils.ToDisplayField(),
            EstablishmentTotalEHCPPupils = additionalMeasuresModel.EstablishmentTotalEHCPPupils.ToDisplayField(),
            MeasuresInTableFormat =
            [
                new (){
                    MeasureName = "Pupils who achieved at least 1 qualification",
                    MeasureFormat = MeasureFormat.Percent,
                    EstablishmentCurrentYear = additionalMeasuresModel.EstablishmentCurrentYear.PercentAchievingAtLeastOneQualification.ToDisplayField(),
                    LocalAuthorityCurrentYear = additionalMeasuresModel.LocalAuthorityCurrentYear.PercentAchievingAtLeastOneQualification.ToDisplayField(),
                    EnglandCurrentYear = additionalMeasuresModel.EnglandCurrentYear.PercentAchievingAtLeastOneQualification.ToDisplayField()
                },
                new ()
                {
                    MeasureName = "Pupils entered for biology, chemistry and physics",
                    MeasureFormat = MeasureFormat.Percent,
                    EstablishmentCurrentYear = additionalMeasuresModel.EstablishmentCurrentYear.PercentEnteredForTripleScience.ToDisplayField(),
                    LocalAuthorityCurrentYear = additionalMeasuresModel.LocalAuthorityCurrentYear.PercentEnteredForTripleScience.ToDisplayField(),
                    EnglandCurrentYear = additionalMeasuresModel.EnglandCurrentYear.PercentEnteredForTripleScience.ToDisplayField()
                },
                new ()
                {
                    MeasureName = "Pupils entered for more than one foreign language",
                    MeasureFormat = MeasureFormat.Percent,
                    EstablishmentCurrentYear = additionalMeasuresModel.EstablishmentCurrentYear.PercentEnteredMoreThanOneForeignLanguage.ToDisplayField(),
                    LocalAuthorityCurrentYear = additionalMeasuresModel.LocalAuthorityCurrentYear.PercentEnteredMoreThanOneForeignLanguage.ToDisplayField(),
                    EnglandCurrentYear = additionalMeasuresModel.EnglandCurrentYear.PercentEnteredMoreThanOneForeignLanguage.ToDisplayField()
                }
            ],
            NumberOfPupilsEndOfKS4 = 
            [
                new ()
                {
                    MeasureName = "Number of pupils at the end of KS4",
                    MeasureFormat = MeasureFormat.Int,
                    EstablishmentCurrentYear = additionalMeasuresModel.EstablishmentCurrentYear.NumberOfPupilsAtTheEndOfKS4.ToDisplayField(),
                    LocalAuthorityCurrentYear = additionalMeasuresModel.LocalAuthorityCurrentYear.NumberOfPupilsAtTheEndOfKS4.ToDisplayField(),
                    EnglandCurrentYear = additionalMeasuresModel.EnglandCurrentYear.NumberOfPupilsAtTheEndOfKS4.ToDisplayField()
                }
            ],
            AverageExamsEnteredPerPupil =
            [
                new() {
                    MeasureName = "GCSE qualifications",
                    MeasureFormat = MeasureFormat.Average,
                    EstablishmentCurrentYear = additionalMeasuresModel.EstablishmentCurrentYear.AverageGCSEExamEntriesPerPupil.ToDisplayField(),
                    LocalAuthorityCurrentYear = additionalMeasuresModel.LocalAuthorityCurrentYear.AverageGCSEExamEntriesPerPupil.ToDisplayField(),
                    EnglandCurrentYear = additionalMeasuresModel.EnglandCurrentYear.AverageGCSEExamEntriesPerPupil.ToDisplayField()
                },
                new ()
                {
                    MeasureName = "All KS4 qualifications",
                    MeasureFormat = MeasureFormat.Average,
                    EstablishmentCurrentYear = additionalMeasuresModel.EstablishmentCurrentYear.AverageAllKS4QualificationsExamEntriesPerPupil.ToDisplayField(),
                    LocalAuthorityCurrentYear = additionalMeasuresModel.LocalAuthorityCurrentYear.AverageAllKS4QualificationsExamEntriesPerPupil.ToDisplayField(),
                    EnglandCurrentYear = additionalMeasuresModel.EnglandCurrentYear.AverageAllKS4QualificationsExamEntriesPerPupil.ToDisplayField()
                },
            ],
            AverageExamsEnteredPerDisadvantagedPupil =
            [
                new() {
                    MeasureName = "GCSE qualifications",
                    MeasureFormat = MeasureFormat.Average,
                    EstablishmentCurrentYear = additionalMeasuresModel.EstablishmentCurrentYear.AverageGCSEExamEntriesPerDisadvantagedPupil.ToDisplayField(),
                    LocalAuthorityCurrentYear = additionalMeasuresModel.LocalAuthorityCurrentYear.AverageGCSEExamEntriesPerDisadvantagedPupil.ToDisplayField(),
                    EnglandCurrentYear = additionalMeasuresModel.EnglandCurrentYear.AverageGCSEExamEntriesPerDisadvantagedPupil.ToDisplayField()
                },
                new ()
                {
                    MeasureName = "All KS4 qualifications",
                    MeasureFormat = MeasureFormat.Average,
                    EstablishmentCurrentYear = additionalMeasuresModel.EstablishmentCurrentYear.AverageAllKS4QualificationsExamEntriesPerDisadvantagedPupil.ToDisplayField(),
                    LocalAuthorityCurrentYear = additionalMeasuresModel.LocalAuthorityCurrentYear.AverageAllKS4QualificationsExamEntriesPerDisadvantagedPupil.ToDisplayField(),
                    EnglandCurrentYear = additionalMeasuresModel.EnglandCurrentYear.AverageAllKS4QualificationsExamEntriesPerDisadvantagedPupil.ToDisplayField()
                },
            ],
            AverageExamsEnteredPerNonDisadvantagedPupil =
            [
                new() {
                    MeasureName = "GCSE qualifications",
                    MeasureFormat = MeasureFormat.Average,
                    EstablishmentCurrentYear = additionalMeasuresModel.EstablishmentCurrentYear.AverageGCSEExamEntriesPerNonDisadvantagedPupil.ToDisplayField(),
                    LocalAuthorityCurrentYear = additionalMeasuresModel.LocalAuthorityCurrentYear.AverageGCSEExamEntriesPerNonDisadvantagedPupil.ToDisplayField(),
                    EnglandCurrentYear = additionalMeasuresModel.EnglandCurrentYear.AverageGCSEExamEntriesPerNonDisadvantagedPupil.ToDisplayField()
                },
                new ()
                {
                    MeasureName = "All KS4 qualifications",
                    MeasureFormat = MeasureFormat.Average,
                    EstablishmentCurrentYear = additionalMeasuresModel.EstablishmentCurrentYear.AverageAllKS4QualificationsExamEntriesPerNonDisadvantagedPupil.ToDisplayField(),
                    LocalAuthorityCurrentYear = additionalMeasuresModel.LocalAuthorityCurrentYear.AverageAllKS4QualificationsExamEntriesPerNonDisadvantagedPupil.ToDisplayField(),
                    EnglandCurrentYear = additionalMeasuresModel.EnglandCurrentYear.AverageAllKS4QualificationsExamEntriesPerNonDisadvantagedPupil.ToDisplayField()
                },
            ]
        };
        }
}