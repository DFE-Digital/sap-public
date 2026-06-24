using SAPPub.Core.Attributes;
using SAPPub.Core.Entities;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Serialization;

namespace SAPPub.Core.ApplicationServices
{

    public interface IEntityPropertyService
    {
        public string GetColumnNamesForType(Type type);
    }

    public sealed class EntityPropertyService : IEntityPropertyService
    {
        private readonly ConcurrentDictionary<string, List<string>> _propMappings = new();

        public EntityPropertyService()
        {
            var asm = typeof(Establishment).Assembly;
            var types = asm.GetTypes()
                .Where(a => a!.FullName!.StartsWith("SAPPub.Core.Entities"));

            foreach (var type in types)
            {
                var key = type.FullName ?? type.Name;
                var props = type
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(a => !a.IsDefined(typeof(IgnoreDataMemberAttribute)))
                    .Select(a => {
                        var att = a.GetCustomAttribute<DbColumnNameAttribute>();
                        return att?.ColumnName ?? a.Name;
                      }
                ).ToList();
                
                _propMappings.TryAdd(key, props);
            }
        }

        public string GetColumnNamesForType<T>() => GetColumnNamesForType(typeof(T));

        public string GetColumnNamesForType(Type type)
        {
            if (type is null)
            {
                return "";
            }

            var key = type.FullName ?? type.Name;

            if (_propMappings.TryGetValue(key, out var list))
            {
                return string.Join(",", list.Select(a=>$"\"{a}\""));
            }

            return "";
        }
    }
}
