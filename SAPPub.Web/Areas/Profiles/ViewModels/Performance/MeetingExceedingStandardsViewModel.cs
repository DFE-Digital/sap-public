namespace SAPPub.Web.Areas.Profiles.ViewModels.Performance;

public class MeetingExceedingStandardsViewModel
{
    public required string Column1Title { get; set; }
    public required IEnumerable<MeetingExceedingStandardsDetailViewModel> Rows{ get; set; }
}
