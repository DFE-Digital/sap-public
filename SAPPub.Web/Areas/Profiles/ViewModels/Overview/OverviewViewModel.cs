using SAPPub.Core.Extensions;
using SAPPub.Core.Helpers;
using SAPPub.Core.ServiceModels.Common;
using SAPPub.Core.ServiceModels.Overview;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Constants;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models.Charts;

namespace SAPPub.Web.Areas.Profiles.ViewModels.Overview;

public sealed class OverviewViewModel : ProfileBaseViewModel
{
    public required DisplayField<string> EducationPhase { get; init; }

    public required DisplayField<string> AgeRange { get; init; }

    public required DisplayField<string> NumberOfPupils { get; init; }

    public required DisplayField<string> SenTypes { get; init; }

    public required DisplayField<string> Telephone { get; init; }

    public required DisplayField<string> SchoolWebsite { get; init; }

    public required DisplayField<string> Address { get; init; }

    public string Longitude { get; set; } = string.Empty;

    public string Latitude { get; set; } = string.Empty;

    public string OfstedReportUrl => $"http://reports.ofsted.gov.uk/inspection-reports/find-inspection-report/provider/ELS/{URN}";

    public required DisplayField<CodedDouble> Attainment8 { get; init; }

    public required DisplayField<CodedDouble> Attainment8LA { get; init; }

    public required DisplayField<CodedDouble> Attainment8England { get; init; }

    public required DisplayField<string> Attainment8Context { get; init; }

    public required DisplayField<CodedDouble> MoreThanOneForeignLanguage { get; init; }

    public required SimpleCodedDoubleTableViewModel? EnglishAndMathsGrade5 { get; init; }

    public required SimpleCodedDoubleTableViewModel? Destinations { get; init; }

    public required SimpleCodedDoubleTableViewModel? ReadingWritingMathsExpected { get; init; }

    public required SimpleCodedDoubleTableViewModel? ReadingWritingMathsHigher { get; init; }

    public required DisplayField<CodedDouble> EnglishAndMathsGrade5Establishment { get; init; }

    public required DisplayField<CodedDouble> EnglishAndMathsGrade5LA { get; init; }

    public required DisplayField<CodedDouble> EnglishAndMathsGrade5England { get; init; }

    public required DisplayField<CodedDouble> DestinationsEstablishment { get; init; }

    public required DisplayField<CodedDouble> DestinationsLA { get; init; }

    public required DisplayField<CodedDouble> DestinationsEngland { get; init; }

    public required DataViewModel DestinationsChart { get; init; }

    public required DataViewModel EnglishAndMathsGrade5Chart { get; init; }

    public required string LocalAuthorityName { get; init; }

    public required IReadOnlyList<NextStepLinkViewModel> NextStepLinks { get; init; }

    public static OverviewViewModel Map(OverviewModel model)
    {
        var latLong = MappingHelper.ConvertToLatLon(model.Easting, model.Northing);

        return new OverviewViewModel
        {
            URN = model.Urn,
            SchoolName = model.SchoolName,
            LocalAuthorityName = model.LocalAuthorityName,
            Address = model.Address.ToDisplayField(),

            Latitude = latLong?.Latitude.ToString() ?? string.Empty,
            Longitude = latLong?.Longitude.ToString() ?? string.Empty,

            IsKS2 = model.IsKS2,
            IsKS4 = model.IsKS4,
            IsKS5 = model.IsKS5,

            EducationPhase = EducationPhaseFormatter.Format(
                model.IsKS2,
                model.IsKS4,
                model.IsKS5)
                .ToDisplayField(),

            AgeRange = GetAgeRange(
                model.AgeRangeLow,
                model.AgeRangeHigh)
                .ToDisplayField(),

            NumberOfPupils = FormatNumberOfPupils(
                model.NumberOfPupils)
                .ToDisplayField(),

            SenTypes = model.SenProvision.ToDisplayField(),
            Telephone = model.Phone.ToDisplayField(),
            SchoolWebsite = model.Website.ToDisplayField(),
            Attainment8 = model.Attainment8.ToDisplayField(),
            Attainment8LA = model.Attainment8LA.ToDisplayField(),
            Attainment8England = model.Attainment8England.ToDisplayField(),
            Attainment8Context = AttainmentHelper.EstablishmentAttainment8ContextStatement(model.Attainment8?.Value).ToDisplayField(),
            MoreThanOneForeignLanguage = model.MoreThanOneForeignLanguage.ToDisplayField(),

            EnglishAndMathsGrade5 = MapComparison(
                model.EnglishAndMathsGrade5Establishment,
                model.EnglishAndMathsGrade5LA,
                model.EnglishAndMathsGrade5England),

            Destinations = MapComparison(
                model.DestinationsEstablishment,
                model.DestinationsLA,
                model.DestinationsEngland),

            ReadingWritingMathsExpected = MapComparison(
                model.ReadingWritingMathsExpectedEstablishment,
                model.ReadingWritingMathsExpectedLA,
                model.ReadingWritingMathsExpectedEngland),

            ReadingWritingMathsHigher = MapComparison(
                model.ReadingWritingMathsHigherEstablishment,
                model.ReadingWritingMathsHigherLA,
                model.ReadingWritingMathsHigherEngland),

            EnglishAndMathsGrade5Establishment =
                model.EnglishAndMathsGrade5Establishment.ToDisplayField(),

                        EnglishAndMathsGrade5LA =
                model.EnglishAndMathsGrade5LA.ToDisplayField(),

                        EnglishAndMathsGrade5England =
                model.EnglishAndMathsGrade5England.ToDisplayField(),

                EnglishAndMathsGrade5Chart = new DataViewModel
                {
                    Labels =
                [
                    "School",
                    $"{model.LocalAuthorityName} average",
                    "England average"
                ],
                            Data =
                [
                    model.EnglishAndMathsGrade5Establishment?.Value,
                    model.EnglishAndMathsGrade5LA?.Value,
                    model.EnglishAndMathsGrade5England?.Value
                ],

            },

            DestinationsEstablishment =
                model.DestinationsEstablishment.ToDisplayField(),

                        DestinationsLA =
                model.DestinationsLA.ToDisplayField(),

                        DestinationsEngland =
                model.DestinationsEngland.ToDisplayField(),

                        DestinationsChart = new DataViewModel
                        {
                            Labels =
                [
                    "School",
                    $"{model.LocalAuthorityName} average",
                    "England average"
                ],
                            Data =
                [
                    model.DestinationsEstablishment?.Value,
                    model.DestinationsLA?.Value,
                    model.DestinationsEngland?.Value
                ]
            },

            NextStepLinks = BuildNextStepLinks(model),
        };
    }

    private static string? GetAgeRange(
        string? ageRangeLow,
        string? ageRangeHigh)
    {
        if (string.IsNullOrWhiteSpace(ageRangeLow) ||
            string.IsNullOrWhiteSpace(ageRangeHigh))
        {
            return null;
        }

        return $"{ageRangeLow} to {ageRangeHigh}";
    }

    private static string? FormatNumberOfPupils(string? numberOfPupils)
    {
        if (string.IsNullOrWhiteSpace(numberOfPupils))
        {
            return null;
        }

        return int.TryParse(numberOfPupils, out var pupils)
            ? pupils.ToString("N0")
            : numberOfPupils;
    }

    private static SimpleCodedDoubleTableViewModel? MapComparison(
    CodedDouble? schoolOrCollege,
    CodedDouble? localAuthority,
    CodedDouble? england)
    {
        if (!schoolOrCollege.HasValue ||
            !localAuthority.HasValue ||
            !england.HasValue)
        {
            return null;
        }

        return SimpleCodedDoubleTableViewModel.Map(
            new SimpleCodedDoubleTableModel
            {
                SchoolOrCollege = schoolOrCollege.Value,
                LocalAuthority = localAuthority.Value,
                England = england.Value
            });
    }

    private static IReadOnlyList<NextStepLinkViewModel> BuildNextStepLinks(OverviewModel model)
    {
        var links = new List<NextStepLinkViewModel>();

        void Add(
            string title,
            string description,
            string routeName)
        {
            if (links.Any(x =>
                    string.Equals(
                        x.Title,
                        title,
                        StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            links.Add(
                new NextStepLinkViewModel(
                    title,
                    description,
                    routeName));
        }

        // About
        Add(
            model.IsKS5 && !model.IsKS2 && !model.IsKS4
                ? "About the school or college"
                : "About the school",
            model.IsKS5 && !model.IsKS2 && !model.IsKS4
                ? "Find out more about the school or college, including policies on school uniform and SEN."
                : "Find out more about the school, including policies on school uniform and SEN.",
            RouteConstants.AboutTheSchool);

        // Admissions
        // Primary comes before Secondary in the established profile order.
        if (model.IsKS2)
        {
            Add(
                "Admissions",
                "Find important dates and learn about the school admissions process.",
                RouteConstants.PrimaryAdmissions);
        }
        else if (model.IsKS4)
        {
            Add(
                "Admissions",
                "Find important dates and learn about the school admissions process.",
                RouteConstants.SecondaryAdmissions);
        }

        // Curriculum and extra-curricular activities
        // Primary comes before Secondary in the established profile order.
        if (model.IsKS2)
        {
            Add(
                "Curriculum and extra-curricular activities",
                "What pupils learn at this school and the activities they can take part in.",
                RouteConstants.PrimaryCurriculumAndExtraCurricularActivities);
        }
        else if (model.IsKS4)
        {
            Add(
                "Curriculum and extra-curricular activities",
                "What pupils learn at this school and the activities they can take part in.",
                RouteConstants.SecondaryCurriculumAndExtraCurricularActivities);
        }

        // Attendance is shared between Primary and Secondary.
        if (model.IsKS2 || model.IsKS4)
        {
            Add(
                "Attendance",
                "Find out more about attendance rates at this school.",
                RouteConstants.Attendance);
        }

        // Primary academic performance
        if (model.IsKS2)
        {
            Add(
                "Primary academic performance",
                "Find out more about this school’s pupil progress, attainment and results.",
                RouteConstants.PrimaryAcademicPerformancePupilProgress);
        }

        // Secondary academic performance
        if (model.IsKS4)
        {
            Add(
                "Secondary academic performance",
                "Find out more about this school’s pupil progress, achievement and results.",
                RouteConstants.SecondaryAcademicPerformanceAttainmentAndProgress);
        }

        // 16 to 19 performance.
        // The root route redirects to the first Level 3 qualifications page.
        if (model.IsKS5)
        {
            Add(
                "Performance in qualifications",
                "Find out more about this school or college’s performance in qualifications.",
                RouteConstants.KS5AcademicPerformanceRoot);
        }

        // Destinations is displayed once.
        // Secondary destinations come before KS5 destinations in the established
        // profile order, so use Secondary when both phases are present.
        if (model.IsKS4)
        {
            Add(
                "Destinations",
                "Find out more about where pupils went after year 11 after leaving this school.",
                RouteConstants.SecondaryDestinations);
        }
        else if (model.IsKS5)
        {
            Add(
                "Destinations",
                "Find out more about where students went after leaving this school or college.",
                RouteConstants.KS5Destinations);
        }

        return links;
    }

}