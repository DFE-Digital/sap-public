using System.Reflection;

namespace SAPPub.E2E.TestDataExpectationGenerator;

/// <summary>
/// Populates model objects from CSV rows, matching each string property to a CSV header
/// through <see cref="CsvColumnResolver"/>.
/// </summary>
internal static class CsvModelReader
{
    public static List<T> Read<T>(
        string csvPath,
        Func<IReadOnlyDictionary<string, string>, bool>? rowFilter = null,
        IReadOnlySet<string>? optionalColumns = null)
        where T : new() =>
        ReadWithRows<T>(csvPath, rowFilter, optionalColumns)
            .Select(item => item.Model)
            .ToList();

    public static List<(IReadOnlyDictionary<string, string> Row, T Model)> ReadWithRows<T>(
        string csvPath,
        Func<IReadOnlyDictionary<string, string>, bool>? rowFilter = null,
        IReadOnlySet<string>? optionalColumns = null)
        where T : new()
    {
        var properties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType == typeof(string) && p.SetMethod is not null)
            .Select(p => (Property: p, Column: CsvColumnResolver.GetColumnName(p)))
            .ToList();

        var results = new List<(IReadOnlyDictionary<string, string> Row, T Model)>();
        foreach (var row in CsvReader.ReadRows(csvPath))
        {
            if (rowFilter is not null && !rowFilter(row))
            {
                continue;
            }

            var model = new T();
            foreach (var (property, column) in properties)
            {
                if (row.TryGetValue(column, out var value))
                {
                    property.SetValue(model, value);
                }
                else if (optionalColumns is null || !optionalColumns.Contains(column))
                {
                    throw new InvalidOperationException(
                        $"Column '{column}' required by {typeof(T).Name}.{property.Name} was not found in {Path.GetFileName(csvPath)}.");
                }
            }

            results.Add((row, model));
        }

        return results;
    }
}
