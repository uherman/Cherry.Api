namespace Proxy;

public record RedisOptions(string ConnectionString, string DataProtectionKey, int DefaultDatabase)
{
    public const string Section = "RedisOptions";
}