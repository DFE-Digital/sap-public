using System.Globalization;

namespace SAPPub.Integration.Tests.Helpers;

public static class AssertHelpers
{
    /// <summary>
    /// Convert the string values to doubles to 2 dp and compare them. 
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

        Assert.Equal(
            double.Parse(expected, CultureInfo.InvariantCulture),
            double.Parse(actual, CultureInfo.InvariantCulture),
            precision);
    }
}
