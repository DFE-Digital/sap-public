using SAPPub.Core.Interfaces.Services;

namespace SAPPub.Core.Services;

public class TimeService : ITimeService
{
    public DateTimeOffset GetUKTime()
    {
        return DateTimeOffset.UtcNow;       // utc or bst ¯\_(ツ)_/¯ ?
    }
}