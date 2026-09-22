using SAPPub.Core.Enums;

namespace SAPPub.Core.ServiceModels.KS4.Admissions;

public class AdmissionsContent
{
    public int CurrentAcademicYear { get; init; }
    public int UpcomingAcademicYear { get; init; }
    public int YearAfterUpcomingAcademicYear { get; init; }
    public int TwoYearsAfterUpcomingAcademicYear { get; init; }
    public int YearChild { get; init; }
    public AdmissionsSectionType AdmissionsSectionType { get; init; }
}