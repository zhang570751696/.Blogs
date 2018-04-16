using CNBlogs.Common;
using CNBlogs.Interface;
using System;

namespace CNBlogs.Impl
{
    public class DateTimeService : IDateTimeService, IQCaching
    {
        [QCaching(AbsoluteExpiration = 10)]
        public string GetCurrentUtcTime()
        {
            return DateTime.UtcNow.ToString();
        }
    }
}
