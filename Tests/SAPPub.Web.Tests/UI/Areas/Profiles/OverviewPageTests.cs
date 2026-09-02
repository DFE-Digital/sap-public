using System.Text.RegularExpressions;
using Microsoft.Playwright;
using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.Areas.Profiles;

[Collection("Playwright Tests")]
public class OverviewPageTests(WebApplicationSetupFixture fixture)
    : BasePageTest(fixture)
{
    private const string Urn = "143034";
    private const string SchoolName = "St Paul's Church of England Academy";
    private const string SchoolSlug = "st-pauls-church-of-england-academy";

    private const string OverviewUrl =
        $"school/{Urn}/{SchoolSlug}/overview";

    private const string AboutUrl =
        $"school/{Urn}/{SchoolSlug}/about";

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