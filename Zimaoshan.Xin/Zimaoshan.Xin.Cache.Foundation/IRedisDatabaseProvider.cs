using StackExchange.Redis;

namespace Zimaoshan.Xin.Cache.Foundation;

public interface IRedisDatabaseProvider
{
    /// <summary>
    /// 获取数据存储对象
    /// </summary>
    /// <returns>功能集</returns>
    IDatabase GetDatabase();

    /// <summary>
    /// 获取订阅者
    /// </summary>
    /// <returns>订阅者</returns>
    ISubscriber GetSubscriber();
}
