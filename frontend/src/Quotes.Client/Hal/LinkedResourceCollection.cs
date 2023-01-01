using System.Text.Json.Serialization;

namespace Quotes.Client.Hal;

public abstract class LinkedResourceCollection<T> : LinkedResource where T : LinkedResource
{
    [JsonPropertyName("_items")]
    public List<T> Items { get; init; } = new List<T>();
}
