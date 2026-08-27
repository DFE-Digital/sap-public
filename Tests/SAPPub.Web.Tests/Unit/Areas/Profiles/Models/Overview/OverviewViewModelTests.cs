using SAPPub.Core.ServiceModels.Overview;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Areas.Profiles.ViewModels.Overview;

namespace SAPPub.Web.Tests.Unit.Areas.Profiles.Models.Overview;

public class OverviewViewModelTests
{
    [Fact]
    public void Map_MapsAndFormatsAllFields()
    {
        var model = CreateCompleteModel();

        var result = OverviewViewModel.Map(model);

        Assert.Equal("123456", result.URN);
        Assert.Equal("Test School", result.SchoolName);
        Assert.True(result.IsKS2);
        Assert.True(result.IsKS4);
        Assert.False(result.IsKS5);
        Assert.Equal("Primary and Secondary", result.EducationPhase);

        Assert.True(result.AgeRange.IsAvailable);
        Assert.Equal("11 to 16", result.AgeRange.Value);
        Assert.True(result.NumberOfPupils.IsAvailable);
        Assert.Equal("1,234", result.NumberOfPupils.Value);
        Assert.Equal("ASD", result.SenTypes.Value);
        Assert.Equal("0114 123 4567", result.Telephone.Value);
        Assert.Equal("https://school.example", result.SchoolWebsite.Value);

        Assert.Equal(52.1, result.Attainment8.Value.Value);
        Assert.Equal(42.5, result.MoreThanOneForeignLanguage.Value.Value);

        AssertComparison(result.EnglishAndMathsGrade5, 61.2, 58.3, 59.4);
        AssertComparison(result.Destinations, 91.1, 89.2, 90.3);
        AssertComparison(result.ReadingWritingMathsExpected, 67.1, 65.2, 64.3);
        AssertComparison(result.ReadingWritingMathsHigher, 12.1, 11.2, 10.3);

        Assert.Equal(
            "http://reports.ofsted.gov.uk/inspection-reports/find-inspection-report/provider/ELS/123456",
            result.OfstedReportUrl);
    }

    [Theory]
    [InlineData(null, "16")]
    [InlineData("", "16")]
    [InlineData(" ", "16")]
    [InlineData("11", null)]
    [InlineData("11", "")]
    [InlineData("11", " ")]
    public void Map_AgeRangeIsNotAvailable_WhenEitherAgeIsMissing(
        string? ageLow,
        string? ageHigh)
    {
        var model = CreateMinimalModel(
            ageRangeLow: ageLow ?? string.Empty,
            ageRangeHigh: ageHigh ?? string.Empty);

        var result = OverviewViewModel.Map(model);

        Assert.False(result.AgeRange.IsAvailable);
        Assert.True(result.AgeRange.IsNotAvailable);
    }

    [Theory]
    [InlineData("1234", "1,234", true)]
    [InlineData("12", "12", true)]
    [InlineData("unknown", "unknown", true)]
    [InlineData("", null, false)]
    [InlineData(" ", null, false)]
    public void Map_FormatsNumberOfPupils(
        string numberOfPupils,
        string? expected,
        bool isAvailable)
    {
        var model = CreateMinimalModel(numberOfPupils: numberOfPupils);

        var result = OverviewViewModel.Map(model);

        Assert.Equal(isAvailable, result.NumberOfPupils.IsAvailable);
        Assert.Equal(expected, result.NumberOfPupils.Value);
    }

    [Fact]
    public void Map_MapsNullSimpleFieldsToNotAvailable()
    {
        var model = CreateMinimalModel(
            senProvision: null,
            phone: string.Empty,
            website: string.Empty,
            attainment8: null,
            moreThanOneForeignLanguage: null);

        var result = OverviewViewModel.Map(model);

        Assert.True(result.SenTypes.IsNotAvailable);
        Assert.True(result.Telephone.IsNotAvailable);
        Assert.True(result.SchoolWebsite.IsNotAvailable);
        Assert.True(result.Attainment8.IsNotAvailable);
        Assert.True(result.MoreThanOneForeignLanguage.IsNotAvailable);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void Map_ComparisonIsNull_WhenAnyComparisonValueIsMissing(int missingIndex)
    {
        CodedDouble? school = Coded(61.2);
        CodedDouble? la = Coded(58.3);
        CodedDouble? england = Coded(59.4);

        if (missingIndex == 0) school = null;
        if (missingIndex == 1) la = null;
        if (missingIndex == 2) england = null;

        var model = CreateMinimalModel(
            englishAndMathsGrade5Establishment: school,
            englishAndMathsGrade5LA: la,
            englishAndMathsGrade5England: england);

        var result = OverviewViewModel.Map(model);

        Assert.Null(result.EnglishAndMathsGrade5);
    }

    [Fact]
    public void Map_CodedValueWithoutNumericValue_IsStillMappedAsAvailableDisplayField()
    {
        var coded = new CodedDouble(null, "Not available", "x");
        var model = CreateMinimalModel(attainment8: coded);

        var result = OverviewViewModel.Map(model);

        Assert.True(result.Attainment8.IsAvailable);
        Assert.NotNull(result.Attainment8.Value);

        Assert.Null(result.Attainment8.Value.Value);
        Assert.Equal("Not available", result.Attainment8.Value.Reason);
    }

    private static OverviewModel CreateCompleteModel() => new()
    {
        Urn = "123456",
        SchoolName = "Test School",
        AgeRangeLow = "11",
        AgeRangeHigh = "16",
        NumberOfPupils = "1234",
        SenProvision = "ASD",
        Phone = "0114 123 4567",
        Website = "https://school.example",
        IsKS2 = true,
        IsKS4 = true,
        IsKS5 = false,
        Attainment8 = Coded(52.1),
        MoreThanOneForeignLanguage = Coded(42.5),
        EnglishAndMathsGrade5Establishment = Coded(61.2),
        EnglishAndMathsGrade5LA = Coded(58.3),
        EnglishAndMathsGrade5England = Coded(59.4),
        DestinationsEstablishment = Coded(91.1),
        DestinationsLA = Coded(89.2),
        DestinationsEngland = Coded(90.3),
        ReadingWritingMathsExpectedEstablishment = Coded(67.1),
        ReadingWritingMathsExpectedLA = Coded(65.2),
        ReadingWritingMathsExpectedEngland = Coded(64.3),
        ReadingWritingMathsHigherEstablishment = Coded(12.1),
        ReadingWritingMathsHigherLA = Coded(11.2),
        ReadingWritingMathsHigherEngland = Coded(10.3)
    };

    private static OverviewModel CreateMinimalModel(
        string ageRangeLow = "",
        string ageRangeHigh = "",
        string numberOfPupils = "",
        string? senProvision = null,
        string phone = "",
        string website = "",
        CodedDouble? attainment8 = null,
        CodedDouble? moreThanOneForeignLanguage = null,
        CodedDouble? englishAndMathsGrade5Establishment = null,
        CodedDouble? englishAndMathsGrade5LA = null,
        CodedDouble? englishAndMathsGrade5England = null) => new()
    {
        Urn = "123456",
        SchoolName = "Test School",
        AgeRangeLow = ageRangeLow,
        AgeRangeHigh = ageRangeHigh,
        NumberOfPupils = numberOfPupils,
        SenProvision = senProvision,
        Phone = phone,
        Website = website,
        Attainment8 = attainment8,
        MoreThanOneForeignLanguage = moreThanOneForeignLanguage,
        EnglishAndMathsGrade5Establishment = englishAndMathsGrade5Establishment,
        EnglishAndMathsGrade5LA = englishAndMathsGrade5LA,
        EnglishAndMathsGrade5England = englishAndMathsGrade5England
    };

    private static CodedDouble Coded(double value) =>
        new(value, string.Empty, value.ToString(System.Globalization.CultureInfo.InvariantCulture));

    private static void AssertComparison(
        SAPPub.Web.Areas.Profiles.ViewModels.SimpleCodedDoubleTableViewModel? comparison,
        double school,
        double la,
        double england)
    {
        Assert.NotNull(comparison);
        Assert.Equal(school, comparison.SchoolOrCollege.Value.Value);
        Assert.Equal(la, comparison.LocalAuthority.Value.Value);
        Assert.Equal(england, comparison.England.Value.Value);
    }
}
