using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Compare.Secondary
{
    [Collection("WebAppCollection")]
    public class ProgressAndAttainmentPageTests(WebAppFixture fixture) : PageTestsBase(fixture)
    {
        private string _pageUrl = "compare/secondary/pupil-performance-attainment-and-progress?urns=119052&urns=124500";
        
        [Fact]
        public async Task AcademicPerformanceProgressAndAttainment_Displays_VerticalNavigation()
        {
            // Act
            var doc = await Fixture.BrowseToPage(_pageUrl);

            // Assert
            Assert.NotNull(doc.QuerySelector(".moj-side-navigation"));
            Assert.Equal(7, doc.QuerySelectorAll(".moj-side-navigation__item").Length);
            Assert.Single(doc.QuerySelectorAll(".moj-side-navigation__item--active"));
        }

        [Fact]
        public async Task AcademicPerformanceProgressAndAttainmentTests_Displays_Sub_Navigation()
        {
            // Act
            var doc = await Fixture.BrowseToPage(_pageUrl);

            // Assert
            Assert.NotNull(doc.QuerySelector("#sub-navigation-academic-performance"));
        }

        [Fact]
        public async Task AcademicPerformanceProgressAndAttainmentTests_Has_Correct_Sub_Navigation_Links()
        {
            // Act
            var doc = await Fixture.BrowseToPage(_pageUrl);
            var container = doc.QuerySelector("#sub-navigation-academic-performance");
            var links = container?.QuerySelectorAll(".moj-sub-navigation__link");

            Assert.NotNull(links);
            Assert.Equal(2, links.Length);
            Assert.Equal("/compare/secondary/pupil-performance-attainment-and-progress?urns=119052&urns=124500", links[0].GetAttribute("href"));
            Assert.Equal("/compare/secondary/english-and-maths-results?urns=119052&urns=124500", links[1].GetAttribute("href"));
        }
    }
}
