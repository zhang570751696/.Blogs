using System;

namespace CNBlogs.Common
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class QCachingAttribute : Attribute
    {
        public int AbsoluteExpiration { get; set; } = 30;
    }
}
