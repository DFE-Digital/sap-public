namespace SAPPub.Core.Extensions;

public static class DoubleExtensions
{
    public static string AsPercentage(this double value)
    {
        return $"{value}%";
    }
}
