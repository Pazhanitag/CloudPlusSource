using System.Runtime.Caching;

namespace CloudPlus.Infrastructure.Cache
{
    public class CacheStoreResolver : ICacheStoreResolver
    {
        public ObjectCache ResolveCache()
        {
            return MemoryCache.Default;
        }
    }
}