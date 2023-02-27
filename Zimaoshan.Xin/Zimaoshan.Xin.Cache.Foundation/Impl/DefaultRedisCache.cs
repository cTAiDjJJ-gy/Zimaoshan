using StackExchange.Redis;

namespace Zimaoshan.Xin.Cache.Foundation.Impl;

/// <summary>
/// redis缓存
/// https://www.thecodebuzz.com/redis-distributed-cache-asp-net-core-csharp-redis-examples/
/// https://www.thecodebuzz.com/redis-dependency-injection-connectionmultiplexer-redis-cache-netcore-csharp/
/// </summary>
public class DefaultRedisCache : IDistributedCache
{
    #region Fields

    private readonly IRedisDatabaseProvider _redisDatabaseProvider;

    #endregion

    #region Ctor

    public DefaultRedisCache(IRedisDatabaseProvider redisDatabaseProvider) => _redisDatabaseProvider = redisDatabaseProvider;

    #endregion

    #region Methods

    public T? Get<T>(string key)
    {
        var result = GetDatabase().StringGet(key);

        return !result.IsNullOrEmpty ? result.Deserialize<T>() : default;
    }

    public void Remove(string key)
    {
        GetDatabase().KeyDelete(key);
    }

    /// <summary>
    /// 设置值
    /// </summary>
    /// <typeparam name="T">泛型值</typeparam>
    /// <param name="key">key</param>
    /// <param name="obj">value</param>
    /// <param name="timeout">默认5分钟</param>
    public void Set<T>(string key, T obj, TimeSpan? timeout = null)
    {
        var cache = GetDatabase();
        cache.StringSet(key, obj.Serialize<T>(), timeout ?? TimeSpan.FromMinutes(5));
    }

    #endregion

    /// <summary>
    /// 获取数据库
    /// </summary>
    /// <returns></returns>
    private IDatabase GetDatabase() => _redisDatabaseProvider.GetDatabase();
}
