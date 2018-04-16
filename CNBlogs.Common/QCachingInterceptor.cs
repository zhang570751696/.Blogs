using Castle.DynamicProxy;
using CNBlogs.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CNBlogs.Common
{
    public class QCachingInterceptor : IInterceptor
    {
        private ICachingProvider _cacheProvider;
        private char _linkChar = ':';

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="cacheProvider"></param>
        public QCachingInterceptor(ICachingProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        /// <summary>
        /// 拦截
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            var qCachingAttribute = this.GetQCachingAttributeInfo(invocation.MethodInvocationTarget ?? invocation.Method);
            if (qCachingAttribute != null)
            {
                ProceedCaching(invocation, qCachingAttribute);
            }
            else
            {
                invocation.Proceed();
            }
        }

        /// <summary>
        /// 判断拦截的请求是否带了qcacheattribute
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        private QCachingAttribute GetQCachingAttributeInfo(MethodInfo method)
        {
            return method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(QCachingAttribute)) as QCachingAttribute;
        }
        
        /// <summary>
        /// 从cache中取出数据
        /// </summary>
        /// <param name="invocation"></param>
        /// <param name="attribute"></param>
        private void ProceedCaching(IInvocation invocation, QCachingAttribute attribute)
        {
            var cacheKey = GenerateCacheKey(invocation);

            var cacheValue = _cacheProvider.Get(cacheKey);
            if (cacheValue != null)
            {
                invocation.ReturnValue = cacheValue;
                return;
            }

            invocation.Proceed();

            if (!string.IsNullOrWhiteSpace(cacheKey))
            {
                _cacheProvider.Set(cacheKey, invocation.ReturnValue, TimeSpan.FromSeconds(attribute.AbsoluteExpiration));
            }
        }

        /// <summary>
        /// 取缓存
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        private string GenerateCacheKey(IInvocation invocation)
        {
            var typeName = invocation.TargetType.Name;
            var methodName = invocation.Method.Name;
            var methodArguments = this.FormatArgumentsToPartOfCacheKey(invocation.Arguments);

            return this.GenerateCacheKey(typeName, methodName, methodArguments);
        }

        /// <summary>
        /// 拼接缓存的键
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="methodName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private string GenerateCacheKey(string typeName, string methodName, IList<string> parameters)
        {
            var builder = new StringBuilder();

            builder.Append(typeName);
            builder.Append(_linkChar);

            builder.Append(methodName);
            builder.Append(_linkChar);

            foreach (var param in parameters)
            {
                builder.Append(param);
                builder.Append(_linkChar);
            }

            return builder.ToString().TrimEnd(_linkChar);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodArguments"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        private IList<string> FormatArgumentsToPartOfCacheKey(IList<object> methodArguments, int maxCount = 5)
        {
            return methodArguments.Select(this.GetArgumentValue).Take(maxCount).ToList();
        }

        /// <summary>
        /// 处理方法的参数，可根据情况自行调整
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private string GetArgumentValue(object arg)
        {
            if (arg is int || arg is long || arg is string)
                return arg.ToString();

            if (arg is DateTime)
                return ((DateTime)arg).ToString("yyyyMMddHHmmss");

            if (arg is IQCachable)
                return ((IQCachable)arg).CacheKey;

            return null;
        }
    }
}
