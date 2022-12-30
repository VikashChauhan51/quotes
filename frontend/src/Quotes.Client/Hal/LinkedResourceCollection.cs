using Newtonsoft.Json;

namespace Quotes.Client.Hal;

public abstract class LinkedResourceCollection<T> : LinkedResource where T : LinkedResource
{

    [JsonProperty(PropertyName = "_items")]
    public IEnumerable<T> Items { get; init; } = new List<T>();
}
