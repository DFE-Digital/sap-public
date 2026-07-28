using Microsoft.FeatureManagement.Mvc;
using Moq;
using SAPPub.Core.Interfaces.Services.KS4.Attendance;
using SAPPub.Core.Interfaces.Services.Search;
using SAPPub.Core.ServiceModels.KS4.Attendance;
using SAPPub.Core.Services.Search;
using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

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
        [InlineData("135600", 6, 7)]
        [InlineData("150009", 6, 6)]
        [InlineData("137552", 6, 6)]
        [InlineData("149328", 6, 7)]
        [InlineData("130499", 2, 3)]
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
