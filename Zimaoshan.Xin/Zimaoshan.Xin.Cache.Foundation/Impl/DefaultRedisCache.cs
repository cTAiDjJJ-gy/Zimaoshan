using StackExchange.Redis;
using System.Text.Json;

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

        return !result.IsNullOrEmpty ? Deserialize<T>(result) : default;
    }

    public void Remove(string key)
    {
        GetDatabase().KeyDelete(key);
    }

    public void Set<T>(string key, T obj)
    {
        var cache = GetDatabase();
        cache.StringSet(key, Serialize(obj), TimeSpan.FromMinutes(5));
    }

    #endregion

    /// <summary>
    /// 获取数据库
    /// </summary>
    /// <returns></returns>
    private IDatabase GetDatabase() => _redisDatabaseProvider.GetDatabase();

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="bytes"></param>
    /// <returns></returns>
    private T? Deserialize<T>(byte[]? bytes)
    {
        if (bytes is null) return default;

        var span = new ReadOnlySpan<byte>(bytes, 0, bytes.Length);
        var value = JsonSerializer.Deserialize<T>(span);
        return value;
    }

    /// <summary>
    /// 序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    private byte[] Serialize<T>(T value)
    {
        using var ms = new MemoryStream();
        var utf8JsonWriter = new Utf8JsonWriter(ms);
        JsonSerializer.Serialize(utf8JsonWriter, value);

        return ms.ToArray();
    }
}
