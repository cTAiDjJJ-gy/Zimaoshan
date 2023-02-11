using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Zimaoshan.Xin.Cache.Foundation.Impl;

namespace Zimaoshan.Xin.Cache.Foundation;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLocalCache(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<ICache, DefaultLocalCache>();

        return services;
    }

    public static IServiceCollection AddRedisCache(this IServiceCollection services)
    {
        services.AddSingleton<ICache, DefaultRedisCache>();
        return services;
    }

    public static IServiceCollection AddHybridCache(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<ILocalCache, DefaultLocalCache>();
        services.AddSingleton<ICacheBus, DefaultRedisBus>();
        services.AddSingleton<IRedisDatabaseProvider, RedisDatabaseProvider>();
        services.AddSingleton<IDistributedCache, DefaultRedisCache>();

        services.Replace(ServiceDescriptor.Singleton(typeof(ICache), typeof(DefaultRedisHybridCache)));

        return services;
    }
}
