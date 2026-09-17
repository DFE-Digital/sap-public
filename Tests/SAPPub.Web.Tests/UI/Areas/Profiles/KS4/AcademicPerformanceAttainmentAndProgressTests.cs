using SAPPub.Core.Enums;
using SAPPub.Playwright.Testing;
using SAPPub.Web.Helpers;
using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.KS4;

[Collection("Playwright Tests")]
public class AcademicPerformanceAttainmentAndProgressTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private string _pageUrl = "school/105574/loreto-high-school-chorlton/secondary-performance/progress-attainment";
    private string _pageUrl2 = "school/107564/todmorden-high-school/secondary-performance/progress-attainment";

    [Fact]
    public async Task AcademicPerformanceAttainmentAndProgressPage_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(_pageUrl);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task AcademicPerformanceAttainmentAndProgressPage_Displays_VerticalNavigation()
    {
        var nav = new VerticalNavigationHelper(Page);
        await Page.GotoAsync(_pageUrl);

        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveOneActiveItemAsync();
        await nav.ShouldHaveActiveHrefAsync(_pageUrl);
    }

    [Fact]
    public async Task AcademicPerformanceAttainmentAndProgressPage_Displays_Sub_Navigation()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#sub-navigation-academic-performance").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AcademicPerformanceAttainmentAndProgressPage_Displays_Attainment8_Details()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#details-attainment8").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AcademicPerformanceAttainmentAndProgressPage_Displays_Progress8_Details()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#details-progress8").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AcademicPerformanceAttainmentAndProgressPage_DisplaysExpectedSections()
    {
        // Arrange
        var progress8CurrentYearId = "prog8-scores-current";
        var progress8PrevYearId = "prog8-scores-prev";
        var progress8Prev2YearId = "prog8-scores-prev2";

        var attainment8CurrentYearId = "attainment8-scores-current";
        var attainment8PrevYearId = "attainment8-scores-prev";
        var attainment8Prev2YearId = "attainment8-scores-prev2";

        // Act
        await Page.GotoAsync(_pageUrl2);
        var content = await Page.ContentAsync();

        // Setup Progress 8 Assertion Objects
        var progress8PreviousYearsAccordion = Page.Locator("#prog8-previous-years-accordion");
        await progress8PreviousYearsAccordion.ClickAsync();

        var progress8CardCurrentYear = Page.GetByTestId(progress8CurrentYearId);
        var progress8EstablishmentCardPrevYear = Page.GetByTestId(progress8PrevYearId);
        var progress8EstablishmentCardPrev2Year = Page.GetByTestId(progress8Prev2YearId);
        var progress8LocalAuthorityAndNationalCardCurrentYear = Page.GetByTestId($"{progress8CurrentYearId}-localauthority-card");
        var progress8LocalAuthorityAndNationalCardPrevYear = Page.GetByTestId($"{progress8PrevYearId}-localauthority-card");
        var progress8LocalAuthorityAndNationalCardPrev2Year = Page.GetByTestId($"{progress8Prev2YearId}-localauthority-card");
        var progress8NoEstablishmentDataCard = Page.GetByTestId($"{progress8CurrentYearId}-no-establishment-data-card");

        // Setup Attainment 8 Assertions Objects
        var attainment8PreviousYearsAccordion = Page.Locator("#attainment8-previous-years-accordion");
        await attainment8PreviousYearsAccordion.ClickAsync();

        var attainment8EstablishmentCardCurrentYear = Page.GetByTestId(attainment8CurrentYearId);
        var attainment8EstablishmentCardPrevYear = Page.GetByTestId(attainment8PrevYearId);
        var attainment8EstablishmentCardPrev2Year = Page.GetByTestId(attainment8Prev2YearId);
        var attainment8LocalAuthorityAndNationalCardCurrentYear = Page.GetByTestId($"{attainment8CurrentYearId}-localauthority-and-national-card");
        var attainment8LocalAuthorityAndNationalCardPrevYear = Page.GetByTestId($"{attainment8PrevYearId}-localauthority-and-national-card");
        var attainment8LocalAuthorityAndNationalCardPrev2Year = Page.GetByTestId($"{attainment8Prev2YearId}-localauthority-and-national-card");
        var attainment8NoEstablishmentDataCard = Page.GetByTestId($"{attainment8CurrentYearId}-no-establishment-data-card");


        var otherPupilCharacteristicsAccordion = Page.Locator("#other-pupil-characteristics-accordion");
        await otherPupilCharacteristicsAccordion.ClickAsync();

        var nonDisadvantagedAdditionalInfoDetails = Page.Locator("#non-disadvantaged-details");
        await nonDisadvantagedAdditionalInfoDetails.ClickAsync();
        var nonDisadvantagedTableCurrentYear = Page.Locator("#breakdown-non-disadvantaged-table");

        var disadvantagedAdditionalInfoDetails = Page.Locator("#disadvantaged-previous-years-details");
        await disadvantagedAdditionalInfoDetails.ClickAsync();

        var disadvantagedTableCurrentYear = Page.Locator("#breakdown-disadvantaged-table-0");
        var disadvantagedTablePreviousYear = Page.Locator("#breakdown-disadvantaged-table-1");
        var disadvantagedTableTwoYearsAgo = Page.Locator("#breakdown-disadvantaged-table-2");

        // Assert
        Assert.False(await progress8CardCurrentYear.IsVisibleAsync());
        Assert.True(await progress8EstablishmentCardPrevYear.IsVisibleAsync());
        Assert.True(await progress8EstablishmentCardPrev2Year.IsVisibleAsync());
        Assert.False(await progress8LocalAuthorityAndNationalCardCurrentYear.IsVisibleAsync());
        Assert.True(await progress8LocalAuthorityAndNationalCardPrevYear.IsVisibleAsync());
        Assert.True(await progress8LocalAuthorityAndNationalCardPrev2Year.IsVisibleAsync());


        Assert.False(await progress8NoEstablishmentDataCard.IsVisibleAsync());

        Assert.True(await attainment8EstablishmentCardCurrentYear.IsVisibleAsync());
        Assert.True(await attainment8EstablishmentCardPrevYear.IsVisibleAsync());
        Assert.True(await attainment8EstablishmentCardPrev2Year.IsVisibleAsync());
        Assert.True(await attainment8LocalAuthorityAndNationalCardCurrentYear.IsVisibleAsync());
        Assert.True(await attainment8LocalAuthorityAndNationalCardPrevYear.IsVisibleAsync());
        Assert.True(await attainment8LocalAuthorityAndNationalCardPrev2Year.IsVisibleAsync());
        Assert.False(await attainment8NoEstablishmentDataCard.IsVisibleAsync());
        
        Assert.True(await otherPupilCharacteristicsAccordion.IsVisibleAsync());
        Assert.True(await disadvantagedTableCurrentYear.IsVisibleAsync());
        Assert.True(await disadvantagedTablePreviousYear.IsVisibleAsync());
        Assert.True(await disadvantagedTableTwoYearsAgo.IsVisibleAsync());
        Assert.True(await nonDisadvantagedTableCurrentYear.IsVisibleAsync());
    }

}
