using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Admissions;
using SAPPub.Core.ServiceModels.KS4.Admissions;

namespace SAPPub.Core.Services.KS4.Admissions;

public class AdmissionsContentService(ITimeService timeService) : IAdmissionsContentService
{
    public AdmissionsContent GetContent()
    {
        var now = timeService.GetUTCTime();

        var upComingAcademicYear = now.Month switch
        {
            >= 1 and <= 10 => now.Year + 1,
            >= 11 and <= 12 => now.Year + 2,
            _ => throw new InvalidOperationException($"Unexpected month value: {now.Month}")
        };

        var yearChild = now.Month switch
        {
            >= 1 and <= 6 => 5,
            >= 7 and <= 10 => 6,
            >= 11 and <= 12 => 5,
            _ => throw new NotImplementedException()
        };

        var secondaryAdmissionsSectionType = now.Month switch
        {
            >= 1 and <= 6 => AdmissionsSectionType.TwoYearsAfterUpcoming,
            >= 7 and <= 10 => AdmissionsSectionType.Upcoming,
            >= 11 and <= 12 => AdmissionsSectionType.YearAfterUpcoming,
            _ => AdmissionsSectionType.TwoYearsAfterUpcoming
        };

        return new AdmissionsContent
        {
            CurrentAcademicYear = upComingAcademicYear - 1,
            UpcomingAcademicYear = upComingAcademicYear,
            YearAfterUpcomingAcademicYear = upComingAcademicYear + 1,
            TwoYearsAfterUpcomingAcademicYear = upComingAcademicYear + 2,
            YearChild = yearChild,
            AdmissionsSectionType = secondaryAdmissionsSectionType
        };
    }
}
