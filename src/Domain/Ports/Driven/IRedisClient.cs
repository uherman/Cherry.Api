namespace Domain.Ports.Driven;

/// <summary>
/// Interface for Redis client operations.
/// </summary>
public interface IRedisClient
{
    /// <summary>
    /// Retrieves an object of type T from Redis by key.
    /// </summary>
    /// <typeparam name="T">The type of the object to retrieve.</typeparam>
    /// <param name="key">The key of the object to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the object of type T.</returns>
    Task<T> Get<T>(string key) where T : class;

    /// <summary>
    /// Stores an object of type T in Redis with the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the object to store.</typeparam>
    /// <param name="key">The key of the object to store.</param>
    /// <param name="value">The object to store.</param>
    /// <param name="expiry">The expiration time for the object.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task Set<T>(string key, T value, TimeSpan? expiry = null) where T : class;

    /// <summary>
    /// Retrieves a string from Redis by key.
    /// </summary>
    /// <param name="key">The key of the string to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the string.</returns>
    Task<string> GetString(string key);

    /// <summary>
    /// Stores a string in Redis with the specified key.
    /// </summary>
    /// <param name="key">The key of the string to store.</param>
    /// <param name="value">The string to store.</param>
    /// <param name="expiry">The expiration time for the string.</param>

    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SetString(string key, string value, TimeSpan? expiry = null);

    /// <summary>
    /// Deletes an object from Redis by key.
    /// </summary>
    /// <param name="key">The key of the object to delete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a boolean indicating whether the
    /// deletion was successful.
    /// </returns>
    Task<bool> Delete(string key);
}