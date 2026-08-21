namespace SAPPub.Web.ViewComponents.SchoolProfilePagination;

public class SchoolProfilePaginationViewModel
{
    public required SchoolProfilePaginationResult Result { get; set; }

    public required IDictionary<string, string> RouteAttributes { get; set; }
}
