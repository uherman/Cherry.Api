namespace Proxy.Models;

/// <summary>
/// Represents a user.
/// </summary>
public class User(UserId id, IReadOnlyCollection<Role> roles)
{
    private readonly HashSet<Role> _roles = roles?.ToHashSet() ?? [];

    /// <summary>
    /// Represents the user's unique identifier.
    /// </summary>
    public UserId Id { get; } = id;

    /// <summary>
    /// Represents the user's roles.
    /// </summary>
    public IReadOnlyCollection<Role> Roles => _roles;

    /// <summary>
    /// Adds a role to the user's collection of roles.
    /// </summary>
    /// <param name="role">The role to add.</param>
    /// <exception cref="ArgumentException">Thrown when attempting to add the 'None' role.</exception>
    public void AddRole(Role role)
    {
        if (role is Role.None)
        {
            throw new ArgumentException("Cannot add role 'None'", nameof(role));
        }

        _roles.Add(role);
    }

    /// <summary>
    /// Determines whether the user has a specific role.
    /// </summary>
    /// <param name="role">The role to check.</param>
    /// <returns><c>true</c> if the user has the specified role; otherwise, <c>false</c>.</returns>
    public bool IsInRole(Role role)
    {
        return _roles.Contains(role);
    }
}