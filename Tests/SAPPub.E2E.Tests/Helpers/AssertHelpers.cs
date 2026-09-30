using System.Globalization;

namespace SAPPub.E2E.Tests.Helpers;

public static class AssertHelpers
{
    /// <summary>
    /// Convert the string values to doubles to 2 dp and compare them and allow for values to be one of the known 'not available' codes. 
    /// This is to avoid issues with rounding differences when comparing string values.
    /// We can't compare the string values themselves because there is not a consistent pattern
    /// for the number of dps when displaying
    /// </summary>
    /// <param name="expected"></param>
    /// <param name="actual"></param>
    /// <param name="precision"></param>
    public static void AssertNumericEqual(
    string expected,
    string actual,
    int precision = 2)
    {
        actual = actual.Trim().TrimEnd('%');
        var expectedParseSuccess = double.TryParse(expected, CultureInfo.InvariantCulture, out var expectedValue);
        var actualParseSuccess = double.TryParse(actual, CultureInfo.InvariantCulture, out var actualValue);

        if(!expectedParseSuccess || !actualParseSuccess)
        {
            if (expected == "c" || expected == "z")
            {
                Assert.Equal("Not available", actual);
                return;
            }
            else throw new ArgumentException($"Unable to parse value to double. Expected: '{expected}', Actual: '{actual}'");
        }
        Assert.Equal(
            expectedValue,
            actualValue,
            precision);
    }
}
