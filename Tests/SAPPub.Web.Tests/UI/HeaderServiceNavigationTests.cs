using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI;

[Collection("Playwright Tests")]
public class HeaderServiceNavigationTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    [Fact]
    public async Task GovUkHeader_Displays()
    {
        // Arrange
        await Page.GotoAsync(string.Empty);

        // Act
        // Locate the GOV.UK header element
        var header = Page.Locator("header.govuk-header");
        var isVisible = await header.IsVisibleAsync();

        // Locate the GOV.UK logotype SVG 
        var govUkLogo = Page.Locator("header.govuk-header svg[aria-label='GOV.UK']");
        var logoVisible = await govUkLogo.IsVisibleAsync();

        // Assert
        Assert.True(isVisible, "GOV.UK header should be visible");
        Assert.True(logoVisible, "GOV.UK SVG logo should be visible in the header");
    }

    [Fact]
    public async Task GovUkLogo_LinksToGovUkHome()
    {
        // Arrange
        await Page.GotoAsync(string.Empty);

        // Act
        // Locate the GOV.UK homepage link in the header
        var govUkLink = Page.Locator("a.govuk-header__link--homepage");
        var href = await govUkLink.GetAttributeAsync("href");

        // Assert
        Assert.Equal("https://www.gov.uk/", href);
    }

    [Fact]
    public async Task ServiceTitle_IsCorrect_And_LinksToSearch()
    {
        await Page.GotoAsync(string.Empty);
        var serviceLink = Page.Locator("span.govuk-service-navigation__service-name a");
        var text = await serviceLink.InnerTextAsync();
        var href = await serviceLink.GetAttributeAsync("href");

        Assert.Equal("School Profiles", text.Trim());
        Assert.Equal("/search", href, ignoreCase: true);
    }

    [Fact]
    public async Task ServiceNavigation_IsCorrect_And_LinksToMySchoolsView()
    {
        await Page.GotoAsync(string.Empty);

        // Act
        var mySchoolsViewLink = Page.Locator("#my-schools-view-link");
        var isVisible = await mySchoolsViewLink.IsVisibleAsync();
        var text = await mySchoolsViewLink.InnerTextAsync();
        var href = await mySchoolsViewLink.GetAttributeAsync("href");

        // Assert
        Assert.True(isVisible, "My schools link should be visible");
        Assert.Equal("My schools list", text.Trim());
        Assert.Equal("/my-schools/view", href, ignoreCase: true);
    }

    [Fact]
    public async Task PhaseBanner_Feedback_Link_IsCorrect()
    {
        await Page.GotoAsync(string.Empty);
        var feedBackLink = Page.Locator("#feedback-link");
        var text = await feedBackLink.InnerTextAsync();
        var href = await feedBackLink.GetAttributeAsync("href");

        Assert.Equal("give your feedback (opens in new tab)", text.Trim());
        Assert.Equal("https://forms.cloud.microsoft/pages/responsepage.aspx?id=yXfS-grGoU2187O4s0qC-bvgNBlcYPxAqCMfEzVBipNUMjBVV05XS1hPNjhGWjZXVTlHV0ZXTjZVWS4u", href, ignoreCase: true);
    }
}