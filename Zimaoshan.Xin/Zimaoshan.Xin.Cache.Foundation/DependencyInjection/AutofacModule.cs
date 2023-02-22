using Autofac;
using Autofac.Builder;
using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Zimaoshan.Xin.Cache.Foundation.Annotations;
using Module = Autofac.Module;

namespace Zimaoshan.Xin.Cache.Foundation.DependencyInjection;

/// <summary>
/// Autofac注册默认模块
/// </summary>
public class AutofacModule : Module
{
    #region Fields

    private readonly Assembly[] _assemblies;

    #endregion

    #region Constructor

    public AutofacModule(params Assembly[] assemblies)
    {
        _assemblies = assemblies;
    }

    #endregion

    protected override void Load(ContainerBuilder builder)
    {
        foreach (var assembly in _assemblies)
        {
            var components = GetAllComponents(assembly);

            foreach (var component in components)
            {
                RegisterDependenciesByAssembly(builder, component);
            }
        }
    }

    /// <summary>
    /// 注册Component
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="component"></param>
    private void RegisterDependenciesByAssembly(ContainerBuilder builder, ComponentModel component)
    {
        var registerInterfaces = component.ServiceType?.FindDependencyInterfaces();
        if (registerInterfaces == null || !registerInterfaces.Any()) return;

        IRegistrationBuilder<object, ReflectionActivatorData, object> next = default!;

        if (component.Mode == LocationMode.Interface)
        {
            foreach(var registerInterface in registerInterfaces)
            {
                next = builder
                    .RegisterType(component.ServiceType!)
                    .As(registerInterface);
            }
        }

        if (component.Mode == LocationMode.Attribute)
        {
            var componentAttribute = component.ServiceType?.GetCustomAttribute<ComponentAttribute>();
            foreach (var registerInterface in registerInterfaces)
            {
                var serviceType = componentAttribute!.Service != null ? componentAttribute.Service : registerInterface;
                next = componentAttribute!.Key != null
                    ? builder.RegisterType(component.ServiceType!).As(serviceType).Keyed(componentAttribute.Key, serviceType)
                    : builder.RegisterType(component.ServiceType!).As(serviceType);
            }
                
        }

        if (next == null) return;

        switch (component.LifeScope)
        {
            case ServiceLifetime.Singleton:
                next.SingleInstance();
                break;
            case ServiceLifetime.Scoped:
                next.InstancePerLifetimeScope();
                break;
            case ServiceLifetime.Transient:
                next.InstancePerDependency();
                break;
        }
    }

    /// <summary>
    /// 获取所有Component
    /// </summary>
    /// <param name="assembly"></param>
    /// <returns></returns>
    private IEnumerable<ComponentModel> GetAllComponents(Assembly assembly)
    {
        // 获取以下Type：
        // 1.获取标注ComponentAttribute类
        // 2.IDependency接口的实现类
        var entities = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface && !t.IsSealed && (typeof(IDependency).IsAssignableFrom(t) || t.GetCustomAttribute<ComponentAttribute>() != null))
            .Distinct()
            .ToList();

        foreach (var entity in entities)
        {
            var lifetime = entity.FindServiceDependencyLifetimeOrNull();
            if (lifetime.HasValue)
            {
                yield return new ComponentModel
                {
                    ServiceType = entity,
                    LifeScope = lifetime.Value,
                    Mode = LocationMode.Interface
                };
            }

            var componentAttribute = entity.GetCustomAttribute<ComponentAttribute>();
            if (componentAttribute != null)
            {
                yield return new ComponentModel
                {
                    ServiceType = entity,
                    LifeScope = componentAttribute.LifetimeScope,
                    Mode = LocationMode.Attribute
                };
            }
        }
    }
}