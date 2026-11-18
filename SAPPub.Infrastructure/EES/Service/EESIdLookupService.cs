using Newtonsoft.Json;
using SAPPub.Infrastructure.EES.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAPPub.Infrastructure.EES.Service
{
    public sealed class EESIdLookupItem
    {
        [JsonProperty("key")]
        public string Key { get; set; } = string.Empty;

        [JsonProperty("value")]
        public string Value { get; set; } = string.Empty;
    }
    public class EESIdLookupService
    {
        private readonly Dictionary<string, EESIdLookupItem> _byId;
        private readonly Dictionary<string, EESIdLookupItem> _byCode;

        public EESIdLookupService(string jsonPath)
        {
            var json = File.ReadAllText(jsonPath);
            var rows = JsonConvert.DeserializeObject<IEnumerable<EESIdLookupItem>>(json) ?? [];

            _byId = rows
                .Where(x => !string.IsNullOrWhiteSpace(x.Key))
                .GroupBy(x => EESTransformations.NormalizeToken(x.Key))
                .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);

            _byCode = rows
                .Where(x => !string.IsNullOrWhiteSpace(x.Value))
                .GroupBy(x => EESTransformations.NormalizeToken(x.Value))
                .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);
        }

        public bool TryGetById(string id, out EESIdLookupItem? match)
        {
            return _byId.TryGetValue(EESTransformations.NormalizeToken(id), out match);
        }

        public bool TryGetByCode(string code, out EESIdLookupItem? match)
        {
            return _byCode.TryGetValue(EESTransformations.NormalizeToken(code), out match);
        }

        public bool TryGetByIdOrCode(string idOrCode, out EESIdLookupItem? match)
        {
            if (TryGetById(idOrCode, out match))
            {
                return true;
            }

            return TryGetByCode(idOrCode, out match);
        }

        public string? GetConstituentPart(string idOrCode)
        {
            if (!TryGetByIdOrCode(idOrCode, out var match) || match == null)
            {
                return null;
            }

            if (string.Equals(EESTransformations.NormalizeToken(idOrCode), EESTransformations.NormalizeToken(match.Key), StringComparison.OrdinalIgnoreCase))
            {
                return match.Value;
            }

            return match.Key;
        }
    }
}
