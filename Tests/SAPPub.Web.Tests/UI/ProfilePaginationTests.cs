using SAPPub.Web.Constants;
using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;
using static SAPPub.Web.Constants.PageTitleConstants;

namespace SAPPub.Web.Tests.UI.Areas.Profiles;

[Collection("Playwright Tests")]
public class ProfilePaginationTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private readonly Dictionary<string, string> _schoolUrnToUrlMap = new()
    {
        ["135600"] = "school/135600/ark-academy/overview", //KS2 + KS4 + KS5
        ["150009"] = "school/150009/abraham-moss-community-school/overview", //KS2 + KS4
        ["137552"] = "school/137552/stewards-academy-science-specialist-harlow/overview", //KS4
        ["149328"] = "school/149328/king-edward-vi-high-school/overview", //KS4 + KS5
        ["130499"] = "school/130499/holy-cross-college/overview", //KS5
        ["143034"] = "school/143034/st-pauls-church-of-england-academy/overview", //KS2
    };

    [Fact]
    public async Task NavigateThroughPaginationNav_ShowsExpectedPages_ForPrimaryOnlySchool()
    {
        await NavigateAndAssertPaginationSequenceAsync(
            "143034",
            [
                PageTitles.Overview,
                PageTitles.About,
                PrimarySchoolPageTitles.Admissions,
                PrimarySchoolPageTitles.Curriculum,
                PageTitles.Attendance,
                PrimarySchoolPageTitles.PupilProgress,
                PrimarySchoolPageTitles.MeetingOrExceedingStandards,
                PrimarySchoolPageTitles.SubjectScaledScores,
                PrimarySchoolPageTitles.AdditionalMeasures,
            ]);
    }

    [Fact]
    public async Task NavigateThroughPaginationNav_ShowsExpectedPages_ForSecondaryOnlySchool()
    {
        await NavigateAndAssertPaginationSequenceAsync(
            "137552",
            [
                PageTitles.Overview,
                PageTitles.About,
                SecondarySchoolPageTitles.Admissions,
                SecondarySchoolPageTitles.Curriculum,
                PageTitles.Attendance,
                SecondarySchoolPageTitles.ProgressAndAttainment,
                SecondarySchoolPageTitles.EnglishAndMathsResults,
                SecondarySchoolPageTitles.SubjectsEntered,
                SecondarySchoolPageTitles.AdditionalMeasures,
                SecondarySchoolPageTitles.Destinations,
            ]);
    }

    [Fact]
    public async Task NavigateThroughPaginationNav_ShowsExpectedPages_ForSixteenToNineteenOnlySchool()
    {
        await NavigateAndAssertPaginationSequenceAsync(
            "130499",
            [
                PageTitles.Overview,
                PageTitles.About,
                KS5SchoolPageTitles.Level3Qualifications,
                KS5SchoolPageTitles.Level2Qualifications,
                KS5SchoolPageTitles.EnglishAndMaths,
                KS5SchoolPageTitles.SubjectsEntered,
                KS5SchoolPageTitles.DestinationsUrl,
                KS5SchoolPageTitles.DestinationsHigherUrl,
            ]);
    }

    [Fact]
    public async Task NavigateThroughPaginationNav_ShowsExpectedPages_ForPrimaryAndSecondarySchool()
    {
        await NavigateAndAssertPaginationSequenceAsync(
            "150009",
            [
                PageTitles.Overview,
                PageTitles.About,
                PrimarySchoolPageTitles.Admissions,
                SecondarySchoolPageTitles.Admissions,
                PrimarySchoolPageTitles.Curriculum,
                SecondarySchoolPageTitles.Curriculum,
                PageTitles.Attendance,
                PrimarySchoolPageTitles.PupilProgress,
                PrimarySchoolPageTitles.MeetingOrExceedingStandards,
                PrimarySchoolPageTitles.SubjectScaledScores,
                PrimarySchoolPageTitles.AdditionalMeasures,
                SecondarySchoolPageTitles.ProgressAndAttainment,
                SecondarySchoolPageTitles.EnglishAndMathsResults,
                SecondarySchoolPageTitles.SubjectsEntered,
                SecondarySchoolPageTitles.AdditionalMeasures,
                SecondarySchoolPageTitles.Destinations,
            ]);
    }

    [Fact]
    public async Task NavigateThroughPaginationNav_ShowsExpectedPages_ForSecondaryAndSixteenToNineteenSchool()
    {
        await NavigateAndAssertPaginationSequenceAsync(
            "149328",
            [
                PageTitles.Overview,
                PageTitles.About,
                SecondarySchoolPageTitles.Admissions,
                SecondarySchoolPageTitles.Curriculum,
                PageTitles.Attendance,
                SecondarySchoolPageTitles.ProgressAndAttainment,
                SecondarySchoolPageTitles.EnglishAndMathsResults,
                SecondarySchoolPageTitles.SubjectsEntered,
                SecondarySchoolPageTitles.AdditionalMeasures,
                KS5SchoolPageTitles.Level3Qualifications,
                KS5SchoolPageTitles.Level2Qualifications,
                KS5SchoolPageTitles.EnglishAndMaths,
                KS5SchoolPageTitles.SubjectsEntered,
                SecondarySchoolPageTitles.Destinations,
                KS5SchoolPageTitles.DestinationsUrl,
                KS5SchoolPageTitles.DestinationsHigherUrl,
            ]);
    }

    [Fact]
    public async Task NavigateThroughPaginationNav_ShowsExpectedPages_ForAllThroughSchool()
    {
        await NavigateAndAssertPaginationSequenceAsync(
            "135600",
            [
                PageTitles.Overview,
                PageTitles.About,
                PrimarySchoolPageTitles.Admissions,
                SecondarySchoolPageTitles.Admissions,
                PrimarySchoolPageTitles.Curriculum,
                SecondarySchoolPageTitles.Curriculum,
                PageTitles.Attendance,
                PrimarySchoolPageTitles.PupilProgress,
                PrimarySchoolPageTitles.MeetingOrExceedingStandards,
                PrimarySchoolPageTitles.SubjectScaledScores,
                PrimarySchoolPageTitles.AdditionalMeasures,
                SecondarySchoolPageTitles.ProgressAndAttainment,
                SecondarySchoolPageTitles.EnglishAndMathsResults,
                SecondarySchoolPageTitles.SubjectsEntered,
                SecondarySchoolPageTitles.AdditionalMeasures,
                KS5SchoolPageTitles.Level3Qualifications,
                KS5SchoolPageTitles.Level2Qualifications,
                KS5SchoolPageTitles.EnglishAndMaths,
                KS5SchoolPageTitles.SubjectsEntered,
                SecondarySchoolPageTitles.Destinations,
                KS5SchoolPageTitles.DestinationsUrl,
                KS5SchoolPageTitles.DestinationsHigherUrl,
            ]);
    }

    private async Task NavigateAndAssertPaginationSequenceAsync(string urn, string[] expectedTitleSubstrings)
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap[urn]);
        var nav = new PaginationNavigationHelper(Page);

        // Assert first page (Overview)
        var title = await Page.TitleAsync();
        Assert.Contains(expectedTitleSubstrings[0], title);

        // Act + Assert remaining pages by clicking "next" through the pagination nav
        for (var i = 1; i < expectedTitleSubstrings.Length; i++)
        {
            await nav.ClickNextLinkAsync();

            title = await Page.TitleAsync();
            Assert.Contains(expectedTitleSubstrings[i], title);
        }
    }
}