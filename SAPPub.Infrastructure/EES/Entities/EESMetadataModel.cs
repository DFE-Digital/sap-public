using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAPPub.Infrastructure.EES.Entities
{
    public class EESMetadataModel
    {
        [JsonProperty("filters")]
        public IEnumerable<EESMetadataFilter>? Filters { get; set; }

        [JsonProperty("indicators")]
        public IEnumerable<EESMetadataIndicators>? Indicators { get; set; }

        [JsonProperty("geographicLevels")]
        public IEnumerable<EESMetadataLocationLevel>? GeographicLevels { get; set; }

        [JsonProperty("locations")]
        public IEnumerable<EESMetadataLocation>? Locations { get; set; }
    }

    public class EESMetadataFilter
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("column")]
        public string? Column { get; set; }

        [JsonProperty("label")]
        public string? Label { get; set; }

        [JsonProperty("hint")]
        public string? Hint { get; set; }

        [JsonProperty("options")]
        public IEnumerable<EESMetadataFilterOption>? Options { get; set; }
    }

    public class EESMetadataFilterOption
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("label")]
        public string? Label { get; set; }
    }

    public class EESMetadataIndicators
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("column")]
        public string? Column { get; set; }

        [JsonProperty("label")]
        public string? Label { get; set; }

        [JsonProperty("decimalPlaces")]
        public int DecimalPlaces { get; set; }
    }

    public class EESMetadataLocation
    {
        [JsonProperty("level")]
        public EESMetadataLocationLevel? Level { get; set; }
        [JsonProperty("options")]
        public IEnumerable<EESMetadataLocationOption>? Options { get; set; }
    }

    public class EESMetadataLocationLevel
    {
        [JsonProperty("code")]
        public string? Code { get; set; }

        [JsonProperty("level")]
        public string? Level { get; set; }
    }

    public class EESMetadataLocationOption
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("label")]
        public string? Label { get; set; }
        [JsonProperty("urn")]
        public string? URN { get; set; }
        [JsonProperty("laEstab")]
        public string? LaEstab { get; set; }

        [JsonProperty("oldCode")]
        public string? OldCode { get; set; }

        [JsonProperty("code")]
        public string? Code { get; set; }
    }
}
