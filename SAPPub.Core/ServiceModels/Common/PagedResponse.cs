namespace SAPPub.Core.ServiceModels.Common;

public class PagedResponse<T>
{
    public int TotalRecords { get; set; }
    public required IList<T> Records { get; set; }
    public required Pager PagerInfo { get; set; }
}
