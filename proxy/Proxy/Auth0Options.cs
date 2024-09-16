using Auth0.AspNetCore.Authentication;

namespace Proxy;

/// <summary>
/// Represents the options for configuring Auth0.
/// </summary>
/// <param name="Domain">The Auth0 domain.</param>
/// <param name="ClientId">The Auth0 client ID.</param>
public record Auth0Options(string Domain, string ClientId)
{
    /// <summary>
    /// The configuration section name for Auth0 options.
    /// </summary>
    public const string Section = "Auth0";

    /// <summary>
    /// Configures the Auth0 web application options using the specified configuration.
    /// </summary>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>An action that configures <see cref="Auth0WebAppOptions" />.</returns>
    public static Action<Auth0WebAppOptions> Configure(IConfiguration configuration)
    {
        var auth0Options = configuration.GetSection(Section).Get<Auth0Options>();
        return options =>
        {
            options.Domain = auth0Options.Domain;
            options.ClientId = auth0Options.ClientId;
        };
    }
}