using SAPPub.Infrastructure.EES.Config;
using SAPPub.Infrastructure.EES.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SAPPub.Infrastructure.EES.Helpers
{
    public class EESTransformations
    {
        public static IEnumerable<EESResultValue> ParseResponseValues(EESMapping configurationMapping, EESResponseModel? response)
        {
            if (response?.Results == null)
            {
                yield break;
            }

            foreach (var row in response.Results)
            {
                if (row?.Values == null)
                {
                    continue;
                }

                var criteriaTokens = GetCriteriaTokens(row);

                foreach (var value in row.Values)
                {
                    var indicatorId = NormalizeToken(value.Key);
                    var timePeriod = NormalizeToken(row.TimePeriod?.Period ?? string.Empty);
                    var locationId = NormalizeToken(GetLocationId(configurationMapping.FilterIdType, row));

                    if (string.IsNullOrWhiteSpace(indicatorId))
                    {
                        continue;
                    }

                    yield return new EESResultValue(
                        indicatorId,
                        NormalizeToken(value.Value),
                        timePeriod,
                        locationId,
                        criteriaTokens);
                }
            }
        }


        public static string[] GetCriteriaTokens(EESResponseResult row)
        {
            var filterValues = row.Filters?.Values ?? Enumerable.Empty<string>();
            var locationValues = row.Locations?.Values ?? Enumerable.Empty<string>();

            return [.. filterValues
                .Concat(locationValues)
                .Select(NormalizeToken)
                .Where(token => !string.IsNullOrWhiteSpace(token))
                .Distinct(StringComparer.OrdinalIgnoreCase)];
        }

        public static string GetLocationId(string check, EESResponseResult row)
        {
            if (row.Locations == null)
            {
                return string.Empty;
            }

            return row.Locations.TryGetValue(check, out var schLocation)
                ? schLocation ?? string.Empty
                : string.Empty;
        }

        public static EESRequestFilterLocation FormatLocations(EESMapping configurationMapping, string[] ids)
        {
            if (configurationMapping.FilterIdType == EESConstants.FilterSchool)
            {
                return new EESRequestFilterLocation
                {
                    In = ids.Select(x => new EESRequestLocation { Level = configurationMapping.FilterIdType, URN = x })
                };
            }
            else if (configurationMapping.FilterIdType == EESConstants.FilterLocalAuthority)
            {
                return new EESRequestFilterLocation
                {
                    In = ids.Select(x => new EESRequestLocation { Level = configurationMapping.FilterIdType, OldLocalAuthorityCode = x })
                };
            }
            else if (configurationMapping.FilterIdType == EESConstants.FilterNational)
            {
                return new EESRequestFilterLocation
                {
                    Equal = new EESRequestLocation { Code = EESConstants.NationalLocationCode, Level = EESConstants.NationalLocationLevel }
                };
            }
            else
            {
                throw new InvalidOperationException($"Unsupported FilterIdType: {configurationMapping.FilterIdType}");

            }
        }

        public static IEnumerable<EESInput> GetInputMap<T>()
        {
            var map = new List<EESInput>();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes<EESInputAttribute>(inherit: false);
                foreach (var attribute in attributes)
                {
                    map.Add(new EESInput(property.Name, attribute.IndicatorId, attribute.Filters, attribute.TimePeriod));
                }
            }

            return map;
        }

        public static T ApplyFromEes<T>(T target, IEnumerable<EESResultValue> values, string? id = null) where T : class
        {
            ArgumentNullException.ThrowIfNull(target);

            var rows = values?.ToList() ?? [];
            if (!string.IsNullOrWhiteSpace(id))
            {
                rows = rows
                    .Where(row => string.Equals(
                        NormalizeToken(row.Id),
                        NormalizeToken(id),
                        StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (rows.Count == 0)
            {
                return target;
            }

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType == typeof(string) && p.CanWrite);

            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes<EESInputAttribute>(inherit: false);
                foreach (var attribute in attributes)
                {
                    var match = rows.FirstOrDefault(row =>
                        string.Equals(NormalizeToken(row.IndicatorId), NormalizeToken(attribute.IndicatorId), StringComparison.OrdinalIgnoreCase)
                        && string.Equals(NormalizeToken(row.TimePeriod), NormalizeToken(attribute.TimePeriod), StringComparison.OrdinalIgnoreCase)
                        && ContainsAllFilters(row.Filters, attribute.Filters));

                    if (match == null)
                    {
                        continue;
                    }

                    property.SetValue(target, match.Value ?? string.Empty);
                    break;
                }
            }

            return target;
        }

        public static bool ContainsAllFilters(IEnumerable<string> rowFilters, IEnumerable<string> requiredFilters)
        {
            var filterSet = rowFilters
                .Where(f => !string.IsNullOrWhiteSpace(f))
                .Select(NormalizeToken)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            return requiredFilters
                .Where(f => !string.IsNullOrWhiteSpace(f))
                .Select(NormalizeToken)
                .All(filterSet.Contains);
        }

        public static string NormalizeToken(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
        }
    }

}