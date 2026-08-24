using SAPPub.Core.ValueObjects;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Areas.Profiles.ViewModels.Performance;

public class ScaledScoresDetailViewModel
{
    public required string RowTitle { get; set; }
    public required DisplayField<CodedDouble> AverageReadingScore { get; set; }
    public required DisplayField<CodedDouble> AverageMathsScore { get; set; }
}
