using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Template.MinimalApi.NET8.Extensions;

public static class SerializationExtension
{
    public static string ToJson<T>(this T obj, bool prettify = true) where T : class
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true, // For case-insensitive property names
            WriteIndented = prettify, // For pretty-printed JSON
            // DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, // Ignore null values when writing JSON
            // Add other options as needed,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            },
            AllowTrailingCommas = true
        };

        return JsonSerializer.Serialize(obj, options);
    }
    
    public static T? FromJson<T>(this string json) where T : class
    {
        return JsonSerializer.Deserialize<T>(json);
    }
    
    public static T? FromJsonV2<T>(this string json) where T : class
    {
        return JsonConvert.DeserializeObject<T>(json);
    }
}