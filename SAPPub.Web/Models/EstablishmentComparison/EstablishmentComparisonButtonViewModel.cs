namespace SAPPub.Web.Models.EstablishmentComparison
{
    public class EstablishmentComparisonButtonViewModel
    {
        public string Urn { get; set; } = default!;

        public string SchoolName { get; set; } = default!;

        public bool IsSaved { get; set; }

        public string SaveText { get; set; } = default!;

        public string SavedText { get; set; } = default!;

        public string ReturnUrl { get; set; } = default!;

        public bool ShowAddSuccessNotification { get; set; }

        public bool ShowRemoveSuccessNotification { get; set; }

        public bool IsComparisonLimitReached { get; set; }

        public string ComparisonPageUrl { get; set; } = default!;
    }
}
