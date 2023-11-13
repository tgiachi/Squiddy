using System.Text.Json;
using System.Text.Json.Serialization;
using Squiddy.Core.Converters;
using Squiddy.Core.Serializers.Json.NamingPolicies;

namespace Squiddy.Core.Utils.Serializers.Json;

/// <summary>
/// Provides utility functions for JSON serialization and deserialization using System.Text.Json.
/// </summary>
/// <remarks>
/// This class encapsulates common JsonSerializer settings in one place for reuse across the application.
/// </remarks>
public static class JsonSerializerUtility
{
    /// <summary>
    /// Gets the default JsonSerializerOptions to be used across the application.
    /// </summary>
    /// <remarks>
    /// DefaultOptions are configured to use snake_case for property names,
    /// be case-insensitive when matching incoming JSON property names,
    /// convert enums to strings, and to write indented JSON.
    /// </remarks>
    public static JsonSerializerOptions DefaultOptions => new()
    {
        PropertyNamingPolicy = JsonSnakeCaseNamingPolicy.Instance,
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new JsonStringEnumConverter(),
            new MillisDateTimeConverter()
        },
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
}
