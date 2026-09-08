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
    public string? AcademicYearInfoParagraph => $"Information in this section is for the {SelectedAcademicYear.GetDisplayName()} academic year.";
    public AcademicYearSelection SelectedAcademicYear { get; set; } = _currentAcademicYear;

    public bool ShowProgress8NotAvailableInfo => SelectedAcademicYear == _currentAcademicYear;

    public bool ShowAttainment8Info => SelectedYearValues?.EstablishmentAttainment8Score.HasValue ?? false;
    public bool ShowProgress8Info => SelectedYearValues?.EstablishmentProgress8Score.HasValue ?? false;

    public AcademicPerformanceAttainmentAndProgressSingleYearViewModel SelectedYearValues => YearValues.GetValueForYear(SelectedAcademicYear) ?? AcademicPerformanceAttainmentAndProgressSingleYearViewModel.Empty;
    public required RelativeYearValues<AcademicPerformanceAttainmentAndProgressSingleYearViewModel> YearValues { get; init; }

    public required DisplayField<CodedDouble> LocalAuthorityAttainment8NonDisadvantagedScore { get; init; }
    public required DisplayField<CodedDouble> EnglandAttainment8NonDisadvantagedScore { get; init; }

    public required SeriesMeasureViewModel BreakdownNonDisadvantaged { get; init; }

    public List<SelectListItem> AcademicYearsSelectList => [.. Enum.GetValues(typeof(AcademicYearSelection)).Cast<AcademicYearSelection>().Select(x => new SelectListItem
    {
        Text = x.GetDisplayName(),
        Value = x.ToString(),
    })];

    public static AcademicPerformanceAttainmentAndProgressViewModel Map(string laName, AttainmentAndProgressModel attainmentAndProgressModel, AcademicYearSelection selectedAcademicYear)
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

        return new AcademicPerformanceAttainmentAndProgressViewModel
        {
            URN = attainmentAndProgressModel.Urn,
            SchoolName = attainmentAndProgressModel.SchoolName ?? string.Empty,
            IsKS2 = attainmentAndProgressModel.IsKS2,
            IsKS4 = attainmentAndProgressModel.IsKS4,
            IsKS5 = attainmentAndProgressModel.IsKS5,
            SelectedAcademicYear = selectedAcademicYear,
            LocalAuthorityAttainment8NonDisadvantagedScore = attainmentAndProgressModel.LocalAuthorityAttainment8NonDisadvantagedScore.ToDisplayField(),
            EnglandAttainment8NonDisadvantagedScore = attainmentAndProgressModel.EnglandAttainment8NonDisadvantagedScore.ToDisplayField(),
            YearValues = new RelativeYearValues<AcademicPerformanceAttainmentAndProgressSingleYearViewModel>
            {
                CurrentYear = AcademicPerformanceAttainmentAndProgressSingleYearViewModel.Map(laName, AcademicYearSelection.Current, attainmentAndProgressModel),
                PreviousYear = AcademicPerformanceAttainmentAndProgressSingleYearViewModel.Map(laName, AcademicYearSelection.Previous, attainmentAndProgressModel),
                TwoYearsAgo = AcademicPerformanceAttainmentAndProgressSingleYearViewModel.Map(laName, AcademicYearSelection.Previous2, attainmentAndProgressModel)
            },
            BreakdownNonDisadvantaged = nonDisadvantagedBreakdownData
        };
    }
}
