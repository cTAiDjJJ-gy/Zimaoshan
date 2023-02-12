using Autofac;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Module = Autofac.Module;

namespace Zimaoshan.Xin.Cache.Foundation.DependencyInjection;

/// <summary>
/// Autofac注册默认模块
/// </summary>
public class AutofacModule : Module
{
    #region Fields

    public Assembly[] Assemblies;

    #endregion

    #region Constructor

    public AutofacModule(params Assembly[] assemblies)
    {
        Assemblies = assemblies;
    }

    #endregion

    protected override void Load(ContainerBuilder builder)
    {
        foreach (var assembly in Assemblies)
        {
            RegisterDependenciesByAssembly(builder, assembly);
        }
    }

    private void RegisterDependenciesByAssembly(ContainerBuilder builder, Assembly assembly)
    {
        var types = assembly.GetDependenciesTypes();

        foreach (var type in types)
        {
            var registerInterface = type.FindDependencyInterface();
            if (registerInterface == null) continue;

            var serviceLifetime = type.FindServiceDependencyLifetime();
            var rb = builder.RegisterType(type).As(registerInterface);

            switch (serviceLifetime)
            {
                case ServiceLifetime.Singleton:
                    rb.SingleInstance();
                    break;
                case ServiceLifetime.Scoped:
                    rb.InstancePerLifetimeScope();
                    break;
                case ServiceLifetime.Transient:
                    rb.InstancePerDependency();
                    break;
            }
        }
    }
}