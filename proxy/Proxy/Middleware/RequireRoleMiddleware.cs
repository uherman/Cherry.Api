using Microsoft.AspNetCore.Authentication;
using Proxy.Extensions;
using Proxy.Models;

namespace Proxy.Middleware;

/// <summary>
/// Options for route protection.
/// </summary>
public class RouteProtectionOptions
{
    /// <summary>
    /// Indicates whether to include the access token in the request headers.
    /// </summary>
    public bool IncludeToken { get; init; }
}

/// <summary>
/// Middleware for requiring a specific role to access a route.
/// </summary>
public static class RequireRoleMiddleware
{
    /// <summary>
    /// Adds middleware to protect a specific route by requiring a user to have a specific role.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <param name="route">The route to protect.</param>
    /// <param name="role">The required role.</param>
    /// <param name="options">Optional route protection options.</param>
    /// <returns>The application builder with the middleware added.</returns>
    public static IApplicationBuilder UseProtectedRoute(this WebApplication app, string route, Role role,
        RouteProtectionOptions options = null)
    {
        options ??= new RouteProtectionOptions();
        ArgumentNullException.ThrowIfNull(route);
        ArgumentNullException.ThrowIfNull(role);
        var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger($"RouteProtection({route}:{role})");


        return app.Use(async (context, next) =>
        {
            if (!context.Request.Path.StartsWithSegments(route))
            {
                await next();
                return;
            }

            var result = await context.UserIsInRole(role);
            if (!result.IsInRole)
            {
                logger.LogInformation("{Message}", result);
                context.Response.StatusCode = result.StatusCode;
                return;
            }

            if (options.IncludeToken)
            {
                var accessToken = await context.GetTokenAsync("access_token");
                if (accessToken is not null)
                {
                    context.Request.Headers.Authorization = $"Bearer {accessToken}";
                }
            }

            logger.LogInformation("{Message}", result);

            await next();
        });
    }

    /// <summary>
    /// Adds an endpoint filter to require a specific role for accessing the endpoint.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the endpoint convention builder.</typeparam>
    /// <param name="builder">The endpoint convention builder.</param>
    /// <param name="role">The required role.</param>
    /// <returns>The endpoint convention builder with the filter added.</returns>
    public static TBuilder RequireRole<TBuilder>(this TBuilder builder, Role role)
        where TBuilder : IEndpointConventionBuilder
    {
        return builder.AddEndpointFilter(async (context, next) =>
        {
            var route = context.HttpContext.Request.Path.Value;
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
                .CreateLogger($"RouteProtection({route}:{role})");

            var result = await context.HttpContext.UserIsInRole(role);
            if (!result.IsInRole)
            {
                logger.LogInformation("{Message}", result);
                return Results.StatusCode(result.StatusCode);
            }

            logger.LogInformation("{Message}", result);

            return await next(context);
        });
    }
}