using SAPPub.Web.Tests.UI.Models;
using System.Collections.Concurrent;
using System.Text;

namespace SAPPub.Web.Tests.UI.Helpers
{
    public static class AccessibilityReportHelper
    {
        private static readonly string COLOUR_CRITICAL = "red";
        private static readonly string COLOUR_SERIOUS = "#b8860b";
        private static readonly string MARKDOWN_FILENAME = "accessibility-violations.md";
        private static readonly ConcurrentDictionary<string, string> ViolationReports = new();

        /// <summary>
        /// Used to ensure any test runs with Accessibility are kept clean.
        /// This method deletes any previous .md files as the writer only appends and things could get confusing.
        /// </summary>
        /// <returns></returns>
        public static void CleanupExistingReports()
        {
            ViolationReports.Clear();

            var reportPath = GetReportPath();
            if (File.Exists(reportPath))
            {
                File.Delete(reportPath);
            }
        }

        public static void AddViolations(string pageName, IList<AxeResult> violations)
        {
            var markdown = BuildTestViolationMarkdown(pageName, violations);
            if (string.IsNullOrWhiteSpace(markdown))
            {
                return;
            }

            ViolationReports[pageName] = markdown;
        }

        public static async Task FlushReportAsync()
        {

            var reportPath = GetReportPath();
            var markdown = BuildFinalReportMarkdown();

            if (File.Exists(reportPath))
            {
                File.Delete(reportPath);
            }

            await File.WriteAllTextAsync(reportPath, markdown);
        }

        public static string GetReportPath()
        {
            var reportDirectory = Environment.GetEnvironmentVariable("ACCESSIBILITY_REPORT_DIR");
            if (string.IsNullOrWhiteSpace(reportDirectory))
            {
                reportDirectory = Path.Combine(GetSolutionPath(), "Tests", "accessibility-results");
            }

            Directory.CreateDirectory(reportDirectory);
            var reportPath = Path.Combine(reportDirectory, MARKDOWN_FILENAME);
            return reportPath;
        }

        public static string GetAxeScriptPath(string? axeScriptPath)
        {
            if (axeScriptPath is not null)
            {
                return axeScriptPath;
            }

            var solutionPath = GetSolutionPath();

            axeScriptPath = Path.Combine(solutionPath, "SAPPub.Web", "node_modules", "axe-core", "axe.min.js");

            if (!File.Exists(axeScriptPath))
            {
                throw new FileNotFoundException();
            }

            return axeScriptPath;
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

        public static string BuildTestViolationMarkdown(string pageName, IList<AxeResult> violations)
        {
            if (!violations.Any())
            {
                return string.Empty;
            }

            var builder = new StringBuilder(500);

            builder.AppendLine($"#### {pageName}");
            builder.AppendLine($"**Violations**: {violations.Count}");
            builder.AppendLine("");

            foreach (var violation in violations)
            {
                var spanColour = violation.Impact switch
                {
                    AxeImpactValue.Critical => COLOUR_CRITICAL,
                    AxeImpactValue.Serious => COLOUR_SERIOUS,
                    _ => "black"
                };

                builder.AppendLine($"<span style='color:{spanColour}'>**IMPACT**: {violation.Impact.ToString()}</span>");
                builder.AppendLine("");
                builder.AppendLine("");
                builder.AppendLine($"**{violation.Id}**: {violation.Help}");
                builder.AppendLine("");

                foreach (var node in violation.Nodes)
                {
                    builder.AppendLine("");
                    builder.AppendLine($"**Target**: {string.Join(", ", node.Target.Select(a => a))}");
                    builder.AppendLine("");
                    builder.AppendLine($"{node.FailureSummary}");
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

            var builder = new StringBuilder();

            foreach (var report in ViolationReports.OrderBy(a => a.Key, StringComparer.OrdinalIgnoreCase))
            {
                builder.Append(report.Value);
            }

            return builder.ToString();
        }

    }
}