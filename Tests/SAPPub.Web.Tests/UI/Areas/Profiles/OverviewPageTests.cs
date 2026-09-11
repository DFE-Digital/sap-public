using System.Text.RegularExpressions;
using Microsoft.Playwright;
using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;
using SAPPub.Web.Tests;
using SAPPub.Playwright.Testing;

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
    private const string MissingDataSchoolName =
        "Stewards Academy - Science Specialist, Harlow";
    private const string MissingDataSlug =
        "stewards-academy-science-specialist-harlow";
    private const string MissingDataOverviewUrl =
        $"school/{MissingDataUrn}/{MissingDataSlug}/overview";

    private const string AchievementUrn = "149328";
    private const string AchievementSchoolName =
        "King Edward VI High School";
    private const string AchievementSlug =
        "king-edward-vi-high-school";
    private const string AchievementOverviewUrl =
        $"school/{AchievementUrn}/{AchievementSlug}/overview";

    private const string PrimaryOnlyOverviewUrl =
    "school/143034/st-pauls-church-of-england-academy/overview";

    private const string SecondaryOnlyOverviewUrl =
        "school/137552/stewards-academy-science-specialist-harlow/overview";

    private const string PrimarySecondaryOverviewUrl =
        "school/150009/abraham-moss-community-school/overview";

    private const string AllThroughOverviewUrl =
        "school/135600/ark-academy/overview";

    private const string Ks5OnlyOverviewUrl =
        "school/130499/holy-cross-college/overview";

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

        var information =
            Page.Locator("#overview-school-information");

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

        var address =
            Page.Locator(".ataglance-address-row");

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

        await Expect(
            map.Locator(".map-loading"))
            .ToHaveCountAsync(0);

        await Expect(map)
            .ToHaveClassAsync(
                new Regex(@"\bleaflet-container\b"));

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

        await Expect(
            map.Locator(".leaflet-marker-icon"))
            .ToHaveCountAsync(1);
    }

    [Fact]
    public async Task OverviewPage_MapDisplaysSchoolMarker()
    {
        await GoToOverviewAsync();

        var marker =
            Page.Locator("#map .leaflet-marker-icon");

        await Expect(marker).ToHaveCountAsync(1);

        await Expect(
            Page.GetByRole(
                AriaRole.Button,
                new()
                {
                    Name = SchoolName,
                    Exact = true
                }))
            .ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_MapProvidesZoomControls()
    {
        await GoToOverviewAsync();

        var map = Page.Locator("#map");

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
                    Name =
                        $"Map showing the location of {SchoolName}",
                    Exact = true
                }))
            .ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_SecondarySchool_DisplaysSecondaryAtAGlanceSection()
    {
        const string url =
            "school/137552/stewards-academy-science-specialist-harlow/overview";

        var response =
            await Page.GotoAsync(url);

        Assert.NotNull(response);
        Assert.True(response.Ok);

        await Expect(
            Page.GetByRole(
                AriaRole.Heading,
                new()
                {
                    Level = 2,
                    Name =
                        "Secondary school profile at a glance",
                    Exact = true
                }))
            .ToBeVisibleAsync();

        await Expect(
            Page.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex =
                        new Regex("Pupil progress")
                }))
            .ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_PupilProgressAccordion_IsClosedByDefault()
    {
        await Page.GotoAsync(SecondaryOnlyOverviewUrl);

        var section =
            Page.Locator(
                "#secondary-at-a-glance-accordion .govuk-accordion__section")
            .Nth(0);

        var accordionButton =
            section.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex = new Regex(
                        "Pupil progress",
                        RegexOptions.IgnoreCase)
                });

        var content =
            Page.Locator(
                "#secondary-at-a-glance-accordion-content-1");

        await Expect(accordionButton)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "false");

        await Expect(content)
            .Not.ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_PupilProgressAccordion_CanBeExpandedAndClosed()
    {
        await Page.GotoAsync(SecondaryOnlyOverviewUrl);

        var section =
            Page.Locator(
                "#secondary-at-a-glance-accordion .govuk-accordion__section")
            .Nth(0);

        var accordionButton =
            section.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex = new Regex(
                        "Pupil progress",
                        RegexOptions.IgnoreCase)
                });

        var content =
            Page.Locator(
                "#secondary-at-a-glance-accordion-content-1");

        await Expect(accordionButton)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "false");

        await Expect(content)
            .Not.ToBeVisibleAsync();

        await accordionButton.ClickAsync();

        await Expect(accordionButton)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "true");

        await Expect(content)
            .ToBeVisibleAsync();

        await Expect(
            content.GetByText(
                "Progress scores are not available for the academic years 2024 to 2025 and 2025 to 2026 due to COVID-19 disruption.",
                new() { Exact = true }))
            .ToBeVisibleAsync();

        await accordionButton.ClickAsync();

        await Expect(accordionButton)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "false");

        await Expect(content)
            .Not.ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_AccordionToggleIds_AreUniqueAndDescriptive()
    {
        await Page.GotoAsync(AllThroughOverviewUrl);

        var expectedIds = new[]
        {
        "primary-progress-toggle",
        "primary-expected-standard-toggle",
        "secondary-progress-toggle",
        "secondary-attainment-toggle",
        "secondary-english-maths-toggle",
        "secondary-destinations-toggle",
        "secondary-extra-curricular-toggle"
    };

        foreach (var id in expectedIds)
        {
            await Expect(
                Page.Locator($"#{id}"))
                .ToHaveCountAsync(1);
        }
    }

    [Fact]
    public async Task OverviewPage_PrimaryPupilProgress_CanBeExpandedAndClosed()
    {
        await Page.GotoAsync(PrimaryOnlyOverviewUrl);

        var section =
            Page.Locator(
                "#primary-at-a-glance-accordion .govuk-accordion__section")
            .Nth(0);

        var button =
            section.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex = new Regex(
                        "Pupil progress",
                        RegexOptions.IgnoreCase)
                });

        var content =
            Page.Locator(
                "#primary-at-a-glance-accordion-content-1");

        await Expect(button)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "false");

        await Expect(content)
            .Not.ToBeVisibleAsync();

        await button.ClickAsync();

        await Expect(button)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "true");

        await Expect(content)
            .ToBeVisibleAsync();

        await button.ClickAsync();

        await Expect(button)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "false");

        await Expect(content)
            .Not.ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_PrimaryExpectedStandardAccordion_CanBeClosedAndExpanded()
    {
        await Page.GotoAsync(PrimaryOnlyOverviewUrl);

        var section =
            Page.Locator(
                "#primary-at-a-glance-accordion .govuk-accordion__section")
            .Nth(1);

        var button =
            section.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex = new Regex(
                        "Meeting expected standard in reading, writing and maths",
                        RegexOptions.IgnoreCase)
                });

        var content =
            Page.Locator(
                "#primary-at-a-glance-accordion-content-2");

        await Expect(button)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "true");

        await Expect(content)
            .ToBeVisibleAsync();

        await button.ClickAsync();

        await Expect(button)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "false");

        await Expect(content)
            .Not.ToBeVisibleAsync();

        await button.ClickAsync();

        await Expect(button)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "true");

        await Expect(content)
            .ToBeVisibleAsync();
    }
    [Fact]
    public async Task OverviewPage_SecondarySchool_DisplaysAtAGlanceSectionsInExpectedOrder()
    {
        await Page.GotoAsync(AchievementOverviewUrl);

        var sections =
            Page.Locator(
                "#secondary-at-a-glance-accordion .govuk-accordion__section");

        await Expect(sections)
            .ToHaveCountAsync(5);

        await Expect(
            sections.Nth(0).GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex = new Regex(
                        "Pupil progress",
                        RegexOptions.IgnoreCase)
                }))
            .ToBeVisibleAsync();

        await Expect(
            sections.Nth(1).GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex = new Regex(
                        "Average pupil achievement",
                        RegexOptions.IgnoreCase)
                }))
            .ToBeVisibleAsync();

        await Expect(
            sections.Nth(2).GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex = new Regex(
                        "English and maths GCSE results",
                        RegexOptions.IgnoreCase)
                }))
            .ToBeVisibleAsync();

        await Expect(
            sections.Nth(3).GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex = new Regex(
                        "What pupils did after year 11",
                        RegexOptions.IgnoreCase)
                }))
            .ToBeVisibleAsync();

        await Expect(
            sections.Nth(4).GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex = new Regex(
                        "Extra-curricular activities",
                        RegexOptions.IgnoreCase)
                }))
            .ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_AveragePupilAchievementAccordion_IsOpenByDefault()
    {
        await Page.GotoAsync(
            AchievementOverviewUrl);

        var button =
            Page.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex =
                        new Regex(
                            "Average pupil achievement")
                });

        await Expect(button)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "true");

        await Expect(
            Page.GetByText(
                "Average results across 8 GCSE-level qualifications",
                new() { Exact = true }))
            .ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_AveragePupilAchievementAccordion_CanBeClosedAndExpanded()
    {
        await Page.GotoAsync(
            AchievementOverviewUrl);

        var button =
            Page.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex =
                        new Regex(
                            "Average pupil achievement")
                });

        await Expect(button)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "true");

        await button.ClickAsync();

        await Expect(button)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "false");

        await Expect(
            Page.GetByText(
                "Average results across 8 GCSE-level qualifications",
                new() { Exact = true }))
            .Not.ToBeVisibleAsync();

        await button.ClickAsync();

        await Expect(button)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "true");
    }

    [Fact]
    public async Task OverviewPage_AveragePupilAchievement_DisplaysResultAndExplanation()
    {
        await Page.GotoAsync(AchievementOverviewUrl);

        var attainmentSection =
            Page.Locator(
                "#secondary-at-a-glance-accordion-content-2");

        await Expect(attainmentSection)
            .ToBeVisibleAsync();

        await Expect(
            attainmentSection.GetByText(
                "Attainment 8 is used to assess a pupil's average achievement across 8 GCSEs and equivalent subjects.",
                new() { Exact = true }))
            .ToBeVisibleAsync();

        await Expect(
            attainmentSection.GetByText(
                "The qualifications included in this average result may include GCSEs and approved technical and vocational qualifications.",
                new() { Exact = true }))
            .ToBeVisibleAsync();

        await Expect(
            attainmentSection.GetByText(
                "Average result",
                new() { Exact = true }))
            .ToBeVisibleAsync();

        await Expect(
            attainmentSection.GetByText(
                "49.9",
                new() { Exact = true }))
            .ToBeVisibleAsync();

        await Expect(
            attainmentSection.GetByText(
                "This means that pupils generally scored the equivalent of just below grade 5 in their 8 subjects.",
                new() { Exact = true }))
            .ToBeVisibleAsync();

        await Expect(
            attainmentSection.GetByText(
                "Sheffield average",
                new() { Exact = true }))
            .ToBeVisibleAsync();

        await Expect(
            attainmentSection.GetByText(
                "43.4",
                new() { Exact = true }))
            .ToBeVisibleAsync();

        await Expect(
            attainmentSection.GetByText(
                "England average",
                new() { Exact = true }))
            .ToBeVisibleAsync();

        await Expect(
            attainmentSection.GetByText(
                "45.2",
                new() { Exact = true }))
            .ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_AveragePupilAchievement_WhenResultUnavailable_DisplaysNotAvailable()
    {
        const string url =
            "school/137552/stewards-academy-science-specialist-harlow/overview";

        await Page.GotoAsync(url);

        var attainmentSection =
            Page.Locator(
                "#secondary-at-a-glance-accordion-content-2");

        await Expect(attainmentSection)
            .ToBeVisibleAsync();

        await Expect(
            attainmentSection.GetByText(
                "Average result",
                new() { Exact = true }))
            .ToBeVisibleAsync();

        var notAvailableValues =
            attainmentSection.GetByText(
                "Not available",
                new() { Exact = true });

        await Expect(notAvailableValues)
            .ToHaveCountAsync(3);
    }

    [Fact]
    public async Task OverviewPage_AveragePupilAchievementLink_NavigatesToSameSchoolProgressAndAttainmentPage()
    {
        await Page.GotoAsync(
            AchievementOverviewUrl);

        var link =
            Page.GetByRole(
                AriaRole.Link,
                new()
                {
                    Name =
                        "Find out more about pupil achievement at this school",
                    Exact = true
                });

        await Expect(link)
            .ToHaveAttributeAsync(
                "href",
                $"/school/{AchievementUrn}/{AchievementSlug}/secondary-performance/progress-attainment");

        Assert.Null(
            await link.GetAttributeAsync(
                "target"));

        await link.ClickAsync();

        await Expect(Page)
            .ToHaveURLAsync(
                new Regex(
                    $@"/school/{AchievementUrn}/{AchievementSlug}/secondary-performance/progress-attainment(?:/current)?/?$"));
    }

    [Fact]
    public async Task OverviewPage_PupilProgressLink_NavigatesToSameSchoolProgressAndAttainmentPage()
    {
        const string urn = "137552";
        const string slug =
            "stewards-academy-science-specialist-harlow";

        await Page.GotoAsync(
            $"school/{urn}/{slug}/overview");

        var accordionButton =
            Page.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex =
                        new Regex("Pupil progress")
                });

        await accordionButton.ClickAsync();

        var link =
            Page.GetByRole(
                AriaRole.Link,
                new()
                {
                    NameRegex =
                        new Regex(
                            "Find out more about the progress pupils make",
                            RegexOptions.IgnoreCase)
                });

        await Expect(link)
            .ToHaveAttributeAsync(
                "href",
                $"/school/{urn}/{slug}/secondary-performance/progress-attainment");

        Assert.Null(
            await link.GetAttributeAsync(
                "target"));

        await link.ClickAsync();

        await Expect(Page)
            .ToHaveURLAsync(
                new Regex(
                    $@"/school/{urn}/{slug}/secondary-performance/progress-attainment(?:/current)?/?$"));
    }

    [Fact]
    public async Task OverviewPage_PrimaryOnlySchool_DoesNotDisplaySecondaryAtAGlanceSection()
    {
        await Page.GotoAsync(PrimaryOnlyOverviewUrl);

        await Expect(
            Page.Locator("#primary-at-a-glance-accordion"))
            .ToBeVisibleAsync();

        await Expect(
            Page.Locator("#secondary-at-a-glance-accordion"))
            .ToHaveCountAsync(0);

        await Expect(
            Page.GetByRole(
                AriaRole.Heading,
                new()
                {
                    Name = "Secondary school profile at a glance",
                    Exact = true
                }))
            .ToHaveCountAsync(0);
    }

    [Fact]
    public async Task OverviewPage_SecondarySchool_DisplaysEnglishAndMathsGcseResultsAfterAveragePupilAchievement()
    {
        await Page.GotoAsync(
            AchievementOverviewUrl);

        var sections =
            Page.Locator(
                "#secondary-at-a-glance-accordion .govuk-accordion__section");

        await Expect(sections)
            .ToHaveCountAsync(5);

        await Expect(
            sections.Nth(2)
                .GetByRole(
                    AriaRole.Button,
                    new()
                    {
                        NameRegex =
                            new Regex(
                                "English and maths GCSE results",
                                RegexOptions.IgnoreCase)
                    }))
            .ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_EnglishAndMathsGcseResultsAccordion_IsClosedByDefault()
    {
        await Page.GotoAsync(
            AchievementOverviewUrl);

        var button =
            Page.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex =
                        new Regex(
                            "English and maths GCSE results",
                            RegexOptions.IgnoreCase)
                });

        var content =
            Page.Locator(
                "#secondary-at-a-glance-accordion-content-3");

        await Expect(button)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "false");

        await Expect(content)
            .Not.ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_EnglishAndMathsGcseResultsAccordion_CanBeExpandedAndClosed()
    {
        await Page.GotoAsync(
            AchievementOverviewUrl);

        var button =
            Page.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex =
                        new Regex(
                            "English and maths GCSE results",
                            RegexOptions.IgnoreCase)
                });

        var content =
            Page.Locator(
                "#secondary-at-a-glance-accordion-content-3");

        await Expect(button)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "false");

        await Expect(content)
            .Not.ToBeVisibleAsync();

        await button.ClickAsync();

        await Expect(button)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "true");

        await Expect(content)
            .ToBeVisibleAsync();

        await button.ClickAsync();

        await Expect(button)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "false");

        await Expect(content)
            .Not.ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_EnglishAndMathsGcseResults_DisplaysExpectedContent()
    {
        await Page.GotoAsync(
            AchievementOverviewUrl);

        var button =
            Page.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex =
                        new Regex(
                            "English and maths GCSE results",
                            RegexOptions.IgnoreCase)
                });

        await button.ClickAsync();

        var content =
            Page.Locator(
                "#secondary-at-a-glance-accordion-content-3");

        await Expect(content)
            .ToBeVisibleAsync();

        await Expect(
            content.GetByRole(
                AriaRole.Heading,
                new()
                {
                    Name =
                        "Percentage of pupils achieving grade 5 and above in English and maths GCSEs",
                    Exact = true
                }))
            .ToBeVisibleAsync();

        await Expect(
            content.GetByText(
                "Grade 5 is comparable to the top of the old grade C.",
                new() { Exact = true }))
            .ToBeVisibleAsync();

        await Expect(
            content.GetByText(
                "This data is for the 2024 to 2025 academic year.",
                new() { Exact = true }))
            .ToBeVisibleAsync();

        await Expect(
            content.Locator(
                "#overview-english-maths-chart"))
            .ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_EnglishAndMathsGcseResultsChart_ContainsExpectedComparisonData()
    {
        await Page.GotoAsync(
            AchievementOverviewUrl);

        var button =
            Page.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex =
                        new Regex(
                            "English and maths GCSE results",
                            RegexOptions.IgnoreCase)
                });

        await button.ClickAsync();

        var chart =
            Page.Locator(
                "#overview-english-maths-chart");

        await Expect(chart)
            .ToBeVisibleAsync();

        var chartData =
            await chart.GetAttributeAsync(
                "data-chart");

        Assert.NotNull(chartData);

        Assert.Contains(
            "School",
            chartData,
            StringComparison.Ordinal);

        Assert.Contains(
            "Sheffield average",
            chartData,
            StringComparison.Ordinal);

        Assert.Contains(
            "England average",
            chartData,
            StringComparison.Ordinal);

        Assert.Contains(
            "63",
            chartData,
            StringComparison.Ordinal);

        Assert.Contains(
            "50",
            chartData,
            StringComparison.Ordinal);

        Assert.Contains(
            "54",
            chartData,
            StringComparison.Ordinal);
    }

    [Fact]
    public async Task OverviewPage_EnglishAndMathsGcseResults_WhenComparisonResultUnavailable_DoesNotSuppressAvailableResults()
    {
        await Page.GotoAsync(
            MissingDataOverviewUrl);

        var button =
            Page.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex =
                        new Regex(
                            "English and maths GCSE results",
                            RegexOptions.IgnoreCase)
                });

        await button.ClickAsync();

        var chart =
            Page.Locator(
                "#overview-english-maths-chart");

        await Expect(chart)
            .ToBeVisibleAsync();

        var chartData =
            await chart.GetAttributeAsync(
                "data-chart");

        Assert.NotNull(chartData);

        Assert.Contains(
            "63",
            chartData,
            StringComparison.Ordinal);

        Assert.Contains(
            "54",
            chartData,
            StringComparison.Ordinal);

        Assert.Contains(
            "null",
            chartData,
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task OverviewPage_EnglishAndMathsGcseResultsLink_NavigatesToSameSchoolResultsPage()
    {
        await Page.GotoAsync(
            AchievementOverviewUrl);

        var button =
            Page.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex =
                        new Regex(
                            "English and maths GCSE results",
                            RegexOptions.IgnoreCase)
                });

        await button.ClickAsync();

        var link =
            Page.GetByRole(
                AriaRole.Link,
                new()
                {
                    Name =
                        "Find out more about English and maths results at this school",
                    Exact = true
                });

        await Expect(link)
            .ToBeVisibleAsync();

        Assert.Null(
            await link.GetAttributeAsync(
                "target"));

        await Expect(link)
            .ToHaveAttributeAsync(
                "href",
                new Regex(
                    $@"/school/{AchievementUrn}/{AchievementSlug}/secondary-performance/.*english.*maths",
                    RegexOptions.IgnoreCase));

        await link.ClickAsync();

        await Expect(Page)
            .ToHaveURLAsync(
                new Regex(
                    $@"/school/{AchievementUrn}/{AchievementSlug}/secondary-performance/.*english.*maths.*",
                    RegexOptions.IgnoreCase));
    }

    [Fact]
    public async Task OverviewPage_EnglishAndMathsAccordion_HasValidAriaControlsReference()
    {
        await Page.GotoAsync(
            AchievementOverviewUrl);

        var button =
            Page.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex =
                        new Regex(
                            "English and maths GCSE results",
                            RegexOptions.IgnoreCase)
                });

        var ariaControls =
            await button.GetAttributeAsync(
                "aria-controls");

        Assert.Equal(
            "secondary-at-a-glance-accordion-content-3",
            ariaControls);

        await Expect(
            Page.Locator(
                $"#{ariaControls}"))
            .ToHaveCountAsync(1);
    }

    [Fact]
    public async Task OverviewPage_DestinationsAccordion_IsClosedByDefault()
    {
        await Page.GotoAsync(
            AchievementOverviewUrl);

        var button =
            Page.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex =
                        new Regex(
                            "What pupils did after year 11",
                            RegexOptions.IgnoreCase)
                });

        var content =
            Page.Locator(
                "#secondary-at-a-glance-accordion-content-4");

        await Expect(button)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "false");

        await Expect(content)
            .Not.ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_DestinationsAccordion_CanBeExpandedAndClosed()
    {
        await Page.GotoAsync(
            AchievementOverviewUrl);

        var button =
            Page.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex =
                        new Regex(
                            "What pupils did after year 11",
                            RegexOptions.IgnoreCase)
                });

        var content =
            Page.Locator(
                "#secondary-at-a-glance-accordion-content-4");

        await Expect(button)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "false");

        await Expect(content)
            .Not.ToBeVisibleAsync();

        await button.ClickAsync();

        await Expect(button)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "true");

        await Expect(content)
            .ToBeVisibleAsync();

        await button.ClickAsync();

        await Expect(button)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "false");

        await Expect(content)
            .Not.ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_Destinations_DisplaysExpectedContent()
    {
        await Page.GotoAsync(
            AchievementOverviewUrl);

        var button =
            Page.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex =
                        new Regex(
                            "What pupils did after year 11",
                            RegexOptions.IgnoreCase)
                });

        await button.ClickAsync();

        var content =
            Page.Locator(
                "#secondary-at-a-glance-accordion-content-4");

        await Expect(content)
            .ToBeVisibleAsync();

        await Expect(
            content.GetByRole(
                AriaRole.Heading,
                new()
                {
                    Name =
                        "Percentage of pupils who either stayed in education, went into an apprenticeship or entered employment for at least 6 months after finishing year 11 (key stage 4)",
                    Exact = true
                }))
            .ToBeVisibleAsync();

        await Expect(
            content.GetByText(
                "This data is for pupils who finished year 11 in the 2022 to 2023 academic year.",
                new() { Exact = true }))
            .ToBeVisibleAsync();

        await Expect(
            content.Locator(
                "#overview-destinations-chart"))
            .ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_DestinationsChart_ContainsExpectedComparisonData()
    {
        await Page.GotoAsync(
            AchievementOverviewUrl);

        var button =
            Page.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex =
                        new Regex(
                            "What pupils did after year 11",
                            RegexOptions.IgnoreCase)
                });

        await button.ClickAsync();

        var chart =
            Page.Locator(
                "#overview-destinations-chart");

        await Expect(chart)
            .ToBeVisibleAsync();

        var chartData =
            await chart.GetAttributeAsync(
                "data-chart");

        Assert.NotNull(chartData);

        Assert.Contains(
            "School",
            chartData,
            StringComparison.Ordinal);

        Assert.Contains(
            "Sheffield average",
            chartData,
            StringComparison.Ordinal);

        Assert.Contains(
            "England average",
            chartData,
            StringComparison.Ordinal);

        Assert.Contains(
            "95",
            chartData,
            StringComparison.Ordinal);

        Assert.Contains(
            "92",
            chartData,
            StringComparison.Ordinal);

        Assert.Contains(
            "87",
            chartData,
            StringComparison.Ordinal);
    }

    [Fact]
    public async Task OverviewPage_Destinations_WhenComparisonResultUnavailable_DoesNotSuppressAvailableResults()
    {
        await Page.GotoAsync(
            MissingDataOverviewUrl);

        var button =
            Page.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex =
                        new Regex(
                            "What pupils did after year 11",
                            RegexOptions.IgnoreCase)
                });

        await button.ClickAsync();

        var chart =
            Page.Locator(
                "#overview-destinations-chart");

        await Expect(chart)
            .ToBeVisibleAsync();

        var chartData =
            await chart.GetAttributeAsync(
                "data-chart");

        Assert.NotNull(chartData);

        Assert.Contains(
            "95",
            chartData,
            StringComparison.Ordinal);

        Assert.Contains(
            "87",
            chartData,
            StringComparison.Ordinal);

        Assert.Contains(
            "null",
            chartData,
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task OverviewPage_DestinationsLink_NavigatesToSameSchoolDestinationsPage()
    {
        await Page.GotoAsync(
            AchievementOverviewUrl);

        var button =
            Page.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex =
                        new Regex(
                            "What pupils did after year 11",
                            RegexOptions.IgnoreCase)
                });

        await button.ClickAsync();

        var link =
            Page.GetByRole(
                AriaRole.Link,
                new()
                {
                    Name =
                        "View breakdowns of pupils in education, apprenticeships or employment",
                    Exact = true
                });

        await Expect(link)
            .ToBeVisibleAsync();

        await Expect(link)
            .ToHaveAttributeAsync(
                "href",
                $"/school/{AchievementUrn}/{AchievementSlug}/destinations/secondary");

        Assert.Null(
            await link.GetAttributeAsync(
                "target"));

        await link.ClickAsync();

        await Expect(Page)
            .ToHaveURLAsync(
                new Regex(
                    $@"/school/{AchievementUrn}/{AchievementSlug}/destinations/secondary/?$"));
    }

    [Fact]
    public async Task OverviewPage_DestinationsAccordion_HasValidAriaControlsReference()
    {
        await Page.GotoAsync(
            AchievementOverviewUrl);

        var button =
            Page.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex =
                        new Regex(
                            "What pupils did after year 11",
                            RegexOptions.IgnoreCase)
                });

        var ariaControls =
            await button.GetAttributeAsync(
                "aria-controls");

        Assert.Equal(
            "secondary-at-a-glance-accordion-content-4",
            ariaControls);

        await Expect(
            Page.Locator(
                $"#{ariaControls}"))
            .ToHaveCountAsync(1);
    }


    [Fact]
    public async Task OverviewPage_EnglishAndMathsChart_CanBeShownAsTableAndChart()
    {
        await Page.GotoAsync(AchievementOverviewUrl);

        var accordionButton = Page.GetByRole(
            AriaRole.Button,
            new()
            {
                NameRegex = new Regex(
                    "English and maths GCSE results",
                    RegexOptions.IgnoreCase)
            });

        await accordionButton.ClickAsync();

        var button = Page.Locator(
            "#overview-english-maths-current-year-show-btn");

        var chart = Page.Locator(
            "#overview-english-maths-current-year-chart-container");

        var table = Page.Locator(
            "#overview-english-maths-current-year-table-container");

        await Expect(button).ToHaveTextAsync("Show as a table");
        await Expect(chart).ToBeVisibleAsync();
        await Expect(table).Not.ToBeVisibleAsync();

        // Show table
        await button.ClickAsync();

        await Expect(button).ToHaveTextAsync("Show as a chart");
        await Expect(chart).Not.ToBeVisibleAsync();
        await Expect(table).ToBeVisibleAsync();

        await Expect(
            table.GetByText("School", new() { Exact = true }))
            .ToBeVisibleAsync();

        await Expect(
            table.GetByText("Sheffield average", new() { Exact = true }))
            .ToBeVisibleAsync();

        await Expect(
            table.GetByText("England average", new() { Exact = true }))
            .ToBeVisibleAsync();

        // Return to chart
        await button.ClickAsync();

        await Expect(button).ToHaveTextAsync("Show as a table");
        await Expect(chart).ToBeVisibleAsync();
        await Expect(table).Not.ToBeVisibleAsync();
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
            await Page.GotoAsync(
                OverviewUrl);

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

    [Fact]
    public async Task OverviewPage_WithoutJavaScript_DisplaysTablesInsteadOfChartControls()
    {
        await Page.GotoAsync(AchievementOverviewUrl);

        var baseUri = new Uri(Page.Url);
        var baseUrl = $"{baseUri.Scheme}://{baseUri.Authority}";

        await using var context = await Browser.NewContextAsync(
            new BrowserNewContextOptions
            {
                JavaScriptEnabled = false,
                IgnoreHTTPSErrors = true,
                BaseURL = baseUrl
            });

        var page = await context.NewPageAsync();

        await page.GotoAsync(AchievementOverviewUrl);

        await Expect(
            page.Locator("#overview-english-maths-current-year-table"))
            .ToBeVisibleAsync();

        await Expect(
            page.Locator("#overview-english-maths-current-year-show-btn"))
            .Not.ToBeVisibleAsync();

        await Expect(
            page.Locator("#overview-english-maths-current-year-chart-container"))
            .Not.ToBeVisibleAsync();

        await Expect(
            page.Locator("#overview-destinations-current-year-table"))
            .ToBeVisibleAsync();

        await Expect(
            page.Locator("#overview-destinations-current-year-show-btn"))
            .Not.ToBeVisibleAsync();

        await Expect(
            page.Locator("#overview-destinations-current-year-chart-container"))
            .Not.ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_PrimaryExpectedStandard_WithoutJavaScript_DisplaysTableOnly()
    {
        await Page.GotoAsync(PrimaryOnlyOverviewUrl);

        var currentUri = new Uri(Page.Url);
        var baseUrl = $"{currentUri.Scheme}://{currentUri.Authority}";

        await using var context =
            await Browser.NewContextAsync(
                new BrowserNewContextOptions
                {
                    JavaScriptEnabled = false,
                    IgnoreHTTPSErrors = true,
                    BaseURL = baseUrl
                });

        var page = await context.NewPageAsync();

        await page.GotoAsync(PrimaryOnlyOverviewUrl);

        var table =
            page.Locator(
                "#overview-primary-expected-standard-current-year-table");

        var button =
            page.Locator(
                "#overview-primary-expected-standard-current-year-show-btn");

        var chart =
            page.Locator(
                "#overview-primary-expected-standard-current-year-chart-container");

        await Expect(table)
            .ToBeVisibleAsync();

        await Expect(button)
            .Not.ToBeVisibleAsync();

        await Expect(chart)
            .Not.ToBeVisibleAsync();

        await Expect(table)
            .ToContainTextAsync("School");

        await Expect(table)
            .ToContainTextAsync("70%");

        await Expect(table)
            .ToContainTextAsync("Birmingham average");

        await Expect(table)
            .ToContainTextAsync("73%");

        await Expect(table)
            .ToContainTextAsync("England average");

        await Expect(table)
            .ToContainTextAsync("77%");
    }

    [Fact]
    public async Task OverviewPage_ExtraCurricularAccordion_IsClosedByDefault()
    {
        await Page.GotoAsync(AchievementOverviewUrl);

        var button =
            Page.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex = new Regex(
                        "Extra-curricular activities",
                        RegexOptions.IgnoreCase)
                });

        var content =
            Page.Locator(
                "#secondary-at-a-glance-accordion-content-5");

        await Expect(button)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "false");

        await Expect(content)
            .Not.ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_ExtraCurricularAccordion_CanBeExpandedAndClosed()
    {
        await Page.GotoAsync(AchievementOverviewUrl);

        var button =
            Page.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex = new Regex(
                        "Extra-curricular activities",
                        RegexOptions.IgnoreCase)
                });

        var content =
            Page.Locator(
                "#secondary-at-a-glance-accordion-content-5");

        await Expect(button)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "false");

        await button.ClickAsync();

        await Expect(button)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "true");

        await Expect(content)
            .ToBeVisibleAsync();

        await button.ClickAsync();

        await Expect(button)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "false");

        await Expect(content)
            .Not.ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_ExtraCurricular_DisplaysExpectedContent()
    {
        await Page.GotoAsync(AchievementOverviewUrl);

        var button =
            Page.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex = new Regex(
                        "Extra-curricular activities",
                        RegexOptions.IgnoreCase)
                });

        await button.ClickAsync();

        var content =
            Page.Locator(
                "#secondary-at-a-glance-accordion-content-5");

        await Expect(
            content.GetByRole(
                AriaRole.Heading,
                new()
                {
                    Name = "Find out more about upcoming information on extra-curricular activities",
                    Exact = true
                }))
            .ToBeVisibleAsync();

        await Expect(
            content.GetByText(
                "Information on extra-curricular opportunities will be available in the future.",
                new() { Exact = true }))
            .ToBeVisibleAsync();

        await Expect(
            content.GetByRole(
                AriaRole.Link,
                new()
                {
                    Name = "Find out more about what types of activities pupils can take part in and where to find this information",
                    Exact = true
                }))
            .ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_ExtraCurricularAccordion_HasValidAriaControlsReference()
    {
        await Page.GotoAsync(AchievementOverviewUrl);

        var button =
            Page.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex = new Regex(
                        "Extra-curricular activities",
                        RegexOptions.IgnoreCase)
                });

        var ariaControls =
            await button.GetAttributeAsync("aria-controls");

        Assert.Equal(
            "secondary-at-a-glance-accordion-content-5",
            ariaControls);

        await Expect(
            Page.Locator($"#{ariaControls}"))
            .ToHaveCountAsync(1);
    }

    [Fact]
    public async Task OverviewPage_NextSteps_PrimarySchool_DisplaysExpectedLinks()
    {
        await Page.GotoAsync(PrimaryOnlyOverviewUrl);

        var nextSteps = Page.Locator("#overview-next-steps");

        await Expect(
            nextSteps.GetByRole(
                AriaRole.Heading,
                new()
                {
                    Name = "Next steps",
                    Exact = true
                }))
            .ToBeVisibleAsync();

        await Expect(
            nextSteps.GetByRole(
                AriaRole.Link,
                new()
                {
                    Name = "About the school",
                    Exact = true
                }))
            .ToBeVisibleAsync();

        await Expect(
            nextSteps.GetByRole(
                AriaRole.Link,
                new()
                {
                    Name = "Admissions",
                    Exact = true
                }))
            .ToBeVisibleAsync();

        await Expect(
            nextSteps.GetByRole(
                AriaRole.Link,
                new()
                {
                    Name = "Curriculum and extra-curricular activities",
                    Exact = true
                }))
            .ToBeVisibleAsync();

        await Expect(
            nextSteps.GetByRole(
                AriaRole.Link,
                new()
                {
                    Name = "Attendance",
                    Exact = true
                }))
            .ToBeVisibleAsync();

        await Expect(
            nextSteps.GetByRole(
                AriaRole.Link,
                new()
                {
                    Name = "Primary academic performance",
                    Exact = true
                }))
            .ToBeVisibleAsync();

        await Expect(
            nextSteps.GetByRole(
                AriaRole.Link,
                new()
                {
                    Name = "Secondary academic performance",
                    Exact = true
                }))
            .ToHaveCountAsync(0);

        await Expect(
            nextSteps.GetByRole(
                AriaRole.Link,
                new()
                {
                    Name = "Destinations",
                    Exact = true
                }))
            .ToHaveCountAsync(0);
    }

    [Fact]
    public async Task OverviewPage_PrimaryAtAGlance_DisplaysForPrimarySchool()
    {
        await Page.GotoAsync(PrimaryOnlyOverviewUrl);

        await Expect(
            Page.GetByRole(
                AriaRole.Heading,
                new()
                {
                    Name = "Primary school profile at a glance",
                    Exact = true
                }))
            .ToBeVisibleAsync();

        var accordion =
            Page.Locator("#primary-at-a-glance-accordion");

        await Expect(accordion)
            .ToBeVisibleAsync();

        await Expect(
            accordion.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex = new Regex(
                        "Pupil progress",
                        RegexOptions.IgnoreCase)
                }))
            .ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_PrimaryPupilProgress_IsFirstAccordion()
    {
        await Page.GotoAsync(PrimaryOnlyOverviewUrl);

        var firstSection =
            Page.Locator(
                    "#primary-at-a-glance-accordion .govuk-accordion__section")
                .First;

        await Expect(
            firstSection.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex = new Regex(
                        "Pupil progress",
                        RegexOptions.IgnoreCase)
                }))
            .ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_PrimaryPupilProgress_IsClosedByDefault()
    {
        await Page.GotoAsync(PrimaryOnlyOverviewUrl);

        var accordion =
            Page.Locator("#primary-at-a-glance-accordion");

        var button =
            accordion.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex = new Regex(
                        "Pupil progress",
                        RegexOptions.IgnoreCase)
                });

        await Expect(button)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "false");

        await Expect(
            Page.Locator("#primary-at-a-glance-accordion-content-1"))
            .Not.ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_PrimaryPupilProgress_DisplaysExpectedContent()
    {
        await Page.GotoAsync(PrimaryOnlyOverviewUrl);

        var accordion =
            Page.Locator("#primary-at-a-glance-accordion");

        await accordion
            .GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex = new Regex(
                        "Pupil progress",
                        RegexOptions.IgnoreCase)
                })
            .ClickAsync();

        var content =
            Page.Locator("#primary-at-a-glance-accordion-content-1");


        await Expect(content)
            .ToContainTextAsync(
                "Progress scores are not available for the academic years 2024 to 2025 and 2025 to 2026 due to COVID-19 disruption.");

        await Expect(content)
            .ToContainTextAsync(
                "You can view progress results from previous years on the school's full profile.");
    }

    [Fact]
    public async Task OverviewPage_PrimaryPupilProgress_LinkNavigatesToPupilProgressPage()
    {
        await Page.GotoAsync(PrimaryOnlyOverviewUrl);

        var accordion =
            Page.Locator("#primary-at-a-glance-accordion");

        await accordion
            .GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex = new Regex(
                        "Pupil progress",
                        RegexOptions.IgnoreCase)
                })
            .ClickAsync();

        var link =
            Page.Locator("#primary-at-a-glance-accordion-content-1")
                .GetByRole(
                    AriaRole.Link,
                    new()
                    {
                        Name =
                            "Find out more about the progress pupils make at this school for previous years",
                        Exact = true
                    });

        await Expect(link)
            .ToHaveAttributeAsync(
                "href",
                "/school/143034/st-pauls-church-of-england-academy/primary-performance/pupil-progress");

        Assert.Null(
            await link.GetAttributeAsync("target"));

        await link.ClickAsync();

        await Expect(Page)
            .ToHaveURLAsync(
                new Regex(
                    @"/school/143034/st-pauls-church-of-england-academy/primary-performance/pupil-progress(?:/current)?/?$"));
    }

    [Fact]
    public async Task OverviewPage_PrimaryAtAGlance_DoesNotDisplayForSecondaryOnlySchool()
    {
        await Page.GotoAsync(SecondaryOnlyOverviewUrl);

        await Expect(
            Page.GetByRole(
                AriaRole.Heading,
                new()
                {
                    Name = "Primary school profile at a glance",
                    Exact = true
                }))
            .ToHaveCountAsync(0);

        await Expect(
            Page.Locator("#primary-at-a-glance-accordion"))
            .ToHaveCountAsync(0);
    }

    [Fact]
    public async Task OverviewPage_PrimaryAndSecondarySchool_DisplaysBothAtAGlanceSections()
    {
        await Page.GotoAsync(PrimarySecondaryOverviewUrl);

        await Expect(
            Page.Locator("#primary-at-a-glance-accordion"))
            .ToBeVisibleAsync();

        await Expect(
            Page.Locator("#secondary-at-a-glance-accordion"))
            .ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_PrimaryExpectedStandardAccordion_IsSecondAndOpenByDefault()
    {
        await Page.GotoAsync(PrimaryOnlyOverviewUrl);

        var sections =
            Page.Locator(
                "#primary-at-a-glance-accordion .govuk-accordion__section");

        await Expect(sections)
            .ToHaveCountAsync(3);

        var secondSection = sections.Nth(1);

        var button =
            secondSection.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex = new Regex(
                        "Meeting expected standard in reading, writing and maths",
                        RegexOptions.IgnoreCase)
                });

        await Expect(button)
            .ToBeVisibleAsync();

        await Expect(button)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "true");

        await Expect(
            Page.Locator(
                "#primary-at-a-glance-accordion-content-2"))
            .ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_PrimaryExpectedStandardChart_ContainsExpectedData()
    {
        await Page.GotoAsync(PrimaryOnlyOverviewUrl);

        var chart =
            Page.Locator(
                "#overview-primary-expected-standard-chart");

        await Expect(chart)
            .ToBeVisibleAsync();

        var chartData =
            await chart.GetAttributeAsync(
                "data-chart");

        Assert.NotNull(chartData);

        Assert.Contains(
            "\"School\"",
            chartData);

        Assert.Contains(
            "\"Birmingham average\"",
            chartData);

        Assert.Contains(
            "\"England average\"",
            chartData);

        Assert.Contains(
            "70",
            chartData);

        Assert.Contains(
            "73",
            chartData);

        Assert.Contains(
            "77",
            chartData);
    }

    [Fact]
    public async Task OverviewPage_PrimaryExpectedStandardChart_CanBeShownAsTableAndChart()
    {
        await Page.GotoAsync(PrimaryOnlyOverviewUrl);

        var button =
            Page.Locator(
                "#overview-primary-expected-standard-current-year-show-btn");

        var chart =
            Page.Locator(
                "#overview-primary-expected-standard-current-year-chart-container");

        var table =
            Page.Locator(
                "#overview-primary-expected-standard-current-year-table-container");

        await Expect(button)
            .ToHaveTextAsync(
                "Show as a table");

        await Expect(chart)
            .ToBeVisibleAsync();

        await Expect(table)
            .Not.ToBeVisibleAsync();

        await button.ClickAsync();

        await Expect(button)
            .ToHaveTextAsync(
                "Show as a chart");

        await Expect(chart)
            .Not.ToBeVisibleAsync();

        await Expect(table)
            .ToBeVisibleAsync();

        await Expect(table)
            .ToContainTextAsync(
                "School");

        await Expect(table)
            .ToContainTextAsync(
                "70%");

        await Expect(table)
            .ToContainTextAsync(
                "Birmingham average");

        await Expect(table)
            .ToContainTextAsync(
                "73%");

        await Expect(table)
            .ToContainTextAsync(
                "England average");

        await Expect(table)
            .ToContainTextAsync(
                "77%");

        await button.ClickAsync();

        await Expect(button)
            .ToHaveTextAsync(
                "Show as a table");

        await Expect(chart)
            .ToBeVisibleAsync();

        await Expect(table)
            .Not.ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_PrimaryExpectedStandard_LinkNavigatesToMeetingOrExceedingStandardsPage()
    {
        await Page.GotoAsync(PrimaryOnlyOverviewUrl);

        var link =
            Page.Locator(
                    "#primary-at-a-glance-accordion-content-2")
                .GetByRole(
                    AriaRole.Link,
                    new()
                    {
                        Name =
                            "Find out more about what pupils achieved at this school",
                        Exact = true
                    });

        await Expect(link)
            .ToHaveAttributeAsync(
                "href",
                "/school/143034/st-pauls-church-of-england-academy/primary-performance/meeting-or-exceeding-standards");

        Assert.Null(
            await link.GetAttributeAsync(
                "target"));

        await link.ClickAsync();

        await Expect(Page)
            .ToHaveURLAsync(
                new Regex(
                    @"/school/143034/st-pauls-church-of-england-academy/primary-performance/meeting-or-exceeding-standards(?:/current)?/?$"));
    }

    [Fact]
    public async Task OverviewPage_PrimaryExpectedStandardAccordion_HasValidAriaControlsReference()
    {
        await Page.GotoAsync(
            PrimaryOnlyOverviewUrl);

        var button =
            Page.Locator("#primary-at-a-glance-accordion")
                .GetByRole(
                    AriaRole.Button,
                    new()
                    {
                        NameRegex =
                            new Regex(
                                "Meeting expected standard in reading, writing and maths",
                                RegexOptions.IgnoreCase)
                    });

        var ariaControls =
            await button.GetAttributeAsync(
                "aria-controls");

        Assert.Equal(
            "primary-at-a-glance-accordion-content-2",
            ariaControls);

        await Expect(
            Page.Locator(
                $"#{ariaControls}"))
            .ToHaveCountAsync(1);
    }

    [Fact]
    public async Task OverviewPage_PrimaryExtraCurricularAccordion_IsClosedByDefault()
    {
        await Page.GotoAsync(PrimaryOnlyOverviewUrl);

        var accordion =
            Page.Locator("#primary-at-a-glance-accordion");

        var button =
            accordion.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex = new Regex(
                        "Extra-curricular activities",
                        RegexOptions.IgnoreCase)
                });

        var content =
            Page.Locator(
                "#primary-at-a-glance-accordion-content-3");

        await Expect(button)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "false");

        await Expect(content)
            .Not.ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_PrimaryExtraCurricularAccordion_CanBeExpandedAndClosed()
    {
        await Page.GotoAsync(PrimaryOnlyOverviewUrl);

        var accordion =
            Page.Locator("#primary-at-a-glance-accordion");

        var button =
            accordion.GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex = new Regex(
                        "Extra-curricular activities",
                        RegexOptions.IgnoreCase)
                });

        var content =
            Page.Locator(
                "#primary-at-a-glance-accordion-content-3");

        await button.ClickAsync();

        await Expect(button)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "true");

        await Expect(content)
            .ToBeVisibleAsync();

        await button.ClickAsync();

        await Expect(button)
            .ToHaveAttributeAsync(
                "aria-expanded",
                "false");

        await Expect(content)
            .Not.ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_PrimaryExtraCurricular_DisplaysExpectedContent()
    {
        await Page.GotoAsync(PrimaryOnlyOverviewUrl);

        var accordion =
            Page.Locator("#primary-at-a-glance-accordion");

        await accordion
            .GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex = new Regex(
                        "Extra-curricular activities",
                        RegexOptions.IgnoreCase)
                })
            .ClickAsync();

        var content =
            Page.Locator(
                "#primary-at-a-glance-accordion-content-3");

        await Expect(
            content.GetByRole(
                AriaRole.Heading,
                new()
                {
                    Name = "Find out more about upcoming information on extra-curricular activities",
                    Exact = true
                }))
            .ToBeVisibleAsync();

        await Expect(
            content.GetByText(
                "Information on extra-curricular opportunities will be available in the future.",
                new() { Exact = true }))
            .ToBeVisibleAsync();

        await Expect(
            content.GetByRole(
                AriaRole.Link,
                new()
                {
                    Name = "Find out more about what types of activities pupils can take part in and where to find this information",
                    Exact = true
                }))
            .ToBeVisibleAsync();
    }

    [Fact]
    public async Task OverviewPage_PrimaryExtraCurricular_LinkOpensPrimaryCurriculumPageInSameTab()
    {
        await Page.GotoAsync(PrimaryOnlyOverviewUrl);

        var accordion =
            Page.Locator("#primary-at-a-glance-accordion");

        await accordion
            .GetByRole(
                AriaRole.Button,
                new()
                {
                    NameRegex = new Regex(
                        "Extra-curricular activities",
                        RegexOptions.IgnoreCase)
                })
            .ClickAsync();

        var link =
            Page
                .Locator("#primary-at-a-glance-accordion-content-3")
                .GetByRole(
                    AriaRole.Link,
                    new()
                    {
                        Name = "Find out more about what types of activities pupils can take part in and where to find this information",
                        Exact = true
                    });

        await Expect(link)
            .Not.ToHaveAttributeAsync(
                "target",
                "_blank");

        await link.ClickAsync();

        await Expect(Page)
            .ToHaveURLAsync(
                new Regex(
                    @"/school/143034/st-pauls-church-of-england-academy/curriculum/primary/?$"));
    }

    [Fact]
    public async Task OverviewPage_PrimaryExtraCurricularAccordion_HasValidAriaControlsReference()
    {
        await Page.GotoAsync(PrimaryOnlyOverviewUrl);

        var button =
            Page
                .Locator("#primary-at-a-glance-accordion")
                .GetByRole(
                    AriaRole.Button,
                    new()
                    {
                        NameRegex = new Regex(
                            "Extra-curricular activities",
                            RegexOptions.IgnoreCase)
                    });

        var ariaControls =
            await button.GetAttributeAsync("aria-controls");

        Assert.Equal(
            "primary-at-a-glance-accordion-content-3",
            ariaControls);

        await Expect(
            Page.Locator($"#{ariaControls}"))
            .ToHaveCountAsync(1);
    }

    [Fact]
    public async Task OverviewPage_SecondaryOnlySchool_DoesNotDisplayPrimaryExtraCurricularAccordion()
    {
        await Page.GotoAsync(SecondaryOnlyOverviewUrl);

        await Expect(
            Page.Locator("#primary-extra-curricular-toggle"))
            .ToHaveCountAsync(0);
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