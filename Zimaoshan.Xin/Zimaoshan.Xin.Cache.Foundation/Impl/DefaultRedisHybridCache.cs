namespace Zimaoshan.Xin.Cache.Foundation.Impl;

/// <summary>
/// 
/// </summary>
public class DefaultRedisHybridCache : ICache
{
    #region Fields

    private readonly ILocalCache _localCache;
    private readonly IDistributedCache _distributedCache;

    #endregion

    #region Ctors

    public DefaultRedisHybridCache(ILocalCache localCache, IDistributedCache distributedCache)
    {
        _localCache = localCache;
        _distributedCache = distributedCache;
    }

    #endregion

    #region Methods

    public T? Get<T>(string key)
    {
        var result = _localCache.Get<T>(key);
        Console.WriteLine($"Get Local Cache Key:{key}");

        if (result != null)
        {
            return result;
        }

        Console.WriteLine($"Get DistributedCache. Cache Key:{key}");
        result = _distributedCache.Get<T>(key);
        if (result != null)
        {
            _localCache.Set(key, result);
        }

        return result;
    }

    public void Remove(string key)
    {
        _localCache.Remove(key);
        _distributedCache.Remove(key);
    }

    public void Set<T>(string key, T obj)
    {
        _localCache.Set(key, obj);
        _distributedCache.Set(key, obj);
    }

    #endregion
}
