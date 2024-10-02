using System.Text.Json.Serialization;

namespace Proxy.Models;

/// <summary>
/// Represents a user role.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Role
{
    None,
    User,
    Admin
}