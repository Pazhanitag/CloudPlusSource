using System;
using System.Threading.Tasks;

namespace CloudPlus.Infrastructure.Cache
{
    public interface ICacheStore
    {
        T AddOrGet<T>(string key, int expirationMinutes, Func<T> repository);
        Task<T> AddOrGetAsync<T>(string key, int expirationMinutes, Func<Task<T>> repository);
        Task<T> AddOrGetAsync<T>(string key, int expirationMinutes, Func<T> repository);
        void Remove(string key);
    }
}