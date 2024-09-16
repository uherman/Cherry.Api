using Domain.Ports.Driven;
using Driven.Storage.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Driven;

public static class ConfigureServices
{
    public static void AddDriven(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRedis(configuration);
    }

    private static void AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var redisOptions = configuration.GetSection(RedisOptions.Section).Get<RedisOptions>();
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisOptions.ConnectionString));
        services.AddSingleton<IRedisClient, RedisClient>();
    }
}