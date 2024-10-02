using SurrealDb.Net.Models;

namespace Proxy.Models;

/// <summary>
/// Represents a user identifier.
/// </summary>
public readonly record struct UserId
{
    /// <summary>
    /// The prefix used for user identifiers.
    /// </summary>
    private const string Prefix = "user";

    /// <summary>
    /// The underlying value of the user identifier.
    /// </summary>
    private readonly Thing _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserId" /> struct.
    /// </summary>
    /// <param name="value">The string value of the user identifier.</param>
    /// <exception cref="ArgumentException">Thrown when the user id is empty.</exception>
    private UserId(string value)
    {
        if (value is not { Length: > 0 })
        {
            throw new ArgumentException("User id cannot be empty", nameof(value));
        }

        if (value.Contains('|'))
        {
            value = value.Split('|')[1];
        }

        _value = new Thing($"{Prefix}:{value}");
    }

    /// <summary>
    /// Implicitly converts a <see cref="UserId" /> to a <see cref="Thing" />.
    /// </summary>
    /// <param name="id">The user identifier.</param>
    public static implicit operator Thing(UserId id) => id._value;

    /// <summary>
    /// Implicitly converts a <see cref="UserId" /> to a <see cref="string" />.
    /// </summary>
    /// <param name="id">The user identifier.</param>
    public static implicit operator string(UserId id) => id._value.Id;

    /// <summary>
    /// Implicitly converts a <see cref="string" /> to a <see cref="UserId" />.
    /// </summary>
    /// <param name="id">The string value of the user identifier.</param>
    public static implicit operator UserId(string id) => new(id);

    /// <summary>
    /// Implicitly converts a <see cref="Thing" /> to a <see cref="UserId" />.
    /// </summary>
    /// <param name="id">The <see cref="Thing" /> value of the user identifier.</param>
    public static implicit operator UserId(Thing id) => new(id.Id);

    /// <summary>
    /// Returns the string representation of the user identifier.
    /// </summary>
    /// <example>
    /// user:123
    /// </example>
    public override string ToString()
    {
        return _value.ToString();
    }
}