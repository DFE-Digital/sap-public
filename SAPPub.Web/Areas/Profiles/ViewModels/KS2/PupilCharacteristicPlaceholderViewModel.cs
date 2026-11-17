using SAPPub.Core.Enums;
using SAPPub.Core.ServiceModels;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS2;

/// <summary>
/// This model is currently used as a placeholder to display the correct heading for the pupil characteristic pages, but will be expanded to include the data required for each page in future tickets.
/// </summary>
public class PupilCharacteristicPlaceholderViewModel : ProfileBaseViewModel
{
    public PupilCharacteristic SelectedPupilCharacteristic { get; set; }

    public string Heading => SelectedPupilCharacteristic.GetDisplayName() ?? string.Empty;

    public static PupilCharacteristicPlaceholderViewModel Map(EstablishmentMinimumServiceModel establishment, PupilCharacteristic characteristic)
    {
        return new PupilCharacteristicPlaceholderViewModel
        {
            URN = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            IsKS2 = establishment.IsKS2,
            IsKS4 = establishment.IsKS4,
            IsKS5 = establishment.IsKS5,
            SelectedPupilCharacteristic = characteristic,
        };
    }
}
