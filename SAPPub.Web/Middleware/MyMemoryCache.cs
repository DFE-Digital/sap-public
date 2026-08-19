using Microsoft.Extensions.Caching.Memory;

namespace SAPPub.Web.Middleware
{
    public class MyMemoryCache
    {
        public MemoryCache Cache { get; } = new MemoryCache(
            new MemoryCacheOptions
            {
            });
    }
}
