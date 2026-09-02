using Microsoft.Playwright;
using SAPPub.Web.Tests.UI.Infrastructure;
using System.Text.RegularExpressions;

namespace SAPPub.Web.Tests.UI.Areas.Profiles
{
    [Collection("Playwright Overview Disabled Tests")]
    public class OverviewFeatureFlagTests(
    OverviewDisabledWebApplicationSetupFixture fixture)
    : BasePageTest(fixture)
    {
        private const string Urn = "143034";
        private const string SchoolSlug =
            "st-pauls-church-of-england-academy";

        [Fact]
        public async Task SchoolEntryRoute_GoesToAbout_WhenOverviewDisabled()
        {
            await Page.GotoAsync(
                $"school/{Urn}/{SchoolSlug}");

            await Expect(Page)
                .ToHaveURLAsync(
                    new Regex(
                        $@"/school/{Urn}/{SchoolSlug}/about/?$"));
        }

        [Fact]
        public async Task AboutPage_DoesNotShowOverviewInVerticalNavigation()
        {
            await Page.GotoAsync(
                $"school/{Urn}/{SchoolSlug}/about");

            await Expect(
                Page
                    .Locator(".moj-side-navigation")
                    .GetByRole(
                        AriaRole.Link,
                        new()
                        {
                            Name = "Overview",
                            Exact = true
                        }))
                .ToHaveCountAsync(0);
        }

        [Fact]
        public async Task AboutPage_DoesNotShowOverviewInPagination()
        {
            await Page.GotoAsync(
                $"school/{Urn}/{SchoolSlug}/about");

            await Expect(
                Page
                    .Locator(".govuk-pagination")
                    .GetByText(
                        "Overview",
                        new() { Exact = true }))
                .ToHaveCountAsync(0);
        }

        [Fact]
        public async Task OverviewPage_IsNotAvailable_WhenFeatureDisabled()
        {
            var response =
                await Page.GotoAsync(
                    $"school/{Urn}/{SchoolSlug}/overview");

            Assert.NotNull(response);
            Assert.NotEqual(200, response.Status);
        }
    }
}
