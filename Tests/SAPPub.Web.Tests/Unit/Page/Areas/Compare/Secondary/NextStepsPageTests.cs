using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Compare.Secondary
{
    [Collection("WebAppCollection")]
    public class NextStepsPageTests(WebAppFixture fixture) : PageTestsBase(fixture)
    {
        private string _pageUrl = "compare/secondary/next-steps?urns=119052&urns=124500";
        
        [Fact]
        public async Task NextSteps_Displays_VerticalNavigation()
        {
            // Act
            var doc = await Fixture.BrowseToPage(_pageUrl);

            // Assert
            Assert.NotNull(doc.QuerySelector(".moj-side-navigation"));
            Assert.Equal(7, doc.QuerySelectorAll(".moj-side-navigation__item").Length);
            Assert.Single(doc.QuerySelectorAll(".moj-side-navigation__item--active"));
        }

        [Fact]
        public async Task NextSteps_Show_ExploreMoreInformation_Header()
        {
            // Act
            var doc = await Fixture.BrowseToPage(_pageUrl);
            var h2Header = doc.QuerySelector("h2");

            Assert.NotNull(h2Header);
        }
    }
}
