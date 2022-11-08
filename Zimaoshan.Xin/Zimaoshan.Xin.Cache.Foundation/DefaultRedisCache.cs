using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Text.Json;

namespace Zimaoshan.Xin.Cache.Foundation;

public class DefaultRedisCache : ICache
{
    #region Fields

    private readonly Lazy<ConnectionMultiplexer> _connectionMultiplexer;

    #endregion

    #region Ctor

    public DefaultRedisCache(IConfiguration configuration)
    {
        var connection = configuration.GetConnectionString("Redis");
        _connectionMultiplexer = new Lazy<ConnectionMultiplexer>(CreateConnectionMultiplexer(connection));
    }

    #endregion

    #region Methods

    public T? Get<T>(string key)
    {
        var cache = GetDatabase();
        var result = cache.StringGet(key);

        return !result.IsNullOrEmpty ? Deserialize<T>(result) : default(T);
    }

    public void Remove(string key)
    {
        var cache = GetDatabase();
        cache.KeyDelete(key);
    }

    public void Set<T>(string key, T obj)
    {
        var cache = GetDatabase();
        cache.StringSet(key, Serialize<T>(obj), TimeSpan.FromMinutes(5));
    }

    #endregion

    /// <summary>
    /// 获取数据库
    /// </summary>
    /// <returns></returns>
    private IDatabase GetDatabase()
    {
        return _connectionMultiplexer.Value.GetDatabase();
    }

    /// <summary>
    /// 链接Redis
    /// </summary>
    /// <param name="connection"></param>
    /// <returns></returns>
    private ConnectionMultiplexer CreateConnectionMultiplexer(string connection)
    {
        return ConnectionMultiplexer.Connect(connection);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="bytes"></param>
    /// <returns></returns>
    private T? Deserialize<T>(byte[]? bytes)
    {
        if (bytes is null) return default(T);

        var span = new ReadOnlySpan<byte>(bytes, 0, bytes.Length);
        var value = JsonSerializer.Deserialize<T>(span);
        return value;
    }

    /// <summary>
    /// 
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
