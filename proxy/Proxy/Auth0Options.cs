using Auth0.AspNetCore.Authentication;

namespace Proxy;

/// <summary>
/// Represents the options for configuring Auth0.
/// </summary>
/// <param name="Domain">The Auth0 domain.</param>
/// <param name="ClientId">The Auth0 client ID.</param>
/// <param name="ClientSecret">The Auth0 client secret.</param>
/// <param name="Audience">The Auth0 audience.</param>
public record Auth0Options(string Domain, string ClientId, string ClientSecret, string Audience)
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
            options.ClientSecret = auth0Options.ClientSecret;
        };
    }

    /// <summary>
    /// Configures the Auth0 web application options with access token settings using the specified configuration.
    /// </summary>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>An action that configures <see cref="Auth0WebAppWithAccessTokenOptions" />.</returns>
    public static Action<Auth0WebAppWithAccessTokenOptions> ConfigureAccessToken(IConfiguration configuration)
    {
        var auth0Options = configuration.GetSection(Section).Get<Auth0Options>();
        return options =>
        {
            options.Audience = auth0Options.Audience;
            options.UseRefreshTokens = true;
        };
    }
}