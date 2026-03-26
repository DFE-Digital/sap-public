using SAPData.Filters;
using Xunit;

namespace SAPData.Unit.Tests
{
    public class SqlViewFilterProviderTests
    {
        [Theory]
        [InlineData("2026-03-10", "2022-09-12")]
        [InlineData("2026-09-11", "2022-09-12")]
        [InlineData("2026-09-12", "2023-09-12")]
        [InlineData("2026-09-13", "2023-09-12")]
        public void GetAcademicYearCutoffDate_returns_expected_cutoff_date(string today, string expected)
        {
            var testDate = DateTime.Parse(today);
            var cutoff = SqlViewFilterProvider.GetAcademicYearCutoffDate(testDate);
            Assert.Equal(expected, cutoff);
        }
    }
}
