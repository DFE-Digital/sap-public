using Microsoft.AspNetCore.Mvc.Rendering;
using SAPPub.Core.Entities;
using SAPPub.Core.Enums;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models;
using SAPPub.Web.Models.Charts;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS4;

public class AcademicPerformanceAttainmentAndProgressSingleYearViewModel
{
    public CodedDouble EstablishmentProgress8Score { get; init; }

    public CodedDouble EstablishmentProgress8CILower { get; init; }

    public CodedDouble EstablishmentProgress8CIUpper { get; init; }

    public string? EstablishmentProgress8Banding { get; init; }

    public required DisplayField<string> EstablishmentProgress8BandingContextDescription { get; init; }

    public CodedDouble LocalAuthorityProgress8Score { get; init; }

    public CodedDouble EstablishmentAttainment8Score { get; init; }
    public required DisplayField<CodedDouble> EstablishmentAttainment8DisadvantagedScore { get; init; }
    public required DisplayField<string> EstablishmentAttainment8ScoreContextDescription { get; init; }

    public CodedDouble LocalAuthorityAttainment8Score { get; init; }
    public required DisplayField<CodedDouble> LocalAuthorityAttainment8DisadvantagedScore { get; init; }
    public required DisplayField<CodedDouble> LocalAuthorityAttainment8NonDisadvantagedScore { get; init; }
    public required DisplayField<string> LocalAuthorityAttainment8ScoreContextDescription { get; init; }

    public CodedDouble EnglandAttainment8Score { get; init; }
    public required DisplayField<CodedDouble> EnglandAttainment8DisadvantagedScore { get; init; }
    public required DisplayField<CodedDouble> EnglandAttainment8NonDisadvantagedScore { get; init; }
    public required DisplayField<string> EnglandAttainment8ScoreContextDescription { get; init; }

    public CodedDouble EstablishmentProgress8TotalPupils { get; init; }

    public CodedDouble EstablishmentTotalPupils { get; init; }

    public required SeriesCodedDoubleViewModel BreakdownDisadvantaged { get; init; }
    public required SeriesCodedDoubleViewModel BreakdownNonDisadvantaged { get; init; }

    public static AcademicPerformanceAttainmentAndProgressSingleYearViewModel Map(string laName, AcademicYearSelection year, AttainmentAndProgressModel attainmentAndProgressModel)
    {
        var laAverageLabel = CommonHelper.GetLocalAuthorityDisplayName(laName);
        var establishmentAttainment8ContextSentence 
            = AttainmentHelper.EstablishmentAttainment8ContextStatement(attainmentAndProgressModel.EstablishmentAttainment8Score.GetValueForYear(year).Value);
        var englandAttainment8ContextSentence 
            = AttainmentHelper.NationalAttainment8ContextStatement(
                nationalScore: attainmentAndProgressModel.EnglandAttainment8Score.GetValueForYear(year).Value,
                schoolScore: attainmentAndProgressModel.EstablishmentAttainment8Score.GetValueForYear(year).Value); // TODO use the CodedDouble, niot the double?
        var localAuthorityAttainment8ContextSentence 
            = AttainmentHelper.LocalAuthorityAttainment8ContextStatement(
                localAuthorityScore: attainmentAndProgressModel.LocalAuthorityAttainment8Score.GetValueForYear(year).Value,
                schoolScore: attainmentAndProgressModel.EstablishmentAttainment8Score.GetValueForYear(year).Value);
        var establishmentProgress8BandingContextDescription 
            = AttainmentHelper.EstablishmentProgress8BandingContextStatement(attainmentAndProgressModel.EstablishmentProgress8Banding.GetValueForYear(year));

        var disadvantagedBreakdownGcseData = new SeriesCodedDoubleViewModel
        {
            Labels = ["Score", "Pupils' average grade across 8 GCSE and equivalent subjects"],
            Datasets =
                [
                    new DatasetCodedDoubleViewModel {
                        Label = "School",
                        Data = [attainmentAndProgressModel.EstablishmentAttainment8DisadvantagedScore.GetValueForYear(year)]
                    },
                    new DatasetCodedDoubleViewModel {
                        Label = laAverageLabel,
                        Data = [attainmentAndProgressModel.LocalAuthorityAttainment8DisadvantagedScore.GetValueForYear(year)]
                    },
                    new DatasetCodedDoubleViewModel {
                        Label = "England average",
                        Data = [attainmentAndProgressModel.EnglandAttainment8DisadvantagedScore.GetValueForYear(year)]
                    },
                ],
        };

        var nonDisadvantagedBreakdownGcseData = new SeriesCodedDoubleViewModel
        {
            Labels = ["Score", "Pupils' average grade across their 8 best GCSE-level subjects"],
            Datasets =
            [
                new DatasetCodedDoubleViewModel {
                            Label = laAverageLabel,
                            Data = [attainmentAndProgressModel.LocalAuthorityAttainment8NonDisadvantagedScore]
                        },
                        new DatasetCodedDoubleViewModel {
                            Label = "England average",
                            Data = [attainmentAndProgressModel.EnglandAttainment8NonDisadvantagedScore]
                        },
                    ],
        };

        return new AcademicPerformanceAttainmentAndProgressSingleYearViewModel
        {
            EstablishmentProgress8Score = attainmentAndProgressModel.EstablishmentProgress8Score.GetValueForYear(year),
            EstablishmentProgress8CILower = attainmentAndProgressModel.EstablishmentProgress8CILower.GetValueForYear(year),
            EstablishmentProgress8CIUpper = attainmentAndProgressModel.EstablishmentProgress8CIUpper.GetValueForYear(year),
            EstablishmentProgress8Banding = attainmentAndProgressModel.EstablishmentProgress8Banding.GetValueForYear(year),
            EstablishmentProgress8BandingContextDescription = establishmentProgress8BandingContextDescription,
            LocalAuthorityProgress8Score = attainmentAndProgressModel.LocalAuthorityProgress8Score.GetValueForYear(year),
            EstablishmentAttainment8Score = attainmentAndProgressModel.EstablishmentAttainment8Score.GetValueForYear(year),
            EstablishmentAttainment8DisadvantagedScore = attainmentAndProgressModel.EstablishmentAttainment8DisadvantagedScore.GetValueForYear(year).ToDisplayField(),
            LocalAuthorityAttainment8DisadvantagedScore = attainmentAndProgressModel.LocalAuthorityAttainment8DisadvantagedScore.GetValueForYear(year).ToDisplayField(),
            EnglandAttainment8DisadvantagedScore = attainmentAndProgressModel.EnglandAttainment8DisadvantagedScore.GetValueForYear(year).ToDisplayField(),
            LocalAuthorityAttainment8NonDisadvantagedScore = attainmentAndProgressModel.LocalAuthorityAttainment8NonDisadvantagedScore.ToDisplayField(),
            EnglandAttainment8NonDisadvantagedScore = attainmentAndProgressModel.EnglandAttainment8NonDisadvantagedScore.ToDisplayField(),
            EstablishmentAttainment8ScoreContextDescription = establishmentAttainment8ContextSentence != null
                ? $"This means that pupils generally scored the equivalent of {establishmentAttainment8ContextSentence} in their 8 best GCSE-level subjects.".ToDisplayField()
                : DisplayField<string>.NotAvailable(),
            LocalAuthorityAttainment8ScoreContextDescription = localAuthorityAttainment8ContextSentence != null
                ? $"{localAuthorityAttainment8ContextSentence}".ToDisplayField()
                : DisplayField<string>.NotAvailable(),
            EnglandAttainment8ScoreContextDescription = englandAttainment8ContextSentence != null
                ? $"{englandAttainment8ContextSentence}".ToDisplayField()
                : DisplayField<string>.NotAvailable(),
            LocalAuthorityAttainment8Score = attainmentAndProgressModel.LocalAuthorityAttainment8Score.GetValueForYear(year),
            EnglandAttainment8Score = attainmentAndProgressModel.EnglandAttainment8Score.GetValueForYear(year),
            EstablishmentProgress8TotalPupils = attainmentAndProgressModel.EstablishmentProgress8TotalPupils.GetValueForYear(year),
            EstablishmentTotalPupils = attainmentAndProgressModel.EstablishmentTotalPupils.GetValueForYear(year),
            BreakdownDisadvantaged = disadvantagedBreakdownGcseData,
            BreakdownNonDisadvantaged = nonDisadvantagedBreakdownGcseData
        };
    }
}

public class AcademicPerformanceAttainmentAndProgressViewModel : BaseViewModel
{
    private const AcademicYearSelection _currentAcademicYear = AcademicYearSelection.Current;
    public string? AcademicYearInfoParagraph => $"Information in this section is for the {SelectedAcademicYear.GetDisplayName()} academic year.";
    public AcademicYearSelection SelectedAcademicYear { get; set; } = _currentAcademicYear;

    public bool ShowProgress8NotAvailableInfo => SelectedAcademicYear == _currentAcademicYear;

    public bool ShowAttainment8Info => SelectedYearValues.EstablishmentAttainment8Score.HasValue;
    public bool ShowProgress8Info => SelectedYearValues.EstablishmentProgress8Score.HasValue;

    public AcademicPerformanceAttainmentAndProgressSingleYearViewModel SelectedYearValues => YearValues.GetValueForYear(SelectedAcademicYear);
    public RelativeYearValues<AcademicPerformanceAttainmentAndProgressSingleYearViewModel> YearValues { get; init; }

    public List<SelectListItem> AcademicYearsSelectList => [.. Enum.GetValues(typeof(AcademicYearSelection)).Cast<AcademicYearSelection>().Select(x => new SelectListItem
    {
        Text = x.GetDisplayName(),
        Value = x.ToString(),
    })];

    public static AcademicPerformanceAttainmentAndProgressViewModel Map(string laName, AttainmentAndProgressModel attainmentAndProgressModel, AcademicYearSelection selectedAcademicYear)
    {
        var laAverageLabel = CommonHelper.GetLocalAuthorityDisplayName(laName);
 
        return new AcademicPerformanceAttainmentAndProgressViewModel
        {
            URN = attainmentAndProgressModel.Urn,
            SchoolName = attainmentAndProgressModel.SchoolName ?? string.Empty,
            IsKS2 = attainmentAndProgressModel.IsKS2,
            IsKS4 = attainmentAndProgressModel.IsKS4,
            IsKS5 = attainmentAndProgressModel.IsKS5,
            SelectedAcademicYear = selectedAcademicYear,
            YearValues = new RelativeYearValues<AcademicPerformanceAttainmentAndProgressSingleYearViewModel>
            {
                CurrentYear = AcademicPerformanceAttainmentAndProgressSingleYearViewModel.Map(laName, AcademicYearSelection.Current, attainmentAndProgressModel),
                PreviousYear = AcademicPerformanceAttainmentAndProgressSingleYearViewModel.Map(laName, AcademicYearSelection.Previous, attainmentAndProgressModel),
                TwoYearsAgo = AcademicPerformanceAttainmentAndProgressSingleYearViewModel.Map(laName, AcademicYearSelection.Previous2, attainmentAndProgressModel)
            },
        };
    }
}
