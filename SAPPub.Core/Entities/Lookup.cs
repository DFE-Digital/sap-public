using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public class Lookup
    {
        public string Id { get; set; } = string.Empty;
        public string LookupType { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
