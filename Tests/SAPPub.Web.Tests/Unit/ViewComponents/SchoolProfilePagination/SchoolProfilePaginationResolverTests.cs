using SAPPub.Web.Constants;
using SAPPub.Web.ViewComponents.SchoolProfilePagination;

namespace SAPPub.Web.Tests.Unit.ViewComponents.SchoolProfilePagination;

/// <summary>
/// Centralised tests for the school-profile bottom pagination resolver. These cover
/// every establishment "shape" described in the pagination ticket plus feature-flag
/// and variable sub-tab edge cases, replacing any per-page pagination test coverage.
/// </summary>
public class SchoolProfilePaginationResolverTests
{
    private readonly SchoolProfilePaginationResolver _resolver = new(new SchoolProfileSitemapProvider());

    private static PaginationContext PrimaryOnly() => new()
    {
        IsKS2 = true,
        IsKS4 = false,
        IsKS5 = false,
        IsPrimaryEnabled = true,
        Is16To19Enabled = true
    };

    private static PaginationContext SecondaryOnly() => new()
    {
        IsKS2 = false,
        IsKS4 = true,
        IsKS5 = false,
        IsPrimaryEnabled = true,
        Is16To19Enabled = true
    };

    private static PaginationContext SixteenToNineteenOnly(IReadOnlyDictionary<string, bool>? variable = null) => new()
    {
        IsKS2 = false,
        IsKS4 = false,
        IsKS5 = true,
        IsPrimaryEnabled = true,
        Is16To19Enabled = true,
        VariableDestinationAvailability = variable ?? AllSixteenToNineteenTabsAvailable()
    };

    private static PaginationContext PrimaryAndSecondary() => new()
    {
        IsKS2 = true,
        IsKS4 = true,
        IsKS5 = false,
        IsPrimaryEnabled = true,
        Is16To19Enabled = true
    };

    private static PaginationContext SecondaryAndSixteenToNineteen(IReadOnlyDictionary<string, bool>? variable = null) => new()
    {
        IsKS2 = false,
        IsKS4 = true,
        IsKS5 = true,
        IsPrimaryEnabled = true,
        Is16To19Enabled = true,
        VariableDestinationAvailability = variable ?? AllSixteenToNineteenTabsAvailable()
    };

    private static PaginationContext AllThrough(IReadOnlyDictionary<string, bool>? variable = null) => new()
    {
        IsKS2 = true,
        IsKS4 = true,
        IsKS5 = true,
        IsPrimaryEnabled = true,
        Is16To19Enabled = true,
        VariableDestinationAvailability = variable ?? AllSixteenToNineteenTabsAvailable()
    };

    private static Dictionary<string, bool> AllSixteenToNineteenTabsAvailable() => new()
    {
        ["SixteenToNineteenLevel3"] = true,
        ["SixteenToNineteenLevel2"] = true
    };

    [Fact]
    public void Primary_FullSequence_MatchesExpectedOrder()
    {
        var context = PrimaryOnly();

        var routes = new[]
        {
            RouteConstants.AboutTheSchool,
            RouteConstants.PrimaryAdmissions,   
            RouteConstants.PrimaryCurriculumAndExtraCurricularActivities,
            RouteConstants.Attendance,
            RouteConstants.PrimaryAcademicPerformancePupilProgress,
            RouteConstants.PrimaryAcademicPerformanceMeetingOrExceedingStandards,
            RouteConstants.PrimaryAcademicPerformanceSubjectScaledScores,
            RouteConstants.PrimaryAcademicPerformanceAdditionalMeasures
        };

        AssertSequence(context, routes);
    }

    [Fact]
    public void Secondary_FullSequence_MatchesExpectedOrder()
    {
        var context = SecondaryOnly();

        var routes = new[]
        {
            RouteConstants.AboutTheSchool,
            RouteConstants.SecondaryAdmissions,
            RouteConstants.SecondaryCurriculumAndExtraCurricularActivities,
            RouteConstants.Attendance,
            RouteConstants.SecondaryAcademicPerformanceAttainmentAndProgress,
            RouteConstants.SecondaryAcademicPerformanceEnglishAndMathsResults,
            RouteConstants.SecondaryAcademicPerformanceSubjectsEntered,
            RouteConstants.SecondaryAcademicPerformanceAdditionalMeasures,
            RouteConstants.SecondaryDestinations
        };

        AssertSequence(context, routes);
    }

    [Fact]
    public void SixteenToNineteen_FullSequence_MatchesExpectedOrder()
    {
        var context = SixteenToNineteenOnly();

        var routes = new[]
        {
            RouteConstants.AboutTheSchool,
            RouteConstants.KS5AcademicPerformanceLevel3,
            RouteConstants.KS5AcademicPerformanceLevel2,
            RouteConstants.KS5AcademicPerformanceEnglishMaths,
            RouteConstants.KS5AcademicPerformanceSubjectsEntered,
            RouteConstants.KS5Destinations,
            RouteConstants.KS5DestinationsHigher
        };

        AssertSequence(context, routes);
    }

    [Fact]
    public void PrimaryAndSecondary_FullSequence_MatchesExpectedOrder()
    {
        var context = PrimaryAndSecondary();

        var routes = new[]
        {
            RouteConstants.AboutTheSchool,
            RouteConstants.PrimaryAdmissions,
            RouteConstants.SecondaryAdmissions,
            RouteConstants.PrimaryCurriculumAndExtraCurricularActivities,
            RouteConstants.SecondaryCurriculumAndExtraCurricularActivities,
            RouteConstants.Attendance,
            RouteConstants.PrimaryAcademicPerformancePupilProgress,
            RouteConstants.PrimaryAcademicPerformanceMeetingOrExceedingStandards,
            RouteConstants.PrimaryAcademicPerformanceSubjectScaledScores,
            RouteConstants.PrimaryAcademicPerformanceAdditionalMeasures,
            RouteConstants.SecondaryAcademicPerformanceAttainmentAndProgress,
            RouteConstants.SecondaryAcademicPerformanceEnglishAndMathsResults,
            RouteConstants.SecondaryAcademicPerformanceSubjectsEntered,
            RouteConstants.SecondaryAcademicPerformanceAdditionalMeasures,
            RouteConstants.SecondaryDestinations
        };

        AssertSequence(context, routes);
    }

    [Fact]
    public void SecondaryAndSixteenToNineteen_FullSequence_MatchesExpectedOrder()
    {
        var context = SecondaryAndSixteenToNineteen();

        var routes = new[]
        {
            RouteConstants.AboutTheSchool,
            RouteConstants.SecondaryAdmissions,
            RouteConstants.SecondaryCurriculumAndExtraCurricularActivities,
            RouteConstants.Attendance,
            RouteConstants.SecondaryAcademicPerformanceAttainmentAndProgress,
            RouteConstants.SecondaryAcademicPerformanceEnglishAndMathsResults,
            RouteConstants.SecondaryAcademicPerformanceSubjectsEntered,
            RouteConstants.SecondaryAcademicPerformanceAdditionalMeasures,
            RouteConstants.KS5AcademicPerformanceLevel3,
            RouteConstants.KS5AcademicPerformanceLevel2,
            RouteConstants.KS5AcademicPerformanceEnglishMaths,
            RouteConstants.KS5AcademicPerformanceSubjectsEntered,
            RouteConstants.SecondaryDestinations,
            RouteConstants.KS5Destinations,
            RouteConstants.KS5DestinationsHigher
        };

        AssertSequence(context, routes);
    }

    [Fact]
    public void AllThrough_FullSequence_MatchesExpectedOrder()
    {
        var context = AllThrough();

        var routes = new[]
        {
            RouteConstants.AboutTheSchool,
            RouteConstants.PrimaryAdmissions,
            RouteConstants.SecondaryAdmissions,
            RouteConstants.PrimaryCurriculumAndExtraCurricularActivities,
            RouteConstants.SecondaryCurriculumAndExtraCurricularActivities,
            RouteConstants.Attendance,
            RouteConstants.PrimaryAcademicPerformancePupilProgress,
            RouteConstants.PrimaryAcademicPerformanceMeetingOrExceedingStandards,
            RouteConstants.PrimaryAcademicPerformanceSubjectScaledScores,
            RouteConstants.PrimaryAcademicPerformanceAdditionalMeasures,
            RouteConstants.SecondaryAcademicPerformanceAttainmentAndProgress,
            RouteConstants.SecondaryAcademicPerformanceEnglishAndMathsResults,
            RouteConstants.SecondaryAcademicPerformanceSubjectsEntered,
            RouteConstants.SecondaryAcademicPerformanceAdditionalMeasures,
            RouteConstants.KS5AcademicPerformanceLevel3,
            RouteConstants.KS5AcademicPerformanceLevel2,
            RouteConstants.KS5AcademicPerformanceEnglishMaths,
            RouteConstants.KS5AcademicPerformanceSubjectsEntered,
            RouteConstants.SecondaryDestinations,
            RouteConstants.KS5Destinations,
            RouteConstants.KS5DestinationsHigher
        };

        AssertSequence(context, routes);
    }

    [Fact]
    public void UnavailableDestinations_AreSkipped_WhenKS4Only()
    {
        // On an exclusively Secondary establishment, About the school -> Next should be
        // Secondary Admissions, never Primary Admissions (which does not exist).
        var context = SecondaryOnly();

        var result = _resolver.Resolve(RouteConstants.AboutTheSchool, context);

        Assert.NotNull(result.Next);
        Assert.Equal(RouteConstants.SecondaryAdmissions, result.Next!.Route);
    }

    [Fact]
    public void UnavailableDestinations_AreSkipped_WhenKS5Only()
    {
        // On an exclusively 16-19 establishment, no Admissions page should exist -
        // About the school or college -> Next should jump straight to 16-19 performance.
        var context = SixteenToNineteenOnly();

        var result = _resolver.Resolve(RouteConstants.AboutTheSchool, context);

        Assert.NotNull(result.Next);
        Assert.Equal(RouteConstants.KS5AcademicPerformanceLevel3, result.Next!.Route);
        Assert.DoesNotContain("Admissions", result.Next.Label);
    }

    [Fact]
    public void FirstDestination_HasNoPrevious()
    {
        var context = AllThrough();

        var result = _resolver.Resolve(RouteConstants.AboutTheSchool, context);

        Assert.Null(result.Previous);
        Assert.NotNull(result.Next);
    }

    [Fact]
    public void LastDestination_HasNoNext()
    {
        var context = AllThrough();

        var result = _resolver.Resolve(RouteConstants.KS5DestinationsHigher, context);

        Assert.NotNull(result.Previous);
        Assert.Null(result.Next);
    }

    [Fact]
    public void CurrentRouteNotInSitemapOrUnavailable_ReturnsEmpty()
    {
        var context = SecondaryOnly();

        var result = _resolver.Resolve(RouteConstants.PrimaryAdmissions, context);

        Assert.Null(result.Previous);
        Assert.Null(result.Next);
    }

    [Fact]
    public void PrimaryFeatureFlagDisabled_HidesPrimaryPages_EvenWhenIsKS2True()
    {
        var context = PrimaryAndSecondary();
        context.IsPrimaryEnabled = false;

        // About the school -> Next should skip straight to Secondary admissions.
        var result = _resolver.Resolve(RouteConstants.AboutTheSchool, context);

        Assert.NotNull(result.Next);
        Assert.Equal(RouteConstants.SecondaryAdmissions, result.Next!.Route);
    }

    [Fact]
    public void SixteenToNineteenFeatureFlagDisabled_HidesSixteenToNineteenPages()
    {
        var context = SecondaryAndSixteenToNineteen();
        context.Is16To19Enabled = false;

        // Secondary additional measures -> Next should skip 16-19 pages and go straight
        // to Secondary destinations.
        var result = _resolver.Resolve(RouteConstants.SecondaryAcademicPerformanceAdditionalMeasures, context);

        Assert.NotNull(result.Next);
        Assert.Equal(RouteConstants.SecondaryDestinations, result.Next!.Route);
    }

    [Fact]
    public void VariableSixteenToNineteenSubTab_ExcludedWhenNotAvailable()
    {
        var variable = AllSixteenToNineteenTabsAvailable();
        variable["SixteenToNineteenLevel2"] = false;

        var context = SixteenToNineteenOnly(variable);

        var result = _resolver.Resolve(RouteConstants.KS5AcademicPerformanceLevel3, context);

        Assert.NotNull(result.Next);
        Assert.Equal(RouteConstants.KS5AcademicPerformanceEnglishMaths, result.Next!.Route);
    }    

    [Theory]
    [InlineData(true, "About the school or college")]
    [InlineData(false, "About the school")]
    public void AboutSchoolLabel_VariesByKS5Availability(bool isKs5Only, string expectedLabel)
    {
        var context = isKs5Only ? SixteenToNineteenOnly() : SecondaryOnly();

        var result = _resolver.Resolve(
            isKs5Only ? RouteConstants.KS5AcademicPerformanceLevel3 : RouteConstants.SecondaryAdmissions,
            context);

        Assert.NotNull(result.Previous);
        Assert.Equal(expectedLabel, result.Previous!.Label);
    }

    [Fact]
    public void PhaseAwareLabel_SecondaryAcademicPerformance_SingleWhenSecondaryOnly()
    {
        var context = SecondaryOnly();

        var result = _resolver.Resolve(RouteConstants.Attendance, context);

        Assert.NotNull(result.Next);
        Assert.Equal($"Secondary academic performance: Progress and attainment", result.Next!.Label);
    }

    [Fact]
    public void PhaseAwareLabel_SecondaryAcademicPerformance_PhaseSpecificWhenMultiPhase()
    {
        var context = PrimaryAndSecondary();

        var result = _resolver.Resolve(RouteConstants.SecondaryAcademicPerformanceEnglishAndMathsResults, context);

        Assert.NotNull(result.Next);
        Assert.Equal($"Secondary academic performance: Subjects entered", result.Next!.Label);
    }

    [Fact]
    public void PhaseAwareLabel_SixteenToNineteenPerformance_UsesExistingConstantWording()
    {
        var context = SixteenToNineteenOnly();

        var result = _resolver.Resolve(RouteConstants.AboutTheSchool, context);

        Assert.NotNull(result.Next);
        Assert.Equal(
            $"16 to 19 performance: Level 3 qualifications",
            result.Next!.Label);
    }

    [Fact]
    public void PhaseAwareLabel_SixteenToNineteenDestinations_UsesExistingConstantWording()
    {
        var context = SixteenToNineteenOnly();

        var result = _resolver.Resolve(RouteConstants.KS5AcademicPerformanceSubjectsEntered, context);

        Assert.NotNull(result.Next);
        Assert.Equal(
            $"16 to 19: Education, apprenticeships or work",
            result.Next!.Label);
    }

    [Fact]
    public void EmptyCurrentRoute_ReturnsEmptyResult()
    {
        var result = _resolver.Resolve(string.Empty, SecondaryOnly());

        Assert.Equal(SchoolProfilePaginationResult.Empty, result);
    }

    [Fact]
    public void NullContext_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => _resolver.Resolve(RouteConstants.AboutTheSchool, null!));
    }

    private void AssertSequence(PaginationContext context, IReadOnlyList<string> expectedRoutes)
    {
        for (var i = 0; i < expectedRoutes.Count; i++)
        {
            var result = _resolver.Resolve(expectedRoutes[i], context);

            if (i == 0)
            {
                Assert.Null(result.Previous);
            }
            else
            {
                Assert.NotNull(result.Previous);
                Assert.Equal(expectedRoutes[i - 1], result.Previous!.Route);
            }

            if (i == expectedRoutes.Count - 1)
            {
                Assert.Null(result.Next);
            }
            else
            {
                Assert.NotNull(result.Next);
                Assert.Equal(expectedRoutes[i + 1], result.Next!.Route);
            }
        }
    }
}
