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
    private readonly List<string> _keys = new();

    #endregion

    #region Constructor

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

    public IEnumerable<string> GetAllKey() => _keys;

    public void Remove(string key)
    {
        _cache.Remove(key);
        _keys.Remove(key);
    }

    public void Set<T>(string key, T obj, TimeSpan? timeout = null)
    {
        _cache.Set(key, obj, DateTimeOffset.Now.Add(timeout ?? TimeSpan.FromMinutes(5)));
        if (!_keys.Any(k => k == key))
            _keys.Add(key);
    }

    #endregion
}
