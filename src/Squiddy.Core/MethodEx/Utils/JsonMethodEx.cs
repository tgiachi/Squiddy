using System.Text.Json;
using System.Text.Json.Nodes;
using Squiddy.Core.Utils.Serializers.Json;

namespace Squiddy.Core.MethodEx.Utils;

/// <summary>
/// Extension class for Serialize/Deserialize JSON.
/// </summary>
public static class JsonMethodEx
{
    private const string JSON_TYPE_KEY = "$type";

    private static readonly JsonSerializerOptions JsonSerializerSettings = JsonSerializerUtility.DefaultOptions;

    /// <summary>
    ///  Serialize object to string.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToJson(this object value) => JsonSerializer.Serialize(value, JsonSerializerSettings);

    /// <summary>
    /// Parse string to Type.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static object? FromJson(this string obj, Type type)
    {
        try
        {
            return JsonSerializer.Deserialize(obj, type, JsonSerializerSettings);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Can't convert {obj} to object {type.Name} => {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Parse string to Generic.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T? FromJson<T>(this string obj)
    {
        try
        {
            return JsonSerializer.Deserialize<T>(obj, JsonSerializerSettings);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Can't convert {obj} to object {typeof(T).Name} => {ex.Message}");
            throw;
        }
    }

    public static async Task<Dictionary<string, List<string>>> DeserializeTypesFromFile(this string path)
    {
        var results = new Dictionary<string, List<string>>();
        var node = JsonSerializer.Deserialize<JsonNode>(await File.ReadAllTextAsync(path), JsonSerializerSettings);
        foreach (var item in node.AsArray())
        {
            if (item != null)
            {
                var itemType = item[JSON_TYPE_KEY].ToString();
                if (!results.ContainsKey(itemType))
                {
                    results.Add(itemType, new List<string>());
                }

                results[itemType].Add(item.ToString());
            }
        }

        return results;
    }
}
