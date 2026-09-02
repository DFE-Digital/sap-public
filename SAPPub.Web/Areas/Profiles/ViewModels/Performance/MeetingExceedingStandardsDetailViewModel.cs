using SAPPub.Core.ValueObjects;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Areas.Profiles.ViewModels.Performance;

public class MeetingExceedingStandardsDetailViewModel
{
    public required string RowTitle { get; set; }
    public required DisplayField<CodedDouble> MeetingStandard { get; set; }
    public required DisplayField<CodedDouble> ExceedingStandard { get; set; }
}
