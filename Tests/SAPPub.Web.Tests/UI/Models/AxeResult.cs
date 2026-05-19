namespace SAPPub.Web.Tests.UI.Models
{
    public class AxeResult
    {
        public string Help { get; set; } = "";

        public string Id { get; set; } = "";

        public AxeImpactValue? Impact { get; set; }
            
        public IList<AxeNodeResult> Nodes { get; set; } = [];
    }
}