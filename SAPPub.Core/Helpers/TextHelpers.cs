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
            name = Regex.Replace(name, "[^a-zA-Z0-9 ]", "");
            name = Regex.Replace(name.Trim(), " +", "-");
            return name;

        }

        public static string EnsureHttpsUrl(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return string.Empty;

            if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                    return url;

            return $"https://{url}";
        }
    }
}
