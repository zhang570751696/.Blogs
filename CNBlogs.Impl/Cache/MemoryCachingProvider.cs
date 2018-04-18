using CNBlogs.Interface;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace CNBlogs.Impl
{
    public class MemoryCachingRepository : ICachingRepository
    {

        private IMemoryCache _cache;

        public MemoryCachingRepository(IMemoryCache cache)
        {
            _cache = cache;
        }

        public object Get(string cacheKey)
        {
            return _cache.Get(cacheKey);
        }

        public void Set(string cacheKey, object cacheValue, TimeSpan absoluteExpirationRelativeToNow)
        {
            _cache.Set(cacheKey, cacheValue, absoluteExpirationRelativeToNow);
        }
    }
}
