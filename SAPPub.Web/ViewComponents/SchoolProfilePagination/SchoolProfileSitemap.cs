using SAPPub.Web.Constants;

namespace SAPPub.Web.ViewComponents.SchoolProfilePagination;

/// <summary>
/// Single, ordered source of truth for every destination that can appear in the
/// school-profile bottom pagination. The order of this list is the pagination order. New
/// destinations are added here only - the resolver logic never needs to change.
/// </summary>
public static class SchoolProfileSitemap
{
    private static string AboutSchoolLabel(PaginationContext ctx) =>
        ctx.IsKS5 ? "About the school or college" : "About the school";

    private static bool IsPrimaryAvailable(PaginationContext ctx) => ctx.IsPrimaryEnabled && ctx.IsKS2;

    private static bool IsSecondaryAvailable(PaginationContext ctx) => ctx.IsKS4;

    private static bool IsSixteenToNineteenAvailable(PaginationContext ctx) => ctx.Is16To19Enabled && ctx.IsKS5;

    public static readonly IReadOnlyList<PaginationDestination> Destinations = new List<PaginationDestination>
    {
        new(
            Key: "AboutSchool",
            Route: RouteConstants.AboutTheSchool,
            Phase: SchoolPhase.None,
            GetLabel: AboutSchoolLabel,
            IsAvailable: _ => true),

        new(
            Key: "PrimaryAdmissions",
            Route: RouteConstants.PrimaryAdmissions,
            Phase: SchoolPhase.Primary,
            GetLabel: _ => "Primary: Admissions",
            IsAvailable: IsPrimaryAvailable),

        new(
            Key: "SecondaryAdmissions",
            Route: RouteConstants.SecondaryAdmissions,
            Phase: SchoolPhase.Secondary,
            GetLabel: _ => "Secondary: Admissions",
            IsAvailable: IsSecondaryAvailable),

        new(
            Key: "PrimaryCurriculum",
            Route: RouteConstants.PrimaryCurriculumAndExtraCurricularActivities,
            Phase: SchoolPhase.Primary,
            GetLabel: _ => "Primary: Curriculum",
            IsAvailable: IsPrimaryAvailable),

        new(
            Key: "SecondaryCurriculum",
            Route: RouteConstants.SecondaryCurriculumAndExtraCurricularActivities,
            Phase: SchoolPhase.Secondary,
            GetLabel: _ => "Secondary: Curriculum",
            IsAvailable: IsSecondaryAvailable),

        new(
            Key: "Attendance",
            Route: RouteConstants.Attendance,
            Phase: SchoolPhase.None,
            GetLabel: _ => PageTitleConstants.PageTitles.Attendance,
            IsAvailable: ctx => IsPrimaryAvailable(ctx) || IsSecondaryAvailable(ctx)),

        // Primary academic performance (fixed set of 4 sub-tabs)
        new(
            Key: "PrimaryAcademicPerformancePupilProgress",
            Route: RouteConstants.PrimaryAcademicPerformancePupilProgress,
            Phase: SchoolPhase.Primary,
            GetLabel: _ => $"Primary academic performance: {PageTitleConstants.PrimarySchoolPageTitles.PupilProgress}",
            IsAvailable: IsPrimaryAvailable),

        new(
            Key: "PrimaryAcademicPerformanceMeetingOrExceedingStandards",
            Route: RouteConstants.PrimaryAcademicPerformanceMeetingOrExceedingStandards,
            Phase: SchoolPhase.Primary,
            GetLabel: _ => $"Primary academic performance: {PageTitleConstants.PrimarySchoolPageTitles.MeetingOrExceedingStandards}",
            IsAvailable: IsPrimaryAvailable),

        new(
            Key: "PrimaryAcademicPerformanceSubjectScaledScores",
            Route: RouteConstants.PrimaryAcademicPerformanceSubjectScaledScores,
            Phase: SchoolPhase.Primary,
            GetLabel: _ => $"Primary academic performance: {PageTitleConstants.PrimarySchoolPageTitles.SubjectScaledScores}",
            IsAvailable: IsPrimaryAvailable),

        new(
            Key: "PrimaryAcademicPerformanceAdditionalMeasures",
            Route: RouteConstants.PrimaryAcademicPerformanceAdditionalMeasures,
            Phase: SchoolPhase.Primary,
            GetLabel: _ => $"Primary academic performance: {PageTitleConstants.PrimarySchoolPageTitles.AdditionalMeasures}",
            IsAvailable: IsPrimaryAvailable),

        // Secondary academic performance (fixed set of 4 sub-tabs)
        new(
            Key: "SecondaryAcademicPerformanceAttainmentAndProgress",
            Route: RouteConstants.SecondaryAcademicPerformanceAttainmentAndProgress,
            Phase: SchoolPhase.Secondary,
            GetLabel: _ => $"Secondary academic performance: {PageTitleConstants.SecondarySchoolPageTitles.ProgressAndAttainment}",
            IsAvailable: IsSecondaryAvailable),

        new(
            Key: "SecondaryAcademicPerformanceEnglishAndMathsResults",
            Route: RouteConstants.SecondaryAcademicPerformanceEnglishAndMathsResults,
            Phase: SchoolPhase.Secondary,
            GetLabel: _ => $"Secondary academic performance: {PageTitleConstants.SecondarySchoolPageTitles.EnglishAndMathsResultsTab}",
            IsAvailable: IsSecondaryAvailable),

        new(
            Key: "SecondaryAcademicPerformanceSubjectsEntered",
            Route: RouteConstants.SecondaryAcademicPerformanceSubjectsEntered,
            Phase: SchoolPhase.Secondary,
            GetLabel: _ => $"Secondary academic performance: {PageTitleConstants.SecondarySchoolPageTitles.SubjectsEntered}",
            IsAvailable: IsSecondaryAvailable),

        new(
            Key: "SecondaryAcademicPerformanceAdditionalMeasures",
            Route: RouteConstants.SecondaryAcademicPerformanceAdditionalMeasures,
            Phase: SchoolPhase.Secondary,
            GetLabel: _ => $"Secondary academic performance: {PageTitleConstants.SecondarySchoolPageTitles.AdditionalMeasures}",
            IsAvailable: IsSecondaryAvailable),

        // 16-19 academic performance (variable sub-tabs)
        new(
            Key: "SixteenToNineteenLevel3",
            Route: RouteConstants.KS5AcademicPerformanceLevel3,
            Phase: SchoolPhase.SixteenToNineteen,
            GetLabel: _ => $"16 to 19 performance: {PageTitleConstants.KS5SchoolPageTitles.Level3Qualifications}",
            IsAvailable: IsSixteenToNineteenAvailable,
            IsVariable: true),

        new(
            Key: "SixteenToNineteenLevel2",
            Route: RouteConstants.KS5AcademicPerformanceLevel2,
            Phase: SchoolPhase.SixteenToNineteen,
            GetLabel: _ => $"16 to 19 performance: {PageTitleConstants.KS5SchoolPageTitles.Level2Qualifications}",
            IsAvailable: IsSixteenToNineteenAvailable,
            IsVariable: true),

        new(
            Key: "SixteenToNineteenEnglishAndMaths",
            Route: RouteConstants.KS5AcademicPerformanceEnglishMaths,
            Phase: SchoolPhase.SixteenToNineteen,
            GetLabel: _ => $"16 to 19 performance: {PageTitleConstants.KS5SchoolPageTitles.EnglishAndMaths}",
            IsAvailable: IsSixteenToNineteenAvailable,
            IsVariable: true),

        new(
            Key: "SixteenToNineteenSubjectsEntered",
            Route: RouteConstants.KS5AcademicPerformanceSubjectsEntered,
            Phase: SchoolPhase.SixteenToNineteen,
            GetLabel: _ => $"16 to 19 performance: {PageTitleConstants.KS5SchoolPageTitles.SubjectsEntered}",
            IsAvailable: IsSixteenToNineteenAvailable,
            IsVariable: true),

        new(
            Key: "SecondaryDestinations",
            Route: RouteConstants.SecondaryDestinations,
            Phase: SchoolPhase.Secondary,
            GetLabel: _ => $"Secondary: {PageTitleConstants.SecondarySchoolPageTitles.Destinations}",
            IsAvailable: IsSecondaryAvailable),

        // 16-19 destinations (up to two variable sub-tabs)
        new(
            Key: "SixteenToNineteenDestinations",
            Route: RouteConstants.KS5Destinations,
            Phase: SchoolPhase.SixteenToNineteen,
            GetLabel: _ => $"16 to 19: {PageTitleConstants.KS5SchoolPageTitles.DestinationsShortTitle}",
            IsAvailable: IsSixteenToNineteenAvailable,
            IsVariable: true),

        new(
            Key: "SixteenToNineteenDestinationsHigher",
            Route: RouteConstants.KS5DestinationsHigher,
            Phase: SchoolPhase.SixteenToNineteen,
            GetLabel: _ => $"16 to 19: {PageTitleConstants.KS5SchoolPageTitles.DestinationsHigherShortTitle}",
            IsAvailable: IsSixteenToNineteenAvailable,
            IsVariable: true),
    };
}
