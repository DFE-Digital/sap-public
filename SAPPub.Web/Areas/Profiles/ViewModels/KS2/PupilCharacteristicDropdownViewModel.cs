using Microsoft.AspNetCore.Mvc.Rendering;
using SAPPub.Core.Enums;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS2;

public class PupilCharacteristicDropdownViewModel
{
    public PupilCharacteristic SelectedPupilCharacteristic { get; set; } = PupilCharacteristic.AllPupils;

    public required Dictionary<string, string> RouteAttributes { get; set; }

    public List<SelectListItem> PupilCharacteristicSelectList =>
        [.. Enum.GetValues<PupilCharacteristic>().Select(x => new SelectListItem
        {
            Text = x.GetDisplayName(),
            Value = x.ToString(),
            Selected = x == SelectedPupilCharacteristic
        })];
}
