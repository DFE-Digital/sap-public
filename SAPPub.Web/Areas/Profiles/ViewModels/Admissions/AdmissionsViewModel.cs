using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels.KS4.Admissions;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Areas.Profiles.ViewModels.Admissions;

public class AdmissionsViewModel : ProfileBaseViewModel
{
    public required DisplayField<string> SchoolWebsite { get; init; }

    public string? LASchoolAdmissionsLinkUrl { get; init; }

    public required string LAName { get; init; }

    public bool IsSchoolClosed { get; init; }

    public bool IsIndependentSchool { get; init; }

    public int CurrentAcademicYear { get; set; }

    public int UpcomingAcadmeicYear { get; set; }
   
    public int YearAfterUpcomingAcademicYear { get; set; }

    public int TwoYearsAfterUpcomingAcademicYear { get; set; }

    public int YearChild { get; set; }

    public SecondaryAdmissionsSectionType SecondaryAdmissionsSectionType { get; set; } = SecondaryAdmissionsSectionType.Upcoming;

    public static AdmissionsViewModel MapFrom(AdmissionsServiceModel serviceModel, string urn)
    {
        return Map(serviceModel, urn);
    }
    
    public static AdmissionsViewModel MapFrom(
        AdmissionsServiceModel serviceModel, 
        string urn,
        ITimeService timeService)
    {
        var admissionsViewModel = Map(serviceModel, urn);

        var now = timeService.GetUKTime();

        var upComingAcademicYear = now.Month switch
        {
            >= 1 and <= 10 => now.Year + 1,
            >= 11 and <= 12 => now.Year + 2,
            _ => throw new NotImplementedException()
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
            >= 1 and <= 6 => SecondaryAdmissionsSectionType.TwoYearsAfterUpcoming,
            >= 7 and <= 10 => SecondaryAdmissionsSectionType.Upcoming,
            >= 11 and <= 12 => SecondaryAdmissionsSectionType.YearAfterUpcoming,
            _ => SecondaryAdmissionsSectionType.TwoYearsAfterUpcoming
        };

        admissionsViewModel.CurrentAcademicYear = upComingAcademicYear - 1;
        admissionsViewModel.UpcomingAcadmeicYear = upComingAcademicYear;
        admissionsViewModel.YearAfterUpcomingAcademicYear = upComingAcademicYear + 1;
        admissionsViewModel.TwoYearsAfterUpcomingAcademicYear = upComingAcademicYear + 2;
        admissionsViewModel.YearChild = yearChild;
        admissionsViewModel.SecondaryAdmissionsSectionType = secondaryAdmissionsSectionType;

        return admissionsViewModel;
    }

    private static AdmissionsViewModel Map(AdmissionsServiceModel serviceModel, string urn)
    {
        return new AdmissionsViewModel
        {
            URN = urn,
            SchoolName = serviceModel.SchoolName ?? string.Empty,
            SchoolWebsite = serviceModel.SchoolWebsite.ToDisplayField(),
            LASchoolAdmissionsLinkUrl = serviceModel.LASchoolAdmissionsUrl,
            LAName = serviceModel.LAName ?? "Local authority",
            IsSchoolClosed = serviceModel.EstablishmentStatus == EstablishmentStatus.Closed,
            IsIndependentSchool = serviceModel.IsIndependentSchool,
            IsKS2 = serviceModel.IsKS2,
            IsKS4 = serviceModel.IsKS4,
            IsKS5 = serviceModel.IsKS5
        };
    }
}
