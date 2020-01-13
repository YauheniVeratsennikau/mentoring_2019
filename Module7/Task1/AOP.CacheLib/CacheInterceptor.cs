using System;
using System.Collections.Generic;
using Castle.DynamicProxy;

namespace AOP.CacheLib
{
    public class CacheInterceptor : IInterceptor
    {
        private readonly Dictionary<string, object> _cache;

        public CacheInterceptor()
        {
            _cache = new Dictionary<string, object>();
        }

        public void Intercept(IInvocation invocation)
        {
            var key = GetCacheKey(invocation.Arguments);
            
            if (_cache.TryGetValue(key, out var value))
            {
                 invocation.ReturnValue = value;
            }
            else
            {
                invocation.Proceed();
                value = invocation.ReturnValue;

                _cache.Add(key, value);
            }
        }

        string GetCacheKey(object[] arguments)
        {
            return string.Join(";", arguments);
        }
    }
}
