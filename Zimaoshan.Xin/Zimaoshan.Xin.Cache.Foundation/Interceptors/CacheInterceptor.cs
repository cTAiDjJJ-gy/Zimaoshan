using Castle.DynamicProxy;
using System.Reflection;
using Zimaoshan.Xin.Cache.Foundation.Annotations;

namespace Zimaoshan.Xin.Cache.Foundation.Interceptors;

public class CacheInterceptor : IInterceptor
{
    #region Fields

    private readonly ICache _cache;

    #endregion

    #region Constructor

    public CacheInterceptor(ICache cache)
    {
        _cache = cache;
    }

    #endregion

    #region Methods

    public void Intercept(IInvocation invocation)
    {
        var cacheAttribute = invocation.Method.GetCustomAttribute<CacheAttribute>();

        if (cacheAttribute != null)
        {
            var key = cacheAttribute.CacheKey;
            var parameters = invocation.Method.GetParameters();
            for (var i = 0; i < parameters.Length; i++)
            {
                var prefix = GetPrefixParameter(parameters[i].Name!);
                if (key.IndexOf(prefix) != -1)
                {
                    var value = parameters[i].ParameterType.Name switch
                    {
                        nameof(String) or nameof(Int32) => invocation.Arguments[i].ToString(),
                        _ => DateTime.UtcNow.ToString("yyyyMMdd_HHmmss")
                    };
                    key = key.Replace($"{prefix}", value);
                }
            }

            var cacheMethod = _cache.GetType().GetMethod(nameof(ICache.Get), BindingFlags.Public | BindingFlags.Instance);

            var cacheMethodFunc = cacheMethod?.MakeGenericMethod(invocation.Method.ReturnType);
            var result = cacheMethodFunc?.Invoke(_cache, new object[] { key });

            if (result != null)
            {
                invocation.ReturnValue = result;
                return;
            }

            invocation.Proceed();

            var expiration = cacheAttribute.Expiration;
            _cache.Set(key, invocation.ReturnValue, expiration);
            return;
        }

        invocation.Proceed();
    }

    private string GetPrefixParameter(string name) => $"@{name}";

    #endregion
}
