using SAPPub.Core.Enums;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Areas.Profiles.ViewModels.Overview;

namespace SAPPub.Web.Tests.Unit.Areas.Profiles.Models.Overview;

public class ProgressBandingSummaryViewModelTests
{
    [Theory]
    [InlineData("1", ProgressBanding.WellAboveAverage, "Well above average", "govuk-tag--green", "This school's pupils made much more progress than pupils nationally.")]
    [InlineData("2", ProgressBanding.AboveAverage, "Above average", "app-tag--above-average", "This school's pupils made more progress than pupils nationally.")]
    [InlineData("3", ProgressBanding.Average, "Average", "govuk-tag--blue", "This school's pupils made similar progress to pupils nationally.")]
    [InlineData("4", ProgressBanding.BelowAverage, "Below average", "govuk-tag--orange", "This school's pupils made less progress than pupils nationally.")]
    [InlineData("5", ProgressBanding.WellBelowAverage, "Well below average", "govuk-tag--red", "This school's pupils made much less progress than pupils nationally.")]
    public void Map_ValidBanding_MapsLabelCssClassAndDescription(
        string code,
        ProgressBanding expectedBanding,
        string expectedLabel,
        string expectedCssClass,
        string expectedDescription)
    {
        var result = ProgressBandingSummaryViewModel.Map("Reading", Coded(code));

        Assert.Equal(expectedBanding, result.Banding);
        Assert.Equal(expectedLabel, result.BandingLabel);
        Assert.Equal(expectedCssClass, result.CssClass);
        Assert.Equal(expectedDescription, result.Description);
    }

    [Theory]
    [InlineData("Reading", "reading")]
    [InlineData("Writing", "writing")]
    [InlineData("Maths", "maths")]
    public void Map_SetsSubjectAndLowercaseId(string subject, string expectedId)
    {
        var result = ProgressBandingSummaryViewModel.Map(subject, Coded("3"));

        Assert.Equal(subject, result.Subject);
        Assert.Equal(expectedId, result.Id);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("SUPP")]
    [InlineData("abc")]
    [InlineData("0")]
    [InlineData("6")]
    [InlineData("-1")]
    public void Map_MissingOrInvalidBanding_MapsAsNotApplicable(string? code)
    {
        var result = ProgressBandingSummaryViewModel.Map("Reading", new CodedString(code, string.Empty, code ?? string.Empty));

        AssertNotApplicable(result);
    }

    [Fact]
    public void Map_NullBanding_MapsAsNotApplicable()
    {
        var result = ProgressBandingSummaryViewModel.Map("Reading", null);

        AssertNotApplicable(result);
    }

    [Theory]
    [InlineData("0", true)]
    [InlineData(" 0 ", true)]
    [InlineData("1", false)]
    [InlineData("SUPP", false)]
    public void Map_Significance_SetsIsNotStatisticallySignificant(string significance, bool expected)
    {
        var result = ProgressBandingSummaryViewModel.Map("Reading", Coded("3"), Coded(significance));

        Assert.Equal(expected, result.IsNotStatisticallySignificant);
    }

    [Fact]
    public void Map_NullOrMissingSignificance_IsNotFlaggedAsNotSignificant()
    {
        var withNull = ProgressBandingSummaryViewModel.Map("Reading", Coded("3"), null);
        var withNoValue = ProgressBandingSummaryViewModel.Map("Reading", Coded("3"), new CodedString(null, string.Empty, string.Empty));
        var withDefault = ProgressBandingSummaryViewModel.Map("Reading", Coded("3"));

        Assert.False(withNull.IsNotStatisticallySignificant);
        Assert.False(withNoValue.IsNotStatisticallySignificant);
        Assert.False(withDefault.IsNotStatisticallySignificant);
    }

    [Fact]
    public void Map_NotSignificant_DoesNotAffectBanding()
    {
        var result = ProgressBandingSummaryViewModel.Map("Reading", Coded("2"), Coded("0"));

        Assert.Equal(ProgressBanding.AboveAverage, result.Banding);
        Assert.True(result.IsNotStatisticallySignificant);
    }

    private static void AssertNotApplicable(ProgressBandingSummaryViewModel result)
    {
        Assert.Null(result.Banding);
        Assert.Equal(Web.Constants.Constants.NotApplicable, result.BandingLabel);
        Assert.Equal("govuk-tag--grey", result.CssClass);
        Assert.Equal("This school's data is 'Not applicable'", result.Description);
    }

    private static CodedString Coded(string value) => new(value, string.Empty, value);
}
