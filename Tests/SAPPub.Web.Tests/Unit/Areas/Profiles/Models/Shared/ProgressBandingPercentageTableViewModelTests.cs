using SAPPub.Core.ValueObjects;
using SAPPub.Web.Areas.Profiles.ViewModels.Performance;

namespace SAPPub.Web.Tests.Unit.ViewModels.Shared;

public class ProgressBandingPercentageTableViewModelTests
{
    [Fact]
    public void Create_MapsBandingsAndPercentages()
    {
        // Arrange
        var percentages = new[]
        {
            new CodedDouble(10, "", "10"),
            new CodedDouble(20, "", "20"),
            new CodedDouble(30, "", "30"),
            new CodedDouble(25, "", "25"),
            new CodedDouble(15, "", "15")
        };

        // Act
        var result = ProgressBandingPercentageTableViewModel.Create(
            percentages,
            "Summary",
            "Description");

        // Assert
        Assert.Equal("Summary", result.Summary);
        Assert.Equal("Description", result.Description);

        Assert.Collection(
            result.Rows,
            row =>
            {
                Assert.Equal("Well above average", row.Banding);
                Assert.Equal("10%", row.Percentage);
            },
            row =>
            {
                Assert.Equal("Above average", row.Banding);
                Assert.Equal("20%", row.Percentage);
            },
            row =>
            {
                Assert.Equal("Average", row.Banding);
                Assert.Equal("30%", row.Percentage);
            },
            row =>
            {
                Assert.Equal("Below average", row.Banding);
                Assert.Equal("25%", row.Percentage);
            },
            row =>
            {
                Assert.Equal("Well below average", row.Banding);
                Assert.Equal("15%", row.Percentage);
            });
    }

    [Fact]
    public void Create_DisplaysNotAvailable_WhenPercentagesMissing()
    {
        // Arrange
        var percentages = new[]
        {
            new CodedDouble(10, "", "10"),
            new CodedDouble(20, "", "20")
        };

        // Act
        var result = ProgressBandingPercentageTableViewModel.Create(
            percentages,
            "Summary",
            "Description");

        // Assert
        Assert.Equal(5, result.Rows.Count);

        Assert.Equal("10%", result.Rows[0].Percentage);
        Assert.Equal("20%", result.Rows[1].Percentage);

        Assert.Equal("Not available", result.Rows[2].Percentage);
        Assert.Equal("Not available", result.Rows[3].Percentage);
        Assert.Equal("Not available", result.Rows[4].Percentage);
    }

    [Theory]
    [InlineData(
    "How many schools are in the Reading banding?",
    nameof(ProgressBandingPercentageTableViewModel.Ks2ProgressReading))]
    [InlineData(
    "How many schools are in the Writing banding?",
    nameof(ProgressBandingPercentageTableViewModel.Ks2ProgressWriting))]
    [InlineData(
    "How many schools are in the Maths banding?",
    nameof(ProgressBandingPercentageTableViewModel.Ks2ProgressMaths))]
    public void Ks2Factories_SetExpectedSummary(
    string expectedSummary,
    string factoryName)
    {
        // Arrange
        var percentages = new[]
        {
            new CodedDouble(1, "", "1"),
            new CodedDouble(1, "", "1"),
            new CodedDouble(1, "", "1"),
            new CodedDouble(1, "", "1"),
            new CodedDouble(1, "", "1")
        };

        // Act
        var result = factoryName switch
        {
            nameof(ProgressBandingPercentageTableViewModel.Ks2ProgressReading)
                => ProgressBandingPercentageTableViewModel.Ks2ProgressReading(percentages),

            nameof(ProgressBandingPercentageTableViewModel.Ks2ProgressWriting)
                => ProgressBandingPercentageTableViewModel.Ks2ProgressWriting(percentages),

            _ => ProgressBandingPercentageTableViewModel.Ks2ProgressMaths(percentages)
        };

        // Assert
        Assert.Equal(expectedSummary, result.Summary);
    }
}