namespace SAPPub.Web.Helpers;
using SAPPub.Web.Constants;

public static class CommonHelper
{
    public static string GetLocalAuthorityDisplayName(string? laName) =>
        string.IsNullOrWhiteSpace(laName) || laName.Length > 19
        ? Constants.LocalCouncilAverage
        : $"{laName} {Constants.Average}";
}
