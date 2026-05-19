namespace SAPPub.Web.Tests.UI.Models
{
    public class AxeResult
    {
        public string Description { get; } = "";

        public string Help { get; } = "";

        public string HelpUrl { get; } = "";

        public string Id { get; } = "";

        public AxeImpactValue? Impact { get; }

        public IList<string> Tags { get; } = [];

        public IList<AxeNodeResult> Nodes { get; } = [];
    }
}