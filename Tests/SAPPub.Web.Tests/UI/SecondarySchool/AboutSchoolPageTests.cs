using Microsoft.Playwright;
using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.SecondarySchool;

[Collection("Playwright Tests")]
public class AboutSchoolPageTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private Dictionary<string, string> _schoolUrnToUrlMap = new Dictionary<string, string>
    {
        ["105574"] = "school/105574/loreto-high-school-chorlton/secondary/about",
        ["137552"] = "school/137552/stewards-academy-science-specialist-harlow/secondary/about",
        ["100273"] = "school/100273/saint-paul-roman-catholic-infant-school/secondary/about",
        ["107564"] = "school/107564/todmorden-high-school/secondary/about",
        ["145744"] = "school/145744/abbey-park-school/secondary/about",
        ["178965"] = "school/178965/predecessor-1-to-abbey-park-school/secondary/about",
        ["178966"] = "school/178966/predecessor-2-to-abbey-park-school/secondary/about"
    };

    [Fact]
    public async Task AboutSchoolPage_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task AboutSchoolPage_HasCorrectTitle()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var title = await Page.TitleAsync();

        // Assert
        Assert.Contains("About the school", title);
    }

    [Fact]
    public async Task AboutSchoolPage_DisplaysMainHeading()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var heading = await Page.Locator("h1").TextContentAsync();

        // Assert
        Assert.NotNull(heading);
        Assert.NotEmpty(heading!.Trim());
    }

    [Fact]
    public async Task AboutSchoolPage_Displays_SchoolName_Caption()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var schoolNameCaptionLocator = Page.Locator("#school-name-caption");
        var isVisible = await schoolNameCaptionLocator.IsVisibleAsync();
        var schoolNameCaption = await schoolNameCaptionLocator.TextContentAsync();

        // Assert
        Assert.True(isVisible);
        Assert.NotNull(schoolNameCaption);
        Assert.Equal("Loreto High School Chorlton", schoolNameCaption);
    }

    [Fact]
    public async Task AboutSchoolPage_Displays_VerticalNavigation()
    {
        var nav = new VerticalNavigationHelper(Page);
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveItemsCountAsync(6);
        await nav.ShouldHaveOneActiveItemAsync();
        await nav.ShouldHaveActiveHrefAsync(_schoolUrnToUrlMap["105574"]);
    }

    [Theory]
    [InlineData("178965")]
    [InlineData("178966")]
    public async Task AboutSchoolPage_SchoolClosedWithSuccessor_DisplaysExpectedSchoolClosedWithSuccessorInfo(string urn)
    {
        // Act
        await Page.GotoAsync(_schoolUrnToUrlMap[urn]);

        // Assert
        var schoolClosedCard = Page.GetByTestId("school-closed-custom-card");

        Assert.True(await schoolClosedCard.IsVisibleAsync());

        var links = await schoolClosedCard.Locator("a[href*='/secondary/about']").AllAsync();
        var linkText = await Task.WhenAll(links.Select(async link => await link.InnerTextAsync()));
        Assert.Collection(linkText,
            linkText => Assert.Contains("Abbey Park School", linkText));

        await Task.WhenAll(
            Page.WaitForURLAsync("**/secondary/about**"),
            links[0].ClickAsync()
        );

        var schoolNameCaptionLocator = Page.Locator("#school-name-caption");
        var isVisible = await schoolNameCaptionLocator.IsVisibleAsync();
        var schoolNameCaption = await schoolNameCaptionLocator.TextContentAsync();

        // Assert
        Assert.True(isVisible);
        Assert.NotNull(schoolNameCaption);
        Assert.Equal("Abbey Park School", schoolNameCaption);
    }

    [Theory]
    [InlineData("105574", false, false)]
    [InlineData("137552", true, false)]
    [InlineData("107564", true, true)]
    public async Task AboutSchoolPage_SchoolClosedWithNoSuccessor_DisplaysExpectedSchoolClosedInfo(string urn, bool isSchoolClosed, bool hasSchoolClosedDate)
    {
        // Act
        await Page.GotoAsync(_schoolUrnToUrlMap[urn]);

        // Assert
        var schoolClosedCard = Page.GetByTestId("school-closed-custom-card");

        Assert.Equal(isSchoolClosed, await schoolClosedCard.IsVisibleAsync());

        if (isSchoolClosed)
        {
            var value = await schoolClosedCard.Locator("p").TextContentAsync();

            var expectedText = hasSchoolClosedDate ? "This school closed on 23 March 2025" : "Closed";

            Assert.NotNull(value);
            Assert.Contains(expectedText, value.Trim());
            Assert.DoesNotContain("It is now ", value.Trim());
        }
    }

    [Theory]
    [InlineData("145744")]
    public async Task AboutSchoolPage_SchoolRecentlyOpenedAndHasPredecessors_DisplaysPredecessorSchoolLinks(string urn)
    {
        // Act
        await Page.GotoAsync(_schoolUrnToUrlMap[urn]);

        // Assert
        var schoolPredecessorsCard = Page.GetByTestId("school-predecessors-custom-card");

        Assert.True(await schoolPredecessorsCard.IsVisibleAsync());

        var links = await schoolPredecessorsCard.Locator("a[href*='/secondary/about']").AllAsync();
        var linkText = await Task.WhenAll(links.Select(async link => await link.InnerTextAsync()));
        Assert.Collection(linkText,
            linkText => Assert.Contains("Predecessor 1 to Abbey Park School", linkText),
            linkText => Assert.Contains("Predecessor 2 to Abbey Park School", linkText));

        await Task.WhenAll(
            Page.WaitForURLAsync("**/secondary/about**"),
            links[0].ClickAsync()
        );

        var schoolNameCaptionLocator = Page.Locator("#school-name-caption");
        var isVisible = await schoolNameCaptionLocator.IsVisibleAsync();
        var schoolNameCaption = await schoolNameCaptionLocator.TextContentAsync();

        // Assert
        Assert.True(isVisible);
        Assert.NotNull(schoolNameCaption);
        Assert.Equal("Predecessor 1 to Abbey Park School", schoolNameCaption);
    }

    [Theory]
    [InlineData("105574", false)]
    [InlineData("137552", true)]
    public async Task AboutSchoolPage_DisplaysSchoolDetails(string urn, bool trustNameIsExpected)
    {
        // Act
        await Page.GotoAsync(_schoolUrnToUrlMap[urn]);

        // Assert
        var detailsSummary = Page.Locator("#school-details-summary");

        Assert.True(await detailsSummary.IsVisibleAsync());
        var row = detailsSummary
            .Locator(".govuk-summary-list__row")
            .Filter(new() { Has = Page.Locator(".govuk-summary-list__key", new() { HasText = " Academy Trust " }) });

        if (trustNameIsExpected)
        {
            var value = await row.Locator(".govuk-summary-list__value").TextContentAsync();
            Assert.NotNull(value);
            Assert.Equal("THE PASSMORES CO-OPERATIVE LEARNING COMMUNITY", value.Trim());
        }
        else
        {
            Assert.False(await row.IsVisibleAsync());
        }
    }

    [Fact]
    public async Task AboutSchoolPage_DisplaysSchoolLocation()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var isVisible = await Page.Locator("#school-location-summary").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AboutSchoolPage_DisplaysSpecialistUnit()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var isVisible = await Page.Locator("#details-sen").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Theory]
    [InlineData("105574", "VI - Visual Impairment, HI - Hearing Impairment")]
    [InlineData("137552", "Not recorded")]
    public async Task AboutSchoolPage_Displays_TypeOfSenProvision(string urn, string senTypes)
    {
        // Act
        await Page.GotoAsync(_schoolUrnToUrlMap[urn]);

        // Assert
        var detailsSummary = Page.Locator("#school-features-summary");

        Assert.True(await detailsSummary.IsVisibleAsync());
        var row = detailsSummary
            .Locator(".govuk-summary-list__row")
            .Filter(new() { Has = Page.Locator(".govuk-summary-list__key", new() { HasText = " Type of SEN provision " }) });

        var value = await row.Locator(".govuk-summary-list__value").TextContentAsync();
        Assert.NotNull(value);
        Assert.Equal(senTypes, value.Trim());
    }

    [Fact]
    public async Task AboutSchoolPage_DisplaysSchoolFeatures()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var isVisible = await Page.Locator("#school-features-summary").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AboutSchoolPage_DisplaysSchoolPolicies()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var schoolPoliciesSummaryCard = Page.Locator("#school-policies-summary");
        var contactSchoolInfo = schoolPoliciesSummaryCard.GetByTestId("contact-school-info");

        // Assert
        Assert.True(await schoolPoliciesSummaryCard.IsVisibleAsync());
        Assert.False(await contactSchoolInfo.IsVisibleAsync());
    }

    [Fact]
    public async Task AboutSchoolPage_DisplaysSchoolPolicies_ContactSchoolText()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["100273"]);

        // Act
        var schoolPoliciesSummaryCard = Page.Locator("#school-policies-summary");
        var contactSchoolInfo = schoolPoliciesSummaryCard.GetByTestId("contact-school-info");
        var isVisible = await contactSchoolInfo.IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AboutSchoolPage_DisplaysPagination()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var isVisible = await Page.Locator("#about-the-school-pagination").IsVisibleAsync();
        var previousPaginationLink = Page.Locator("#about-the-school-pagination .govuk-pagination__previous a");
        var nextPaginationLink = Page.Locator("#about-the-school-pagination .govuk-pagination__next a");
        var previousPaginationIsVisible = await previousPaginationLink.IsVisibleAsync();
        var nextPaginationText = await nextPaginationLink.TextContentAsync();

        // Assert
        Assert.True(isVisible);
        Assert.False(previousPaginationIsVisible);
        Assert.Equal("Admissions", nextPaginationText?.Trim());
    }

    [Fact]
    public async Task AboutSchoolPage_ShowsSchoolComparisonLimit_WhenLimitReached()
    {
        // Arrange
        var cookieValue = string.Join(",", Enumerable.Range(1, 100).Select(a => a.ToString()).ToList());
        var schoolUrl = $"{fixture.BaseUrl.TrimEnd('/')}/{_schoolUrnToUrlMap["105574"]}";

        await Page.Context.ClearCookiesAsync();
        await Page.Context.AddCookiesAsync([new Cookie { Name = "MySchoolsList", Value = cookieValue, Domain = "127.0.0.1", Path = "/", SameSite = SameSiteAttribute.Lax, Secure = true }]);
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var limitNotificationBannerVisible = await Page.Locator("#establishment-comparison-105574-limit-notification").IsVisibleAsync();
        var compareButtonVisible = await Page.Locator(".compare-establishment-btn").IsVisibleAsync();

        // Assert
        Assert.True(limitNotificationBannerVisible);
        Assert.False(compareButtonVisible);
    }

    [Fact]
    public async Task AboutSchoolPage_DoesNotShowSchoolComparisonLimit_WhenLimitNotReached()
    {
        // Arrange
        var cookieValue = string.Join(",", Enumerable.Range(1, 99).Select(a => a.ToString()).ToList());
        var schoolUrl = $"{fixture.BaseUrl.TrimEnd('/')}/{_schoolUrnToUrlMap["105574"]}";

        await Page.Context.ClearCookiesAsync();
        await Page.Context.AddCookiesAsync([new Cookie { Name = "MySchoolsList", Value = cookieValue, Domain = "127.0.0.1", Path = "/", SameSite = SameSiteAttribute.Lax, Secure = true }]);
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var limitNotificationVisible = await Page.Locator("#school-compare-limit-notification").IsVisibleAsync();

        // Assert
        Assert.False(limitNotificationVisible);
    }

    [Fact]
    public async Task AboutSchoolPage_ShowsRemoveButton_WhenSchoolOnListAndLimitReached()
    {
        // Arrange
        var cookieValue = string.Join(",", Enumerable.Range(1, 99).Select(a => a.ToString()).ToList());
        cookieValue += ",105574";
        var schoolUrl = $"{fixture.BaseUrl.TrimEnd('/')}/{_schoolUrnToUrlMap["105574"]}";

        await Page.Context.ClearCookiesAsync();
        await Page.Context.AddCookiesAsync([new Cookie { Name = "MySchoolsList", Value = cookieValue, Domain = "127.0.0.1", Path = "/", SameSite = SameSiteAttribute.Lax, Secure = true }]);
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var compareButton = Page.Locator(".compare-establishment-btn");

        // Assert
        await Expect(compareButton).ToContainTextAsync("Saved to");
        await Expect(compareButton).ToContainClassAsync("saved");
    }

    [Fact]
    public async Task AboutSchoolPage_ShowsAddButton_WhenSchoolNotOnList_AndLimitNotReached()
    {
        // Arrange
        var cookieValue = string.Join(",", Enumerable.Range(1, 20).Select(a => a.ToString()).ToList());
        var schoolUrl = $"{fixture.BaseUrl.TrimEnd('/')}/{_schoolUrnToUrlMap["105574"]}";

        await Page.Context.ClearCookiesAsync();
        await Page.Context.AddCookiesAsync([new Cookie { Name = "MySchoolsList", Value = cookieValue, Domain = "127.0.0.1", Path = "/", SameSite = SameSiteAttribute.Lax, Secure = true }]);
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var compareButton = await Page.Locator(".compare-establishment-btn").InnerTextAsync();

        // Assert
        Assert.Contains("Save to", compareButton);
    }

    [Fact]
    public async Task AboutSchoolPage_ShowsRemoveButton_WhenSchoolOnList_AndLimitNotReached()
    {
        // Arrange
        var cookieValue = string.Join(",", Enumerable.Range(1, 20).Select(a => a.ToString()).ToList());
        cookieValue += ",105574";
        var schoolUrl = $"{fixture.BaseUrl.TrimEnd('/')}/{_schoolUrnToUrlMap["105574"]}";

        await Page.Context.ClearCookiesAsync();
        await Page.Context.AddCookiesAsync([new Cookie { Name = "MySchoolsList", Value = cookieValue, Domain = "127.0.0.1", Path = "/", SameSite = SameSiteAttribute.Lax, Secure = true }]);
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);
        var content = await Page.ContentAsync();

        // Act
        var compareButton = await Page.Locator(".compare-establishment-btn").InnerTextAsync();

        // Assert
        Assert.Contains("Saved to", compareButton);
    }


    [Fact]
    public async Task AboutSchoolPage_AddButtonClick_ShowsAddSuccessBanner()
    {
        // Arrange
        var urn = "105574";
        var cookieValue = string.Join(",", Enumerable.Range(1, 20).Select(a => a.ToString()).ToList());
        var schoolUrl = $"{fixture.BaseUrl.TrimEnd('/')}/{_schoolUrnToUrlMap["105574"]}";

        await Page.Context.ClearCookiesAsync();
        await Page.Context.AddCookiesAsync([new Cookie { Name = "MySchoolsList", Value = cookieValue, Domain = "127.0.0.1", Path = "/", SameSite = SameSiteAttribute.Lax, Secure = true }]);
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        await Page.ClickAsync(".compare-establishment-btn");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var content = await Page.ContentAsync();

        var compareButtonText = await Page.Locator(".compare-establishment-btn").InnerTextAsync();
        var comparisonAddSuccessBanner =  Page.Locator($"#establishment-comparison-{urn}-add-success");
        var comparisonRemoveSuccessBanner = Page.Locator($"#establishment-comparison-{urn}-remove-success");

        // Assert
        Assert.Contains("Saved to", compareButtonText);
        await Expect(comparisonAddSuccessBanner).Not.ToHaveAttributeAsync("hidden", "");
        await Expect(comparisonRemoveSuccessBanner).ToHaveAttributeAsync("hidden", "");
    }

    [Fact]
    public async Task AboutSchoolPage_RemoveButtonClick_ShowsRemoveSuccessBanner()
    {
        // Arrange
        var urn = "105574";
        var cookieValue = string.Join(",", Enumerable.Range(1, 20).Select(a => a.ToString()).ToList());
        cookieValue += ",105574";
        var schoolUrl = $"{fixture.BaseUrl.TrimEnd('/')}/{_schoolUrnToUrlMap["105574"]}";

        await Page.Context.ClearCookiesAsync();
        await Page.Context.AddCookiesAsync([new Cookie { Name = "MySchoolsList", Value = cookieValue, Domain = "127.0.0.1", Path = "/", SameSite = SameSiteAttribute.Lax, Secure = true }]);
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        await Page.ClickAsync(".compare-establishment-btn");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var compareButton = await Page.Locator(".compare-establishment-btn").InnerTextAsync();
        var comparisonAddSuccessBanner = Page.Locator($"#establishment-comparison-{urn}-add-success");
        var comparisonRemoveSuccessBanner = Page.Locator($"#establishment-comparison-{urn}-remove-success");

        // Assert
        Assert.Contains("Save to", compareButton);
        await Expect(comparisonAddSuccessBanner).ToHaveAttributeAsync("hidden", "");
        await Expect(comparisonRemoveSuccessBanner).Not.ToHaveAttributeAsync("hidden", "");
    }
}