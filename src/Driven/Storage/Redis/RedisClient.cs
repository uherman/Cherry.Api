using System.Text.Json;
using System.Text.Json.Serialization;
using Domain.Ports.Driven;
using StackExchange.Redis;

namespace Driven.Storage.Redis;

public class RedisClient(IConnectionMultiplexer connectionMultiplexer) : IRedisClient
{
    private readonly IDatabase _database = connectionMultiplexer.GetDatabase();

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    // <inheritdoc />
    public async Task<T> Get<T>(string key) where T : class, new()
    {
        var response = await _database.StringGetAsync(key);
        return response.HasValue ? JsonSerializer.Deserialize<T>(response, JsonSerializerOptions) : default;
    }

    // <inheritdoc />
    public async Task Set<T>(string key, T value) where T : class, new()
    {
        await _database.StringSetAsync(key, JsonSerializer.Serialize(value, JsonSerializerOptions));
    }

    // <inheritdoc />
    public async Task<string> GetString(string key)
    {
        return await _database.StringGetAsync(key);
    }

    // <inheritdoc />
    public async Task SetString(string key, string value)
    {
        await _database.StringSetAsync(key, value);
    }

    // <inheritdoc />
    public Task<bool> Delete(string key)
    {
        return _database.KeyDeleteAsync(key);
    }
}