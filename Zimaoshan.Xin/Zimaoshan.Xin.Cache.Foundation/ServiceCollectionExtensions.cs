using Microsoft.Extensions.DependencyInjection;

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
}
