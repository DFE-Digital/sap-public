using SAPPub.Core.Entities;
using SAPPub.Core.Enums;
using SAPPub.Core.Extensions;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models;
using SAPPub.Web.Models.Charts;
using SAPPub.Web.Models.Config;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS4;

public class AcademicPerformanceAttainmentAndProgressViewModel : BaseViewModel
{
    private const string PupilsAverageLabel = "Pupils' average grade across 8 GCSE and equivalent subjects";

    private const AcademicYearSelection _currentAcademicYear = AcademicYearSelection.Current;

    public string? AcademicYearInfoParagraph => $"Information in this section is for the {_currentAcademicYear.GetDisplayName()} academic year.";

    // No Progress 8 scores available for the academic years 2024 to 2025 and 2025 to 2026 as no KS2 baseline available (due to covid)
    public bool ShowProgress8NotAvailableInfo => _currentAcademicYear.GetDisplayName() is "2024 to 2025" or "2025 to 2026";

    public required RelativeYearValues<AcademicPerformanceAttainmentAndProgressSingleYearViewModel> YearValues { get; init; }

    public required DisplayField<CodedDouble> LocalAuthorityAttainment8NonDisadvantagedScore { get; init; }
    public required DisplayField<CodedDouble> EnglandAttainment8NonDisadvantagedScore { get; init; }

    public required SeriesMeasureViewModel BreakdownNonDisadvantaged { get; init; }
    public required SeriesMeasureViewModel BreakdownGirlsBoys { get; init; }
    public required SeriesMeasureViewModel BreakdownEAL { get; init; }
    public required SeriesMeasureViewModel BreakdownNonMobile { get; init; }

    public bool ShowUTCCaveat { get; set; }

    public bool ShowStudioSchoolCaveat { get; set; }

    public bool ShowFurtherEducationCaveat { get; set; }

    public string? SecondarySchoolAccountabilityPriorAttainmentLinkUrl { get; set; }
    public bool SecondarySchoolAccountabilityPriorAttainmentNewTab { get; set; }

    public static AcademicPerformanceAttainmentAndProgressViewModel Map(
        string laName,
        string ageRangeFrom,
        TypeOfEstablishment typeOfEstablishment,
        AttainmentAndProgressModel attainmentAndProgressModel,
        UrlLinksOptions urlLinksOptions)
    {
        var laAverageLabel = CommonHelper.GetLocalAuthorityDisplayName(laName);

        var nonDisadvantagedBreakdownData = new SeriesMeasureViewModel
        {
            TableId = "breakdown-non-disadvantaged-table",
            TableHeader = "Pupil group (non-disadvantaged)",
            Labels = ["Score", "Pupils' average grade across their 8 best GCSE-level subjects"],
            Datasets =
                [
                    new DatasetMeasureViewModel {
                        Label = laAverageLabel,
                        Data = [new Measure
                            {
                                Value = attainmentAndProgressModel.LocalAuthorityAttainment8NonDisadvantagedScore, Unit = DataUnit.Score}]
                            },
                    new DatasetMeasureViewModel {
                        Label = "England average",
                        Data = [new Measure { Value = attainmentAndProgressModel.EnglandAttainment8NonDisadvantagedScore, Unit = DataUnit.Score }]
                    },
                ],
        };



        var girlBoyBreakdownData = new SeriesMeasureViewModel
        {
            TableId = "characteristics-girlboy-table",
            TableHeader = PupilGroup,
            Labels = ["Score", PupilsAverageLabel],
            Datasets =
            [
                new DatasetMeasureViewModel {
                    Label = "Girls",
                    Data = [
                        new Measure { Value = attainmentAndProgressModel.EstablishmentAttainment8GirlsScore, Unit = DataUnit.Score }
                    ]
                },
                new DatasetMeasureViewModel {
                    Label = "Boys",
                    Data = [
                        new Measure { Value = attainmentAndProgressModel.EstablishmentAttainment8BoysScore, Unit = DataUnit.Score }
                    ]
                },
                new DatasetMeasureViewModel {
                    Label = AllPupilsAtTheSchool,
                    Data = [
                        new Measure { Value = attainmentAndProgressModel.EstablishmentAttainment8Score.GetValueForYear(AcademicYearSelection.Current), Unit = DataUnit.Score }
                    ]
                },
            ]
        };

        var ealBreakdownData = new SeriesMeasureViewModel
        {
            TableId = "characteristics-eal-table",
            TableHeader = PupilGroup,
            Labels = ["Score", PupilsAverageLabel],
            Datasets =
            [
            new DatasetMeasureViewModel {
                    Label = "Pupils with EAL",
                    Data = [
                        new Measure { Value = attainmentAndProgressModel.EstablishmentAttainment8EALScore, Unit = DataUnit.Score }
                    ]
                },
                new DatasetMeasureViewModel {
                    Label = AllPupilsAtTheSchool,
                    Data = [
                        new Measure { Value = attainmentAndProgressModel.EstablishmentAttainment8Score.GetValueForYear(AcademicYearSelection.Current), Unit = DataUnit.Score }
                    ]
                },
            ]

        };

        var nonMobileBreakdownData = new SeriesMeasureViewModel
        {
            TableId = "characteristics-nonmobile-table",
            TableHeader = PupilGroup,
            Labels = ["Score", PupilsAverageLabel],
            Datasets =
            [
                new()
                {
                    Label = "Non-mobile pupils",
                    Data = [ new() { Value = attainmentAndProgressModel.EstablishmentAttainment8NonMobileScore, Unit = DataUnit.Score } ]
                },
                new()
                {
                    Label = AllPupilsAtTheSchool,
                    Data = [ new () { Value = attainmentAndProgressModel.EstablishmentAttainment8Score.GetValueForYear(AcademicYearSelection.Current), Unit = DataUnit.Score } ]
                },
            ]

        };

        int.TryParse(ageRangeFrom, out int ageFrom);

        return new AcademicPerformanceAttainmentAndProgressViewModel
        {
            URN = attainmentAndProgressModel.Urn,
            SchoolName = attainmentAndProgressModel.SchoolName ?? string.Empty,
            IsKS2 = attainmentAndProgressModel.IsKS2,
            IsKS4 = attainmentAndProgressModel.IsKS4,
            IsKS5 = attainmentAndProgressModel.IsKS5,
            LocalAuthorityAttainment8NonDisadvantagedScore = attainmentAndProgressModel.LocalAuthorityAttainment8NonDisadvantagedScore.ToDisplayField(),
            EnglandAttainment8NonDisadvantagedScore = attainmentAndProgressModel.EnglandAttainment8NonDisadvantagedScore.ToDisplayField(),
            YearValues = new RelativeYearValues<AcademicPerformanceAttainmentAndProgressSingleYearViewModel>
            {
                CurrentYear = AcademicPerformanceAttainmentAndProgressSingleYearViewModel.Map(laName, AcademicYearSelection.Current, attainmentAndProgressModel),
                PreviousYear = AcademicPerformanceAttainmentAndProgressSingleYearViewModel.Map(laName, AcademicYearSelection.Previous, attainmentAndProgressModel),
                TwoYearsAgo = AcademicPerformanceAttainmentAndProgressSingleYearViewModel.Map(laName, AcademicYearSelection.Previous2, attainmentAndProgressModel)
            },
            BreakdownNonDisadvantaged = nonDisadvantagedBreakdownData,
            ShowUTCCaveat = typeOfEstablishment == TypeOfEstablishment.UniversityTechnicalCollege,
            ShowStudioSchoolCaveat = typeOfEstablishment == TypeOfEstablishment.StudioSchools,
            ShowFurtherEducationCaveat
                = typeOfEstablishment == TypeOfEstablishment.FurtherEducation
                || (typeOfEstablishment != TypeOfEstablishment.UniversityTechnicalCollege
                    && typeOfEstablishment != TypeOfEstablishment.StudioSchools
                    && typeOfEstablishment != TypeOfEstablishment.FurtherEducation
                    && ageFrom >= 12),
            BreakdownGirlsBoys = girlBoyBreakdownData,
            BreakdownEAL = ealBreakdownData,
            BreakdownNonMobile = nonMobileBreakdownData,
            SecondarySchoolAccountabilityPriorAttainmentLinkUrl = urlLinksOptions.SecondarySchoolAccountabilityPriorAttainment.Url,
            SecondarySchoolAccountabilityPriorAttainmentNewTab = urlLinksOptions.SecondarySchoolAccountabilityPriorAttainment.NewTab
        };
    }
}
