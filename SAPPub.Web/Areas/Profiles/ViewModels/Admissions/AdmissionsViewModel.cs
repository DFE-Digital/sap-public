using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels.KS4.Admissions;
using SAPPub.Web.Helpers;
using System.ComponentModel;

namespace SAPPub.Web.Areas.Profiles.ViewModels.Admissions;

public class AdmissionsViewModel : ProfileBaseViewModel
{
    public required DisplayField<string> SchoolWebsite { get; init; }

    public string? LASchoolAdmissionsLinkUrl { get; init; }

    public required string LAName { get; init; }

    public bool IsSchoolClosed { get; init; }

    public bool IsIndependentSchool { get; init; }

    public int CurrentAcademicYear { get; set; }

    public int UpcomingAcademicYear { get; set; }
   
    public int YearAfterUpcomingAcademicYear { get; set; }

    public int TwoYearsAfterUpcomingAcademicYear { get; set; }

    public int YearChild { get; set; }

    public int UpcomingStartChildYear => YearChild;

    public int CurrentStartChildYear => YearChild + 1;

    public int NextStartChildYear => YearChild - 1;

    public int FollowingStartChildYear => YearChild - 2;

    public int PreviousAcademicYear => CurrentAcademicYear - 1;

    public AdmissionsSectionType SecondaryAdmissionsSectionType { get; set; } = AdmissionsSectionType.Upcoming;

    public static AdmissionsViewModel MapFrom(AdmissionsServiceModel serviceModel, string urn)
    {
        return Map(serviceModel, urn);
    }
    
    public static AdmissionsViewModel MapFrom(
        AdmissionsServiceModel serviceModel, 
        string urn,
        AdmissionsContent admissionsContent)
    {
        var admissionsViewModel = Map(serviceModel, urn);

        admissionsViewModel.CurrentAcademicYear = admissionsContent.CurrentAcademicYear;
        admissionsViewModel.UpcomingAcademicYear = admissionsContent.UpcomingAcademicYear;
        admissionsViewModel.YearAfterUpcomingAcademicYear = admissionsContent.YearAfterUpcomingAcademicYear;
        admissionsViewModel.TwoYearsAfterUpcomingAcademicYear = admissionsContent.TwoYearsAfterUpcomingAcademicYear;
        admissionsViewModel.YearChild = admissionsContent.YearChild;
        admissionsViewModel.SecondaryAdmissionsSectionType = admissionsContent.AdmissionsSectionType;

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
