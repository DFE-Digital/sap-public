using Microsoft.AspNetCore.Mvc.Rendering;
using SAPPub.Core.Enums;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models;
using SAPPub.Web.Models.Config;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS2;

public class AcademicPerformancePupilProgressViewModel : BaseViewModel
{
    private const AcademicYearSelection _currentAcademicYear = AcademicYearSelection.Current;
    public string? AcademicYearInfoParagraph => $"Information in this section is for the {SelectedAcademicYear.GetDisplayName()} academic year.";
    public AcademicYearSelection SelectedAcademicYear { get; set; } = _currentAcademicYear;
    public bool ShowDataNotAvailableInfo => SelectedAcademicYear == _currentAcademicYear || SelectedAcademicYear == AcademicYearSelection.Previous;
    public bool ShowReadingScore => EstablishmentReadingScore.Score.HasValue;
    public bool ShowWritingScore => EstablishmentWritingScore.Score.HasValue;
    public bool ShowMathsScore => EstablishmentMathsScore.Score.HasValue;
    public string? PrimarySchoolAccountabilityLinkUrl { get; set; }
    public bool PrimarySchoolAccountabilityLinkNewTab { get; set; }
    public required ProgressScoreModel EstablishmentReadingScore { get; init; }
    public required ProgressScoreModel EstablishmentWritingScore { get; init; }
    public required ProgressScoreModel EstablishmentMathsScore { get; init; }
    public CodedDouble LaReadingAverage { get; init; }
    public CodedDouble LaWritingAverage { get; init; }
    public CodedDouble LaMathsAverage { get; init; }

    public List<SelectListItem> AcademicYearsSelectList => 
        [.. Enum.GetValues(typeof(AcademicYearSelection)).Cast<AcademicYearSelection>().Select(x => new SelectListItem
    {
        Text = x.GetDisplayName(),
        Value = x.ToString(),
    })];

    public static AcademicPerformancePupilProgressViewModel Map(
        KS2PupilPerformance ks2PupilPerformance,
        EstablishmentMinimumServiceModel establishment, 
        AcademicYearSelection selectedAcademicYear,
        UrlLinksOptions urlLinksOptions)
    {
        return new AcademicPerformancePupilProgressViewModel
        {
            URN = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            IsKS2 = establishment.IsKS2,
            IsKS4 = establishment.IsKS4,
            IsKS5 = establishment.IsKS5,
            SelectedAcademicYear = selectedAcademicYear,
            EstablishmentReadingScore = new ProgressScoreModel
            {
                Score = ks2PupilPerformance.EstablishmentReadingScore,
                ConfidenceLevelUpper = ks2PupilPerformance.EstablishmentReadingConfidenceUpper,
                ConfidenceLevelLower = ks2PupilPerformance.EstablishmentReadingConfidenceLower,
                BandingRating = ks2PupilPerformance.EstablishmentReadingDescription
            },
            LaReadingAverage = ks2PupilPerformance.LaReadingScore,
            EstablishmentWritingScore = new ProgressScoreModel
            {
                Score = ks2PupilPerformance.EstablishmentWritingScore,
                ConfidenceLevelUpper = ks2PupilPerformance.EstablishmentWritingConfidenceUpper,
                ConfidenceLevelLower = ks2PupilPerformance.EstablishmentWritingConfidenceLower,
                BandingRating = ks2PupilPerformance.EstablishmentWritingDescription
            },
            LaWritingAverage = ks2PupilPerformance.LaWritingScore,
            EstablishmentMathsScore = new ProgressScoreModel
            {
                Score = ks2PupilPerformance.EstablishmentMathsScore,
                ConfidenceLevelUpper = ks2PupilPerformance.EstablishmentMathsConfidenceUpper,
                ConfidenceLevelLower = ks2PupilPerformance.EstablishmentMathsConfidenceLower,
                BandingRating = ks2PupilPerformance.EstablishmentMathsDescription
            },
            LaMathsAverage = ks2PupilPerformance.LaMathsScore,
            PrimarySchoolAccountabilityLinkUrl = urlLinksOptions.PrimarySchoolAccountability.Url,
            PrimarySchoolAccountabilityLinkNewTab = urlLinksOptions.PrimarySchoolAccountability.NewTab
        };
    }
}