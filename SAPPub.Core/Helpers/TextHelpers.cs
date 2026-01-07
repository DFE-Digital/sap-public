using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
    }
}
