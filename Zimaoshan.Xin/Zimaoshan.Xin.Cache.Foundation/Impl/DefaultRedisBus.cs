using StackExchange.Redis;

namespace Zimaoshan.Xin.Cache.Foundation.Impl;

/// <summary>
/// 默认缓存事件总线
/// </summary>
public class DefaultRedisBus : ICacheBus
{
    #region Feilds

    /// <summary>
    /// Redis订阅者
    /// </summary>
    private readonly ISubscriber _subscriber;

    /// <summary>
    /// 订阅消息处理
    /// </summary>
    private Action<CacheMessage>? _handler;

    #endregion

    #region Constructor

    public DefaultRedisBus(IRedisDatabaseProvider provider)
    {
        _subscriber = provider.GetSubscriber();
    }

    #endregion

    #region Methods

    public void Publish(string topic, CacheMessage message)
    {
        _subscriber.Publish(topic, message.Serialize());
    }

    public void Subscribe(string topic, Action<CacheMessage> action)
    {
        _handler = action;
        _subscriber.Subscribe(topic, OnMessage);
    }

    /// <summary>
    /// 订阅处理
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="value"></param>
    private void OnMessage(RedisChannel channel, RedisValue value)
    {
        var message = value.Deserialize<CacheMessage>() ?? new();
        _handler?.Invoke(message);
    }

    #endregion
}
