using System.Net;
using System.Security.Claims;
using Proxy.Models;
using Proxy.Services;

namespace Proxy.Extensions;

/// <summary>
/// Provides extension methods for <see cref="ClaimsPrincipal" /> to check user roles.
/// </summary>
public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Tries to get the <see cref="UserId" /> from the <see cref="ClaimsPrincipal" />.
    /// </summary>
    /// <param name="principal">The <see cref="ClaimsPrincipal" /> to extract the user ID from.</param>
    /// <param name="id">
    /// When this method returns, contains the userId if found; otherwise, the default value of <see cref="UserId" />.
    /// </param>
    /// <returns><c>true</c> if the userId was found; otherwise, <c>false</c>.</returns>
    public static bool TryGetId(this ClaimsPrincipal principal, out UserId id)
    {
        var idString = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (idString is null)
        {
            id = default;
            return false;
        }

        id = idString;
        return true;
    }

    /// <summary>
    /// Checks if the <see cref="ClaimsPrincipal" /> is in the specified role.
    /// </summary>
    /// <param name="principal">The <see cref="ClaimsPrincipal" /> to check.</param>
    /// <param name="role">The role to check against.</param>
    /// <param name="userService">The <see cref="UserService" /> to use for role checking.</param>
    /// <returns>A <see cref="RoleResult" /> indicating whether the user is in the role and the corresponding status code.</returns>
    public static async Task<RoleResult> IsInRole(this ClaimsPrincipal principal, Role role, UserService userService)
    {
        if (principal.Identity?.IsAuthenticated is not true || !principal.TryGetId(out var id))
        {
            return new RoleResult(false, HttpStatusCode.Unauthorized);
        }

        var isInRole = await userService.IsInRole(id, role);

        return isInRole
            ? new RoleResult(true, HttpStatusCode.OK, id)
            : new RoleResult(false, HttpStatusCode.Forbidden, id);
    }

    /// <summary>
    /// Checks if the user in the <see cref="HttpContext" /> is in the specified role.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext" /> containing the user.</param>
    /// <param name="role">The role to check against.</param>
    /// <returns>A <see cref="RoleResult" /> indicating whether the user is in the role and the corresponding status code.</returns>
    public static async Task<RoleResult> UserIsInRole(this HttpContext context, Role role)
    {
        return await context.User.IsInRole(role, context.RequestServices.GetRequiredService<UserService>());
    }
}

/// <summary>
/// Represents the result of a role check.
/// </summary>
/// <param name="isInRole">Indicates whether the user is in the role.</param>
/// <param name="statusCode">The HTTP status code corresponding to the role check result.</param>
/// <param name="userId">The user ID, if available.</param>
public readonly struct RoleResult(bool isInRole, HttpStatusCode statusCode, UserId? userId = null)
{
    /// <summary>
    /// Gets a value indicating whether the user is in the role.
    /// </summary>
    public bool IsInRole { get; } = isInRole;

    /// <summary>
    /// Gets the HTTP status code corresponding to the role check result.
    /// </summary>
    public int StatusCode { get; } = (int)statusCode;

    /// <summary>
    /// Returns a string representation of the role check result.
    /// </summary>
    /// <returns>A string indicating whether the user is authorized or unauthorized.</returns>
    public override string ToString()
    {
        if (userId is null)
        {
            return IsInRole ? "Authorized user" : "Unauthorized user";
        }

        return IsInRole ? $"Authorized user '{userId}'" : $"Unauthorized user '{userId}'";
    }
}