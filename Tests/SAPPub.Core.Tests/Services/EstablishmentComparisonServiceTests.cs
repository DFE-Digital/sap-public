using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using SAPPub.Core.Services;
using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Tests.Services
{
    [ExcludeFromCodeCoverage]
    public class EstablishmentComparisonServiceTests
    {
        private const string CookieName = "MySchoolsList";
        private readonly EstablishmentComparisonService _service;
        private readonly HttpContextAccessor _contextAccessor;

        public EstablishmentComparisonServiceTests()
        {
            _contextAccessor = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext()
            };

            _service = new EstablishmentComparisonService(_contextAccessor);
        }

        [Fact]
        public void GetSavedEstablishments_ShouldReturnEmpty_WhenCookieIsMissing()
        {
            // Arrange
           
            // Act
            var result = _service.GetSavedEstablishments();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetSavedEstablishments_ShouldReturnValues_WhenCookiesExist()
        {
            // Arrange
            var urn1 = "123";
            var urn2 = "456";

            _contextAccessor.HttpContext!.Request.Headers.Cookie = $"{CookieName}={urn1},{urn2}";

            // Act
            var result = _service.GetSavedEstablishments();

            // Assert
            Assert.Equal([urn1, urn2], result);
        }

        [Fact]
        public void IsSaved_ShouldReturnTrue_WhenUrnExistsInCookie()
        {
            // Arrange
            var urn1 = "123";
            var urn2 = "456";

            _contextAccessor.HttpContext!.Request.Headers.Cookie = $"{CookieName}={urn1},{urn2}";

            // Act
            var result = _service.IsSaved(urn2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Toggle_ShouldRemoveUrn_WhenAlreadySaved()
        {
            // Arrange
            var urn1 = "123";
            var urn2 = "456";

            _contextAccessor.HttpContext!.Request.Headers.Cookie = $"{CookieName}={urn1},{urn2}";

            // Act
            _service.Toggle(urn1);

            // Assert
            var cookie = GetCookie();
            Assert.Equal(urn2, Uri.UnescapeDataString(cookie.Value.ToString()));
        }

        [Fact]
        public void Toggle_ShouldSaveUrn_WhenNotAlreadySaved()
        {
            // Arrange
            var urn1 = "123";
            var urn2 = "456";

            _contextAccessor.HttpContext!.Request.Headers.Cookie = $"{CookieName}={urn1}";

            // Act
            _service.Toggle(urn2);

            // Assert
            var cookie = GetCookie();
            Assert.Equal($"{urn1},{urn2}", Uri.UnescapeDataString(cookie.Value.ToString()));
        }

        [Fact]
        public void IsComparisonReached_ShouldReturnTrue_WhenComparisonLimitReached()
        {
            // Arrange
            _contextAccessor.HttpContext!.Request.Headers.Cookie = $"{CookieName}={string.Join(",", Enumerable.Range(1,100))}";

            // Act
            var result = _service.IsComparisonLimitReached();

            // Assert
            Assert.True(result);
        }

        private SetCookieHeaderValue GetCookie()
        {
            var header = _contextAccessor.HttpContext!.Response.Headers.SetCookie.ToArray();
            return SetCookieHeaderValue.Parse(header[0]);
        }
    }
}
