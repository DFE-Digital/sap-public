namespace SAPPub.Web.Areas.Profiles.ViewModels.Performance;

public class ScaledScoresViewModel
{
    public required string Column1Title { get; set; }
    public required IEnumerable<ScaledScoresDetailViewModel> Rows{ get; set; }
}
