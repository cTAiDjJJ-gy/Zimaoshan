using Microsoft.Extensions.Caching.Memory;

namespace Zimaoshan.Xin.Cache.Foundation.Impl;

/// <summary>
/// 本地缓存
/// https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-6.0
/// </summary>
public class DefaultLocalCache : ILocalCache
{
    #region Field

    private readonly IMemoryCache _cache;

    #endregion

    #region Ctor

    public DefaultLocalCache(IMemoryCache cache) => _cache = cache;

    #endregion

    #region Methods

    public T? Get<T>(string key)
    {
        var obj = _cache.Get(key);

        if (obj is not null)
        {
            return (T)obj;
        }

        return default;
    }

    public void Remove(string key) => _cache.Remove(key);

    public void Set<T>(string key, T obj) => _cache.Set(key, obj, DateTimeOffset.Now.Add(TimeSpan.FromMinutes(5)));

    #endregion
}
