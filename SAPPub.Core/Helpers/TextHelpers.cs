using System.Text.RegularExpressions;

namespace SAPPub.Core.Helpers
{
    public class TextHelpers
    {
        public static string ConcatListToString(List<string> items, string separator = ", ")
        {
            if (items == null || items.Count == 0)
            {
                return string.Empty;
            }
            return string.Join(separator, items.Where(x => !string.IsNullOrEmpty(x)));
        }

        public static string CleanForUrl(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;

            name = name.ToLowerInvariant();

            // Remove all non-alphanumeric and non-space characters
            name = Regex.Replace(name, @"[^a-z0-9 ]+", "");

            // Replace one or more spaces with a single dash
            name = Regex.Replace(name.Trim(), @"\s+", "-");

            name = name.Trim('-');

            return name;

        }
    }
}
