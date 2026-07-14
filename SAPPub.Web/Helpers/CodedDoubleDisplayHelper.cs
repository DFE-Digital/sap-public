using SAPPub.Core.ValueObjects;

namespace SAPPub.Web.Helpers;

public class CodedDoubleDisplayHelper
{
    public static string DisplayFormat(CodedDouble codedDouble)
    {
        if (!codedDouble.HasValue)
        {
            return "Not available"; // later, this can return different values depending on CodedDouble:Reason
        }
        else
        {
            return codedDouble.Value!.ToString();
        }
    }
}
