using System.Reflection;

namespace Zimaoshan.Xin.Cache.Foundation;

/// <summary>
/// 缓存选项
/// </summary>
public class CacheOptions
{
    /// <summary>
    /// 主题名称
    /// </summary>
    public string TopicName { get; set; } = Assembly.GetEntryAssembly()!.GetFriendlyAssemblyName();
}
