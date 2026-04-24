namespace SAPPub.Web.Models.Common;

public class PaginationItem
{
    public int? PageNumber { get; set; }

    public bool IsCurrent { get; set; }

    public bool IsEllipsis => !PageNumber.HasValue;

    private PaginationItem(int? pageNumber, bool isCurrent)
    {
        PageNumber = pageNumber;
        IsCurrent = isCurrent;
    }

    public static PaginationItem Page(int pageNumber, bool isCurrent)
        => new(pageNumber, isCurrent);

    public static PaginationItem Ellipsis()
        => new(null, false);
}
