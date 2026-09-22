using SAPPub.Core.Enums;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Tests.Unit.Helpers;

public class ProgressBandingHelperTests
{
    private static readonly CodedString AverageDescription = new("Average description", "", "");

    private static ProgressBandingDescriptions CreateDescriptions() =>
        new(
            WellAboveAverage: new CodedString("Well above average description", "", ""),
            AboveAverage: new CodedString("Above average description", "", ""),
            Average: AverageDescription,
            BelowAverage: new CodedString("Below average description", "", ""),
            WellBelowAverage: new CodedString("Well below average description", "", ""));

    [Theory]
    [InlineData(ProgressBanding.WellAboveAverage, "well above average")]
    [InlineData(ProgressBanding.AboveAverage, "above average")]
    [InlineData(ProgressBanding.Average, "average")]
    [InlineData(ProgressBanding.BelowAverage, "below average")]
    [InlineData(ProgressBanding.WellBelowAverage, "well below average")]
    public void ToBandingContextStatement_Enum_ReturnsStatementWithDescription_WhenDescriptionAvailable(ProgressBanding banding, string displayName)
    {
        var descriptions = CreateDescriptions();

        var result = ((ProgressBanding?)banding).ToBandingContextStatement(descriptions);

        Assert.True(result.IsAvailable);
        Assert.Equal($"This is {displayName} because {descriptions.GetDescriptionFor(banding)}.", result.DisplayText());
    }

    [Fact]
    public void ToBandingContextStatement_Enum_ReturnsStatementWithoutDescription_WhenDescriptionNotAvailable()
    {
        var result = ((ProgressBanding?)ProgressBanding.Average).ToBandingContextStatement(ProgressBandingDescriptions.Empty);

        Assert.True(result.IsAvailable);
        Assert.Equal("This is average.", result.DisplayText());
    }

    [Fact]
    public void ToBandingContextStatement_Enum_ReturnsNotAvailable_WhenBandingIsNull()
    {
        ProgressBanding? banding = null;

        var result = banding.ToBandingContextStatement(CreateDescriptions());

        Assert.True(result.IsNotAvailable);
    }

    [Theory]
    [InlineData("Average", "This is average because Average description.")]
    [InlineData("above average", "This is above average because Above average description.")]
    [InlineData(" WELL BELOW AVERAGE ", "This is well below average because Well below average description.")]
    public void ToBandingContextStatement_String_ParsesAndReturnsStatement(string rawBanding, string expected)
    {
        var descriptions = CreateDescriptions();

        var result = rawBanding.ToBandingContextStatement(descriptions);

        Assert.True(result.IsAvailable);
        Assert.Equal(expected, result.DisplayText());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("SUPP")]
    [InlineData("not a banding value")]
    public void ToBandingContextStatement_String_ReturnsNotAvailable_ForUnparseableValues(string? rawBanding)
    {
        var result = rawBanding.ToBandingContextStatement(CreateDescriptions());

        Assert.True(result.IsNotAvailable);
    }

    [Fact]
    public void ToBandingString_CodedString_ReturnsNotAvailable_WhenValueIsNotNumeric()
    {
        var codedString = new CodedString("SUPP", "Suppressed", "SUPP");

        var result = codedString.ToBandingString(CreateDescriptions());

        Assert.True(result.IsNotAvailable);
    }

    [Fact]
    public void ToBandingString_CodedString_ReturnsNotAvailable_WhenValueIsBlank()
    {
        var codedString = CodedString.Empty;

        var result = codedString.ToBandingString(CreateDescriptions());

        Assert.True(result.IsNotAvailable);
    }

    [Fact]
    public void ToBandingString_CodedString_ReturnsStatement_WhenValueIsValidBandingNumber()
    {
        var codedString = new CodedString(((int)ProgressBanding.Average).ToString(), "", "3");

        var result = codedString.ToBandingString(CreateDescriptions());

        Assert.True(result.IsAvailable);
        Assert.Equal("This is average because Average description.", result.DisplayText());
    }

    [Fact]
    public void ToBandingString_CodedString_WithoutDescriptions_UsesEmptyDescriptions()
    {
        var codedString = new CodedString(((int)ProgressBanding.AboveAverage).ToString(), "", "2");

        var result = codedString.ToBandingString();

        Assert.True(result.IsAvailable);
        Assert.Equal("This is above average.", result.DisplayText());
    }
}
