using SAPPub.Core.Enums;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Areas.Profiles.ViewModels.Overview;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models.Config;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS2;

public class PupilCharacteristicBaseViewModel : KS2BandingsBaseViewModel
{
    public PupilCharacteristic SelectedPupilCharacteristic { get; set; }

    public string? TechnicalGuideLinkUrl { get; set; }
    public bool TechnicalGuideLinkNewTab { get; set; }

    public string CurrentAcademicYearDisplayName => Ks4AcademicYearHelper.GetDisplayName(AcademicYearSelection.Current);

    public string Heading => SelectedPupilCharacteristic.GetDisplayName() ?? string.Empty;

    public string CharacteristicName { get; set; } = string.Empty;

    public string LocalAuthorityName { get; set; } = string.Empty;

    public CodedDouble? EstablishmentReadingScore { get; init; }
    public CodedDouble? EstablishmentWritingScore { get; init; }
    public CodedDouble? EstablishmentMathsScore { get; init; }

    public CodedDouble? LocalAuthorityReadingScore { get; init; }
    public CodedDouble? LocalAuthorityWritingScore { get; init; }
    public CodedDouble? LocalAuthorityMathsScore { get; init; }

    public CodedDouble? EnglandReadingScore { get; init; }
    public CodedDouble? EnglandWritingScore { get; init; }
    public CodedDouble? EnglandMathsScore { get; init; }

    public IReadOnlyList<PupilProgressSectionViewModel> Sections =>
    [
        CreateSection("Reading", 0, EstablishmentReadingScore, LocalAuthorityReadingScore, EnglandReadingScore),
        CreateSection("Writing", 1, EstablishmentWritingScore, LocalAuthorityWritingScore, EnglandWritingScore),
        CreateSection("Maths", 2, EstablishmentMathsScore, LocalAuthorityMathsScore, EnglandMathsScore)
    ];

    public IReadOnlyList<PupilProgressSectionViewModel> SectionsWithSubsections =>
    [
        CreateSection("Reading", 0, EstablishmentReadingScore, LocalAuthorityReadingScore, EnglandReadingScore),
        CreateSection("Writing", 1, EstablishmentWritingScore, LocalAuthorityWritingScore, EnglandWritingScore),
        CreateSection("Maths", 2, EstablishmentMathsScore, LocalAuthorityMathsScore, EnglandMathsScore)
    ];

    private PupilProgressSectionViewModel CreateSection(
        string subject,
        int bandingIndex,
        CodedDouble? score,
        CodedDouble? localAuthorityScore,
        CodedDouble? englandScore) => new()
        {
            Subject = subject,
            SchoolName = SchoolName,
            LocalAuthorityName = LocalAuthorityName,
            Banding = Ks2Bandings is null ? null : Ks2Bandings![bandingIndex],
            Score = score.ToDisplayField(),
            LocalAuthorityScore = localAuthorityScore.ToDisplayField(),
            EnglandScore = englandScore.ToDisplayField(),
            SmallNumberOfPupilsWarning = Ks2SmallNumberOfPupilsWarning
        };

    public static PupilCharacteristicBaseViewModel Map(
        KS2PupilPerformanceBandings kS2PupilPerformanceBandings, 
        EstablishmentMinimumServiceModel establishment, 
        PupilCharacteristic characteristic,
        UrlLinksOptions urlLinksOptions)
    {
        return new PupilCharacteristicBaseViewModel
        {
            URN = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            IsKS2 = establishment.IsKS2,
            IsKS4 = establishment.IsKS4,
            IsKS5 = establishment.IsKS5,
            SelectedPupilCharacteristic = characteristic,
            Ks2Bandings =
            [
                ProgressBandingSummaryViewModel.Map("Reading", kS2PupilPerformanceBandings.EstablishmentReadingProgressBanding, kS2PupilPerformanceBandings.EstablishmentReadingProgressSignificance),
                ProgressBandingSummaryViewModel.Map("Writing", kS2PupilPerformanceBandings.EstablishmentWritingProgressBanding, kS2PupilPerformanceBandings.EstablishmentWritingProgressSignificance),
                ProgressBandingSummaryViewModel.Map("Maths", kS2PupilPerformanceBandings.EstablishmentMathsProgressBanding, kS2PupilPerformanceBandings.EstablishmentMathsProgressSignificance)
            ],
            EstablishmentReadingScore = kS2PupilPerformanceBandings.EstablishmentReadingScore,
            EstablishmentWritingScore = kS2PupilPerformanceBandings.EstablishmentWritingScore,
            EstablishmentMathsScore = kS2PupilPerformanceBandings.EstablishmentMathsScore,
            LocalAuthorityReadingScore = kS2PupilPerformanceBandings.LocalAuthorityReadingScore,
            LocalAuthorityWritingScore = kS2PupilPerformanceBandings.LocalAuthorityWritingScore,
            LocalAuthorityMathsScore = kS2PupilPerformanceBandings.LocalAuthorityMathsScore,
            EnglandReadingScore = kS2PupilPerformanceBandings.EnglandAverageReadingScore,
            EnglandWritingScore = kS2PupilPerformanceBandings.EnglandAverageWritingScore,
            EnglandMathsScore = kS2PupilPerformanceBandings.EnglandAverageMathsScore,
            LocalAuthorityName = GetLaName(establishment),
            TechnicalGuideLinkUrl = urlLinksOptions.PrimaryAttainmentGroupingsTechnicalGuide.Url,
            TechnicalGuideLinkNewTab = urlLinksOptions.PrimaryAttainmentGroupingsTechnicalGuide.NewTab
        };
    }

    public static IReadOnlyList<PupilCharacteristicBaseViewModel> MapCollectionWithoutBandings(
        KS2PupilPerformanceBandings[] kS2PupilPerformanceBandings,
        EstablishmentMinimumServiceModel establishment,
        PupilCharacteristic characteristic,
        UrlLinksOptions urlLinksOptions)
    {
        return
            [.. kS2PupilPerformanceBandings.Select(pupilPerformance => new PupilCharacteristicBaseViewModel() {
                URN = establishment.URN,
                SchoolName = establishment.EstablishmentName,
                IsKS2 = establishment.IsKS2,
                IsKS4 = establishment.IsKS4,
                IsKS5 = establishment.IsKS5,
                Ks2Bandings = null,
                SelectedPupilCharacteristic = characteristic,
                CharacteristicName = pupilPerformance.CharacteristicName,
                EstablishmentReadingScore = pupilPerformance.EstablishmentReadingScore,
                EstablishmentWritingScore = pupilPerformance.EstablishmentWritingScore,
                EstablishmentMathsScore = pupilPerformance.EstablishmentMathsScore,
                LocalAuthorityReadingScore = pupilPerformance.LocalAuthorityReadingScore,
                LocalAuthorityWritingScore = pupilPerformance.LocalAuthorityWritingScore,
                LocalAuthorityMathsScore = pupilPerformance.LocalAuthorityMathsScore,
                EnglandReadingScore = pupilPerformance.EnglandAverageReadingScore,
                EnglandWritingScore = pupilPerformance.EnglandAverageWritingScore,
                EnglandMathsScore = pupilPerformance.EnglandAverageMathsScore,
                LocalAuthorityName = GetLaName(establishment),
                TechnicalGuideLinkUrl = urlLinksOptions.PrimaryAttainmentGroupingsTechnicalGuide.Url,
                TechnicalGuideLinkNewTab = urlLinksOptions.PrimaryAttainmentGroupingsTechnicalGuide.NewTab
            })];
    }

    static string GetLaName(EstablishmentMinimumServiceModel establishment)
    {
        return string.IsNullOrWhiteSpace(establishment.LAName)
                            ? "Not available"
                            : $"{establishment.LAName} average";
    }
}
