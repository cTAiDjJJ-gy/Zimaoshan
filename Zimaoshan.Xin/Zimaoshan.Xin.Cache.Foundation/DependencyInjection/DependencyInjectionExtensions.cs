using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Zimaoshan.Xin.Cache.Foundation.DependencyInjection
{
    /// <summary>
    /// .Net Core 依赖注入扩展
    /// https://mp.weixin.qq.com/s/47jvM9lRtxk_JkHEQAh09w
    /// </summary>
    public static class DependencyInjectionExtensions
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
        private static void RegisterDependenciesByAssembly(this IServiceCollection services, Assembly assembly)
        {
            var types = assembly.GetDependenciesTypes();

            foreach (var type in types)
            {
                var registerType = type.FindDependencyInterface();
                if (registerType == null) continue;

                var serviceLifetime = type.FindServiceDependencyLifetime();
                services.Add(new ServiceDescriptor(registerType, type, serviceLifetime));
            }
        }

        /// <summary>
        /// 获取程序集依赖注入的类型列表
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private static List<Type> GetDependenciesTypes(this Assembly assembly)
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
        /// <returns></returns>
        private static Type? FindDependencyInterface(this Type type)
        {
            var t = type
                .GetInterfaces();

            var interfaces = t
                .Where(x => 
                    !x.IsAssignableFrom(typeof(IDependency)) &&
                    !x.IsAssignableFrom(typeof(ISingletonDependency)) &&
                    !x.IsAssignableFrom(typeof(IScopedDependency)) &&
                    !x.IsAssignableFrom(typeof(ITransientDependency)))
                .ToList();

            if (!interfaces.Any()) { return null; }

            var registerType = interfaces.FirstOrDefault(t => t.Name.ToUpper().Contains(type.Name.ToUpper()));

            return registerType;
        }

        /// <summary>
        /// 根据依赖注入注册类型获取服务对应生命周期
        /// </summary>
        /// <param name="type">依赖注入注册类型</param>
        /// <returns>服务对应生命周期</returns>
        /// <exception cref="ArgumentOutOfRangeException">注册服务找不到对应的周期类型</exception>
        private static ServiceLifetime FindServiceDependencyLifetime(this Type type)
        {
            var interfaces = type.GetInterfaces();

            if (interfaces.Any(x => x == typeof(ISingletonDependency)))
            {
                return ServiceLifetime.Singleton;
            }
            else if (interfaces.Any(x => x == typeof(IScopedDependency)))
            {
                return ServiceLifetime.Scoped;
            }
            else if (interfaces.Any(x => x == typeof(ITransientDependency)))
            {
                return ServiceLifetime.Transient;
            }
            else
            {
                throw new ArgumentOutOfRangeException($"Provided ServiceLifetime type is invalid. Lifetime:{type.Name}");
            }
        }
    }
}
