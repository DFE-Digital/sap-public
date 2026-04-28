namespace SAPPub.Core.ServiceModels.Common;

public class Pager
{
    public Pager(int totalItems, int? currentPage, int pageSize)
    {
        var totalPages = (int)Math.Ceiling(totalItems / (decimal)pageSize);
        var currentPageNumber = currentPage != null && currentPage > 0 ? (int)currentPage : 1;

        if(totalPages > 0 && currentPageNumber > totalPages)
        {
            currentPageNumber = totalPages;
        }

        TotalItems = totalItems;
        CurrentPage = currentPageNumber;
        PageSize = pageSize;
        TotalPages = totalPages;
    }


    public int TotalItems { get; set; }

    public int CurrentPage { get; set; }

    public int PageSize { get; set; }

    public int TotalPages { get; set; }
}
