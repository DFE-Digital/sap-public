namespace SAPPub.Web.Models.Common;

public class PagerViewModel
{
    private const int DefaultWindowSize = 1;
    private const int FirstMiddlePage = 2;

    public int TotalItems { get; init; }
    public int CurrentPage { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }

    public int WindowSize { get; init; } = DefaultWindowSize;
    public bool ShowPrevious => CurrentPage > 1;
    public bool ShowNext => CurrentPage < TotalPages;

    public IReadOnlyList<PaginationItem> Pages => BuildPages();

    private IReadOnlyList<PaginationItem> BuildPages()
    {
        if (TotalPages <= 1)
        {
            return [];
        }

        var items = new List<PaginationItem>();

        int start = Math.Max(FirstMiddlePage, CurrentPage - WindowSize);
        int end = Math.Min(TotalPages - 1, CurrentPage + WindowSize);

        // First page
        items.Add(PaginationItem.Page(1, CurrentPage == 1));

        // Left ellipsis
        if (start > FirstMiddlePage)
        {
            items.Add(PaginationItem.Ellipsis());
        }

        // Middle pages
        for (int i = start; i <= end; i++)
        {
            items.Add(PaginationItem.Page(i, i == CurrentPage));
        }

        // Right ellipsis
        if (end < TotalPages -1)
        {
            items.Add(PaginationItem.Ellipsis());
        }

        // Last page
        if (TotalPages > 1)
        {
            items.Add(PaginationItem.Page(TotalPages, CurrentPage == TotalPages));
        }

        return items;
    }
}
