using StackExchange.Redis;

namespace Proxy;

/// <summary>
/// Represents the options for configuring Redis.
/// </summary>
/// <param name="ConnectionString">The connection string to connect to the Redis server.</param>
/// <param name="DataProtectionKey">The key used for data protection.</param>
/// <param name="DefaultDatabase">The default database index to use.</param>
public record RedisOptions(string ConnectionString, string DataProtectionKey, int DefaultDatabase)
{
    /// <summary>
    /// The configuration section name for Redis options.
    /// </summary>
    public const string Section = "RedisOptions";

    /// <summary>
    /// Gets a connection multiplexer for Redis using the specified configuration.
    /// </summary>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>
    /// A tuple containing an instance of <see cref="IConnectionMultiplexer" /> connected to the Redis server and the
    /// data protection key.
    /// </returns>
    public static (IConnectionMultiplexer multiplexer, string dataProtectionKey) GetConnectionMultiplexer(
        IConfiguration configuration)
    {
        var redisOptions = configuration.GetSection(Section).Get<RedisOptions>();
        return (ConnectionMultiplexer.Connect(new ConfigurationOptions
        {
            EndPoints = { redisOptions.ConnectionString },
            DefaultDatabase = redisOptions.DefaultDatabase
        }), redisOptions.DataProtectionKey);
    }
}