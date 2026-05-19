namespace SAPPub.Web.Tests.UI.Models
{
    public class AxeNode
    {
        public string Html { get; set; } = "";

        public string TargetSummary { get; set; } = "";

        public List<string> Target { get; set; } = [];

        public string FailureSummary { get; set; } = "";
    }
}
