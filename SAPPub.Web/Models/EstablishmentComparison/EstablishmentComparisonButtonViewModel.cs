namespace SAPPub.Web.Models.EstablishmentComparison
{
    public class EstablishmentComparisonButtonViewModel
    {
        public string Urn { get; set; } = default!;

        public bool IsSaved { get; set; }

        public string SaveText { get; set; } = default!;

        public string SavedText { get; set; } = default!;

        public bool IsComparisonLimitReached { get; set; }

        public string AddedSchoolListPageUrl { get; set; } = default!;

        public bool IsFeatureEnabled { get; set; }

        public bool IsSearchPage { get; set; }
    }
}
