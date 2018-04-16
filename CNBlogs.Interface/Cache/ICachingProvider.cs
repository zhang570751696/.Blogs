using System;

namespace CNBlogs.Interface
{
    public interface ICachingProvider
    {
        object Get(string cacheKey);

        void Set(string cacheKey, object cacheValue, TimeSpan absoluteExpirationRelativeToNow);
    }
}
