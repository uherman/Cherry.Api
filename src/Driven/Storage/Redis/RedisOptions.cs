namespace Driven.Storage.Redis;

public record RedisOptions
{
    public const string Section = "RedisOptions";
    public string ConnectionString { get; init; }
}