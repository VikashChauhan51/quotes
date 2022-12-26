using Newtonsoft.Json;
 

namespace Quotes.API.Hal;

public abstract class LinkedResourceCollection<T> : LinkedResource where T : LinkedResource
{
 
    [JsonProperty(PropertyName = "_items")]
    public IEnumerable<T> Items { get; init; } = new List<T>();
}
