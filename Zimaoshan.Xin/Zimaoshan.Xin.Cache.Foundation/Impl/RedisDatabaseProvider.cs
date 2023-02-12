using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Zimaoshan.Xin.Cache.Foundation.Impl;

public class RedisDatabaseProvider : IRedisDatabaseProvider
{
    #region Fields

    /// <summary>
    /// The connection multiplexer.
    /// </summary>
    private readonly Lazy<ConnectionMultiplexer> _connectionMultiplexer;

    #endregion

    #region Constructor

    public RedisDatabaseProvider(IConfiguration configuration)
    {
        var connection = configuration.GetConnectionString("Redis");
        _connectionMultiplexer = new Lazy<ConnectionMultiplexer>(CreateConnectionMultiplexer(connection!));
    }

    #endregion

    #region Methods

    public IDatabase GetDatabase() => _connectionMultiplexer.Value.GetDatabase();

    public ISubscriber GetSubscriber() => _connectionMultiplexer.Value.GetSubscriber();

    #endregion

    #region Utilities

    /// <summary>
    /// 链接Redis
    /// </summary>
    /// <param name="connection"></param>
    /// <returns></returns>
    private ConnectionMultiplexer CreateConnectionMultiplexer(string connection)
    {
        return ConnectionMultiplexer.Connect(connection);
    }

    #endregion
}
