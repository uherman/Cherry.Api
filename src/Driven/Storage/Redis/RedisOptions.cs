namespace Driven.Storage.Redis;

public record RedisOptions(string ConnectionString)
{
    public const string Section = "RedisOptions";
}