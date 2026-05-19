namespace SAPPub.Web.Tests.UI.Models
{
    public class AxeNodeResult
    {
        public string Html { get; } = "";

        public AxeImpactValue? Impact { get; }

        public IList<string> Target { get; } = [];

        public IList<string>? XPath { get; } = [];

        public IList<string>? Ancestry { get; } = [];

        public IList<AxeCheckResult> Any { get; } = [];

        public IList<AxeCheckResult> All { get; } = [];

        public IList<AxeCheckResult> None { get; } = [];

        public string? FailureSummary { get; }
    }
}
