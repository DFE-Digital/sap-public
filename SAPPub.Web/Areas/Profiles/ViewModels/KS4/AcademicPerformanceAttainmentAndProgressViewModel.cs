using Microsoft.AspNetCore.Mvc.Rendering;
using SAPPub.Core.Entities;
using SAPPub.Core.Enums;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models;
using SAPPub.Web.Models.Charts;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS4;

public class AcademicPerformanceAttainmentAndProgressViewModel : BaseViewModel
{
    private const AcademicYearSelection _currentAcademicYear = AcademicYearSelection.Current;
    public string? AcademicYearInfoParagraph => $"Information in this section is for the {_currentAcademicYear.GetDisplayName()} academic year.";

    // No Progress 8 scores available for the academic years 2024 to 2025 and 2025 to 2026 as no KS2 baseline available (due to covid)
    //public bool ShowProgress8NotAvailableInfo => _currentAcademicYear.GetDisplayName() is not "2024 to 2025" and not "2025 to 2026";
    public bool ShowProgress8NotAvailableInfo => _currentAcademicYear.GetDisplayName() is "2024 to 2025" or "2025 to 2026";

    public required RelativeYearValues<AcademicPerformanceAttainmentAndProgressSingleYearViewModel> YearValues { get; init; }

    public required DisplayField<CodedDouble> LocalAuthorityAttainment8NonDisadvantagedScore { get; init; }
    public required DisplayField<CodedDouble> EnglandAttainment8NonDisadvantagedScore { get; init; }

    public required SeriesMeasureViewModel BreakdownNonDisadvantaged { get; init; }

    public bool ShowUTCCaveat { get; set; }

    public bool ShowStudioSchoolCaveat { get; set; }

    public bool ShowFurtherEducationCaveat { get; set; }

    public List<SelectListItem> AcademicYearsSelectList => [.. Enum.GetValues(typeof(AcademicYearSelection)).Cast<AcademicYearSelection>().Select(x => new SelectListItem
    {
        Text = x.GetDisplayName(),
        Value = x.ToString(),
    })];

    public static AcademicPerformanceAttainmentAndProgressViewModel Map(
        string laName,
        string ageRangeFrom,
        TypeOfEstablishment typeOfEstablishment,
        AttainmentAndProgressModel attainmentAndProgressModel)
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
                    && ageFrom >= 12)
        };
    }
}
