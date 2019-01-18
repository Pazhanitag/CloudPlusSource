using System.Runtime.Caching;

namespace CloudPlus.Infrastructure.Cache
{
    public interface ICacheStoreResolver
    {
        ObjectCache ResolveCache();
    }
}