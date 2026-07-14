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
                PercentAchievingAtLeastOneQualification = performanceMeasures.AnyQual_Tot_Est_Current_Pct_Coded,
                PercentEnteredForTripleScience = performanceMeasures.TripSci_Tot_Est_Current_Pct_Coded,
                PercentEnteredMoreThanOneForeignLanguage = performanceMeasures.More1FL_Tot_Est_Current_Pct_Coded,
                AverageGCSEExamEntriesPerPupil = performanceMeasures.ExamEntriesGSCE_Tot_Est_Current_Num_Coded,
                AverageAllKS4QualificationsExamEntriesPerPupil = performanceMeasures.ExamEntriesKS4_Tot_Est_Current_Num_Coded,
                NumberOfPupilsAtTheEndOfKS4 = performanceMeasures.Pup_Tot_Est_Current_Num_Coded
            },
            LocalAuthorityCurrentYear = new()
            {
                PercentAchievingAtLeastOneQualification = laPeformanceMeasures.AnyQual_Tot_LA_Current_Pct_Coded,
                PercentEnteredForTripleScience = laPeformanceMeasures.TripSci_Tot_LA_Current_Pct_Coded,
                PercentEnteredMoreThanOneForeignLanguage = laPeformanceMeasures.More1FL_Tot_LA_Current_Pct_Coded,
                AverageGCSEExamEntriesPerPupil = laPeformanceMeasures.ExamEntriesGSCE_Tot_LA_Current_Num_Coded,
                AverageAllKS4QualificationsExamEntriesPerPupil = laPeformanceMeasures.ExamEntriesKS4_Tot_LA_Current_Num_Coded,
                NumberOfPupilsAtTheEndOfKS4 = laPeformanceMeasures.Pup_Tot_LA_Current_Num_Coded
            },
            EnglandCurrentYear = new()
            {
                PercentAchievingAtLeastOneQualification = englandPerformanceMeasures.AnyQual_Tot_Eng_Current_Pct_Coded,
                PercentEnteredForTripleScience = englandPerformanceMeasures.TripSci_Tot_Eng_Current_Pct_Coded,
                PercentEnteredMoreThanOneForeignLanguage = englandPerformanceMeasures.More1FL_Tot_Eng_Current_Pct_Coded,
                AverageGCSEExamEntriesPerPupil = englandPerformanceMeasures.ExamEntriesGSCE_Tot_Eng_Current_Num_Coded,
                AverageAllKS4QualificationsExamEntriesPerPupil = englandPerformanceMeasures.ExamEntriesKS4_Tot_Eng_Current_Num_Coded,
                NumberOfPupilsAtTheEndOfKS4 = englandPerformanceMeasures.Pup_Tot_Eng_Current_Num_Coded
            }
        };
    }
}
