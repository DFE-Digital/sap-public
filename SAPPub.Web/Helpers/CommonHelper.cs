namespace SAPPub.Web.Helpers;
using SAPPub.Web.Constants;

public static class CommonHelper
{
    public static string GetLocalAuthorityDisplayName(string? laName) =>
        string.IsNullOrWhiteSpace(laName) || laName.Length > 19
        ? Constants.LocalCouncilAverage
        : $"{laName} {Constants.Average}";

    public static double? AddNullable(double? a, double? b)
    {
        return a.HasValue && b.HasValue ? a + b : a ?? b;
    }
}
