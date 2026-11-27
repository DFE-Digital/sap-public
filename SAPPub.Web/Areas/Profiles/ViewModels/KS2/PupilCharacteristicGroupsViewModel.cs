using SAPPub.Core.Enums;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models.Config;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS2;

public class PupilCharacteristicGroupsViewModel : ProfileBaseViewModel
{
    public required IReadOnlyList<PupilCharacteristicBaseViewModel> Groups { get; init; }
    public string? TechnicalGuideLinkUrl { get; init; }
    public bool TechnicalGuideLinkNewTab { get; init; }
    public string CurrentAcademicYearDisplayName => Ks4AcademicYearHelper.GetDisplayName(AcademicYearSelection.Current);

    public static PupilCharacteristicGroupsViewModel Map(
        KS2PupilPerformanceBandings[] performance,
        EstablishmentMinimumServiceModel establishment,
        UrlLinksOptions urlLinksOptions) => new()
        {
            URN = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            IsKS2 = establishment.IsKS2,
            IsKS4 = establishment.IsKS4,
            IsKS5 = establishment.IsKS5,
            TechnicalGuideLinkUrl = urlLinksOptions.PrimaryAttainmentGroupingsTechnicalGuide.Url,
            TechnicalGuideLinkNewTab = urlLinksOptions.PrimaryAttainmentGroupingsTechnicalGuide.NewTab,
            Groups = PupilCharacteristicBaseViewModel.MapCollectionWithoutBandings(
                performance, establishment, PupilCharacteristic.GirlsAndBoys, urlLinksOptions)
        };
}
