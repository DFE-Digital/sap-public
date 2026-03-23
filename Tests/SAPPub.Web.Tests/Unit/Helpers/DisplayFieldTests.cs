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

}
