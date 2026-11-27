using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAPPub.Infrastructure.EES.Entities
{
    public class EESResponseModel
    {
        [JsonProperty("paging")]
        public EESResponsePaging? Paging { get; set; }

        [JsonProperty("results")]
        public EESResponseResult[]? Results { get; set; }
    }

    public class EESResponsePaging
    {
        [JsonProperty("page")]
        public int Page { get; set; }
        [JsonProperty("pageSize")]
        public int PageSize { get; set; }
        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }
        [JsonProperty("totalResults")]
        public int TotalResults { get; set; }
    }

    public class EESResponseResult
    {
        [JsonProperty("timePeriod")]
        public EESRequestTimePeriod? TimePeriod { get; set; }

        [JsonProperty("geographicLevel")]
        public string? GeographicLevel { get; set; }

        [JsonProperty("locations")]
        public Dictionary<string, string>? Locations { get; set; }

        [JsonProperty("filters")]
        public Dictionary<string, string>? Filters { get; set; }

        [JsonProperty("values")]
        public Dictionary<string, string>? Values { get; set; }

    }
}
