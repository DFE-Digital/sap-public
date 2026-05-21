using Deque.AxeCore.Commons;
using SAPPub.Web.Tests.UI.Helpers;

namespace SAPPub.Web.Tests.UI
{
    public class AccessibilityReportHelperTests
    {
        [Fact]
        public void BuildTestViolationMarkdown_WithNoVioloations_ReturnsEmptyString()
        {
            var result = AccessibilityReportHelper.BuildTestViolationMarkdown("Page name", "/test-url", []);
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public async Task AddViolations_OnlyIncludesCriticalAndSerious_AndOrders()
        {
            // Arrange
            var violations = new[]
            {
                CreateViolation("critical", "color-contrast", "Critical_HelpString", [".govuk-link"]),
                CreateViolation("minor", "minor-violoation", "Minor_HelpString", [".minor"]),
                CreateViolation("serious", "label", "Serious_HelpString", ["#search-input"]),
            };

            AccessibilityReportHelper.AddViolations("Results page", "/results", violations);
            await AccessibilityReportHelper.FlushReportAsync();

            // Act
            var result = await File.ReadAllTextAsync(AccessibilityReportHelper.GetReportPath());

            // Assert
            var split = result.Split("\r\n");
            Assert.Contains("Critical", split[4]);
            Assert.Contains("Serious", split[10]);
            Assert.DoesNotContain("minor", result);
        }

        [Fact]
        public async Task FlushReportAsync_WritesCorrectTextWhenNoSuitableViolationsFound()
        {
            // Arrange
            var violations = new[]
            {
                CreateViolation("minor", "minor-violoation", "Minor_HelpString", [".minor"]),
            };

            AccessibilityReportHelper.AddViolations("Results page", "/results", violations);
            await AccessibilityReportHelper.FlushReportAsync();

            // Act
            var result = await File.ReadAllTextAsync(AccessibilityReportHelper.GetReportPath());

            // Assert
            Assert.Equal("No critical or severe accessibility issues found", result);
        }

        private static AxeResultItem CreateViolation(string impact, string id, string help, string[] targets)
        {
            return new AxeResultItem
            {
                Impact = impact,
                Id = id,
                Help = help,
                Nodes = [.. targets.Select(a => new AxeResultNode { Target = new AxeSelector(a) })]
            };
        }
    }
}
