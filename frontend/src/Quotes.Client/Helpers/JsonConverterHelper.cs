using System.Text.Json;

namespace Quotes.Client.Helpers;

public static class JsonConverterHelper
{
    public async static ValueTask<T?> DeserializeAsync<T>(Stream utf8Json)
    {
        var settings = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        settings.Converters.Add(new StringToHttpVerbsEnumConverter());
        return await JsonSerializer.DeserializeAsync<T>(utf8Json, settings);
    }

    public static T? Deserialize<T>(string json)
    {
        var settings = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        settings.Converters.Add(new StringToHttpVerbsEnumConverter());
        return JsonSerializer.Deserialize<T>(json, settings);
    }
    public static string Serialize<T>(T value)
    {
        var settings = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true

        };
        settings.Converters.Add(new StringToHttpVerbsEnumConverter());
        return JsonSerializer.Serialize(value, settings);
    }
    
}
