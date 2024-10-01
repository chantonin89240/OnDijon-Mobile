using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using OnDijon.Common.Utils.Services.Interfaces;

namespace OnDijon.Common.Utils.Services
{
    public class CacheService : ICacheService
    {
        public async Task<T> Get<T>(string key, CacheType cacheType = CacheType.Default)
        {
            return await GetCache(cacheType).GetOrCreateObject<T>(key, () => default);
        }

        public async Task Put<T>(string key, T value, CacheType cacheType = CacheType.Default)
        {
            await GetCache(cacheType).InsertObject(key, value);
        }

        public async Task Put<T>(string key, T value, TimeSpan duration, CacheType cacheType = CacheType.Default)
        {
            await GetCache(cacheType).InsertObject(key, value, DateTime.Now.Add(duration));
        }

        public async Task Delete<T>(string key, CacheType cacheType = CacheType.Default)
        {
            await GetCache(cacheType).InvalidateObject<T>(key);
        }

        private static IBlobCache GetCache(CacheType type)
        {
            switch (type)
            {
                case CacheType.User:
                    return BlobCache.UserAccount;
                case CacheType.Secure:
                    return BlobCache.Secure;
                case CacheType.InMemory:
                    return BlobCache.InMemory;
                default:
                    return BlobCache.LocalMachine;
            }
        }
    }
}
