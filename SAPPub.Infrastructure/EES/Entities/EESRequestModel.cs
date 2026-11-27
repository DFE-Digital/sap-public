using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAPPub.Infrastructure.EES.Entities
{
    public class EESRequestModel
    {
        [JsonProperty("indicators")]
        public IEnumerable<string>? Indicators { get; set; }

        [JsonProperty("debug")]
        public bool IsDebug { get; set; } = false;

        [JsonProperty("pageSize")]
        public int PageSize { get; set; } = 1000;

        [JsonProperty("page")]
        public int Page { get; set; } = 1;

        [JsonProperty("criteria")]
        public EESRequestCriteriaGrouping? Criteria { get; set; }
    }

    public class EESRequestCriteriaGrouping
    {
        [JsonProperty("and")]
        public EESRequestFacets[]? CriteriaAnd { get; set; }

        [JsonProperty("or")]
        public EESRequestFacets[]? CriteriaOr { get; set; }

        [JsonProperty("not")]
        public EESRequestFacets[]? CriteriaNot { get; set; }

        [JsonProperty("facets")]
        public EESRequestFacets[]? CriteriaFacets { get; set; }
    }

    public class EESRequestFacets
    {
        [JsonProperty("filters")]
        public EESRequestFilter? Filters { get; set; }
        [JsonProperty("geographicLevels")]
        public EESRequestFilter? GeographicLevels { get; set; }
        [JsonProperty("locations")]
        public EESRequestFilterLocation? Locations { get; set; }
        [JsonProperty("timePeriods")]
        public EESRequestTimePeriods? TimePeriods { get; set; }
    }

    public class EESRequestFilter
    {
        [JsonProperty("in")]
        public IEnumerable<string>? In { get; set; }
        [JsonProperty("notIn")]
        public IEnumerable<string>? NotIn { get; set; }
        [JsonProperty("eq")]
        public string? Equal { get; set; }
        [JsonProperty("notEq")]
        public string? NotEqual { get; set; }
    }

    public class EESRequestFilterLocation
    {
        [JsonProperty("in")]
        public IEnumerable<EESRequestLocation>? In { get; set; }
        [JsonProperty("notIn")]
        public IEnumerable<EESRequestLocation>? NotIn { get; set; }
        [JsonProperty("eq")]
        public EESRequestLocation? Equal { get; set; }
        [JsonProperty("notEq")]
        public EESRequestLocation? NotEqual { get; set; }
    }

    public class EESRequestTimePeriods
    {
        [JsonProperty("in")]
        public IEnumerable<EESRequestTimePeriod>? In { get; set; }
        [JsonProperty("notIn")]
        public IEnumerable<EESRequestTimePeriod>? NotIn { get; set; }
        [JsonProperty("eq")]
        public EESRequestTimePeriod? Equal { get; set; }
        [JsonProperty("notEq")]
        public EESRequestTimePeriod? NotEqual { get; set; }
        [JsonProperty("gt")]
        public EESRequestTimePeriod? GreaterThan { get; set; }
        [JsonProperty("gte")]
        public EESRequestTimePeriod? GreaterThanEqual { get; set; }
        [JsonProperty("lt")]
        public EESRequestTimePeriod? LessThan { get; set; }
        [JsonProperty("lte")]
        public EESRequestTimePeriod? LessThanEqual { get; set; }
    }

    public class EESRequestTimePeriod
    {
        [JsonProperty("period")]
        public string? Period { get; set; }
        [JsonProperty("code")]
        public string? Code { get; set; }
    }

    public class EESRequestLocation
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("code")]
        /// Could be ONS code, or Local Authority Code (E10000002)
        public string? Code { get; set; }

        [JsonProperty("oldCode")]
        /// Old three digit local authority code (e.g. 100, 350, 500)
        public string? OldLocalAuthorityCode { get; set; }

        [JsonProperty("ukprn")]
        public string? UKPRN { get; set; }

        [JsonProperty("laEstab")]
        /// The concatenataion of the local authority code and the establishment number (e.g. 1001234)
        public string? LocalAuthorityEstablishmentCode { get; set; }

        [JsonProperty("urn")]
        public string? URN { get; set; }

        [JsonProperty("level")]
        public string? Level { get; set; }

    }
}
