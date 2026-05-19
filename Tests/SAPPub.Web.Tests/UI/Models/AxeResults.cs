namespace SAPPub.Web.Tests.UI.Models
{
    public class AxeResults
    {
        /// <summary>
        /// These results indicate what elements failed the rules.
        /// </summary>
        public IList<AxeResult> Violations { get; set; } = [];
    }
}