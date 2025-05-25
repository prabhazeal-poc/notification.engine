using System.Text.Json;

namespace notification.engine.ServiceDefaults.Extensions
{
    /// <summary>
    /// Provides extension methods for string serialization.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Serializes an object to a JSON string.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static string ToJson<T>(this T obj)
        {
            return JsonSerializer.Serialize(obj);
        }
    }
}
