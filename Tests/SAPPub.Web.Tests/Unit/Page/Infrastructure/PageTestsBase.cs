using SAPPub.Core.Helpers;

namespace SAPPub.Web.Tests.Unit.Page.Infrastructure;

public class PageTestsBase
{
    protected string BuildUrl(string urn, string schoolName, string pageRoute)
    {
        var encodedSchoolName = TextHelpers.CleanForUrl(schoolName);
        return $"/school/{urn}/{encodedSchoolName}{pageRoute}";
    }
}
