using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SAPPub.EESCodeGenerator.Helpers
{
    public static class ModelGeneratorTransforms
    {
        public static string GetUniqueName(string baseName, HashSet<string> usedNames)
        {
            var candidate = string.IsNullOrWhiteSpace(baseName) ? "Value" : baseName;
            var suffix = 1;

            while (!usedNames.Add(candidate))
            {
                candidate = $"{baseName}_{suffix++}";
            }

            return candidate;
        }

        public static string ToValidIdentifier(string label)
        {
            var cleaned = Regex.Replace(label.Trim(), "[^a-zA-Z0-9_ ]", " ");
            var parts = cleaned
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(static p => char.ToUpperInvariant(p[0]) + p[1..]);

            var combined = string.Concat(parts);

            if (string.IsNullOrWhiteSpace(combined))
            {
                return "Value";
            }

            if (char.IsDigit(combined[0]))
            {
                combined = "_" + combined;
            }

            return combined;
        }

        public static string EscapeString(string value)
        {
            return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }
    }
}
