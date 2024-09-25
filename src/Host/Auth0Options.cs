using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Host;

/// <summary>
/// Represents the options for configuring Auth0.
/// </summary>
/// <param name="Authority">The Auth0 authority.</param>
/// <param name="Audience">The Auth0 audience.</param>
public record Auth0Options(string Authority, string Audience)
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
    public static Action<JwtBearerOptions> Configure(IConfiguration configuration)
    {
        var auth0Options = configuration.GetSection(Section).Get<Auth0Options>();
        return options =>
        {
            options.Authority = auth0Options.Authority;
            options.Audience = auth0Options.Audience;
        };
    }
}