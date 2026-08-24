using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Playwright;
using SAPPub.Core.Enums;
using SAPPub.Web.Helpers;
using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.Areas.Profiles.KS2;

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
        var isVisible = await Page.Locator("#sub-navigation-academic-performance").IsVisibleAsync();

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

        var academicYearSelector = Page.Locator("#academicYearSelector");
        var academicYearInfo = Page.Locator("#academic-year-info");
        var dataNotAvailable = Page.Locator("#data-not-available-custom-card");

        // Act, Assert
        await AssertCorrectProgressCardsAsync("reading", false, false);
        await AssertCorrectProgressCardsAsync("writing", false, false);
        await AssertCorrectProgressCardsAsync("maths", false, false);
       
        Assert.True(await dataNotAvailable.IsVisibleAsync());
        Assert.Contains(AcademicYearSelection.Current.GetDisplayName()!, await academicYearInfo.InnerTextAsync());

        // select previous year
        var academicYearSelection = AcademicYearSelection.Previous2;

        await academicYearSelector.SelectOptionAsync(new SelectOptionValue { Label = academicYearSelection.GetDisplayName()! });
        var buttonSelector = Page.GetByRole(AriaRole.Button, new() { Name = "Show results" });

        await buttonSelector.ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert Previous year (2) content
        await AssertCorrectProgressCardsAsync("reading", true, false);
        await AssertCorrectProgressCardsAsync("writing", true, false);
        await AssertCorrectProgressCardsAsync("maths", true, false);
        Assert.False(await dataNotAvailable.IsVisibleAsync());
        Assert.Contains(academicYearSelection.GetDisplayName()!, await academicYearInfo.InnerTextAsync());
        Assert.Equal(AcademicYearSelection.Previous2.ToString(), await academicYearSelector.InputValueAsync());

        // Reading assertions
        var readingEstablishmentCard = await Page.Locator("#reading-establishment-card").InnerTextAsync();
        var readingLaCard = await Page.Locator("#reading-localauthority-card").InnerTextAsync();
        Assert.Contains("Pupils at this school score 7.", readingEstablishmentCard);
        Assert.Contains("This is above average.", readingEstablishmentCard);
        Assert.Contains("The confidence interval is 9 to 8", readingEstablishmentCard);
        Assert.Contains("The local authority average is 4", readingLaCard);

        // Writing assertions
        var writingEstablishmentCard = await Page.Locator("#writing-establishment-card").InnerTextAsync();
        var writingLaCard = await Page.Locator("#writing-localauthority-card").InnerTextAsync();
        Assert.Contains("Pupils at this school score 10.", writingEstablishmentCard);
        Assert.Contains("This is well above average.", writingEstablishmentCard);
        Assert.Contains("The confidence interval is 12 to 11", writingEstablishmentCard);
        Assert.Contains("The local authority average is 5", writingLaCard);

        // Maths assertions
        var mathsEstablishmentCard = await Page.Locator("#maths-establishment-card").InnerTextAsync();
        var mathsLaCard = await Page.Locator("#maths-localauthority-card").InnerTextAsync();
        Assert.Contains("Pupils at this school score 13.", mathsEstablishmentCard);
        Assert.Contains("This is average.", mathsEstablishmentCard);
        Assert.Contains("The confidence interval is 15 to 14", mathsEstablishmentCard);
        Assert.Contains("The local authority average is 6", mathsLaCard);

        // Act select previous year with no progress data
        var previousYearSelection = AcademicYearSelection.Previous;
        await academicYearSelector.SelectOptionAsync(new SelectOptionValue { Label = previousYearSelection.GetDisplayName()! });
        await buttonSelector.ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await AssertCorrectProgressCardsAsync("reading", false, false);
        await AssertCorrectProgressCardsAsync("writing", false, false);
        await AssertCorrectProgressCardsAsync("maths", false, false);

        Assert.False(await dataNotAvailable.IsVisibleAsync());
        Assert.Contains(previousYearSelection.GetDisplayName()!, await academicYearInfo.InnerTextAsync());
        Assert.Equal(previousYearSelection.ToString(), await academicYearSelector.InputValueAsync());

        var cont = await Page.ContentAsync();

        var noProgressDataCard = Page.Locator(".nodata-no-progress-data");
        Assert.True(await noProgressDataCard.IsVisibleAsync());
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
