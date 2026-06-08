using SAPPub.Core.Enums;

namespace SAPPub.Core.Extensions;

public static class EstablishmentStatusExtensions
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

    public static int? ToStatusCode(this EstablishmentStatus status)
    {
        return ((EstablishmentStatus?)status).ToStatusCode();
    }
}