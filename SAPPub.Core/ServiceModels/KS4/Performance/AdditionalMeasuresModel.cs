using SAPPub.Core.Entities.KS4.Performance;

namespace SAPPub.Core.ServiceModels.KS4.Performance;

public record AdditionalMeasuresModel
{
    public required AdditionalMeasures EstablishmentCurrentYear { get; set; }
    public required AdditionalMeasures LocalAuthorityCurrentYear { get; set; }
    public required AdditionalMeasures EnglandCurrentYear { get; set; }

    public static AdditionalMeasuresModel Map(
        EstablishmentPerformance performanceMeasures,
        LAPerformance laPeformanceMeasures,
        EnglandPerformance englandPerformanceMeasures)
    {
        return new AdditionalMeasuresModel
        {
            EstablishmentCurrentYear = new()
            {
                AchievingAtLeastOneQualification = performanceMeasures.AnyQual_Tot_Est_Current_Pct_Coded,
                EnteredForTripleScience = performanceMeasures.TripSci_Tot_Est_Current_Pct_Coded,
                EnteredMoreThanOneForeignLanguage = performanceMeasures.More1FL_Tot_Est_Current_Pct_Coded,
                GCSEExamEntriesPerPupil = performanceMeasures.ExamEntriesGSCE_Tot_Est_Current_Num_Coded,
                AllKS4QualificationsExamEntriesPerPupil = performanceMeasures.ExamEntriesKS4_Tot_Est_Current_Num_Coded,
                PupilsAtTheEndOfKS4 = performanceMeasures.Pup_Tot_Est_Current_Num_Coded
            },
            LocalAuthorityCurrentYear = new()
            {
                AchievingAtLeastOneQualification = laPeformanceMeasures.AnyQual_Tot_LA_Current_Pct_Coded,
                EnteredForTripleScience = laPeformanceMeasures.TripSci_Tot_LA_Current_Pct_Coded,
                EnteredMoreThanOneForeignLanguage = laPeformanceMeasures.More1FL_Tot_LA_Current_Pct_Coded,
                GCSEExamEntriesPerPupil = laPeformanceMeasures.ExamEntriesGSCE_Tot_LA_Current_Num_Coded,
                AllKS4QualificationsExamEntriesPerPupil = laPeformanceMeasures.ExamEntriesKS4_Tot_LA_Current_Num_Coded,
                PupilsAtTheEndOfKS4 = laPeformanceMeasures.Pup_Tot_LA_Current_Num_Coded
            },
            EnglandCurrentYear = new()
            {
                AchievingAtLeastOneQualification = englandPerformanceMeasures.AnyQual_Tot_Eng_Current_Pct_Coded,
                EnteredForTripleScience = englandPerformanceMeasures.TripSci_Tot_Eng_Current_Pct_Coded,
                EnteredMoreThanOneForeignLanguage = englandPerformanceMeasures.More1FL_Tot_Eng_Current_Pct_Coded,
                GCSEExamEntriesPerPupil = englandPerformanceMeasures.ExamEntriesGSCE_Tot_Eng_Current_Num_Coded,
                AllKS4QualificationsExamEntriesPerPupil = englandPerformanceMeasures.ExamEntriesKS4_Tot_Eng_Current_Num_Coded,
                PupilsAtTheEndOfKS4 = englandPerformanceMeasures.Pup_Tot_Eng_Current_Num_Coded
            }
        };
    }
}
