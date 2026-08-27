using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement.Mvc;
using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI
{

    [Collection("Playwright Tests")]
    public class VerticalNavigationTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
    {
        private bool EnabledKS5Flag = fixture.Configuration.GetValue<bool>("FeatureManagement:Enable16to19");

        private readonly Dictionary<string, string> _schoolUrnToUrlMap = new()
        {
            ["135600"] = "school/135600/ark-academy/about", //KS2 + KS4 + KS5
            //["149976"] = "school/149976/four-elms-primary-school/about", //KS2
            ["150009"] = "school/150009/abraham-moss-community-school/about", //KS2 + KS4
            ["137552"] = "school/137552/stewards-academy-science-specialist-harlow/about", //KS4
            ["149328"] = "school/149328/king-edward-vi-high-school/about", //KS4 + KS5
            ["130499"] = "school/130499/holy-cross-college/about", //KS5
        };


        [Theory]
        [InlineData("135600", 7, 9)] // KS2, KS4, KS5
        [InlineData("150009", 7, 8)] // KS2, KS4
        [InlineData("137552", 7, 7)] // KS4
        [InlineData("149328", 7, 8)] // KS4, KS5
        [InlineData("130499", 3, 4)] // KS5
        [FeatureGate("Enable16to19")]
        public async Task VerticalNav_AboutSchool_DisplayNumberExpectedPerPhase_NoKS5(string urn, int shownNav, int showNavWithKs5)
        {
            var isKS5 = EnabledKS5Flag;

            var nav = new VerticalNavigationHelper(Page);
            await Page.GotoAsync(_schoolUrnToUrlMap[urn]);

            await nav.ShouldBeVisibleAsync();
            //feature flag for ks5
            if (isKS5)
            {
                await nav.ShouldHaveItemsCountAsync(showNavWithKs5);
            }
            else
            {
                await nav.ShouldHaveItemsCountAsync(shownNav);
            }

        }
    }
}
