using Newtonsoft.Json;

namespace Quotes.Client.Hal;

public class LinkedResourceItem<T> : LinkedResource
{
    [JsonProperty(PropertyName = "_data")]
    public T Data { get; init; } = default!;
}

