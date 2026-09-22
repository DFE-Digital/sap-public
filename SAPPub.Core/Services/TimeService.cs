using SAPPub.Core.Interfaces.Services;

namespace SAPPub.Core.Services;

public class TimeService : ITimeService
{
    public DateTimeOffset GetUTCTime()
    {
        return DateTimeOffset.UtcNow;
    }
}