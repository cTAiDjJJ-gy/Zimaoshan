namespace Zimaoshan.Xin.Cache.Foundation;

/// <summary>
/// 缓存事件总线
/// </summary>
public interface ICacheBus
{
    /// <summary>
    /// 发布消息
    /// </summary>
    /// <param name="topic">主题</param>
    /// <param name="message">缓存信息</param>
    void Publish(string topic, CacheMessage message);

    /// <summary>
    /// 订阅主题和操作
    /// </summary>
    /// <param name="topic">主题</param>
    /// <param name="action">操作</param>
    void Subscribe(string topic, Action<CacheMessage> action);
}
