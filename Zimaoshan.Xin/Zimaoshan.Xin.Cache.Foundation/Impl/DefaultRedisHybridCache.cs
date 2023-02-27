using Microsoft.Extensions.Options;

namespace Zimaoshan.Xin.Cache.Foundation.Impl;

/// <summary>
/// 混合模式
/// </summary>
public class DefaultRedisHybridCache : ICache
{
    #region Fields

    private readonly ILocalCache _localCache;
    private readonly IDistributedCache _distributedCache;

    private readonly ICacheBus _bus;
    private readonly string _cacheId;
    private readonly string _topicName;

    #endregion

    #region Ctors

    public DefaultRedisHybridCache(
        ILocalCache localCache,
        IDistributedCache distributedCache,
        ICacheBus bus,
        IOptions<CacheOptions> optionsAccessor)
    {
        _localCache = localCache;
        _distributedCache = distributedCache;
        _bus = bus;

        _cacheId = Guid.NewGuid().ToString();
        _topicName = optionsAccessor.Value.TopicName;

        this.InitSubscribe();
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

        _bus.Publish(_topicName, new() { Id = _cacheId, CacheKey = key });
    }

    public void Set<T>(string key, T obj, TimeSpan? timeout = null)
    {
        _localCache.Set(key, obj, timeout);

        _distributedCache.Set(key, obj, timeout);

        _bus.Publish(_topicName, new() { Id = _cacheId, CacheKey = key });
    }

    #endregion

    #region Extensions

    /// <summary>
    /// 初始化订阅者
    /// </summary>
    private void InitSubscribe()
    {
        _bus.Subscribe(_topicName, OnMessage);
    }

    /// <summary>
    /// 消息处理
    /// </summary>
    /// <param name="message"></param>
    private void OnMessage(CacheMessage message)
    {
        // 忽略发布实例消息处理
        if (!string.IsNullOrEmpty(message.Id) && message.Id.Equals(_cacheId,StringComparison.OrdinalIgnoreCase)) { return; }

        _localCache.Remove(message.CacheKey!);
    }

    #endregion
}
