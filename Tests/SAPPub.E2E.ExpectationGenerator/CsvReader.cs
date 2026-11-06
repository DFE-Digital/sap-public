using System.Text;

namespace SAPPub.E2E.TestDataExpectationGenerator;

/// <summary>
/// Minimal CSV parser. Handles quoted fields, escaped quotes, and commas or line breaks inside quotes.
/// </summary>
internal static class CsvReader
{
    public static IEnumerable<Dictionary<string, string>> ReadRows(string path)
    {
        using var reader = new StreamReader(path);

        var header = ReadRecord(reader)
            ?? throw new InvalidOperationException($"CSV file is empty: {path}");

        while (ReadRecord(reader) is { } record)
        {
            if (record.Count == 1 && record[0].Length == 0)
            {
                continue;
            }

            var row = new Dictionary<string, string>(header.Count, StringComparer.Ordinal);
            for (var i = 0; i < header.Count; i++)
            {
                row[header[i]] = i < record.Count ? record[i] : string.Empty;
            }

            yield return row;
        }
    }

    private static List<string>? ReadRecord(StreamReader reader)
    {
        if (reader.Peek() < 0)
        {
            return null;
        }

        var fields = new List<string>();
        var field = new StringBuilder();
        var inQuotes = false;

        int next;
        while ((next = reader.Read()) >= 0)
        {
            var c = (char)next;

            if (inQuotes)
            {
                if (c == '"')
                {
                    if (reader.Peek() == '"')
                    {
                        field.Append('"');
                        reader.Read();
                    }
                    else
                    {
                        inQuotes = false;
                    }
                }
                else
                {
                    field.Append(c);
                }

                continue;
            }

            switch (c)
            {
                case '"':
                    inQuotes = true;
                    break;
                case ',':
                    fields.Add(field.ToString());
                    field.Clear();
                    break;
                case '\r':
                    if (reader.Peek() == '\n')
                    {
                        reader.Read();
                    }

                    fields.Add(field.ToString());
                    return fields;
                case '\n':
                    fields.Add(field.ToString());
                    return fields;
                default:
                    field.Append(c);
                    break;
            }
        }

        fields.Add(field.ToString());
        return fields;
    }
}
