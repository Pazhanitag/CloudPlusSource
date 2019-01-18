using System;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace CloudPlus.Infrastructure.Cache
{
    public class InMemoryCacheStore : ICacheStore
    {
        private readonly ObjectCache _cache;

        public InMemoryCacheStore(ICacheStoreResolver cacheStoreResolver)
        {
            _cache = cacheStoreResolver.ResolveCache();
        }

        public T AddOrGet<T>(string key, int expirationMinutes, Func<T> repository)
        {
            var newValue = new Lazy<T>(repository);

            var oldValue = _cache.AddOrGetExisting(key, newValue, new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.UtcNow.AddMinutes(expirationMinutes)
            }) as Lazy<T>;

            try
            {
                return (oldValue ?? newValue).Value;
            }
            catch (Exception)
            {
                _cache.Remove(key);

                throw;
            }
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public async Task<T> AddOrGetAsync<T>(string key, int expirationMinutes, Func<Task<T>> repository)
        {
            var newValue = new Lazy<Task<T>>(() =>
                Task.Factory.StartNew(repository).Unwrap()
            );

            var oldValue = _cache.AddOrGetExisting(key, newValue, new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.UtcNow.AddMinutes(expirationMinutes)
            }) as Lazy<T>;

            try
            {
                return oldValue != null ? oldValue.Value : await newValue.Value;
            }
            catch (Exception)
            {
                _cache.Remove(key);

                throw;
            }
        }

        public async Task<T> AddOrGetAsync<T>(string key, int expirationMinutes, Func<T> repository)
        {
            var newValue = new Lazy<Task<T>>(() =>
                Task.Factory.StartNew(repository)
            );

            var oldValue = _cache.AddOrGetExisting(key, newValue, new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.UtcNow.AddMinutes(expirationMinutes)
            }) as Lazy<T>;

            try
            {
                return oldValue != null ? oldValue.Value : await newValue.Value;
            }
            catch (Exception)
            {
                _cache.Remove(key);

                throw;
            }
        }
    }
}