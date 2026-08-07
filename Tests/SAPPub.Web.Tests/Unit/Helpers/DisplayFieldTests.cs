using SAPPub.Core.ValueObjects;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Tests.Unit.Helpers;

public class DisplayFieldTests
{
    [Fact]
    public void DisplayFieldString_Available_ShouldBeAvailable()
    {
        var raw = "Test";
        var field = raw.ToDisplayField();

        Assert.NotNull(field);
        Assert.True(field.IsAvailable);
        Assert.False(field.IsNotAvailable);
        Assert.Equal(raw, field.DisplayText());
    }

    [Fact]
    public void DisplayFieldString_NotAvailable_ShouldReturnDefaultText()
    {
        string? raw = null;
        var field = raw.ToDisplayField();

        Assert.NotNull(field);
        Assert.False(field.IsAvailable);
        Assert.True(field.IsNotAvailable);
        Assert.Equal("Not available", field.DisplayText());
    }

    [Fact]
    public void DisplayFieldDecimal_Available_ShouldBeAvailable()
    {
        decimal value = 1234.56m;
        var field = value.ToDisplayField();

        Assert.NotNull(field);
        Assert.True(field.IsAvailable);
        Assert.False(field.IsNotAvailable);
        Assert.Equal(value, field.Value);
    }

    [Fact]
    public void DisplayFieldDecimal_Available_FormatText()
    {
        decimal value = 1234.547m;
        var field = value.ToDisplayField();

        Assert.NotNull(field);
        Assert.True(field.IsAvailable);
        Assert.False(field.IsNotAvailable);

        var result = field.DisplayText(d => $"{d:F2}");

        Assert.Equal("1234.55", result);
    }

    [Fact]
    public void DisplayFieldDecimal_NotAvailable_ShouldReturnDefaultText()
    {
        string? raw = null;
        var field = raw.ToDisplayField();

        Assert.NotNull(field);
        Assert.False(field.IsAvailable);
        Assert.True(field.IsNotAvailable);
        Assert.Equal("Not available", field.DisplayText());
    }

    [Fact]
    public void DisplayFieldDateTime_Available_FormatText()
    {
        var value = new DateTime(2026, 3, 20);

        var field = value.ToDisplayField();

        Assert.NotNull(field);
        Assert.True(field.IsAvailable);
        Assert.False(field.IsNotAvailable);

        var result = field.DisplayText(d => d.ToString("dd MMM yyyy"));

        Assert.Equal("20 Mar 2026", result);
    }

    [Fact]
    public void DisplayFieldDateTime_NotAvailable_ShouldReturnDefaultText()
    {
        DateTime? value = null;
        var field = value.ToDisplayField();

        Assert.NotNull(field);
        Assert.False(field.IsAvailable);
        Assert.True(field.IsNotAvailable);
        Assert.Equal("Not available", field.DisplayText());
    }

    [Fact]
    public void DisplayNumber_HasValue_ReturnsValue()
    {
        var property = new CodedDouble(20.2, string.Empty, "20.2").ToDisplayField();

        var result = property.DisplayNumber();

        Assert.Equal("20.2", result);
    }

    [Fact]
    public void DisplayNumber_HasValue_WithFormat_ReturnsFormattedValue()
    {
        var property = new CodedDouble(20.222, string.Empty, "20.222").ToDisplayField();

        var result = property.DisplayNumber("F1");

        Assert.Equal("20.2", result);
    }

    [Fact]
    public void DisplayNumber_NoValue_DefaultDisplayReasonFalse_ReturnsNotAvailable()
    {
        var property = new CodedDouble(null, "Redacted for confidentiality", "c").ToDisplayField();

        var result = property.DisplayNumber();

        Assert.Equal("Not available", result);
    }

    [Fact]
    public void DisplayNumber_NoValue_DefaultDisplayReasonTrue_ReturnsReason()
    {
        var property = new CodedDouble(null, "Redacted for confidentiality", "c").ToDisplayField();

        var result = property.DisplayNumber(displayReason: true);

        Assert.Equal("Redacted for confidentiality", result);
    }

    [Fact]
    public void DisplayPercentage_HasValue_ReturnsFormattedPercentage()
    {
        var property = new CodedDouble(20.2, string.Empty, "20.2").ToDisplayField();

        var result = property.DisplayPercentage();

        Assert.Equal("20.2%", result);
    }

    [Fact]
    public void DisplayPercentage_HasValue_DefaultDisplayReasonFalse_ReturnsNotAvailable()
    {
        var property = new CodedDouble(null, "Redacted for confidentiality", "c").ToDisplayField();

        var result = property.DisplayPercentage();

        Assert.Equal("Not available", result);
    }

    [Fact]
    public void DisplayPercentage_HasValue_DefaultDisplayReasonTrue_ReturnsReason()
    {
        var property = new CodedDouble(null, "Redacted for confidentiality", "c").ToDisplayField();

        var result = property.DisplayPercentage(displayReason: true);

        Assert.Equal("Redacted for confidentiality", result);
    }

    [Fact]
    public void DisplayFieldCodedDouble_Available_FormatText()
    {
        var value = new CodedDouble(1234.5478, string.Empty, string.Empty);
        var field = value.ToDisplayField();

        Assert.NotNull(field);
        Assert.True(field.IsAvailable);
        Assert.False(field.IsNotAvailable);

        var result = field.DisplayText(d => $"{d.Value:F2}");

        Assert.Equal("1234.55", result);
    }

    [Fact]
    public void DisplayFieldCodedDouble_Available_NullValue_Returns_NotAvailable()
    {
        var value = new CodedDouble(null, string.Empty, string.Empty);
        var field = value.ToDisplayField();

        Assert.NotNull(field);
        Assert.True(field.IsAvailable);
        Assert.False(field.IsNotAvailable);

        var result = field.DisplayText();

        Assert.Equal("Not available", result);
    }

    [Fact]
    public void DisplayFieldCodedDouble_Available_NullValue_DisplayReasonTrue_Returns_Reason()
    {
        var value = new CodedDouble(null, "Redacted for confidentiality", "c");
        var field = value.ToDisplayField();

        Assert.NotNull(field);
        Assert.True(field.IsAvailable);
        Assert.False(field.IsNotAvailable);

        var result = field.DisplayText(d => $"{d.Value:F2}", displayReason: true);

        Assert.Equal("Redacted for confidentiality", result);
    }

    [Fact]
    public void DisplayFieldCodedString_Available_ReturnsText()
    {
        var value = new CodedString("School", string.Empty, string.Empty);
        var field = value.ToDisplayField();

        Assert.NotNull(field);
        Assert.True(field.IsAvailable);
        Assert.False(field.IsNotAvailable);

        var result = field.DisplayText();

        Assert.Equal("School", result);
    }

    [Fact]
    public void DisplayFieldCodedString_Available_NullValue_Returns_NotAvailable()
    {
        var value = new CodedString(null, string.Empty, string.Empty);
        var field = value.ToDisplayField();

        Assert.NotNull(field);
        Assert.True(field.IsAvailable);
        Assert.False(field.IsNotAvailable);

        var result = field.DisplayText();

        Assert.Equal("Not available", result);
    }

    [Fact]
    public void DisplayFieldCodedString_Available_NullValue_DisplayReasonFalse_Returns_DefaultText()
    {
        var value = new CodedString(null, "Not applicable", "z");
        var field = value.ToDisplayField();

        Assert.NotNull(field);
        Assert.True(field.IsAvailable);
        Assert.False(field.IsNotAvailable);

        var result = field.DisplayText(displayReason: false);

        Assert.Equal("Not available", result);
    }

    [Fact]
    public void DisplayFieldCodedString_Available_NullValue_DisplayReasonTrue_Returns_Reason()
    {
        var value = new CodedString(null, "Redacted for confidentiality", "c");
        var field = value.ToDisplayField();

        Assert.NotNull(field);
        Assert.True(field.IsAvailable);
        Assert.False(field.IsNotAvailable);

        var result = field.DisplayText(d => $"{d.Value:F2}", displayReason: true);

        Assert.Equal("Redacted for confidentiality", result);
    }
}
