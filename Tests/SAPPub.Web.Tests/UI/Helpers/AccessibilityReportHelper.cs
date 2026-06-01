using Deque.AxeCore.Commons;
using Humanizer;
using System.Collections.Concurrent;
using System.Text;

namespace SAPPub.Web.Tests.UI.Helpers
{
    public static class AccessibilityReportHelper
    {
        private static readonly string COLOUR_CRITICAL = "red";
        private static readonly string COLOUR_SERIOUS = "#b8860b";
        private static readonly string MARKDOWN_FILENAME = "accessibility-violations.md";
        private static readonly ConcurrentDictionary<string, AccessibilityPageReport> ViolationReports = new();

        public static void Reset()
        {
            ViolationReports.Clear();
        }

        public static void AddViolations(string pageName, string url, IList<AxeResultItem> violations)
        {
            if (violations?.Any() != true)
            {
                return;
            }

            var filteredOrderedViolations = violations
                .Where(a => GetSeverityRank(a.Impact) == 1 || GetSeverityRank(a.Impact) == 2)       // only critical & severe
                .OrderBy(a => GetSeverityRank(a.Impact));

            ViolationReports[pageName] = new AccessibilityPageReport(pageName, BuildTestViolationMarkdown(pageName, url, filteredOrderedViolations));
        }

        public static async Task FlushReportAsync(string? reportDirectory = null)
        {

            var reportPath = GetReportPath(reportDirectory);
            var markdown = BuildFinalReportMarkdown();

            if (File.Exists(reportPath))
            {
                File.Delete(reportPath);
            }

            if (string.IsNullOrWhiteSpace(markdown))
            {
                markdown = "No critical or severe accessibility issues found";
            }
            
            await File.WriteAllTextAsync(reportPath, markdown);
        }

        public static string GetReportPath(string? reportDirectory)
        {
            reportDirectory ??= Environment.GetEnvironmentVariable("ACCESSIBILITY_REPORT_DIR");
            if (string.IsNullOrWhiteSpace(reportDirectory))
            {
                reportDirectory = Path.Combine(GetSolutionPath(), "Tests", "accessibility-results");
            }

            Directory.CreateDirectory(reportDirectory);
            var reportPath = Path.Combine(reportDirectory, MARKDOWN_FILENAME);
            return reportPath;
        }

        public static string GetSolutionPath()
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);

            while (directory != null && directory.GetFiles("*.sln").Length == 0)
            {
                directory = directory.Parent;
            }

            if (directory is null)
            {
                throw new InvalidOperationException("Could not find solution file path");
            }

            return directory.FullName;
        }

        public static string BuildTestViolationMarkdown(string pageName, string url, IEnumerable<AxeResultItem> violations)
        {
            if (!violations.Any())
            {
                return string.Empty;
            }

            var builder = new StringBuilder(1000);

            builder.AppendLine($"#### {pageName}<br/>");
            builder.AppendLine($"{url}<br/>");
            builder.AppendLine($"**Violations**: {violations.Count()}<br/>");

            foreach (var violation in violations)
            {
                var spanColour = violation.Impact switch
                {
                    "critical" => COLOUR_CRITICAL,
                    "serious" => COLOUR_SERIOUS,
                    _ => "black"
                };

                builder.AppendLine($"<span style='color:{spanColour}'>**IMPACT**: {violation.Impact.Titleize()}</span>");
                builder.AppendLine("<br/>");
                builder.AppendLine($"**{violation.Id}**: {violation.Help}<br/>");

                foreach (var node in violation.Nodes)
                {
                    builder.AppendLine("");
                    builder.AppendLine($"**Target**: {string.Join(", ", node.Target)}");
                }
            }
            builder.AppendLine("<br/><br/>");
            return builder.ToString();
        }

        private static string BuildFinalReportMarkdown()
        {
            if (ViolationReports.IsEmpty)
            {
                return string.Empty;
            }

            var builder = new StringBuilder(1000);

            foreach (var report in ViolationReports.Values)
            {
                builder.Append(report.Markdown);
            }

            return builder.ToString();
        }

        private static int GetSeverityRank(string impact)
        {
            return impact switch
            {
                "critical" => 1,
                "serious" => 2,
                "moderate" => 3,
                "minor" => 4,
                _ => 0,
            };
        }

        private sealed record AccessibilityPageReport(string PageName, string Markdown);
    }
}