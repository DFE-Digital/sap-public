namespace SAPPub.Web.Tests.Unit.Page.Infrastructure;

public static class DocumentExtensions
{
    public static string? GetRowContentByIdAndKey(this AngleSharp.Dom.IDocument doc, string summaryListId, string key)
    {
        // get the summary list by id
        var summaryList = doc.QuerySelector($"#{summaryListId}");

        // within that summary list, find the row with the specified key in the dt element and return the content of the dd element in that row
        var rows = summaryList?.QuerySelectorAll(".govuk-summary-list__row");
        var nameRow = rows?.SingleOrDefault(r =>
                    r.QuerySelector("dt")?.TextContent.Trim() == key);
        return nameRow?.QuerySelector("dd")?.TextContent.Trim();
    }

    public static string? GetTableHeaderContentByIdAndIndex(this AngleSharp.Dom.IDocument doc, string tableId, int rowIndex, int cellIndex)
    {
        // get the summary list by id
        var table = doc.QuerySelector($"#{tableId}");

        var rows = table?.QuerySelectorAll(".govuk-table__row");

        var headerCells = rows?[rowIndex].QuerySelectorAll(".govuk-table__header");

        return headerCells?[cellIndex]?.TextContent.Trim();
    }

    public static string? GetTableCellContentByIdAndIndex(this AngleSharp.Dom.IDocument doc, string tableId, int rowIndex, int cellIndex)
    {
        // get the summary list by id
        var table = doc.QuerySelector($"#{tableId}");

        var rows = table?.QuerySelectorAll(".govuk-table__row");

        var cells = rows?[rowIndex].QuerySelectorAll(".govuk-table__cell");

        return cells?[cellIndex]?.TextContent.Trim();
    }

    public static string? GetTableCellContentByRowAndCellIndex(this AngleSharp.Dom.IElement ele, int rowIndex, int cellIndex)
    {
        var rows = ele?.QuerySelectorAll(".govuk-table__row");

        var cells = rows?[rowIndex].QuerySelectorAll(".govuk-table__cell");

        return cells?[cellIndex]?.TextContent.Trim();
    }
}
