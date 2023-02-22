using Microsoft.Extensions.DependencyInjection;

namespace Zimaoshan.Xin.Cache.Foundation.Annotations;

/// <summary>
/// 用于扫描
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ComponentAttribute : Attribute
{
    #region Fields

    /// <summary>
    /// 注册关键词
    /// </summary>
    public object? Key { get; set; }

    /// <summary>
    /// 服务类型
    /// </summary>
    public Type? Service { get; set; }

    /// <summary>
    /// 生命周期
    /// </summary>
    public ServiceLifetime LifetimeScope { get; } = ServiceLifetime.Transient;

    #endregion

    #region Constructor

    /// <summary>
    /// 初始化
    /// </summary>
    public ComponentAttribute() { }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="lifetimeScope">服务的生命作用域</param>
    public ComponentAttribute(ServiceLifetime lifetimeScope)
    {
        LifetimeScope = lifetimeScope;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="service"></param>
    public ComponentAttribute(Type service)
    {
        Service = service;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="service">对象类型</param>
    /// <param name="lifetimeScope">作用域</param>
    public ComponentAttribute(Type service, ServiceLifetime lifetimeScope) : this(lifetimeScope)
    {
        Service = service;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="key">关键服务Key</param>
    /// <param name="service">服务</param>
    /// <param name="lifetime">作用域</param>
    public ComponentAttribute(object key, Type service, ServiceLifetime lifetimeScope = ServiceLifetime.Transient) : this(service, lifetimeScope)
    {
        Key = key ?? throw new ArgumentNullException(nameof(key));
        Service = service;
    }

    #endregion
}