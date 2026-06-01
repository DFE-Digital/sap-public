using Microsoft.AspNetCore.Http;
using Microsoft.FeatureManagement;
using SAPPub.Core.Interfaces.Services;

namespace SAPPub.Core.Services
{
    public class EstablishmentComparisonService(IHttpContextAccessor contextAccessor, IFeatureManager featureManager) : IEstablishmentComparisonService
    {
        private const string CookieName = "MySchoolsList";
        private const string FeatureName = "EstablishmentComparisonEnabled";
        private const int ComparisonLimit = 100;

        public string ComparisonPageUrl = "/compare-schools";           // TODO: Change this once the url is known.
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;
        private readonly IFeatureManager _featureManager = featureManager;

        public IReadOnlyCollection<string> GetSavedEstablishments()
        {
            var cookie = _contextAccessor.HttpContext?.Request.Cookies[CookieName];
            cookie ??= GetCookieValueFromHeader();
            return string.IsNullOrWhiteSpace(cookie) ? [] : cookie.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public bool IsSaved(string urn) => GetSavedEstablishments().Contains(urn);

        public void RemoveAll()
        {
            _contextAccessor.HttpContext.Response.Cookies.Delete(CookieName);
        }

        public void Toggle(string urn)
        {
            if (IsSaved(urn))
            {
                Remove(urn);
            }
            else
            {
                Save(urn);
            }
        }

        public void Remove(string urn)
        {
            var establishments = GetSavedEstablishments().ToList();
            if (establishments.Remove(urn))
            {
                WriteCookie(establishments);
            }
        }

        public void Save(string urn)
        {
            var establishments = GetSavedEstablishments().ToList();
            if (!establishments.Contains(urn) && !string.IsNullOrWhiteSpace(urn))
            {
                establishments.Add(urn);
                WriteCookie(establishments);
            }
        }

        public bool IsComparisonLimitReached() => GetSavedEstablishments().Count >= ComparisonLimit;

        public string GetComparisonPageUrl() => ComparisonPageUrl;

        public async Task<bool> IsFeatureEnabled() => await _featureManager.IsEnabledAsync(FeatureName);

        private void WriteCookie(List<string> establishments)
        {
            var context = _contextAccessor.HttpContext;
            var options = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                HttpOnly = false,
                SameSite = SameSiteMode.Lax,
                Secure = true,
                IsEssential = true
            };

            context.Response.Cookies.Append(CookieName, string.Join(",", establishments), options);
        }

        private string? GetCookieValueFromHeader()
        {
            var cookieHeader = _contextAccessor.HttpContext.Request.Headers["Cookie"];

            return cookieHeader
                .ToString()
                ?.Split(";", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                ?.Select(a => a.Split("=", 2))
                ?.Where(a => a.Length == 2)
                ?.FirstOrDefault(a => a[0] == CookieName)?[1];
        }
    }
}
