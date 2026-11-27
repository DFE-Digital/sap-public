using System;
using System.Collections.Generic;
using System.Text;

namespace SAPPub.Infrastructure.EES.Entities
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class EESInputAttribute(string indicatorId, string timePeriod, params string[] filters) : Attribute
    {
        public string IndicatorId { get; } = indicatorId;
        public string[] Filters { get; } = filters ?? [];
        public string TimePeriod { get; } = timePeriod;
    }

    public sealed class EESResultValue(string indicatorId, string value, string timePeriod, string id, params string[] filters)
    {
        public string IndicatorId { get; set; } = indicatorId;
        public string Value { get; set; } = value;
        public string[] Filters { get; set; } = filters ?? [];
        public string TimePeriod { get; set; } = timePeriod;
        public string Id { get; set; } = id;
    }

    public class EESInput
    {
        public EESInput(string propertyName, string indicatorId, string[] filters, string timePeriod)
        {
            PropertyName = propertyName;
            IndicatorId = indicatorId;
            Filters = filters;
            TimePeriod = timePeriod;
        }

        public EESInput(string propertyName, string indicatorId, string filter, string timePeriod)
        {
            PropertyName = propertyName;
            IndicatorId = indicatorId;
            Filters = [filter];
            TimePeriod = timePeriod;
        }

        public string PropertyName { get; set; }
        public string IndicatorId { get; set; }
        public string[] Filters { get; set; }
        public string TimePeriod { get; set; }

    }
}
