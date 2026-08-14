using Microsoft.Playwright;
using SAPPub.Core.Enums;
using SAPPub.Web.Helpers;
using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.Areas.KS2;

[Collection("Playwright Tests")]
public class PupilProgressTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private Dictionary<string, string> _schoolUrnToUrlMap = new Dictionary<string, string>
    {
        ["149976"] = "school/149976/four-elms-primary-school/primary-performance/pupil-progress",
        ["143034"] = "school/143034/st-pauls-church-of-england-academy/primary-performance/pupil-progress"
    };

    [Fact]
    public async Task PupilProgressPage_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task PupilProgressPage_Displays_VerticalNavigation()
    {
        var nav = new VerticalNavigationHelper(Page);
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveOneActiveItemAsync();
        await nav.ShouldHaveActiveHrefAsync(_schoolUrnToUrlMap["143034"]);
    }

    [Fact]
    public async Task PupilProgressPage_Displays_Sub_Navigation()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        var isVisible = await Page.Locator("#sub-navigation-academic-pupilprogress").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task PupilProgressPage_Displays_AllAppropriateStartingElements()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        var academicYearSelector = Page.Locator("#academicYearSelector");
        var academicYearInfo = Page.Locator("#academic-year-info");
        var pupilProgressContent = Page.Locator("#details-pupil-progress");
        var dataNotAvailable = Page.Locator("#data-not-available-custom-card");   // defaults to current year (24/25) (data not available due to covid)

        var readingInformation = Page.Locator("#reading-information");
        var writingInformation = Page.Locator("#writing-information");
        var mathsInformation = Page.Locator("#maths-information");

        // Assert
        Assert.True(await academicYearSelector.IsVisibleAsync());
        Assert.True(await academicYearInfo.IsVisibleAsync());
        Assert.True(await pupilProgressContent.IsVisibleAsync());
        Assert.True(await dataNotAvailable.IsVisibleAsync());
        Assert.False(await readingInformation.IsVisibleAsync());
        Assert.False(await writingInformation.IsVisibleAsync());
        Assert.False(await mathsInformation.IsVisibleAsync());
    }

    [Fact]
    public async Task PupilProgressPage_ChangeAcademicYear_ChangesRelevantContent()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["149976"]);

        // Act, Assert
        await AssertCorrectProgressCardsAsync("reading", false, false);
        await AssertCorrectProgressCardsAsync("writing", false, false);
        await AssertCorrectProgressCardsAsync("maths", false, false);
        var covidInfo = Page.Locator("#data-not-available-custom-card");
        Assert.NotNull(covidInfo);

        // select previous year
        var academicYearSelection = AcademicYearSelection.Previous2;

        var academicyearSelector = Page.Locator("#academicYearSelector");
        await academicyearSelector.SelectOptionAsync([academicYearSelection.GetDisplayName()!]);
        var buttonSelector = Page.Locator("button:has-text(\"Show results\")");
        await buttonSelector.ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        await AssertCorrectProgressCardsAsync("reading", true, false);
        await AssertCorrectProgressCardsAsync("writing", true, false);
        await AssertCorrectProgressCardsAsync("maths", true, false);


        var readingEstablishmentCard = await Page.Locator($"#reading-establishment-card").Locator("p").AllAsync();
        var readingLocalAuthorityCard = await Page.Locator($"#reading-localauthority-card").Locator("p").AllAsync();
    
        var writingEstablishmentCard = Page.Locator($"#writing-establishment-card");
        var writingLocalAuthorityCard = Page.Locator($"#writing-localauthority-card");
        var mathsEstablishmentCard = Page.Locator($"#maths-establishment-card");
        var mathsLocalAuthorityCard = Page.Locator($"#maths-localauthority-card");

        var readingCont1 = await readingEstablishmentCard[0].AllInnerTextsAsync();
        var readingCont2 = await readingEstablishmentCard[1].AllInnerTextsAsync();
        var readingCont3 = await readingLocalAuthorityCard[0].AllInnerTextsAsync();

        Assert.Equal("Pupils at this school score 7. This is above average.", readingCont1[0]);
        Assert.Equal("The confidence interval is 9 to 8", readingCont2[0]);
        Assert.Equal("The local authority average is 4", readingCont3[0]);

    }

    private async Task AssertCorrectProgressCardsAsync(string idPrefix, bool cardIsVisible, bool noDataSectionIsVisible)
    {
        var informationSection = Page.Locator($"#{idPrefix}-information");
        var establishmentCard = Page.Locator($"#{idPrefix}-establishment-card");
        var localAuthorityCard = Page.Locator($"#{idPrefix}-localauthority-card");
        var noAcademicProgressData = informationSection.Locator($".{idPrefix}-no-progress-data");

        Assert.Equal(await informationSection.IsVisibleAsync(), cardIsVisible);
        Assert.Equal(await establishmentCard.IsVisibleAsync(), cardIsVisible);
        Assert.Equal(await localAuthorityCard.IsVisibleAsync(), cardIsVisible);
        Assert.Equal(await noAcademicProgressData.IsVisibleAsync(), noDataSectionIsVisible);
    }
}
