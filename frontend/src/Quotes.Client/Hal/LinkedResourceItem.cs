using System.Text.Json.Serialization;

namespace Quotes.Client.Hal;

public class LinkedResourceItem<T> : LinkedResource
{

    [JsonPropertyName("_data")]
    public T Data { get; init; } = default!;
}

