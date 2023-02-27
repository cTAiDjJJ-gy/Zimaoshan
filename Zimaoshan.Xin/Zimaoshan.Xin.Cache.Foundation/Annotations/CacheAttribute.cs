namespace Zimaoshan.Xin.Cache.Foundation.Annotations;

/// <summary>
/// 缓存代理
/// 默认1分钟
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class CacheAttribute : Attribute
{
    #region Fields

    /// <summary>
    /// 过期时间
    /// </summary>
    public TimeSpan Expiration { get; set; }

    /// <summary>
    /// 缓存Key
    /// </summary>
    public string CacheKey { get; set; }

    #endregion

    #region Constructor

    public CacheAttribute(string cacheKey, int expiration = 60)
    {
        this.CacheKey= cacheKey;
        this.Expiration = TimeSpan.FromSeconds(expiration);
    }

    #endregion
}
