using System;

namespace CNBlogs.Interface
{
    public interface ICachingRepository
    {
        object Get(string cacheKey);

        void Set(string cacheKey, object cacheValue, TimeSpan absoluteExpirationRelativeToNow);
    }
}
