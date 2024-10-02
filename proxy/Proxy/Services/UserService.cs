using Proxy.Models;
using SurrealDb.Net;
using SurrealDb.Net.Models;

namespace Proxy.Services;

/// <summary>
/// Service for managing users and their roles.
/// </summary>
/// <param name="client">The SurrealDB client used for database operations.</param>
public class UserService(ISurrealDbClient client)
{
    /// <summary>
    /// Checks if a user is in a specific role.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="role">The role to check.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a boolean indicating whether the
    /// user is in the specified role.
    /// </returns>
    public async Task<bool> IsInRole(UserId id, Role role)
    {
        var user = await GetUser(id);
        return user?.IsInRole(role) is true;
    }

    /// <summary>
    /// Saves a user to the database.
    /// </summary>
    /// <param name="user">The user to save.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SaveUser(User user)
    {
        var record = new UserRecord
        {
            Id = user.Id,
            Roles = user.Roles
        };

        await client.Create(record);
    }

    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the user if found; otherwise,
    /// null.
    /// </returns>
    private async Task<User> GetUser(UserId id)
    {
        var record = await client.Select<UserRecord>((Thing)id);

        return record is null ? null : new User(record.Id, record.Roles);
    }
}

/// <summary>
/// Represents a record of a user in the database.
/// </summary>
public class UserRecord : Record
{
    /// <summary>
    /// Represents the roles of the user.
    /// </summary>
    public IReadOnlyCollection<Role> Roles { get; init; }
}