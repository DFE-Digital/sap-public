using SAPPub.Core.Enums;

namespace SAPPub.Core.Tests.EnumTests;

public class ProgressBandingExtensionsTests
{
    [Theory]
    [InlineData("well above average", ProgressBanding.WellAboveAverage)]
    [InlineData("above average", ProgressBanding.AboveAverage)]
    [InlineData("average", ProgressBanding.Average)]
    [InlineData("below average", ProgressBanding.BelowAverage)]
    [InlineData("well below average", ProgressBanding.WellBelowAverage)]
    [InlineData("WELL ABOVE AVERAGE", ProgressBanding.WellAboveAverage)]
    [InlineData("Above Average", ProgressBanding.AboveAverage)]
    [InlineData("AVERAGE", ProgressBanding.Average)]
    [InlineData("  average  ", ProgressBanding.Average)]
    [InlineData(" Below Average ", ProgressBanding.BelowAverage)]
    public void ToProgressBanding_ReturnsExpectedBanding(string value, ProgressBanding expected)
    {
        var result = value.ToProgressBanding();

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ToProgressBanding_ReturnsNull_ForBlankValues(string? value)
    {
        var result = value.ToProgressBanding();

        Assert.Null(result);
    }

    [Theory]
    [InlineData("SUPP")]
    [InlineData("not a banding")]
    [InlineData("z")]
    [InlineData("c")]
    public void ToProgressBanding_ReturnsNull_ForUnrecognizedValues(string value)
    {
        var result = value.ToProgressBanding();

        Assert.Null(result);
    }
}
