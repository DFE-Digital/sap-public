using SAPPub.Infrastructure.Mapping.ValueCodes;

namespace SAPPub.Infrastructure.Tests.Mapping;

public class ReasonCodeLookupTests
{
    [Theory]
    [InlineData("z", true, "Not applicable")]
    [InlineData("Z", false, null)]
    [InlineData("c", true, "Redacted for confidentiality")]
    [InlineData("C", false, null)]
    [InlineData("x", true, "Not available")]
    [InlineData("X", false, null)]
    [InlineData("low", true, "positive % less than 0.5")]
    [InlineData("LOW", false, null)]
    public void TryGet_ReturnsExpectedResult(
        string code,
        bool expectedResult,
        string? expectedReason)
    {
        // Arrange
        var map = new ReasonCodeLookup(new Dictionary<string, string>
        {
            ["z"] = "Not applicable",
            ["c"] = "Redacted for confidentiality",
            ["x"] = "Not available",
            ["low"] = "positive % less than 0.5"
        });

        // Act
        var result = map.TryGet(code, out var reason);

        // Assert
        Assert.Equal(expectedResult, result);
        Assert.Equal(expectedReason, reason);
    }

    [Fact]
    public void Constructor_CreatesDefensiveCopy()
    {
        // Arrange
        var map = new Dictionary<string, string>
        {
            ["c"] = "Redacted for confidentiality"
        };

        var sut = new ReasonCodeLookup(map);

        // Modify original collection after construction
        map["c"] = "Modified";

        // Act
        sut.TryGet("c", out var reason);

        // Assert
        Assert.Equal("Redacted for confidentiality", reason);
    }
}

