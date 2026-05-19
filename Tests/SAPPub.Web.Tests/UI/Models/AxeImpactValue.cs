using System.Text.Json.Serialization;

namespace SAPPub.Web.Tests.UI.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AxeImpactValue
    {
        Minor,
        Moderate,
        Serious,
        Critical
    }
}
