using SAPPub.Web.Helpers;


namespace SAPPub.Web.Tests.Unit.Helpers
{
    public class UrlHelperTests
    {

        [Theory]
        [InlineData("example.com", "https://example.com")]
        [InlineData("www.example.com", "https://www.example.com")]
        [InlineData("http://example.com", "http://example.com")]
        [InlineData("https://example.com", "https://example.com")]
        [InlineData("", "")]
        [InlineData(null, "")]
        public void EnsureHttpsUrl_ShouldFormatCorrectly(string? input, string expected)
        {
            var result = UrlHelper.EnsureHttpsUrl(input);
            Assert.Equal(expected, result);
        }
    }
}
