using Newtonsoft.Json;

namespace SAPPub.Web.Tests.UI.Models
{
    public class AxeViolation
    {
        public string Id { get; set; } = "";

        [JsonProperty("impact")]
        public string Impact { get; set; } = "";

        public string Description { get; set; } = "";

        public string Help { get; set; } = "";

        public string HelpUrl { get; set; } = "";

        public List<AxeNode> Nodes { get; set;  } = [];
    }
}
