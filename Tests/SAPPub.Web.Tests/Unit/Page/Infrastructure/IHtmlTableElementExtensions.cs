using AngleSharp.Html.Dom;

namespace SAPPub.Web.Tests.Unit.Page.Infrastructure;

public static class IHtmlTableElementExtensions
{
    public static string? GetTableValueByRowHeader(
        this IHtmlTableElement table,
        string rowName)
    {
        var row = table.Rows
            .FirstOrDefault(r =>
                string.Equals(
                    r.Cells[0].TextContent.Trim(),
                    rowName,
                    StringComparison.OrdinalIgnoreCase));

        return row?.Cells.Length > 1
            ? row.Cells[1].TextContent.Trim()
            : null;
    }
}
