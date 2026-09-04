using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.ServiceModels.KS4.Performance;

public record AdditionalMeasuresModel
{
    public required AdditionalMeasures EstablishmentCurrentYear { get; set; }
    public required AdditionalMeasures LocalAuthorityCurrentYear { get; set; }
    public required AdditionalMeasures EnglandCurrentYear { get; set; }

    public required CodedDouble EstablishmentGirlsEndOfKS4 { get; set; }
    public required CodedDouble EstablishmentBoysEndOfKS4 { get; set; }
    public required CodedDouble EstablishmentEALEndOfKS4 { get; set; }
    public required CodedDouble EstablishmentNonMobilePupilsEndOfKS4 { get; set; }
    public required CodedDouble EstablishmentDisadvantagedPupilsEndOfKS4 { get; set; }

    public required CodedDouble LocalAuthorityDisadvantagedPupilsEndOfKS4 { get; set; }
    public required CodedDouble EnglandDisadvantagedPupilsEndOfKS4 { get; set; }

    public required CodedDouble LocalAuthorityNonDisadvantagedPupilsEndOfKS4 { get; set; }
    public required CodedDouble EnglandNonDisadvantagedPupilsEndOfKS4 { get; set; }

    public required CodedDouble EstablishmentTotalPupils { get; set; }
    public required CodedDouble EnglandTotalPupils { get; set; }

    public required CodedDouble EstablishmentTotalSENPupils { get; set; }
    public required CodedDouble EstablishmentTotalEHCPPupils { get; set; }

    public static AdditionalMeasuresModel Map(
        EstablishmentPerformance performanceMeasures,
        LAPerformance laPeformanceMeasures,
        EnglandPerformance englandPerformanceMeasures)
    {
        return new AdditionalMeasuresModel
        {
            EstablishmentGirlsEndOfKS4 = performanceMeasures.Pup_Grl_Est_Current_Num_Coded,
            EstablishmentBoysEndOfKS4 = performanceMeasures.Pup_Boy_Est_Current_Num_Coded,
            EstablishmentEALEndOfKS4 = performanceMeasures.Pup_EAL_Est_Current_Num_Coded,
            EstablishmentNonMobilePupilsEndOfKS4 = performanceMeasures.Pup_Dis_Est_Current_Num_Coded,
            EstablishmentDisadvantagedPupilsEndOfKS4 = performanceMeasures.Pup_Dis_Est_Current_Num_Coded,
            LocalAuthorityDisadvantagedPupilsEndOfKS4 = laPeformanceMeasures.Pup_Dis_LA_Current_Num_Coded,
            EnglandDisadvantagedPupilsEndOfKS4 = englandPerformanceMeasures.Pup_Dis_Eng_Current_Num_Coded,
            LocalAuthorityNonDisadvantagedPupilsEndOfKS4 = laPeformanceMeasures.Pup_NDi_LA_Current_Num_Coded,
            EnglandNonDisadvantagedPupilsEndOfKS4 = englandPerformanceMeasures.Pup_NDi_Eng_Current_Num_Coded,
            EstablishmentTotalPupils = performanceMeasures.Pup_Tot_Est_Current_Num_Coded,
            EnglandTotalPupils = englandPerformanceMeasures.Pup_Tot_Eng_Current_Num_Coded,
            EstablishmentTotalSENPupils = performanceMeasures.PupSEN_Est_Current_Num_Coded,
            EstablishmentTotalEHCPPupils = performanceMeasures.PupEHCP_Est_Current_Num_Coded,

            EstablishmentCurrentYear = new()
            {
                PercentAchievingAtLeastOneQualification = performanceMeasures.AnyQual_Tot_Est_Current_Pct_Coded,
                PercentEnteredForTripleScience = performanceMeasures.TripSci_Tot_Est_Current_Pct_Coded,
                PercentEnteredMoreThanOneForeignLanguage = performanceMeasures.More1FL_Tot_Est_Current_Pct_Coded,
                AverageGCSEExamEntriesPerPupil = performanceMeasures.ExamEntriesGSCE_Tot_Est_Current_Num_Coded,
                AverageAllKS4QualificationsExamEntriesPerPupil = performanceMeasures.ExamEntriesKS4_Tot_Est_Current_Num_Coded,
                AverageGCSEExamEntriesPerDisadvantagedPupil = performanceMeasures.ExamEntriesGSCE_Dis_Est_Current_Num_Coded,
                AverageAllKS4QualificationsExamEntriesPerDisadvantagedPupil = performanceMeasures.ExamEntriesKS4_Dis_Est_Current_Num_Coded,
                NumberOfPupilsAtTheEndOfKS4 = performanceMeasures.Pup_Tot_Est_Current_Num_Coded
            },
            LocalAuthorityCurrentYear = new()
            {
                PercentAchievingAtLeastOneQualification = laPeformanceMeasures.AnyQual_Tot_LA_Current_Pct_Coded,
                PercentEnteredForTripleScience = laPeformanceMeasures.TripSci_Tot_LA_Current_Pct_Coded,
                PercentEnteredMoreThanOneForeignLanguage = laPeformanceMeasures.More1FL_Tot_LA_Current_Pct_Coded,
                AverageGCSEExamEntriesPerPupil = laPeformanceMeasures.ExamEntriesGSCE_Tot_LA_Current_Num_Coded,
                AverageAllKS4QualificationsExamEntriesPerPupil = laPeformanceMeasures.ExamEntriesKS4_Tot_LA_Current_Num_Coded,
                AverageGCSEExamEntriesPerDisadvantagedPupil = laPeformanceMeasures.ExamEntriesGSCE_Dis_LA_Current_Num_Coded,
                AverageAllKS4QualificationsExamEntriesPerDisadvantagedPupil = laPeformanceMeasures.ExamEntriesKS4_Dis_LA_Current_Num_Coded,
                AverageGCSEExamEntriesPerNonDisadvantagedPupil = laPeformanceMeasures.ExamEntriesGSCE_NDi_LA_Current_Num_Coded,
                AverageAllKS4QualificationsExamEntriesPerNonDisadvantagedPupil = laPeformanceMeasures.ExamEntriesKS4_NDi_LA_Current_Num_Coded,

                NumberOfPupilsAtTheEndOfKS4 = laPeformanceMeasures.Pup_Tot_LA_Current_Num_Coded
            },
            EnglandCurrentYear = new()
            {
                PercentAchievingAtLeastOneQualification = englandPerformanceMeasures.AnyQual_Tot_Eng_Current_Pct_Coded,
                PercentEnteredForTripleScience = englandPerformanceMeasures.TripSci_Tot_Eng_Current_Pct_Coded,
                PercentEnteredMoreThanOneForeignLanguage = englandPerformanceMeasures.More1FL_Tot_Eng_Current_Pct_Coded,
                AverageGCSEExamEntriesPerPupil = englandPerformanceMeasures.ExamEntriesGSCE_Tot_Eng_Current_Num_Coded,
                AverageAllKS4QualificationsExamEntriesPerPupil = englandPerformanceMeasures.ExamEntriesKS4_Tot_Eng_Current_Num_Coded,
                AverageGCSEExamEntriesPerDisadvantagedPupil = englandPerformanceMeasures.ExamEntriesGSCE_Dis_Eng_Current_Num_Coded,
                AverageAllKS4QualificationsExamEntriesPerDisadvantagedPupil = englandPerformanceMeasures.ExamEntriesKS4_Dis_Eng_Current_Num_Coded,
                AverageGCSEExamEntriesPerNonDisadvantagedPupil = englandPerformanceMeasures.ExamEntriesGSCE_NDi_Eng_Current_Num_Coded,
                AverageAllKS4QualificationsExamEntriesPerNonDisadvantagedPupil = englandPerformanceMeasures.ExamEntriesKS4_NDi_Eng_Current_Num_Coded,
                NumberOfPupilsAtTheEndOfKS4 = englandPerformanceMeasures.Pup_Tot_Eng_Current_Num_Coded
            }
        };
    }
}
