using SAPPub.Core.Enums;
using SAPPub.Web.Helpers;
using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.SecondarySchool;

[Collection("Playwright Tests")]
public class AcademicPerformanceAttainmentAndProgressTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private string _pageUrl = "school/105574/loreto-high-school-chorlton/secondary/academic-performance-attainment-and-progress";

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
    public async Task AcademicPerformanceAttainmentAndProgressPage_Displays_AcademicYear_Selector()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var academicYearSelector = Page.Locator("#academicYearSelector");
        var progress8CustomCard = Page.GetByTestId("progress8-custom-card");

        var attainment8EstablishmentCard = Page.GetByTestId("attainment8-establishment-card");
        var attainment8LocalAuthorityAndNationalCard = Page.GetByTestId("attainment8-localauthority-and-national-card");
        var attainmnet8NoEstablishmentDataCard = Page.GetByTestId("attainment8-no-establishment-data-card");

        // Assert
        Assert.True(await academicYearSelector.IsVisibleAsync());
        Assert.True(await progress8CustomCard.IsVisibleAsync());
        Assert.True(await attainment8EstablishmentCard.IsVisibleAsync());
        Assert.True(await attainment8LocalAuthorityAndNationalCard.IsVisibleAsync());
        Assert.False(await attainmnet8NoEstablishmentDataCard.IsVisibleAsync());
    }

    [Fact]
    public async Task AcademicPerformanceAttainmentAndProgressPage_ChangeAcademicYear_ChangesRelevantContent()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act, Assert
        await AssertCorrectProgress8CardAsync("progress8-custom-card");
        await AssertCorrectAttainmentCardAsync("attainment8-establishment-card");

        // select previous year
        var academicYearSelection = AcademicYearSelection.Previous; // establishment has previous year's data

        var academicyearSelector = Page.Locator("#academicYearSelector");
        await academicyearSelector.SelectOptionAsync([academicYearSelection.GetDisplayName()!]);
        var buttonSelector = Page.Locator("button:has-text(\"Show results\")");
        await buttonSelector.ClickAsync();

        // Assert
        await AssertCorrectProgress8CardAsync("progress8-establishment-card");
        await AssertCorrectAttainmentCardAsync("attainment8-establishment-card");

        var progress8PupilDetails = Page.Locator("#pupil-details-progress8");
        Assert.True(await progress8PupilDetails.IsVisibleAsync());

        var progress8LocalAuthorityCard = Page.GetByTestId("progress8-localauthority-card");
        Assert.True(await progress8LocalAuthorityCard.IsVisibleAsync());

        var attainment8EstablishmentCard = Page.GetByTestId("attainment8-establishment-card");
        Assert.True(await attainment8EstablishmentCard.IsVisibleAsync());

        var attainment8LocalAuthorityAndNationlCard = Page.GetByTestId("attainment8-localauthority-and-national-card");
        Assert.True(await attainment8LocalAuthorityAndNationlCard.IsVisibleAsync());

        var paragraphStatingYear = Page.GetByTestId("academic-year-info");
        Assert.True(await paragraphStatingYear.IsVisibleAsync());
        Assert.Contains(academicYearSelection.GetDisplayName()!, await paragraphStatingYear.InnerTextAsync());
        var attainmnet8NoEstablishmentDataCard = Page.GetByTestId("attainment8-no-establishment-data-card");
        Assert.False(await attainmnet8NoEstablishmentDataCard.IsVisibleAsync());

        // select previous2 year
        academicYearSelection = AcademicYearSelection.Previous2; // establishment has no previous2 year's data
        await academicyearSelector.SelectOptionAsync([academicYearSelection.GetDisplayName()!]);
        buttonSelector = Page.Locator("button:has-text(\"Show results\")");
        await buttonSelector.ClickAsync();

        // Assert
        await AssertCorrectProgress8CardAsync("progress8-no-establishment-data-card");
        await AssertCorrectAttainmentCardAsync("attainment8-no-establishment-data-card");

        paragraphStatingYear = Page.GetByTestId("academic-year-info");
        Assert.True(await paragraphStatingYear.IsVisibleAsync());
        Assert.Contains(academicYearSelection.GetDisplayName()!, await paragraphStatingYear.InnerTextAsync());
    }

    [Theory]
    [InlineData(AcademicYearSelection.Current)]
    [InlineData(AcademicYearSelection.Previous)]
    [InlineData(AcademicYearSelection.Previous2)]
    public async Task AcademicPerformanceAttainmentAndProgressPage_Display_No_Attainment8_Info(AcademicYearSelection academicYearSelection)
    {
        // Arrange
        _pageUrl = "school/100273/Saint%20Paul%20Roman%20Catholic%20Infant%20School/secondary/academic-performance-attainment-and-progress";
        await Page.GotoAsync(_pageUrl);

        // Act
        var academicyearSelector = Page.Locator("#academicYearSelector");
        await academicyearSelector.SelectOptionAsync([academicYearSelection.GetDisplayName()!]);
        var buttonSelector = Page.Locator("button:has-text(\"Show results\")");
        await buttonSelector.ClickAsync();

        // Assert        
        var attainmnet8NoEstablishmentDataCard = Page.GetByTestId("attainment8-no-establishment-data-card");
        Assert.True(await attainmnet8NoEstablishmentDataCard.IsVisibleAsync());
    }

    [Fact]
    public async Task AcademicPerformanceAttainmentAndProgressPage_DisplaysPagination()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#academic-performance-attainment-and-progress-pagination").IsVisibleAsync();
        var previousPaginationLink = Page.Locator("#academic-performance-attainment-and-progress-pagination .govuk-pagination__prev a");
        var nextPaginationLink = Page.Locator("#academic-performance-attainment-and-progress-pagination .govuk-pagination__next a");

        var previousPaginationText = await previousPaginationLink.TextContentAsync();
        var nextPaginationText = await nextPaginationLink.TextContentAsync();

        // Assert
        Assert.True(isVisible);
        Assert.Equal("Attendance", previousPaginationText?.Trim());
        Assert.Equal("Academic performance: English and maths results", nextPaginationText?.Trim());
    }

    private async Task AssertCorrectProgress8CardAsync(string expectedcardTestId)
    {
        var progress8EstablishmentCard = Page.GetByTestId("progress8-establishment-card");
        var progress8LocalAuthorityCard = Page.GetByTestId("progress8-localauthority-card");
        var progress8CustomCard = Page.GetByTestId("progress8-custom-card");
        var progress8NoEstablishmentDataCard = Page.GetByTestId("progress8-no-establishment-data-card");
        var progress8EstablishmentCardVisible = await progress8EstablishmentCard.IsVisibleAsync();
        var progress8LocalAuthorityCardVisible = await progress8LocalAuthorityCard.IsVisibleAsync();
        var progress8CustomCardVisible = await progress8CustomCard.IsVisibleAsync();
        var progress8NoEstablishmentDataCardVisible = await progress8NoEstablishmentDataCard.IsVisibleAsync();
        Assert.True(expectedcardTestId switch
        {
            "progress8-establishment-card" =>
                progress8EstablishmentCardVisible
                    && progress8LocalAuthorityCardVisible
                    && !progress8CustomCardVisible
                    && !progress8NoEstablishmentDataCardVisible,
            "progress8-custom-card" =>
                !progress8EstablishmentCardVisible
                    && !progress8LocalAuthorityCardVisible
                    && progress8CustomCardVisible
                    && !progress8NoEstablishmentDataCardVisible,
            "progress8-no-establishment-data-card" =>
                !progress8EstablishmentCardVisible
                    && !progress8LocalAuthorityCardVisible
                    && !progress8CustomCardVisible
                    && progress8NoEstablishmentDataCardVisible,
            _ => false
        });
    }

    private async Task AssertCorrectAttainmentCardAsync(string expectedCardTestId)
    {
        var attainment8EstablishmentCard = Page.GetByTestId("attainment8-establishment-card");
        var attainment8LocalAuthorityCard = Page.GetByTestId("attainment8-localauthority-and-national-card");
        var attainment8NoEstablishmentDataCard = Page.GetByTestId("attainment8-no-establishment-data-card");
        var attainment8EstablishmentCardVisible = await attainment8EstablishmentCard.IsVisibleAsync();
        var attainment8LocalAuthorityCardVisible = await attainment8LocalAuthorityCard.IsVisibleAsync();
        var attainment8NoEstablishmentDataCardVisible = await attainment8NoEstablishmentDataCard.IsVisibleAsync();
        Assert.True(expectedCardTestId switch
        {
            "attainment8-establishment-card" =>
                attainment8EstablishmentCardVisible
                    && attainment8LocalAuthorityCardVisible
                    && !attainment8NoEstablishmentDataCardVisible,
            "attainment8-no-establishment-data-card" =>
                !attainment8EstablishmentCardVisible
                    && !attainment8LocalAuthorityCardVisible
                    && attainment8NoEstablishmentDataCardVisible,
            _ => false
        });
    }
}
