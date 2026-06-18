namespace SAPPub.Web.Models.MySchools;

public class AddSchoolButtonViewModel
{
    public string Urn { get; set; } = default!;

    public bool IsSaved { get; set; }

    public string SaveText { get; set; } = default!;

    public string SavedText { get; set; } = default!;

    public bool IsListLimitReached { get; set; }

    public bool IsFeatureEnabled { get; set; }

    public bool IsSearchPage { get; set; }
}
