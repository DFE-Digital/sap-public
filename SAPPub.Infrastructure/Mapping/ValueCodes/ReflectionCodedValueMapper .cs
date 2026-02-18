using SAPPub.Core.ValueObjects;
using System.Collections.Concurrent;
using System.Reflection;

namespace SAPPub.Infrastructure.Mapping.ValueCodes;

public sealed class ReflectionCodedValueMapper : ICodedValueMapper
{
    // Cache per entity type so we only do reflection once
    private static readonly ConcurrentDictionary<Type, Action<object>> _appliers = new();

    public void Apply<T>(IEnumerable<T> items) where T : class
    {
        var applier = _appliers.GetOrAdd(typeof(T), BuildApplier);
        foreach (var item in items)
            applier(item);
    }

    public void Apply<T>(T item) where T : class
    {
        var applier = _appliers.GetOrAdd(typeof(T), BuildApplier);
        applier(item);
    }

    private static Action<object> BuildApplier(Type entityType)
    {
        // Find all properties like Foo_Bar_Pct_Coded : CodedDouble
        var codedProps = entityType
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.CanRead
                        && p.PropertyType == typeof(CodedDouble)
                        && p.Name.EndsWith("_Coded", StringComparison.OrdinalIgnoreCase))
            .ToArray();

        if (codedProps.Length == 0)
            return _ => { }; // no-op

        // Map codedProp -> numericProp + reasonProp (if they exist)
        var mappings = new List<(PropertyInfo coded, PropertyInfo? numeric, PropertyInfo? reason)>();

        foreach (var coded in codedProps)
        {
            var baseName = coded.Name[..^"_Coded".Length]; // remove suffix

            // numeric property expected to be double?
            var numeric = entityType.GetProperty(baseName, BindingFlags.Instance | BindingFlags.Public);

            // reason property expected to be string and named {baseName}_Reason
            var reason = entityType.GetProperty(baseName + "_Reason", BindingFlags.Instance | BindingFlags.Public);

            // We only set if writable and compatible
            if (numeric is { CanWrite: true } && numeric.PropertyType != typeof(double?))
                numeric = null;

            if (reason is { CanWrite: true } && reason.PropertyType != typeof(string))
                reason = null;

            // Even if one of numeric/reason is missing, still support the other
            mappings.Add((coded, numeric, reason));
        }

        return obj =>
        {
            foreach (var (coded, numeric, reason) in mappings)
            {
                var cv = (CodedDouble)coded.GetValue(obj)!;

                if (numeric != null)
                    numeric.SetValue(obj, cv.Value);

                if (reason != null)
                    reason.SetValue(obj, cv.Reason ?? string.Empty);
            }
        };
    }
}

