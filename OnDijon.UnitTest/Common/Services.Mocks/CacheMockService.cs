using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using OnDijon.Common.Utils.Services.Interfaces;

namespace OnDijon.UnitTest.Common.Services.Mocks
{
    class CacheMockService : ICacheService
    {
        public async Task<T> Get<T>(string key, CacheType cacheType = CacheType.Default)
        {
            return await BlobCache.InMemory.GetOrCreateObject<T>(key, () => default);
        }

        public async Task Put<T>(string key, T value, CacheType cacheType = CacheType.Default)
        {
            await BlobCache.InMemory.InsertObject(key, value);
        }

        public async Task Put<T>(string key, T value, TimeSpan duration, CacheType cacheType = CacheType.Default)
        {
            await BlobCache.InMemory.InsertObject(key, value, DateTime.Now.Add(duration));
        }


        public async Task Delete<T>(string key, CacheType cacheType = CacheType.Default)
        {
            await BlobCache.InMemory.InvalidateObject<T>(key);
        }
    }
}
