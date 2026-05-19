using System.Text.Json.Serialization;

namespace SAPPub.Web.Tests.UI.Models
{
    public class AxeResults
    {
        /// <summary>
        /// These results indicate what elements passed the rules.
        /// </summary>
        [JsonPropertyName("passes")]
        public IList<AxeResult> Passes { get; set; } = [];

        /// <summary>
        /// These results indicate what elements failed the rules.
        /// </summary>
        public IList<AxeResult> Violations { get; set; } = [];

        /// <summary>
        /// These results indicate which rules did not run because no matching content was found on the page. 
        /// For example, with no video, those rules won't run.
        /// </summary>
        public IList<AxeResult> Incomplete { get; set; } = [];

        /// <summary>
        /// These results were aborted and require further testing. 
        /// This can happen either because of technical restrictions to what the rule can test, or because a javascript error occurred.
        /// </summary>
        public IList<AxeResult> Inapplicable { get; set; } = [];
    }
}
