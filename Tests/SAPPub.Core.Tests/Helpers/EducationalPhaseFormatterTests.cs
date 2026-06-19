using SAPPub.Core.Extensions;

namespace SAPPub.Core.Tests.Helpers
{
    public class EducationalPhaseFormatterTests
    {
        [Theory]
        [InlineData(false, false, false, null)]
        [InlineData(true, false, false, "Primary")]
        [InlineData(true, true, false, "Primary and Secondary")]
        [InlineData(true, true, true, "Primary, Secondary and 16 to 18")]
        [InlineData(false, true, true, "Secondary and 16 to 18")]
        [InlineData(false, false, true, "16 to 18")]
        [InlineData(false, true, false, "Secondary")]
        [InlineData(true, false, true, "Primary and 16 to 18")]
        public void CleanForUrl_RemovesNonAlphaChars(bool isKS2, bool isKS4, bool isKS5, string? output)
        {
            // Arrange
            // Act
            var result = EducationPhaseFormatter.Format(isKS2, isKS4, isKS5);

            // Assert
            Assert.Equal(output, result);
        }
    }
}
