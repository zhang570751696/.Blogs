using CNBlogs.Common;
using CNBlogs.Interface;
using System;

namespace CNBlogs.BLL
{
    public class DateTimeBLL : IQCaching
    {
        [QCaching(AbsoluteExpiration = 10)]
        public virtual string GetCurrentUtcTime()
        {
            return DateTime.UtcNow.ToString();
        }
    }
}
