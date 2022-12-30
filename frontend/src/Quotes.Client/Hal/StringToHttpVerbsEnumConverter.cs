

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Quotes.Client.Hal
{
    public class StringToHttpVerbsEnumConverter : JsonConverter<HttpVerbs>
    {
        public override HttpVerbs Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return (HttpVerbs)Enum.Parse(typeof(HttpVerbs), reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, HttpVerbs value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
