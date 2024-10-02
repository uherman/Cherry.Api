using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Proxy.Models;
using Proxy.Services;

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
    public static IApplicationBuilder UseProtectedRoute(this IApplicationBuilder app, string route, Role role,
        RouteProtectionOptions options = null)
    {
        options ??= new RouteProtectionOptions();
        ArgumentNullException.ThrowIfNull(route);
        ArgumentNullException.ThrowIfNull(role);

        return app.Use(async (context, next) =>
        {
            if (!context.Request.Path.StartsWithSegments(route))
            {
                await next();
                return;
            }

            var userService = context.RequestServices.GetRequiredService<UserService>();
            if (context.User.Identity?.IsAuthenticated is not true)
            {
                context.Response.StatusCode = 401;
                return;
            }

            var id = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id is null)
            {
                context.Response.StatusCode = 401;
                return;
            }

            var isInRole = await userService.IsInRole(id, role);
            if (!isInRole)
            {
                context.Response.StatusCode = 403;
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
            var userService = context.HttpContext.RequestServices.GetRequiredService<UserService>();
            if (context.HttpContext.User.Identity?.IsAuthenticated is not true)
            {
                return Results.Unauthorized();
            }

            var id = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id is null)
            {
                return Results.Unauthorized();
            }

            var isInRole = await userService.IsInRole(id, role);
            if (!isInRole)
            {
                return Results.Forbid();
            }

            return await next(context);
        });
    }
}