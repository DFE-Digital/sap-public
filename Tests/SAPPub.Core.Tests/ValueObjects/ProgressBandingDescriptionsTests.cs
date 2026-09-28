using SAPPub.Core.Enums;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Tests.ValueObjectTests;

public class ProgressBandingDescriptionsTests
{
    private static readonly CodedString WellAboveAverageDescription = new("Well above average description", "", "");
    private static readonly CodedString AboveAverageDescription = new("Above average description", "", "");
    private static readonly CodedString AverageDescription = new("Average description", "", "");
    private static readonly CodedString BelowAverageDescription = new("Below average description", "", "");
    private static readonly CodedString WellBelowAverageDescription = new("Well below average description", "", "");

    private static ProgressBandingDescriptions CreateDescriptions() =>
        new(
            WellAboveAverageDescription,
            AboveAverageDescription,
            AverageDescription,
            BelowAverageDescription,
            WellBelowAverageDescription);

    [Fact]
    public void GetDescriptionFor_ReturnsWellAboveAverageDescription()
    {
        var descriptions = CreateDescriptions();

        var result = descriptions.GetDescriptionFor(ProgressBanding.WellAboveAverage);

        Assert.Equal(WellAboveAverageDescription, result);
    }

    [Fact]
    public void GetDescriptionFor_ReturnsAboveAverageDescription()
    {
        var descriptions = CreateDescriptions();

        var result = descriptions.GetDescriptionFor(ProgressBanding.AboveAverage);

        Assert.Equal(AboveAverageDescription, result);
    }

    [Fact]
    public void GetDescriptionFor_ReturnsAverageDescription()
    {
        var descriptions = CreateDescriptions();

        var result = descriptions.GetDescriptionFor(ProgressBanding.Average);

        Assert.Equal(AverageDescription, result);
    }

    [Fact]
    public void GetDescriptionFor_ReturnsBelowAverageDescription()
    {
        var descriptions = CreateDescriptions();

        var result = descriptions.GetDescriptionFor(ProgressBanding.BelowAverage);

        Assert.Equal(BelowAverageDescription, result);
    }

    [Fact]
    public void GetDescriptionFor_ReturnsWellBelowAverageDescription()
    {
        var descriptions = CreateDescriptions();

        var result = descriptions.GetDescriptionFor(ProgressBanding.WellBelowAverage);

        Assert.Equal(WellBelowAverageDescription, result);
    }

    [Fact]
    public void GetDescriptionFor_ReturnsEmpty_WhenBandingIsNull()
    {
        var descriptions = CreateDescriptions();

        var result = descriptions.GetDescriptionFor(null);

        Assert.Equal(CodedString.Empty, result);
    }

    [Fact]
    public void Empty_ReturnsAllCodedStringEmpty()
    {
        var descriptions = ProgressBandingDescriptions.Empty;

        Assert.Equal(CodedString.Empty, descriptions.WellAboveAverage);
        Assert.Equal(CodedString.Empty, descriptions.AboveAverage);
        Assert.Equal(CodedString.Empty, descriptions.Average);
        Assert.Equal(CodedString.Empty, descriptions.BelowAverage);
        Assert.Equal(CodedString.Empty, descriptions.WellBelowAverage);
    }
}
