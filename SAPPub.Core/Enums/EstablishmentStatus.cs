namespace SAPPub.Core.Enums;

public enum EstablishmentStatus
{
    Open = 1,
    Closed = 2
}

public static class StatusExtensions
{
    public static EstablishmentStatus? ToStatus(this int? statusCode)
    {
        return statusCode switch
        {
            1 => EstablishmentStatus.Open,
            2 => EstablishmentStatus.Closed,
            _ => null
        };
    }

    public static int? ToStatusCode(this EstablishmentStatus? status)
    {
        return status switch
        {
            EstablishmentStatus.Open => 1,
            EstablishmentStatus.Closed => 2,
            _ => null
        };
    }
}
