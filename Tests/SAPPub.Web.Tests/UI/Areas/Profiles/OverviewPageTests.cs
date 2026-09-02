using System.Text.RegularExpressions;
using Microsoft.Playwright;
using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;
using SAPPub.Web.Tests;

namespace SAPPub.Web.Tests.UI.Areas.Profiles;

[Collection("Playwright Tests")]
public class OverviewPageTests(WebApplicationSetupFixture fixture)
    : BasePageTest(fixture)
{
    private const string Urn = "143034";
    private const string SchoolName = "St Paul's Church of England Academy";
    private const string SchoolSlug = "st-pauls-church-of-england-academy";
    private const string OverviewUrl = $"school/{Urn}/{SchoolSlug}/overview";
    private const string AboutUrl = $"school/{Urn}/{SchoolSlug}/about";
    private const string Address = "Grove Lane, Handsworth, Birmingham, B21 9ET";
    private const string MissingDataUrn = "137552";
    private const string MissingDataSchoolName = "Stewards Academy - Science Specialist, Harlow";
    private const string MissingDataSlug = "stewards-academy-science-specialist-harlow";
    private const string MissingDataOverviewUrl = $"school/{MissingDataUrn}/{MissingDataSlug}/overview";

    [Fact]
    public async Task OverviewPage_LoadsSuccessfully()
    {
        var response = await GoToOverviewAsync();

        Assert.True(response.Ok);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task OverviewPage_HasExpectedUrl()
    {
        await GoToOverviewAsync();

        await Expect(Page)
            .ToHaveURLAsync(
                new Regex(
                    $@"/school/{Urn}/{SchoolSlug}/overview/?$"));
    }

    [Fact]
    public async Task OverviewPage_HasCorrectBrowserTitle()
    {
        await GoToOverviewAsync();

        await Expect(Page)
            .ToHaveTitleAsync(
                $"{SchoolName} - Overview - School Profiles - GOV.UK");
    }

    [Fact]
    public async Task OverviewPage_DisplaysSchoolNameAsMainHeading()
    {
        await GoToOverviewAsync();

        var heading = Page.GetByRole(
            AriaRole.Heading,
            new()
            {
                Level = 1,
                Name = SchoolName,
                Exact = true
            });

        await Expect(heading).ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_DisplaysOverviewSectionHeading()
    {
        await GoToOverviewAsync();

        var heading = Page.GetByRole(
            AriaRole.Heading,
            new()
            {
                Level = 2,
                Name = "Overview",
                Exact = true
            });

        await Expect(heading).ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_HasSingleMainHeading()
    {
        await GoToOverviewAsync();

        var mainHeadings =
            Page.GetByRole(
                AriaRole.Heading,
                new() { Level = 1 });

        await Expect(mainHeadings)
            .ToHaveCountAsync(1);
    }

    [Fact]
    public async Task OverviewPage_DisplaysSchoolProfileBanner()
    {
        await GoToOverviewAsync();

        var banner = Page.Locator(".school-profile-banner");

        await Expect(banner).ToBeVisibleAsync();

        await Expect(
            banner.GetByRole(
                AriaRole.Heading,
                new()
                {
                    Level = 1,
                    Name = SchoolName,
                    Exact = true
                }))
            .ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_BannerSpansViewport()
    {
        await GoToOverviewAsync();

        var banner = Page.Locator(".school-profile-banner");

        await Expect(banner).ToBeVisibleAsync();

        var bannerBox = await banner.BoundingBoxAsync();

        Assert.NotNull(bannerBox);
        Assert.NotNull(Page.ViewportSize);

        var viewportWidth = Page.ViewportSize!.Width;

        // Allow a small tolerance for browser rendering/sub-pixel rounding.
        Assert.InRange(
            bannerBox!.Width,
            viewportWidth - 4,
            viewportWidth + 4);

        Assert.InRange(
            bannerBox.X,
            -2,
            2);
    }

    [Fact]
    public async Task OverviewPage_DisplaysTopServiceNavigation()
    {
        await GoToOverviewAsync();

        var navigation =
            Page.Locator(".govuk-service-navigation");

        await Expect(navigation).ToBeVisibleAsync();

        await Expect(
                navigation.GetByRole(
                    AriaRole.Link,
                    new() { Name = "School Profiles" }))
            .ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_ServiceNavigationUsesProfileBannerColour()
    {
        await GoToOverviewAsync();

        var serviceNavigation =
            Page.Locator(".govuk-service-navigation");

        var banner =
            Page.Locator(".school-profile-banner");

        await Expect(serviceNavigation).ToBeVisibleAsync();
        await Expect(banner).ToBeVisibleAsync();

        var serviceNavigationColour =
            await serviceNavigation.EvaluateAsync<string>(
                "el => getComputedStyle(el).backgroundColor");

        var bannerColour =
            await banner.EvaluateAsync<string>(
                "el => getComputedStyle(el).backgroundColor");

        Assert.Equal(
            bannerColour,
            serviceNavigationColour);
    }

    [Fact]
    public async Task OverviewPage_ServiceNavigationHasWhiteTopAndBottomBorders()
    {
        await GoToOverviewAsync();

        var navigation =
            Page.Locator(".govuk-service-navigation");

        await Expect(navigation).ToBeVisibleAsync();

        var topBorderStyle =
            await navigation.EvaluateAsync<string>(
                "el => getComputedStyle(el).borderTopStyle");

        var bottomBorderStyle =
            await navigation.EvaluateAsync<string>(
                "el => getComputedStyle(el).borderBottomStyle");

        var topBorderColour =
            await navigation.EvaluateAsync<string>(
                "el => getComputedStyle(el).borderTopColor");

        var bottomBorderColour =
            await navigation.EvaluateAsync<string>(
                "el => getComputedStyle(el).borderBottomColor");

        var topBorderWidth =
            await navigation.EvaluateAsync<string>(
                "el => getComputedStyle(el).borderTopWidth");

        var bottomBorderWidth =
            await navigation.EvaluateAsync<string>(
                "el => getComputedStyle(el).borderBottomWidth");

        Assert.Equal("solid", topBorderStyle);
        Assert.Equal("solid", bottomBorderStyle);

        Assert.Equal(
            "rgb(255, 255, 255)",
            topBorderColour);

        Assert.Equal(
            "rgb(255, 255, 255)",
            bottomBorderColour);

        Assert.NotEqual("0px", topBorderWidth);
        Assert.NotEqual("0px", bottomBorderWidth);
    }

    [Fact]
    public async Task OverviewPage_DisplaysVerticalNavigation()
    {
        await GoToOverviewAsync();

        var navigation =
            new VerticalNavigationHelper(Page);

        await navigation.ShouldBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_HighlightsOverviewNavigationItem()
    {
        await GoToOverviewAsync();

        var activeItem =
            Page.Locator(
                ".moj-side-navigation__item--active");

        await Expect(activeItem)
            .ToHaveCountAsync(1);

        var overviewLink =
            activeItem.GetByRole(
                AriaRole.Link,
                new()
                {
                    Name = "Overview",
                    Exact = true
                });

        await Expect(overviewLink)
            .ToBeVisibleAsync();

        await Expect(overviewLink)
            .ToHaveAttributeAsync(
                "href",
                $"/school/{Urn}/{SchoolSlug}/overview");
    }

    [Fact]
    public async Task OverviewPage_OtherProfileNavigationRetainsSameEstablishment()
    {
        await GoToOverviewAsync();

        var aboutLink =
            Page
                .Locator(".moj-side-navigation")
                .GetByRole(
                    AriaRole.Link,
                    new()
                    {
                        Name = "About the school",
                        Exact = true
                    });

        await Expect(aboutLink)
            .ToBeVisibleAsync();

        await Expect(aboutLink)
            .ToHaveAttributeAsync(
                "href",
                $"/school/{Urn}/{SchoolSlug}/about");
    }

    [Fact]
    public async Task OverviewPage_HasAboutAsNextPaginationDestination()
    {
        await GoToOverviewAsync();

        var pagination =
            Page.Locator(".govuk-pagination");

        await Expect(pagination)
            .ToBeVisibleAsync();

        // Overview is the first page, so it must not have a Previous link.
        await Expect(
                pagination.Locator(
                    ".govuk-pagination__prev"))
            .ToHaveCountAsync(0);

        var next =
            pagination.Locator(
                ".govuk-pagination__next");

        await Expect(next)
            .ToBeVisibleAsync();

        await Expect(
                next.Locator(
                    ".govuk-pagination__link-label"))
            .ToHaveTextAsync("About the school");

        await Expect(
                next.Locator("a"))
            .ToHaveAttributeAsync(
                "href",
                $"/school/{Urn}/{SchoolSlug}/about");
    }

    [Fact]
    public async Task OverviewPage_ClickingNextNavigatesToAbout()
    {
        await GoToOverviewAsync();

        var pagination =
            new PaginationNavigationHelper(Page);

        await pagination.ClickNextLinkAsync();

        await Expect(Page)
            .ToHaveURLAsync(
                new Regex(
                    $@"/school/{Urn}/{SchoolSlug}/about/?$"));
    }

    [Fact]
    public async Task AboutPage_HasOverviewAsPreviousPaginationDestination()
    {
        var response =
            await Page.GotoAsync(AboutUrl);

        Assert.NotNull(response);
        Assert.True(
            response.Ok,
            await GetFailureMessageAsync(
                "About page failed to load",
                response));

        var previous =
            Page.Locator(
                ".govuk-pagination__prev");

        await Expect(previous)
            .ToBeVisibleAsync();

        await Expect(
                previous.Locator(
                    ".govuk-pagination__link-label"))
            .ToHaveTextAsync("Overview");

        await Expect(
                previous.Locator("a"))
            .ToHaveAttributeAsync(
                "href",
                $"/school/{Urn}/{SchoolSlug}/overview");
    }

    [Fact]
    public async Task AboutPage_ClickingPreviousNavigatesToOverview()
    {
        var response =
            await Page.GotoAsync(AboutUrl);

        Assert.NotNull(response);
        Assert.True(
            response.Ok,
            await GetFailureMessageAsync(
                "About page failed to load",
                response));

        var pagination =
            new PaginationNavigationHelper(Page);

        await pagination.ClickPreviousLinkAsync();

        await Expect(Page)
            .ToHaveURLAsync(
                new Regex(
                    $@"/school/{Urn}/{SchoolSlug}/overview/?$"));
    }

    [Fact]
    public async Task OverviewPage_IsUsableAtMobileWidth()
    {
        await Page.SetViewportSizeAsync(
            375,
            667);

        await GoToOverviewAsync();

        await Expect(
                Page.GetByRole(
                    AriaRole.Heading,
                    new()
                    {
                        Level = 1,
                        Name = SchoolName,
                        Exact = true
                    }))
            .ToBeVisibleAsync();

        await Expect(
                Page.GetByRole(
                    AriaRole.Heading,
                    new()
                    {
                        Level = 2,
                        Name = "Overview",
                        Exact = true
                    }))
            .ToBeVisibleAsync();

        await Expect(
                Page.Locator(
                    ".school-profile-banner"))
            .ToBeVisibleAsync();

        var pageWidth =
            await Page.EvaluateAsync<int>(
                "() => document.documentElement.scrollWidth");

        var viewportWidth =
            await Page.EvaluateAsync<int>(
                "() => document.documentElement.clientWidth");

        Assert.True(
            pageWidth <= viewportWidth,
            $"Page has horizontal overflow. " +
            $"scrollWidth={pageWidth}, " +
            $"clientWidth={viewportWidth}");
    }

    [Fact]
    public async Task OverviewPage_IsUsableAtDesktopWidth()
    {
        await Page.SetViewportSizeAsync(
            1280,
            720);

        await GoToOverviewAsync();

        await Expect(
                Page.Locator(
                    ".school-profile-banner"))
            .ToBeVisibleAsync();

        await Expect(
                Page.Locator(
                    ".moj-side-navigation"))
            .ToBeVisibleAsync();

        await Expect(
                Page.Locator(
                    ".map-address"))
                .ToBeVisibleAsync();

        await Expect(
                Page.Locator(
                    ".ataglance-address-row"))
                .ToContainTextAsync(Address);

        await Expect(
                Page.Locator(
                    "#overview-school-information"))
                .ToBeVisibleAsync();

        await Expect(
                Page.GetByRole(
                    AriaRole.Link,
                    new()
                    {
                        NameRegex =
                            new Regex(
                                "Find out more about the school",
                                RegexOptions.IgnoreCase)
                    }))
                .ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_DisplaysAvailableSchoolInformation()
    {
        await GoToOverviewAsync();

        var information = Page.Locator("#overview-school-information");

        await Expect(information).ToBeVisibleAsync();
        await Expect(information).ToContainTextAsync("Phase of education");
        await Expect(information).ToContainTextAsync("Primary");
        await Expect(information).ToContainTextAsync("Age range");
        await Expect(information).ToContainTextAsync("2 to 11");
        await Expect(information).ToContainTextAsync("Number of pupils");
        await Expect(information).ToContainTextAsync("661");
        await Expect(information).ToContainTextAsync("Type of SEN provision");
        await Expect(information).ToContainTextAsync("ASD - Autistic Spectrum Disorder");
        await Expect(information).ToContainTextAsync("Phone");
        await Expect(information).ToContainTextAsync("01424 424530");
        await Expect(information).ToContainTextAsync("Website");
        await Expect(information).ToContainTextAsync("Ofsted report");
    }

    [Fact]
    public async Task OverviewPage_DisplaysSchoolAddressBelowMap()
    {
        await GoToOverviewAsync();

        var address = Page.Locator(".ataglance-address-row");

        await Expect(address).ToBeVisibleAsync();
        await Expect(address).ToContainTextAsync("Address");
        await Expect(address).ToContainTextAsync(Address);
    }

    [Fact]
    public async Task OverviewPage_MapInitialises()
    {
        await GoToOverviewAsync();

        var map = Page.Locator("#map");

        await Expect(map).ToBeVisibleAsync();

        // Loading placeholder should have gone.
        await Expect(
            map.Locator(".map-loading"))
            .ToHaveCountAsync(0);

        // Leaflet adds this class to the map host when initialised.
        await Expect(map)
            .ToHaveClassAsync(
                new Regex(@"\bleaflet-container\b"));

        // Interactive Leaflet controls prove the map is active.
        await Expect(
            map.GetByRole(
                AriaRole.Button,
                new()
                {
                    Name = "Zoom in",
                    Exact = true
                }))
            .ToBeVisibleAsync();

        await Expect(
            map.GetByRole(
                AriaRole.Button,
                new()
                {
                    Name = "Zoom out",
                    Exact = true
                }))
            .ToBeVisibleAsync();

        // School marker should also have been created.
        await Expect(
            map.Locator(".leaflet-marker-icon"))
            .ToHaveCountAsync(1);
    }

    [Fact]
    public async Task OverviewPage_MapDisplaysSchoolMarker()
    {
        await GoToOverviewAsync();

        var marker = Page.Locator("#map .leaflet-marker-icon");

        await Expect(marker).ToHaveCountAsync(1);
        await Expect(Page.GetByRole(AriaRole.Button, new() { Name = SchoolName, Exact = true })).ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_MapProvidesZoomControls()
    {
        await GoToOverviewAsync();

        var map = Page.Locator("#map");

        await Expect(map.GetByRole(AriaRole.Button, new() { Name = "Zoom in", Exact = true })).ToBeVisibleAsync();
        await Expect(map.GetByRole(AriaRole.Button, new() { Name = "Zoom out", Exact = true })).ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_WebsiteLink_OpensInNewTab()
    {
        await GoToOverviewAsync();

        var websiteLink =
            Page.GetByRole(
                AriaRole.Link,
                new()
                {
                    NameRegex =
                        new Regex(
                            @"View .*website",
                            RegexOptions.IgnoreCase)
                });

        await Expect(websiteLink)
            .ToBeVisibleAsync();

        await Expect(websiteLink)
            .ToHaveAttributeAsync(
                "target",
                "_blank");

        await Expect(websiteLink)
            .ToHaveAttributeAsync(
                "rel",
                new Regex(@"noopener"));

        await Expect(websiteLink)
            .ToHaveAttributeAsync(
                "href",
                new Regex(@"^https://"));
    }

    [Fact]
    public async Task OverviewPage_DisplaysOfstedReportLink()
    {
        await GoToOverviewAsync();

        var ofstedLink =
            Page.GetByRole(
                AriaRole.Link,
                new()
                {
                    NameRegex =
                        new Regex(
                            "Ofsted report",
                            RegexOptions.IgnoreCase)
                });

        await Expect(ofstedLink)
            .ToBeVisibleAsync();

        await Expect(ofstedLink)
            .ToHaveAttributeAsync(
                "href",
                new Regex(Urn));

        await Expect(ofstedLink)
            .ToHaveAttributeAsync(
                "target",
                "_blank");
    }

    [Fact]
    public async Task OverviewPage_AboutLink_NavigatesToSameSchoolInSameTab()
    {
        await GoToOverviewAsync();

        var aboutLink =
            Page.GetByRole(
                AriaRole.Link,
                new()
                {
                    NameRegex =
                        new Regex(
                            "Find out more about the school",
                            RegexOptions.IgnoreCase)
                });

        await Expect(aboutLink)
            .ToBeVisibleAsync();

        await Expect(aboutLink)
            .ToHaveAttributeAsync(
                "href",
                $"/school/{Urn}/{SchoolSlug}/about");

        Assert.Null(
            await aboutLink.GetAttributeAsync(
                "target"));
    }

    [Fact]
    public async Task OverviewPage_WhenSchoolInformationIsMissing_DisplaysNotAvailable()
    {
        var response =
            await Page.GotoAsync(
                MissingDataOverviewUrl);

        Assert.NotNull(response);

        Assert.True(
            response.Ok,
            await GetFailureMessageAsync(
                "Missing-data overview page failed to load",
                response));

        var information =
            Page.Locator("#overview-school-information");

        await Expect(information)
            .ToBeVisibleAsync();

        await AssertRowDisplaysNotAvailableAsync(
            "#overview-age-range",
            "Age range");

        await AssertRowDisplaysNotAvailableAsync(
            "#overview-pupil-count",
            "Number of pupils");

        await AssertRowDisplaysNotAvailableAsync(
            "#overview-sen",
            "Type of SEN provision");

        await AssertRowDisplaysNotAvailableAsync(
            "#overview-phone",
            "Phone");

        await AssertRowDisplaysNotAvailableAsync(
            "#overview-website",
            "Website");
    }

    [Fact]
    public async Task OverviewPage_WhenAddressIsMissing_DisplaysNotAvailable()
    {
        var response =
            await Page.GotoAsync(
                MissingDataOverviewUrl);

        Assert.NotNull(response);

        Assert.True(
            response.Ok,
            await GetFailureMessageAsync(
                "Missing-data overview page failed to load",
                response));

        var address =
            Page.Locator(".ataglance-address-row");

        await Expect(address)
            .ToBeVisibleAsync();

        await Expect(address)
            .ToContainTextAsync("Address");

        await Expect(address)
            .ToContainTextAsync("Not available");
    }

    [Fact]
    public async Task OverviewPage_WhenJavaScriptDisabled_HidesMapButKeepsContentAvailable()
    {
        var context =
            await Browser.NewContextAsync(
                new BrowserNewContextOptions
                {
                    BaseURL = BaseUrl.TrimEnd('/'),
                    IgnoreHTTPSErrors = true,
                    ViewportSize =
                        new ViewportSize
                        {
                            Width = 1280,
                            Height = 720
                        },
                    Locale = "en-GB",
                    TimezoneId = "Europe/London",
                    JavaScriptEnabled = false
                });

        try
        {
            var page =
                await context.NewPageAsync();

            var response =
                await page.GotoAsync(
                    OverviewUrl);

            Assert.NotNull(response);
            Assert.True(response.Ok);

            await Expect(
                page.Locator(
                    ".map-container"))
                .ToHaveClassAsync(
                    new Regex(
                        @"\bgovuk-visually-hidden\b"));

            await Expect(
                page.Locator(
                    ".ataglance-address-row"))
                .ToContainTextAsync(Address);

            await Expect(
                page.Locator(
                    "#overview-school-information"))
                .ToBeVisibleAsync();

            await Expect(
                page.GetByText(
                    "Primary",
                    new() { Exact = true }))
                .ToBeVisibleAsync();

            await Expect(
                page.GetByText(
                    "2 to 11",
                    new() { Exact = true }))
                .ToBeVisibleAsync();
        }
        finally
        {
            await context.CloseAsync();
        }
    }

    [Fact]
    public async Task OverviewPage_MapHasAccessibleName()
    {
        await GoToOverviewAsync();

        await Expect(
            Page.GetByRole(
                AriaRole.Region,
                new()
                {
                    Name = $"Map showing the location of {SchoolName}",
                    Exact = true
                }))
            .ToBeVisibleAsync();
    }

    private async Task AssertRowDisplaysNotAvailableAsync(
    string selector,
    string label)
    {
        var row = Page.Locator(selector);

        await Expect(row)
            .ToBeVisibleAsync();

        await Expect(row)
            .ToContainTextAsync(label);

        await Expect(row)
            .ToContainTextAsync("Not available");
    }

    private async Task<IResponse> GoToOverviewAsync()
    {
        var response =
            await Page.GotoAsync(OverviewUrl);

        Assert.NotNull(response);

        Assert.True(
            response.Ok,
            await GetFailureMessageAsync(
                "Overview page failed to load",
                response));

        await Expect(
            Page.GetByRole(
                AriaRole.Heading,
                new()
                {
                    Level = 1,
                    Name = SchoolName,
                    Exact = true
                }))
            .ToBeVisibleAsync();

        return response;
    }

    private async Task<string> GetFailureMessageAsync(
        string message,
        IResponse response)
    {
        var body =
            await Page.Locator("body")
                .InnerTextAsync();

        return
            $"{message}.{Environment.NewLine}" +
            $"Status: {response.Status}{Environment.NewLine}" +
            $"URL: {Page.Url}{Environment.NewLine}" +
            $"Body:{Environment.NewLine}{body}";
    }
}