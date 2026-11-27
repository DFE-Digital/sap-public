namespace SAPPub.Web.Areas.Profiles.ViewModels.KS2;

public class PupilCharacteristicGroupsViewModel : ISchoolProfileViewModel
{
    public required string SchoolName { get; init; }    
    public required IReadOnlyList<PupilCharacteristicBaseViewModel> Groups { get; init; }
}
