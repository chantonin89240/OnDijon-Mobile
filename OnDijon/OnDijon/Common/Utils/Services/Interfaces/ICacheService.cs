using System;
using System.Threading.Tasks;

namespace OnDijon.Common.Utils.Services.Interfaces
{
    public interface ICacheService
    {
        Task<T> Get<T>(string key, CacheType cacheType = CacheType.Default);

        Task Put<T>(string key, T value, CacheType cacheType = CacheType.Default);

        Task Put<T>(string key, T value, TimeSpan duration, CacheType cacheType = CacheType.Default);

        Task Delete<T>(string key, CacheType cacheType = CacheType.Default);
    }

    public enum CacheType
    {
        Default,
        User,
        Secure,
        InMemory
    }
}
