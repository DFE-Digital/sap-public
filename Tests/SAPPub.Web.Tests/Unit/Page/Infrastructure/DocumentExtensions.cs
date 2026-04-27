namespace SAPPub.Web.Tests.Unit.Page.Infrastructure;

public static class DocumentExtensions
{
    public static string GetRowContentByIdAndKey(this AngleSharp.Dom.IDocument doc, string summaryListId, string key)
    {
        // get the summary list by id
        var summaryList = doc.QuerySelector($"#{summaryListId}");

        // within that summary list, find the row with the specified key in the dt element and return the content of the dd element in that row
        var rows = summaryList?.QuerySelectorAll(".govuk-summary-list__row");
        var nameRow = rows?.FirstOrDefault(r =>
                    r.QuerySelector("dt")?.TextContent.Trim() == key);
        return nameRow?.QuerySelector("dd")?.TextContent.Trim() ?? string.Empty;
    }
}
