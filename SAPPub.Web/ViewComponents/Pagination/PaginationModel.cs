using SAPPub.Web.Models.Common;

namespace SAPPub.Web.ViewComponents.Pagination;

public class PaginationModel
{
    public required PagerViewModel PagerInfo { get; set; }

    public string? RouteName { get; set; }

    public Dictionary<string, string?> RouteAttributes { get; set; } = [];
}
