using Microsoft.Playwright;
using SAPPub.Integration.Tests;
using SAPPub.Playwright.Testing;

namespace SAPPub.IntegrationTests.SecondarySchoolTests;

[Collection("Integration Tests")]
public class AboutSchoolPageTests() : BasePageTest()
{
    private string PageUrl(string urn) => $"/school/{urn}";

    [Fact]
    public async Task AboutSchoolPage_LoadsSuccessfully()
    {
        // Act
        var response = await Page.GotoAsync("/school/105574");
        var nav = new VerticalNavigationHelper(Page);
        var navItem = nav.GetItem("About the school");
        await navItem.ClickAsync();

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Theory]
    [InlineData("114311", true, "31 December 2022")]
    [InlineData("149251", false, null)]
    public async Task AboutSchoolPage_DisplaysSchoolClosedInfo(string urn, bool isSchoolClosed, string? date)
    {
        // Act
        await Page.GotoAsync(PageUrl(urn));
        var nav = new VerticalNavigationHelper(Page);
        var navItem = nav.GetItem("About the school");
        await navItem.ClickAsync();

        // Assert
        var schoolClosedCard = Page.GetByTestId("school-closed-custom-card");

        Assert.Equal(isSchoolClosed, await schoolClosedCard.IsVisibleAsync());

        if (isSchoolClosed)
        {
            var schoolClosedPara = await schoolClosedCard.Locator("p").Filter(new() { HasText = "School closed" }).TextContentAsync();

            var expectedText = date != null ? $"This school closed on {date}" : "Closed";

            Assert.NotNull(schoolClosedPara);
            Assert.Contains(expectedText, schoolClosedPara.Trim());
        }
    }

    [Theory]
    [InlineData("105574", null)]
    [InlineData("137552", "THE PASSMORES CO-OPERATIVE LEARNING COMMUNITY")]
    public async Task AboutSchoolPage_DisplaysTrustNameRow_WhenTrustNameNotNull(string urn, string? trustName)
    {
        // Act
        await Page.GotoAsync(PageUrl(urn));
        var nav = new VerticalNavigationHelper(Page);
        var navItem = nav.GetItem("About the school");
        await navItem.ClickAsync();

        // Assert
        var detailsSummary = Page.Locator("#school-details-summary");

        Assert.True(await detailsSummary.IsVisibleAsync());
        var row = detailsSummary
            .Locator(".govuk-summary-list__row")
            .Filter(new() { Has = Page.Locator(".govuk-summary-list__key", new() { HasText = " Academy Trust " }) });

        if (trustName != null)
        {
            var value = await row.Locator(".govuk-summary-list__value").TextContentAsync();
            Assert.NotNull(value);
            Assert.Equal(trustName, value.Trim());
        }
        else
        {
            Assert.False(await row.IsVisibleAsync());
        }
    }
}