namespace Proxy;

public record Auth0Options(string Domain, string ClientId)
{
    public const string Section = "Auth0";
}