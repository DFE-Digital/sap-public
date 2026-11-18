using Newtonsoft.Json;
using SAPPub.Infrastructure.EES.Config;
using SAPPub.Infrastructure.EES.Entities;
using SAPPub.Infrastructure.EES.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SAPPub.Infrastructure.EES.Service
{
    public class EESAPIService(EESClient client)
    {
        private readonly EESClient _client = client;

        public async Task<IEnumerable<T>> GetData<T>(string[] ids) where T : class, new()
        {
            var configurationType = EESMapping.LookupFromModel(typeof(T));

            var configurationMapping = EESMapping.GetMapping(configurationType) ?? throw new ArgumentException("No mapping found for {configurationType}", nameof(configurationType));

            var lookupService = new EESIdLookupService($"C:\\Working\\sap-public\\SAPPub.Infrastructure\\EES\\Data\\LookupList_{configurationMapping.FilterIdType}.json");
            var inputMapped = EESTransformations.GetInputMap<T>();

            var distinctIndicators = inputMapped.Select(x => x.IndicatorId).Distinct();
            var distinctFilters = inputMapped.SelectMany(x => x.Filters).Distinct();
            var distinctTimePeriods = inputMapped.Select(x => x.TimePeriod).Distinct().Select(x => new EESRequestTimePeriod { Period = x, Code = EESConstants.YearIdentifier });

            var distinctLocations = EESTransformations.FormatLocations(configurationMapping, ids);

            var testModel = new EESRequestModel
            {
                Indicators = distinctIndicators,
                IsDebug = false,
                Criteria = new EESRequestCriteriaGrouping
                {
                    CriteriaAnd =
            [
                new EESRequestFacets
                    {
                        Filters = new EESRequestFilter
                        {
                            In = distinctFilters
                        },
                        Locations = distinctLocations,
                        TimePeriods = new EESRequestTimePeriods
                        {
                            In = distinctTimePeriods
                        },
                        GeographicLevels = new EESRequestFilter{
                            Equal = configurationMapping.FilterIdType
                        }
                    }
            ]
                }
            };

            var doAction = await _client.GetDataQuery(configurationMapping.PublicationId, configurationMapping.CurrentMajorVersion, testModel);

            var flattenedValues = EESTransformations.ParseResponseValues(configurationMapping, doAction);

            var modelsBySch = flattenedValues
                .Where(x => !string.IsNullOrWhiteSpace(x.Id))
                .GroupBy(x => x.Id, StringComparer.OrdinalIgnoreCase)
                .Select(
                    group =>
                    {
                        var model = new T();
                        EESTransformations.ApplyFromEes(model, group, group.Key);

                        var idProperty = typeof(T).GetProperty("Id");
                        if (idProperty?.CanWrite == true && idProperty.PropertyType == typeof(string))
                        {
                            var mappedId = group.Key;
                            if (lookupService?.TryGetById(group.Key, out var lookup) == true && lookup != null)
                            {
                                mappedId = lookup.Value;
                            }

                            idProperty.SetValue(model, mappedId);
                        }

                        return model;
                    });

            return modelsBySch;
        }

        public async Task<EESMetadataModel> GetMetadataModel(EESMappingType mappingType)
        {
            var configurationMapping = EESMapping.GetMapping(mappingType) ?? throw new ArgumentException($"No mapping found for {mappingType}", nameof(mappingType));
            var result = await _client.GetMetadata(configurationMapping.PublicationId, configurationMapping.CurrentMajorVersion);
            return result;
        }
    }
}
