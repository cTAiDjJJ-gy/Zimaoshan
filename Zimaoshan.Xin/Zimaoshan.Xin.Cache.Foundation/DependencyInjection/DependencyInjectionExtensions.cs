using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Zimaoshan.Xin.Cache.Foundation.Annotations;

namespace Zimaoshan.Xin.Cache.Foundation.DependencyInjection
{
    /// <summary>
    /// .Net Core 依赖注入扩展
    /// https://mp.weixin.qq.com/s/47jvM9lRtxk_JkHEQAh09w
    /// </summary>
    internal static class DependencyInjectionExtensions
    {
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection ScanAndRegisterServices(this IServiceCollection services, params Assembly[] assemblies)
        {
            foreach(var assembly in assemblies)
            {
                services.RegisterDependenciesByAssembly(assembly);
            }

            return services;
        }

        /// <summary>
        /// 注册依赖注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        public static void RegisterDependenciesByAssembly(this IServiceCollection services, Assembly assembly)
        {
            var types = assembly.GetDependenciesTypes();

            foreach (var type in types)
            {
                var registerInterfaces = type.FindDependencyInterfaces();
                if (registerInterfaces == null) continue;

                var serviceLifetime = type.FindServiceDependencyLifetime();
                foreach(var registerInterface in registerInterfaces)
                {
                    services.Add(new ServiceDescriptor(registerInterface.Interface, type, serviceLifetime));
                }
            }
        }

        /// <summary>
        /// 获取程序集依赖注入的类型列表
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static List<Type> GetDependenciesTypes(this Assembly assembly)
        {
            return assembly
                .GetTypes()
                .Where(t =>
                    typeof(IDependency).GetTypeInfo().IsAssignableFrom(t)
                    && t.GetTypeInfo().IsClass
                    && !t.GetTypeInfo().IsAbstract
                    && !t.GetTypeInfo().IsSealed)
                .ToList();
        }

        /// <summary>
        /// 获取类型依赖注入接口
        /// </summary>
        /// <param name="type"></param>
        /// <returns>业务接口和接口缓存</returns>
        public static IEnumerable<(Type Interface, bool WithCache)> FindDependencyInterfaces(this Type type)
        {
            return type
                .GetInterfaces()
                .Where(x =>
                    !x.IsAssignableFrom(typeof(IDependency)) &&
                    !x.IsAssignableFrom(typeof(ISingletonDependency)) &&
                    !x.IsAssignableFrom(typeof(IScopedDependency)) &&
                    !x.IsAssignableFrom(typeof(ITransientDependency)))
                .Select(t =>
                {
                    if (t.GetCustomAttribute<WithCacheAttribute>() != null)
                    {
                        return (t, true);
                    }
                    else
                    {
                        return (t, false);
                    }
                });
        }

        /// <summary>
        /// 根据依赖注入注册类型获取服务对应生命周期
        /// </summary>
        /// <param name="type">依赖注入注册类型</param>
        /// <returns>服务对应生命周期</returns>
        /// <exception cref="ArgumentOutOfRangeException">注册服务找不到对应的周期类型</exception>
        public static ServiceLifetime FindServiceDependencyLifetime(this Type type)
        {
            var lifeScope = type.FindServiceDependencyLifetimeOrNull();

            return lifeScope.HasValue ? lifeScope.Value : throw new ArgumentOutOfRangeException($"Provided ServiceLifetime type is invalid. Lifetime:{type.Name}");
        }

        /// <summary>
        /// 获取服务生命周期，可空
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ServiceLifetime? FindServiceDependencyLifetimeOrNull(this Type type)
        {
            var interfaces = type.GetInterfaces();

            if (interfaces.Any(x => x == typeof(ISingletonDependency)))
            {
                return ServiceLifetime.Singleton;
            }

            if (interfaces.Any(x => x == typeof(IScopedDependency)))
            {
                return ServiceLifetime.Scoped;
            }

            if (interfaces.Any(x => x == typeof(ITransientDependency)))
            {
                return ServiceLifetime.Transient;
            }

            return null;
        }
    }
}
