using SAPPub.Core.ValueObjects;
using SAPPub.Web.Areas.Profiles.ViewModels.Overview;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS2;

public class PupilProgressSectionViewModel
{
    public required string Subject { get; init; }
    public required string SchoolName { get; init; }
    public required string LocalAuthorityName { get; init; }
    public required ProgressBandingSummaryViewModel Banding { get; init; }
    public required DisplayField<CodedDouble> Score { get; init; }
    public required DisplayField<CodedDouble> LocalAuthorityScore { get; init; }
    public required DisplayField<CodedDouble> EnglandScore { get; init; }
    public string? SmallNumberOfPupilsWarning { get; init; }

    public List<PupilProgressSectionViewModel> SubSections
    { 
    
    }
}
